using NHibernate.Criterion;
using Remember.Core;
using Remember.Domain;
using Remember.Service;
using Remember.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.Controllers
{
    public class AccountController : Controller
    {
        private const string _sessionLoginAccount = "LoginAccount";
        private const string _cookieToken = "Token";
        private const int _rememberMeDayCount = 7;


        #region 登录
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            #region 检查 登录状态
            if (Session[_sessionLoginAccount] != null)
            {
                return LoginSuccessRedirectResult(returnUrl);
            }
            #region 记住我
            if (Request.Cookies.AllKeys.Contains(_cookieToken))
            {
                if (Request.Cookies[_cookieToken] != null && string.IsNullOrEmpty(Request.Cookies[_cookieToken].Value) == false)
                {
                    string cookieTokenValue = Request.Cookies[_cookieToken].Value;
                    SysUser user = Container.Instance.Resolve<SysUserService>().Query(new List<ICriterion>
                    {
                        Expression.Eq("Token", cookieTokenValue)
                    }).FirstOrDefault();

                    if (user == null)
                    {
                        // 口令不正确
                        Response.Cookies[_cookieToken].Expires = DateTime.UtcNow.AddDays(-1);
                    }
                    else if (user.LastLoginTime.AddDays(_rememberMeDayCount) > DateTime.UtcNow)
                    {
                        // 最多 "记住我" 保存7天的 登录状态
                        Session[_sessionLoginAccount] = user;
                        return LoginSuccessRedirectResult(returnUrl);
                    }
                    else
                    {
                        // 登录 已过期
                        ModelState.AddModelError("", "登录已过期，请重新登录");
                    }
                }
            }
            #endregion

            #endregion

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var dbUser = Container.Instance.Resolve<SysUserService>().Query(new List<ICriterion>
                {
                    Expression.Eq("LoginAccount", model.LoginAccount)
                }).FirstOrDefault();
                if (dbUser == null)
                {
                    ModelState.AddModelError("", "账号不存在");
                    return View();
                }
                if (dbUser.Password != Common.StringHelper.EncodeMD5(model.Password))
                {
                    ModelState.AddModelError("", "账号或密码错误");
                    return View();
                }
                if (dbUser.Status == 1)
                {
                    ModelState.AddModelError("", "账号被禁用");
                    return View();
                }

                // 登录成功
                Session[_sessionLoginAccount] = dbUser;
                // 浏览器移除 Token
                if (Request.Cookies.AllKeys.Contains(_cookieToken))
                {
                    Response.Cookies[_cookieToken].Expires = DateTime.UtcNow.AddDays(-1);
                }

                #region 记住我
                if (model.IsRememberMe == true)
                {
                    string token = Guid.NewGuid().ToString();
                    // token 存入登录用户--数据库
                    dbUser.Token = token;
                    // token 存入 浏览器
                    HttpCookie cookieToken = new HttpCookie(_cookieToken, token)
                    {
                        Expires = DateTime.UtcNow.AddDays(_rememberMeDayCount),
                        HttpOnly = true
                    };
                    Response.Cookies.Add(cookieToken);
                }
                else
                {
                    // 数据库-当前用户移除 Token
                    dbUser.Token = null;
                }
                #endregion

                // 更新用户--最后登录时间等
                dbUser.LastLoginTime = DateTime.UtcNow;
                Container.Instance.Resolve<SysUserService>().Edit(dbUser);

                return LoginSuccessRedirectResult(returnUrl);
            }

            return View(model);
        }
        #endregion

        #region 登录成功跳转结果
        private ActionResult LoginSuccessRedirectResult(string returnUrl)
        {
            if (Request.IsAjaxRequest())
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return Json(new { code = 1, message = "登录成功", targetUrl = Url.Action("Index", "Home") });
                }
                else
                {
                    return Json(new { code = 1, message = "登录成功", targetUrl = returnUrl });
                }
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
        }
        #endregion
    }
}

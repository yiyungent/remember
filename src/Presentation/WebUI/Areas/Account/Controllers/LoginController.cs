using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using Domain;
using Framework.Common;
using Services;
using Framework.Config;
using WebUI.Areas.Account.Models;
using Framework.Infrastructure.Concrete;
using Core.Common;
using Framework.Models;
using Services.Interface;
using Domain.Entities;

namespace WebUI.Areas.Account.Controllers
{
    public class LoginController : Controller
    {
        #region Fields
        private static string _cookieKeyToken = AppConfig.JwtName;
        private static int _rememberMeDayCount = AppConfig.RememberMeDayCount;

        private readonly IUserInfoService _userInfoService;
        private readonly ISettingService _settingService;
        #endregion

        #region Properities
        public Dictionary<string, string> EmailDic { get; set; }
        #endregion

        #region Ctor
        public LoginController(IUserInfoService userInfoService,
            ISettingService settingService)
        {
            InitEmailDic();

            this._userInfoService = userInfoService;
            this._settingService = settingService;
        }
        #endregion

        #region 登录视图
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            #region 检查 登录状态
            CurrentAccountModel currentAccount = AccountManager.GetCurrentAccount();
            if (!currentAccount.IsGuest)
            {
                return LoginSuccessRedirectResult(returnUrl);
            }
            #region 记住我
            if (Request.Cookies.AllKeys.Contains(_cookieKeyToken))
            {
                if (Request.Cookies[_cookieKeyToken] != null && string.IsNullOrEmpty(Request.Cookies[_cookieKeyToken].Value) == false)
                {
                    string cookieTokenValue = Request.Cookies[_cookieKeyToken].Value;
                    //UserInfo user = Container.Instance.Resolve<UserInfoService>().Query(new List<ICriterion>
                    //{
                    //    Expression.Eq(_cookieKeyToken, cookieTokenValue)
                    //}).FirstOrDefault();
                    // TODO: 需配合 JWT 的更新而更新
                    UserInfo user = null;

                    if (user == null)
                    {
                        // 口令不正确
                        Response.Cookies[_cookieKeyToken].Expires = DateTime.Now.AddDays(-1);
                    }
                    else if (user.LastLoginTime.AddDays(_rememberMeDayCount) > DateTime.Now)
                    {
                        // 最多 "记住我" 保存7天的 登录状态
                        //Session[_sessionKeyLoginAccount] = user.ID;
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

        #endregion

        #region 登录验证
        [HttpPost]
        public ActionResult Index(string userName, string password, string returnUrl)
        {
            password = Framework.Common.EncryptHelper.MD5Encrypt32(password);

            // 1.滑动验证码 二次验证
            string message = string.Empty;
            bool isPass = Core.Common.VerifyCode.SecondVerifyCode(Request.Form["ticket"], Request.Form["randStr"], Request.UserHostAddress, out message);
            if (!isPass)
            {
                // 验证码错误
                return Json(new { code = -2, message });
            }

            // 验证通过
            //var dbUser = Container.Instance.Resolve<UserInfoService>().Query(new List<ICriterion>
            //    {
            //        Expression.Eq("UserName", userName)
            //    }).FirstOrDefault();
            var dbUser = this._userInfoService.Find(m =>
                m.UserName == userName
                && !m.IsDeleted
            );
            if (dbUser == null)
            {
                return Json(new { code = -1, message = "用户不存在,检查或前往注册" });
            }
            if (dbUser.Password != password)
            {
                return Json(new { code = -1, message = "用户名或密码错误" });
            }
            //if (dbUser.Status == Domain.Base.StatusEnum.Banned)
            //{
            //    return Json(new { code = -1, message = "账号被禁用" });
            //}

            // 登录成功
            // TODO: 注意，由于EF的懒加载，而 UserInfo.RoleInfos是Get{},而在其中的一个RoleInfo.FunctionInfos也是get,而get是必须调用的，他是方法，不代表具体的数据，所以 Session 中的用户信息的角色没有FunctionInfos，而导致没有权限
            // TODO: 这样将用户信息的全部都存于 Session 果然还是不太好，更新自己的用户信息，还必须手动更新 Session 才会使之有效，因为其他地方获取用户信息都是从Session内，决定改为 Session 内只保存用户的ID，每次获取用户信息都重新用保存在Session内的UserInfo.ID查询数据库
            //Session[_sessionKeyLoginAccount] = dbUser.ID;
            // 浏览器移除 Token
            if (Request.Cookies.AllKeys.Contains(_cookieKeyToken))
            {
                Response.Cookies[_cookieKeyToken].Expires = DateTime.Now.AddDays(-1);
            }

            // 无论是否 "记住我"，都下发口令
            JWTokenViewModel tokenModel = new JWTokenViewModel
            {
                Create = DateTimeHelper.NowTimeStamp10(),
                Expire = DateTimeHelper.ToTimeStamp10(DateTime.Now.AddDays(1)),
                ID = dbUser.ID,
                UserName = dbUser.UserName
            };
            string token = JwtHelper.Encode(tokenModel);
            // 是否“记住我” 的口令过期时间不同--过期时间在数据库，浏览器均不同
            #region 记住我
            HttpCookie cookieToken;
            if (Request["isRememberMe"] != null && bool.Parse(Request["isRememberMe"].ToString()))
            {
                // 过期时间延长
                tokenModel.Expire = DateTimeHelper.ToTimeStamp10(DateTime.Now.AddDays(_rememberMeDayCount));
                token = JwtHelper.Encode(tokenModel);
                // token 存入 浏览器
                cookieToken = new HttpCookie(_cookieKeyToken, token)
                {
                    Expires = DateTime.Now.AddDays(_rememberMeDayCount),
                    HttpOnly = true
                };
            }
            else
            {
                // token 存入 浏览器
                cookieToken = new HttpCookie(_cookieKeyToken, token)
                {
                    // 不设置 cookie 的过期时间，默认关闭浏览器过期
                    HttpOnly = true
                };
            }
            #endregion
            Response.Cookies.Add(cookieToken);

            // 更新用户--最后登录时间等
            dbUser.LastLoginTime = DateTime.Now;
            // TODO: 去除 RefreshToken
            //dbUser.RefreshToken = token;

            //Container.Instance.Resolve<UserInfoService>().Edit(dbUser);
            this._userInfoService.Update(dbUser);

            return Json(new { code = 1, message = "登录成功", returnUrl = returnUrl });
        }
        #endregion

        #region 登录成功跳转结果
        private ActionResult LoginSuccessRedirectResult(string returnUrl)
        {
            if (Request.IsAjaxRequest())
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return Json(new { code = 1, message = "登录成功", targetUrl = Url.Action("Index", "Home", new { area = "" }) });
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
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
        }
        #endregion

        #region 找回密码视图
        public ActionResult FindPassword()
        {
            return View();
        }
        #endregion

        #region 滑动二次验证
        /// <summary>
        /// 滑动二次验证(目前此方法仅用于找回密码时验证)
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="randStr"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyCode(string ticket, string randStr, string action)
        {
            string[] actions = action.Split('|');
            string ip = Request.UserHostAddress;
            string message = string.Empty;
            if (Core.Common.VerifyCode.SecondVerifyCode(ticket, randStr, ip, out message))
            {
                // 验证通过
                // 根据用户的邮箱类型返回不同的邮箱查看地址
                // ewffnrwf@126.com --->  126.com
                string emailType = actions[1].Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries)[1];
                string emailLoginAddress = EmailDic[emailType];
                // 邮件验证码
                //Container.Instance.Resolve<SettingService>().SendMailVerifyCodeForFindPwd(actions[1], out string vCode);
                this._settingService.SendMailVerifyCodeForFindPwd(actions[1], out string vCode);
                // 保存到Session["vCode"];
                Session["vCode"] = vCode;

                return Json(new { code = 1, message = $"验证码短信/邮件已发出，5分钟内有效，请注意<a target=\"_blank\" href=\"//{emailLoginAddress}\" style=\"font-size: 14px;\">查收</a>" });
            }
            else
            {
                // 验证不通过
                return Json(new { code = -1, message = "滑动验证不通过或失效，请重新验证" });
            }
        }
        #endregion

        #region 重置密码
        [HttpPost]
        public ActionResult ResetPwd(string userName, string password, string vCode)
        {
            // 效验验证码
            string inputVCode = vCode;
            string rightVCode = Session["vCode"] != null ? Session["vCode"].ToString() : "";
            // 使用一次后使其立即失效
            Session["vCode"] = null;
            if (inputVCode == rightVCode)
            {
                // 验证通过
                // 更改密码
                // 注意：实际上传过来的是邮箱
                //bool existEmail = Container.Instance.Resolve<UserInfoService>().Count(Expression.Eq("Email", userName)) >= 1;
                bool existEmail = this._userInfoService.Contains(m => m.Email == userName && !m.IsDeleted);
                if (!existEmail)
                {
                    return Json(new { code = -1, message = "没有绑定此邮箱的用户，请更换" });
                }
                //UserInfo dbModel = Container.Instance.Resolve<UserInfoService>().Query(new List<ICriterion>
                //{
                //    Expression.Eq("Email", userName)
                //}).FirstOrDefault();
                UserInfo dbModel = this._userInfoService.Find(m =>
                    m.Email == userName
                    && !m.IsDeleted
                );
                dbModel.Password = Framework.Common.EncryptHelper.MD5Encrypt32(password);
                //Container.Instance.Resolve<UserInfoService>().Edit(dbModel);
                this._userInfoService.Update(dbModel);

                return Json(new { code = 1, message = "密码重置成功，请前往登录" });
            }
            else
            {
                return Json(new { code = -1, message = "验证码错误，请重新获取并填写" });
            }
        }
        #endregion

        #region 退出账号
        public ViewResult Exit(string returnUrl = null)
        {
            if (returnUrl == null)
            {
                returnUrl = Url.Action("Index", "Home", new { area = "" });
            }
            //ViewBag.ReturnUrl = returnUrl;
            AccountManager.Exit();
            RedirectViewModel model = new RedirectViewModel
            {
                Title = "退出账号",
                Message = "成功退出登录",
                RedirectUrl = returnUrl,
                WaitSecond = 3
            };

            return View("_Redirect", model);
        }
        #endregion

        #region 初始化邮件地址数据
        private void InitEmailDic()
        {
            this.EmailDic = new Dictionary<string, string>();
            this.EmailDic.Add("126.com", "mail.126.com");
            this.EmailDic.Add("qq.com", "mail.qq.com");
            this.EmailDic.Add("163.com", "www.163.com");
        }
        #endregion
    }
}

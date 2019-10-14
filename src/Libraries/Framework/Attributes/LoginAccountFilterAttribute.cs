using System;
using System.Linq;

namespace Framework.Attributes
{
    using Framework.Common;
    using Framework.Config;
    using System.Web.Mvc;
    using Framework.Infrastructure.Abstract;
    using Framework.Factories;
    using Framework.Models;
    using Domain.Entities;
    using global::Core.Common;
    using Framework.Infrastructure.Concrete;

    /// <summary>
    /// 登录用户Session 维护器
    /// <para>当浏览器端Session被移除，但拥有 Cookie token 时，只要有效，则会在任何请求之前 将此用户信息存于 Session</para>
    /// </summary>
    public class LoginAccountFilterAttribute : ActionFilterAttribute
    {
        private static string _loginAccountSessionKey = AppConfig.LoginAccountSessionKey;
        private static string _tokenCookieKey = AppConfig.JwtName;

        //private static IDBAccessProvider _dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string areaName = filterContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RouteData.DataTokens["area"].ToString();
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            // hack install url
            if (areaName == "" && controllerName == "Install")
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            this.CheckRememberMe(filterContext);
            base.OnActionExecuting(filterContext);
        }

        #region 维护当前请求 Session 始终有登录用户值（若 已登录）
        /// <summary>
        /// 检查当前请求是否 有cookie token，如果是，则为其Session再次赋值此用户
        /// </summary>
        private void CheckRememberMe(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            var session = filterContext.HttpContext.Session;
            // 如果 Session 中已经有 登录用户，则不需要再根据 "记住我" Token 保存登录用户到 Session
            //UserInfo userInfo = Tools.GetSession<UserInfo>(AppConfig.LoginAccountSessionKey);
            UserInfo userInfo = AccountManager.GetCurrentUserInfo();
            if (userInfo != null)
            {
                return;
            }
            // 是否请求浏览器有 cookie Token
            if (request.Cookies.AllKeys.Contains(_tokenCookieKey))
            {
                if (request.Cookies[_tokenCookieKey] != null && string.IsNullOrEmpty(request.Cookies[_tokenCookieKey].Value) == false)
                {
                    string token = request.Cookies[_tokenCookieKey].Value;

                    var tokenModel = JwtHelper.Decode<JWTokenViewModel>(token, out bool verifyPass);
                    if (verifyPass)
                    {
                        // token有效 -> 检测是否已经过期
                        bool isExpired = DateTimeHelper.NowTimeStamp10() >= tokenModel.Expire;
                        if (!isExpired)
                        {
                            // token未过期
                            // 经过效验的用户信息
                            UserInfo user = HttpOneRequestFactory.Get<IDBAccessProvider>().GetUserInfoById(tokenModel.ID);
                            if (user != null)
                            {
                                // 保存到 Session
                                session[_loginAccountSessionKey] = user.ID;
                            }
                            else
                            {
                                // 用户不存在 -> 移除装有 token 的 cookie
                                response.Cookies[_tokenCookieKey].Expires = DateTime.Now.AddDays(-1);
                            }
                        }
                        else
                        {
                            // token 过期
                            response.Cookies[_tokenCookieKey].Expires = DateTime.Now.AddDays(-1);
                        }
                    }
                    else
                    {
                        // token 无效
                        response.Cookies[_tokenCookieKey].Expires = DateTime.Now.AddDays(-1);
                    }
                }
            }
        }
        #endregion
    }
}

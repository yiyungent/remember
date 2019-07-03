using Core;
using Domain;
using Framework.Config;
using Framework.Models;
using Service;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Framework.Infrastructure.Abstract;
using Framework.Factories;

namespace Framework.Infrastructure.Concrete
{
    /// <summary>
    /// 获取当前账号
    ///     若已登录，则为登录账号
    ///     未登录，则为游客角色(虚拟)账号
    /// </summary>
    public class CurrentAccountModelBinder : IModelBinder
    {
        private string _loginAccountSessionKey = AppConfig.LoginAccountSessionKey;
        private string _rememberMeTokenCookieKey = AppConfig.TokenCookieKey;
        private int _rememberMeDayCount = AppConfig.RememberMeDayCount;

        private IDBAccessProvider _dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            CurrentAccountModel rtnAccount = null;
            // 1. 检查 Session
            var user = controllerContext.HttpContext.Session?[_loginAccountSessionKey] as UserInfo;
            if (user == null)
            {
                // 1-a-A 若 Session 无登录用户，则检查 是否有"记住我" RememberMeTokenCookieKey

                if (controllerContext.HttpContext.Request.Cookies.AllKeys.Contains(_rememberMeTokenCookieKey))
                {
                    //  若有 "记住我" 则效验 "记住我" 口令
                    var request = controllerContext.HttpContext.Request;
                    var response = controllerContext.HttpContext.Response;
                    var session = controllerContext.HttpContext.Session;
                    string cookieTokenValue = request.Cookies[_rememberMeTokenCookieKey].Value;
                    UserInfo userFromToken = _dBAccessProvider.GetUserInfoByTokenCookieKey(cookieTokenValue);

                    if (userFromToken == null)
                    {
                        // 口令不正确
                        // 1-a-A-A-B 若 "记住我" 效验失败, 则为 未登录，创建游客角色账号，存于 Session, 并存账号account

                        response.Cookies[_rememberMeTokenCookieKey].Expires = DateTime.UtcNow.AddDays(-1);

                        rtnAccount = new CurrentAccountModel
                        {
                            UserInfo = UserInfo_Guest.Instance,
                            IsGuest = true
                        };
                    }
                    else if (userFromToken.LastLoginTime.AddDays(_rememberMeDayCount) > DateTime.UtcNow)
                    {
                        // 最多 "记住我" 保存7天的 登录状态

                        // 1-a-A-A-A 若 "记住我" 效验通过，则 账号存于 Session，并 存账号account

                        // 保存到 Session
                        session[_loginAccountSessionKey] = userFromToken;

                        rtnAccount = new CurrentAccountModel
                        {
                            UserInfo = userFromToken,
                            IsGuest = false
                        };
                    }
                    else
                    {
                        // 登录 已过期
                        // 1-a-A-A-B 若 "记住我" 效验失败, 则为 未登录，创建游客角色账号，存于 Session, 并存账号account

                        rtnAccount = new CurrentAccountModel
                        {
                            UserInfo = UserInfo_Guest.Instance,
                            IsGuest = true
                        };
                    }
                }
                else
                {
                    // 若无 "记住我" 则为 未登录, 则为游客
                    rtnAccount = new CurrentAccountModel
                    {
                        UserInfo = UserInfo_Guest.Instance,
                        IsGuest = true
                    };
                }

            }
            else
            {
                // 1-b 若 Session 有登录用户，则取出， 并存账号account
                rtnAccount = new CurrentAccountModel
                {
                    UserInfo = user,
                    IsGuest = false
                };
            }

            return rtnAccount;
        }
    }
}

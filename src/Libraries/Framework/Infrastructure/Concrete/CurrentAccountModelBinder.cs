using Core;
using Domain;
using Framework.Config;
using Framework.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Framework.Infrastructure.Abstract;
using Framework.Factories;
using Domain.Entities;
using Core.Common;

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
        private string _rememberMeTokenCookieKey = AppConfig.JwtName;
        private int _rememberMeDayCount = AppConfig.RememberMeDayCount;

        private IDBAccessProvider _dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            CurrentAccountModel rtnAccount = null;
            // 1. 检查 Session
            var user = controllerContext.HttpContext.Session?[_loginAccountSessionKey] as UserInfo;
            if (user == null)
            {
                // 1-a-A 若 Session 无登录用户，则检查 是否有 token: RememberMeTokenCookieKey

                if (controllerContext.HttpContext.Request.Cookies.AllKeys.Contains(_rememberMeTokenCookieKey))
                {
                    //  若有 token 则效验 token
                    var request = controllerContext.HttpContext.Request;
                    var response = controllerContext.HttpContext.Response;
                    var session = controllerContext.HttpContext.Session;
                    string token = request.Cookies[_rememberMeTokenCookieKey].Value;

                    var tokenModel = JwtHelper.Decode<JWTokenViewModel>(token, out bool verifyPass);
                    if (verifyPass)
                    {
                        // token有效 -> 检测是否已经过期
                        bool isExpired = DateTimeHelper.NowTimeStamp10() >= tokenModel.Expire;
                        if (!isExpired)
                        {
                            // token未过期
                            // 经过效验的用户信息
                            user = _dBAccessProvider.GetUserInfoById(tokenModel.ID);
                            if (user != null)
                            {
                                // 保存到 Session
                                session[_loginAccountSessionKey] = user;

                                rtnAccount = new CurrentAccountModel
                                {
                                    UserInfo = user,
                                    IsGuest = false
                                };
                            }
                            else
                            {
                                // 用户不存在 -> 移除装有 token 的 cookie
                                response.Cookies[_rememberMeTokenCookieKey].Expires = DateTime.UtcNow.AddDays(-1);

                                rtnAccount = new CurrentAccountModel
                                {
                                    UserInfo = UserInfo_Guest.Instance,
                                    IsGuest = true
                                };
                            }
                        }
                        else
                        {
                            // token 过期
                            response.Cookies[_rememberMeTokenCookieKey].Expires = DateTime.UtcNow.AddDays(-1);

                            rtnAccount = new CurrentAccountModel
                            {
                                UserInfo = UserInfo_Guest.Instance,
                                IsGuest = true
                            };
                        }
                    }
                    else
                    {
                        // token 无效
                        // 口令不正确
                        // 1-a-A-A-B 若 "记住我" 效验失败, 则为 未登录，创建游客角色账号，存于 Session, 并存账号account
                        response.Cookies[_rememberMeTokenCookieKey].Expires = DateTime.UtcNow.AddDays(-1);

                        rtnAccount = new CurrentAccountModel
                        {
                            UserInfo = UserInfo_Guest.Instance,
                            IsGuest = true
                        };
                    }
                }
                else
                {
                    // 若无 token 则为 未登录, 则为游客
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

using Core;
using Core.Common;
using Domain.Entities;
using Framework.Config;
using Framework.Models;
using Services.Interface;
using System;
using System.Linq;
using System.Web;

namespace Framework.Infrastructure.Concrete
{


    public class AccountManager
    {
        private static string _jwtName = AppConfig.JwtName;

        #region 获取当前UserInfo
        /// <summary>
        /// 获取当前 <see cref="UserInfo"/>, 若未登录，则为 <see cref="UserInfo_Guest.Instance"/>
        /// 线程内单例
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrentUserInfo(bool withoutLoginReturnGuest = false)
        {
            UserInfo rtnUserInfo = HttpSingleRequestStore.GetData("CurrentUserInfo") as UserInfo;
            if (rtnUserInfo == null)
            {
                HttpRequest request = HttpContext.Current.Request;
                try
                {
                    // 获取当前登录用户
                    int currentUserId = GetCurrentAccount().UserId;
                    try
                    {
                        rtnUserInfo = GetUserInfoById(currentUserId);
                    }
                    catch (Exception ex)
                    { }
                    if (rtnUserInfo == null)
                    {
                        if (withoutLoginReturnGuest)
                        {
                            rtnUserInfo = UserInfo_Guest.Instance;
                        }
                        else
                        {
                            rtnUserInfo = null;
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (withoutLoginReturnGuest)
                    {
                        rtnUserInfo = UserInfo_Guest.Instance;
                    }
                }

                HttpSingleRequestStore.SetData("CurrentUserInfo", rtnUserInfo);
            }

            return rtnUserInfo;
        }
        #endregion

        #region 获取当前账号
        /// <summary>
        /// 获取当前账号
        /// 线程内单例
        /// </summary>
        /// <returns></returns>
        public static CurrentAccountModel GetCurrentAccount()
        {
            CurrentAccountModel currentAccount = HttpSingleRequestStore.GetData("CurrentAccountModel") as CurrentAccountModel;
            if (currentAccount == null)
            {
                currentAccount = new CurrentAccountModel();
                HttpRequest request = HttpContext.Current.Request;
                currentAccount.LoginStatus = LoginStatus.WithoutLogin;
                try
                {
                    #region 验证口令
                    string token = null;
                    // header -> cookie
                    try
                    {
                        // header 中找 token
                        // 注意：一定要在这里才能找到自定义 Header，直接HttpContext.Current.Request.Headers是封装过的，没有自定义 Header
                        //token = request.RequestContext.HttpContext.Request.Headers[AppConfig.JwtName];
                        string authHeader = request.Headers["Authorization"];
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer"))
                        {
                            token = authHeader.Substring("Bearer ".Length).Trim();
                        }
                    }
                    catch (Exception ex)
                    { }
                    if (string.IsNullOrEmpty(token))
                    {
                        // cookie 中找 token
                        if (request.Cookies.AllKeys.Contains(_jwtName))
                        {
                            if (request.Cookies[_jwtName] != null && string.IsNullOrEmpty(request.Cookies[_jwtName].Value) == false)
                            {
                                token = request.Cookies[_jwtName].Value;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(token))
                    {
                        var tokenModel = JwtHelper.Decode<JWTokenViewModel>(token, out bool verifyPass);
                        if (verifyPass)
                        {
                            // token有效 -> 检测是否已经过期
                            bool isExpired = DateTimeHelper.NowTimeStamp10() >= tokenModel.Expire;
                            if (!isExpired)
                            {
                                // token未过期
                                // 经过效验的用户信息
                                currentAccount.LoginStatus = LoginStatus.IsLogin;
                                // 用户是否存在: 可能在过期之前被删除了
                                bool isExist = ContainerManager.Resolve<IUserInfoService>().Contains(m => m.ID == tokenModel.ID && !m.IsDeleted);
                                if (isExist)
                                {
                                    currentAccount.UserId = tokenModel.ID;
                                    currentAccount.LoginStatus = LoginStatus.IsLogin;
                                }
                                else
                                {
                                    currentAccount.LoginStatus = LoginStatus.WithoutLogin;
                                }
                            }
                            else
                            {
                                currentAccount.LoginStatus = LoginStatus.LoginTimeOut;
                            }
                        }
                        else
                        {
                            currentAccount.LoginStatus = LoginStatus.WithoutLogin;
                        }
                    }
                    else
                    {
                        currentAccount.LoginStatus = LoginStatus.WithoutLogin;
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    currentAccount.LoginStatus = LoginStatus.WithoutLogin;
                }

                HttpSingleRequestStore.SetData("CurrentAccountModel", currentAccount);
            }

            return currentAccount;
        }
        #endregion

        #region 根据UserName获取UserInfo
        public static UserInfo GetUserInfoByUserName(string userName)
        {
            UserInfo rtn = null;
            try
            {
                rtn = ContainerManager.Resolve<IUserInfoService>().Find(m => m.UserName == userName && !m.IsDeleted);
            }
            catch (Exception ex)
            { }

            return rtn;
        }
        #endregion

        #region 根据UserInfo.ID获取UserInfo
        public static UserInfo GetUserInfoById(int id)
        {
            UserInfo rtn = null;
            try
            {
                rtn = ContainerManager.Resolve<IUserInfoService>().Find(m => m.ID == id && !m.IsDeleted);
            }
            catch (Exception ex)
            { }

            return rtn;
        }
        #endregion

        #region 退出账号
        /// <summary>
        /// 退出当前登录账号
        /// </summary>
        public static void Exit()
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;
            // 浏览器 删除 cookie token
            if (request.Cookies.AllKeys.Contains(_jwtName))
            {
                response.Cookies[_jwtName].Expires = DateTime.Now.AddDays(-1);
            }
        }
        #endregion
    }
}

using Domain;
using Framework.Common;
using Framework.Config;
using Framework.Factories;
using Framework.Infrastructure.Abstract;
using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Framework.Infrastructure.Concrete
{
    public enum LoginStatus
    {
        /// <summary>
        /// 已登录
        /// </summary>
        IsLogin,

        /// <summary>
        /// 未登录
        /// </summary>
        WithoutLogin,

        /// <summary>
        /// 登录超时
        /// <para>使用"记住我", 但 已经过期</para>
        /// </summary>
        LoginTimeOut
    }

    public class AccountManager
    {
        private static string _loginAccountSessionKey = AppConfig.LoginAccountSessionKey;
        private static string _tokenCookieKey = AppConfig.TokenCookieKey;
        private static int _rememberMeDayCount = AppConfig.RememberMeDayCount;

        private static IDBAccessProvider _dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();

        #region 获取当前UserInfo
        /// <summary>
        /// 获取当前 <see cref="UserInfo"/>, 若未登录，则为 <see cref="UserInfo_Guest.Instance"/>
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrentUserInfo()
        {
            HttpRequest request = HttpContext.Current.Request;

            try
            {
                // 获取当前登录用户
                UserInfo rtnUserInfo = Tools.GetSession<UserInfo>(AppConfig.LoginAccountSessionKey);
                if (rtnUserInfo == null)
                {
                    #region 验证口令
                    if (request.Cookies.AllKeys.Contains(_tokenCookieKey))
                    {
                        if (request.Cookies[_tokenCookieKey] != null && string.IsNullOrEmpty(request.Cookies[_tokenCookieKey].Value) == false)
                        {
                            string cookieTokenValue = request.Cookies[_tokenCookieKey].Value;
                            UserInfo dbUser = _dBAccessProvider.GetUserInfoByTokenCookieKey(cookieTokenValue);

                            if (dbUser == null)
                            {
                                // 口令不正确---游客
                                rtnUserInfo = UserInfo_Guest.Instance;
                            }
                            else if (dbUser.TokenExpireAt > DateTime.UtcNow)
                            {
                                // 最多 "记住我" 保存7天的 登录状态
                                rtnUserInfo = dbUser;
                            }
                            else
                            {
                                // 登录 已过期---游客
                                rtnUserInfo = UserInfo_Guest.Instance;
                            }
                        }
                    }
                    else
                    {
                        rtnUserInfo = UserInfo_Guest.Instance;
                    }
                    #endregion
                }

                return rtnUserInfo;
            }
            catch (Exception ex)
            {
                return UserInfo_Guest.Instance;
            }
        }
        #endregion

        #region 获取当前账号
        public static CurrentAccountModel GetCurrentAccount()
        {
            CurrentAccountModel currentAccount = new CurrentAccountModel();
            currentAccount.UserInfo = GetCurrentUserInfo();
            switch (CheckLoginStatus())
            {
                case LoginStatus.IsLogin:
                    currentAccount.IsGuest = false;
                    break;
                case LoginStatus.WithoutLogin:
                    currentAccount.IsGuest = true;
                    break;
                case LoginStatus.LoginTimeOut:
                    currentAccount.IsGuest = true;
                    break;
                default:
                    currentAccount.IsGuest = true;
                    break;
            }

            return currentAccount;
        }
        #endregion

        #region 根据UserName获取UserInfo
        public static UserInfo GetUserInfoByUserName(string userName)
        {
            UserInfo rtn = null;
            rtn = _dBAccessProvider.GetUserInfoByUserName(userName);

            return rtn;
        }
        #endregion

        #region 检查登录状态-已登录/未登录(登录超时)
        /// <summary>
        /// 检查登录状态
        /// <para>已登录:1.Session有UserInfo 2.Cookie 有 有效Token(记住我)</para>
        /// <para>未登录: 无Session 且 Cookie无Token</para>
        /// <para>登录超时: Cookie 有 Token ,Token存在但已过期</para>
        /// </summary>
        /// <returns></returns>
        public static LoginStatus CheckLoginStatus()
        {
            LoginStatus loginStatus = LoginStatus.WithoutLogin;

            HttpRequest request = HttpContext.Current.Request;
            HttpSessionState session = HttpContext.Current.Session;

            UserInfo userInfo = Tools.GetSession<UserInfo>(AppConfig.LoginAccountSessionKey);
            if (userInfo != null)
            {
                loginStatus = LoginStatus.IsLogin;
            }
            else
            {

                #region 验证口令
                if (request.Cookies.AllKeys.Contains(_tokenCookieKey))
                {
                    if (request.Cookies[_tokenCookieKey] != null && string.IsNullOrEmpty(request.Cookies[_tokenCookieKey].Value) == false)
                    {
                        string cookieTokenValue = request.Cookies[_tokenCookieKey].Value;
                        UserInfo user = _dBAccessProvider.GetUserInfoByTokenCookieKey(cookieTokenValue);

                        if (user == null)
                        {
                            // 口令不正确
                            loginStatus = LoginStatus.WithoutLogin;
                        }
                        else if (user.TokenExpireAt > DateTime.UtcNow)
                        {
                            // 最多 "记住我" 保存7天的 登录状态
                            loginStatus = LoginStatus.IsLogin;
                        }
                        else
                        {
                            // 登录 已过期
                            loginStatus = LoginStatus.LoginTimeOut;
                        }
                    }
                }
                else
                {
                    loginStatus = LoginStatus.WithoutLogin;
                }
                #endregion

            }

            return loginStatus;
        }
        #endregion

        #region 更新 当前 Session 内的 UserInfo
        public static void UpdateSessionAccount()
        {
            UserInfo userInfo = GetCurrentUserInfo();
            if (userInfo == null)
            {
                // 游客(未登录)
            }
            else
            {
                int userInfoId = userInfo.ID;
                Tools.SetSession(AppConfig.LoginAccountSessionKey, _dBAccessProvider.GetUserInfoById(userInfoId));
            }
        }
        #endregion

        #region 退出账号
        /// <summary>
        /// 退出当前登录账号
        /// </summary>
        public static void Exit()
        {
            // 浏览器删除 session 用户信息
            Tools.SetSession(_loginAccountSessionKey, null);
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;
            // 浏览器 删除 cookie token
            if (request.Cookies.AllKeys.Contains(_tokenCookieKey))
            {
                response.Cookies[_tokenCookieKey].Expires = DateTime.UtcNow.AddDays(-1);
            }
            // 数据库删除 token，并过期
            UserInfo userInfo = GetCurrentUserInfo();
            userInfo.Token = null;
            userInfo.TokenExpireAt = DateTime.Now;
            _dBAccessProvider.EditUserInfo(userInfo);
        }
        #endregion
    }
}

using Core.Common;
using Domain;
using Domain.Entities;
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
        private static string _jwtName = AppConfig.JwtName;

        /// <summary>
        /// TODO: 不能这么做，因为静态会导致全局唯一，而 DBAccessProvider 中的Service中存有DbContext，这样会导致使用旧的已经释放的DbContext
        /// </summary>
        //private static IDBAccessProvider _dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();

        #region 获取当前UserInfo
        /// <summary>
        /// 获取当前 <see cref="UserInfo"/>, 若未登录，则为 <see cref="UserInfo_Guest.Instance"/>
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrentUserInfo(bool withoutLoginReturnGuest = false)
        {
            HttpRequest request = HttpContext.Current.Request;
            UserInfo rtnUserInfo = null;
            try
            {
                // 获取当前登录用户
                try
                {
                    int userInfoId = Tools.GetSession<int>(AppConfig.LoginAccountSessionKey);
                    rtnUserInfo = GetUserInfoById(userInfoId);
                }
                catch (Exception ex)
                { }

                if (rtnUserInfo == null)
                {
                    if (withoutLoginReturnGuest)
                    {
                        rtnUserInfo = UserInfo_Guest.Instance;
                    }
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
                                //rtnUserInfo = _dBAccessProvider.GetUserInfoById(tokenModel.ID);
                                rtnUserInfo = HttpOneRequestFactory.Get<IDBAccessProvider>().GetUserInfoById(tokenModel.ID);
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (withoutLoginReturnGuest)
                {
                    rtnUserInfo = UserInfo_Guest.Instance;
                }
            }

            return rtnUserInfo;
        }
        #endregion

        #region 获取当前账号
        public static CurrentAccountModel GetCurrentAccount()
        {
            CurrentAccountModel currentAccount = new CurrentAccountModel();
            currentAccount.UserInfo = GetCurrentUserInfo(withoutLoginReturnGuest: true);
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
            rtn = HttpOneRequestFactory.Get<IDBAccessProvider>().GetUserInfoByUserName(userName);

            return rtn;
        }
        #endregion

        #region 根据UserInfo.ID获取UserInfo
        public static UserInfo GetUserInfoById(int id)
        {
            UserInfo rtn = null;
            rtn = HttpOneRequestFactory.Get<IDBAccessProvider>().GetUserInfoById(id);

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
            // TODO:未进行检测 Headers JWT
            LoginStatus loginStatus = LoginStatus.WithoutLogin;

            HttpRequest request = HttpContext.Current.Request;
            HttpSessionState session = HttpContext.Current.Session;

            int userInfoId = Tools.GetSession<int>(AppConfig.LoginAccountSessionKey);
            UserInfo userInfo = GetUserInfoById(userInfoId);
            if (userInfo != null)
            {
                // Session内存中存在 -> 已登录
                loginStatus = LoginStatus.IsLogin;
            }
            else
            {
                #region 验证口令
                bool cookieExistJwt = request.Cookies.AllKeys.Contains(_jwtName) && request.Cookies[_jwtName] != null && string.IsNullOrEmpty(request.Cookies[_jwtName].Value) == false;
                string authHeader = null;
                string token = null;
                try
                {
                    authHeader = request.Headers["Authorization"];
                }
                catch (Exception ex)
                { }
                if (cookieExistJwt)
                {
                    token = request.Cookies[_jwtName].Value;
                }
                else if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer"))
                {
                    token = authHeader.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    // 无 token -> 未登录
                    loginStatus = LoginStatus.WithoutLogin;
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
                            UserInfo user = HttpOneRequestFactory.Get<IDBAccessProvider>().GetUserInfoById(tokenModel.ID);
                            if (user != null)
                            {
                                loginStatus = LoginStatus.IsLogin;
                            }
                            else
                            {
                                // 不存在此用户 -> 未登录
                                loginStatus = LoginStatus.WithoutLogin;
                            }
                        }
                        else
                        {
                            // 登录 已过期
                            // 最多 "记住我" 保存7天的 登录状态
                            loginStatus = LoginStatus.LoginTimeOut;
                        }
                    }
                    else
                    {
                        // token 无效 -> 未登录
                        loginStatus = LoginStatus.WithoutLogin;
                    }
                }
                #endregion

            }

            return loginStatus;
        }
        #endregion

        #region 更新 当前 Session 内的 UserInfo
        /// <summary>
        /// 
        /// </summary>
        [Obsolete(message: "现在已经改为Session只保存用户的ID，每次都利用此ID重新查询用户信息，此方法不再需要")]
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
                Tools.SetSession(AppConfig.LoginAccountSessionKey, userInfoId);
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
            if (request.Cookies.AllKeys.Contains(_jwtName))
            {
                response.Cookies[_jwtName].Expires = DateTime.Now.AddDays(-1);
            }
            // 数据库删除 token，并过期
            //UserInfo userInfo = GetCurrentUserInfo();
            //userInfo.RefreshToken = null;
            //HttpOneRequestFactory.Get<IDBAccessProvider>().EditUserInfo(userInfo);
        }
        #endregion
    }
}

using Common;
using Framework.Config;
using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using WebApi.Infrastructure;
using WebApi.Models.Common;

namespace WebApi.Attributes
{
    public class NeedAuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// TODO: 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // 获取 token -> 1.header 2.cookie
            // TODO: 从 Header->Authorization 获取 token
            string token = "";
            // Bearer JWToken ->  空格前 Bearer 为 Scheme，空格后 JWToken 为 Parameter
            AuthenticationHeaderValue authenticationHeader = actionContext.Request.Headers.Authorization;
            //if (actionContext.Request.Headers.TryGetValues(AppConfig.JwtName, out var headerValues))
            if (authenticationHeader != null && authenticationHeader.Scheme == "Bearer")
            {
                token = authenticationHeader.Parameter;

                if (string.IsNullOrEmpty(token))
                {
                    // header 中没有 token，尝试从 cookie 中读取
                    var cookies = actionContext.Request.Headers.GetCookies(AppConfig.JwtName)?.FirstOrDefault();
                    token = cookies[AppConfig.JwtName]?.Value;
                }

                var tokenModel = JwtHelper.Decode<JWTokenViewModel>(token, out bool verifyPass);
                if (verifyPass)
                {
                    // token有效 -> 检测是否已经过期
                    bool isExpired = DateTimeHelper.NowTimeStamp10() >= tokenModel.Expire;
                    if (!isExpired)
                    {
                        // token未过期
                        // 将经过效验的用户信息保存
                        actionContext.RequestContext.Principal = new UserPrincipal(new UserIdentity(tokenModel.ID, tokenModel.UserName));

                        return true;
                    }
                    else
                    {
                        // token过期
                        return false;
                    }
                }
                else
                {
                    // token无效
                    return false;
                }
            }
            else
            {
                // 无 token
                return false;
            }
        }
    }
}
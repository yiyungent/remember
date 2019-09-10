using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            string token = "";
            if (actionContext.Request.Headers.TryGetValues("token", out var headerValues))
            {
                token = headerValues.FirstOrDefault();

                var tokenModel = JwtHelper.Decode<JWTokenViewModel>(token, out bool verifyPass);
                if (verifyPass)
                {
                    // 令牌有效 -> 检测是否已经过期
                    bool isExpired = DateTimeHelper.NowTimeStamp10() >= tokenModel.Expire;
                    if (!isExpired)
                    {
                        // 令牌未过期
                        // 将经过效验的用户信息保存
                        actionContext.RequestContext.Principal = new UserPrincipal(new UserIdentity(tokenModel.ID, tokenModel.UserName));

                        return base.IsAuthorized(actionContext);
                    }
                    else
                    {
                        // 令牌过期
                        return false;
                    }
                }
                else
                {
                    // 令牌无效
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
using Remember.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.Attributes
{
    /// <summary>
    /// 登录认证
    /// </summary>
    public class AuthenLoginAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string _sessionKey = "User";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.Session[_sessionKey];
            if (user == null)
            {
                string returnUrl = filterContext.HttpContext.Request.RawUrl;
                returnUrl = HttpUtility.UrlDecode(returnUrl);
                System.Web.Routing.RouteValueDictionary routeDic = new System.Web.Routing.RouteValueDictionary();
                routeDic.Add("Action", "Login");
                routeDic.Add("returnUrl", returnUrl);
                filterContext.Result = new RedirectToRouteResult(routeDic);
            }
        }
    }
}
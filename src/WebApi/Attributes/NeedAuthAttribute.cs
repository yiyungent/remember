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
            actionContext.RequestContext.Principal = new UserPrincipal(new UserIdentity(1, "admin"));

            return base.IsAuthorized(actionContext);
        }
    }
}
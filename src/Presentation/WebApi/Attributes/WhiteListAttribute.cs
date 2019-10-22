using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApi.Attributes
{
    /// <summary>
    /// 注意：对于所有 OPTIONS 请求，必须在 Action 上标记 HttpOptions ，否则根本不会进过滤器，而是先会被 asp.net webapi 内部拦截，因为没有支持此http方法的标记
    /// 此方法失败：改用消息管道，先于  HttpMethod 注解之前
    /// </summary>
    public class WhiteListAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IList<string> whiteList = new List<string>();
            // TODO: WebApiWhiteList 白名单待查数据库
            whiteList.Add("*");

            actionContext.Response = new HttpResponseMessage();
            actionContext.Response.Headers.Add("Access-Control-Allow-Origin", whiteList);

            if (actionContext.Request.Method == HttpMethod.Options)
            {
                actionContext.Response.StatusCode = System.Net.HttpStatusCode.OK;
                return;
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }
    }
}
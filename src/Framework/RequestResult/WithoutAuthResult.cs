using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Framework.RequestResult
{

    public class WithoutAuthResultProvider
    {
        public static ActionResult Get(bool isAjax, string returnUrl = null)
        {
            ActionResult rtnResult = null;
            if (isAjax)
            {
                rtnResult = new Ajax_WithoutAuthResult(returnUrl);
            }
            else
            {
                rtnResult = new Redirect_WithoutAuthResult(returnUrl);
            }

            return rtnResult;
        }

        public static ActionResult Get(HttpRequestBase requestBase)
        {
            bool isAjax = requestBase.IsAjaxRequest();
            string returnUrl = requestBase.Url.AbsoluteUri;

            return Get(isAjax, returnUrl);
        }

        public static ActionResult Get()
        {
            HttpRequestBase requestBase = HttpContext.Current.Request.RequestContext.HttpContext.Request;

            return Get(requestBase);
        }
    }

    #region RedirectResult
    public class Redirect_WithoutAuthResult : ActionResult
    {
        public Redirect_WithoutAuthResult(string returnUrl = null)
        {
            RouteValueDictionary routeValDic = new RouteValueDictionary();
            routeValDic.Add("controller", "Errors");
            routeValDic.Add("action", "WithoutAuth");
            routeValDic.Add("area", "");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                routeValDic.Add("returnUrl", returnUrl);
            }
            this.RouteValues = routeValDic;

            this.ReturnUrl = returnUrl;
        }

        /// <summary>
        /// 注意：在 new 时 RouteValues 其中的 returnUrl 就已经被决定
        /// </summary>
        public string ReturnUrl { get; private set; }

        public RouteValueDictionary RouteValues { get; set; }

        /// <summary>
        /// 因为 <see cref="MvcResources"/> 为 internal ，导致无法使用，暂时用此方法
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            RedirectToRouteResult redirectToRouteResult = new RedirectToRouteResult(this.RouteValues);
            redirectToRouteResult.ExecuteResult(context);
        }
    }
    #endregion

    #region AjaxJsonResult
    public class Ajax_WithoutAuthResult : JsonResult
    {
        public Ajax_WithoutAuthResult(string returnUrl = null)
        {
            this.Data = new { code = -1, message = "无此权限", returnUrl = returnUrl };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            this.ReturnUrl = returnUrl;
        }

        /// <summary>
        /// 注意：在 new 时 Data 其中的 returnUrl 就已经被决定
        /// </summary>
        public string ReturnUrl { get; private set; }
    }
    #endregion
}

using Framework.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Properties;
using System.Web.Routing;

namespace Framework.RequestResult
{
    /// <summary>
    /// 指示需要登录，当前未登录
    /// </summary>
    public class NeedLoginResultProvider
    {
        public static ActionResult Get(bool isAjax, string returnUrl = null)
        {
            ActionResult rtnResult = null;
            if (isAjax)
            {
                rtnResult = new Ajax_NeedLoginResult(returnUrl);
            }
            else
            {
                //rtnResult = new Redirect_NeedLoginResult(returnUrl);
                rtnResult = new View_NeedLoginResult(returnUrl);
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
    /// <summary>
    /// 需要登录结果
    /// <para>跳转到错误地址</para>
    /// </summary>
    public class Redirect_NeedLoginResult : ActionResult
    {

        public Redirect_NeedLoginResult(string returnUrl = null)
        {
            RouteValueDictionary routeValDic = new RouteValueDictionary();
            routeValDic.Add("controller", "Errors");
            routeValDic.Add("action", "NeedLogin");
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

    #region ViewResult
    public class View_NeedLoginResult : ViewResult
    {
        public View_NeedLoginResult(string returnUrl = null)
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            this.ReturnUrl = returnUrl;
            // 注意, this.Model 不可写，且无法 override, 通过写入 this.ViewData.Model，然后读 this.Model 时实则就是 读 this.ViewData.Model
            this.ViewData.Model = new ErrorRedirectViewModel
            {
                Title = "请登录",
                Message = "该页面需要登录",
                RedirectUrl = url.Action("Index", "Login", new { area = "Account", returnUrl = returnUrl }),
                RedirectUrlName = "登录页",
                WaitSecond = 8
            };

            this.ViewName = "_ErrorRedirect";
        }

        public string ReturnUrl { get; private set; }
    }
    #endregion

    #region AjaxJsonResult
    public class Ajax_NeedLoginResult : JsonResult
    {
        public Ajax_NeedLoginResult(string returnUrl = null)
        {
            this.Data = new { code = -1, message = "请登录", returnUrl = returnUrl };
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

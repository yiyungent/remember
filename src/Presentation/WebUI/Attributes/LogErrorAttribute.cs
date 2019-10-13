using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Attributes
{
    public class LogErrorAttribute : HandleErrorAttribute
    {
        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();
        public override void OnException(ExceptionContext filterContext)
        {
            ExceptionQueue.Enqueue(filterContext.Exception);
            //filterContext.HttpContext.Response.Redirect("~/Error.html");//出现异常时可以考虑让系统跳转到友好界面
            base.OnException(filterContext);
        }
    }
}
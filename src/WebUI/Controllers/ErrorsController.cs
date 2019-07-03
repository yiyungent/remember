using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ErrorsController : Controller
    {
        public ViewResult WithoutAuth(string returnUrl)
        {
            ErrorRedirectViewModel model = new ErrorRedirectViewModel
            {
                Title = "权限不足",
                Message = "你没有权限访问此页面",
                RedirectUrl = Url.Action("Index", "Home", new { area = "", returnUrl = returnUrl }),
                RedirectUrlName = "首页",
                WaitSecond = 8
            };

            return View("_ErrorRedirect", model);
        }

        public ViewResult NeedLogin(string returnUrl)
        {
            ErrorRedirectViewModel model = new ErrorRedirectViewModel
            {
                Title = "请登录",
                Message = "该页面需要登录",
                RedirectUrl = Url.Action("Index", "Login", new { area = "Account", returnUrl = returnUrl }),
                RedirectUrlName = "登录页",
                WaitSecond = 8
            };

            return View("_ErrorRedirect", model);
        }

        public ViewResult LoginTimeOut(string returnUrl)
        {
            ErrorRedirectViewModel model = new ErrorRedirectViewModel
            {
                Title = "请重新登录",
                Message = "你的登录状态已超时",
                RedirectUrl = Url.Action("Index", "Login", new { area = "Account", returnUrl = returnUrl }),
                RedirectUrlName = "登录页",
                WaitSecond = 8
            };

            return View("_ErrorRedirect", model);
        }

        /// <summary>
        /// page not found
        /// </summary>
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;
            // 不知道为什么，不加这句默认会网页乱码，浏览器切换字符集又会显示为网页源代码
            this.Response.ContentType = "text/html";

            return View();
        }
    }
}

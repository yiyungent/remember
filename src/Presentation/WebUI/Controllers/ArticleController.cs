using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ArticleController : Controller
    {
        #region 用于自定义Url的文章内容展示
        public ActionResult Index()
        {
            Article viewModel = (Article)System.Web.HttpContext.Current.Items["CmsPage"];

            return View(viewModel);
        } 
        #endregion
    }
}
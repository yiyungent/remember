using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using Framework.Mvc.ViewEngines.Templates;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region Ctor
        public HomeController()
        {

        }
        #endregion

        #region 首页
        public ActionResult Index()
        {
            // 当前登录账号/未登录
            CurrentAccountModel currentAccount = AccountManager.GetCurrentAccount();

            ArticleService articleService = Container.Instance.Resolve<ArticleService>();
            var articleList = articleService.GetAll();
            ViewBag.ArticleList = articleList;

            return View(currentAccount);
        }
        #endregion

        #region 测试首页
        public ActionResult Test()
        {
            return View();
        }
        #endregion
    }
}
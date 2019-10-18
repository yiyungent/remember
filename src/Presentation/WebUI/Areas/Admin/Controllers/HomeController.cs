using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;
using Framework.Extensions;
using Domain.Entities;
using Services.Interface;

namespace WebUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private AuthManager _authManager;

        public HomeController()
        {
            this._authManager = new AuthManager();
            string title = WebSetting.Get("WebUITitle").Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries)[0];
            ViewBag.PageHeader = title.Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries)[0];
            ViewBag.PageHeaderDescription = title;
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>();

            UserInfo currentUserInfo = AccountManager.GetCurrentUserInfo(true);
            ViewBag.CurrentUserInfo = currentUserInfo;
            // TODO: 后台菜单加载
            ViewBag.MenuList = null;
        }

        #region 后台框架
        public ViewResult Index(CurrentAccountModel currentAccount)
        {
            return View();
        }
        #endregion

        #region 后台默认主页-Home
        public ViewResult Default()
        {
            return View();
        }
        #endregion

        public PartialViewResult LeftMenuPartial()
        {
            ViewBag.AllMenuList = null;// this._authManager.GetMenuListByUserInfo(AccountManager.GetCurrentUserInfo());

            return PartialView("_LeftMenuPartial");
        }
    }
}
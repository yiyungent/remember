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
        #region Fields
        private AuthManager _authManager;
        private readonly IUserInfoService _userInfoService;
        private readonly IRoleInfoService _roleInfoService;
        private readonly IRole_MenuService _role_MenuService;
        private readonly ISys_MenuService _sys_MenuService;
        #endregion

        public HomeController(IUserInfoService userInfoService, IRoleInfoService roleInfoService, IRole_MenuService role_MenuService, ISys_MenuService sys_MenuService)
        {
            this._authManager = new AuthManager();
            this._userInfoService = userInfoService;
            this._roleInfoService = roleInfoService;
            this._role_MenuService = role_MenuService;
            this._sys_MenuService = sys_MenuService;

            string title = WebSetting.Get("WebUITitle").Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries)[0];
            ViewBag.PageHeader = title.Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries)[0];
            ViewBag.PageHeaderDescription = title;
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>();

            UserInfo currentUserInfo = AccountManager.GetCurrentUserInfo(true);
            ViewBag.CurrentUserInfo = currentUserInfo;
            // TODO: 后台菜单加载
            ViewBag.MenuList = this._userInfoService.UserHaveSys_Menus(currentUserInfo.ID);
        }

        #region 后台框架
        public ViewResult Index()
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
            int currentUserId = AccountManager.GetCurrentAccount().UserId;
            ViewBag.AllMenuList = this._userInfoService.UserHaveSys_Menus(currentUserId);// this._authManager.GetMenuListByUserInfo(AccountManager.GetCurrentUserInfo());

            return PartialView("_LeftMenuPartial");
        }
    }
}
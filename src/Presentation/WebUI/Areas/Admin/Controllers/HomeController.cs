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
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly IUserInfoService _userInfoService;
        private readonly IRoleInfoService _roleInfoService;
        private readonly IRole_MenuService _role_MenuService;
        private readonly ISys_MenuService _sys_MenuService;
        #endregion

        public HomeController(IUserInfoService userInfoService, IRoleInfoService roleInfoService, IRole_MenuService role_MenuService, ISys_MenuService sys_MenuService)
        {
            this._userInfoService = userInfoService;
            this._roleInfoService = roleInfoService;
            this._role_MenuService = role_MenuService;
            this._sys_MenuService = sys_MenuService;
        }

        #region 后台首页
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Dashboard", new { area = "Admin" });
        }
        #endregion


    }
}
using Remember.Core;
using Remember.Domain;
using Remember.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.Controllers
{
    public class AdminController : Controller
    {
        #region 后台首页
        public ViewResult Index()
        {
            IList<SysMenu> allMenuList=Container.Instance.Resolve<SysMenuService>().GetAll();
            ViewBag.AllMenuList = allMenuList;

            return View();
        }
        #endregion

    }
}

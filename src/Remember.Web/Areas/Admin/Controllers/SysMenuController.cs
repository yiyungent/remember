using Remember.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.Areas.Admin.Controllers
{
    public class SysMenuController : BaseController
    {
        #region 菜单列表首页
        public ViewResult Index()
        {
            return View();
        }
        #endregion
    }
}

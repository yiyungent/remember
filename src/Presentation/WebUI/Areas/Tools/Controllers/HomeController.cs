using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Tools.Controllers
{
    /// <summary>
    /// 工具箱
    /// </summary>
    public class HomeController : Controller
    {
        #region 首页
        /// <summary>
        /// 首页-展示有哪些小工具
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        } 
        #endregion
    }
}
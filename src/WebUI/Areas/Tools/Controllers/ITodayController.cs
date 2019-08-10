using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Tools.Controllers
{
    /// <summary>
    /// 工具箱-爱今天
    /// </summary>
    public class ITodayController : Controller
    {
        #region 首页-展示可视化数据
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 导入数据
        [HttpPost]
        public JsonResult Import()
        {
            return Json(new { });
        }
        #endregion
    }
}
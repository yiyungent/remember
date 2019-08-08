using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class CardBoxController : Controller
    {
        #region 卡片盒市场首页
        /// <summary>
        /// 卡片盒市场首页
        /// </summary>
        /// <param name="q">搜索关键词</param>
        /// <returns></returns>
        public ActionResult Index(string q = null)
        {
            return View();
        }
        #endregion

        #region 卡片盒描述页
        /// <summary>
        /// 卡片盒描述页
        /// </summary>
        /// <param name="id">卡片盒Id</param>
        /// <returns></returns>
        public ActionResult Index(int id, string q = null)
        {
            return View();
        }
        #endregion

        #region 卡片页
        /// <summary>
        /// 卡片页
        /// </summary>
        /// <param name="id">卡片Id</param>
        /// <returns></returns>
        public ActionResult Card(int id = 0, string q = null)
        {
            return View();
        }
        #endregion
    }
}
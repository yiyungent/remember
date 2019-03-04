using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;

namespace Remember.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        #region 初始化数据库
        public ActionResult InitDB()
        {
            return View();
        }

        public ActionResult StartInitDB()
        {
            CreateDB();
            return View("InitDB");
        }

        private void CreateDB()
        {
            try
            {
                Response.Write("开始创建数据库结构<br/>");
                ActiveRecordStarter.CreateSchema();
                Response.Write("......成功<br/>");
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("......失败！原因： {0}<br/>", ex.Message));
            }
        }
        #endregion

    }
}

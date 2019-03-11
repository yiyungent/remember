using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Newtonsoft.Json;
using Remember.Web.Models;
using System.Threading;

namespace Remember.Web.Controllers
{
    public delegate void Del_Install(ref InstallProgressList list);
    public class InstallController : Controller
    {
        private event Del_Install _onInstall;

        private static InstallProgressList _installProgressList = new InstallProgressList();

        private bool _isInstallPause = false;

        public InstallController()
        {
            _onInstall += CreateSchema;
            _onInstall += InitTableData;
        }

        #region 安装视图
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 开始安装
        [HttpPost]
        public ActionResult Index(string flag)
        {
            if (Request.IsAjaxRequest())
            {
                ExecInstall();

                return PartialView("_InstallProgress");
            }
            else
            {
                return Content("非法请求");
            }
        }
        #endregion

        #region 执行安装
        public void ExecInstall()
        {
            _onInstall(ref _installProgressList);
        }
        #endregion

        #region 初始化表数据
        private void InitTableData(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "表初始化数据" };
            try
            {

                pro.isSuccess = true;
            }
            catch (Exception ex)
            {
                pro.exception = ex;
            }
            Thread.Sleep(3000);
        }
        #endregion

        #region 创建数据库表结构
        private void CreateSchema(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "创建数据库 表结构" };
            list.AddItem(pro);
            try
            {
                ActiveRecordStarter.CreateSchema();
                pro.isSuccess = true;
            }
            catch (Exception ex)
            {
                pro.exception = ex;
            }
            Thread.Sleep(3000);
        }
        #endregion

        #region 获取安装进度
        [HttpPost]
        public JsonResult GetProgress()
        {
            var progressInfo = new { progress = _installProgressList.List, code = _isInstallPause ? -1 : 1 };
            return Json(progressInfo);
        }
        #endregion

        #region 获取安装步骤数
        public int GetProgressCount()
        {
            return _onInstall.GetInvocationList().Count();
        }
        #endregion
    }
}

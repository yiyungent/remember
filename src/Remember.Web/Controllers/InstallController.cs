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
    public delegate void Del_OnInstall(ref InstallProgressList list);
    public class InstallController : Controller
    {
        private event Del_OnInstall _onInstall;

        private static InstallProgressList _installProgressList = new InstallProgressList();

        public InstallController()
        {
            _onInstall += CreateSchema;
            _onInstall += InitTableData;
        }

        #region Index视图
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 开始安装
        [HttpPost]
        public void Index(InstallConfig installConfig)
        {
            if(!ModelState.IsValid)
            { 
                
                return;
            }
            Response.RedirectToRoute(new { action = "InstallProgress" });
        }
        #endregion

        #region 安装进度视图
        public ActionResult InstallProgress()
        {
            return View("InstallProgress");
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
            _installProgressList.AddItem(pro);
            try
            {

                ShowProgressMsg(pro.info);
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
                ShowProgressMsg(pro);
            }
            catch (Exception ex)
            {
                pro.exception = ex;
            }
            Thread.Sleep(3000);
        }
        #endregion

        #region 输出当前进度消息
        private void ShowProgressMsg(InstallProgress progres)
        {
            string js = "<script type=\"text/javascript\">window.scrollTo(0, document.body.scrollHeight);<" + "/script>";
            string message = string.Format("<div>{0}...{1}</div>", progres.info, progres.isSuccess ? "成功" : "失败");
            Response.Write(message);
            Response.Write(js);
            Response.Flush();
        }
        private void ShowProgressMsg(string message)
        {
            string js = "<script type=\"text/javascript\">window.scrollTo(0, document.body.scrollHeight);<" + "/script>";
            Response.Write(message);
            Response.Write(js);
            Response.Flush();
        }
        #endregion
    }
}

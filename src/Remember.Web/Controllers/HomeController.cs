using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Remember.Core;
using Remember.Service;
using Remember.Domain;

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
            CreateSchema();
            InitUser();
        }

        #region 初始化用户
        private void InitUser()
        {
            try
            {
                Response.Write("......初始化用户<br/>");

                Container.Instance.Resolve<SysUserService>().Create(new SysUser
                {
                    Name = "系统管理员",
                    LoginAccount = "admin",
                    Password = "123456",
                    Status = 0
                });

                Container.Instance.Resolve<SysUserService>().Create(new SysUser
                {
                    Name = "亦云",
                    LoginAccount = "yiyun",
                    Password = "123456",
                    Status = 0
                });

                Response.Write("......初始化用户ok<br/>");
            }
            catch (Exception ex)
            {
                Response.Write("......初始化用户Error<br/>");
            }
        }
        #endregion

        #region 创建数据库结构
        private void CreateSchema()
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

        #endregion

    }
}

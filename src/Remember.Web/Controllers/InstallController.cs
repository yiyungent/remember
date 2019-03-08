using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Newtonsoft.Json;

namespace Remember.Web.Controllers
{
    public class InstallController : Controller
    {
        public static Models.CurrentProgress CurrentProgress { get; set; }

        #region 安装视图
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 执行安装
        [HttpPost]
        public ActionResult Index(string flag)
        {
            if (Request.IsAjaxRequest())
            {
                Models.InstallInfoViewModel model = new Models.InstallInfoViewModel
                {
                    InstallTime = DateTime.Now,
                    Errors = new List<Exception>()
                };

                bool isContinue = true;

                try
                {
                    CurrentProgress = new Models.CurrentProgress();
                    System.Threading.Thread.Sleep(1000);
                    CurrentProgress.currentInfo = "开始创建数据库表结构...";
                    CurrentProgress.currentPos = 10;
                    CurrentProgress.code = 1;
                    CreateSchema();
                    System.Threading.Thread.Sleep(1000);
                    CurrentProgress.currentInfo = "数据库表结构创建成功";
                    CurrentProgress.currentPos += 10;
                }
                catch (Exception ex)
                {
                    CurrentProgress.currentInfo = "数据库表结构创建失败";
                    CurrentProgress.code = -2;
                    model.Errors.Add(ex);
                    isContinue = false;
                }

                if (isContinue)
                {
                    try
                    {
                        // 若数据库表结构创建成功，才继续进行初始化表数据
                        System.Threading.Thread.Sleep(1000);
                        CurrentProgress.currentInfo = "开始表初始化数据...";
                        CurrentProgress.currentPos += 10;
                        InitTableData();
                        System.Threading.Thread.Sleep(1000);
                        CurrentProgress.currentInfo = "表初始化数据完成";
                    }
                    catch (Exception ex)
                    {
                        CurrentProgress.currentInfo = "数据表初始化数据失败";
                        CurrentProgress.code = -2;
                        model.Errors.Add(ex);
                    }
                }

                if (model.Errors.Count == 0)
                {
                    model.Result = Models.InstallResult.Success;
                }
                else
                {
                    model.Result = Models.InstallResult.Failure;
                }

                return PartialView("_InstallProgress", model);
            }
            else
            {
                return Content("非法请求");
            }
        }

        #region 初始化表数据
        private void InitTableData()
        {

        }
        #endregion

        #region 创建数据库表结构
        private void CreateSchema()
        {
            ActiveRecordStarter.CreateSchema();
        }
        #endregion

        #endregion

        #region 获取当前安装进度
        [HttpPost]
        public JsonResult GetProgress()
        {
            return Json(CurrentProgress);
        }
        #endregion

    }
}

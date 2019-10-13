using System;
using System.Collections.Generic;
using System.IO;
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

        #region 上传数据
        [HttpGet]
        public ViewResult Upload()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload(bool flag = true)
        {
            string basePath = "~/Upload/Tools/IToday/";

            // 如果路径含有~，即需要服务器映射为绝对路径，则进行映射
            basePath = (basePath.IndexOf("~") > -1) ? System.Web.HttpContext.Current.Server.MapPath(basePath) : basePath;
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            // 如果目录不存在，则创建目录
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            string fileName = System.Web.HttpContext.Current.Request["name"];
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = file.FileName;
            }
            // 文件保存
            string fullPath = basePath + fileName;
            file.SaveAs(fullPath);

            FileResult rtnJsonObj = new FileResult
            {
                fileName = fileName
            };

            return Json(rtnJsonObj);
        }
        #endregion

        #region Helpers

        #region 上传文件json结果
        sealed class FileResult
        {
            public string fileName { get; set; }
        }
        #endregion  

        #endregion
    }
}
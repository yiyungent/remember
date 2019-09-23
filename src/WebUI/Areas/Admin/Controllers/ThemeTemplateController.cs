using Common;
using Core;
using Domain;
using Framework.Common;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc.ViewEngines.Templates;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Areas.Admin.Models.ThemeTemplateVM;

namespace WebUI.Areas.Admin.Controllers
{
    public class ThemeTemplateController : Controller
    {
        #region Ctor
        public ThemeTemplateController()
        {
            ViewBag.PageHeader = "主题模板";
            ViewBag.PageHeaderDescription = "主题模板";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("系统管理"),
            };
        }
        #endregion

        #region 主题模板列表
        public ActionResult Index(int pageIndex = 1, int pageSize = 6, string cat = "open")
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();
            ThemeTemplateListViewModel viewModel = new ThemeTemplateListViewModel(queryConditions, pageIndex, pageSize, HttpContext, cat);

            ViewBag.Cat = cat;
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }
        #endregion

        #region 上传本地主题模板
        [HttpGet]
        public ViewResult UploadTemplate()
        {
            return View();
        }

        public JsonResult UploadTemplateFile()
        {
            string basePath = "~/Upload/TemplateInstallZip/";

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

        #region 安装包
        public JsonResult InstallZip(string templateName)
        {
            try
            {
                bool isSuccess = InstallLocationTemplate(templateName);
                if (isSuccess)
                {
                    return Json(new { code = 1, message = templateName + " 安装成功" });
                }
                else
                {
                    return Json(new { code = -1, message = templateName + " 安装失败" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = templateName + " 安装失败" });
            }
        }
        #endregion

        #region 卸载
        public JsonResult Uninstall(int id)
        {
            try
            {
                bool isExist = Container.Instance.Resolve<ThemeTemplateService>().Exist(id);
                if (!isExist)
                {
                    return Json(new { code = -1, message = "卸载失败, 要卸载的模板不存在，或未安装" });
                }
                ThemeTemplate dbModel = Container.Instance.Resolve<ThemeTemplateService>().GetEntity(id);

                Container.Instance.Resolve<ThemeTemplateService>().Delete(id);
                try
                {
                    FileHelper.DeleteDir(Server.MapPath(@"~/Templates/" + dbModel.TemplateName));
                }
                catch (Exception ex)
                { }

                return Json(new { code = 1, message = "卸载成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "卸载失败" });
            }
        }
        #endregion

        #region 删除安装包
        public JsonResult DeleteInstallZip(string templateName)
        {
            try
            {
                string installZipPath = Server.MapPath("~/Upload/TemplateInstallZip/" + templateName + ".zip");
                System.IO.File.Delete(installZipPath);

                return Json(new { code = 1, message = "删除安装包 " + templateName + ".zip 成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "删除安装包 " + templateName + ".zip 失败" });
            }
        }
        #endregion

        #region 启用禁用模板
        public JsonResult OpenClose(int id)
        {
            try
            {
                if (!Container.Instance.Resolve<ThemeTemplateService>().Exist(id))
                {
                    return Json(new { code = -1, message = "指定的模板不存在" });
                }
                ThemeTemplate dbModel = Container.Instance.Resolve<ThemeTemplateService>().GetEntity(id);
                string msg = "";
                switch (dbModel.IsOpen)
                {
                    case 0:
                        msg = "启用";
                        dbModel.IsOpen = 1;
                        break;
                    case 1:
                        dbModel.IsOpen = 0;
                        string defaultTemplateName = Container.Instance.Resolve<SettingService>().GetSet("DefaultTemplateName");
                        if (defaultTemplateName.ToLower() == dbModel.TemplateName)
                        {
                            return Json(new { code = -2, message = "当前模板为默认模板，请先设置其它模板为默认模板，再禁用此模板" });
                        }
                        msg = "禁用";
                        break;
                }
                Container.Instance.Resolve<ThemeTemplateService>().Edit(dbModel);

                return Json(new { code = 1, message = msg + "成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "切换失败" });
            }
        }
        #endregion

        #region 设置为默认模板
        public JsonResult SetDefault(int id)
        {
            try
            {
                bool isExist = Container.Instance.Resolve<ThemeTemplateService>().Exist(id);
                if (!isExist)
                {
                    return Json(new { code = -1, message = "模板不存在，或未安装" });
                }

                ThemeTemplate dbModel = Container.Instance.Resolve<ThemeTemplateService>().GetEntity(id);
                if (dbModel.IsOpen == 0)
                {
                    return Json(new { code = -2, message = "模板未启用，请先启用再设置为默认模板" });
                }

                Container.Instance.Resolve<SettingService>().Set("DefaultTemplateName", dbModel.TemplateName);

                return Json(new { code = 1, message = dbModel.Title + " 成功设置为默认模板" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "设置为默认模板失败" });
            }
        }
        #endregion

        #region 选择模板
        public JsonResult SelectTemplate(int id)
        {
            try
            {
                CurrentAccountModel currentAccount = AccountManager.GetCurrentAccount();
                if (currentAccount.IsGuest)
                {
                    return Json(new { code = -2, message = "未登录状态不允许切换模板" });
                }
                bool isExist = Container.Instance.Resolve<ThemeTemplateService>().Exist(id);
                if (!isExist)
                {
                    return Json(new { code = -3, message = "切换模板失败, 不存在此模板" });
                }
                ThemeTemplate dbModel = Container.Instance.Resolve<ThemeTemplateService>().GetEntity(id);
                if (dbModel.IsOpen == 0)
                {
                    return Json(new { code = -4, message = "切换模板失败，此模板被禁用" });
                }

                currentAccount.UserInfo.TemplateName = dbModel.TemplateName;
                Container.Instance.Resolve<UserInfoService>().Edit(currentAccount.UserInfo);

                return Json(new { code = 1, message = "切换模板成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "切换模板失败" });
            }
        }
        #endregion


        #region Helpers

        #region 安装本地主题模板
        /// <summary>
        /// 安装本地主题模板
        /// </summary>
        /// <param name="templateServerPath">eg. ~/Upload/TemplateInstallZip/Red.zip</param>
        /// <returns></returns>
        private bool InstallLocationTemplate(string templateName)
        {
            string serverPath = "~/Upload/TemplateInstallZip/" + templateName + ".zip";
            string fullPhysicalPath = System.Web.HttpContext.Current.Server.MapPath(serverPath);

            try
            {
                // 解压上传的模板包 到 视图地 ~/Templates
                UnZipTemplate(fullPhysicalPath);
                // 将模板信息 插入数据库，默认安装完后为启用
                Container.Instance.Resolve<ThemeTemplateService>().Create(new ThemeTemplate
                {
                    TemplateName = templateName,
                    Title = templateName,
                    // 默认安装后 启用
                    IsOpen = 1,
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 解压主题模板
        private void UnZipTemplate(string templateZipPath)
        {
            string sourcePath = templateZipPath;
            string targetPath = Server.MapPath("~/Templates/");

            SharpZip.DecomparessFile(sourcePath, targetPath);
        }
        #endregion

        #region 上传文件json结果
        sealed class FileResult
        {
            public string fileName { get; set; }
        }
        #endregion 

        #endregion
    }
}
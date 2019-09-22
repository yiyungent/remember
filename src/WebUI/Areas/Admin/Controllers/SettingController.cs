using Core;
using Framework.Extensions;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Areas.Admin.Models.SettingVM;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {
        #region Ctor
        public SettingController()
        {
            ViewBag.PageHeader = "网站设置";
            ViewBag.PageHeaderDescription = "网站设置";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("系统管理"),
            };
        }
        #endregion

        #region 常规设置
        [HttpGet]
        public ViewResult Index()
        {
            SettingViewModel viewModel = new SettingViewModel();

            viewModel.WebUISite = WebSetting.Get("WebUISite");
            viewModel.WebUITitle = WebSetting.Get("WebUITitle");
            viewModel.WebUIDesc = WebSetting.Get("WebUIDesc");
            viewModel.WebUIKeywords = WebSetting.Get("WebUIKeywords");
            viewModel.WebUIStat = WebSetting.Get("WebUIStat");

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Index(SettingViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {

                    #region 数据有效效验

                    #endregion

                    try
                    {
                        WebSetting.Set("WebUISite", inputModel.WebUISite);
                        WebSetting.Set("WebUITitle", inputModel.WebUITitle);
                        WebSetting.Set("WebUIDesc", inputModel.WebUIDesc);
                        WebSetting.Set("WebUIKeywords", inputModel.WebUIKeywords);
                        WebSetting.Set("WebUIStat", inputModel.WebUIStat);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { code = -1, message = "保存失败" });
                    }

                    return Json(new { code = 1, message = "保存成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "保存失败" });
            }
        }
        #endregion

        #region WebAPI设置
        [HttpGet]
        public ViewResult WebApi()
        {
            SettingViewModel viewModel = new SettingViewModel();

            viewModel.WebApiSite = WebSetting.Get("WebApiSite");
            viewModel.WebApiTitle = WebSetting.Get("WebApiTitle");
            viewModel.WebApiDesc = WebSetting.Get("WebApiDesc");
            viewModel.WebApiKeywords = WebSetting.Get("WebApiKeywords");
            viewModel.WebApiStat = WebSetting.Get("WebApiStat");

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult WebApi(SettingViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {

                    #region 数据有效效验

                    #endregion

                    try
                    {
                        WebSetting.Set("WebApiSite", inputModel.WebApiSite);
                        WebSetting.Set("WebApiTitle", inputModel.WebApiTitle);
                        WebSetting.Set("WebApiDesc", inputModel.WebApiDesc);
                        WebSetting.Set("WebApiKeywords", inputModel.WebApiKeywords);
                        WebSetting.Set("WebApiStat", inputModel.WebApiStat);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { code = -1, message = "保存失败" });
                    }

                    return Json(new { code = 1, message = "保存成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "保存失败" });
            }
        }
        #endregion

        #region 高级设置
        [HttpGet]
        public ViewResult Advanced()
        {
            SettingViewModel viewModel = new SettingViewModel();

            viewModel.EnableRedisSession = Convert.ToInt32(WebSetting.Get("EnableRedisSession")) == 1;
            viewModel.EnableLog = Convert.ToInt32(WebSetting.Get("EnableLog")) == 1;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Advanced(SettingViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {

                    #region 数据有效效验

                    #endregion

                    try
                    {
                        WebSetting.Set("EnableRedisSession", inputModel.EnableRedisSession ? "1" : "0");
                        WebSetting.Set("EnableLog", inputModel.EnableLog ? "1" : "0");
                    }
                    catch (Exception ex)
                    {
                        return Json(new { code = -1, message = "保存失败" });
                    }

                    return Json(new { code = 1, message = "保存成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "保存失败" });
            }
        }
        #endregion

        #region Helpers

        #endregion
    }
}
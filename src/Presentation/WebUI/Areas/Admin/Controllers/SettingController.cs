using Core;
using Framework.Extensions;
using Services.Interface;
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
        #region Fields
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public SettingController(ISettingService settingService)
        {
            //ViewBag.PageHeader = "网站设置";
            //ViewBag.PageHeaderDescription = "网站设置";
            //ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            //{
            //    new BreadcrumbItem("系统管理"),
            //};

            this._settingService = settingService;
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

        #region 系统邮箱
        [HttpGet]
        public ViewResult SysEmail()
        {
            SettingViewModel viewModel = new SettingViewModel();

            viewModel.MailUserName = WebSetting.Get("MailUserName");
            viewModel.MailDisplayAddress = WebSetting.Get("MailDisplayAddress");
            viewModel.MailDisplayName = WebSetting.Get("MailDisplayName");
            viewModel.SmtpHost = WebSetting.Get("SmtpHost");
            viewModel.SmtpPort = Convert.ToInt32(WebSetting.Get("SmtpPort"));
            viewModel.SmtpEnableSsl = WebSetting.Get("SmtpEnableSsl") == "1";

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult SysEmail(SettingViewModel inputModel)
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
                        WebSetting.Set("MailUserName", inputModel.MailUserName);
                        WebSetting.Set("MailDisplayAddress", inputModel.MailDisplayAddress);
                        WebSetting.Set("MailDisplayName", inputModel.MailDisplayName);
                        WebSetting.Set("SmtpHost", inputModel.SmtpHost);
                        WebSetting.Set("SmtpPort", inputModel.SmtpPort.ToString());

                        WebSetting.Set("SmtpEnableSsl", inputModel.SmtpEnableSsl ? "1" : "0");

                        if (!string.IsNullOrEmpty(inputModel.MailPassword))
                        {
                            WebSetting.Set("MailPassword", inputModel.MailPassword);
                        }

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

        #region 找回密码
        [HttpGet]
        public ViewResult FindPwd()
        {
            SettingViewModel viewModel = new SettingViewModel();

            viewModel.FindPwd_MailSubject = WebSetting.Get("FindPwd_MailSubject");
            viewModel.FindPwd_MailContent = WebSetting.Get("FindPwd_MailContent");

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult FindPwd(SettingViewModel inputModel)
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
                        WebSetting.Set("FindPwd_MailSubject", inputModel.FindPwd_MailSubject);
                        WebSetting.Set("FindPwd_MailContent", inputModel.FindPwd_MailContent);
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
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

            viewModel.WebName = WebSetting.Get("Web.Name");
            viewModel.WebUIStat = WebSetting.Get("WebUI.Stat");

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
                        WebSetting.Set("Web.Name", inputModel.WebName);
                        WebSetting.Set("WebUI.Stat", inputModel.WebUIStat);
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

            viewModel.CorsWhiteList = WebSetting.Get("CorsWhiteList");

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
                        WebSetting.Set("CorsWhiteList", inputModel.CorsWhiteList);
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

            viewModel.MailUserName = WebSetting.Get("Mail.UserName");
            viewModel.MailDisplayAddress = WebSetting.Get("Mail.DisplayAddress");
            viewModel.MailDisplayName = WebSetting.Get("Mail.DisplayName");
            viewModel.SmtpHost = WebSetting.Get("Smtp.Host");
            viewModel.SmtpPort = Convert.ToInt32(WebSetting.Get("Smtp.Port"));
            viewModel.SmtpEnableSsl = WebSetting.Get("Smtp.EnableSsl") == "1";

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
                        WebSetting.Set("Mail.UserName", inputModel.MailUserName);
                        WebSetting.Set("Mail.DisplayAddress", inputModel.MailDisplayAddress);
                        WebSetting.Set("Mail.DisplayName", inputModel.MailDisplayName);
                        WebSetting.Set("Smtp.Host", inputModel.SmtpHost);
                        WebSetting.Set("Smtp.Port", inputModel.SmtpPort.ToString());

                        WebSetting.Set("Smtp.EnableSsl", inputModel.SmtpEnableSsl ? "1" : "0");

                        if (!string.IsNullOrEmpty(inputModel.MailPassword))
                        {
                            WebSetting.Set("Mail.Password", inputModel.MailPassword);
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

            viewModel.FindPwd_MailSubject = WebSetting.Get("FindPwd.Mail.Subject");
            viewModel.FindPwd_MailContent = WebSetting.Get("FindPwd.Mail.Content");

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
                        WebSetting.Set("FindPwd.Mail.Subject", inputModel.FindPwd_MailSubject);
                        WebSetting.Set("FindPwd.Mail.Content", inputModel.FindPwd_MailContent);
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

            viewModel.LogEnable = Convert.ToInt32(WebSetting.Get("Log.Enable")) == 1;

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
                        WebSetting.Set("Log.Enable", inputModel.LogEnable ? "1" : "0");
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
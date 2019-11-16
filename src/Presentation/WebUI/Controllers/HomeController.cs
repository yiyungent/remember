using Core;
using Core.Common;
using Domain;
using Domain.Entities;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using Framework.Mvc.ViewEngines.Templates;
using Newtonsoft.Json;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Models.LogVM;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly ICourseBoxService _courseBoxService;
        private readonly ILogInfoService _logInfoService;
        #endregion

        #region Ctor
        public HomeController(ICourseBoxService courseBoxService, ILogInfoService logInfoService)
        {
            this._courseBoxService = courseBoxService;
            this._logInfoService = logInfoService;
        }
        #endregion

        #region 首页
        public ActionResult Index()
        {
            IList<CourseBox> courseBoxes = this._courseBoxService.Filter(m => !m.IsDeleted).ToList();
            ViewBag.CourseBoxes = courseBoxes;

            return View();
        }
        #endregion


        #region 写入访问日志
        public JsonResult Log(AccessLogInputModel inputModel)
        {
            JsonResult responseData = null;
            try
            {
                UserAgentModel userAgent = JsonConvert.DeserializeObject<UserAgentModel>(inputModel.UserAgent);
                int accessUserId = 0;
                try
                {
                    accessUserId = AccountManager.GetCurrentAccount().UserId;
                }
                catch (Exception ex)
                {
                }

                // 优先从 参数中获取 IdCode
                if (string.IsNullOrEmpty(inputModel.IdCode))
                {
                    inputModel.IdCode = Request.Cookies["IdCode"].Value;
                }

                this._logInfoService.Create(new LogInfo
                {
                    IdCode = inputModel.IdCode,
                    VisitorInfo = inputModel.VisitorInfo,
                    ClickCount = inputModel.ClickCount,
                    AccessIp = inputModel.Ip,
                    AccessCity = inputModel.City,
                    AccessTime = inputModel.AccessTime.ToDateTime13(),
                    JumpTime = inputModel.JumpTime.ToDateTime13(),
                    CreateTime = DateTime.Now,
                    UserAgent = inputModel?.UserAgent,
                    AccessUrl = inputModel?.AccessUrl,
                    RefererUrl = inputModel?.RefererUrl,
                    AccessUserId = accessUserId,
                    Browser = userAgent?.Browser?.Name + " " + userAgent?.Browser?.Version,
                    BrowserEngine = userAgent?.Engine?.Name,
                    Device = userAgent?.Device?.Model,
                    Cpu = userAgent?.Cpu?.Architecture,
                    OS = userAgent?.OS?.Name + " " + userAgent?.OS?.Version,
                    Duration = (int)(inputModel.JumpTime - inputModel.AccessTime) / 1000
                });

                responseData = Json(new { code = 1, message = "写入访问日志成功" });
            }
            catch (Exception ex)
            {
                responseData = Json(new
                {
                    code = -1,
                    message = "写入访问日志失败: " + ex.Message + ex.InnerException?.Message
                });
            }

            return responseData;
        }
        #endregion

    }
}
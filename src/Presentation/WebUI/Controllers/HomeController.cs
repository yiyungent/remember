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
using WebUI.Areas.Admin.Models.Common;
using WebUI.Models.LogVM;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly IArticleService _articleService;
        private readonly ILogInfoService _logInfoService;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public HomeController(IArticleService articleService, ILogInfoService logInfoService, ISettingService settingService)
        {
            this._articleService = articleService;
            this._logInfoService = logInfoService;
            this._settingService = settingService;
        }
        #endregion

        #region 首页
        public ActionResult Index(string cat = "all", int pageIndex = 1)
        {
            int pageSize = 6;
            pageIndex = pageIndex >= 1 ? pageIndex : 1;
            Query(pageIndex, pageSize, out IList<Article> list, out int totalCount);

            ListViewModel<Article> articles = new ListViewModel<Article>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            string webName = _settingService.GetSet("Web.Name");
            // SEO
            string homeTitle = _settingService.GetSet("SEO.Home.Index.Title");
            string homeKeywords = _settingService.GetSet("SEO.Home.Index.Keywords");
            string homeDesc = _settingService.GetSet("SEO.Home.Index.Desc");

            ViewBag.ArticleVM = articles;
            ViewBag.WebName = webName;
            ViewBag.Title = homeTitle.Replace("{{Web.Name}}", webName);
            ViewBag.Keywords = homeKeywords.Replace("{{Web.Name}}", webName);
            ViewBag.Desc = homeDesc.Replace("{{Web.Name}}", webName);

            return View();
        }

        private void Query(int pageIndex, int pageSize, out IList<Article> list, out int totalCount)
        {
            // 输入的查询关键词
            string query = Request["q"]?.Trim() ?? "";
            // 查询类型
            QueryType queryType = new QueryType();
            queryType.Val = Request["type"]?.Trim() ?? "title";
            switch (queryType.Val.ToLower())
            {
                case "title":
                    queryType.Text = "标题";
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Title.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    if (int.TryParse(query, out int userId))
                    {
                        list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == userId && !m.IsDeleted, m => m.ID, false).ToList();
                    }
                    else if (string.IsNullOrEmpty(query))
                    {
                        list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => !m.IsDeleted, m => m.ID, false).ToList();
                    }
                    else
                    {
                        list = new List<Article>();
                        totalCount = 0;
                    }
                    break;
                default:
                    queryType.Text = "标题";
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Title.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
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
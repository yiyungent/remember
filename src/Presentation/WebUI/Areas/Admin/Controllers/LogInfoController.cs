using Core;
using Domain;
using Domain.Entities;
using Newtonsoft.Json;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Areas.Admin.Models.LogInfoVM;

namespace WebUI.Areas.Admin.Controllers
{
    public class LogInfoController : Controller
    {
        #region Fields
        private readonly ILogInfoService _logInfoService;
        private readonly IUserInfoService _userInfoService;
        #endregion

        #region Ctor
        public LogInfoController(ILogInfoService logInfoService, IUserInfoService userInfoService)
        {
            this._logInfoService = logInfoService;
            this._userInfoService = userInfoService;
        }
        #endregion

        #region 列表
        public ActionResult Index(int pageIndex = 1, int pageSize = 6)
        {
            Query(pageIndex, pageSize, out IList<LogInfo> list, out int totalCount);

            ListViewModel<LogInfo> viewModel = new ListViewModel<LogInfo>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(int pageIndex, int pageSize, out IList<LogInfo> list, out int totalCount)
        {
            // 输入的查询关键词
            string query = Request["q"]?.Trim() ?? "";
            // 查询类型
            QueryType queryType = new QueryType();
            queryType.Val = Request["type"]?.Trim() ?? "ip";
            switch (queryType.Val.ToLower())
            {
                case "uid":
                    queryType.Text = "用户ID";
                    int userId = int.Parse(query);
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessUserId == userId, m => m.ID, false).ToList();
                    break;
                case "username":
                    queryType.Text = "用户名";
                    userId = this._userInfoService.Find(m => m.UserName.Contains(query) && !m.IsDeleted)?.ID ?? -1;
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessUserId == userId, m => m.ID, false).ToList();
                    break;
                case "browser":
                    queryType.Text = "浏览器";
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Browser.Contains(query), m => m.ID, false).ToList();
                    break;
                case "ip":
                    queryType.Text = "IP";
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessIp.Contains(query), m => m.ID, false).ToList();
                    break;
                case "city":
                    queryType.Text = "城市";
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessCity.Contains(query), m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    int logId = int.Parse(query);
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == logId, m => m.ID, false).ToList();
                    break;
                default:
                    queryType.Text = "IP";
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessIp.Contains(query), m => m.ID, false).ToList();
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 查看
        public ViewResult Details(int id)
        {
            LogInfo viewModel = this._logInfoService.Find(m => m.ID == id);
            VisitorInfoViewModel visitorInfo = JsonConvert.DeserializeObject<VisitorInfoViewModel>(viewModel.VisitorInfo);
            if (visitorInfo == null)
            {
                visitorInfo = new VisitorInfoViewModel();
            }
            if (visitorInfo.Screen == null)
            {
                visitorInfo.Screen = new VisitorInfoViewModel.ScreenModel { Width = 0, Height = 0 };
            }
            ViewBag.VisitorInfo = visitorInfo;

            return View(viewModel);
        }
        #endregion

        // TODO: 点击记录操作 - 屏蔽IP，屏蔽用户

    }
}
using Core;
using Domain;
using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;

namespace WebUI.Areas.Admin.Controllers
{
    public class LogInfoController : Controller
    {
        #region Fields
        private readonly ILogInfoService _logInfoService;
        #endregion

        #region Ctor
        public LogInfoController(ILogInfoService logInfoService)
        {
            this._logInfoService = logInfoService;
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
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessUserId == int.Parse(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "browser":
                    queryType.Text = "浏览器";
                    //queryConditions.Add(Expression.Like("Browser", query, MatchMode.Anywhere));
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Browser.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "ip":
                    queryType.Text = "IP";
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessIp.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == int.Parse(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                default:
                    queryType.Text = "IP";
                    list = this._logInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AccessIp.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 删除
        public JsonResult Delete(int id)
        {
            try
            {
                // TODO: 无法删除访问日志
                var dbModel = this._logInfoService.Find(m => m.ID == id && !m.IsDeleted);
                dbModel.IsDeleted = true;
                dbModel.DeletedAt = DateTime.Now;
                this._logInfoService.Update(dbModel);

                return Json(new { code = 1, message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "删除失败" });
            }
        }
        #endregion
    }
}
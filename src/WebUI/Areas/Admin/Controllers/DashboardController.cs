using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.DashboardVM;

namespace WebUI.Areas.Admin.Controllers
{
    /// <summary>
    /// 仪表盘
    /// </summary>
    public class DashboardController : Controller
    {
        #region Ctor
        public DashboardController()
        {
            ViewBag.PageHeader = "评价分析";
            ViewBag.PageHeaderDescription = "评价分析";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("仪表盘"),
                new BreadcrumbItem("评价分析"),
            };
        }
        #endregion

    }
}
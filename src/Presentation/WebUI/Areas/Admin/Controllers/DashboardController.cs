using Core;
using Domain;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Areas.Admin.Models.DashboardVM;

namespace WebUI.Areas.Admin.Controllers
{
    /// <summary>
    /// 仪表盘
    /// </summary>
    public class DashboardController : Controller
    {
        #region Fields
        private readonly ILogInfoService _logInfoService;
        #endregion

        #region Ctor
        public DashboardController(ILogInfoService logInfoService)
        {
            this._logInfoService = logInfoService;
        }
        #endregion

        #region 仪表盘1
        public ViewResult Index()
        {
            DashboardOneViewModel viewModel = new DashboardOneViewModel();
            viewModel.PV = 1;
            viewModel.UV = 2;
            viewModel.JumpRate = 23;
            viewModel.NewUserReg = 213;

            return View(viewModel);
        }
        #endregion

        #region 仪表盘2
        public ViewResult IndexTwo()
        {
            return View();
        }
        #endregion

    }
}
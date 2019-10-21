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
        private readonly IUserInfoService _userInfoService;
        #endregion

        #region Ctor
        public DashboardController(ILogInfoService logInfoService, IUserInfoService userInfoService)
        {
            this._logInfoService = logInfoService;
            this._userInfoService = userInfoService;
        }
        #endregion

        #region 仪表盘1
        public ViewResult Index()
        {
            DashboardOneViewModel viewModel = new DashboardOneViewModel();
            int todayPV = 0, todayUV = 0, todayJumpRate = 0, todayNewUserReg = 0;
            DateTime now = DateTime.Now;
            string today = now.ToString("yyyy-MM-dd");
            // TODO: 发现这样写 linq ，EF仍然无法翻译成SQL，所以待转为手写SQL，或则推荐使用long类型存13位时间戳
            // 计算PV：当天访问量
            //todayPV = this._logInfoService.Filter(m => !m.IsDeleted).Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).Count();
            todayPV = this._logInfoService.Filter(m => !m.IsDeleted).ToList().Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).Count();
            // TODO: UV 本来算 cookie，但这里简化，直接算 IP
            //todayUV = this._logInfoService.Filter(m => !m.IsDeleted).Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).GroupBy(m => m.AccessIp).Count();
            todayUV = this._logInfoService.Filter(m => !m.IsDeleted).ToList().Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).GroupBy(m => m.AccessIp).Count();
            //todayNewUserReg = this._userInfoService.All().Where(m => m.CreateTime.ToString("yyyy-MM-dd") == today).Count();
            todayNewUserReg = this._userInfoService.All().ToList().Where(m => m.CreateTime.ToString("yyyy-MM-dd") == today).Count();
            // TODO: 当天跳出率计算

            viewModel.PV = todayPV;
            viewModel.UV = todayUV;
            viewModel.JumpRate = todayJumpRate;
            viewModel.NewUserReg = todayNewUserReg;

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
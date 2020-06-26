using Core;
using Domain;
using Domain.Entities;
using Framework.Attributes;
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
            return View();
        }
        #endregion

        #region 仪表盘2
        public ViewResult IndexTwo()
        {
            IndexTwoViewModel viewModel = new IndexTwoViewModel();
            viewModel.ServerName = Server.MachineName.ToString();
            viewModel.ServerIp = Request.ServerVariables["LOCAL_ADDR"];
            viewModel.ServerOutTime = Server.ScriptTimeout.ToString() + "秒";
            viewModel.NetVer = System.Environment.Version.ToString();
            viewModel.IISVer = Request.ServerVariables["SERVER_SOFTWARE"];
            viewModel.CpuNum = int.Parse(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));

            return View(viewModel);
        }
        #endregion

        #region PV,UV,跳出率,新注册
        [AuthKey("Admin.Dashboard.Index")]
        public JsonResult PvUv()
        {
            PvUvViewModel viewModel = new PvUvViewModel();
            try
            {
                int todayPV = 0, todayUV = 0, todayJumpRate = 0, todayNewUserReg = 0;
                DateTime now = DateTime.Now;
                string today = now.ToString("yyyy-MM-dd");
                // TODO: 发现这样写 linq ，EF仍然无法翻译成SQL，所以待转为手写SQL，或则推荐使用long类型存13位时间戳
                // 计算PV：当天访问量
                //todayPV = this._logInfoService.Filter(m => !m.IsDeleted).Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).Count();
                todayPV = this._logInfoService.All().Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).Count();
                //todayUV = this._logInfoService.Filter(m => !m.IsDeleted).Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).GroupBy(m => m.IdCode).Count();
                todayUV = this._logInfoService.All().Where(m => m.AccessTime.ToString("yyyy-MM-dd") == today).GroupBy(m => m.IdCode).Count();
                //todayNewUserReg = this._userInfoService.All().Where(m => m.CreateTime.ToString("yyyy-MM-dd") == today).Count();
                todayNewUserReg = this._userInfoService.All().ToList().Where(m => m.CreateTime.ToString("yyyy-MM-dd") == today).Count();
                // TODO: 当天跳出率计算

                viewModel.PV = todayPV;
                viewModel.UV = todayUV;
                viewModel.jumpRate = todayJumpRate;
                viewModel.newUserReg = todayNewUserReg;
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "获取PV，UV失败" });
            }


            return Json(new { code = 1, message = "获取PV, UV 成功", data = viewModel }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 访问情况
        [AuthKey("Admin.Dashboard.Index")]
        public JsonResult AccessLog()
        {
            AccessLogViewModel viewModel = new AccessLogViewModel();
            try
            {
                DateTime now = DateTime.Now;

                viewModel.title = new Title() { text = "最近七天访问趋势图" };
                viewModel.legend = new Legend() { data = new string[] { "PV", "UV" } };
                viewModel.tooltip = new Tooltip();
                viewModel.tooltip.trigger = "axis";
                viewModel.tooltip.axisPointer = new Axispointer
                {
                    type = "cross",
                    label = new Label
                    {
                        backgroundColor = "#6a7985"
                    }
                };
                viewModel.toolbox = new Toolbox
                {
                    feature = new Feature
                    {
                        saveAsImage = new Saveasimage()
                    }
                };
                viewModel.grid = new Grid
                {
                    left = "3%",
                    right = "4%",
                    bottom = "3%",
                    containLabel = true
                };

                viewModel.xAxis = new Xaxi[1];
                viewModel.xAxis[0] = new Xaxi();
                viewModel.xAxis[0].type = "category";
                viewModel.xAxis[0].boundaryGap = false;
                viewModel.xAxis[0].data = new string[7] { now.AddDays(-6).ToString("yyyy-MM-dd"), now.AddDays(-5).ToString("yyyy-MM-dd"), now.AddDays(-4).ToString("yyyy-MM-dd"), now.AddDays(-3).ToString("yyyy-MM-dd"), now.AddDays(-2).ToString("yyyy-MM-dd"), now.AddDays(-1).ToString("yyyy-MM-dd"), now.ToString("yyyy-MM-dd") };
                viewModel.yAxis = new Yaxi[1];
                viewModel.yAxis[0] = new Yaxi();
                viewModel.yAxis[0].type = "value";

                IList<Series> series = new List<Series>();
                IList<LogInfo> logInfos = this._logInfoService.All().ToList();

                // PV, UV
                IList<int> pvList = new List<int>();
                IList<int> uvList = new List<int>();
                for (int i = 6; i >= 0; i--)
                {
                    // 注意添加顺序
                    int pvCount = logInfos.Where(m => m.AccessTime.ToString("yyyy-MM-dd") == now.AddDays(-i).ToString("yyyy-MM-dd")).Count();
                    int uvCount = logInfos.Where(m => m.AccessTime.ToString("yyyy-MM-dd") == now.AddDays(-i).ToString("yyyy-MM-dd")).GroupBy(m => m.AccessIp).Count();

                    pvList.Add(pvCount);
                    uvList.Add(uvCount);
                }

                series.Add(new Series
                {
                    name = "PV",
                    type = "line",
                    stack = "总量",
                    areaStyle = new Areastyle(),
                    data = pvList.ToArray()
                });
                series.Add(new Series
                {
                    name = "UV",
                    type = "line",
                    stack = "总量",
                    areaStyle = new Areastyle(),
                    data = uvList.ToArray()
                });
                viewModel.series = series.ToArray();

            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "获取 访问情况 失败" });
            }


            return Json(new { code = 1, message = "获取 访问情况 成功", data = viewModel }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CPU，内存等实时使用率
        [AuthKey("Admin.Dashboard.IndexTwo")]
        public JsonResult CpuMemory()
        {
            CpuMemoryViewModel viewModel = new CpuMemoryViewModel();
            try
            {
                viewModel.serverSessionTotal = Session.Contents.Count;
                viewModel.serverRunTime = (Math.Round((double)Environment.TickCount / 600 / 60) / 100).ToString() + "小时";
                viewModel.cpu = ((TimeSpan)System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime).TotalSeconds.ToString("N0");
                viewModel.memory = ((Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2");
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "获取 CPU，内存等实时使用率 失败" });
            }

            return Json(new { code = 1, message = "获取 CPU，内存等实时使用率 成功", data = viewModel }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
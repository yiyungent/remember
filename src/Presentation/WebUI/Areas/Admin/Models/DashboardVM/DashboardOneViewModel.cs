using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.DashboardVM
{
    public class DashboardOneViewModel
    {
        /// <summary>
        /// 独立访客量
        /// </summary>
        public int UV { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int PV { get; set; }

        /// <summary>
        /// 跳出率
        /// </summary>
        public int JumpRate { get; set; }

        /// <summary>
        /// 新用户注册量
        /// 今日新注册量
        /// </summary>
        public int NewUserReg { get; set; }

    }
}
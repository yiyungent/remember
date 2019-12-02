using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.DashboardVM
{
    public class CpuMemoryViewModel
    {
        /// <summary>
        /// ASP.NET所占CPU
        /// %
        /// </summary>
        public string cpu { get; set; }

        /// <summary>
        /// ASP.NET所占内存
        /// M
        /// </summary>
        public string memory { get; set; }

        /// <summary>
        /// Session总数
        /// </summary>
        public int serverSessionTotal { get; set; }

        /// <summary>
        /// 运行时间
        /// </summary>
        public string serverRunTime { get; set; }
    }
}
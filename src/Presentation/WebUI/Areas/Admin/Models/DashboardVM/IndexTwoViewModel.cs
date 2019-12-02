using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.DashboardVM
{
    public class IndexTwoViewModel
    {
        /// <summary>
        /// 操作系统
        /// </summary>
        public string ServerVer
        {
            get
            {
                return Environment.OSVersion.ToString();
            }
        }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// 服务端脚本执行超时
        /// </summary>
        public string ServerOutTime { get; set; }

        /// <summary>
        /// Application总数
        /// </summary>
        public int ServerApplicationTotal { get; set; }

        /// <summary>
        /// IIS版本
        /// </summary>
        public string IISVer { get; set; }

        /// <summary>
        /// .NET Framework 版本
        /// </summary>
        public string NetVer { get; set; }

        /// <summary>
        /// CPU 数量
        /// </summary>
        public int CpuNum { get; set; }

        
    }
}
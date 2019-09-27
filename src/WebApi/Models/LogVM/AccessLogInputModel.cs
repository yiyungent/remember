using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.LogVM
{
    public class AccessLogInputModel
    {
        /// <summary>
        /// 访问者的IP
        /// </summary>
        public string AccessIp { get; set; }

        /// <summary>
        /// 访问时间：进入网页，加载完的时间
        /// </summary>
        public string AccessTime { get; set; }

        /// <summary>
        /// 跳出网页时间
        /// </summary>
        public string JumpTime { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string AccessUrl { get; set; }

        /// <summary>
        /// 来源URL
        /// </summary>
        public string RefererUrl { get; set; }

    }
}
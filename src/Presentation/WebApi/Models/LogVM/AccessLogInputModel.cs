using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.LogVM
{
    public class AccessLogInputModel
    {
        /// <summary>
        /// 访客识别码
        /// </summary>
        public string IdCode { get; set; }

        /// <summary>
        /// 访问者的IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 访问者的城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 访问时间：进入网页，加载完的时间
        /// js 13位毫秒时间戳
        /// </summary>
        public long AccessTime { get; set; }

        /// <summary>
        /// 跳出网页时间
        /// js 13位毫秒时间戳
        /// </summary>
        public long JumpTime { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string AccessUrl { get; set; }

        /// <summary>
        /// 来源URL
        /// </summary>
        public string RefererUrl { get; set; }

        /// <summary>
        /// UserAgent
        /// json字符串
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 访客信息
        /// json字符串
        /// </summary>
        public string VisitorInfo { get; set; }

        /// <summary>
        /// 页面点击次数
        /// </summary>
        public int ClickCount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Models
{
    public class ErrorRedirectViewModel
    {
        /// <summary>
        /// 错误标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 跳转到目标地址
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 跳转到目标地址的站点名
        /// </summary>
        public string RedirectUrlName { get; set; }

        /// <summary>
        /// 等待跳转秒数
        /// </summary>
        public int WaitSecond { get; set; }
    }
}
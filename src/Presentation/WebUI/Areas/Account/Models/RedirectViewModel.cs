using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Account.Models
{
    public class RedirectViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 跳转到目标地址
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 等待跳转秒数
        /// </summary>
        public int WaitSecond { get; set; }
    }
}
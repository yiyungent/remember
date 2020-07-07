using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Admin.Models.SettingVM
{
    public class SettingViewModel
    {
        public string WebName { get; set; }

        /// <summary>
        /// 网站底部
        /// </summary>
        [AllowHtml]
        public string WebFooter { get; set; }

        public bool LogEnable { get; set; }

        /// <summary>
        /// 跨域白名单
        /// </summary>
        public string CorsWhiteList { get; set; }

        #region 系统邮箱

        /// <summary>
        /// 邮箱（登录用户名）
        /// </summary>
        public string MailUserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string MailPassword { get; set; }

        public string MailDisplayAddress { get; set; }

        public string MailDisplayName { get; set; }

        /// <summary>
        /// SMTP Server
        /// </summary>
        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public bool SmtpEnableSsl { get; set; }

        /// <summary>
        /// 找回密码-邮件主题模板
        /// </summary>
        [AllowHtml]
        public string FindPwd_MailSubject { get; set; }

        /// <summary>
        /// 找回密码-邮件内容模板
        /// </summary>
        [AllowHtml]
        public string FindPwd_MailContent { get; set; }

        #endregion
    }
}
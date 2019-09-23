using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Admin.Models.SettingVM
{
    public class SettingViewModel
    {
        public string WebUISite { get; set; }

        public string WebUITitle { get; set; }

        public string WebUIDesc { get; set; }

        public string WebUIKeywords { get; set; }

        public string WebApiSite { get; set; }

        public string WebApiTitle { get; set; }

        public string WebApiDesc { get; set; }

        public string WebApiKeywords { get; set; }

        /// <summary>
        /// 网站底部统计代码
        /// </summary>
        [AllowHtml]
        public string WebUIStat { get; set; }

        public string WebApiStat { get; set; }

        public bool EnableRedisSession { get; set; }

        public bool EnableLog { get; set; }

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
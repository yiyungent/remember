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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}
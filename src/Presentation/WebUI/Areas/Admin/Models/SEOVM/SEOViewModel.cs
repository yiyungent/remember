using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace WebUI.Areas.Admin.Models.SEOVM
{
    public class SEOViewModel
    {
        public string HomeTitle { get; set; }

        public string HomeDesc { get; set; }

        public string HomeKeywords { get; set; }

        public string ArticleTitle { get; set; }

        public string ArticleDesc { get; set; }

        public string ArticleKeywords { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Admin.Models.Common
{
    public class BreadcrumbItem
    {
        public string Link { get; set; }

        public string Text { get; set; }

        public BreadcrumbItem(string text)
        {
            this.Text = text;
        }

        public BreadcrumbItem(string text, string link)
        {
            this.Text = text;
            this.Link = link;
        }

        public MvcHtmlString GenerateHtmlTag()
        {
            MvcHtmlString htmlTag = null;
            StringBuilder sbHtmlTag = new StringBuilder();
            if (this.Link == null)
            {
                sbHtmlTag.AppendFormat("<li class=\"active\">{0}</li>", this.Text);
            }
            else
            {
                sbHtmlTag.AppendFormat("<li class=\"active\"><a href=\"{0}\">{1}</a></li>", this.Link, this.Text);
            }
            htmlTag = new MvcHtmlString(sbHtmlTag.ToString());

            return htmlTag;
        }
    }
}
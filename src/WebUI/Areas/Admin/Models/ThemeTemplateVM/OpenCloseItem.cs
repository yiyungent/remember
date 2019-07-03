using Framework.Mvc.ViewEngines.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.ThemeTemplateVM
{
    public class OpenCloseItem
    {
        public int ID { get; set; }

        public Source Source { get; set; }

        /// <summary>
        /// 服务器相对位置
        /// <para>eg. ~/Upload/Templates/Red.zip</para>
        /// </summary>
        public string ServerPath { get; set; }

        public string TemplateName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IList<string> Authors { get; set; }

        public string Version { get; set; }

        public string Url { get; set; }

        public bool IsDefault { get; set; }

        /// <summary>
        /// 状态
        /// <para>1: 启用</para>
        /// <para>0: 禁用</para>
        /// </summary>
        public int Status { get; set; }

        public string PreviewImageUrl { get; set; }

        public static explicit operator OpenCloseItem(TemplateConfiguration templateConfiguration)
        {
            OpenCloseItem rtn = new OpenCloseItem();
            rtn.TemplateName = templateConfiguration.TemplateName;
            rtn.Title = templateConfiguration.Title;
            rtn.Authors = templateConfiguration.Authors;
            rtn.Description = templateConfiguration.Description;
            rtn.PreviewImageUrl = templateConfiguration.PreviewImageUrl;

            return rtn;
        }
    }
}
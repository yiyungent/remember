using Core;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework.Extensions;
using Domain.Entities;
using Services.Interface;

namespace WebUI.Areas.Admin.Models.Common
{
    public class AdminWebBaseInfoViewModel
    {
        public string Logo_mini { get; set; }
        public string Logo_lg { get; set; }
        public string Copyright { get; set; }

        public Dictionary<int, string> ThemeTemplateDic { get; set; }

        public AdminWebBaseInfoViewModel()
        {
            string title_mini = WebSetting.Get("WebUITitle").Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries)[0];
            string webUiUrl = WebSetting.Get("WebUISite");
            this.Logo_mini = title_mini;
            this.Logo_lg = title_mini;
            this.Copyright = "<strong>Copyright &copy; " + DateTime.Now.Year + $" <a target='_blank' href=\"{webUiUrl}\">{title_mini}</a>.</strong> All rights reserved.";

            IList<ThemeTemplate> themeTemplates = ContainerManager.Resolve<IThemeTemplateService>().Filter(m => m.IsOpen == 1).ToList();
            this.ThemeTemplateDic = new Dictionary<int, string>();
            foreach (var item in themeTemplates)
            {
                this.ThemeTemplateDic.Add(item.ID, item.Title);
            }
        }
    }
}
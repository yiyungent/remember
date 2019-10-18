using Core;
using Domain;
using Domain.Entities;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Extensions
{
    public static class DomainExt
    {
        /// <summary>
        /// 数据库中存的含标记的相对url转换成绝对http url
        /// </summary>
        /// <param name="dbRelativeUrl"></param>
        /// <returns></returns>
        public static string ToHttpAbsoluteUrl(this string dbRelativeUrl)
        {
            string rtnStr = "";
            if (!string.IsNullOrEmpty(dbRelativeUrl))
            {
                ISettingService settingService = ContainerManager.Resolve<ISettingService>();

                string webApiSite = settingService.GetSet("WebApiSite");
                string webUISite = settingService.GetSet("WebUISite");

                rtnStr = dbRelativeUrl.Replace(":WebApiSite:", webApiSite).Replace(":WebUISite:", webUISite);
            }

            return rtnStr;
        }
    }
}

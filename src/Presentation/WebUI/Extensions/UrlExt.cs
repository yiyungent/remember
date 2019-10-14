using Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Extensions
{
    public static class UrlExt
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
                //SettingService settingService = Container.Instance.Resolve<SettingService>();
                //string webApiSite = settingService.Query(new List<ICriterion>
                //{
                //    Expression.Eq("SetKey", "WebApiSite")
                //}).FirstOrDefault()?.SetValue;
                string webApiSite = ContainerManager.Resolve<ISettingService>().GetSet("WebApiSite");
                string webUISite = ContainerManager.Resolve<ISettingService>().GetSet("WebUISite");

                rtnStr = dbRelativeUrl.Replace(":WebApiSite:", webApiSite).Replace(":WebUISite:", webUISite);
            }

            return rtnStr;
        }
    }
}
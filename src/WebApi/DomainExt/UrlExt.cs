using Core;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DomainExt
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
            SettingService settingService = Container.Instance.Resolve<SettingService>();
            string webApiSite = settingService.Query(new List<ICriterion>
            {
                Expression.Eq("SetKey", "WebApiSite")
            }).FirstOrDefault()?.SetValue;
            string webUISite = settingService.Query(new List<ICriterion>
            {
                Expression.Eq("SetKey", "WebUISite")
            }).FirstOrDefault()?.SetValue;

            rtnStr = dbRelativeUrl.Replace(":WebApiSite:", webApiSite).Replace(":WebUISite:", webUISite);

            return rtnStr;
        }
    }
}
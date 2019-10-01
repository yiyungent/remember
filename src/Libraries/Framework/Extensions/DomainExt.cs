using Core;
using Domain;
using Domain.Entities;
using Framework.Factories;
using Framework.Infrastructure.Abstract;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Extensions
{
    public static class DomainExt
    {
        public static IList<FunctionInfo> FunctionInfoList(this Sys_Menu sys_Menu)
        {
            IList<FunctionInfo> rtn = null;
            IDBAccessProvider dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();
            int menuId = sys_Menu.ID;
            rtn = dBAccessProvider.GetFunctionListBySys_MenuId(menuId);

            return rtn;
        }

        /// <summary>
        /// 数据库中存的含标记的相对url转换成绝对http url
        /// </summary>
        /// <param name="dbRelativeUrl"></param>
        /// <returns></returns>
        public static string ToHttpAbsoluteUrl(this string dbRelativeUrl)
        {
            string rtnStr = "";
            // TODO: 数据库操作
            //if (!string.IsNullOrEmpty(dbRelativeUrl))
            //{
            //    SettingService settingService = Container.Instance.Resolve<SettingService>();
            //    string webApiSite = settingService.Query(new List<ICriterion>
            //{
            //    Expression.Eq("SetKey", "WebApiSite")
            //}).FirstOrDefault()?.SetValue;
            //    string webUISite = settingService.Query(new List<ICriterion>
            //{
            //    Expression.Eq("SetKey", "WebUISite")
            //}).FirstOrDefault()?.SetValue;

            //    rtnStr = dbRelativeUrl.Replace(":WebApiSite:", webApiSite).Replace(":WebUISite:", webUISite);
            //}

            return rtnStr;
        }
    }
}

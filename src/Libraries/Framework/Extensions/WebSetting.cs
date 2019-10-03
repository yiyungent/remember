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
    public static class WebSetting
    {
        public static string Get(string key)
        {
            #region 废弃
            //string value = Container.Instance.Resolve<SettingService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("SetKey", key)
            //}).FirstOrDefault()?.SetValue ?? ""; 
            #endregion
            IDBAccessProvider dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();
            string value = dBAccessProvider.GetSet(key);

            return value;
        }

        public static void Set(string key, string value)
        {
            #region 废弃
            //Setting setting = Container.Instance.Resolve<SettingService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("SetKey", key)
            //}).FirstOrDefault();
            //if (setting != null)
            //{
            //    setting.SetValue = value;
            //    Container.Instance.Resolve<SettingService>().Edit(setting);
            //} 
            #endregion
            IDBAccessProvider dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();
            dBAccessProvider.Set(key, value);

        }
    }
}

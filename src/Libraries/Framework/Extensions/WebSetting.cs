using Core;
using Domain;
using Domain.Entities;
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
            //string value = Container.Instance.Resolve<SettingService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("SetKey", key)
            //}).FirstOrDefault()?.SetValue ?? "";
            // TODO: Ioc
            string value = null;

            return value;
        }

        public static void Set(string key, string value)
        {
            // TODO: Ioc
            //Setting setting = Container.Instance.Resolve<SettingService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("SetKey", key)
            //}).FirstOrDefault();
            //if (setting != null)
            //{
            //    setting.SetValue = value;
            //    Container.Instance.Resolve<SettingService>().Edit(setting);
            //}
        }
    }
}

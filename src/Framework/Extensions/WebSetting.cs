using Core;
using Domain;
using NHibernate.Criterion;
using Service;
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
            string value = Container.Instance.Resolve<SettingService>().Query(new List<ICriterion>
            {
                Expression.Eq("SetKey", key)
            }).FirstOrDefault()?.SetValue ?? "";

            return value;
        }

        public static void Set(string key, string value)
        {
            Setting setting = Container.Instance.Resolve<SettingService>().Query(new List<ICriterion>
            {
                Expression.Eq("SetKey", key)
            }).FirstOrDefault();
            if (setting != null)
            {
                setting.SetValue = value;
                Container.Instance.Resolve<SettingService>().Edit(setting);
            }
        }
    }
}

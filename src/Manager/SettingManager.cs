using Domain;
using Manager.Base;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager
{
    public class SettingManager : BaseManager<Setting>
    {
        public string GetSet(string key)
        {
            string value = null;
            try
            {
                value = Query(new List<ICriterion>
                {
                    Expression.Eq("SetKey", key)
                }).FirstOrDefault()?.SetValue ?? "";
            }
            catch (Exception ex)
            { }

            return value;
        }

        public void Set(string key, string value)
        {
            Setting dbModel = null;
            try
            {
                dbModel = Query(new List<ICriterion>
                {
                    Expression.Eq("SetKey", key)
                }).FirstOrDefault();
            }
            catch (Exception ex)
            { }
            if (dbModel == null)
            {
                // 创建
                dbModel = new Setting();
                dbModel.SetKey = key;
                dbModel.SetValue = value;
                Create(dbModel);
            }
            else
            {
                // 更新
                dbModel.SetValue = value;
                Edit(dbModel);
            }
        }
    }
}

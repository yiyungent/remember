using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PluginHub.Configuration;
using PluginHub.Domain.Configuration;
using PluginHub.Services.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace WebUI.Imples
{
    public class CustomSettingImply : ISettingService
    {
        public void ClearCache()
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting(Setting setting)
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting<T>() where T : ISettings, new()
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting<T, TPropType>(T settings) where T : ISettings, new()
        {
            throw new NotImplementedException();
        }

        public IList<Setting> GetAllSettings()
        {
            throw new NotImplementedException();
        }

        public Setting GetSetting(string key)
        {
            throw new NotImplementedException();
        }

        public Setting GetSettingById(int settingId)
        {
            throw new NotImplementedException();
        }

        public T GetSettingByKey<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T LoadSetting<T>() where T : ISettings, new()
        {
            // 提取插件设置名作为 插件表名
            string pluginTableName = typeof(T).Name;
            // 简化--直接使用 json 文件模拟数据库表
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/PluginTables/" + pluginTableName + ".json");
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            string jsonStr = File.ReadAllText(filePath);
            T jsonObj = JsonConvert.DeserializeObject<T>(jsonStr);

            return jsonObj;
        }

        public void SaveSetting<T>(T settings) where T : ISettings, new()
        {
            // 提取插件设置名作为 插件表名
            string pluginTableName = typeof(T).Name;
            // 简化--直接使用 json 文件模拟数据库表
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/PluginTables/" + pluginTableName + ".json");
            JObject jObject = new JObject();
            // 获取该类的所有属性
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (var property in propertyInfos)
            {
                jObject.Add(property.Name,new JValue(property.GetValue(settings)));
            }
            File.WriteAllText(filePath, jObject.ToString(), System.Text.Encoding.UTF8);
        }

        public void SetSetting<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool SettingExists<T, TPropType>(T settings) where T : ISettings, new()
        {
            throw new NotImplementedException();
        }
    }
}
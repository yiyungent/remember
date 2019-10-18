using Core;
using Services.Interface;

namespace Framework.Extensions
{
    public static class WebSetting
    {
        public static string Get(string key)
        {
            ISettingService settingService = ContainerManager.Resolve<ISettingService>();
            string value = settingService.GetSet(key);

            return value;
        }

        public static void Set(string key, string value)
        {
            ISettingService settingService = ContainerManager.Resolve<ISettingService>();
            settingService.Set(key, value);
        }
    }
}

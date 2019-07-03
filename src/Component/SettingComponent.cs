using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class SettingComponent : BaseComponent<Setting, SettingManager>, SettingService
    {
        public string GetSet(string key)
        {
            return manager.GetSet(key);
        }

        public void Set(string key, string value)
        {
            manager.Set(key, value);
        }
    }
}

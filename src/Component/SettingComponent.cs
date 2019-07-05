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
            return _manager.GetSet(key);
        }

        public void Set(string key, string value)
        {
            _manager.Set(key, value);
        }
    }
}

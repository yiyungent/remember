using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class LogInfoComponent : BaseComponent<LogInfo, LogInfoManager>, LogInfoService
    {
    }
}

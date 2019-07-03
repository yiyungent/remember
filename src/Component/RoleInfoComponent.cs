using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class RoleInfoComponent : BaseComponent<RoleInfo, RoleInfoManager>, RoleInfoService
    {
    }
}

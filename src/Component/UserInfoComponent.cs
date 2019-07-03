using Component.Base;
using Domain;
using Manager;
using Service;


namespace Component
{
    public class UserInfoComponent : BaseComponent<UserInfo, UserInfoManager>, UserInfoService
    {
        public bool Exist(string userName, int exceptId = 0)
        {
            return manager.Exist(userName, exceptId: exceptId);
        }
    }
}

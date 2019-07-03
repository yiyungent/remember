using Domain;
using Service.Base;

namespace Service
{
    public interface UserInfoService : BaseService<UserInfo>
    {
        bool Exist(string userName, int exceptId = 0);
    }
}

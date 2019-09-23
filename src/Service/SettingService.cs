using Domain;
using Service.Base;

namespace Service
{
    public interface SettingService : BaseService<Setting>
    {
        string GetSet(string key);

        void Set(string key, string value);

        bool SendMailVerifyCodeForFindPwd(string mail, out string vCode);
    }
}

using Domain;
using Service.Base;

namespace Service
{
    public interface ThemeTemplateService : BaseService<ThemeTemplate>
    {
        bool Exist(string templateName, int exceptId = 0);
    }
}

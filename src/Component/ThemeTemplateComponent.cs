using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class ThemeTemplateComponent : BaseComponent<ThemeTemplate, ThemeTemplateManager>, ThemeTemplateService
    {
        public bool Exist(string templateName, int exceptId = 0)
        {
            return _manager.Exist(templateName, exceptId: exceptId);
        }
    }
}

using Domain.Entities;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class ThemeTemplateService : BaseService<ThemeTemplate>, IThemeTemplateService
    {
        /// <summary>
        /// 指定模板是否存在
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="exceptId"></param>
        /// <returns></returns>
        public bool Exists(string templateName, int exceptId = 0)
        {
            bool isExist = this._repository.Count(
                m => m.TemplateName == templateName 
                && m.ID != exceptId 
                && !m.IsDeleted) > 0;

            return isExist;
        }
    }
}

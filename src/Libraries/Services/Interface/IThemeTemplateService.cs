using Domain.Entities;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public partial interface IThemeTemplateService : IService<ThemeTemplate>
    {
        /// <summary>
        /// 指定模板是否存在
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="exceptId"></param>
        /// <returns></returns>
        bool Exists(string templateName, int exceptId = 0);
    }
}

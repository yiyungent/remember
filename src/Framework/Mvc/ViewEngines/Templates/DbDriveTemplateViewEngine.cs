using Core;
using Framework.Factories;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.Mvc.ViewEngines.Templates
{
    public class DbDriveTemplateViewEngine : TemplateViewEngine
    {
        protected override string GetCurrentTemplate(ControllerContext controllerContext)
        {
            string templateName = null;
            try
            {
                ITemplateContext templateContext = HttpOneRequestFactory.Get<ITemplateContext>();
                templateName = templateContext.WorkingTemplateName;
            }
            catch (Exception ex)
            { }

            return templateName;
        }
    }
}

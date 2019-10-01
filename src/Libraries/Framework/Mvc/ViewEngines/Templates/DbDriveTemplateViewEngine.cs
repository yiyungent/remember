using Framework.Factories;
using System;
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

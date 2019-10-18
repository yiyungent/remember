using Core;
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
                ITemplateContext templateContext = ContainerManager.Resolve<ITemplateContext>();
                templateName = templateContext.WorkingTemplateName;
            }
            catch (Exception ex)
            { }

            return templateName;
        }
    }
}

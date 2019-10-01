using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Mvc.ViewEngines.Templates
{
    public interface ITemplateProvider
    {
        TemplateConfiguration GetTemplateConfiguration(string templateName);

        IList<TemplateConfiguration> GetTemplateConfigurations();

        bool TemplateConfigurationExists(string templateName);
    }
}

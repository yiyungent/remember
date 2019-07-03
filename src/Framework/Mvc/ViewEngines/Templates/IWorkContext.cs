using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Mvc.ViewEngines.Templates
{
    public interface IWorkContext
    {
        UserInfo CurrentUser { get; }

        bool AllowSelectTemplate { get; }

        string DefaultTemplateName { get; }

        void SaveSelectedTemplate(string templateName);
    }
}

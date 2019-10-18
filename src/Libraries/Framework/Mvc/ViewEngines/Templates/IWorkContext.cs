using Domain.Entities;

namespace Framework.Mvc.ViewEngines.Templates
{
    public interface IWorkContext
    {
        int CurrentUserId { get; }

        bool AllowSelectTemplate { get; }

        string DefaultTemplateName { get; }

        void SaveSelectedTemplate(string templateName);
    }
}

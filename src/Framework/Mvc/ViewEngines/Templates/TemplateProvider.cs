using Framework.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Framework.Mvc.ViewEngines.Templates
{
    public class TemplateProvider : ITemplateProvider
    {
        #region Fields

        private readonly IList<TemplateConfiguration> _templateConfigurations = new List<TemplateConfiguration>();
        private readonly string _basePath = string.Empty;

        #endregion

        #region Constructors

        public TemplateProvider(IWebHelper webHelper)
        {
            this._basePath = webHelper.MapPath("~/Templates/");
            LoadConfigurations();
        }

        #endregion

        #region IThemeProvider

        public TemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            return _templateConfigurations
                .SingleOrDefault(x => x.TemplateName.Equals(templateName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IList<TemplateConfiguration> GetTemplateConfigurations()
        {
            return _templateConfigurations;
        }

        public bool TemplateConfigurationExists(string templateName)
        {
            return GetTemplateConfigurations().Any(configuration => configuration.TemplateName.Equals(templateName, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        #region Utility

        private void LoadConfigurations()
        {
            //TODO:Use IFileStorage?
            foreach (string templatePath in Directory.GetDirectories(_basePath))
            {
                var configuration = CreateTemplateConfiguration(templatePath);
                if (configuration != null)
                {
                    _templateConfigurations.Add(configuration);
                }
            }
        }

        private TemplateConfiguration CreateTemplateConfiguration(string templatePath)
        {
            var templateDirectory = new DirectoryInfo(templatePath);
            var templateConfigFile = new FileInfo(Path.Combine(templateDirectory.FullName, "template.json"));

            if (templateConfigFile.Exists)
            {
                string jsonStr = File.ReadAllText(templateConfigFile.FullName, System.Text.Encoding.UTF8);
                return new TemplateConfiguration(templateDirectory.Name, templateDirectory.FullName, jsonStr);
            }

            return null;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Mvc.ViewEngines.Templates
{
    /// <summary>
    /// Template context
    /// </summary>
    public class TemplateContext : ITemplateContext
    {
        #region MyRegion
        //private readonly IWorkContext _workContext;
        //private readonly IStoreContext _storeContext;
        //private readonly IGenericAttributeService _genericAttributeService;
        //private readonly StoreInformationSettings _storeInformationSettings; 
        #endregion

        private readonly IWorkContext _workContext;
        private readonly ITemplateProvider _templateProvider;

        private bool _templateIsCached;
        private string _cachedTemplateName;

        #region MyRegion
        //public ThemeContext(IWorkContext workContext,
        //    IStoreContext storeContext,
        //    IGenericAttributeService genericAttributeService,
        //    StoreInformationSettings storeInformationSettings,
        //    IThemeProvider themeProvider)
        //{
        //    this._workContext = workContext;
        //    this._storeContext = storeContext;
        //    this._genericAttributeService = genericAttributeService;
        //    this._storeInformationSettings = storeInformationSettings;
        //    this._themeProvider = themeProvider;
        //} 
        #endregion

        public TemplateContext(IWorkContext workContext, ITemplateProvider templateProvider)
        {
            this._workContext = workContext;
            this._templateProvider = templateProvider;
        }

        /// <summary>
        /// Get or set current template system name
        /// </summary>
        public string WorkingTemplateName
        {
            get
            {
                if (_templateIsCached)
                    return _cachedTemplateName;

                string templateName = "";
                // 若允许用户选择模板，则获取当前用户选择的模板
                if (_workContext.AllowSelectTemplate)
                {
                    if (_workContext.CurrentUser != null)
                    {
                        templateName = _workContext.CurrentUser.TemplateName;
                    }
                }
                #region MyRegion
                //if (_storeInformationSettings.AllowCustomerToSelectTheme)
                //{
                //    if (_workContext.CurrentCustomer != null)
                //        theme = _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.WorkingThemeName, _genericAttributeService, _storeContext.CurrentStore.Id);
                //} 
                #endregion

                // 如果模板名为空，则获取默认模板
                if (string.IsNullOrEmpty(templateName))
                {
                    templateName = _workContext.DefaultTemplateName;
                }
                #region MyRegion
                //if (string.IsNullOrEmpty(theme))
                //template = _storeInformationSettings.DefaultStoreTheme; 
                #endregion

                // ensure that template exists
                if (!_templateProvider.TemplateConfigurationExists(templateName))
                {
                    var templateInstance = _templateProvider.GetTemplateConfigurations()
                        .FirstOrDefault();
                    if (templateInstance == null)
                        throw new Exception("No template could be loaded");
                    templateName = templateInstance.TemplateName;
                }

                // cache theme
                this._cachedTemplateName = templateName;
                this._templateIsCached = true;
                return templateName;
            }
            set
            {
                // 系统设置允许用户选择主题模板吗, 不允许 return
                if (!_workContext.AllowSelectTemplate)
                {
                    return;
                }
                #region MyRegion
                //if (!_storeInformationSettings.AllowCustomerToSelectTheme)
                //    return; 
                #endregion

                // 不允许未登录用户选择模板
                if (_workContext.CurrentUser == null)
                {
                    return;
                }
                #region MyRegion
                //if (_workContext.CurrentCustomer == null)
                //    return; 
                #endregion

                // 为当前的用户保存选择的主体模板
                _workContext.SaveSelectedTemplate(value);
                #region MyRegion
                //_genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, SystemCustomerAttributeNames.WorkingThemeName, value, _storeContext.CurrentStore.Id); 
                #endregion

                //clear cache
                this._templateIsCached = false;
            }
        }
    }
}

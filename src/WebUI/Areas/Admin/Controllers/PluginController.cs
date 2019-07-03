using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PluginHub;
using PluginHub.Domain.Cms;
using PluginHub.Infrastructure;
using PluginHub.Plugins;
using PluginHub.Services.Cms;
using PluginHub.Web.Mvc.Routes;
using WebUI.Areas.Admin.Models.PluginVM;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    public class PluginController : Controller
    {
        #region Fields

        private readonly IPluginFinder _pluginFinder;
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;

        #endregion

        #region Constructors

        public PluginController()
        {
            this._pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
            this._webHelper = EngineContext.Current.Resolve<IWebHelper>();
            this._widgetSettings = new WidgetSettings();
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual PluginModel PreparePluginModel(PluginDescriptor pluginDescriptor,
            bool prepareLocales = true, bool prepareStores = true)
        {
            var pluginModel = pluginDescriptor.ToModel();
            //logo
            pluginModel.LogoUrl = pluginDescriptor.GetLogoUrl(_webHelper);

            //configuration URLs

            if (pluginDescriptor.Installed)
            {
                //specify configuration URL only when a plugin is already installed

                //plugins do not provide a general URL for configuration
                //because some of them have some custom URLs for configuration
                //for example, discount requirement plugins require additional parameters and attached to a certain discount
                var pluginInstance = pluginDescriptor.Instance();
                string configurationUrl = null;
                if (pluginInstance is IWidgetPlugin)
                {
                    //Misc plugins
                    configurationUrl = Url.Action("ConfigureWidget", "Widget", new { systemName = pluginDescriptor.SystemName });
                }
                //else if (pluginInstance is IMiscPlugin)
                //{
                //    //Misc plugins
                //    configurationUrl = Url.Action("ConfigureMiscPlugin", "Plugin", new { systemName = pluginDescriptor.SystemName });
                //}
                pluginModel.ConfigurationUrl = configurationUrl;




                //enabled/disabled (only for some plugin types)
                if (pluginInstance is IWidgetPlugin)
                {
                    //Misc plugins
                    pluginModel.CanChangeEnabled = true;
                    pluginModel.IsEnabled = ((IWidgetPlugin)pluginInstance).IsWidgetActive(_widgetSettings);
                }

            }
            return pluginModel;
        }

        #endregion

        #region Methods

        #region Methods

        #region 列表
        public ActionResult Index()
        {
            var viewModel = new PluginListViewModel();
            var allPluginDescriptor = _pluginFinder.GetPluginDescriptors(LoadPluginsMode.All);
            foreach (var item in allPluginDescriptor)
            {
                var instance = item.Instance();
                var itemModel = new PluginItemModel
                {
                    SystemName = item.SystemName,
                    FriendlyName = item.FriendlyName,
                    Version = item.Version,
                    Author = item.Author,
                    SupportedVersions = item.SupportedVersions,
                    Installed = item.Installed,
                    HaveConfigure = instance is IWidgetPlugin,
                    HaveRoute = instance is IRouteProvider
                };
                if (itemModel.HaveRoute)
                {
                    //itemModel.RouteUrl=((IRouteProvider)instance)
                }
                viewModel.List.Add(itemModel);
            }

            return View(viewModel);
        }
        #endregion

        #region 安装
        public ActionResult Install(string systemName)
        {
            try
            {
                var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(systemName, LoadPluginsMode.All);
                if (pluginDescriptor == null)
                    //No plugin found with the specified id
                    return RedirectToAction("List");

                //check whether plugin is not installed
                if (pluginDescriptor.Installed)
                    return RedirectToAction("List");

                //install plugin
                pluginDescriptor.Instance().Install();

                //restart application
                _webHelper.RestartAppDomain();
            }
            catch (Exception exc)
            {
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region 卸载
        public ActionResult Uninstall(string systemName)
        {
            try
            {
                var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(systemName, LoadPluginsMode.All);
                if (pluginDescriptor == null)
                    //No plugin found with the specified id
                    return RedirectToAction("List");

                //check whether plugin is installed
                if (!pluginDescriptor.Installed)
                    return RedirectToAction("List");

                //uninstall plugin
                pluginDescriptor.Instance().Uninstall();

                //restart application
                _webHelper.RestartAppDomain();
            }
            catch (Exception exc)
            {
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region 重新加载
        public ActionResult ReloadList()
        {
            //restart application
            _webHelper.RestartAppDomain();
            return RedirectToAction("Index");
        }
        #endregion

        #region 配置
        [HttpGet]
        public ActionResult Configure(string systemName)
        {
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName(systemName, LoadPluginsMode.All);

            ConfigureViewModel viewModel = new ConfigureViewModel();
            viewModel.FriendlyName = pluginDescriptor.FriendlyName;
            viewModel.SystemName = pluginDescriptor.SystemName;

            #region 是否具有配置页
            string actionName = null, controllerName = null;
            RouteValueDictionary routeValues = null;
            if (pluginDescriptor.Instance() is IWidgetPlugin)
            {
                var widgetPlugin = pluginDescriptor.Instance<IWidgetPlugin>();
                widgetPlugin.GetConfigurationRoute(out actionName, out controllerName, out routeValues);
            }
            else
            {
                viewModel = null;
                return View(viewModel);
            }
            viewModel.ConfigurationActionName = actionName;
            viewModel.ConfigurationControllerName = controllerName;
            viewModel.ConfigurationRouteValues = routeValues;
            #endregion

            return View(viewModel);
        }
        #endregion

        #endregion

        #endregion
    }
}
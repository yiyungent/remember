using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using PluginHub;
using PluginHub.Domain.Cms;
using PluginHub.Infrastructure;
using PluginHub.Plugins;
using PluginHub.Services.Cms;
//using LightPlugin.Web.Infrastructure.Cache;
//using LightPlugin.Web.Models.Cms;

using WebUI.Models.Cms;

namespace WebUI.Controllers
{
    public partial class WidgetController : Controller
    {
        #region Fields

        private readonly IWidgetService _widgetService;
        //private readonly IStoreContext _storeContext;
        //private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructors

        public WidgetController()
        {
            this._widgetService = EngineContext.Current.Resolve<IWidgetService>();
            //this._storeContext = storeContext;
            //this._cacheManager = new PerRequestCacheManager(HttpContext);
        }

        #endregion

        #region Methods

        [ChildActionOnly]
        public ActionResult WidgetsByZone(string widgetZone, object additionalData = null)
        {
            List<RenderWidgetModel> viewModel = new List<RenderWidgetModel>();

            var widgets = _widgetService.LoadActiveWidgetsByWidgetZone(widgetZone);
            foreach (var widget in widgets)
            {
                RenderWidgetModel widgetModel = new RenderWidgetModel();

                string actionName;
                string controllerName;
                RouteValueDictionary routeValues;
                widget.GetDisplayWidgetRoute(widgetZone, out actionName, out controllerName, out routeValues);
                widgetModel.ActionName = actionName;
                widgetModel.ControllerName = controllerName;
                widgetModel.RouteValues = routeValues;

                //"RouteValues" property of widget models depends on "additionalData".
                if (additionalData != null)
                {
                    if (widgetModel.RouteValues == null)
                        widgetModel.RouteValues = new RouteValueDictionary();
                    widgetModel.RouteValues.Add("additionalData", additionalData);
                }

                viewModel.Add(widgetModel);
            }

            //no data?
            if (viewModel.Count == 0)
                return Content("");

            return PartialView(viewModel);
        }

        #endregion



    }
}

using Framework.Attributes;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc.ViewEngines.Templates;
using System.Web.Mvc;
using System.Web.Routing;

namespace Framework.Config
{
    public static class FrameworkConfig
    {
        public static void Register()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterModelBinder(ModelBinders.Binders);
            RegisterViewEngine(ViewEngines.Engines);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LoginAccountFilterAttribute());
            filters.Add(new AuthFilterAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
        }

        public static void RegisterModelBinder(ModelBinderDictionary binders)
        {
            binders.Add(typeof(CurrentAccountModel), new CurrentAccountModelBinder());
        }

        public static void RegisterViewEngine(ViewEngineCollection viewEngines)
        {
            viewEngines.Clear();
            //viewEngines.Add(new DbDriveTemplateViewEngine());
            viewEngines.Add(new RazorViewEngine());
        }
    }
}

using System.Web.Mvc;

namespace WebUI.Areas.Demo
{
#if DEBUG
    public class DemoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Demo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Demo_default",
                "Demo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
#endif
}
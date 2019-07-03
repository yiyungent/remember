using System.Web.Mvc;

namespace WebUI.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_UserInfo_Page",
                "Admin/UserInfo/Page{pageIndex}",
                new { controller = "UserInfo", action = "Index" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WebUI.Areas.Admin.Controllers" }
            );
        }
    }
}
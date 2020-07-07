using System.Web.Mvc;

namespace WebUI.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Account_Profile_Index_ById",
               "Account/u{id}",
               new { controller = "Profile", action = "Index" }
            );
            context.MapRoute(
              "Account_Profile_Index_ByName",
              "Account/@{userName}",
              new { controller = "Profile", action = "Index2" }
            );

            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "WebUI.Areas.Account.Controllers" }
            );
        }
    }
}
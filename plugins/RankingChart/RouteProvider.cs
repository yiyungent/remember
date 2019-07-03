using PluginHub.Web.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RankingChart
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority { get { return 1; } }

        public void RegisterRoutes(RouteCollection routes)
        {
            Route route = routes.MapRoute(
                           name: "PluginHub.RankingChart",
                           url: "plugin-RankingChart/{controller}/{action}/{id}",
                           defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                           namespaces: new string[] { "RankingChart.Controllers" }
                       );
            route.DataTokens["area"] = "RankingChart";
        }
    }
}
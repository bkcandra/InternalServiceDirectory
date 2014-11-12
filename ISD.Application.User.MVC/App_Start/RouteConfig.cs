using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ISD.Application.User.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "ISDPage",
              url: "Pages/{name}",
              defaults: new { controller = "Pages", action = "Index" }
          );
            routes.MapRoute(
                name: "ServiceNoAction",
                url: "Service/{id}/{name}",
                defaults: new { controller = "Service", action = "Index", name = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

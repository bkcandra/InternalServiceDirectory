using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ISD.Application.provider.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProviderServices",
                url: "Services/q=(page[{page}],pagesize[{pageSize}])",
                defaults: new { controller = "Services", action = "Index" }
            );
            routes.MapRoute(
               name: "ISDPage",
               url: "Page/{name}",
               defaults: new { controller = "Page", action = "Index" }
           );
           

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{name}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, name = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "ServiceNoAction",
            url: "Service/{id}/{name}",
            defaults: new { controller = "Service", action = "Index", name = UrlParameter.Optional }
        );
           
        }

       
    }
}

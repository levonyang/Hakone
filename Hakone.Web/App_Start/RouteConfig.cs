using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hakone.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("shopproductlist_1", "dian/{id}/{orderby}/p{page}",
               new { controller = "Shop", action = "Detail" },
               new { id = @"\d+", page = @"\d+" }
               );

            routes.MapRoute("shopproductlist_2", "dian/{id}/p{page}",
               new { controller = "Shop", action = "Detail" },
               new { id = @"\d+", page = @"\d+" }
               );

            routes.MapRoute("", "dian/p{page}",
               new { controller = "Dian", action = "Index" },
               new { page = @"\d+" }
               );

            routes.MapRoute("", "dian/{id}",
               new { controller = "Shop", action = "Detail" },
               new { id = @"\d+" }
               );

            routes.MapRoute("", "huo/{id}",
               new { controller = "Product", action = "Detail" },
               new { id = @"\d+" }
               );            

            routes.MapRoute("", "product/list/{catId}/p{page}",
               new { controller = "Product", action = "List" },
               new { catId = @"\d+", page = @"\d+" }
               );

            routes.MapRoute("", "product/list/p{page}",
               new { controller = "Product", action = "List" },
               new { page = @"\d+" }
               );

            routes.MapRoute("product_list_cat", "product/list/{catId}",
               new { controller = "Product", action = "List" },
               new { catId = @"\d+" }
               );

            //~/shop/all/cat_id/p100
            routes.MapRoute("", "Shop/{listFor}/cat{catId}/p{page}",
               new { controller = "Shop", action = "List" },
               new { listFor = "(all|hot|selected|recommend)", catId = @"\d+", page = @"\d+" }
               );

            //~/shop/all/cat_id
            routes.MapRoute("", "Shop/{listFor}/cat{catId}",
               new { controller = "Shop", action = "List" },
               new { listFor = "(all|hot|selected|recommend)", catId = @"\d+" }
               );

            // ~/shop/all/p100
            routes.MapRoute("", "Shop/{listFor}/p{page}",
               new { controller = "Shop", action = "List" },
               new { listFor = "(all|hot|selected|recommend)", page = @"\d+" }
               );

            // ~/shop/selected
            routes.MapRoute("", "Shop/{listFor}",
               new { controller = "Shop", action = "List" },
               new { listFor = "(all|hot|selected|recommend)" }
               );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}

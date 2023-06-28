using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SuperMarketK
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
              new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Add cart",
                url: "them-vao-gio",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Events",
                url: "tin-tuc",
                defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Detail News",
                url: "tin-tuc/{metatitle}--{id}",
                defaults: new { controller = "Content", action = "DetailNew", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Check Order",
                url: "kiem-tra-don-hang",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Search Product",
                url: "tim-kiem",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "About Us",
                url: "ve-chung-toi",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Contact Us",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Success",
                url: "hoan-thanh",
                defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Đăng Ký",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Category Product",
                url: "category/{metatitle}",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Detail Product",
                url: "products/{metatitle}/{id}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SuperMarketK.Controllers" }
            );
        }
    }
}

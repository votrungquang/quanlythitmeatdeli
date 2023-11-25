using System.Web.Mvc;
using System.Web.Routing;

namespace WebBanHangMeatDeli
{
    public class RouteConfig
    {
        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "CategoryProduct",
               url: "san-pham",
               defaults: new { controller = "Product", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "ShoppingCart",
               url: "cart",
               defaults: new { controller = "Cart", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "CategoryNew",
               url: "tin-tuc",
               defaults: new { controller = "New", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "AccountRegister",
               url: "account/register",
               defaults: new { controller = "Account", action = "Register", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "AccountLogin",
               url: "account/login",
               defaults: new { controller = "Account", action = "Login", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "CategoryAbout",
               url: "gioi-thieu",
               defaults: new { controller = "About", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "CategoryContact",
               url: "lien-he",
               defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "ProductDetail",
               url: "san-pham/{alias}-p-{id}",
               defaults: new { controller = "Product", action = "Detail", alias = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "ProductShow",
               url: "san-pham/{Name}",
               defaults: new { controller = "Product", action = "Show", name = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
               name: "PaymentCart",
               url: "cart/thanh-toan",
               defaults: new { controller = "Cart", action = "Checkout", name = UrlParameter.Optional },
               namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebBanHangMeatDeli.Controllers" }
            );
        }
    }
}

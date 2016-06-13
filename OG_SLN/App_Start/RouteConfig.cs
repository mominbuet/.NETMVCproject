using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Http.WebHost;
using System.Web.SessionState;

namespace OG_SLN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UserType",
                url: "{controller}/{action}/{id}/{type}",
                defaults: new { id = UrlParameter.Optional, type = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
            

            var route_defult = routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{id}",
             defaults: new { id = RouteParameter.Optional }
            );
            route_defult.RouteHandler = new MyHttpControllerRouteHandler();

            
            var route_action = routes.MapHttpRoute(
             name: "With_Action_Api",
                routeTemplate: "web/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            route_action.RouteHandler = new MyHttpControllerRouteHandler();
            

            var route_down_time = routes.MapHttpRoute(
             name: "Down_Api_Time",
                routeTemplate: "web/{controller}/{action}/{DEVICE_ID}"
            );
            route_down_time.RouteHandler = new MyHttpControllerRouteHandler();


            var route_down = routes.MapHttpRoute(
             name: "Download_Api",
                routeTemplate: "web/{controller}/{action}/{MAX_LAST_DOWN_TIME}/{DEVICE_ID}"
            );
            route_down.RouteHandler = new MyHttpControllerRouteHandler();
            
        }
    }

    public class MyHttpControllerHandler : HttpControllerHandler, IRequiresSessionState {
        public MyHttpControllerHandler(RouteData routeData)
            : base(routeData) {
        }
    }

    public class MyHttpControllerRouteHandler : HttpControllerRouteHandler {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext) {
            return new MyHttpControllerHandler(requestContext.RouteData);
        }
    }
    
}
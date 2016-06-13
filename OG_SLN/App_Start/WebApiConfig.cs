using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web;

namespace OG_SLN
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/

            
            config.Routes.MapHttpRoute(
                name: "WithActionApi",
                routeTemplate: "web/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );            
            
            config.Routes.MapHttpRoute(
                name: "DownApiTime",
                routeTemplate: "web/{controller}/{action}/{DEVICE_ID}"
            ); //.RouteHandler = new SessionRouteHandler(); 

            config.Routes.MapHttpRoute(
                name: "DownloadApi",
                routeTemplate: "web/{controller}/{action}/{MAX_LAST_DOWN_TIME}/{DEVICE_ID}"
            ); //.RouteHandler = new SessionRouteHandler(); 
            
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }

    /*
    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData) { }
    }

    public class SessionRouteHandler : IRouteHandler {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext) {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }

    public interface IDataPersistance<T> {
        T ObjectValue { get; set; }
    }

    public class SessionDataPersistance<T> : IDataPersistance<T>
      where T : class {
        private static string key = typeof(T).FullName.ToString();

        public T ObjectValue {
            get {
                return HttpContext.Current.Session[key] as T;
            }
            set {
                HttpContext.Current.Session[key] = value;
            }
        }
    }
    */
}

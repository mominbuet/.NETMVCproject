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
using System.IO;
using System.Configuration;
using System.Text;

using OG_SLN.Filters;

namespace OG_SLN {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            /*ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonDotNetValueProviderFactory());*/
        }


        /*
        public static void RegisterRoutes(RouteCollection routes) {
            var route_defult = routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{id}",
             defaults: new { id = RouteParameter.Optional }
            );
            route_defult.RouteHandler = new MyHttpControllerRouteHandler();


            var route_action = routes.MapHttpRoute(
             name: "WithActionApi",
                routeTemplate: "web/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            route_action.RouteHandler = new MyHttpControllerRouteHandler();

            var route_down_time = routes.MapHttpRoute(
             name: "DownApiTime",
                routeTemplate: "web/{controller}/{action}/{DEVICE_ID}"
            );
            route_down_time.RouteHandler = new MyHttpControllerRouteHandler();


            var route_down = routes.MapHttpRoute(
             name: "DownloadApi",
                routeTemplate: "web/{controller}/{action}/{MAX_LAST_DOWN_TIME}/{DEVICE_ID}"
            );
            route_down.RouteHandler = new MyHttpControllerRouteHandler();
        }
        */
        /*
        public override void Init() {
            this.AuthenticateRequest += new EventHandler(WebApiApplication_AuthenticateRequest);
            base.Init();
        }

        void WebApiApplication_AuthenticateRequest(object sender, EventArgs e) {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

            SampleIdentity id = new SampleIdentity(ticket);
            GenericPrincipal prin = new GenericPrincipal(id, null);

            HttpContext.Current.User = prin;
        }
        */

        
        protected void Application_Error(object sender, EventArgs e) {
            Exception ex = Server.GetLastError();
            HttpException httpException = ex as HttpException;

            this.WriteErrorLog(GetErrorMessage(ex, true), ex);

            //Response.Redirect("~/Error/Index");
        }
        
        public static string GetErrorMessage(Exception ex, bool includeStackTrace) {
            StringBuilder msg = new StringBuilder();
            BuildErrorMessage(ex, ref msg);
            if (includeStackTrace) {
                msg.Append("\n");
                msg.Append(ex.StackTrace);
            }
            return msg.ToString();
        }

        private static void BuildErrorMessage(Exception ex, ref StringBuilder msg) {
            if (ex != null) {
                msg.Append(ex.Message);
                msg.Append("\n");
                if (ex.InnerException != null) {
                    BuildErrorMessage(ex.InnerException, ref msg);
                }
            }
        }

        private void WriteErrorLog(string err_msg, Exception ex) {
            string file_path = ConfigurationManager.AppSettings["ERROR_APTH"];
            string file_name = DateTime.Today.ToString("yyyy_MM_dd") + ".txt";
            string full_error_path = string.Empty;

            string controllerName = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller").Trim();
            string actionName = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action").Trim();

            FileStream fs = null;
            StreamWriter sw = null;

            try {
                full_error_path = Server.MapPath(file_path + file_name);

                if (!File.Exists(full_error_path)) {
                    fs = File.Create(full_error_path);
                    fs.Close();
                }

                sw = File.AppendText(full_error_path);
                sw.WriteLine("\n");
                sw.WriteLine(" \n");
                sw.WriteLine("******************************************************************");

                sw.WriteLine("\nDATE: " + System.DateTime.Now);

                if (controllerName != null) {
                    sw.WriteLine("\nCONTROLLER: " + controllerName);
                }
                if (actionName != null) {
                    sw.WriteLine("\nACTION: " + actionName);
                }

                if (HttpContext.Current.Session["SEES_USER_NO"] != null) {
                    sw.WriteLine("\nUSER_NO: " + HttpContext.Current.Session["SEES_USER_NO"]);
                }
                if (HttpContext.Current.Session["SEES_LOGON_HIS_NO"] != null) {
                    sw.WriteLine("\nLOGON_HIS_NO: " + HttpContext.Current.Session["SEES_LOGON_HIS_NO"]);
                }
                if (HttpContext.Current.Session["SEES_LOGON_NAME"] != null) {
                    sw.WriteLine("\nLOGON_NAME: " + HttpContext.Current.Session["SEES_LOGON_NAME"]);
                }

                sw.WriteLine("\nMESSAGE: " + ex.Message);

                sw.WriteLine("\nSOURCE: " + ex.Source);
                sw.WriteLine("\nINSTANCE: " + ex.InnerException);

                sw.WriteLine("\nDATA: " + ex.Data);
                sw.WriteLine("\nTARGETSITE: " + ex.TargetSite);

                sw.WriteLine("\nSTACKTRACE: " + ex.StackTrace + "\n");
                sw.WriteLine("\nERROR DETAILS: " + err_msg + "\n");

                sw.WriteLine("\n******************************************************************");
                //sw.Close();

            } catch (Exception errEx) {

            } finally {
                sw.Close();
            }
        }
        
    }

    /*
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

    // Now Session is visible in your Web API
    public class ValuesController : ApiController {
        public string GET(string input) {
            var session = HttpContext.Current.Session;
            if (session != null) {
                if (session["Time"] == null)
                    session["Time"] = DateTime.Now;
                return "Session Time: " + session["Time"] + input;
            }
            return "Session is not availabe" + input;
        }
    }
    */
}
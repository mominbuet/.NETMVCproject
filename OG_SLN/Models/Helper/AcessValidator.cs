using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace OG_SLN.Models
{
    public class AcessValidator
    {
        /*
        public static void LoadSecurityAccess()
        {
            using (OGDBEntities db = new OGDBEntities())
            {
                long? ROLE_NO = null;
                List<SET_ROLE_ACTION> per_list = new List<SET_ROLE_ACTION>();
                try
                {
                    ROLE_NO = long.Parse(HttpContext.Current.Session["ROLE_NO"].ToString());
                }
                catch (Exception ex)
                {
                }

                if (ROLE_NO.HasValue)
                {
                    var rd = HttpContext.Current.Request.RequestContext.RouteData;

                    string controller_name = rd.GetRequiredString("controller").Trim();
                    string action_name = rd.GetRequiredString("action").Trim();

                    per_list = db.SET_ROLE_ACTION.Where(a => (a.ROLE_NO == ROLE_NO.Value)).ToList();

                }
            }
        }
        */
        private static bool IsAllowed()
        {
            using (OGDBEntities db = new OGDBEntities())
            {
                long? ROLE_NO = null;
                try
                {
                    ROLE_NO = long.Parse(HttpContext.Current.Session["ROLE_NO"].ToString());
                }
                catch (Exception ex)
                {
                }

                if (ROLE_NO.HasValue)
                {
                    var rd = HttpContext.Current.Request.RequestContext.RouteData;

                    string controller_name = rd.GetRequiredString("controller").Trim();
                    string action_name = rd.GetRequiredString("action").Trim();

                    SET_ROLE_ACTION per = db.SET_ROLE_ACTION
                        .Where(a => a.ROLE_NO == ROLE_NO.Value).FirstOrDefault();
                }

                return false;
            }
        }
    }


    public class ControllerList
    {
        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }

        public List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<Controller>().ForEach(
                type => controllerNames.Add(type.Name));
            return controllerNames;
        }

        public List<ControllerActionViewModel> GetControllerActions()
        {
            var allTypes = from asm
                           in AppDomain.CurrentDomain.GetAssemblies()
                           let types = (
                               from type
                               in asm.GetTypes()
                               let controllerName = type.Name.Replace("Controller", "")
                               let methods = (
                                   from method
                                   in type.GetMethods()
                                   let hasHttpPost =
                                       method.GetCustomAttributes(
                                       typeof(HttpPostAttribute),
                                       false).Length > 0
                                   where (method.ReturnType.IsSubclassOf(typeof(ActionResult))
                                           || method.ReturnType == typeof(ActionResult))
                                   //&& !hasHttpPost
                                   select new
                                   {
                                       Controller = controllerName,
                                       Action = method.Name
                                   }
                               )
                               where type.IsSubclassOf(typeof(Controller))
                               select new { Name = controllerName, Actions = methods }
                           )
                           select new { asm.FullName, AllControllers = types };

            List<ControllerActionViewModel> controllerList = new List<ControllerActionViewModel>();
            try
            {

                foreach (var controllers in allTypes)
                {
                    foreach (var cc in controllers.AllControllers.OrderBy(a => a.Name))
                    {

                        foreach (var aa in cc.Actions.Distinct().OrderBy(a => a.Controller).OrderBy(a => a.Action))
                        {
                            if (aa.Action.Trim() != "DeleteConfirmed")
                            {
                                ControllerActionViewModel c = new ControllerActionViewModel();

                                c.controllerName = aa.Controller;
                                c.actionName = aa.Action;
                                c.IsAutoInclude = false;
                                c.IsActive = true;

                                controllerList.Add(c);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return controllerList;
        }
    }
}
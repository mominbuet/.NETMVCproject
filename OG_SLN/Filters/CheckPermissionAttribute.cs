using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

using OG_SLN.Models;

namespace OG_SLN.Filters
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        
        public CheckPermissionAttribute()
        { }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            using (OGDBEntities db = new OGDBEntities())
            {
                HttpBrowserCapabilitiesBase browser = httpContext.Request.Browser;

                string controllerName = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller").Trim();
                string actionName = httpContext.Request.RequestContext.RouteData.GetRequiredString("action").Trim();


                List<GEN_CONTROLLER_ACTION> public_list = httpContext.Session["sess_PUBLIC_LIST"] as List<GEN_CONTROLLER_ACTION>;
                if (public_list == null)
                {
                    public_list = db.GEN_CONTROLLER_ACTION.Where(a => (a.IS_ACTIVE == 1) && (a.IS_PUBLIC == 1)).ToList();
                    httpContext.Session["sess_PUBLIC_LIST"] = public_list;
                }


                GEN_CONTROLLER_ACTION public_allow = public_list.Where(a =>
                    (a.CONTROLLER_NAME.Trim().ToUpper() == controllerName.Trim().ToUpper())
                    && (a.ACTION_NAME.Trim().ToUpper() == actionName.Trim().ToUpper())
                    ).FirstOrDefault();
                if (public_allow != null)
                {
                    return true;
                }

                SEC_USERS_LOGIN_Result1 user = httpContext.Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;

                if (user != null && user.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser)
                {
                    List<SET_USER_ACTION> perm_list = httpContext.Session["sess_PERMISSION_LIST"] as List<SET_USER_ACTION>;

                    if(perm_list == null)
                    {
                        perm_list = db.SET_USER_ACTION.Include(a => a.GEN_CONTROLLER_ACTION)
                            .Where(a => a.USER_NO == user.USER_NO).ToList();
                        httpContext.Session["sess_PERMISSION_LIST"] = perm_list;
                    }

                    if ((perm_list == null) || (perm_list.Count == 0))
                    {
                        return false;
                    }
                    else
                    {
                        SET_USER_ACTION action_allow = perm_list.Where(a =>
                            (a.GEN_CONTROLLER_ACTION.CONTROLLER_NAME.Trim().ToUpper() == controllerName.Trim().ToUpper())
                        && (a.GEN_CONTROLLER_ACTION.ACTION_NAME.Trim().ToUpper() == actionName.Trim().ToUpper())
                            /*&& (a.IS_ALLOWED == 1)*/).FirstOrDefault();
                        if (action_allow != null)
                        {
                            return true;
                        }
                    }
                }

                else
                {
                    List<SET_ROLE_ACTION> perm_list = httpContext.Session["sess_PERMISSION_LIST"] as List<SET_ROLE_ACTION>;

                    long? role_no = null;

                    if (httpContext.Session["ROLE_NO"] != null)
                    {
                        role_no = long.Parse(httpContext.Session["ROLE_NO"].ToString());
                    }

                    if (role_no == null)
                    {
                        //userType = (long)UserTypes.Public;
                        return false;
                    }

                    if (perm_list == null)
                    {
                        perm_list = db.SET_ROLE_ACTION.Include(a => a.GEN_CONTROLLER_ACTION)
                            .Where(a => a.ROLE_NO == role_no).ToList();
                        httpContext.Session["sess_PERMISSION_LIST"] = perm_list;
                    }

                    if ((perm_list == null) || (perm_list.Count == 0))
                    {
                        return false;
                    }
                    else
                    {
                        SET_ROLE_ACTION action_allow = perm_list.Where(a =>
                            (a.GEN_CONTROLLER_ACTION.CONTROLLER_NAME.Trim().ToUpper() == controllerName.Trim().ToUpper())
                        && (a.GEN_CONTROLLER_ACTION.ACTION_NAME.Trim().ToUpper() == actionName.Trim().ToUpper())
                            /*&& (a.IS_ALLOWED == 1)*/).FirstOrDefault();
                        if (action_allow != null)
                        {
                            return true;
                        }
                    }
                }

                return base.AuthorizeCore(httpContext);
            }
        }
        
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }
}
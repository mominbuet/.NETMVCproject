using System.Web;
using System.Web.Mvc;

using OG_SLN.Filters;

namespace OG_SLN
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new CheckPermissionAttribute());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Mvc;
using System.Web.Script.Services;

namespace OG_SLN.Services
{
    /// <summary>
    /// Summary description for SessionSaveSpecimenDistribution
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SessionSaveSpecimenDistribution : System.Web.Services.WebService
    {
        [WebMethod]
        public string SaveSpecimenOnSession(int id, int qty)
        {
            return  id + "Hello World" + qty;
        }
    }
}

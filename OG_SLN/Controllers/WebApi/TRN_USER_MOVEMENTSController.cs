using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class TRN_USER_MOVEMENTSController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }       
        //
        // GET: /TRN_USER_MOVEMENTS/

        

    }
}

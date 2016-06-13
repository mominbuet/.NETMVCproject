using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;

namespace OG_SLN.Controllers
{
    public class DownloadsController : Controller
    {
        //
        // GET: /Downloads/
        private OGDBEntities db = new OGDBEntities();

        public ActionResult Reset()
        {
            ObjectResult<DOWNLOAD_RESET_ZONAL_GET_Result> zonals = null;
            UpDownSearchModel model = null;

            decimal? USER_NO = decimal.Parse(Session["sess_USER_NO"].ToString());

            ViewBag.Search_Division = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(USER_NO, null, null, null).ToList(), 
                "USER_NO", "USER_FULL_NAME");

            if (Request.QueryString.HasKeys())
            {
                model = new UpDownSearchModel();
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Division"])
                    && decimal.Parse(Request.QueryString["Search_Division"]) != 0)
                {
                    model.Search_Division = decimal.Parse(Request.QueryString["Search_Division"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Zilla"])
                    && decimal.Parse(Request.QueryString["Search_Zilla"]) != 0)
                {
                    model.Search_Zilla = decimal.Parse(Request.QueryString["Search_Zilla"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Thana"])
                    && decimal.Parse(Request.QueryString["Search_Thana"]) != 0)
                {
                    model.Search_Thana = decimal.Parse(Request.QueryString["Search_Thana"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User"])
                    && decimal.Parse(Request.QueryString["Search_User"]) != 0)
                {
                    model.Search_User = decimal.Parse(Request.QueryString["Search_User"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_To"]))
                {
                    model.Search_To = DateTime.Parse(Request.QueryString["Search_To"]);
                }



                zonals = db.DOWNLOAD_RESET_ZONAL_GET(USER_NO, model.Search_User, model.Search_Division, 
                    model.Search_Zilla, model.Search_Thana);
            }

            ViewBag.Search_Model = model;

            return View(zonals);
        }

        public JsonResult doReset(string name, DateTime? date)
        {
            db.DOWNLOAD_RESET(name, date);
            return Json(new { message = "Download reset successful"});
        }

    }
}

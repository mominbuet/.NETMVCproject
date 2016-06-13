using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using OG_SLN.Models;

namespace OG_SLN.Controllers
{
    public class ViewDCRSheetController : Controller
    {
        //
        // GET: /ViewDCRSheet/

        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            ViewBag.USER_NO = new SelectList(db.SEC_USERS
                .Where(a => a.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager)
                .ToList(), "USER_NO", "USER_FULL_NAME");

            bool Is_ZonalOrAgent = (bool)Session["sess_Is_ZonalOrAgent"];
            ViewBag.Is_ZonalOrAgent = Is_ZonalOrAgent;

            return View();
        }

        [HttpPost]
        public ActionResult Index(VIEW_DCR_SHEET_Search obj) {

            SEC_USERS_LOGIN_Result1 user = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;

            if ((user != null) && (user.USER_TYPE_NO == (decimal) UserType.Zonal_Manager)) {
                obj.USER_NO = user.USER_NO;
            }

            SEC_USERS_ZM_LOGIN_GET_Result user_result = db.SEC_USERS_ZM_LOGIN_GET(null, null, obj.USER_NO, null).FirstOrDefault();


            List<TRN_DCR_SHEET_SUM_Result> dcr_sum_result = db.TRN_DCR_SHEET_SUM(obj.USER_NO, obj.TRN_DCR_DATE).ToList();
            foreach (var dcr in dcr_sum_result) {
                dcr.TRN_DCR_SHEET_DET_List = db.TRN_DCR_SHEET_DET(dcr.DCR_NO).ToList();
            }

            List<TRN_EXPENSE_SUM_Result> exp_result = db.TRN_EXPENSE_SUM(obj.USER_NO, obj.TRN_DCR_DATE).ToList();

            bool Is_ZonalOrAgent = (bool)Session["sess_Is_ZonalOrAgent"];
            ViewBag.Is_ZonalOrAgent = Is_ZonalOrAgent;

            ViewBag.TRN_DCR_DATE = obj.TRN_DCR_DATE;
            ViewBag.user_result = user_result;
            ViewBag.dcr_sum_result = dcr_sum_result;
            ViewBag.exp_result = exp_result;

            return View("DcrSheet");
        }

        [ActionName("ZonalManager")]
        public ActionResult GetZonalManagerForm()
        {
            ViewBag.Zonal_Dept  = new SelectList(db.SET_DEPARTMENT, "DEPT_NO", "DEPT_NAME");
            ViewBag.Zonal_Desig = new SelectList(db.SET_DESIGNATION, "DESIG_NO", "DESIG_NAME");

            ViewBag.Division_No = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Zilla_No = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0), 
                "ZILLA_NO", "ZILLA_NAME");
            ViewBag.Zonal_Thana = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0), 
                "THANA_NO", "THANA_NAME");

            SEC_USERS_LOGIN_Result1 user = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;

            ViewBag.Zonal_Parent_No = user.USER_NO;


            return PartialView("_GetZonalManagerPartial");
        }
    }
}

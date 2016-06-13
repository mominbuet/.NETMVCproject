using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Models;

namespace OG_SLN.Controllers {
    public class DcrExpenseAllCostController : Controller {
        //
        // GET: /DcrExpenseAllCost/

        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index() {

            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.GEN_USER_TYPES.SHORT_NAME == "ZM");
            ViewBag.fromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.toDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();
            ViewBag.verify_status = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE");
            ViewBag.ddldivision = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.zmusers = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");

            IList<TRN_GET_ALL_COST_Result> result = db.TRN_GET_ALL_COST(null, decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                null, null, null, DateTime.Today, DateTime.Today).ToList();

            ViewBag.result = result;


            return View();
        }

        [HttpPost]
        public ActionResult Index(DcrExpenseAllCostSearch search_obj) {
            search_obj.USER_PARENT_NO = decimal.Parse(Session["sess_USER_NO"].ToString());

            if (search_obj.USER_NO == 0) {
                search_obj.USER_NO = null;
            }
            if (search_obj.ddldivision == 0) {
                search_obj.ddldivision = null;
            }
            if (search_obj.ddlZillas == 0) {
                search_obj.ddlZillas = null;
            }
            if (search_obj.ddlThanas == 0) {
                search_obj.ddlThanas = null;
            }
            if (search_obj.zmusers == 0) {
                search_obj.zmusers = null;
            }

            if (search_obj.zmusers > 0) {
                search_obj.USER_NO = search_obj.zmusers;
            }


            IList<TRN_GET_ALL_COST_Result> result = db.TRN_GET_ALL_COST(search_obj.USER_NO, search_obj.USER_PARENT_NO,//parent
                search_obj.ddldivision, search_obj.ddlZillas, search_obj.ddlThanas, search_obj.DateFrom, search_obj.DateTo).ToList();

            ViewBag.result = result;

            return View("Result");
        }
    }
}

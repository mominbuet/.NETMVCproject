using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Models;
using PagedList;

namespace OG_SLN.Controllers {
    public class DCRSummarySearchController : Controller {
        //
        // GET: /DCRSummarySearch/
        int page_size = 250;

        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        SEC_USERS_LOGIN_Result1 sess_sec_users = null;
        decimal? sess_USER_NO = null;
        decimal? sess_LOGON_NO = null;

        private void GetLoggedInInfo() {
            this.sess_sec_users = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;
            this.sess_LOGON_NO = Session["sess_LOGON_NO"] as decimal?;
            this.sess_USER_NO = Session["sess_USER_NO"] as decimal?;

        }

        public JsonResult GetZilla(decimal DIVISION_NO) {
            var zilla_list = (from z in db.SET_ZILLA
                              where z.DIVISION_NO == DIVISION_NO
                              select new {
                                  @ZILLA_NO = z.ZILLA_NO,
                                  @ZILLA_NAME = z.ZILLA_NAME, 
                              });
            return Json(zilla_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThana(decimal ZILLA_NO) {
            var thana_list = (from t in db.SET_THANA
                              where t.ZILLA_NO == ZILLA_NO
                              select new {
                                  @THANA_NO = t.THANA_NO,
                                  @THANA_NAME = t.THANA_NAME,
                              });
            return Json(thana_list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index() {

            List<SET_DIVISION> div_list = db.SET_DIVISION.OrderBy(a => a.DIVISION_NAME).ToList();
            List<SET_ZILLA> zilla_list = new List<SET_ZILLA>();
            List<SET_THANA> thana_list = new List<SET_THANA>();


            ViewBag.DIVISION_NO = new SelectList(div_list, "DIVISION_NO", "DIVISION_NAME", "");
            ViewBag.ZILLA_NO = new SelectList(zilla_list, "ZILLA_NO", "ZILLA_NAME");
            ViewBag.THANA_NO = new SelectList(thana_list, "THANA_NO", "THANA_NAME");
            var user_list = db.SEC_USERS.Where(a => (a.USER_TYPE_NO == (long) UserType.Zonal_Manager) || (a.USER_TYPE_NO == (long) UserType.Agents)).OrderBy(a => a.USER_NAME).ToList();
            ViewBag.USER_NO = new SelectList(user_list, "USER_NO", "USER_NAME");

            Session["sess_TRN_DCR_SUM_SEARCH"] = null;

            return View();
        }

        [HttpPost]
        public ActionResult Index(TRN_DCR_SUM_SEARCH search_obj) {
            this.GetLoggedInInfo();

            ViewBag.DIVISION_NO = search_obj.DIVISION_NO;
            ViewBag.ZILLA_NO = search_obj.ZILLA_NO;
            ViewBag.THANA_NO = search_obj.THANA_NO;

            ViewBag.MON_NO = search_obj.MON_NO;
            ViewBag.YEAR = search_obj.YEAR;
            ViewBag.TRN_DCR_DATE_FROM = search_obj.TRN_DCR_DATE_FROM;
            ViewBag.TRN_DCR_DATE_TO = search_obj.TRN_DCR_DATE_TO;

            //var data_list = db.TRN_DCR_SUM_SEARCH(search_obj.DIVISION_NO, search_obj.ZILLA_NO, search_obj.THANA_NO, search_obj.USER_NO, search_obj.MON_NO, search_obj.YEAR, search_obj.TRN_DCR_DATE_FROM, search_obj.TRN_DCR_DATE_TO);


            List<TRN_DCR_SUM_SEARCH_01_Result1> data_list = db.TRN_DCR_SUM_SEARCH_01(search_obj.DIVISION_NO, search_obj.ZILLA_NO, search_obj.THANA_NO, search_obj.USER_NO, search_obj.MON_NO, search_obj.YEAR, search_obj.TRN_DCR_DATE_FROM, search_obj.TRN_DCR_DATE_TO, this.sess_USER_NO).ToList();

            ViewBag.TotalRecords = data_list.Count;
            ViewBag.page_number = 1;

            Session["sess_TRN_DCR_SUM_SEARCH"] = data_list;

            return View("List", data_list.Take(this.page_size));

        }

        public ActionResult GetPaged(int page_number) {
            List<TRN_DCR_SUM_SEARCH_01_Result1> data_list = Session["sess_TRN_DCR_SUM_SEARCH"] as List<TRN_DCR_SUM_SEARCH_01_Result1>;

            ViewBag.TotalRecords = data_list.Count;
            ViewBag.page_number = page_number;

            if (page_number == 1) {
                return View("List", data_list.Take(this.page_size));
            } else {
                return View("List", data_list.Skip((page_number * this.page_size) - 1).Take(this.page_size));
            }
            
        }

        [HttpPost]
        public ActionResult GetDCRSumDet(TRN_DCR_SUM_SEARCH search_obj) {
            this.GetLoggedInInfo();

            //var data_list = db.TRN_DCR_SUMDET_SEARCH(search_obj.DEPT_NO, search_obj.DIVISION_NO, search_obj.ZILLA_NO, search_obj.THANA_NO, search_obj.USER_NO, search_obj.MON_NO, search_obj.YEAR, search_obj.TRN_DCR_DATE_FROM, search_obj.TRN_DCR_DATE_TO, search_obj.DCR_TYPE_NO);
            var data_list = db.TRN_DCR_SUMDET_SEARCH(search_obj.DEPT_NO, null, null, null, 
                search_obj.USER_NO, null, null, search_obj.TRN_DCR_DATE_FROM, 
                search_obj.TRN_DCR_DATE_TO, search_obj.DCR_TYPE_NO, this.sess_USER_NO);

            if (search_obj.TRN_DCR_DATE_FROM.Value.Date == search_obj.TRN_DCR_DATE_TO.Value.Date)
            {
                ViewBag.Same_Date = true;
            }
            else
            {
                ViewBag.Same_Date = false;
            }

            return View(data_list);
        }

    }
}

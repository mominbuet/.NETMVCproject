using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class UserInstituteController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        private SEC_USERS sess_sec_users = null;
        decimal? sess_USER_NO = null;
        decimal? sess_LOGON_NO = null;

        //
        // GET: /UserInstitute/

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void GetLoginInfo() {
            this.sess_sec_users = Session["sess_sec_users"] as SEC_USERS;
            this.sess_USER_NO = Session["sess_USER_NO"] as decimal?;
            this.sess_LOGON_NO = Session["sess_LOGON_NO"] as decimal?;
        }

        public ActionResult Index()
        {
            return HttpNotFound();
            List<SEC_USER_INSTITUTE> sec_user_institute = new List<SEC_USER_INSTITUTE>();
            //var sec_user_institute = db.SEC_USER_INSTITUTE.Include("SEC_USERS").Include("SET_INSTITUTE");
            return View(sec_user_institute.ToList());
        }

        //
        // GET: /UserInstitute/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SEC_USER_INSTITUTE sec_user_institute = db.SEC_USER_INSTITUTE.Single(s => s.USER_INSTITUTE_NO == id);
            if (sec_user_institute == null)
            {
                return HttpNotFound();
            }
            return View(sec_user_institute);
        }

        //
        // GET: /UserInstitute/Create

        public ActionResult Create()
        {
            decimal? USER_NO = null;

            List<SEC_USERS> user_list = db.SEC_USERS.Where(a => a.USER_TYPE_NO == 5).ToList();
            ViewBag.user_list = user_list;
            ViewBag.USER_NO = new SelectList(user_list, "USER_NO", "USER_NAME");

            if ((user_list != null) && (user_list.Count > 0)) {
                USER_NO = user_list.FirstOrDefault().USER_NO;
            }

            ViewBag.INST_TYPE_NO = new SelectList(db.SET_INST_TYPE, "INST_TYPE_NO", "INST_TYPE_NAME");

            List<SET_ZILLA> zilla_list = db.SET_ZILLA.OrderBy(a => a.ZILLA_NAME).ToList();
            ViewBag.ZILLA_NO = new SelectList(zilla_list, "ZILLA_NO", "ZILLA_NAME");

            decimal ZILLA_NO = zilla_list.FirstOrDefault().ZILLA_NO;

            List<SET_THANA> thana_list = db.SET_THANA.Where(a => a.ZILLA_NO == ZILLA_NO).OrderBy(a => a.THANA_NAME).ToList();
            ViewBag.THANA_NO = new SelectList(thana_list, "THANA_NO", "THANA_NAME");

            //ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "LAST_ACTION");

            var institute_list = db.SEC_USER_INSTITUTE_SEARCH(null, USER_NO, null, null).ToList();

            ViewBag.institute_list = institute_list;

            return View();
        }

        //
        // POST: /UserInstitute/Create

        [HttpPost]
        public JsonResult Create(SecUserInstituteSave sec_user_institute)
        {
            bool is_success = false;
            string msg = @"";

            if (ModelState.IsValid)
            {

                this.GetLoginInfo();
                try {
                    if (sec_user_institute.IS_ACTIVE.HasValue) {                        
                        //db.SEC_USER_INSTITUTE_INSERT(this.sess_USER_NO, this.sess_LOGON_NO, sec_user_institute.USER_NO, sec_user_institute.INSTITUTE_NO, sec_user_institute.IS_ACTIVE, sec_user_institute.ACTIVE_FROM, sec_user_institute.ACTIVE_TO, sec_user_institute.SL_NUM);                        
                        db.SEC_USER_INSTITUTE_SAVE(this.sess_USER_NO, this.sess_LOGON_NO, sec_user_institute.USER_NO, sec_user_institute.INSTITUTE_NO, sec_user_institute.IS_ACTIVE, sec_user_institute.ACTIVE_FROM, sec_user_institute.ACTIVE_TO, sec_user_institute.SL_NUM);                        
                    }

                    is_success = true;
                    msg = "saved successfully.";

                } catch (Exception ex) {
                    is_success = false;
                    msg = "May be already added.";
                }
                
            }

            var ressult = new { @is_success = is_success, @msg = msg };

            return Json(ressult, JsonRequestBehavior.AllowGet);

        }       

        [HttpPost]
        public ActionResult LoadInstitute(InstituteSearch search_obj) {

            var institute_list = db.SET_INSTITUTE_SEARCH(search_obj.INSTITUTE_NO, search_obj.INSTITUTE_NAME, search_obj.INSTITUTE_NAME_BNG, search_obj.INST_TYPE_NO, search_obj.THANA_NO, search_obj.F_INSTITUTION_DB_ID, search_obj.EIIN_NUMBER, search_obj.USER_NO).ToList();

            return View(institute_list);
        }

        [HttpPost]
        public ActionResult LoadUserInstitute(SecUserInstituteSearch search_obj) {
            var institute_list = db.SEC_USER_INSTITUTE_SEARCH(search_obj.USER_INSTITUTE_NO, search_obj.USER_NO, search_obj.INSTITUTE_NO, search_obj.IS_ACTIVE).ToList();
            return View(institute_list);
        }

        public JsonResult LoadUserThana(SecUserThanaSearch search_obj) {
            bool is_success = false;
            string msg = @"";

            SEC_USER_THANA_GET_Result sec_user_thana_result = db.SEC_USER_THANA_GET(null, search_obj.THANA_NO, search_obj.USER_NO, null, null, null, null, 1, int.MaxValue).FirstOrDefault();

            if ((sec_user_thana_result != null) && (sec_user_thana_result.IS_ACTIVE.HasValue) && (sec_user_thana_result.IS_ACTIVE.Value == 1)) {
                is_success = true;
            }

            var ressult = new { @is_success = is_success, @msg = msg };

            return Json(ressult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUserThana(SecUserThanaSearch search_obj) {
            bool is_success = false;
            string msg = @"";
            try {
                db.SEC_USER_THANA_STORE(this.sess_USER_NO, this.sess_LOGON_NO, search_obj.THANA_NO, search_obj.USER_NO, search_obj.IS_ACTIVE);

                is_success = true;
                msg = "saved successfully";

            } catch (Exception ex) {
                msg = "failed to save";
            }

            var ressult = new { @is_success = is_success, @msg = msg };

            return Json(ressult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThanaByZilla(decimal ZILLA_NO) {
            var result = (from t in db.SET_THANA
                          where t.ZILLA_NO == ZILLA_NO
                          orderby t.THANA_NAME
                          select new {
                              @THANA_NO = t.THANA_NO ,
                              @THANA_NAME = t.THANA_NAME , 
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
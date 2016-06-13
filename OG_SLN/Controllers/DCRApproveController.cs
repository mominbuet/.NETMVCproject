using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using System.Data.Common;
using OG_SLN.Models;
using System.Data.Entity.Infrastructure;

namespace OG_SLN.Controllers {
    public class DCRApproveController : Controller {

        int page_size = 10;

        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        SEC_USERS_LOGIN_Result1 sess_sec_users = null;
        decimal? sess_USER_NO = null;
        decimal? sess_LOGON_NO = null;
        String sess_entry_user_name = null;

        private void GetLoggedInInfo()
        {
            this.sess_sec_users = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;
            this.sess_LOGON_NO = Session["sess_LOGON_NO"] as decimal?;
            this.sess_USER_NO = Session["sess_USER_NO"] as decimal?;
            sess_entry_user_name = sess_sec_users.USER_NAME;
        }

        //
        // GET: /DCRApprove/

        public ActionResult Index() {
            this.GetLoggedInInfo();

            //return View(db.VIEW_TRN_DCR_APPROVE.ToList());
            ViewBag.DCR_TYPE_NO = new SelectList(db.GEN_DCR_TYPE, "DCR_TYPE_NO", "DCR_TYPE");
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "FY_NAME");
            ViewBag.USER_NO = new SelectList(db.SEC_USERS.Where(a => a.USER_TYPE_NO == 5).ToList(), "USER_NO", "USER_FULL_NAME");
            ViewBag.INST_TYPE_NO = new SelectList(db.SET_INST_TYPE, "INST_TYPE_NO", "INST_TYPE_NAME");
            ViewBag.TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "TRANS_TYPE_NAME");
            ViewBag.APPROVE_TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "TRANS_TYPE_NAME");
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", (decimal) ApproveType.Pending);
            return View();
        }

        [HttpPost]
        public ActionResult Index(DCRViewSearchModel obj) {
            this.GetLoggedInInfo();

            //var data_list = db.VIEW_TRN_DCR_APPROVE_GET(obj.DCR_NO, obj.INSERT_IS_OFFLINE, obj.DCR_TYPE_NO, obj.FY_NO, obj.USER_NO, obj.IS_REF_ZM, obj.INSTITUTE_NO, obj.INST_TYPE_NO, obj.TRANS_TYPE_NO, obj.APPROVE_TRANS_TYPE_NO, obj.APPROVE_TYPE_NO, obj.IS_MANUAL_ENTRY, obj.IS_TRN_LOCKED, obj.IS_LOCK_BY_SYSTEM, obj.TRN_DCR_DATE_FROM, obj.TRN_DCR_DATE_TO);
            List<VIEW_TRN_DCR_APPROVE_GET_Result2> data_list = db.VIEW_TRN_DCR_APPROVE_GET(obj.DCR_NO, obj.INSERT_IS_OFFLINE, obj.DCR_TYPE_NO, obj.FY_NO, obj.USER_NO, obj.IS_REF_ZM, obj.INSTITUTE_NO, obj.INST_TYPE_NO, obj.TRANS_TYPE_NO, obj.APPROVE_TRANS_TYPE_NO, obj.APPROVE_TYPE_NO, obj.IS_MANUAL_ENTRY, obj.IS_TRN_LOCKED, obj.IS_LOCK_BY_SYSTEM, obj.TRN_DCR_DATE_FROM, obj.TRN_DCR_DATE_TO, this.sess_USER_NO, obj.IS_VERIFY).ToList();
            
            ViewBag.TotalRecords = data_list.Count;
            ViewBag.page_number = 1;

            Session["sess_VIEW_TRN_DCR_APPROVE_GET_Result2"] = data_list;

            //return View("List", data_list);
            return View("List", data_list.Take(this.page_size));
        }

        public ActionResult GetPaged(int page_number) {
            List<VIEW_TRN_DCR_APPROVE_GET_Result2> data_list = Session["sess_VIEW_TRN_DCR_APPROVE_GET_Result2"] as List<VIEW_TRN_DCR_APPROVE_GET_Result2>;

            ViewBag.TotalRecords = data_list.Count;
            ViewBag.page_number = page_number;

            if (page_number == 1) {
                return View("List", data_list.Take(this.page_size));
            } else {
                return View("List", data_list.Skip((page_number * this.page_size) - 1).Take(this.page_size));
            }

        }

        public ActionResult Details(int id) {
            this.GetLoggedInInfo();

            decimal? DCR_NO = (decimal) id;
            VIEW_TRN_DCR_APPROVE_GET_Result2 dcr = db.VIEW_TRN_DCR_APPROVE_GET(DCR_NO, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, this.sess_USER_NO, null).FirstOrDefault();
            if (dcr != null) {
                dcr.VIEW_TRN_DCR_DET_APRROVE_GET_Result = db.VIEW_TRN_DCR_DET_APRROVE_GET(null, DCR_NO, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null).ToList();

                ViewBag.APPROVE_TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "TRANS_TYPE_NAME");
                ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", dcr.APPROVE_TYPE_NO);
                ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "REASON_NAME", dcr.REASON_NO);
            }

            SEC_USERS_LOGIN_Result1 user = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;

            ViewBag.USER_TYPE_NO = user.USER_TYPE_NO;

            return View(dcr);
        }

        [HttpPost]
        public JsonResult DoApprove(TRN_DCR_DO_APPROVE appr_data) {
            bool is_success = false;
            string msg = @"failed to save";            

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew)) {
                try {

                    this.GetLoggedInInfo();

                    appr_data.APPROVE_USER_NO = this.sess_USER_NO;
                    appr_data.APPROVE_LOGON_NO = this.sess_LOGON_NO;

                    db.TRN_DCR_DO_APPROVE(appr_data.DCR_NO, appr_data.APPROVE_TYPE_NO, appr_data.APPROVE_USER_NO, appr_data.APPROVE_LOGON_NO, appr_data.REASON_NO, appr_data.REAMRKS, appr_data.APPROVE_TRANS_TYPE_NO, appr_data.APPROVE_FARE_AMT);
                    scope.Complete();

                    is_success = true;
                    msg = @"saved successfully";
                } catch (DbException ex) {
                    scope.Dispose();
                    msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                } catch (EntitySqlException ex) {
                    scope.Dispose();
                    msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                } catch (DbUpdateException ex) {
                    scope.Dispose();
                    msg = msg = ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                } catch (Exception ex) {
                    scope.Dispose();
                    msg = ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                }
            }
            var result = new { @is_success = is_success, @msg = msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Verify(TRN_DCR_DO_APPROVE appr_data)
        {
            bool is_success = false;
            string msg = @"failed to save";

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {

                    this.GetLoggedInInfo();

                    decimal? USER_NO = this.sess_USER_NO;
                    decimal? LOGON_NO = this.sess_LOGON_NO;

                    db.TRN_DCR_VERIFY(appr_data.DCR_NO, 1, USER_NO, LOGON_NO,appr_data.APPROVE_TRANS_TYPE_NO,appr_data.APPROVE_FARE_AMT);
                    scope.Complete();

                    is_success = true;
                    msg = @"saved successfully";
                }
                catch (DbException ex)
                {
                    scope.Dispose();
                    msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                }
                catch (EntitySqlException ex)
                {
                    scope.Dispose();
                    msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                }
                catch (DbUpdateException ex)
                {
                    scope.Dispose();
                    msg = msg = ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    msg = ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex,sess_entry_user_name);
                }
            }
            var result = new { @is_success = is_success, @msg = msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
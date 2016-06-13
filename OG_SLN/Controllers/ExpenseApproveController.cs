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
    public class ExpenseApproveController : Controller {

        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        SEC_USERS_LOGIN_Result1 sess_sec_users = null;
        decimal? sess_USER_NO = null;
        decimal? sess_LOGON_NO = null;
        String sess_entry_user_name = null;
        private void GetLoggedInInfo() {
            this.sess_sec_users = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;
            this.sess_LOGON_NO = Session["sess_LOGON_NO"] as decimal?;
            this.sess_USER_NO = Session["sess_USER_NO"] as decimal?;
            this.sess_entry_user_name = sess_sec_users.USER_NAME;

        }

        //
        // GET: /ExpenseApprove/        

        public ActionResult Index() {
            //return View(db.VIEW_TRN_DCR_APPROVE.ToList());
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "FY_NAME");
            ViewBag.USER_NO = new SelectList(db.SEC_USERS.Where(a => a.USER_TYPE_NO == 5).ToList(), "USER_NO", "USER_FULL_NAME");
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", (decimal) ApproveType.Pending);
            return View();
        }

        [HttpPost]
        public ActionResult Index(DERViewSearchModel obj) {
            var data_list = db.VIEW_TRN_EXPENSE_APPROVE_GET(obj.EXP_NO, obj.FY_NO, obj.USER_NO, obj.APPROVE_TYPE_NO, obj.TRN_EXP_DATE_FROM, obj.TRN_EXP_DATE_TO);
            return View("List", data_list);
        }

        public ActionResult Details(int id) {
            decimal? EXP_NO = (decimal) id;
            VIEW_TRN_EXPENSE_APPROVE_GET_Result expense = db.VIEW_TRN_EXPENSE_APPROVE_GET(EXP_NO, null, null, null, null, null).FirstOrDefault();
            if (expense != null) {
                expense.VIEW_TRN_EXPENSE_DET_APPR_GET_Result = db.VIEW_TRN_EXPENSE_DET_APPR_GET(null, EXP_NO, null).ToList();

                ViewBag.APPROVE_TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "TRANS_TYPE_NAME");
                ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", expense.APPROVE_TYPE_NO);
                ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "REASON_NAME", expense.REASON_NO);
            }

            return View(expense);
            
        }

        [HttpPost]
        public JsonResult DoApprove(TRN_EXPENSE_DO_APPROVE appr_data) {
            bool is_success = false;
            string msg = @"failed to save";

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew)) {
                try {

                    this.GetLoggedInInfo();

                    appr_data.APPROVE_USER_NO = this.sess_USER_NO;
                    appr_data.APPROVE_LOGON_NO = this.sess_LOGON_NO;

                    db.TRN_EXPENSE_DO_APPROVE(appr_data.EXP_NO, appr_data.APPROVE_TYPE_NO, appr_data.APPROVE_USER_NO, appr_data.APPROVE_LOGON_NO, appr_data.REASON_NO, appr_data.REAMRKS);
                    scope.Complete();

                    is_success = true;
                    msg = @"saved successfully";
                } catch (DbException ex) {
                    scope.Dispose();
                    msg = "An Error Occurred during Saving Expense, Error Code - 999, Please Try Again."; //ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                } catch (EntitySqlException ex) {
                    scope.Dispose();
                    msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                } catch (DbUpdateException ex) {
                    scope.Dispose();
                    msg = msg = ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                } catch (Exception ex) {
                    scope.Dispose();
                    msg = ex.Message;
                    is_success = false;
                    msg = ex.Message;
                    DbErrorTracker db_error = new DbErrorTracker();
                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                }
            }
            var result = new { @is_success = is_success, @msg = msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}

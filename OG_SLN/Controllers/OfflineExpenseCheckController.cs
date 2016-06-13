using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Models;

namespace OG_SLN.Controllers
{
    public class OfflineExpenseCheckController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /OfflineExpenseCheck/

        public ActionResult Index()
        {
            SearchOfflineExpense dcrSearch = (Session["SearchOfflineExpense"] == null) ? new SearchOfflineExpense()
                : (SearchOfflineExpense)Session["SearchOfflineExpense"];
            ViewBag.dcrSearch = dcrSearch;
            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.GEN_USER_TYPES.SHORT_NAME == "ZM");
            ViewBag.fromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.toDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();
            ViewBag.verify_status = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE");
            ViewBag.ddldivision = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");
            //error in this
            IList<TRN_EXPENSE_APPROVAL_GET_Result> tmp = db.TRN_EXPENSE_APPROVAL_GET(
                null,//user
                decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                null,//division
                null,//zilla
                null,//thana
                DateTime.Now,//date from
                DateTime.Now,//date to
                0,
                0,null).ToList();
            ViewBag.expenses = tmp;

            decimal? dcr_count = db.TRN_EXPENSE_VERIFY_COUNT(decimal.Parse(Session["sess_USER_NO"].ToString())).FirstOrDefault().CNT;
            ViewBag.dcr_count = dcr_count;

            return View();
        }

        [HttpPost]
        public ActionResult Index(ExpenseDetVerifySearch search_obj)
        {
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();

            IList<TRN_EXPENSE_APPROVAL_GET_Result> tmp = db.TRN_EXPENSE_APPROVAL_GET(
                search_obj.USER_NO,//user
                decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                search_obj.ddldivision,//division
                search_obj.ddlZillas,//zilla
                search_obj.ddlThanas,//thana
                search_obj.DateFrom,//date from
                search_obj.DateTo,//date to
                0,
               search_obj.verify_status,null).ToList();
            ViewBag.expenses = tmp;

            return View("_ExpenseTable");
        }

        //
        // GET: /OfflineExpenseCheck/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            if (trn_expense == null)
            {
                return HttpNotFound();
            }
            return View(trn_expense);
        }

        //
        // GET: /OfflineExpenseCheck/Create

        public ActionResult Create()
        {
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE");
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION");
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION");
            ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "LAST_ACTION");
            return View();
        }

        //
        // POST: /OfflineExpenseCheck/Create

        [HttpPost]
        public ActionResult Create(TRN_EXPENSE trn_expense)
        {
            if (ModelState.IsValid)
            {
                db.TRN_EXPENSE.AddObject(trn_expense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", trn_expense.APPROVE_TYPE_NO);
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_expense.USER_NO);
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION", trn_expense.FY_NO);
            ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "LAST_ACTION", trn_expense.REASON_NO);
            return View(trn_expense);
        }

        //
        // GET: /OfflineExpenseCheck/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            if (trn_expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", trn_expense.APPROVE_TYPE_NO);
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_expense.USER_NO);
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION", trn_expense.FY_NO);
            ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "LAST_ACTION", trn_expense.REASON_NO);
            return View(trn_expense);
        }

        //
        // POST: /OfflineExpenseCheck/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_EXPENSE trn_expense)
        {
            if (ModelState.IsValid)
            {
                db.TRN_EXPENSE.Attach(trn_expense);
                db.ObjectStateManager.ChangeObjectState(trn_expense, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", trn_expense.APPROVE_TYPE_NO);
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_expense.USER_NO);
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION", trn_expense.FY_NO);
            ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "LAST_ACTION", trn_expense.REASON_NO);
            return View(trn_expense);
        }

        //
        // GET: /OfflineExpenseCheck/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            if (trn_expense == null)
            {
                return HttpNotFound();
            }
            return View(trn_expense);
        }

        //
        // POST: /OfflineExpenseCheck/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            db.TRN_EXPENSE.DeleteObject(trn_expense);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        /**
         * Web service call
         * */
        public JsonResult AcceptRejectExpense(decimal amt, decimal? expdetNo, decimal? type)
        {
            //string ret="";
            if (expdetNo != null)
                db.TRN_EXPENSE_VERIFY(expdetNo, type, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), amt);
            return Json(new { output = (expdetNo != null) ? ((type == 1) ? "Accepted" : "Rejected") : "Rejected" }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
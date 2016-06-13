using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using OG_SLN.Models;
namespace OG_SLN.Controllers
{
    public class ExpenseController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Expense/
        public Tuple<SearchExpense, IQueryable<TRN_EXPENSE>> setSearch(IQueryable<TRN_EXPENSE> dcrs, SearchExpense dcrSearch, bool search)
        {
            dcrSearch = (dcrSearch.dirty) ? dcrSearch : new SearchExpense();
            if (!string.IsNullOrEmpty(Request.QueryString["USER_NO"]) || search)
            {
                dcrSearch.userNo = (!search) ? decimal.Parse(Request.QueryString["USER_NO"].ToString()) : dcrSearch.userNo;
                if (!string.IsNullOrEmpty(dcrSearch.userNo.ToString()))
                    dcrs = dcrs.Where(s => s.SEC_USERS.USER_NO==dcrSearch.userNo);
            }
            else
                dcrSearch.userNo = null;
            if (!string.IsNullOrEmpty(Request.QueryString["userName"]) || search)
            {

                dcrSearch.userName = (!search) ? Request.QueryString["userName"] : dcrSearch.userName;
                if (!string.IsNullOrEmpty(dcrSearch.userName))
                    dcrs = dcrs.Where(s => s.SEC_USERS.USER_NAME.Contains(dcrSearch.userName));
            }
            else
                dcrSearch.userName = "";
            if (!string.IsNullOrEmpty(Request.QueryString["userContact"]) || search)
            {
                dcrSearch.userContact = (!search) ? Request.QueryString["userContact"] : dcrSearch.userContact;
                if (!string.IsNullOrEmpty(dcrSearch.userContact))
                    dcrs = dcrs.Where(s => s.SEC_USERS.USER_CONTACT.Contains(dcrSearch.userContact));
            }
            else
                dcrSearch.userContact = "";
            if (!string.IsNullOrEmpty(Request.QueryString["dateExpFrom"]) || search)
            {
                dcrSearch.dateExpFrom = (!search) ? DateTime.Parse(Request.QueryString["dateExpFrom"]) : dcrSearch.dateExpFrom;
                dcrs = dcrs.Where(s => s.INSERT_TIME >= dcrSearch.dateExpFrom);
            }
            else
                dcrSearch.dateExpFrom = null;
            if (!string.IsNullOrEmpty(Request.QueryString["dateExpTo"]) || search)
            {
                dcrSearch.dateExpTo = (!search) ? DateTime.Parse(Request.QueryString["dateExpTo"]) : dcrSearch.dateExpTo;
                dcrs = dcrs.Where(s => s.INSERT_TIME <= dcrSearch.dateExpTo);
            }
            else
                dcrSearch.dateExpTo = null;
            dcrSearch.dirty = true;
            return new Tuple<SearchExpense, IQueryable<TRN_EXPENSE>>(dcrSearch, dcrs);
        }
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;
            IPagedList<TRN_EXPENSE> pagedIns = null;
            IQueryable<TRN_EXPENSE> expenses = db.TRN_EXPENSE.AsQueryable();
            SearchExpense dcrSearch = (Session["EXPENSESearch"] == null) ? new SearchExpense()
                : (SearchExpense)Session["EXPENSESearch"];

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                Tuple<SearchExpense, IQueryable<TRN_EXPENSE>> tmp = setSearch(expenses, dcrSearch, false);
                expenses = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && dcrSearch.dirty)
            {
                dcrSearch = (SearchExpense)Session["EXPENSESearch"];
                Tuple<SearchExpense, IQueryable<TRN_EXPENSE>> tmp = setSearch(expenses, dcrSearch, true);
                expenses = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && Session["EXPENSESearch"] == null)
            {
                dcrSearch = new SearchExpense();
            }
            Session["EXPENSESearch"] = dcrSearch;
            pagedIns = expenses.OrderByDescending(m => m.INSERT_TIME).ToPagedList(pageIndex, pageSize);
            ViewBag.expSearch = dcrSearch;
            ViewBag.USER_NO = new SelectList(db.SEC_USERS.Where(a => a.USER_TYPE_NO == 
                (decimal)EUserTypes.ZonalManager)
                .ToList(), "USER_NO", "USER_FULL_NAME");
            return View(pagedIns);
        }

        //
        // GET: /Expense/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            if (trn_expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.jsmsg = "<script type='text/javascript'>$(document).ready(function () {setEdit(" + id + ");}); </script>";
            return View(trn_expense);
        }

        //
        // GET: /Expense/Create

        public ActionResult Create()
        {
            ViewBag.Users = db.SEC_USERS.ToList();
            ViewBag.FYs = db.SET_FISCAL_YEAR.ToList();
            ViewBag.ExpenseTypes = db.SET_EXP_TYPE.ToList();
            return View();
        }

        //
        // POST: /Expense/Create

        [HttpPost]
        public ActionResult Create(TRN_EXPENSE trn_expense)
        {
            if (ModelState.IsValid)
            {
                decimal id = db.TRN_EXPENSE_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                        DateTime.Now,
                        null, 1, trn_expense.USER_NO,
                        null, trn_expense.TRN_EXP_DATE, 1, null).FirstOrDefault().Value;
                List<string> lstTmp = new List<string>();
                foreach (string curSession in Session)
                {
                    if (curSession.Contains("ExpenseEntry"))
                    {
                        lstTmp.Add(curSession);
                        temp_TRN_EXPENSE_DET temp_sess_det = (temp_TRN_EXPENSE_DET)Session[curSession];
                        if (!temp_sess_det.set_deleted)
                        {
                            db.TRN_EXPENSE_DET_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                                DateTime.Now, null, 1, id, temp_sess_det.exp_type, temp_sess_det.amount,
                                null, null, null, null, temp_sess_det.vendor, temp_sess_det.comments);
                        }
                    }
                }
                foreach (string del in lstTmp)
                    Session.Remove(del);
                return RedirectToAction("Index");
            }

            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", trn_expense.APPROVE_TYPE_NO);
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_expense.USER_NO);
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION", trn_expense.FY_NO);
            ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "LAST_ACTION", trn_expense.REASON_NO);
            return View(trn_expense);
        }

        //
        // GET: /Expense/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            if (trn_expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.Users = db.SEC_USERS.ToList();
            ViewBag.FYs = db.SET_FISCAL_YEAR.ToList();
            ViewBag.ExpenseTypes = db.SET_EXP_TYPE.ToList();
            return View(trn_expense);
        }

        //
        // POST: /Expense/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_EXPENSE trn_expense)
        {
            if (ModelState.IsValid)
            {
                db.TRN_EXPENSE_UPDATE(trn_expense.EXP_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()), DateTime.Now,
                                null, 1, trn_expense.USER_NO, trn_expense.AGENT_USER_NO, trn_expense.TRN_EXP_DATE,
                                trn_expense.APPROVE_TYPE_NO, trn_expense.APPROVE_TIME, trn_expense.APPROVE_USER_NO,
                                trn_expense.APPROVE_LOGON_NO, trn_expense.REASON_NO, trn_expense.REAMRKS,
                                trn_expense.IS_TRN_LOCKED, trn_expense.IS_LOCK_BY_SYSTEM,
                                trn_expense.TRN_LOCK_DATE, trn_expense.TRN_LOCK_USER_NO,
                                trn_expense.TRN_LOCK_LOGON_NO, trn_expense.IS_MANUAL_ENTRY, trn_expense.OFFLINE_EXP_NO);
                List<string> lstTmp = new List<string>();
                foreach (string curSession in Session)
                {
                    if (curSession.Contains("ExpenseEntry"))
                    {
                        lstTmp.Add(curSession);
                        temp_TRN_EXPENSE_DET temp_sess_det = (temp_TRN_EXPENSE_DET)Session[curSession];
                        if (!temp_sess_det.set_deleted)
                        {
                            TRN_EXPENSE_DET dt = db.TRN_EXPENSE_DET.Where(s => s.EXP_DET_NO == temp_sess_det.editID).FirstOrDefault();
                            if(dt!=null)
                                db.TRN_EXPENSE_DET_UPDATE(Convert.ToDecimal(temp_sess_det.editID), decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()), DateTime.Now, null, 1,
                                trn_expense.EXP_NO, temp_sess_det.exp_type, temp_sess_det.amount, dt.APPROVE_EXP_TYPE_NO,
                                dt.APPROVE_EXP_AMT, dt.APPROVE_TYPE_NO, dt.APPROVE_TIME, dt.APPROVE_USER_NO, dt.APPROVE_LOGON_NO,
                                dt.REASON_NO, dt.REAMRKS, dt.OFFLINE_EXP_NO, dt.OFFLINE_EXP_DET_NO, dt.LAT_VAL, dt.LON_VAL, temp_sess_det.vendor,
                                temp_sess_det.comments);
                            else
                                db.TRN_EXPENSE_DET_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                                DateTime.Now, null, 1, trn_expense.EXP_NO, temp_sess_det.exp_type, temp_sess_det.amount,
                                null, null, null, null, temp_sess_det.vendor, temp_sess_det.comments);
                        }
                        else{
                            TRN_EXPENSE_DET dt = db.TRN_EXPENSE_DET.Where(s => s.EXP_DET_NO == temp_sess_det.editID).FirstOrDefault();
                            db.TRN_EXPENSE_DET_DELETE(dt.EXP_DET_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()));
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", trn_expense.APPROVE_TYPE_NO);
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_expense.USER_NO);
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION", trn_expense.FY_NO);
            ViewBag.REASON_NO = new SelectList(db.SET_REASON, "REASON_NO", "LAST_ACTION", trn_expense.REASON_NO);
            return View(trn_expense);
        }

        //
        // GET: /Expense/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            if (trn_expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.jsmsg = "<script type='text/javascript'>$(document).ready(function () {setEdit(" + id + ");}); </script>";
            return View(trn_expense);
        }

        //
        // POST: /Expense/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_EXPENSE trn_expense = db.TRN_EXPENSE.Single(t => t.EXP_NO == id);
            IList<TRN_EXPENSE_DET> detList = db.TRN_EXPENSE_DET.Where(x => x.EXP_NO == trn_expense.EXP_NO).ToList();
            foreach(TRN_EXPENSE_DET dt in detList)
                db.TRN_EXPENSE_DET_DELETE(dt.EXP_DET_NO,decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()));
            db.TRN_EXPENSE_DELETE(trn_expense.EXP_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()));

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        
        /**Web service part
         * */
        public JsonResult RemoveExpenseFromSession(string test)
        {
            temp_TRN_EXPENSE_DET tmpdet = (temp_TRN_EXPENSE_DET)Session["ExpenseEntry" + test];
            tmpdet.set_deleted = true;
            Session["ExpenseEntry" + test] = tmpdet;
            return Json(new
            {
                type = tmpdet.exp_type,
                amnt = tmpdet.amount,
                vendor = tmpdet.vendor,
                coment = tmpdet.comments,
                editid = tmpdet.editID
            });
        }
        public JsonResult GetExpenseFromSession(string test)
        {
            temp_TRN_EXPENSE_DET tmpdet = (temp_TRN_EXPENSE_DET)Session["ExpenseEntry" + test];
            SET_EXP_TYPE exptype = db.SET_EXP_TYPE.Where(s => s.EXP_TYPE_NO == tmpdet.exp_type).First();
            
            return Json(new
            {
                
                type = exptype.EXP_TYPE_NAME,
                amnt = tmpdet.amount,
                vendor = tmpdet.vendor,
                coment = tmpdet.comments,
                editid = tmpdet.editID
            });
        }
        public JsonResult SaveExpenseOnSession(decimal ExpenseTypes, decimal txtamnt, string txtVendor, string txtComments,string sessName)
        {
            if (sessName == "")
                if (Session["ExpenseEntry" + ExpenseTypes + "-" + txtamnt + "-" + txtVendor.Replace(" ","")] == null)
                    Session["ExpenseEntry" + ExpenseTypes + "-" + txtamnt + "-" + txtVendor.Replace(" ","")] = new temp_TRN_EXPENSE_DET(ExpenseTypes, txtamnt, txtVendor, txtComments);
                else
                {
                    temp_TRN_EXPENSE_DET tmp = (temp_TRN_EXPENSE_DET)Session["ExpenseEntry" + ExpenseTypes + "-" + txtamnt + "-" + txtVendor.Replace(" ","")];
                    tmp.set_deleted = true;
                    Session["ExpenseEntry" + ExpenseTypes + "-" + txtamnt + "-" + txtVendor.Replace(" ","")] = tmp;
                }
            else
            {
                temp_TRN_EXPENSE_DET tmp = (temp_TRN_EXPENSE_DET)Session["ExpenseEntry" + sessName];
                tmp.set_deleted = true;
                Session["ExpenseEntry" + ExpenseTypes + "-" + txtamnt + "-" + txtVendor.Replace(" ", "")] = new temp_TRN_EXPENSE_DET(ExpenseTypes, txtamnt, txtVendor, txtComments); 
            }
            return Json(new { dt = ExpenseTypes + "-" + txtamnt });
        }
        
        public JsonResult getPreviousDetails(decimal id)
        {
            List<string> lstTmp = new List<string>();
            foreach (string curSession in Session)
                if (curSession.Contains("ExpenseEntry"))
                    lstTmp.Add(curSession);
            foreach (string del in lstTmp)
                Session.Remove(del);
            string ret = "";
            IList<TRN_EXPENSE_DET> exp_det_list = db.TRN_EXPENSE_DET.Where(s => s.EXP_NO == id).ToList();
            foreach (TRN_EXPENSE_DET dt in exp_det_list)
            {
                ret += "<tr  id='" + dt.EXP_TYPE_NO + "-" + dt.EXP_AMT + "-" + dt.VENDOR.Replace(" ", "") + "' tmp='" + dt.EXP_DET_NO + "'><td>" + dt.SET_EXP_TYPE.EXP_TYPE_NAME +
                    "</td><td>" + dt.EXP_AMT + "</td><td>" + dt.VENDOR + "</td><td>" +
                    "<input type=\"button\" value=\"Edit\" class=\"btn btn-small btn-warning\" id=\"btnEdit\" onclick=\"setforEdit($(this).closest('tr').attr('id'))\" />" +
                    "<input type=\"button\" value=\"Remove\" class=\"btn btn-small btn-danger\" id=\"btnRemove\" onclick = \"setforDelete($(this).closest('tr').attr('id'))\" />" + "</td></tr>";
                Session["ExpenseEntry" + dt.EXP_TYPE_NO + "-" + dt.EXP_AMT + "-" + dt.VENDOR.Replace(" ","")] = new temp_TRN_EXPENSE_DET(dt.EXP_TYPE_NO, dt.EXP_AMT, dt.VENDOR, dt.COMMENTS, dt.EXP_DET_NO);
            }

            return Json(new { html = ret });
        }
        public JsonResult getPreviousDetailsWithoutButton(decimal id)
        {
           
            string ret = "";
            IList<TRN_EXPENSE_DET> exp_det_list = db.TRN_EXPENSE_DET.Where(s => s.EXP_NO == id).ToList();
            foreach (TRN_EXPENSE_DET dt in exp_det_list)
                ret += "<tr  id='" + dt.EXP_TYPE_NO + "-" + dt.EXP_AMT + "-" + dt.VENDOR.Replace(" ", "") + "' tmp='" + dt.EXP_DET_NO + "'><td>" + dt.SET_EXP_TYPE.EXP_TYPE_NAME +
                    "</td><td>" + dt.EXP_AMT + "</td><td>" + dt.VENDOR + "</td><td></td></tr>";
                

            return Json(new { html = ret });
        }

    }
}
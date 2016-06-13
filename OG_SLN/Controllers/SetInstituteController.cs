using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace OG_SLN.Controllers
{
    public class SetInstituteController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        public Tuple<InsituteSearch, IQueryable<SET_INSTITUTE>> setSearch(IQueryable<SET_INSTITUTE> dcrs, InsituteSearch dcrSearch, bool search)
        {
            dcrSearch = (dcrSearch.dirty) ? dcrSearch : new InsituteSearch();
            if (!string.IsNullOrEmpty(Request.QueryString["insName"]) || search)
            {
                dcrSearch.insName = (!search) ? Request.QueryString["insName"] : dcrSearch.insName;
                if (!string.IsNullOrEmpty(dcrSearch.insName))
                    dcrs = dcrs.Where(s => s.INSTITUTE_NAME.Contains(dcrSearch.insName));
            }
            else
                dcrSearch.insName = "";
            if (!string.IsNullOrEmpty(Request.QueryString["F_INSTITUTION_DB_ID"]) || search)
            {
                dcrSearch.F_INSTITUTION_DB_ID = (!search) ? Request.QueryString["F_INSTITUTION_DB_ID"] : dcrSearch.F_INSTITUTION_DB_ID;
                if (!string.IsNullOrEmpty(dcrSearch.F_INSTITUTION_DB_ID))
                    dcrs = dcrs.Where(s => s.F_INSTITUTION_DB_ID.Contains(dcrSearch.F_INSTITUTION_DB_ID));
            }
            else
                dcrSearch.F_INSTITUTION_DB_ID = "";
            if (!string.IsNullOrEmpty(Request.QueryString["insID"]) || search)
            {
                dcrSearch.insID = (!search) ? decimal.Parse(Request.QueryString["insID"]) : dcrSearch.insID;
                if (dcrSearch.insID != 0 || dcrSearch.insID !=null)
                    dcrs = dcrs.Where(s => s.INSTITUTE_NO == (dcrSearch.insID));
            }
            else
                dcrSearch.insID = null;
            if (!string.IsNullOrEmpty(Request.QueryString["EIIN_NUMBER"]) || search)
            {
                dcrSearch.EIIN_NUMBER = (!search) ? Request.QueryString["EIIN_NUMBER"] : dcrSearch.EIIN_NUMBER;
                if (!string.IsNullOrEmpty(dcrSearch.EIIN_NUMBER)) 
                    dcrs = dcrs.Where(s => s.EIIN_NUMBER.Contains(dcrSearch.EIIN_NUMBER));
            }
            else
                dcrSearch.EIIN_NUMBER = "";
            
            dcrSearch.dirty = true;
            return new Tuple<InsituteSearch, IQueryable<SET_INSTITUTE>>(dcrSearch, dcrs);
        }
        //
        // GET: /SetInstitute/

        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;
            IPagedList<SET_INSTITUTE> pagedIns = null;
            IQueryable<SET_INSTITUTE> institutes = db.SET_INSTITUTE.AsQueryable();
            InsituteSearch dcrSearch = (Session["INSSearch"] == null) ? new InsituteSearch()
                : (InsituteSearch)Session["INSSearch"];

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                Tuple<InsituteSearch, IQueryable<SET_INSTITUTE>> tmp = setSearch(institutes, dcrSearch, false);
                institutes = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && dcrSearch.dirty)
            {
                dcrSearch = (InsituteSearch)Session["INSSearch"];
                Tuple<InsituteSearch, IQueryable<SET_INSTITUTE>> tmp = setSearch(institutes, dcrSearch, true);
                institutes = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && Session["INSSearch"] == null)
            {
                dcrSearch = new InsituteSearch();
            }
            Session["INSSearch"] = dcrSearch;
            //pagedIns = institutes.OrderByDescending(m => m.INSERT_TIME).ToPagedList(pageIndex, pageSize);
            pagedIns = institutes.OrderByDescending(m => m.INSTITUTE_NO).ToPagedList(pageIndex, pageSize);
            ViewBag.insSearch = dcrSearch; 
            return View(pagedIns);
        }

        //
        // GET: /SetInstitute/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_INSTITUTE set_institute = db.SET_INSTITUTE.Single(s => s.INSTITUTE_NO == id);
            if (set_institute == null)
            {
                return HttpNotFound();
            }
            return View(set_institute);
        }

        //
        // GET: /SetInstitute/Create

        public ActionResult Create()
        {
            ViewBag.ins = db.SET_INST_TYPE.ToList();
            ViewBag.Thanas =db.SET_ZILLA.OrderBy(x=>x.ZILLA_NAME).ToList();
            return View();
        }

        //
        // POST: /SetInstitute/Create

        [HttpPost]
        public ActionResult Create(SET_INSTITUTE set_institute)
        {
            if (ModelState.IsValid)
            {
                List<SET_INSTITUTE> search = db.SET_INSTITUTE.Where(s=>s.THANA_NO==set_institute.THANA_NO
                    && s.INST_TYPE_NO==set_institute.INST_TYPE_NO
                    && s.INSTITUTE_NAME==set_institute.INSTITUTE_NAME).ToList();
                if (search.Count==0)
                {
                    db.SET_INSTITUTE_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                            decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                            set_institute.INSTITUTE_NAME,
                            set_institute.INSTITUTE_NAME_BNG,
                            set_institute.INSTITUTE_DESC,
                            set_institute.INST_TYPE_NO,
                            set_institute.THANA_NO,
                            set_institute.F_INSTITUTION_DB_ID,
                            set_institute.EIIN_NUMBER,
                            set_institute.MEET_PERSON_NAME,
                            set_institute.CONTACT_NUMBER,
                            set_institute.ACTUAL_LATI_VAL,
                            set_institute.ACTUAL_LONG_VAL, set_institute.FROM_LATI_VAL, set_institute.FROM_LONG_VAL,
                            set_institute.TO_LATI_VAL, set_institute.TO_LONG_VAL, set_institute.IS_ACTIVE,
                            set_institute.ACTIVE_FROM, set_institute.ACTIVE_TO, set_institute.SL_NUM);
                }
                return RedirectToAction("Index");
            }

            return View(set_institute);
        }

        //
        // GET: /SetInstitute/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_INSTITUTE set_institute = db.SET_INSTITUTE.Single(s => s.INSTITUTE_NO == id);
            ViewBag.ins = db.SET_INST_TYPE.ToList();
            ViewBag.Thanas = db.SET_ZILLA.OrderBy(x => x.ZILLA_NAME).ToList();
            if (set_institute == null)
            {
                return HttpNotFound();
            }
            return View(set_institute);
        }

        //
        // POST: /SetInstitute/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_INSTITUTE set_institute)
        {
            if (ModelState.IsValid)
            {
                db.SET_INSTITUTE_UPDATE(set_institute.INSTITUTE_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), set_institute.INSTITUTE_NAME,
                        set_institute.INSTITUTE_NAME_BNG, set_institute.INSTITUTE_DESC,
                        set_institute.INST_TYPE_NO, set_institute.THANA_NO,
                        set_institute.F_INSTITUTION_DB_ID, set_institute.EIIN_NUMBER,
                        set_institute.MEET_PERSON_NAME, set_institute.CONTACT_NUMBER,
                        set_institute.ACTUAL_LATI_VAL, set_institute.ACTUAL_LONG_VAL,
                        set_institute.FROM_LATI_VAL, set_institute.FROM_LONG_VAL,
                        set_institute.TO_LATI_VAL, set_institute.TO_LONG_VAL,
                        set_institute.IS_ACTIVE, set_institute.ACTIVE_FROM, set_institute.ACTIVE_TO,
                        set_institute.SL_NUM);
                return RedirectToAction("Index");
            }
            return View(set_institute);
        }

        //
        // GET: /SetInstitute/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_INSTITUTE set_institute = db.SET_INSTITUTE.Single(s => s.INSTITUTE_NO == id);
            if (set_institute == null)
            {
                return HttpNotFound();
            }
            return View(set_institute);
        }

        //
        // POST: /SetInstitute/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_INSTITUTE set_institute = db.SET_INSTITUTE.Single(s => s.INSTITUTE_NO == id);
            db.SET_INSTITUTE.DeleteObject(set_institute);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        /**
         * get selected zilla's thana
         * */
        public JsonResult getThana(decimal zillaNo)
        {
            if (zillaNo != 0)
            {
                string ret ="";
                IList<SET_THANA> thanas = db.SET_THANA.Where(x => x.ZILLA_NO == zillaNo).OrderBy(x=>x.THANA_NAME).ToList();
                foreach (SET_THANA th in thanas)
                    ret += "<option value='"+th.THANA_NO+"'>"+th.THANA_NAME+"</option>";
                return Json(new { html = ret }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { html = "" }, JsonRequestBehavior.AllowGet);
           
        }
    }
}
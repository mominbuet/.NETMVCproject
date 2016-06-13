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
    public class SpecimenController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Specimen/
        public Tuple<SpecimenSearch, IQueryable<SET_SPECIMEN>> setSearch(IQueryable<SET_SPECIMEN> dcrs, SpecimenSearch specSearch, bool search)
        {
            specSearch = (specSearch.dirty) ? specSearch : new SpecimenSearch();
            if (!string.IsNullOrEmpty(Request.QueryString["sno"]) || search)
            {
                specSearch.sno = (!search) ? decimal.Parse(Request.QueryString["sno"]) : specSearch.sno;
                if (specSearch.sno.HasValue)
                    dcrs = dcrs.Where(s => s.SPECIMEN_NO == specSearch.sno);
            }
            else
                specSearch.sno = null;
            if (!string.IsNullOrEmpty(Request.QueryString["sname"]) || search)
            {
                specSearch.sname = (!search) ? Request.QueryString["sname"] : specSearch.sname;
                if (!string.IsNullOrEmpty(specSearch.sname))
                    dcrs = dcrs.Where(s => s.SPECIMEN_NAME.Contains(specSearch.sname));
            }
            else
                specSearch.sname = "";
            if (!string.IsNullOrEmpty(Request.QueryString["scode"]) || search)
            {
                specSearch.scode = (!search) ? Request.QueryString["scode"] : specSearch.scode;
                if (!string.IsNullOrEmpty(specSearch.scode))
                    dcrs = dcrs.Where(s => s.SPECIMEN_CODE.Contains(specSearch.scode));
            }
            else
                specSearch.scode = "";
            if (!string.IsNullOrEmpty(Request.QueryString["activeTo"]) || search)
            {
                specSearch.activeTo = (!search) ? DateTime.Parse( Request.QueryString["activeTo"]) : specSearch.activeTo;
                if (specSearch.activeTo!=null)
                    dcrs = dcrs.Where(s => s.INSERT_TIME >= specSearch.activeTo);
            }
            else
                specSearch.activeTo = null;
            if (!string.IsNullOrEmpty(Request.QueryString["activeFrom"]) || search)
            {
                specSearch.activeFrom = (!search) ? DateTime.Parse(Request.QueryString["activeFrom"]) : specSearch.activeFrom;
                if (specSearch.activeFrom != null)
                    dcrs = dcrs.Where(s => s.INSERT_TIME <= specSearch.activeFrom);
            }
            else
                specSearch.activeFrom = null;
            specSearch.dirty = true;
            return new Tuple<SpecimenSearch, IQueryable<SET_SPECIMEN>>(specSearch, dcrs);
        }
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;
            IPagedList<SET_SPECIMEN> pagedIns = null;
            IQueryable<SET_SPECIMEN> specs = db.SET_SPECIMEN.AsQueryable();
            SpecimenSearch dcrSearch = (Session["SPECSearch"] == null) ? new SpecimenSearch()
                : (SpecimenSearch)Session["SPECSearch"];

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                Tuple<SpecimenSearch, IQueryable<SET_SPECIMEN>> tmp = setSearch(specs, dcrSearch, false);
                specs = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && dcrSearch.dirty)
            {
                dcrSearch = (SpecimenSearch)Session["SPECSearch"];
                Tuple<SpecimenSearch, IQueryable<SET_SPECIMEN>> tmp = setSearch(specs, dcrSearch, true);
                specs = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && Session["SPECSearch"] == null)
            {
                dcrSearch = new SpecimenSearch();
            }
            Session["SPECSearch"] = dcrSearch;
            pagedIns = specs.OrderByDescending(m => m.SPECIMEN_NO).ToPagedList(pageIndex, pageSize);
            ViewBag.insSearch = dcrSearch;
            
            return View(pagedIns);
        }

        //
        // GET: /Specimen/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_SPECIMEN set_specimen = db.SET_SPECIMEN.Single(s => s.SPECIMEN_NO == id);
            if (set_specimen == null)
            {
                return HttpNotFound();
            }
            return View(set_specimen);
        }

        //
        // GET: /Specimen/Create

        public ActionResult Create()
        {
            ViewBag.BOOK_TYPE_NO = new SelectList(db.SET_BOOK_TYPE, "BOOK_TYPE_NO", "BOOK_TYPE_NAME");
            return View();
        }

        //
        // POST: /Specimen/Create

        [HttpPost]
        public ActionResult Create(SET_SPECIMEN set_specimen)
        {
            if (ModelState.IsValid)
            {
                db.SET_SPECIMEN_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), set_specimen.SPECIMEN_NAME,
                        set_specimen.SPECIMEN_CODE, set_specimen.SPECIMEN_NAME_BNG,
                        set_specimen.SPECIMEN_DESC, set_specimen.BOOK_TYPE_NO,
                        set_specimen.BOOK_UNIQUE_CODE, set_specimen.IS_ACTIVE,
                        set_specimen.ACTIVE_FROM, set_specimen.ACTIVE_TO,
                        set_specimen.SL_NUM);
                return RedirectToAction("Index");
            }

            ViewBag.BOOK_TYPE_NO = new SelectList(db.SET_BOOK_TYPE, "BOOK_TYPE_NO", "BOOK_TYPE_NAME", set_specimen.BOOK_TYPE_NO);
            return View(set_specimen);
        }

        //
        // GET: /Specimen/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_SPECIMEN set_specimen = db.SET_SPECIMEN.Single(s => s.SPECIMEN_NO == id);
            if (set_specimen == null)
            {
                return HttpNotFound();
            }
            ViewBag.BOOK_TYPE_NO = new SelectList(db.SET_BOOK_TYPE, "BOOK_TYPE_NO", "BOOK_TYPE_NAME", set_specimen.BOOK_TYPE_NO);
            return View(set_specimen);
        }

        //
        // POST: /Specimen/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_SPECIMEN set_specimen)
        {
            if (ModelState.IsValid)
            {
                db.SET_SPECIMEN_UPDATE(set_specimen.SPECIMEN_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), set_specimen.SPECIMEN_NAME,
                        set_specimen.SPECIMEN_CODE, set_specimen.SPECIMEN_NAME_BNG, set_specimen.SPECIMEN_DESC,
                        set_specimen.BOOK_TYPE_NO, set_specimen.BOOK_UNIQUE_CODE, set_specimen.POSITION_ID,
                        set_specimen.IS_ACTIVE, set_specimen.ACTIVE_FROM, set_specimen.ACTIVE_TO,
                        set_specimen.SL_NUM);
                return RedirectToAction("Index");
            }
            ViewBag.BOOK_TYPE_NO = new SelectList(db.SET_BOOK_TYPE, "BOOK_TYPE_NO", "BOOK_TYPE_NAME", set_specimen.BOOK_TYPE_NO);
            return View(set_specimen);
        }

        //
        // GET: /Specimen/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_SPECIMEN set_specimen = db.SET_SPECIMEN.Single(s => s.SPECIMEN_NO == id);
            if (set_specimen == null)
            {
                return HttpNotFound();
            }
            return View(set_specimen);
        }

        //
        // POST: /Specimen/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_SPECIMEN set_specimen = db.SET_SPECIMEN.Single(s => s.SPECIMEN_NO == id);
            db.SET_SPECIMEN_DELETE(set_specimen.SPECIMEN_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
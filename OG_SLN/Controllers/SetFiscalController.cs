using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class SetFiscalController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SetFiscal/

        public ActionResult Index()
        {
            return View(db.SET_FISCAL_YEAR.ToList());
        }

        //
        // GET: /SetFiscal/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_FISCAL_YEAR set_fiscal_year = db.SET_FISCAL_YEAR.Single(s => s.FY_NO == id);
            if (set_fiscal_year == null)
            {
                return HttpNotFound();
            }
            return View(set_fiscal_year);
        }

        //
        // GET: /SetFiscal/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SetFiscal/Create

        [HttpPost]
        public ActionResult Create(SET_FISCAL_YEAR set_fiscal_year)
        {
            if (ModelState.IsValid)
            {
                db.SET_FISCAL_YEAR.AddObject(set_fiscal_year);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(set_fiscal_year);
        }

        //
        // GET: /SetFiscal/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_FISCAL_YEAR set_fiscal_year = db.SET_FISCAL_YEAR.Single(s => s.FY_NO == id);
            if (set_fiscal_year == null)
            {
                return HttpNotFound();
            }
            return View(set_fiscal_year);
        }

        //
        // POST: /SetFiscal/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_FISCAL_YEAR set_fiscal_year)
        {
            if (ModelState.IsValid)
            {
                db.SET_FISCAL_YEAR.Attach(set_fiscal_year);
                db.ObjectStateManager.ChangeObjectState(set_fiscal_year, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(set_fiscal_year);
        }

        //
        // GET: /SetFiscal/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_FISCAL_YEAR set_fiscal_year = db.SET_FISCAL_YEAR.Single(s => s.FY_NO == id);
            if (set_fiscal_year == null)
            {
                return HttpNotFound();
            }
            return View(set_fiscal_year);
        }

        //
        // POST: /SetFiscal/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_FISCAL_YEAR set_fiscal_year = db.SET_FISCAL_YEAR.Single(s => s.FY_NO == id);
            db.SET_FISCAL_YEAR.DeleteObject(set_fiscal_year);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
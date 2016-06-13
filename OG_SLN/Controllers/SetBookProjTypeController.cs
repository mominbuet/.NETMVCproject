using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class SetBookProjTypeController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SetBookProjType/

        public ActionResult Index()
        {
            var set_book_proj_type = db.SET_BOOK_PROJ_TYPE.Include("SET_BOOK_BRAND_TYPE");
            return View(set_book_proj_type.ToList());
        }

        //
        // GET: /SetBookProjType/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_BOOK_PROJ_TYPE set_book_proj_type = db.SET_BOOK_PROJ_TYPE.Single(s => s.BOOK_PROJ_TYPE_NO == id);
            if (set_book_proj_type == null)
            {
                return HttpNotFound();
            }
            DateTime currentDate = DateTime.Now;
            
            Console.WriteLine( currentDate.Ticks);
            Console.WriteLine( currentDate);
            long ret = ( currentDate.Ticks);
            DateTime currentDate2 = new DateTime(ret);

            return View(set_book_proj_type);
        }

        //
        // GET: /SetBookProjType/Create

        public ActionResult Create()
        {
            ViewBag.brandType = db.SET_BOOK_BRAND_TYPE.ToList();
            return View();
        }

        //
        // POST: /SetBookProjType/Create

        [HttpPost]
        public ActionResult Create(SET_BOOK_PROJ_TYPE set_book_proj_type)
        {
            if (ModelState.IsValid)
            {
                db.SET_BOOK_PROJ_TYPE.AddObject(set_book_proj_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BOOK_BRAND_TYPE_NO = new SelectList(db.SET_BOOK_BRAND_TYPE, "BOOK_BRAND_TYPE_NO", "LAST_ACTION", set_book_proj_type.BOOK_BRAND_TYPE_NO);
            return View(set_book_proj_type);
        }

        //
        // GET: /SetBookProjType/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_BOOK_PROJ_TYPE set_book_proj_type = db.SET_BOOK_PROJ_TYPE.Single(s => s.BOOK_PROJ_TYPE_NO == id);
            if (set_book_proj_type == null)
            {
                return HttpNotFound();
            }
            ViewBag.brandType = db.SET_BOOK_BRAND_TYPE.ToList();
            return View(set_book_proj_type);
        }

        //
        // POST: /SetBookProjType/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_BOOK_PROJ_TYPE set_book_proj_type)
        {
            if (ModelState.IsValid)
            {
                db.SET_BOOK_PROJ_TYPE.Attach(set_book_proj_type);
                db.ObjectStateManager.ChangeObjectState(set_book_proj_type, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BOOK_BRAND_TYPE_NO = new SelectList(db.SET_BOOK_BRAND_TYPE, "BOOK_BRAND_TYPE_NO", "LAST_ACTION", set_book_proj_type.BOOK_BRAND_TYPE_NO);
            return View(set_book_proj_type);
        }

        //
        // GET: /SetBookProjType/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_BOOK_PROJ_TYPE set_book_proj_type = db.SET_BOOK_PROJ_TYPE.Single(s => s.BOOK_PROJ_TYPE_NO == id);
            if (set_book_proj_type == null)
            {
                return HttpNotFound();
            }
            return View(set_book_proj_type);
        }

        //
        // POST: /SetBookProjType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_BOOK_PROJ_TYPE set_book_proj_type = db.SET_BOOK_PROJ_TYPE.Single(s => s.BOOK_PROJ_TYPE_NO == id);
            db.SET_BOOK_PROJ_TYPE.DeleteObject(set_book_proj_type);
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
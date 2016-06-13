using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class SET_BOOK_TYPEController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SET_BOOK_TYPE/

        public ActionResult Index()
        {
            var set_book_type = db.SET_BOOK_TYPE.Include("SET_BOOK_GROUP");
            return View(set_book_type.ToList());
        }

        //
        // GET: /SET_BOOK_TYPE/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_BOOK_TYPE set_book_type = db.SET_BOOK_TYPE.Single(s => s.BOOK_TYPE_NO == id);
            if (set_book_type == null)
            {
                return HttpNotFound();
            }
            return View(set_book_type);
        }

        //
        // GET: /SET_BOOK_TYPE/Create

        public ActionResult Create()
        {
            ViewBag.BOOK_GROUP_NO = db.SET_BOOK_GROUP.ToList();
            return View();
        }

        //
        // POST: /SET_BOOK_TYPE/Create

        [HttpPost]
        public ActionResult Create(SET_BOOK_TYPE set_book_type)
        {
            if (ModelState.IsValid)
            {
                db.SET_BOOK_TYPE.AddObject(set_book_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BOOK_GROUP_NO = new SelectList(db.SET_BOOK_GROUP, "BOOK_GROUP_NO", "LAST_ACTION", set_book_type.BOOK_GROUP_NO);
            return View(set_book_type);
        }

        //
        // GET: /SET_BOOK_TYPE/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_BOOK_TYPE set_book_type = db.SET_BOOK_TYPE.Single(s => s.BOOK_TYPE_NO == id);
            if (set_book_type == null)
            {
                return HttpNotFound();
            }
            ViewBag.BOOK_GROUP_NO = db.SET_BOOK_GROUP.ToList();
            return View(set_book_type);
        }

        //
        // POST: /SET_BOOK_TYPE/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_BOOK_TYPE set_book_type)
        {
            if (ModelState.IsValid)
            {
                db.SET_BOOK_TYPE.Attach(set_book_type);
                db.ObjectStateManager.ChangeObjectState(set_book_type, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BOOK_GROUP_NO = new SelectList(db.SET_BOOK_GROUP, "BOOK_GROUP_NO", "LAST_ACTION", set_book_type.BOOK_GROUP_NO);
            return View(set_book_type);
        }

        //
        // GET: /SET_BOOK_TYPE/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_BOOK_TYPE set_book_type = db.SET_BOOK_TYPE.Single(s => s.BOOK_TYPE_NO == id);
            if (set_book_type == null)
            {
                return HttpNotFound();
            }
            return View(set_book_type);
        }

        //
        // POST: /SET_BOOK_TYPE/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_BOOK_TYPE set_book_type = db.SET_BOOK_TYPE.Single(s => s.BOOK_TYPE_NO == id);
            db.SET_BOOK_TYPE.DeleteObject(set_book_type);
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
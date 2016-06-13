using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class UserLocationController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /UserLocation/

        public ActionResult Index()
        {
            return View(db.TRN_USER_LOCATION.ToList());
        }

        //
        // GET: /UserLocation/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_USER_LOCATION trn_user_location = db.TRN_USER_LOCATION.Single(t => t.LOC_NO == id);
            if (trn_user_location == null)
            {
                return HttpNotFound();
            }
            return View(trn_user_location);
        }

        //
        // GET: /UserLocation/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserLocation/Create

        [HttpPost]
        public ActionResult Create(TRN_USER_LOCATION trn_user_location)
        {
            if (ModelState.IsValid)
            {
                db.TRN_USER_LOCATION.AddObject(trn_user_location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trn_user_location);
        }

        //
        // GET: /UserLocation/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_USER_LOCATION trn_user_location = db.TRN_USER_LOCATION.Single(t => t.LOC_NO == id);
            if (trn_user_location == null)
            {
                return HttpNotFound();
            }
            return View(trn_user_location);
        }

        //
        // POST: /UserLocation/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_USER_LOCATION trn_user_location)
        {
            if (ModelState.IsValid)
            {
                db.TRN_USER_LOCATION.Attach(trn_user_location);
                db.ObjectStateManager.ChangeObjectState(trn_user_location, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trn_user_location);
        }

        //
        // GET: /UserLocation/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_USER_LOCATION trn_user_location = db.TRN_USER_LOCATION.Single(t => t.LOC_NO == id);
            if (trn_user_location == null)
            {
                return HttpNotFound();
            }
            return View(trn_user_location);
        }

        //
        // POST: /UserLocation/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_USER_LOCATION trn_user_location = db.TRN_USER_LOCATION.Single(t => t.LOC_NO == id);
            db.TRN_USER_LOCATION.DeleteObject(trn_user_location);
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
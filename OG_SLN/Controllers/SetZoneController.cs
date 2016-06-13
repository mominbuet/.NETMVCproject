using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class SetZoneController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SetZone/

        public ActionResult Index()
        {
            return View(db.SET_ZONE.ToList());
        }

        //
        // GET: /SetZone/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_ZONE set_zone = db.SET_ZONE.Single(s => s.ZONE_NO == id);
            if (set_zone == null)
            {
                return HttpNotFound();
            }
            return View(set_zone);
        }

        //
        // GET: /SetZone/Create

        public ActionResult Create()
        {
            ViewBag.divisions = db.SET_DIVISION.ToList();
            return View();
        }

        //
        // POST: /SetZone/Create

        [HttpPost]
        public ActionResult Create(SET_ZONE set_zone)
        {
            if (ModelState.IsValid)
            {
                db.SET_ZONE_INSERT(
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_zone.DIVISION_NO,
                    set_zone.ZONE_NAME,
                    set_zone.ZONE_NAME_BNG,
                    set_zone.ZONE_CODE,
                    set_zone.ZONE_DETAILS,
                    set_zone.ZONE_DESC,
                    set_zone.TOTAL_STUDENTS,
                    set_zone.ZONE_GRADE,
                    set_zone.ACTUAL_LATI_VAL,
                    set_zone.ACTUAL_LONG_VAL,
                    set_zone.FROM_LATI_VAL,
                    set_zone.FROM_LONG_VAL,
                    set_zone.TO_LATI_VAL,
                    set_zone.TO_LONG_VAL,
                    set_zone.IS_ACTIVE,
                    set_zone.ACTIVE_FROM,
                    set_zone.ACTIVE_TO,
                    set_zone.SL_NUM);
                return RedirectToAction("Index");
            }

            return View(set_zone);
        }

        //
        // GET: /SetZone/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_ZONE set_zone = db.SET_ZONE.Single(s => s.ZONE_NO == id);
            if (set_zone == null)
            {
                return HttpNotFound();
            }
            ViewBag.divisions = db.SET_DIVISION.ToList();
            return View(set_zone);
        }

        //
        // POST: /SetZone/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_ZONE set_zone)
        {
            if (ModelState.IsValid)
            {
                db.SET_ZONE_UPDATE(set_zone.ZONE_NO,
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_zone.DIVISION_NO,
                    set_zone.ZONE_NAME,
                    set_zone.ZONE_NAME_BNG,
                    set_zone.ZONE_CODE,
                    set_zone.ZONE_DETAILS,
                    set_zone.ZONE_DESC,
                    set_zone.TOTAL_STUDENTS,
                    set_zone.ZONE_GRADE,
                    set_zone.ACTUAL_LATI_VAL,
                    set_zone.ACTUAL_LONG_VAL,
                    set_zone.FROM_LATI_VAL,
                    set_zone.FROM_LONG_VAL,
                    set_zone.TO_LATI_VAL,
                    set_zone.TO_LONG_VAL,
                    set_zone.IS_ACTIVE,
                    set_zone.ACTIVE_FROM,
                    set_zone.ACTIVE_TO,
                    set_zone.SL_NUM);

                return RedirectToAction("Index");
            }
            return View(set_zone);
        }

        //
        // GET: /SetZone/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_ZONE set_zone = db.SET_ZONE.Single(s => s.ZONE_NO == id);
            if (set_zone == null)
            {
                return HttpNotFound();
            }
            return View(set_zone);
        }

        //
        // POST: /SetZone/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_ZONE set_zone = db.SET_ZONE.Single(s => s.ZONE_NO == id);
            db.SET_ZONE_DELETE(set_zone.ZONE_NO,
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
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
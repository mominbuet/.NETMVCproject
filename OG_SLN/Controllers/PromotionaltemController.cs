using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class PromotionaltemController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Promotionaltem/

        public ActionResult Index()
        {
            return View(db.SET_PROMO_ITEM.ToList());
        }

        //
        // GET: /Promotionaltem/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_PROMO_ITEM set_promo_item = db.SET_PROMO_ITEM.Single(s => s.PROMO_ITEM_NO == id);
            if (set_promo_item == null)
            {
                return HttpNotFound();
            }
            return View(set_promo_item);
        }

        //
        // GET: /Promotionaltem/Create

        public ActionResult Create()
        {
            ViewBag.currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.futureDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            ViewBag.PROMO_ITEM_TYPES = from p in db.SET_PROMO_ITEM_TYPE select p;
            return View();
        }

        //
        // POST: /Promotionaltem/Create

        [HttpPost]
        public ActionResult Create(SET_PROMO_ITEM set_promo_item)
        {
            if (ModelState.IsValid)
            {
                db.SET_PROMO_ITEM_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_promo_item.PROMO_ITEM_TYPE_NO,
                    set_promo_item.PROMO_ITEM_NAME,
                    set_promo_item.PROMO_ITEM_CODE,
                    set_promo_item.PROMO_ITEM_DESC,
                    set_promo_item.IS_ACTIVE,
                    set_promo_item.ACTIVE_FROM,
                    set_promo_item.ACTIVE_TO,
                    set_promo_item.SL_NUM);

                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(set_promo_item);
        }

        //
        // GET: /Promotionaltem/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_PROMO_ITEM set_promo_item = db.SET_PROMO_ITEM.Single(s => s.PROMO_ITEM_NO == id);
            if (set_promo_item == null)
            {
                return HttpNotFound();
            }
            ViewBag.PROMO_ITEM_TYPES = from p in db.SET_PROMO_ITEM_TYPE select p;
            return View(set_promo_item);
        }

        //
        // POST: /Promotionaltem/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_PROMO_ITEM set_promo_item)
        {
            if (ModelState.IsValid)
            {
                db.SET_PROMO_ITEM_UPDATE(set_promo_item.PROMO_ITEM_NO,
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                    decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_promo_item.PROMO_ITEM_TYPE_NO,
                    set_promo_item.PROMO_ITEM_NAME,
                    set_promo_item.PROMO_ITEM_CODE,
                    set_promo_item.PROMO_ITEM_DESC,
                    set_promo_item.IS_ACTIVE,
                    set_promo_item.ACTIVE_FROM,
                    set_promo_item.ACTIVE_TO,
                    set_promo_item.SL_NUM);
                
                return RedirectToAction("Index");
            }
            return View(set_promo_item);
        }

        //
        // GET: /Promotionaltem/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_PROMO_ITEM set_promo_item = db.SET_PROMO_ITEM.Single(s => s.PROMO_ITEM_NO == id);
            if (set_promo_item == null)
            {
                return HttpNotFound();
            }
            return View(set_promo_item);
        }

        //
        // POST: /Promotionaltem/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_PROMO_ITEM set_promo_item = db.SET_PROMO_ITEM.Single(s => s.PROMO_ITEM_NO == id);
            db.SET_PROMO_ITEM.DeleteObject(set_promo_item);
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
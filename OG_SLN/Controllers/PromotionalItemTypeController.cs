using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class PromotionalItemTypeController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /PromotionalItemType/

        public ActionResult Index()
        {
            return View(db.SET_PROMO_ITEM_TYPE.ToList());
        }

        //
        // GET: /PromotionalItemType/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_PROMO_ITEM_TYPE set_promo_item_type = db.SET_PROMO_ITEM_TYPE.Single(s => s.PROMO_ITEM_TYPE_NO == id);
            if (set_promo_item_type == null)
            {
                return HttpNotFound();
            }
            return View(set_promo_item_type);
        }

        //
        // GET: /PromotionalItemType/Create

        public ActionResult Create()
        {
            ViewBag.currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.futureDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            return View();
        }

        //
        // POST: /PromotionalItemType/Create

        [HttpPost]
        public ActionResult Create(SET_PROMO_ITEM_TYPE set_promo_item_type)
        {
            if (ModelState.IsValid)
            {
                set_promo_item_type.PROMO_ITEM_TYPE_NO =  db.SET_PROMO_ITEM_TYPE_INSERT(
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_promo_item_type.PROMO_ITEM_TYPE_NAME,
                    set_promo_item_type.PROMO_ITEM_TYPE_CODE,
                    set_promo_item_type.PROMO_ITEM_TYPE_DESC,
                    set_promo_item_type.IS_ACTIVE,
                    set_promo_item_type.ACTIVE_FROM,
                    set_promo_item_type.ACTIVE_TO,
                    set_promo_item_type.SL_NUM).First().Value;
                /*db.SET_PROMO_ITEM_TYPE.AddObject(set_promo_item_type);
                db.SaveChanges();*/
                return RedirectToAction("Index");
            }

            return View(set_promo_item_type);
        }

        //
        // GET: /PromotionalItemType/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_PROMO_ITEM_TYPE set_promo_item_type = db.SET_PROMO_ITEM_TYPE.Single(s => s.PROMO_ITEM_TYPE_NO == id);
            if (set_promo_item_type == null)
            {
                return HttpNotFound();
            }
            return View(set_promo_item_type);
        }

        //
        // POST: /PromotionalItemType/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_PROMO_ITEM_TYPE set_promo_item_type)
        {
            if (ModelState.IsValid)
            {
                db.SET_PROMO_ITEM_TYPE_UPDATE(set_promo_item_type.PROMO_ITEM_TYPE_NO,
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_promo_item_type.PROMO_ITEM_TYPE_NAME,
                    set_promo_item_type.PROMO_ITEM_TYPE_CODE,
                    set_promo_item_type.PROMO_ITEM_TYPE_DESC,
                    set_promo_item_type.IS_ACTIVE,
                    set_promo_item_type.ACTIVE_FROM,
                    set_promo_item_type.ACTIVE_TO,
                    set_promo_item_type.SL_NUM);

               /* db.SET_PROMO_ITEM_TYPE.Attach(set_promo_item_type);
                db.ObjectStateManager.ChangeObjectState(set_promo_item_type, EntityState.Modified);
                db.SaveChanges();*/
                return RedirectToAction("Index");
            }
            return View(set_promo_item_type);
        }

        //
        // GET: /PromotionalItemType/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_PROMO_ITEM_TYPE set_promo_item_type = db.SET_PROMO_ITEM_TYPE.Single(s => s.PROMO_ITEM_TYPE_NO == id);
            if (set_promo_item_type == null)
            {
                return HttpNotFound();
            }
            return View(set_promo_item_type);
        }

        //
        // POST: /PromotionalItemType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_PROMO_ITEM_TYPE set_promo_item_type = db.SET_PROMO_ITEM_TYPE.Single(s => s.PROMO_ITEM_TYPE_NO == id);
            db.SET_PROMO_ITEM_TYPE.DeleteObject(set_promo_item_type);
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
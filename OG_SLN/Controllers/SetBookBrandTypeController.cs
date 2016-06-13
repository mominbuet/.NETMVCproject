using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class SetBookBrandTypeController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SetBookBrandType/

        public ActionResult Index()
        {
            return View(db.SET_BOOK_BRAND_TYPE.ToList());
        }

        //
        // GET: /SetBookBrandType/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_BOOK_BRAND_TYPE set_book_brand_type = db.SET_BOOK_BRAND_TYPE.Single(s => s.BOOK_BRAND_TYPE_NO == id);
            if (set_book_brand_type == null)
            {
                return HttpNotFound();
            }
            return View(set_book_brand_type);
        }

        //
        // GET: /SetBookBrandType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SetBookBrandType/Create

        [HttpPost]
        public ActionResult Create(SET_BOOK_BRAND_TYPE set_book_brand_type)
        {
            if (ModelState.IsValid)
            {
                db.SET_BOOK_BRAND_TYPE_INSERT(
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                        set_book_brand_type.BOOK_BRAND_TYPE_NAME,
                        set_book_brand_type.BOOK_BRAND_TYPE_DESC,
                        set_book_brand_type.IS_ACTIVE,
                        set_book_brand_type.ACTIVE_FROM,
                        set_book_brand_type.ACTIVE_TO,
                        set_book_brand_type.SL_NUM);
                return RedirectToAction("Index");
            }

            return View(set_book_brand_type);
        }

        //
        // GET: /SetBookBrandType/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_BOOK_BRAND_TYPE set_book_brand_type = db.SET_BOOK_BRAND_TYPE.Single(s => s.BOOK_BRAND_TYPE_NO == id);
            if (set_book_brand_type == null)
            {
                return HttpNotFound();
            }
            return View(set_book_brand_type);
        }

        //
        // POST: /SetBookBrandType/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_BOOK_BRAND_TYPE set_book_brand_type)
        {
            if (ModelState.IsValid)
            {
                db.SET_BOOK_BRAND_TYPE_UPDATE(set_book_brand_type.BOOK_BRAND_TYPE_NO,
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                         decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                         set_book_brand_type.BOOK_BRAND_TYPE_NAME,
                         set_book_brand_type.BOOK_BRAND_TYPE_DESC,
                         set_book_brand_type.IS_ACTIVE,
                         set_book_brand_type.ACTIVE_FROM,
                         set_book_brand_type.ACTIVE_TO, set_book_brand_type.SL_NUM);
                return RedirectToAction("Index");
            }
            return View(set_book_brand_type);
        }

        //
        // GET: /SetBookBrandType/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_BOOK_BRAND_TYPE set_book_brand_type = db.SET_BOOK_BRAND_TYPE.Single(s => s.BOOK_BRAND_TYPE_NO == id);
            if (set_book_brand_type == null)
            {
                return HttpNotFound();
            }
            return View(set_book_brand_type);
        }

        //
        // POST: /SetBookBrandType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_BOOK_BRAND_TYPE set_book_brand_type = db.SET_BOOK_BRAND_TYPE.Single(s => s.BOOK_BRAND_TYPE_NO == id);
            db.SET_BOOK_BRAND_TYPE_DELETE(set_book_brand_type.BOOK_BRAND_TYPE_NO,
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class SetBookGroupController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SetBookGroup/

        public ActionResult Index()
        {
            var set_book_group = db.SET_BOOK_GROUP.Include("SET_BOOK_PROJ_TYPE");
            return View(set_book_group.ToList());
        }

        //
        // GET: /SetBookGroup/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_BOOK_GROUP set_book_group = db.SET_BOOK_GROUP.Single(s => s.BOOK_GROUP_NO == id);
            if (set_book_group == null)
            {
                return HttpNotFound();
            }
            return View(set_book_group);
        }

        //
        // GET: /SetBookGroup/Create

        public ActionResult Create()
        {
            //ViewBag.BOOK_PROJ_TYPE_NO = new SelectList(db.SET_BOOK_PROJ_TYPE, "BOOK_PROJ_TYPE_NO", "LAST_ACTION");

            ViewBag.types = db.SET_BOOK_PROJ_TYPE.ToList(); 
            return View();
        }

        //
        // POST: /SetBookGroup/Create

        [HttpPost]
        public ActionResult Create(SET_BOOK_GROUP set_book_group)
        {
            if (ModelState.IsValid)
            {
                db.SET_BOOK_GROUP_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_book_group.BOOK_GROUP_NAME,
                    set_book_group.BOOK_GROUP_DESC,
                    set_book_group.BOOK_PROJ_TYPE_NO,
                    set_book_group.IS_ACTIVE,
                    set_book_group.ACTIVE_FROM,
                    set_book_group.ACTIVE_TO,
                    set_book_group.SL_NUM);
                /*db.SET_BOOK_GROUP.AddObject(set_book_group);
                db.SaveChanges();*/
                return RedirectToAction("Index");
            }

            ViewBag.BOOK_PROJ_TYPE_NO = new SelectList(db.SET_BOOK_PROJ_TYPE, "BOOK_PROJ_TYPE_NO", "LAST_ACTION", set_book_group.BOOK_PROJ_TYPE_NO);
            return View(set_book_group);
        }

        //
        // GET: /SetBookGroup/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_BOOK_GROUP set_book_group = db.SET_BOOK_GROUP.Single(s => s.BOOK_GROUP_NO == id);
            if (set_book_group == null)
            {
                return HttpNotFound();
            }
            ViewBag.types = db.SET_BOOK_PROJ_TYPE.ToList(); 
            return View(set_book_group);
        }

        //
        // POST: /SetBookGroup/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_BOOK_GROUP set_book_group)
        {
            if (ModelState.IsValid)
            {
                /*db.SET_BOOK_GROUP.Attach(set_book_group);
                db.ObjectStateManager.ChangeObjectState(set_book_group, EntityState.Modified);
                db.SaveChanges();*/
                db.SET_BOOK_GROUP_UPDATE(set_book_group.BOOK_GROUP_NO,
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                    set_book_group.BOOK_GROUP_NAME,
                    set_book_group.BOOK_GROUP_DESC,
                    set_book_group.BOOK_PROJ_TYPE_NO,
                    set_book_group.IS_ACTIVE,
                    set_book_group.ACTIVE_FROM,
                    set_book_group.ACTIVE_TO,
                    set_book_group.SL_NUM);
                return RedirectToAction("Index");
            }
            ViewBag.BOOK_PROJ_TYPE_NO = new SelectList(db.SET_BOOK_PROJ_TYPE, "BOOK_PROJ_TYPE_NO", "LAST_ACTION", set_book_group.BOOK_PROJ_TYPE_NO);
            return View(set_book_group);
        }

        //
        // GET: /SetBookGroup/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_BOOK_GROUP set_book_group = db.SET_BOOK_GROUP.Single(s => s.BOOK_GROUP_NO == id);
            if (set_book_group == null)
            {
                return HttpNotFound();
            }
            return View(set_book_group);
        }

        //
        // POST: /SetBookGroup/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_BOOK_GROUP set_book_group = db.SET_BOOK_GROUP.Single(s => s.BOOK_GROUP_NO == id);
            /*db.SET_BOOK_GROUP.DeleteObject(set_book_group);
            db.SaveChanges();*/
            db.SET_BOOK_GROUP_DELETE(set_book_group.BOOK_GROUP_NO,
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
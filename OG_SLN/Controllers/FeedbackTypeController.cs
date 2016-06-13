using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class FeedbackTypeController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /FeedbackType/

        public ActionResult Index()
        {
            return View(db.SET_FEEDBACK_TYPE.ToList());
        }

        //
        // GET: /FeedbackType/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_FEEDBACK_TYPE set_feedback_type = db.SET_FEEDBACK_TYPE.Single(s => s.FEEDBACK_TYPE_NO == id);
            if (set_feedback_type == null)
            {
                return HttpNotFound();
            }
            return View(set_feedback_type);
        }

        //
        // GET: /FeedbackType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FeedbackType/Create

        [HttpPost]
        public ActionResult Create(SET_FEEDBACK_TYPE feed)
        {
            if (ModelState.IsValid)
            {
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                db.SET_FEEDBACK_TYPE_INSERT(USER_NO, LOGON_NO, feed.FEEDBACK_NAME, feed.FEEDBACK_CODE,
                    feed.FEEDBACK_DESC, feed.IS_ACTIVE);

                return RedirectToAction("Index");
            }

            return View(feed);
        }

        //
        // GET: /FeedbackType/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_FEEDBACK_TYPE set_feedback_type = db.SET_FEEDBACK_TYPE.Single(s => s.FEEDBACK_TYPE_NO == id);
            if (set_feedback_type == null)
            {
                return HttpNotFound();
            }
            return View(set_feedback_type);
        }

        //
        // POST: /FeedbackType/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_FEEDBACK_TYPE feed)
        {
            if (ModelState.IsValid)
            {
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                db.SET_FEEDBACK_TYPE_UPDATE(feed.FEEDBACK_TYPE_NO, USER_NO, LOGON_NO, feed.FEEDBACK_NAME,
                    feed.FEEDBACK_CODE, feed.FEEDBACK_DESC, feed.IS_ACTIVE);

                return RedirectToAction("Index");
            }
            return View(feed);
        }

        //
        // GET: /FeedbackType/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_FEEDBACK_TYPE set_feedback_type = db.SET_FEEDBACK_TYPE.Single(s => s.FEEDBACK_TYPE_NO == id);
            if (set_feedback_type == null)
            {
                return HttpNotFound();
            }
            return View(set_feedback_type);
        }

        //
        // POST: /FeedbackType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.SET_FEEDBACK_TYPE_DELETE(id, USER_NO, LOGON_NO);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Models;
using PagedList;
using System.Data.Objects;

namespace OG_SLN.Controllers
{
    public class FeedbackController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Feedback/

        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            //var trn_user_feed_back = db.TRN_USER_FEED_BACK.Include("SET_FEEDBACK_TYPE");
            //return View(trn_user_feed_back.ToList());

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<TRN_USER_FEED_BACK> feedback = null;
            FeedbackSearchViewModel feedbackModel = null;

            var feeds = db.TRN_USER_FEED_BACK.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                feedbackModel = new FeedbackSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Feedback_Type"]))
                {
                    feedbackModel.Search_Feedback_Type = decimal.Parse(Request.QueryString["Search_Feedback_Type"]);
                    feeds = feeds.Where(f => f.FEEDBACK_TYPE_NO == feedbackModel.Search_Feedback_Type);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_User"]))
                {
                    feedbackModel.Search_User = decimal.Parse(Request.QueryString["Search_User"]);
                    feeds = feeds.Where(f => f.USER_NO == feedbackModel.Search_User);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
                {
                    feedbackModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
                    feeds = feeds.Where(f => f.INSERT_TIME >= feedbackModel.Search_Date_From);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
                {
                    feedbackModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
                    feeds = feeds.Where(f => f.INSERT_TIME <= feedbackModel.Search_Date_To);
                }


                Session["Feedback_Search_Model"] = feedbackModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["Feedback_Search_Model"] = null;
            }

            if (Session["Feedback_Search_Model"] != null)
            {
                feedbackModel = (FeedbackSearchViewModel)Session["Feedback_Search_Model"];

                if (feedbackModel.Search_Feedback_Type.HasValue)
                {
                    feeds = feeds.Where(f => f.FEEDBACK_TYPE_NO == feedbackModel.Search_Feedback_Type);
                }

                if (feedbackModel.Search_User.HasValue)
                {
                    feeds = feeds.Where(f => f.USER_NO == feedbackModel.Search_User);
                }

                if (feedbackModel.Search_Date_From.HasValue)
                {
                    feeds = feeds.Where(f => f.INSERT_TIME >= feedbackModel.Search_Date_From);
                }

                if (feedbackModel.Search_Date_To.HasValue)
                {
                    feeds = feeds.Where(f => f.INSERT_TIME <= feedbackModel.Search_Date_To);
                }
            }

            feedback = feeds.OrderBy(m => m.INSERT_TIME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_Feedback_Model = (FeedbackSearchViewModel)Session["Feedback_Search_Model"];

            ViewBag.Search_Feedback_Type = new SelectList(db.SET_FEEDBACK_TYPE, "FEEDBACK_TYPE_NO", "FEEDBACK_NAME");

            return View(feedback);
        }

        //
        // GET: /Feedback/Details/5

        public ActionResult Details(decimal id = 0)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            TRN_USER_FEED_BACK trn_user_feed_back = db.TRN_USER_FEED_BACK.Single(t => t.FEED_BACK_NO == id);
            if (trn_user_feed_back == null)
            {
                return HttpNotFound();
            }

            if (!trn_user_feed_back.READ_STATUS)
            {
                db.TRN_USER_FEED_BACK_READ(trn_user_feed_back.FEED_BACK_NO, USER_NO, LOGON_NO);
            }

            return View(trn_user_feed_back);
        }

        //
        // GET: /Feedback/Create

        public ActionResult Create()
        {
            ViewBag.FEEDBACK_TYPE_NO = new SelectList(db.SET_FEEDBACK_TYPE, "FEEDBACK_TYPE_NO", "LAST_ACTION");
            return View();
        }

        //
        // POST: /Feedback/Create

        [HttpPost]
        public ActionResult Create(TRN_USER_FEED_BACK trn_user_feed_back)
        {
            if (ModelState.IsValid)
            {
                db.TRN_USER_FEED_BACK.AddObject(trn_user_feed_back);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FEEDBACK_TYPE_NO = new SelectList(db.SET_FEEDBACK_TYPE, "FEEDBACK_TYPE_NO", "LAST_ACTION", trn_user_feed_back.FEEDBACK_TYPE_NO);
            return View(trn_user_feed_back);
        }

        //
        // GET: /Feedback/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_USER_FEED_BACK trn_user_feed_back = db.TRN_USER_FEED_BACK.Single(t => t.FEED_BACK_NO == id);
            if (trn_user_feed_back == null)
            {
                return HttpNotFound();
            }
            ViewBag.FEEDBACK_TYPE_NO = new SelectList(db.SET_FEEDBACK_TYPE, "FEEDBACK_TYPE_NO", "LAST_ACTION", trn_user_feed_back.FEEDBACK_TYPE_NO);
            return View(trn_user_feed_back);
        }

        //
        // POST: /Feedback/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_USER_FEED_BACK trn_user_feed_back)
        {
            if (ModelState.IsValid)
            {
                db.TRN_USER_FEED_BACK.Attach(trn_user_feed_back);
                db.ObjectStateManager.ChangeObjectState(trn_user_feed_back, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FEEDBACK_TYPE_NO = new SelectList(db.SET_FEEDBACK_TYPE, "FEEDBACK_TYPE_NO", "LAST_ACTION", trn_user_feed_back.FEEDBACK_TYPE_NO);
            return View(trn_user_feed_back);
        }

        //
        // GET: /Feedback/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_USER_FEED_BACK trn_user_feed_back = db.TRN_USER_FEED_BACK.Single(t => t.FEED_BACK_NO == id);
            if (trn_user_feed_back == null)
            {
                return HttpNotFound();
            }
            return View(trn_user_feed_back);
        }

        //
        // POST: /Feedback/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_USER_FEED_BACK trn_user_feed_back = db.TRN_USER_FEED_BACK.Single(t => t.FEED_BACK_NO == id);
            db.TRN_USER_FEED_BACK.DeleteObject(trn_user_feed_back);
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
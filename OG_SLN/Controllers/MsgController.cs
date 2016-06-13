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
    public class MsgController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Msg/

        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            //var trn_msg = db.TRN_MSG;
            //return View(trn_msg.ToList());

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<TRN_MSG> msg = null;
            MessageSearchViewModel messageModel = null;

            var messages = db.TRN_MSG.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                messageModel = new MessageSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_From_User"]))
                {
                    messageModel.Search_From_User = decimal.Parse(Request.QueryString["Search_From_User"]);
                    messages = messages.Where(u => u.FROM_USER_NO == messageModel.Search_From_User);
                }
                /*
                if (!string.IsNullOrEmpty(Request.QueryString["Search_To_User"]))
                {
                    messageModel.Search_To_User = decimal.Parse(Request.QueryString["Search_To_User"]);
                    messages = messages.Where(u => u.FROM_USER_NO == messageModel.Search_To_User);
                }
                */
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Subject"]))
                {
                    messageModel.Search_Subject = Request.QueryString["Search_Subject"];
                    messages = messages.Where(u => u.MSG_SUBJECT.ToLower()
                        .Contains(messageModel.Search_Subject.ToLower()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
                {
                    messageModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
                    messages = messages.Where(u => u.INSERT_TIME >= messageModel.Search_Date_From);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
                {
                    messageModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
                    messages = messages.Where(u => u.INSERT_TIME <= messageModel.Search_Date_To);
                }
                

                Session["Message_Search_Model"] = messageModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["Message_Search_Model"] = null;
            }

            if (Session["Message_Search_Model"] != null)
            {
                messageModel = (MessageSearchViewModel)Session["Message_Search_Model"];

                if (messageModel.Search_From_User.HasValue)
                {
                    messages = messages.Where(u => u.FROM_USER_NO == messageModel.Search_From_User);
                }
                /*
                if (messageModel.Search_To_User.HasValue)
                {
                    messages = messages.Where(u => u.FROM_USER_NO == messageModel.Search_To_User);
                }
                */
                if (!string.IsNullOrEmpty(messageModel.Search_Subject))
                {
                    messages = messages.Where(u => u.MSG_SUBJECT.ToLower()
                        .Contains(messageModel.Search_Subject.ToLower()));
                }

                if (messageModel.Search_Date_From.HasValue)
                {
                    messages = messages.Where(u => u.INSERT_TIME >= messageModel.Search_Date_From);
                }

                if (messageModel.Search_Date_To.HasValue)
                {
                    messages = messages.Where(u => u.INSERT_TIME <= messageModel.Search_Date_To);
                }
            }

            msg = messages.OrderBy(m => m.INSERT_TIME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_Message_Model = (MessageSearchViewModel)Session["Message_Search_Model"];

            return View(msg);
        }

        //
        // GET: /Msg/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_MSG trn_msg = db.TRN_MSG.Single(t => t.MSG_NO == id);
            if (trn_msg == null)
            {
                return HttpNotFound();
            }
            return View(trn_msg);
        }

        //
        // GET: /Msg/Create

        public ActionResult Create()
        {
            ViewBag.REF_MSG_NO = new SelectList(db.TRN_MSG, "MSG_NO", "LAST_ACTION");
            return View();
        }

        //
        // POST: /Msg/Create

        [HttpPost]
        public ActionResult Create(TRN_MSG msg)
        {
            if (ModelState.IsValid)
            {
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                //db.TRN_MSG.AddObject(trn_msg);
                //db.SaveChanges();

                decimal? msg_no = db.TRN_MSG_INSERT(USER_NO, LOGON_NO, USER_NO, msg.MSG_SUBJECT, msg.MSG_BODY,
                    null, null, null, null).FirstOrDefault().Value;

                foreach (ZonalManagerViewModel manager in msg.ZonalManagers)
                {
                    db.TRN_MSG_RECIEVER_INSERT(USER_NO, LOGON_NO, msg_no, manager.USER_NO);
                }

                return RedirectToAction("Index");
            }

            ViewBag.REF_MSG_NO = new SelectList(db.TRN_MSG, "MSG_NO", "LAST_ACTION", msg.REF_MSG_NO);
            ViewBag.ZONAL_USER_NO = new SelectList(db.SEC_USERS
                .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_NO", "USER_FULL_NAME");

            return View(msg);
        }

        //
        // GET: /Msg/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_MSG trn_msg = db.TRN_MSG.Single(t => t.MSG_NO == id);
            if (trn_msg == null)
            {
                return HttpNotFound();
            }
            ViewBag.REF_MSG_NO = new SelectList(db.TRN_MSG, "MSG_NO", "LAST_ACTION", trn_msg.REF_MSG_NO);
            return View(trn_msg);
        }

        //
        // POST: /Msg/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_MSG trn_msg)
        {
            if (ModelState.IsValid)
            {
                db.TRN_MSG.Attach(trn_msg);
                db.ObjectStateManager.ChangeObjectState(trn_msg, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.REF_MSG_NO = new SelectList(db.TRN_MSG, "MSG_NO", "LAST_ACTION", trn_msg.REF_MSG_NO);
            return View(trn_msg);
        }

        //
        // GET: /Msg/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_MSG trn_msg = db.TRN_MSG.Single(t => t.MSG_NO == id);
            if (trn_msg == null)
            {
                return HttpNotFound();
            }
            return View(trn_msg);
        }

        //
        // POST: /Msg/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_MSG trn_msg = db.TRN_MSG.Single(t => t.MSG_NO == id);
            db.TRN_MSG.DeleteObject(trn_msg);
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
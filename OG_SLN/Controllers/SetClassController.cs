using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OG_SLN.Controllers
{
    public class SetClassController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /SetClass/

        public ActionResult Index()
        {
            return View(db.SET_CLASS.ToList());
        }

        //
        // GET: /SetClass/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_CLASS set_class = db.SET_CLASS.Single(s => s.CLASS_NO == id);
            if (set_class == null)
            {
                return HttpNotFound();
            }
            return View(set_class);
        }

        //
        // GET: /SetClass/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SetClass/Create

        [HttpPost]
        public ActionResult Create(SET_CLASS set_class)
        {
            if (ModelState.IsValid)
            {
                db.SET_CLASS.AddObject(set_class);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(set_class);
        }

        //
        // GET: /SetClass/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_CLASS set_class = db.SET_CLASS.Single(s => s.CLASS_NO == id);
            if (set_class == null)
            {
                return HttpNotFound();
            }
            return View(set_class);
        }

        //
        // POST: /SetClass/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_CLASS set_class)
        {
            if (ModelState.IsValid)
            {
                db.SET_CLASS.Attach(set_class);
                db.ObjectStateManager.ChangeObjectState(set_class, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(set_class);
        }

        //
        // GET: /SetClass/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_CLASS set_class = db.SET_CLASS.Single(s => s.CLASS_NO == id);
            if (set_class == null)
            {
                return HttpNotFound();
            }
            return View(set_class);
        }

        //
        // POST: /SetClass/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SET_CLASS set_class = db.SET_CLASS.Single(s => s.CLASS_NO == id);
            db.SET_CLASS.DeleteObject(set_class);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddSubject(decimal id)
        {
            ViewBag.CLASS_NO = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddSubject(SET_SUBJECT subject)
        {
            if (subject != null)
            {
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                try
                {
                    db.SET_SUBJECT_INSERT(USER_NO, LOGON_NO, subject.SUBJECT_NAME, subject.SUBJECT_NAME_BNG,
                        subject.SUBJECT_CODE, subject.SUBJECT_DESC, subject.IS_ACTIVE, subject.ACTIVE_FROM,
                        subject.ACTIVE_TO, subject.SL_NUM);

                    return RedirectToAction("AssignSubject", new { id = subject.CLASS_NO });
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("unique constraint"))
                    {
                        ModelState.AddModelError("SUBJECT_NAME", "Unique Constraint Violated. "
                            + "Same Subject Name is not allowed for multiple subjects.");
                    }
                    else
                    {
                        ModelState.AddModelError("ACTIVE_TO", "An error occured while adding subject.");
                    }
                }

                ViewBag.CLASS_NO = subject.CLASS_NO;
                return View(subject);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult EditSubject(decimal id = 0)
        {
            SET_SUBJECT subject = db.SET_SUBJECT.Single(s => s.SUBJECT_NO == id);
            if (subject == null)
                return HttpNotFound();

            return View(subject);
        }

        [HttpPost]
        public ActionResult EditSubject(SET_SUBJECT subject)
        {
            if (subject != null)
            {
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                try
                {
                    db.SET_SUBJECT_UPDATE(subject.SUBJECT_NO, USER_NO, LOGON_NO, subject.SUBJECT_NAME,
                        subject.SUBJECT_NAME_BNG, subject.SUBJECT_CODE, subject.SUBJECT_DESC, subject.IS_ACTIVE,
                        subject.ACTIVE_FROM, subject.ACTIVE_TO, subject.SL_NUM);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("unique constraint"))
                    {
                        ModelState.AddModelError("SUBJECT_NAME", "Unique Constraint Violated. "
                            + "Same Subject Name is not allowed for multiple subjects.");
                    }
                    else
                    {
                        ModelState.AddModelError("ACTIVE_TO", "An error occured while adding subject.");
                    }
                }

                ViewBag.CLASS_NO = subject.CLASS_NO;
                return View(subject);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult AssignSubject(decimal id = 0)
        {
            SET_CLASS set_class = db.SET_CLASS.Single(s => s.CLASS_NO == id);

            List<Class_Subject> subjects = (from ss in db.SET_SUBJECT
                                           join cs in db.SET_CLASS_SUBJECT
                                                on ss.SUBJECT_NO equals cs.SUBJECT_NO into joined
                                            from j in joined.Where(a => a.CLASS_NO == set_class.CLASS_NO || a.CLASS_NO == null).DefaultIfEmpty()
                                           //where j.CLASS_NO == id || j.CLASS_NO == null
                                           select new Class_Subject 
                                           { 
                                               SUBJECT_NO = ss.SUBJECT_NO,
                                               SUBJECT_NAME = ss.SUBJECT_NAME,
                                               SUBJECT_NAME_BNG = ss.SUBJECT_NAME_BNG,
                                               IS_ACTIVE = j.IS_ACTIVE == 1 ? true : false
                                           }).OrderBy(a => a.SUBJECT_NO).ToList();

            ViewBag.Subjects = subjects;

            return View(set_class);
        }

        [HttpPost]
        public ActionResult AssignSubject(SET_CLASS set_class)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            List<Class_Subject> subjects = set_class.Subjects;

            foreach (var subject in subjects)
            {
                decimal is_active = subject.IS_ACTIVE == true ? 1 : 0;

                db.SET_CLASS_SUBJECT_ASSIGN(set_class.CLASS_NO, subject.SUBJECT_NO, is_active, USER_NO, LOGON_NO);
            }

            subjects = (from ss in db.SET_SUBJECT
                        join cs in db.SET_CLASS_SUBJECT
                             on ss.SUBJECT_NO equals cs.SUBJECT_NO into joined
                        from j in joined.Where(a => a.CLASS_NO == set_class.CLASS_NO || a.CLASS_NO == null).DefaultIfEmpty()
                        //where (j.CLASS_NO == set_class.CLASS_NO || j.CLASS_NO == null)
                        select new Class_Subject
                        {
                            SUBJECT_NO = ss.SUBJECT_NO,
                            SUBJECT_NAME = ss.SUBJECT_NAME,
                            SUBJECT_NAME_BNG = ss.SUBJECT_NAME_BNG,
                            IS_ACTIVE = j.IS_ACTIVE == 1 ? true : false
                        }).OrderBy(a => a.SUBJECT_NO).ToList();

            ViewBag.Subjects = subjects;

            return View(set_class);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
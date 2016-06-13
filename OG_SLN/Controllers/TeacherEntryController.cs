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
    public class TeacherEntryController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /TeacherEntry/

        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            //var set_teacher_info = db.SET_TEACHER_INFO.Include("SET_TEACHER_DESIG");
            //return View(set_teacher_info.ToList());

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<SET_TEACHER_INFO> teacher_info = null;
            TeacherSearchViewModel teacherModel = null;

            var teachers = db.SET_TEACHER_INFO.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                teacherModel = new TeacherSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Name"]))
                {
                    teacherModel.Search_Name = Request.QueryString["Search_Name"];
                    teachers = teachers.Where(u => u.TEACHER_NAME.ToLower()
                        .Contains(teacherModel.Search_Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Mobile"]))
                {
                    teacherModel.Search_Mobile = Request.QueryString["Search_Mobile"];
                    teachers = teachers.Where(u => u.TEACHER_MOBILE.ToLower()
                        .Contains(teacherModel.Search_Mobile.ToLower()));
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Designation"]))
                {
                    teacherModel.Search_Designation = decimal.Parse(Request.QueryString["Search_Designation"]);
                    teachers = teachers.Where(u => u.TEACH_DESIG_NO == teacherModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Active"])
                    && Request.QueryString["Search_Active"] != "null")
                {
                    teacherModel.Search_Active = bool.Parse(Request.QueryString["Search_Active"]);
                    decimal active = teacherModel.Search_Active == true ? 1 : 0;
                    teachers = teachers.Where(u => u.IS_ACTIVE == active);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Approved"])
                    && Request.QueryString["Search_Approved"] != "null")
                {
                    teacherModel.Search_Approved = bool.Parse(Request.QueryString["Search_Approved"]);
                    decimal approved = teacherModel.Search_Approved == true ? 1 : 0;
                    teachers = teachers.Where(u => u.IS_APPROVED == approved);
                }

                Session["Teacher_Search_Model"] = teacherModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["Teacher_Search_Model"] = null;
            }

            if (Session["Teacher_Search_Model"] != null)
            {
                teacherModel = (TeacherSearchViewModel)Session["Teacher_Search_Model"];
                if (!string.IsNullOrEmpty(teacherModel.Search_Name))
                {
                    teachers = teachers.Where(u => u.TEACHER_NAME.ToLower()
                        .Contains(teacherModel.Search_Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(teacherModel.Search_Mobile))
                {
                    teachers = teachers.Where(u => u.TEACHER_MOBILE.ToLower()
                        .Contains(teacherModel.Search_Mobile.ToLower()));
                }
                if (teacherModel.Search_Institute.HasValue)
                {
                    teachers = teachers.Where(u => u.INSTITUTE_NO == teacherModel.Search_Institute);
                }
                if (teacherModel.Search_Designation.HasValue)
                {
                    teachers = teachers.Where(u => u.TEACH_DESIG_NO == teacherModel.Search_Designation);
                }

                if (teacherModel.Search_Active.HasValue)
                {
                    decimal active = teacherModel.Search_Active == true ? 1 : 0;
                    teachers = teachers.Where(u => u.IS_ACTIVE == active);
                }
                if (teacherModel.Search_Approved.HasValue)
                {
                    decimal approved = teacherModel.Search_Approved == true ? 1 : 0;
                    teachers = teachers.Where(u => u.IS_APPROVED == approved);
                }
            }

            teacher_info = teachers.OrderBy(m => m.TEACHER_NAME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_Teacher_Model = (TeacherSearchViewModel)Session["Teacher_Search_Model"];

            if (teacherModel != null && teacherModel.Search_Designation.HasValue)
            {
                ViewBag.Search_Designation = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME",
                    teacherModel.Search_Designation);
            }
            else
            {
                ViewBag.Search_Designation = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME");
            }

            return View(teacher_info);
        }

        //
        // GET: /TeacherEntry/Details/5

        public ActionResult Details(decimal id = 0)
        {
            SET_TEACHER_INFO set_teacher_info = db.SET_TEACHER_INFO.Single(s => s.TEACHER_NO == id);
            if (set_teacher_info == null)
            {
                return HttpNotFound();
            }
            return View(set_teacher_info);
        }

        //
        // GET: /TeacherEntry/Create

        public ActionResult Create()
        {
            //ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "LAST_ACTION");
            ViewBag.TEACH_DESIG_NO = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME");
            ViewBag.SUBJECT = db.SET_SUBJECT.OrderBy(s => s.SUBJECT_NAME).ToList();
            return View();
        }

        //
        // POST: /TeacherEntry/Create

        [HttpPost]
        public ActionResult Create(SET_TEACHER_INFO teacher)
        {
            if (ModelState.IsValid)
            {
                //db.SET_TEACHER_INFO.AddObject(set_teacher_info);
                //db.SaveChanges();
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                ObjectResult<decimal?> Teacher_No = 

                db.SET_TEACHER_INFO_INSERT(USER_NO, LOGON_NO, teacher.TEACHER_NAME, teacher.TEACHER_NAME_BNG,
                    teacher.TEACHER_NICK_NAME, teacher.TEACHER_DOB, teacher.TEACHER_MARRIAGE_DATE, teacher.TEACHER_ADDR,
                    teacher.TEACHER_COMENTS, teacher.TEACHER_MOBILE, teacher.INSTITUTE_NO, teacher.TEACH_DESIG_NO, teacher.IS_OFFLINE_ENTRY,
                    teacher.OFFLINE_ENTRY_TIME, teacher.OFFLINE_ENTRY_SYNC, teacher.IS_ACTIVE, teacher.ACTIVE_FROM,
                    teacher.ACTIVE_TO, teacher.SL_NUM);

                decimal? teacher_no = Teacher_No.ElementAt(0);

                foreach (Teacher_Subject subject in teacher.TeacherSubjects)
                {
                    decimal? subject_no = decimal.Parse(subject.SUBJECT_NO);
                    decimal? active = subject.IS_ACTIVE ? 1 : 0;

                    db.SET_TEACHER_SUBJECT_INSERT(USER_NO, LOGON_NO, teacher_no, subject_no, null,
                        1, null, null, null, active, null, null, null);
                }

                return RedirectToAction("Index");
            }

            //ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "LAST_ACTION", set_teacher_info.INSTITUTE_NO);
            ViewBag.TEACH_DESIG_NO = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME",
                teacher.TEACH_DESIG_NO);
            ViewBag.SUBJECT = ViewBag.SUBJECT = db.SET_SUBJECT.OrderBy(s => s.SUBJECT_NAME).ToList();
            return View(teacher);
        }

        //
        // GET: /TeacherEntry/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            SET_TEACHER_INFO set_teacher_info = db.SET_TEACHER_INFO.Single(s => s.TEACHER_NO == id);
            if (set_teacher_info == null)
            {
                return HttpNotFound();
            }
            //ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "LAST_ACTION", set_teacher_info.INSTITUTE_NO);
            ViewBag.TEACH_DESIG_NO = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME", 
                set_teacher_info.TEACH_DESIG_NO);
            ViewBag.SUBJECT = db.SET_SUBJECT.ToList();

            return View(set_teacher_info);
        }

        //
        // POST: /TeacherEntry/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_TEACHER_INFO teacher)
        {
            if (ModelState.IsValid)
            {
                /*
                db.SET_TEACHER_INFO.Attach(set_teacher_info);
                db.ObjectStateManager.ChangeObjectState(set_teacher_info, EntityState.Modified);
                db.SaveChanges();
                */
                decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
                decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

                db.SET_TEACHER_INFO_UPDATE(teacher.TEACHER_NO, USER_NO, LOGON_NO, teacher.TEACHER_NAME,
                    teacher.TEACHER_NAME_BNG, teacher.TEACHER_NICK_NAME, teacher.TEACHER_DOB, teacher.TEACHER_MARRIAGE_DATE,
                    teacher.TEACHER_ADDR, teacher.TEACHER_COMENTS, teacher.TEACHER_MOBILE, teacher.INSTITUTE_NO, teacher.TEACH_DESIG_NO,
                    teacher.IS_ACTIVE, teacher.ACTIVE_FROM, teacher.ACTIVE_TO, teacher.SL_NUM);

                foreach (Teacher_Subject subject in teacher.TeacherSubjects)
                {
                    decimal? subject_no = decimal.Parse(subject.SUBJECT_NO);
                    decimal? active = subject.IS_ACTIVE ? 1 : 0;

                    db.SET_TEACHER_SUB_STATUS_UPDATE(teacher.TEACHER_NO, subject_no, USER_NO, LOGON_NO, active);
                }

                return RedirectToAction("Index");
            }
            //ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "LAST_ACTION", set_teacher_info.INSTITUTE_NO);
            ViewBag.TEACH_DESIG_NO = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME", 
                teacher.TEACH_DESIG_NO);
            ViewBag.SUBJECT = db.SET_SUBJECT.ToList();

            return View(teacher);
        }

        //
        // GET: /TeacherEntry/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            SET_TEACHER_INFO set_teacher_info = db.SET_TEACHER_INFO.Single(s => s.TEACHER_NO == id);
            if (set_teacher_info == null)
            {
                return HttpNotFound();
            }
            return View(set_teacher_info);
        }

        //
        // POST: /TeacherEntry/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            /*
            SET_TEACHER_INFO set_teacher_info = db.SET_TEACHER_INFO.Single(s => s.TEACHER_NO == id);
            db.SET_TEACHER_INFO.DeleteObject(set_teacher_info);
            db.SaveChanges();
             * */

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.SET_TEACHER_INFO_DELETE(id, USER_NO, LOGON_NO);

            return RedirectToAction("Index");
        }

        public ActionResult Approve(string sortOrder, string CurrentSort, int? page)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;
            ViewBag.USER_NO = USER_NO;
            ViewBag.LOGON_NO = LOGON_NO;

            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<SET_TEACHER_INFO> teacher_info = null;
            TeacherApproveSearchViewModel teacherModel = null;

            var teachers = db.SET_TEACHER_INFO.AsQueryable();

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                teacherModel = new TeacherApproveSearchViewModel();

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Insert_User"]))
                {
                    teacherModel.Search_Insert_User = decimal.Parse(Request.QueryString["Search_Insert_User"]);
                    teachers = teachers.Where(u => u.INSERT_USER_NO == teacherModel.Search_Insert_User);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Institute"]))
                {
                    teacherModel.Search_Institute = decimal.Parse(Request.QueryString["Search_Institute"]);
                    teachers = teachers.Where(u => u.INSTITUTE_NO == teacherModel.Search_Institute);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Designation"]))
                {
                    teacherModel.Search_Designation = decimal.Parse(Request.QueryString["Search_Designation"]);
                    teachers = teachers.Where(u => u.TEACH_DESIG_NO == teacherModel.Search_Designation);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
                {
                    teacherModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
                    teachers = teachers.Where(u => u.INSERT_TIME.Value >= teacherModel.Search_Date_From.Value);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
                {
                    teacherModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
                    teachers = teachers.Where(u => u.INSERT_TIME.Value <= teacherModel.Search_Date_To.Value);
                }
                
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Approved"])
                    && Request.QueryString["Search_Approved"] != "null")
                {
                    teacherModel.Search_Approved = decimal.Parse(Request.QueryString["Search_Approved"]);
                    teachers = teachers.Where(u => u.IS_APPROVED == teacherModel.Search_Approved);
                }

                Session["Teacher_Approve_Search_Model"] = teacherModel;
            }

            if (!Request.QueryString.HasKeys() && !page.HasValue)
            {
                Session["Teacher_Approve_Search_Model"] = null;
            }

            if (Session["Teacher_Approve_Search_Model"] != null)
            {
                teacherModel = (TeacherApproveSearchViewModel)Session["Teacher_Approve_Search_Model"];
                if (teacherModel.Search_Insert_User.HasValue)
                {
                    teachers = teachers.Where(u => u.INSERT_USER_NO == teacherModel.Search_Insert_User);
                }
                if (teacherModel.Search_Institute.HasValue)
                {
                    teachers = teachers.Where(u => u.INSTITUTE_NO == teacherModel.Search_Institute);
                }
                if (teacherModel.Search_Designation.HasValue)
                {
                    teachers = teachers.Where(u => u.TEACH_DESIG_NO == teacherModel.Search_Designation);
                }
                if (teacherModel.Search_Date_From.HasValue)
                {
                    teachers = teachers.Where(u => u.INSERT_TIME.Value >= teacherModel.Search_Date_From.Value);
                }
                if (teacherModel.Search_Date_To.HasValue)
                {
                    teachers = teachers.Where(u => u.INSERT_TIME.Value <= teacherModel.Search_Date_To.Value);
                }
                if (teacherModel.Search_Approved.HasValue)
                {
                    teachers = teachers.Where(u => u.IS_APPROVED == teacherModel.Search_Approved.Value);
                }
            }

            teacher_info = teachers.OrderBy(m => m.TEACHER_NAME)
                            .ToPagedList(pageIndex, pageSize);

            ViewBag.Search_Teacher_Model = (TeacherApproveSearchViewModel)Session["Teacher_Approve_Search_Model"];

            if (teacherModel != null && teacherModel.Search_Insert_User.HasValue)
            {
                ViewBag.Search_Insert_User = new SelectList(db.SEC_USERS
                    .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_NO", "USER_NAME",
                    teacherModel.Search_Insert_User.Value);
            }
            else
            {
                ViewBag.Search_Insert_User = new SelectList(db.SEC_USERS
                    .Where(u => u.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager), "USER_NO", "USER_NAME");
            }

            if (teacherModel != null && teacherModel.Search_Institute.HasValue)
            {
                ViewBag.Search_Institute = teacherModel.Search_Institute.Value;
            }

            if (teacherModel != null && teacherModel.Search_Designation.HasValue)
            {
                ViewBag.Search_Designation = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME",
                    teacherModel.Search_Designation);
            }
            else
            {
                ViewBag.Search_Designation = new SelectList(db.SET_TEACHER_DESIG, "TEACH_DESIG_NO", "TEACHER_DESIG_NAME");
            }

            var approveTypes = from ApproveType a in Enum.GetValues(typeof(ApproveType))
                               select new { ID = (int)Enum.Parse(typeof(ApproveType), a.ToString()), Name = a.ToString() };

            if (teacherModel != null && teacherModel.Search_Approved.HasValue)
            {
                ViewBag.Search_Approved = new SelectList(approveTypes, "ID", "Name", teacherModel.Search_Approved);
            }
            else
            {
                ViewBag.Search_Approved = new SelectList(approveTypes, "ID", "Name");
            }
            

            return View(teacher_info);
        }

        [ActionName("Institute")]
        public ActionResult GetInstituteForm()
        {
            ViewBag.Institute_Type = new SelectList(db.SET_INST_TYPE, "INST_TYPE_NO", "INST_TYPE_NAME");

            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            SET_DIVISION division = db.SET_DIVISION.FirstOrDefault();
            ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA
                .Where(z => z.DIVISION_NO == division.DIVISION_NO), "ZILLA_NO", "ZILLA_NAME");
            ViewBag.THANA_NO = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0), "THANA_NO", "THANA_NAME");

            return PartialView("_GetInstitutePartial");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
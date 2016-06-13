using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using OG_SLN.Models;
using PagedList;

namespace OG_SLN.Controllers
{
    public class QuestionProjectController : Controller
    {
        private OGDBEntities db = new OGDBEntities();
        private int pageSize = 10;

        private string question_upload_url = System.Configuration.ConfigurationManager.AppSettings["QUESTION_UPLOAD_URL"];

        //QUESTION_UPLOAD_URL
        // GET: /QuestionProject/

        public ActionResult Index(int? page)
        {
            int pageIndex = 1;
            int startFrom = 1;

            if (page.HasValue)
            {
                pageIndex = page.Value;

                startFrom = page.Value * pageSize + 1;
            }

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            //var tpr_project = db.TPR_PROJECT.Include("SET_CLASS").OrderByDescending(p => p.DECLARE_DATE);
            //var tpr_project = db.TPR_PROJECT_GET(null, null, null, null, null, null, null, null, null, 1, 10);

            List<TPR_PROJECT_GET_Result> tpr_project = db.TPR_PROJECT_GET(null, null, null, null,
                null, null, null, null, USER_NO, startFrom, pageSize).ToList();

            IPagedList<TPR_PROJECT_GET_Result> data = tpr_project.ToPagedList(pageIndex, pageSize);
            //return View(tpr_project.ToList());
            return View(data);
        }

        //
        // GET: /QuestionProject/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TPR_PROJECT tpr_project = db.TPR_PROJECT.Single(t => t.PROJECT_NO == id);
            if (tpr_project == null)
            {
                return HttpNotFound();
            }
            return View(tpr_project);
        }

        //
        // GET: /QuestionProject/Create

        public ActionResult Create()
        {
            ViewBag.CLASS_NO = new SelectList(db.SET_CLASS, "CLASS_NO", "CLASS_NAME");
            return View();
        }

        //
        // POST: /QuestionProject/Create

        [HttpPost]
        public ActionResult Create(TPR_PROJECT project)
        {

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            if (ModelState.IsValid)
            {
                try
                {
                    db.TPR_PROJECT_INSERT(USER_NO, LOGON_NO, project.PROJECT_NAME, project.CLASS_NO, project.DEADLINE_DATE);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("unique constraint"))
                    {
                        ModelState.AddModelError("PROJECT_NAME", "Unique Constraint Violated. "
                            + "Same Porject Name is not allowed for multiple projects.");
                    }
                    else
                    {
                        ModelState.AddModelError("DEADLINE_DATE", "An error occured while adding collection project.");
                    }
                }
            }

            ViewBag.CLASS_NO = new SelectList(db.SET_CLASS, "CLASS_NO", "CLASS_NAME", project.CLASS_NO);
            return View(project);
        }

        //
        // GET: /QuestionProject/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TPR_PROJECT tpr_project = db.TPR_PROJECT.Single(t => t.PROJECT_NO == id);
            if (tpr_project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLASS_NO = new SelectList(db.SET_CLASS, "CLASS_NO", "CLASS_NAME", tpr_project.CLASS_NO);
            return View(tpr_project);
        }

        //
        // POST: /QuestionProject/Edit/5

        [HttpPost]
        public ActionResult Edit(TPR_PROJECT project)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            if (ModelState.IsValid)
            {
                try
                {
                    db.TPR_PROJECT_UPDATE(project.PROJECT_NO, USER_NO, LOGON_NO, project.PROJECT_NAME,
                                project.CLASS_NO, project.DECLARE_DATE, project.DEADLINE_DATE, project.IS_ACTIVE);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("unique constraint"))
                    {
                        ModelState.AddModelError("PROJECT_NAME", "Unique Constraint Violated. " 
                            + "Same Porject Name is not allowed for multiple projects.");
                    }
                    else
                    {
                        ModelState.AddModelError("DEADLINE_DATE", "An error occured while adding collection project.");
                    }
                }
            }

            ViewBag.CLASS_NO = new SelectList(db.SET_CLASS, "CLASS_NO", "CLASS_NAME", project.CLASS_NO);
            return View(project);
        }

        //
        // GET: /QuestionProject/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TPR_PROJECT tpr_project = db.TPR_PROJECT.Single(t => t.PROJECT_NO == id);
            if (tpr_project == null)
            {
                return HttpNotFound();
            }
            return View(tpr_project);
        }

        //
        // POST: /QuestionProject/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.TPR_PROJECT_DELETE(id, USER_NO, LOGON_NO);

            return RedirectToAction("Index");
        }

        public ActionResult Select(decimal id = 0)
        {
            TPR_PROJECT project = db.TPR_PROJECT.Include("SET_CLASS").Single(t => t.PROJECT_NO == id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Project = project;
            ViewBag.Subjects = db.SET_CLASS_SUBJECT_GET(project.CLASS_NO).ToList();
            
            return View();
        }

        [HttpPost]
        public ActionResult Select(TPR_PROJECT_INSTITUTE pr_ins)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            decimal? project_ins_no =  db.TPR_PROJECT_INSTITUTE_INSERT(pr_ins.PROJECT_NO, pr_ins.USER_NO, 
                pr_ins.INSTITUTE_NO, pr_ins.IS_ACTIVE, USER_NO, LOGON_NO).FirstOrDefault();

            TPR_PROJECT project = db.TPR_PROJECT.Single(p => p.PROJECT_NO == pr_ins.PROJECT_NO);

            if (project_ins_no.HasValue)
            {
                foreach (var subject in pr_ins.Subjects)
                {
                    if (subject.TARGET_DATE == null)
                    {
                        subject.TARGET_DATE = project.DEADLINE_DATE;
                    }
                    decimal? is_active = subject.IS_ACTIVE ? 1 : 0;
                    db.TPR_INSTITUTE_SUBJECT_INSERT(project_ins_no, subject.SUBJECT_NO, USER_NO, LOGON_NO,
                        subject.TARGET_DATE, is_active);
                }
            }

            return RedirectToAction("GetAssignments", new { pid = pr_ins.PROJECT_NO, uid = pr_ins.USER_NO });
        }

        public ActionResult GetAssignments(decimal? pid, decimal? uid, decimal? ins_no)
        {
            var assignments = db.TPR_PROJECT_ASSIGNMENTS_GET(pid, uid, ins_no).ToList();

            return PartialView("_GetUserAssignments", assignments);
        }

        public ActionResult EditAssign(decimal id = 0)
        {
            var project_ins = db.TPR_PROJECT_INSTITUTE.Single(t => t.PROJECT_INS_NO == id);
            TPR_PROJECT project = db.TPR_PROJECT.Include("SET_CLASS").Single(p => p.PROJECT_NO == project_ins.PROJECT_NO);
            
            USER_INFO_VIEWMODEL zonal_info =
                             (from u in db.SEC_USERS
                              join d in db.SET_DESIGNATION on u.DESIG_NO equals d.DESIG_NO
                              join t in db.SET_THANA on u.THANA_NO equals t.THANA_NO
                              join z in db.SET_ZILLA on t.ZILLA_NO equals z.ZILLA_NO
                              join div in db.SET_DIVISION on z.DIVISION_NO equals div.DIVISION_NO
                              where u.USER_NO == project_ins.USER_NO
                              select new USER_INFO_VIEWMODEL
                              {
                                  userno = u.USER_NO,
                                  fullname = u.USER_FULL_NAME,
                                  username = u.USER_NAME,
                                  hrid = u.HR_EMP_ID,
                                  mobile = u.USER_MOBILE,
                                  designation = d.DESIG_NAME,
                                  thana = t.THANA_NAME,
                                  zilla = z.ZILLA_NAME,
                                  division = div.DIVISION_NAME
                              }).FirstOrDefault();

            INSTITUTE_INFO_VIEWMODEL institute_info =
                                 (from i in db.SET_INSTITUTE
                                  join t in db.SET_THANA on i.THANA_NO equals t.THANA_NO
                                  join z in db.SET_ZILLA on t.ZILLA_NO equals z.ZILLA_NO
                                  join d in db.SET_DIVISION on z.DIVISION_NO equals d.DIVISION_NO
                                  where i.INSTITUTE_NO == project_ins.INSTITUTE_NO
                                  select new INSTITUTE_INFO_VIEWMODEL
                                  {
                                      insname = i.INSTITUTE_NAME,
                                      inscode = i.INSTITUTE_NO,
                                      thana = t.THANA_NAME,
                                      zilla = z.ZILLA_NAME,
                                      division = d.DIVISION_NAME
                                  }).FirstOrDefault();

            ViewBag.Subjects = db.SET_CLASS_SUBJECT_ASSIGN_GET(id, project.CLASS_NO).ToList();
            ViewBag.PROJECT = Session["TPR_PROJECT"] = project;
            ViewBag.ZONAL_INFO = Session["TPR_ZONAL_INFO"] = zonal_info;
            ViewBag.INSTITUTE_INFO = Session["TPR_INS_INFO"] = institute_info;
            ViewBag.PROJECT_INSTITITE = Session["TPR_PROJECT_INS"] = project_ins;

            return View();
        }

        [HttpPost]
        public ActionResult EditAssign(TPR_PROJECT_INSTITUTE project_ins)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            TPR_PROJECT project = db.TPR_PROJECT.Single(p => p.PROJECT_NO == project_ins.PROJECT_NO);

            foreach (var subject in project_ins.Subjects)
            {
                if (subject.TARGET_DATE == null)
                {
                    subject.TARGET_DATE = project.DEADLINE_DATE;
                }
                decimal is_active = subject.IS_ACTIVE ? 1 : 0;
                db.SET_CLASS_SUB_ASSIGN_UPDATE(project_ins.PROJECT_INS_NO, subject.SUBJECT_NO, is_active,
                    subject.TARGET_DATE, USER_NO, LOGON_NO);
            }

            ViewBag.Subjects = db.SET_CLASS_SUBJECT_ASSIGN_GET(project_ins.PROJECT_INS_NO, project.CLASS_NO).ToList();
            ViewBag.PROJECT = project;
            ViewBag.ZONAL_INFO = Session["TPR_ZONAL_INFO"] as USER_INFO_VIEWMODEL;
            ViewBag.INSTITUTE_INFO = Session["TPR_INS_INFO"] as INSTITUTE_INFO_VIEWMODEL;
            ViewBag.PROJECT_INSTITITE = project_ins;

            return View();
        }

        public JsonResult DeleteAssign(decimal id = 0)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            TPR_PROJECT_INSTITUTE project_ins = db.TPR_PROJECT_INSTITUTE.Single(t => t.PROJECT_INS_NO == id);

            if (project_ins == null)
            {
                return Json(new { message = "not found" }, JsonRequestBehavior.AllowGet);
            }
            if (USER_NO.HasValue)
            {
                db.TPR_PROJECT_INSTITUTE_UPDATE(project_ins.PROJECT_INS_NO, project_ins.PROJECT_NO, project_ins.USER_NO,
                    project_ins.INSTITUTE_NO, USER_NO, LOGON_NO, 0);

                return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "no success"}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubjects(decimal? p_ins, string type)
        {
            ViewBag.Download_Allowed = false;
            ViewBag.Reset_Allowed = false;
            
            SEC_USERS_LOGIN_Result1 user = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;

            if (user.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser)
            {
                List<SET_USER_ACTION> perm_list = Session["sess_PERMISSION_LIST"] as List<SET_USER_ACTION>;
                foreach (var perm in perm_list)
                {
                    GEN_CONTROLLER_ACTION controller_action = perm.GEN_CONTROLLER_ACTION;
                    if (controller_action.CONTROLLER_NAME == "QuestionMonitor" && controller_action.ACTION_NAME == "Download")
                    {
                        ViewBag.Download_Allowed = true;
                    }
                    if (controller_action.CONTROLLER_NAME == "QuestionMonitor" && controller_action.ACTION_NAME == "Reset")
                    {
                        ViewBag.Reset_Allowed = true;
                    }
                }
            }
            else
            {
                List<SET_ROLE_ACTION> perm_list = Session["sess_PERMISSION_LIST"] as List<SET_ROLE_ACTION>;
                foreach (var perm in perm_list)
                {
                    GEN_CONTROLLER_ACTION controller_action = perm.GEN_CONTROLLER_ACTION;
                    if (controller_action.CONTROLLER_NAME == "QuestionMonitor" && controller_action.ACTION_NAME == "Download")
                    {
                        ViewBag.Download_Allowed = true;
                    }
                    if (controller_action.CONTROLLER_NAME == "QuestionMonitor" && controller_action.ACTION_NAME == "Reset")
                    {
                        ViewBag.Reset_Allowed = true;
                    }
                }
            }

            TPR_PROJECT_INSTITUTE pins = db.TPR_PROJECT_INSTITUTE
                .Single(t => t.PROJECT_INS_NO == p_ins);

            TPR_PROJECT project = db.TPR_PROJECT.Include("SET_CLASS")
                .Single(p => p.PROJECT_NO == pins.PROJECT_NO);

            USER_INFO_VIEWMODEL zonal_info = 
                             (from u in db.SEC_USERS
                             join d in db.SET_DESIGNATION on u.DESIG_NO equals d.DESIG_NO
                             join t in db.SET_THANA on u.THANA_NO equals t.THANA_NO
                             join z in db.SET_ZILLA on t.ZILLA_NO equals z.ZILLA_NO
                             join div in db.SET_DIVISION on z.DIVISION_NO equals div.DIVISION_NO
                             where u.USER_NO == pins.USER_NO
                             select new USER_INFO_VIEWMODEL
                             {
                                 userno = u.USER_NO,
                                 fullname = u.USER_FULL_NAME,
                                 username = u.USER_NAME,
                                 hrid = u.HR_EMP_ID,
                                 mobile = u.USER_MOBILE,
                                 designation = d.DESIG_NAME,
                                 thana = t.THANA_NAME,
                                 zilla = z.ZILLA_NAME,
                                 division = div.DIVISION_NAME
                             }).FirstOrDefault();

            INSTITUTE_INFO_VIEWMODEL institute_info = 
                                 (from i in db.SET_INSTITUTE
                                 join t in db.SET_THANA on i.THANA_NO equals t.THANA_NO
                                 join z in db.SET_ZILLA on t.ZILLA_NO equals z.ZILLA_NO
                                 join d in db.SET_DIVISION on z.DIVISION_NO equals d.DIVISION_NO
                                 where i.INSTITUTE_NO == pins.INSTITUTE_NO
                                 select new INSTITUTE_INFO_VIEWMODEL
                                 {
                                     insname = i.INSTITUTE_NAME,
                                     inscode = i.INSTITUTE_NO,
                                     thana = t.THANA_NAME,
                                     zilla = z.ZILLA_NAME,
                                     division = d.DIVISION_NAME
                                 }).FirstOrDefault();

            ViewBag.PROJECT = project;
            ViewBag.ZONAL_INFO = zonal_info;
            ViewBag.INSTITUTE_INFO = institute_info;
            ViewBag.PROJECT_INSTITUTE = pins;

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            var subjects = db.TPR_SUBJECTS_GET(p_ins, type, 0, null, USER_NO, LOGON_NO).ToList();

            return PartialView("_GetSubjects", subjects);
        }

        public JsonResult SearchInstitute(decimal id, string cell)
        {
            IList<TABULAR_SEARCH_USER_INS_GET_Result> result = 
                db.TABULAR_SEARCH_USER_INS_GET(id, cell).ToList();
            var institute = (from data in result
                        select new
                        {
                            institute = (data.INSTITUTE_NAME != null) ? data.INSTITUTE_NAME : " ",
                            inscode = data.INSTITUTE_NO,
                            division = (data.DIVISION_NAME != null) ? data.DIVISION_NAME : " ",
                            zilla = (data.ZILLA_NAME != null) ? data.ZILLA_NAME : " ",
                            thana = (data.THANA_NAME != null) ? data.THANA_NAME : " ",
                        }).ToList();

            return Json(institute, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadView()
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            //List<TPR_USER_PROJECTS_GET_Result> tpr_projects = db.TPR_USER_PROJECTS_GET(USER_NO).ToList();

            var tpr_projects = db.TPR_PROJECT_GET(null, null, null, null, null, null, null, null, USER_NO, 1, 10);

            return View(tpr_projects);
        }

        public ActionResult UploadSelect(decimal id = 0)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            TPR_PROJECT project = db.TPR_PROJECT.Include("SET_CLASS").Single(t => t.PROJECT_NO == id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Project = project;
            ViewBag.Institutes = db.TPR_PROJECT_ASSIGNMENTS_GET(id, USER_NO, null).ToList();
            ViewBag.USER_NO = USER_NO;

            return View();
        }

        public ActionResult GetUploadSubjects(decimal? pid, decimal? uid, decimal? ins_no)
        {
            List<TPR_UPLOAD_SUBJECTS_GET_Result> subjects = db.TPR_UPLOAD_SUBJECTS_GET(pid, uid, ins_no).ToList();
            return PartialView("_GetUploadSubjects", subjects);
        }

        public ActionResult GetUploadForm(decimal id = 0)
        {
            decimal ins_subject_no = id;

            ViewBag.Ins_Subject_No = ins_subject_no;

            return PartialView("_GetUploadForm");
        }

        [ActionName("Institute")]
        public ActionResult GetAssignedInstituteForm(decimal p_no = 0, decimal u_no = 0)
        {
            ViewBag.Division_No = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Zilla_No = new SelectList(db.SET_ZILLA.Where(z => z.ZILLA_NO < 0),
                "ZILLA_NO", "ZILLA_NAME");
            ViewBag.Zonal_Thana = new SelectList(db.SET_THANA.Where(t => t.THANA_NO < 0),
                "THANA_NO", "THANA_NAME");

            ViewBag.PROJECT_NO = p_no;
            ViewBag.USER_NO = u_no;

            return PartialView("_AssignedInstituteFormPartial");
        }

        public ActionResult GetUploadFiles(decimal id = 0)
        {
            List<TPR_UPLOAD_LIST> uploads = db.TPR_UPLOAD_LIST.Where(t => t.INS_SUBJECT_NO == id).ToList();

            return PartialView("_GetUploadFiles", uploads);
        }

        [HttpPost]
        public ActionResult AttachFile(TPR_UPLOAD_LIST upload)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            if (upload.QuestionFile != null && upload.QuestionFile.ContentLength > 0)
            {
                int length = upload.QuestionFile.ContentLength;

                upload.FILE_NAME = upload.QuestionFile.FileName;
                upload.FILE_TYPE = upload.QuestionFile.ContentType;
                upload.FILE_EXT = Path.GetExtension(upload.QuestionFile.FileName);

                if (!System.IO.Directory.Exists(question_upload_url))
                {
                    System.IO.Directory.CreateDirectory(question_upload_url);
                }

                string file_new_name = Path.GetFileNameWithoutExtension(upload.FILE_NAME)
                                        + DateTime.Now.ToString("_yyyy_MM_dd_hh_mm_ss_") + DateTime.Now.Millisecond
                                        + Path.GetExtension(upload.FILE_NAME);
                string fileLocation = question_upload_url + file_new_name;
                while (System.IO.File.Exists(fileLocation))
                {
                    file_new_name = Path.GetFileNameWithoutExtension(upload.FILE_NAME)
                                        + DateTime.Now.ToString("_yyyy_MM_dd_hh_mm_ss_") + DateTime.Now.Millisecond
                                        + Path.GetExtension(upload.FILE_NAME);

                    fileLocation = question_upload_url + file_new_name;
                }
                upload.QuestionFile.SaveAs(fileLocation);

                db.TPR_UPLOAD_LIST_INSERT(upload.INS_SUBJECT_NO, USER_NO, LOGON_NO, upload.FILE_NAME,
                    upload.FILE_TYPE, upload.FILE_EXT, fileLocation, file_new_name);
            }

            TPR_PROJECT_INSTITUTE pro_ins = db.TPR_INSTITUTE_SUBJECT.Include("TPR_PROJECT_INSTITUTE")
                                                    .Single(i => i.INS_SUBJECT_NO == upload.INS_SUBJECT_NO)
                                                    .TPR_PROJECT_INSTITUTE;
            
            return RedirectToAction("GetUploadSubjects", new
            {
                pid = pro_ins.PROJECT_NO,
                uid = pro_ins.USER_NO,
                ins_no = pro_ins.INSTITUTE_NO
            });
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
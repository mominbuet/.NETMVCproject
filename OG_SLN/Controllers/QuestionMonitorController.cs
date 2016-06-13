using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Utils;
using OG_SLN.Models;

using PagedList;

namespace OG_SLN.Controllers
{
    public class QuestionMonitorController : Controller
    {
        private OGDBEntities db = new OGDBEntities();
        private int pageSize = 10;

        private string question_upload_url = System.Configuration.ConfigurationManager.AppSettings["QUESTION_UPLOAD_URL"];

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
            List<TPR_PROJECT_MONITOR_GET_Result> tpr_project = db.TPR_PROJECT_MONITOR_GET(null, null, null, 
                null, null, null, null, null, USER_NO, startFrom, pageSize).ToList();
            IPagedList<TPR_PROJECT_MONITOR_GET_Result> data = tpr_project.ToPagedList(pageIndex, pageSize);
            return View(data);
        }

        public ActionResult MonitorSelect(decimal id = 0)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            TPR_PROJECT project = db.TPR_PROJECT.Include("SET_CLASS").Single(t => t.PROJECT_NO == id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Assigned_List = db.TPR_ASSIGN_MONITOR_GET(project.PROJECT_NO,
                null, null, null, null, USER_NO).ToList();

            ViewBag.Project = project;
            ViewBag.Subjects = db.SET_CLASS_SUBJECT_GET(project.CLASS_NO).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult MonitorSelect(TPR_PROJECT_INSTITUTE pr_ins)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            decimal? user_no = pr_ins.USER_NO;
            if (user_no == 0) user_no = null;

            decimal? ins_no = pr_ins.INSTITUTE_NO;
            if (ins_no == 0) ins_no = null;

            List<TPR_ASSIGN_MONITOR_GET_Result> assigned_list =  db.TPR_ASSIGN_MONITOR_GET(pr_ins.PROJECT_NO,
                user_no, ins_no, pr_ins.Target_Date, pr_ins.Subject_List, USER_NO).ToList();

            return PartialView("_GetAssignedList", assigned_list);
        }

        [HttpPost]
        public ActionResult GetInstituteAssigned(TPR_PROJECT_INSTITUTE pr_ins)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            USER_INFO_VIEWMODEL zonal_info =
                             (from u in db.SEC_USERS
                              join d in db.SET_DESIGNATION on u.DESIG_NO equals d.DESIG_NO
                              join t in db.SET_THANA on u.THANA_NO equals t.THANA_NO
                              join z in db.SET_ZILLA on t.ZILLA_NO equals z.ZILLA_NO
                              join div in db.SET_DIVISION on z.DIVISION_NO equals div.DIVISION_NO
                              where u.USER_NO == pr_ins.USER_NO
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

            decimal? ins_no = pr_ins.INSTITUTE_NO;
            if (ins_no == 0) ins_no = null;

            var assignments = db.TPR_MONITOR_DETAIL_GET(pr_ins.PROJECT_NO, pr_ins.USER_NO, 
                ins_no, pr_ins.Target_Date, pr_ins.Subject_List, pr_ins.Is_Collected, pr_ins.Is_New, USER_NO).ToList();

            ViewBag.ZONAL_INFO = zonal_info;
            ViewBag.Is_New = pr_ins.Is_New;
            ViewBag.Is_Collected = pr_ins.Is_Collected;

            return PartialView("_GetInstituteAssignments", assignments);
        }

        public ActionResult GetSubjects(TPR_PROJECT_INSTITUTE pr_ins)
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
                .Single(t => t.PROJECT_INS_NO == pr_ins.Proj_Ins_No);

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

            string type = pr_ins.Is_Collected == 1 ? "C" : "A";
            var subjects = db.TPR_SUBJECTS_GET(pr_ins.Proj_Ins_No, type, pr_ins.Is_New, pr_ins.Subject_List, USER_NO, LOGON_NO).ToList();

            return PartialView("_GetSubjects", subjects);
        }

        public ZipResult Download(decimal id = 0)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            SEC_USER_LOGONS logon = db.SEC_USER_LOGONS.Single(l => l.LOGON_NO == LOGON_NO);

            List<TPR_IP_LIST> ip_list = db.TPR_IP_LIST.Where(i => i.IP_VAL == logon.IP_ADDR && i.IS_ACTIVE == 1).ToList();

            if (ip_list.Count > 0)
            {
                TPR_INSTITUTE_SUBJECT ins_subject = db.TPR_INSTITUTE_SUBJECT
                    .Include("SET_SUBJECT")
                    .Single(u => u.INS_SUBJECT_NO == id);

                List<TPR_UPLOAD_LIST> downloads = db.TPR_UPLOAD_LIST.Where(u => u.INS_SUBJECT_NO == id).ToList();

                List<string> files = new List<string>();

                foreach (var file in downloads)
                {
                    files.Add(question_upload_url + file.FILE_NEW_NAME);
                }

                string subject_name = ins_subject.SET_SUBJECT.SUBJECT_NAME.Replace(' ', '-');

                db.TPR_USER_DOWNLOAD_INSERT(id, USER_NO, USER_NO, LOGON_NO, 1);

                return new ZipResult(files, subject_name);
            }
            else
            {
                db.TPR_USER_DOWNLOAD_INSERT(id, USER_NO, USER_NO, LOGON_NO, 0);

                return new ZipResult(null);
            }
        }

        public JsonResult Reset(decimal id = 0)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.TPR_UPLOAD_RESET(id, USER_NO, LOGON_NO);
            return Json(new { message = "success"}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
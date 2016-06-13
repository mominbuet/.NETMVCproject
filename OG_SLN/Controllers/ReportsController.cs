using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using System.Data.Common;
using OG_SLN.Models;
using System.Data.Entity.Infrastructure;
using OG_SLN.ProcedurePagedList;
using System.Data.Objects;


namespace OG_SLN.Controllers
{
    public class ReportsController : Controller
    {
        private OGDBEntities db = new OGDBEntities();
        private int page_size = 3;

        public ActionResult ZonalWiseSpecimen(int? page)
        {
            int pageIndex = 1;
            int totalItemCount = 104;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            List<VIEW_TRN_DCR_APPROVE_GET_Result2> data_list =
                db.VIEW_TRN_DCR_APPROVE_GET(null, null, null, null, null, null, null, null, null, null, null,
                null, null, null, null, null, 65, 1).ToList();

            IProcPagedList<VIEW_TRN_DCR_APPROVE_GET_Result2> data =
                data_list.ToProcPagedList(pageIndex, page_size, totalItemCount);

            return View(data);
        }

        public ActionResult DcrPublish()
        {
            DcrPublishSearchModel searchModel = new DcrPublishSearchModel();
            DcrPublishViewModel dcrPublishModel = new DcrPublishViewModel();

            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            ViewBag.Search_Division = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(USER_NO,
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");
            
            bool Is_ZonalOrAgent = (bool)Session["sess_Is_ZonalOrAgent"];
            ViewBag.Is_ZonalOrAgent = Is_ZonalOrAgent;

            string Search_User = Request.QueryString["Search_User"];
            if (string.IsNullOrEmpty(Search_User))
            {
                Search_User = Request.QueryString["USER_NO"];
            }

            if (Is_ZonalOrAgent)
            {
                Search_User = Session["sess_USER_NO"].ToString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search"]))
            {
                searchModel.Search_User = decimal.Parse(Search_User);
                ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                    null, null, null).ToList(), "USER_NO", "USER_FULL_NAME", searchModel.Search_User);

                SEC_USERS user = db.SEC_USERS.Single(u => u.USER_NO == searchModel.Search_User);

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
                {
                    searchModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    DateTime FirstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                    searchModel.Search_Date_From = FirstDayOfMonth;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
                {
                    searchModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
                }
                else
                {
                    searchModel.Search_Date_To = DateTime.Now;
                }

                List<DCR_PUB_EXP_GET_Result> expense =
                    db.DCR_PUB_EXP_GET(searchModel.Search_User, null,
                        searchModel.Search_Date_From, searchModel.Search_Date_To, 0).ToList();

                List<DCR_PUB_SPECIMEN_GET_Result> specimen =
                    db.DCR_PUB_SPECIMEN_GET(searchModel.Search_User, null,
                        searchModel.Search_Date_From, searchModel.Search_Date_To, 0).ToList();

                dcrPublishModel.expenses = new DcrExpenseModel();
                dcrPublishModel.expenses.user = user;
                dcrPublishModel.expenses.date = DateTime.Now;
                dcrPublishModel.expenses.expense = expense;

                dcrPublishModel.specimens = new DcrSpecimenModel();
                dcrPublishModel.specimens.user = user;
                dcrPublishModel.specimens.date = DateTime.Now;
                dcrPublishModel.specimens.specimen = specimen;

                if (!string.IsNullOrEmpty(Request.QueryString["Print"]))
                {
                    string print = Request.QueryString["Print"];
                    dcrPublishModel.details = new List<DcrDetailsModel>();

                    if (print.Length > 0 && print == "Y")
                    {
                        dcrPublishModel.expenses.date = searchModel.Search_Date_From;
                        dcrPublishModel.specimens.date = searchModel.Search_Date_From;

                        for (DateTime? date = searchModel.Search_Date_From;
                            date <= searchModel.Search_Date_To; date = date.Value.AddDays(1))
                        {
                            DcrDetailsModel dcrDetails = new DcrDetailsModel();

                            List<DCR_PUB_DCR_DETAIL_GET_Result> detail =
                                db.DCR_PUB_DCR_DETAIL_GET(searchModel.Search_User, null, date, date, 0).ToList();

                            List<TRN_EXPENSE_APPROVAL_GET_Result> detailexpense =
                                db.TRN_EXPENSE_APPROVAL_GET(
                                searchModel.Search_User, decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                                null, null, null, date, date, 1, 1, null).ToList();

                            dcrDetails.date = date;
                            dcrDetails.expense = detailexpense;
                            dcrDetails.detail = detail;

                            dcrPublishModel.details.Add(dcrDetails);
                        }
                    }

                    ViewBag.Print = print;

                    return PartialView("_DcrPublishPrint", dcrPublishModel);
                }

                ViewBag.Search_DcrPublish_Model = searchModel;

                Session["sess_Search_DcrPublish_Model"] = searchModel;

                return View(dcrPublishModel);
            }

            return View();
        }



     
 

        [HttpPost]
        public JsonResult DcrDoPublish(DcrPublishSearchModel model)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;


            db.DCR_PUB_DO_PUBLISH(model.User_list, USER_NO, LOGON_NO, model.Search_Date_From,
                 model.Search_Date_To, model.Search_Date_From, model.Search_Date_To, 
                 model.Dcr_Amt, model.Der_Amt, model.Sd_Cnt, model.Comments);


            return Json(new { users = model.User_list, fromd = model.Search_Date_From, todat = model.Search_Date_To });

        }

        public ActionResult DcrEdit(decimal? id)
        {
            var trn_dcr = db.TRN_DCR.Single(d => d.DCR_NO == id);

            ViewBag.TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE,
                "TRANS_TYPE_NO", "TRANS_TYPE_CODE", trn_dcr.APPROVE_TRANS_TYPE_NO);

            return PartialView("_DcrEdit", trn_dcr);
        }

        [HttpPost]
        public JsonResult DcrEdit(TRN_DCR trn_dcr)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.TRN_DCR_DO_APPROVE(trn_dcr.DCR_NO, 1, USER_NO, LOGON_NO, null, null,
                trn_dcr.TRANS_TYPE_NO, trn_dcr.APPROVE_FARE_AMT);

            return Json(new { msg = "success" });
        }

        public ActionResult ExpenseEdit(decimal? id)
        {
            var exp_det = db.TRN_EXPENSE_DET.Single(e => e.EXP_DET_NO == id);

            return PartialView("_ExpenseEdit", exp_det);
        }

        [HttpPost]
        public JsonResult ExpenseEdit(TRN_EXPENSE_DET exp_det)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            db.TRN_EXPENSE_DET_DO_APPROVE(exp_det.EXP_DET_NO, 1, USER_NO, LOGON_NO, null, null,
                exp_det.APPROVE_EXP_TYPE_NO, exp_det.APPROVE_EXP_AMT);

            return Json(new { msg = "success" });
        }

        [HttpPost]
        public ActionResult GetDcrDetailsForDate(DcrPublishSearchModel searchModel)
        {
            DcrDetailsModel dcrDetails = new DcrDetailsModel();

            bool Is_ZonalOrAgent = (bool)Session["sess_Is_ZonalOrAgent"];
            ViewBag.Is_ZonalOrAgent = Is_ZonalOrAgent;

            if (Is_ZonalOrAgent)
            {
                searchModel.Search_User = decimal.Parse(Session["sess_USER_NO"].ToString());
            }

            List<DCR_PUB_DCR_DETAIL_GET_Result> detail =
                db.DCR_PUB_DCR_DETAIL_GET(searchModel.Search_User, null,
                    searchModel.Search_Date_From, searchModel.Search_Date_To, 0).ToList();

            List<TRN_EXPENSE_APPROVAL_GET_Result> detailexpense =
                db.TRN_EXPENSE_APPROVAL_GET(
                    searchModel.Search_User, decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                    null, null, null, searchModel.Search_Date_From, searchModel.Search_Date_To, 1, 1, null).ToList();

            dcrDetails.date = searchModel.Search_Date_From;
            dcrDetails.expense = detailexpense;
            dcrDetails.detail = detail;

            ViewBag.Print = String.Empty;

            return PartialView("_DcrDetailsForDate", dcrDetails);
        }

        public ActionResult Downloads()
        {
            ObjectResult<DOWNLOAD_USERS_GET_Result> downloads = null;
            UpDownSearchModel model = null;

            ViewBag.Search_Division = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");

            if (Request.QueryString.HasKeys())
            {
                model = new UpDownSearchModel();
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Division"])
                    && decimal.Parse(Request.QueryString["Search_Division"]) != 0)
                {
                    model.Search_Division = decimal.Parse(Request.QueryString["Search_Division"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Zilla"])
                    && decimal.Parse(Request.QueryString["Search_Zilla"]) != 0)
                {
                    model.Search_Zilla = decimal.Parse(Request.QueryString["Search_Zilla"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Thana"])
                    && decimal.Parse(Request.QueryString["Search_Thana"]) != 0)
                {
                    model.Search_Thana = decimal.Parse(Request.QueryString["Search_Thana"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User"])
                    && decimal.Parse(Request.QueryString["Search_User"]) != 0)
                {
                    model.Search_User = decimal.Parse(Request.QueryString["Search_User"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_From"]))
                {
                    model.Search_From = DateTime.Parse(Request.QueryString["Search_From"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_To"]))
                {
                    model.Search_To = DateTime.Parse(Request.QueryString["Search_To"]);
                }

                downloads = db.DOWNLOAD_USERS_GET(model.Search_Division, model.Search_Zilla,
                    model.Search_Thana, model.Search_User, model.Search_From, model.Search_To);
            }

            ViewBag.Search_Model = model;

            return View(downloads);
        }

        [HttpPost]
        public ActionResult GetDownloadDetails(DownloadDetailsSearchModel searchModel)
        {
            DownloadDetailsViewModel model = new DownloadDetailsViewModel();

            List<DOWNLOAD_USERS_DET_GET_Result> details =
            db.DOWNLOAD_USERS_DET_GET(searchModel.User_No, searchModel.Date_From, searchModel.Date_To).ToList();

            model.User_Name = searchModel.User_Name;
            model.Division_Name = searchModel.Division_Name;
            model.Zilla_Name = searchModel.Zilla_Name;
            model.Thana_Name = searchModel.Thana_Name;
            model.Date = searchModel.Date_From;

            model.details = details;

            return PartialView("_getDownloadDetails", model);
        }

        public ActionResult Uploads()
        {
            ObjectResult<UPLOAD_USERS_GET_Result> uploads = null;
            UpDownSearchModel model = null;

            ViewBag.Search_Division = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");

            if (Request.QueryString.HasKeys())
            {
                model = new UpDownSearchModel();
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Division"])
                    && decimal.Parse(Request.QueryString["Search_Division"]) != 0)
                {
                    model.Search_Division = decimal.Parse(Request.QueryString["Search_Division"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Zilla"])
                    && decimal.Parse(Request.QueryString["Search_Zilla"]) != 0)
                {
                    model.Search_Zilla = decimal.Parse(Request.QueryString["Search_Zilla"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Thana"])
                    && decimal.Parse(Request.QueryString["Search_Thana"]) != 0)
                {
                    model.Search_Thana = decimal.Parse(Request.QueryString["Search_Thana"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_User"])
                    && decimal.Parse(Request.QueryString["Search_User"]) != 0)
                {
                    model.Search_User = decimal.Parse(Request.QueryString["Search_User"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_From"]))
                {
                    model.Search_From = DateTime.Parse(Request.QueryString["Search_From"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_To"]))
                {
                    model.Search_To = DateTime.Parse(Request.QueryString["Search_To"]);
                }

                uploads = db.UPLOAD_USERS_GET(model.Search_Division, model.Search_Zilla,
                    model.Search_Thana, model.Search_User, model.Search_From, model.Search_To);
            }

            ViewBag.Search_Model = model;

            return View(uploads);
        }

        public ActionResult GetUploadDetails(UploadDetailsSearchModel searchModel)
        {
            UploadDetailsViewModel model = new UploadDetailsViewModel();

            List<UPLOAD_USERS_DET_GET_Result> details =
            db.UPLOAD_USERS_DET_GET(searchModel.User_No, searchModel.Date_From, searchModel.Date_To).ToList();

            model.User_Name = searchModel.User_Name;
            model.Division_Name = searchModel.Division_Name;
            model.Zilla_Name = searchModel.Zilla_Name;
            model.Thana_Name = searchModel.Thana_Name;
            model.Date = searchModel.Date_From;

            model.details = details;

            return PartialView("_getUploadDetails", model);
        }

        public ActionResult AppVersion()
        {
            ViewBag.app_version = new SelectList(db.GEN_APP_VERSION
                .OrderByDescending(a => a.IS_CURR_VER), "APP_VERSION", "APP_VERSION");

            AppVersionSearchModel model = null;
            ObjectResult<SEC_USERS_APP_VERSION_GET_Result> app_versions = null;

            if (Request.QueryString.HasKeys())
            {
                model = new AppVersionSearchModel();
                if (!string.IsNullOrEmpty(Request.QueryString["app_version"]))
                {
                    model.app_version = Request.QueryString["app_version"];
                    ViewBag.app_version = new SelectList(db.GEN_APP_VERSION
                        .OrderByDescending(a => a.IS_CURR_VER), "APP_VERSION", "APP_VERSION", model.app_version);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["is_current"]))
                {
                    model.is_current = bool.Parse(Request.QueryString["is_current"]);
                }

                decimal is_current = !model.is_current ? 1 : 0;

                app_versions = db.SEC_USERS_APP_VERSION_GET(model.app_version, is_current);
            }

            ViewBag.Search_Model = model;

            return View(app_versions);
        }

        public ActionResult SpecimenAdjustment()
        {
            decimal? USER_NO = decimal.Parse(Session["sess_USER_NO"].ToString());
            SpecimenAdjustmentSearchModel searchModel = new SpecimenAdjustmentSearchModel();

            ViewBag.Search_Division = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");

            bool Is_ZonalOrAgent = (bool)Session["sess_Is_ZonalOrAgent"];
            ViewBag.Is_ZonalOrAgent = Is_ZonalOrAgent;

            string Search_User = Request.QueryString["Search_User"];

            if (Is_ZonalOrAgent)
            {
                Search_User = Session["sess_USER_NO"].ToString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Search"]))
            {
                if (!string.IsNullOrEmpty(Search_User))
                {
                    searchModel.Search_User = decimal.Parse(Search_User);
                }

                ViewBag.Search_User = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                    null, null, null).ToList(), "USER_NO", "USER_FULL_NAME", searchModel.Search_User);

                //SEC_USERS user = db.SEC_USERS.Single(u => u.USER_NO == searchModel.Search_User);

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
                {
                    searchModel.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    DateTime FirstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                    searchModel.Search_Date_From = FirstDayOfMonth;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
                {
                    searchModel.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
                }
                else
                {
                    searchModel.Search_Date_To = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Specimen"]))
                {
                    searchModel.Search_Specimen = decimal.Parse(Request.QueryString["Search_Specimen"]);
                    ViewBag.Specimen = db.SET_SPECIMEN
                        .Where(s => s.SPECIMEN_NO == searchModel.Search_Specimen).FirstOrDefault();
                }

                List<SPECIMEN_ADJUSTMENT_GET_Result> specimen =
                    db.SPECIMEN_ADJUSTMENT_GET(searchModel.Search_User, USER_NO,
                        searchModel.Search_Date_From, searchModel.Search_Date_To,
                        searchModel.Search_Specimen).ToList();


                ViewBag.Search_Specimen_Adjustment_Model = searchModel;

                Session["sess_Search_Specimen_Adjustment_Model"] = searchModel;

                return View(specimen);
            }
            else
            {
                DateTime now = DateTime.Now;
                DateTime FirstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                searchModel.Search_Date_From = FirstDayOfMonth;

                searchModel.Search_Date_To = DateTime.Now;

                List<SPECIMEN_ADJUSTMENT_GET_Result> specimen =
                    db.SPECIMEN_ADJUSTMENT_GET(searchModel.Search_User, USER_NO,
                        searchModel.Search_Date_From, searchModel.Search_Date_To,
                        searchModel.Search_Specimen).ToList();


                ViewBag.Search_Specimen_Adjustment_Model = searchModel;

                Session["sess_Search_Specimen_Adjustment_Model"] = searchModel;

                return View(specimen);
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetAdjustmentUsers(AdjustmentUserSearchModel searchModel)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            AdjustmentUserRecordModel specimenUsers = new AdjustmentUserRecordModel();

            List<SPECIMEN_ADJUST_USERWISE_GET_Result> userRecords =
                db.SPECIMEN_ADJUST_USERWISE_GET(null, USER_NO, searchModel.Date_From, searchModel.Date_To,
                    searchModel.Specimen_No).ToList();

            specimenUsers.Specimen_No = searchModel.Specimen_No;
            specimenUsers.Specimen_Name = searchModel.Specimen_Name;
            specimenUsers.Specimen_Name_Bng = searchModel.Specimen_Name_Bng;
            specimenUsers.Specimen_Code = searchModel.Specimen_Code;
            specimenUsers.Date_From = searchModel.Date_From;
            specimenUsers.Date_To = searchModel.Date_To;

            specimenUsers.Specimen_Users = userRecords;

            return PartialView("_getAdjustmentUsers", specimenUsers);
        }
        [HttpPost]
        public ActionResult GetAdjustmentApprovedDetails(AdjustmentApprovedDetailsSearchModel searchModel)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            AdjustmentApprovedDetailsViewModel approvedDetails = new AdjustmentApprovedDetailsViewModel();

            List<SPECIMEN_ADJUST_DIST_GET_Result> approvedRecords =
                db.SPECIMEN_ADJUST_DIST_GET(searchModel.User_No, USER_NO, searchModel.Date_From, searchModel.Date_To, searchModel.Specimen_No).ToList();

            approvedDetails.User_Name = searchModel.User_Name;
            approvedDetails.Specimen_No = searchModel.Specimen_No;
            approvedDetails.Specimen_Name = searchModel.Specimen_Name;
            approvedDetails.Specimen_Name_Bng = searchModel.Specimen_Name_Bng;
            approvedDetails.Specimen_Code = searchModel.Specimen_Code;
            approvedDetails.Date_From = searchModel.Date_From;
            approvedDetails.Date_To = searchModel.Date_To;

            approvedDetails.Approved_Details = approvedRecords;

            return PartialView("_getApprovedDetails", approvedDetails);
        }

        public ActionResult Dashboard()
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;

            DashboardSearchModel model = new DashboardSearchModel();
            ObjectResult<DASHBOARD_REPORT_Result> dashboard = null;

            model.Search_AM_User = null;

            DateTime now = DateTime.Now;
            DateTime FirstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            model.Search_Date_From = FirstDayOfMonth;

            model.Search_Date_To = DateTime.Now;

            if (!string.IsNullOrEmpty(Request.QueryString["Search"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Search_AM_User"]))
                {
                    model.Search_AM_User = decimal.Parse(Request.QueryString["Search_AM_User"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_From"]))
                {
                    model.Search_Date_From = DateTime.Parse(Request.QueryString["Search_Date_From"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["Search_Date_To"]))
                {
                    model.Search_Date_To = DateTime.Parse(Request.QueryString["Search_Date_To"]);
                }
            }

            dashboard = db.DASHBOARD_REPORT(null, model.Search_AM_User, USER_NO,
                                            model.Search_Date_From, model.Search_Date_To);

            ViewBag.Search_Model = model;

            return View(dashboard);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OG_SLN.Models;

namespace OG_SLN.Controllers
{
    public class DcrCostApprovalController : Controller
    {
        //
        // GET: /DcrCostApproval/

        private OGDBEntities db = new OGDBEntities();


        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult CostAmmendment()
        {

            SearchDCRCostApproval dcrSearch = (Session["SearchDCRCostApproval"] == null) ? new SearchDCRCostApproval()
                : (SearchDCRCostApproval)Session["SearchDCRCostApproval"];
            ViewBag.dcrSearch = dcrSearch;
            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.GEN_USER_TYPES.SHORT_NAME == "ZM");
            ViewBag.fromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.toDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();
            ViewBag.verify_status = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE");
            ViewBag.ddldivision = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.zmusers = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");
            //error in this
            IList<TRN_EXPENSE_APPROVAL_GET_Result> tmp = db.TRN_EXPENSE_APPROVAL_GET(
                null,//user
                decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                null,//division
                null,//zilla
                null,//thana
                DateTime.Now,//date from
                DateTime.Now,//date to
                0, 1, null).ToList();
            ViewBag.expenses = tmp;

            List<TRN_DCR_APPROVAL_GET_Result> dcr_List = db.TRN_DCR_APPROVAL_GET(dcrSearch.dcrType,//dcr type
                null,//user no
                decimal.Parse(Session["sess_USER_NO"].ToString()),
                null,//division
                null,//zilla
                null,//thana
                DateTime.Now, //date to
                DateTime.Now,//date from
                0, null).ToList();

            ViewBag.dcr_List = dcr_List;

            decimal? dcr_count = db.TRN_DCR_APPROVAL_COUNT(decimal.Parse(Session["sess_USER_NO"].ToString())).FirstOrDefault();
            ViewBag.dcr_count = dcr_count;

            return View();
        }
        [HttpPost]
        public ActionResult CostAmmendment(DcrVerifyApproveSearch search_obj)
        {
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();

            IList<TRN_EXPENSE_APPROVAL_GET_Result> tmp = db.TRN_EXPENSE_APPROVAL_GET(
                search_obj.USER_NO,//user
                decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                search_obj.ddldivision,//division
                search_obj.ddlZillas,//zilla
                search_obj.ddlThanas,//thana
                search_obj.DateFrom,//date from
                search_obj.DateTo,//date to
                search_obj.verify_status, 1, search_obj.fare_cost).ToList();
            ViewBag.expenses = tmp;
            List<TRN_DCR_APPROVAL_GET_Result> dcr_List = db.TRN_DCR_APPROVAL_GET(search_obj.dcrType,//dcr type
                search_obj.USER_NO,//user no
                decimal.Parse(Session["sess_USER_NO"].ToString()),
                search_obj.ddldivision,//division
                search_obj.ddlZillas,//zilla
                search_obj.ddlThanas,//thana
                search_obj.DateFrom,//date from
                search_obj.DateTo,//date to
                search_obj.verify_status, search_obj.fare_cost).ToList();

            ViewBag.dcr_List = dcr_List;

            return View("_DcrAmend");
        }
        public ActionResult Index()
        {
            
            SearchDCRCostApproval dcrSearch = (Session["SearchDCRCostApproval"] == null) ? new SearchDCRCostApproval()
                : (SearchDCRCostApproval)Session["SearchDCRCostApproval"];
            ViewBag.dcrSearch = dcrSearch; 
            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.GEN_USER_TYPES.SHORT_NAME == "ZM");
            ViewBag.fromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.toDate = DateTime.Now.ToString("yyyy-MM-dd"); 
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();
            ViewBag.verify_status = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE");
            ViewBag.ddldivision = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.zmusers = new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()), 
                null, null, null).ToList(), "USER_NO", "USER_FULL_NAME");
            //error in this
            IList <TRN_EXPENSE_APPROVAL_GET_Result> tmp = db.TRN_EXPENSE_APPROVAL_GET(
                null,//user
                decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                null,//division
                null,//zilla
                null,//thana
                DateTime.Now,//date from
                DateTime.Now,//date to
                0,1,null).ToList();
            ViewBag.expenses = tmp;

            List<TRN_DCR_APPROVAL_GET_Result> dcr_List = db.TRN_DCR_APPROVAL_GET(dcrSearch.dcrType,//dcr type
                null,//user no
                decimal.Parse(Session["sess_USER_NO"].ToString()),
                null,//division
                null,//zilla
                null,//thana
                DateTime.Now, //date to
                DateTime.Now,//date from
                0, null).ToList();

            ViewBag.dcr_List = dcr_List;

            decimal? dcr_count = db.TRN_DCR_APPROVAL_COUNT(decimal.Parse(Session["sess_USER_NO"].ToString())).FirstOrDefault();
            ViewBag.dcr_count = dcr_count;

            return View();
        }

        [HttpPost]
        public ActionResult Index(DcrVerifyApproveSearch search_obj) {
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();

            IList<TRN_EXPENSE_APPROVAL_GET_Result> tmp = db.TRN_EXPENSE_APPROVAL_GET(
                search_obj.USER_NO,//user
                decimal.Parse(Session["sess_USER_NO"].ToString()),//parent
                search_obj.ddldivision,//division
                search_obj.ddlZillas,//zilla
                search_obj.ddlThanas,//thana
                search_obj.DateFrom,//date from
                search_obj.DateTo,//date to
                search_obj.verify_status,1, search_obj.fare_cost).ToList();
            ViewBag.expenses = tmp;




            List<TRN_DCR_APPROVAL_GET_Result> dcr_List = db.TRN_DCR_APPROVAL_GET(search_obj.dcrType,//dcr type
                search_obj.USER_NO,//user no
                decimal.Parse(Session["sess_USER_NO"].ToString()),
                search_obj.ddldivision,//division
                search_obj.ddlZillas,//zilla
                search_obj.ddlThanas,//thana
                search_obj.DateFrom,//date from
                search_obj.DateTo,//date to
                search_obj.verify_status, search_obj.fare_cost).ToList();

            ViewBag.dcr_List = dcr_List;

            return View("_DcrTable");
        }

        /**
         * Web service call
         * */
        public JsonResult GetDDLData(decimal? id, string type)
        {
            string ret_zill = "<option value='0'>Please Select</option>", ret_thana = "<option value='0'>Please Select</option>", ret_zm = "<option value='0'>Please Select</option>";
            if (!string.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                if (type == "division")
                {
                    IList<SET_ZILLA> zillas = db.SET_ZILLA.Where(x => x.DIVISION_NO == id).ToList();
                    foreach (SET_ZILLA zill in zillas)
                        ret_zill += "<option value=\"" + zill.ZILLA_NO + "\">" + zill.ZILLA_NAME + "</option>";
                    IList<SET_THANA> thanas = db.SET_THANA.Where(x => x.SET_ZILLA.SET_DIVISION.DIVISION_NO == id).ToList();
                    foreach (SET_THANA thn in thanas)
                        ret_thana += "<option value=\"" + thn.THANA_NO + "\">" + thn.THANA_NAME + "</option>";
                    IList<SEC_USERS_GET_RPT_ZONAL_Result> zms = db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                        id, null, null).ToList();
                    foreach (SEC_USERS_GET_RPT_ZONAL_Result thn in zms)
                        ret_zm += "<option value=\"" + thn.USER_NO + "\">" + thn.USER_NAME + "</option>";
                }
                else if (type == "zilla")
                {
                    IList<SET_THANA> thanas = db.SET_THANA.Where(x => x.SET_ZILLA.ZILLA_NO == id).ToList();
                    foreach (SET_THANA thn in thanas)
                        ret_thana += "<option value=\"" + thn.THANA_NO + "\">" + thn.THANA_NAME + "</option>";
                    IList<SEC_USERS_GET_RPT_ZONAL_Result> zms = db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                        null, id, null).ToList();
                    foreach (SEC_USERS_GET_RPT_ZONAL_Result thn in zms)
                        ret_zm += "<option value=\"" + thn.USER_NO + "\">" + thn.USER_NAME + "</option>";
                }
                else if (type == "thana")
                {
                    IList<SEC_USERS_GET_RPT_ZONAL_Result> zms = db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                        null, null, id).ToList();
                    foreach (SEC_USERS_GET_RPT_ZONAL_Result thn in zms)
                        ret_zm += "<option value=\"" + thn.USER_NO + "\">" + thn.USER_NAME + "</option>";
                }

            }
            else
            {
                IList<SEC_USERS_GET_RPT_ZONAL_Result> zms = db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                            null, null, null).ToList();
                foreach (SEC_USERS_GET_RPT_ZONAL_Result thn in zms)
                    ret_zm += "<option value=\"" + thn.USER_NO + "\">" + thn.USER_NAME + "</option>";
            }
            return Json(new { zill = ret_zill, thana = ret_thana, zms = ret_zm }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AmmendDCR( decimal fare, decimal? dcrNo, decimal? type)
        {
            //string ret="";
            if (dcrNo != null)
                db.TRN_DCR_DO_AMEND(dcrNo, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), null, null,  fare);
            return Json(new { output = (dcrNo != null) ? ((type == 1) ? "Accepted" : "Rejected") : "Rejected" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AmmendExpense(decimal? EXP_DET_NO, decimal? APPROVE_TYPE_NO, decimal? APPROVE_EXP_TYPE_NO, decimal? APPROVE_EXP_AMT)
        {
            //string ret="";
            if (EXP_DET_NO != null)
                db.TRN_EXPENSE_DET_DO_AMEND(EXP_DET_NO,decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), null, null,  APPROVE_EXP_AMT);
            return Json(new { output = (EXP_DET_NO != null) ? ((APPROVE_TYPE_NO == 1) ? "Accepted" : "Rejected") : "Rejected" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AcceptRejectDCR(decimal transtype, decimal fare, decimal? dcrNo, decimal? type) {
            //string ret="";
            if (dcrNo != null)
                db.TRN_DCR_DO_APPROVE(dcrNo, type, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), null, null, transtype, fare);
            return Json(new { output = (dcrNo != null) ? ((type == 1) ? "Accepted" : "Rejected") : "Rejected" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AcceptRejectExpense(decimal? EXP_DET_NO, decimal? APPROVE_TYPE_NO, decimal? APPROVE_EXP_TYPE_NO, decimal? APPROVE_EXP_AMT) {
            //string ret="";
            if (EXP_DET_NO != null)
                db.TRN_EXPENSE_DET_DO_APPROVE(EXP_DET_NO, APPROVE_TYPE_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), null, null, APPROVE_EXP_TYPE_NO, APPROVE_EXP_AMT);
            return Json(new { output = (EXP_DET_NO != null) ? ((APPROVE_TYPE_NO == 1) ? "Accepted" : "Rejected") : "Rejected" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDCRDetailsEditable(decimal dcrNo)
        {
            TRN_DCR dcr = db.TRN_DCR.Where(x => x.DCR_NO == dcrNo).First();
            string tmp = "";
            if (dcr.IS_REF_ZM.HasValue)
            {
                if (dcr.IS_REF_ZM == 1)
                {
                    tmp += "<b>Ref</b></td><td>" + dcr.REF_ZM_MOBILE;
                }
            }
            string ret = @"<table class='table table-responsive' style='width:95%;margin-left:auto;'><tbody>
                        <tr><td><b>Work Type</b></td><td>" + dcr.GEN_DCR_TYPE.DCR_TYPE + "</td><td></td></tr>" +
                        "<tr><td><b>ZM</b></td><td>" + dcr.SEC_USERS.USER_FULL_NAME + "(" + dcr.SEC_USERS.USER_MOBILE + ")" + "</td><td>" + tmp + "</td><td></td></tr>" +
                        "<tr><td><b>Date</b></td><td>" + dcr.TRN_DCR_DATE.ToString("yyyy-MM-dd") + "</td><td></td></tr>" +
                        "<tr><td><b>Start Location</b></td><td>" + dcr.WORK_AREA_FROM_NAME + "</td><td><b>End Location</b></td><td>" + dcr.WORK_AREA_TO_NAME + "</td></tr>" +
                        "<tr><td><b>Start Time</b></td><td>" + DateTime.Parse(dcr.TIME_FROM.ToString()).ToShortTimeString() + "</td><td><b>End Location</b></td><td>" + DateTime.Parse(dcr.TIME_TO.ToString()).ToShortTimeString() + "</td></tr>" +
                        "<tr><td><b>Transport</b></b></td><td>" + dcr.SET_TRANSPORT_TYPE.TRANS_TYPE_NAME + "</td><td><b>Cost/Fare</b></td><td>" + dcr.FARE_AMT.ToString() + "</td></tr>" +
                        "<tr><td><b>Institute</b></td><td>" + ((dcr.SET_INSTITUTE != null) ? dcr.SET_INSTITUTE.INSTITUTE_NAME : "") +
                            "</td><td></td></tr></tbody></table><br/>";

            ret += "<table class=\"table table-responsive\" style=\"width:95%;margin-left:auto;\">";
            IList<TRN_DCR_DET> dcr_dets = db.TRN_DCR_DET.Where(x => x.DCR_NO == dcrNo).ToList();
            if (dcr_dets.Count != 0)
            {
                switch (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE)
                {
                    case "TC":

                        ret += "<tr><th>Contact No</th><th>Type</th></tr>";

                        foreach (TRN_DCR_DET dts in dcr_dets)
                            ret += "<tr><td>" + dts.TEACHER_MOBILE + "</td><td>" + "Teacher" + "</td></tr>";

                        break;
                    case "LC":
                        ret += "<tr><th>Contact No</th><th>Type</th></tr>";
                        foreach (TRN_DCR_DET dts in dcr_dets)
                        {
                            bool is_teacher = true;
                            if (dts.IS_FOR_CLIENT != null)
                                if (dts.IS_FOR_CLIENT == 1)
                                    is_teacher = false;
                            ret += "<tr><td>" + ((!is_teacher) ? dts.CLIENT_MOBILE : dts.TEACHER_MOBILE) + "</td><td>" + ((!is_teacher) ? "Client" : "Teacher") + "</td></tr>";
                        }
                        break;
                    case "CSR":
                    case "SD":
                        SEC_USERS_LOGIN_Result1 usertp = (SEC_USERS_LOGIN_Result1)Session["sess_sec_users"];
                        decimal tmpdet = decimal.Parse(usertp.USER_TYPE_NO.ToString());
                        GEN_USER_TYPES gentype = db.GEN_USER_TYPES.Single(x => x.USER_TYPE_NO == tmpdet);
                        ret += "<tr><th>Item</th><th>Qty</th><th>Contact</th><th>On Behalf?</th><th>On Behalf Mobile No</th><th></th></tr>";
                        dcr_dets = dcr_dets.OrderByDescending(x => x.SET_SPECIMEN.SPECIMEN_NAME).Reverse().ToList();
                        
                            
                        foreach (TRN_DCR_DET dts in dcr_dets)
                        {
                            bool is_behalf = true;
                            if (dts.IS_ON_BEHALF != null)
                                if (dts.IS_ON_BEHALF != 1)
                                    is_behalf = false;
                            
                            ret += "<tr><td>" + dts.SET_SPECIMEN.SPECIMEN_NAME + "</td><td>"+
                            ((gentype.IS_ADMIN == 1) ? "<input id='txtsdup" + dts.DCR_DET_NO + "' type='text' class='form-control input-small' value='" + dts.APPROVE_SPECIMEN_QTY + "'></input>" : dts.APPROVE_SPECIMEN_QTY.ToString()) +
                            "</td><td>" + dts.TEACHER_MOBILE + "</td><td>" + ((is_behalf) ? "Yes" : "No") + "</td><td>" +
                            ((is_behalf) ? dts.BEHALF_MOBILE : "-") + "</td><td>"+
                            ((gentype.IS_ADMIN == 1) ? "<button class='btn btn-warning btn-small btnupdate' attr='" + dts.DCR_DET_NO + "' onclick='changeSDonApprove($(this));'>Edit</button>" : "") + "</td></tr>";
                        }
                        break;

                    case "MP":
                        ret += "<tr><th>Item</th><th>Whom</th><th>Qty</th><th>Contact</th><th>On Behalf?</th><th>On Behalf Mobile No</th></tr>";
                        dcr_dets = dcr_dets.OrderByDescending(x => x.SET_PROMO_ITEM.PROMO_ITEM_NAME).Reverse().ToList();
                        foreach (TRN_DCR_DET dts in dcr_dets)
                        {
                            bool is_behalf = true;
                            if (dts.IS_ON_BEHALF != null)
                                if (dts.IS_ON_BEHALF != 1)
                                    is_behalf = false;
                            ret += "<tr><td>" + dts.SET_PROMO_ITEM.PROMO_ITEM_NAME + "</td><td>" + ((dts.IS_FOR_TEACHER == 1) ? "Teacher" : "Client") + "</td><td>" +
                                        dts.PROMO_ITEM_QTY + "</td><td>" + ((dts.IS_FOR_TEACHER == 1) ? dts.TEACHER_MOBILE : dts.CLIENT_MOBILE) +
                                        "</td><td>" + ((is_behalf) ? "Yes" : "No") + "</td><td>" +
                                        ((is_behalf) ? dts.BEHALF_MOBILE : "") + "</td></tr>";
                        }
                        break;
                    default:
                        ret += "";
                        break;
                }
            }
            ret += "</table>";

            return Json(new { html = ret }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult setUpdatedSDQuantity(decimal dcrDetNo, int value) {
            try
            {
                db.TRN_DCR_DET_SD_QUANTITY_UPDATE(dcrDetNo, 
                    decimal.Parse(Session["sess_USER_NO"].ToString()),
                    decimal.Parse(Session["sess_LOGON_NO"].ToString()), 
                    value);
                return Json(new { html = "1" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                return Json(new { html = "0" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getDCRDetails(decimal dcrNo)
        {
            TRN_DCR dcr = db.TRN_DCR.Where(x=>x.DCR_NO==dcrNo).First();
            string tmp="";
            if(dcr.IS_REF_ZM.HasValue){
                if(dcr.IS_REF_ZM==1){
                    tmp+="<b>Ref</b></td><td>"+dcr.REF_ZM_MOBILE;
                }
            }
            string ret = @"<table class='table table-responsive' style='width:95%;margin-left:auto;'><tbody>
                        <tr><td><b>Work Type</b></td><td>" +dcr.GEN_DCR_TYPE.DCR_TYPE+"</td><td></td></tr>"+
                        "<tr><td><b>ZM</b></td><td>" + dcr.SEC_USERS.USER_FULL_NAME + "(" + dcr.SEC_USERS.USER_MOBILE + ")" + "</td><td>" + tmp + "</td><td></td></tr>" +
                        "<tr><td><b>Date</b></td><td>" + dcr.TRN_DCR_DATE.ToString("yyyy-MM-dd") + "</td><td></td></tr>" +
                        "<tr><td><b>Start Location</b></td><td>" + dcr.WORK_AREA_FROM_NAME + "</td><td><b>End Location</b></td><td>" + dcr.WORK_AREA_TO_NAME + "</td></tr>" +
                        "<tr><td><b>Start Time</b></td><td>" + DateTime.Parse(dcr.TIME_FROM.ToString()).ToShortTimeString() + "</td><td><b>End Location</b></td><td>" + DateTime.Parse(dcr.TIME_TO.ToString()).ToShortTimeString() + "</td></tr>" +
                        "<tr><td><b>Transport</b></b></td><td>" + dcr.SET_TRANSPORT_TYPE.TRANS_TYPE_NAME + "</td><td><b>Cost/Fare</b></td><td>" + dcr.FARE_AMT.ToString() + "</td></tr>" +
                        "<tr><td><b>Institute</b></td><td>" + ((dcr.SET_INSTITUTE != null) ? dcr.SET_INSTITUTE.INSTITUTE_NAME : "") +
                            "</td><td></td></tr></tbody></table><br/>";
            
            ret += "<table class=\"table table-responsive\" style=\"width:95%;margin-left:auto;\">";
            IList<TRN_DCR_DET> dcr_dets = db.TRN_DCR_DET.Where(x => x.DCR_NO == dcrNo).ToList();
            if (dcr_dets.Count != 0)
            {
                switch (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE)
                {
                    case "TC":

                        ret += "<tr><th>Contact No</th><th>Type</th></tr>";

                        foreach (TRN_DCR_DET dts in dcr_dets)
                            ret += "<tr><td>" + dts.TEACHER_MOBILE + "</td><td>" + "Teacher" + "</td></tr>";

                        break;
                    case "LC":
                        ret += "<tr><th>Contact No</th><th>Type</th></tr>";
                        foreach (TRN_DCR_DET dts in dcr_dets)
                        {
                            bool is_teacher = true;
                            if (dts.IS_FOR_CLIENT != null)
                                if (dts.IS_FOR_CLIENT == 1)
                                    is_teacher = false;
                            ret += "<tr><td>" + ((!is_teacher) ? dts.CLIENT_MOBILE : dts.TEACHER_MOBILE) + "</td><td>" + ((!is_teacher) ? "Client" : "Teacher") + "</td></tr>";
                        }
                        break;
                    case "SD":
                    case "CSR":
                        ret += "<tr><th>Item</th><th>Qty</th><th>Contact</th><th>On Behalf?</th><th>On Behalf Mobile No</th></tr>";
                        dcr_dets = dcr_dets.OrderByDescending(x => x.SET_SPECIMEN.SPECIMEN_NAME).Reverse().ToList();
                        foreach (TRN_DCR_DET dts in dcr_dets)
                        {
                            bool is_behalf = true;
                            if (dts.IS_ON_BEHALF != null)
                                if (dts.IS_ON_BEHALF != 1)
                                    is_behalf = false;
                            ret += "<tr><td>" + dts.SET_SPECIMEN.SPECIMEN_NAME + "</td><td>" + dts.SPECIMEN_QTY + "</td>" +
                                        "<td>" + dts.TEACHER_MOBILE + "</td><td>" + ((is_behalf) ? "Yes" : "No") + "</td><td>" +
                                        ((is_behalf) ? dts.BEHALF_MOBILE : "-") + "</td></tr>";
                        }
                        break;
                    case "MP":
                        ret += "<tr><th>Item</th><th>Whom</th><th>Qty</th><th>Contact</th><th>On Behalf?</th><th>On Behalf Mobile No</th></tr>";
                        dcr_dets = dcr_dets.OrderByDescending(x => x.SET_PROMO_ITEM.PROMO_ITEM_NAME).Reverse().ToList();
                        foreach (TRN_DCR_DET dts in dcr_dets)
                        {
                            bool is_behalf = true;
                            if (dts.IS_ON_BEHALF != null)
                                if (dts.IS_ON_BEHALF != 1)
                                    is_behalf = false;
                            ret += "<tr><td>" + dts.SET_PROMO_ITEM.PROMO_ITEM_NAME + "</td><td>" + ((dts.IS_FOR_TEACHER == 1) ? "Teacher" : "Client") + "</td><td>" +
                                        dts.PROMO_ITEM_QTY + "</td><td>" + ((dts.IS_FOR_TEACHER == 1) ? dts.TEACHER_MOBILE : dts.CLIENT_MOBILE) +
                                        "</td><td>" + ((is_behalf) ? "Yes" : "No") + "</td><td>" +
                                        ((is_behalf) ? dts.BEHALF_MOBILE : "") + "</td></tr>";
                        }
                        break;
                    default:
                        ret += "";
                        break;
                }
            }
            ret += "</table>";

            return Json(new { html =ret},JsonRequestBehavior.AllowGet); ;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OG_SLN.Models;

namespace OG_SLN.Controllers
{
    public class OfflineEntryMonitorController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /OfflineEntryMonitor/
        //public Tuple<SearchOfflineEntry, IQueryable<TRN_DCR>> setSearch(IQueryable<TRN_DCR> dcrs, SearchOfflineEntry dcrSearch, bool search)
        //{
        //    dcrSearch = (dcrSearch.dirty) ? dcrSearch : new SearchOfflineEntry();

        //    if (!string.IsNullOrEmpty(Request.QueryString["USER_NO"]) || search)
        //    {
        //        dcrSearch.USER_NO = (!search) ? decimal.Parse(Request.QueryString["USER_NO"].ToString()) : dcrSearch.USER_NO;
        //        if (!string.IsNullOrEmpty(dcrSearch.USER_NO.ToString()))
        //            dcrs = dcrs.Where(s => s.USER_NO == dcrSearch.USER_NO);
        //    }
        //    else
        //        dcrSearch.USER_NO = null;

        //    if (!string.IsNullOrEmpty(Request.QueryString["DateFrom"]) || search)
        //    {
        //        dcrSearch.dateOffFrom = (!search) ? DateTime.Parse(Request.QueryString["DateFrom"]) : dcrSearch.dateOffFrom;
        //        if (dcrSearch.dateOffFrom != null)
        //            dcrs = dcrs.Where(s => s.TRN_DCR_DATE > dcrSearch.dateOffFrom);
        //    }
        //    else
        //        dcrSearch.dateOffFrom = null;
        //    if (!string.IsNullOrEmpty(Request.QueryString["DateTo"]) || search)
        //    {
        //        dcrSearch.dateOffTo = (!search) ? DateTime.Parse(Request.QueryString["DateTo"]) : dcrSearch.dateOffTo;
        //        if (dcrSearch.dateOffTo != null)
        //            dcrs = dcrs.Where(s => s.TRN_DCR_DATE < dcrSearch.dateOffTo);
        //    }
        //    else
        //        dcrSearch.dateOffTo = null;

        //    if (!string.IsNullOrEmpty(Request.QueryString["dcrType"]) || search)
        //    {
        //        dcrSearch.dcrType = (!search) ? decimal.Parse( Request.QueryString["dcrType"]) : dcrSearch.dcrType;
        //        if (!string.IsNullOrEmpty(dcrSearch.dcrType.ToString()))
        //            dcrs = dcrs.Where(s => s.DCR_TYPE_NO== dcrSearch.dcrType);
        //    }
        //    else
        //        dcrSearch.dcrType = null;
        //    if (!string.IsNullOrEmpty(Request.QueryString["verify"]) || search)
        //    {
        //        dcrSearch.verify = (!search) ? decimal.Parse(Request.QueryString["verify"]) : dcrSearch.verify;
        //        if (!string.IsNullOrEmpty(dcrSearch.verify.ToString()))
        //            dcrs = dcrs.Where(s => s.APPROVE_TYPE_NO == dcrSearch.verify);
        //    }
        //    else
        //        dcrSearch.verify = null;
        //    dcrSearch.dirty = true;
        //    return new Tuple<SearchOfflineEntry, IQueryable<TRN_DCR>>(dcrSearch, dcrs);
        //}
        public ActionResult Index()
        {
            /**
             * viewbag config for searching only
             * */
            ViewBag.ZonalUsers = db.SEC_USERS.Where(u => u.GEN_USER_TYPES.SHORT_NAME == "ZM");
            ViewBag.fromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.toDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            SearchOfflineEntry dcrSearch = (Session["SearchOfflineEntry"] == null) ? new SearchOfflineEntry()
                : (SearchOfflineEntry)Session["SearchOfflineEntry"];
            Session["DCRSearch"] = dcrSearch;
            ViewBag.dcrSearch = dcrSearch;
            ViewBag.dcrType = new SelectList(db.GEN_DCR_TYPE, "DCR_TYPE_NO", "DCR_TYPE");
            ViewBag.verify_status = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE");
            ViewBag.ddldivision = new SelectList(db.SET_DIVISION, "DIVISION_NO", "DIVISION_NAME");
            ViewBag.zmusers =new SelectList(db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),null,null,null).ToList(),
            "USER_NO", "USER_FULL_NAME");
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();
            /**
             * proc call to get all unverified data
             * */
            List<TRN_DCR_OFF_VERIFY_GET_Result> dcr_List = db.TRN_DCR_OFF_VERIFY_GET(dcrSearch.dcrType,//dcr type
                null,//user no
                decimal.Parse(Session["sess_USER_NO"].ToString()),
                null,//division
                null,//zilla
                null,//thana
                DateTime.Now, //date to
                DateTime.Now,//date from
                0).ToList();
            ViewBag.dcr_List = dcr_List;

            decimal? dcr_count = db.TRN_DCR_OFF_VERIFY_COUNT(decimal.Parse(Session["sess_USER_NO"].ToString())).FirstOrDefault();
            ViewBag.dcr_count = dcr_count;

            return View();

        }

        [HttpPost]
        public ActionResult Index(DcrVerifyApproveSearch search_obj) {
            ViewBag.trnasType = db.SET_TRANSPORT_TYPE.ToList();

            List<TRN_DCR_OFF_VERIFY_GET_Result> dcr_List = db.TRN_DCR_OFF_VERIFY_GET(
                search_obj.dcrType,//dcr type
                search_obj.USER_NO,//user no
                decimal.Parse(Session["sess_USER_NO"].ToString()),
                search_obj.ddldivision,//division
                search_obj.ddlZillas,//zilla
                search_obj.ddlThanas,//thana
                search_obj.DateFrom,//date from
                search_obj.DateTo,//date to
                search_obj.verify_status).ToList();

            ViewBag.dcr_List = dcr_List;

            return View("_DcrOffTable");
        }

        
        /**
         * Web service call
         * */
        public JsonResult AcceptRejectDCR(decimal transtype, decimal fare, decimal? dcrNo, decimal? type)
        {
            //string ret="";
            if (dcrNo != null)
                db.TRN_DCR_VERIFY(dcrNo, type, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()), transtype, fare);
            return Json(new { output = (dcrNo != null) ? ((type == 1) ? "Accepted" : "Rejected") : "Rejected" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDDLData(decimal? id,string type)
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
            else {
                IList<SEC_USERS_GET_RPT_ZONAL_Result> zms = db.SEC_USERS_GET_RPT_ZONAL(decimal.Parse(Session["sess_USER_NO"].ToString()),
                            null, null, null).ToList();
                foreach (SEC_USERS_GET_RPT_ZONAL_Result thn in zms)
                    ret_zm += "<option value=\"" + thn.USER_NO + "\">" + thn.USER_NAME + "</option>";
            }
            return Json(new { zill = ret_zill, thana = ret_thana, zms = ret_zm }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetThanas(decimal ZillaID)
        {
            string ret_zill = "", ret_thana = "<option value='0'>Please Select</option>", ret_zm = "<option value='0'>Please Select</option>";
            if (ZillaID != 0)
            {

                IList<SET_THANA> thanas = db.SET_THANA.Where(x => x.SET_ZILLA.ZILLA_NO == ZillaID).ToList();
                foreach (SET_THANA thn in thanas)
                    ret_thana += "<option value=\"" + thn.THANA_NO + "\">" + thn.THANA_NAME + "</option>";
                //IList<SEC_USERS> zms = db.SET_THANA.Where(x => x.SET_ZILLA.SET_DIVISION.DIVISION_NO == divisionID).ToList();
                //foreach (SET_THANA thn in thanas)
                //    ret_zill += "<option value=\"" + thn.THANA_NO + "\">" + thn.THANA_NAME + "</option>";

            }
            return Json(new { zill = ret_zill, thana = ret_thana, zms = ret_zm }, JsonRequestBehavior.AllowGet);
        }
    }
}
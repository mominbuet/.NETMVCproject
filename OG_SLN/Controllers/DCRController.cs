using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using PagedList;
using System.Data.Objects.SqlClient;
using OG_SLN.Models;

namespace OG_SLN.Controllers
{
    public class DCRController : Controller
    {
        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /DCR/
        public Tuple<DCRSearch, IQueryable<TRN_DCR>> setSearch(IQueryable<TRN_DCR> dcrs, DCRSearch dcrSearch,bool search)
        {
            dcrSearch = (dcrSearch.dirty) ? dcrSearch :new DCRSearch();
            if (!string.IsNullOrEmpty(Request.QueryString["ZMName"]) || search)
            {
                dcrSearch.ZMName =(!search)? Request.QueryString["ZMName"]:dcrSearch.ZMName;
                if (!string.IsNullOrEmpty(dcrSearch.ZMName.ToString()))
                 dcrs = dcrs.Where(s => s.SEC_USERS.USER_NAME.Contains(dcrSearch.ZMName));
            }
            else
                dcrSearch.ZMName = "";
            if (!string.IsNullOrEmpty(Request.QueryString["USER_NO"]) || search)
            {
                dcrSearch.userNo = (!search) ? decimal.Parse(Request.QueryString["USER_NO"].ToString()) : dcrSearch.userNo;
                if (!string.IsNullOrEmpty(dcrSearch.userNo.ToString()))
                    dcrs = dcrs.Where(s => s.USER_NO == dcrSearch.userNo);
            }
            else
                dcrSearch.userNo = null;
            if (!string.IsNullOrEmpty(Request.QueryString["ZMNo"]) || search)
            {
                dcrSearch.ZMNo = (!search) ? decimal.Parse( Request.QueryString["ZMNo"]) : dcrSearch.ZMNo;
                if (dcrSearch.ZMNo != 0)
                    dcrs = dcrs.Where(s => s.USER_NO == dcrSearch.ZMNo);
            }
            else
                dcrSearch.ZMNo = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["DateFrom"]) || search)
            {
                dcrSearch.DateFrom = (!search) ? DateTime.Parse(Request.QueryString["DateFrom"]) : dcrSearch.DateFrom;
                if (dcrSearch.DateFrom != null) 
                    dcrs = dcrs.Where(s => s.TRN_DCR_DATE > dcrSearch.DateFrom);
            }
            else
                dcrSearch.DateFrom = null;
            if (!string.IsNullOrEmpty(Request.QueryString["DateTo"]) || search)
            {
                dcrSearch.DateTo = (!search) ? DateTime.Parse(Request.QueryString["DateTo"]) : dcrSearch.DateTo;
                if (dcrSearch.DateTo!=null)
                    dcrs = dcrs.Where(s => s.TRN_DCR_DATE < dcrSearch.DateTo);
            }
            else
                dcrSearch.DateTo = null;

            if (!string.IsNullOrEmpty(Request.QueryString["RefZMName"]) || search)
            {
                dcrSearch.RefZMName = (!search) ? Request.QueryString["RefZMName"] : dcrSearch.RefZMName;
                if (!string.IsNullOrEmpty(dcrSearch.RefZMName.ToString())) 
                    dcrs = dcrs.Where(s => s.REF_ZM_MOBILE.Contains(dcrSearch.RefZMName));
            }
            else
                dcrSearch.RefZMName = "";
            if (!string.IsNullOrEmpty(Request.QueryString["institution"]) || search)
            {
                dcrSearch.institution = (!search) ? Request.QueryString["institution"] : dcrSearch.institution;
                if (!string.IsNullOrEmpty(dcrSearch.institution.ToString())) 
                    dcrs = dcrs.Where(s => s.SET_INSTITUTE.INSTITUTE_NAME.Contains(dcrSearch.institution));
            }
            else
                dcrSearch.institution = "";
            dcrSearch.dirty = true;
            return new Tuple<DCRSearch, IQueryable<TRN_DCR>>(dcrSearch, dcrs);
        }
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CurrentSort = sortOrder;
            //sortOrder = String.IsNullOrEmpty(sortOrder) ? "USER_NO" : sortOrder;
            IPagedList<TRN_DCR> pagedSpecimens = null;
            IQueryable<TRN_DCR> dcrs = db.TRN_DCR.AsQueryable();

            var user_list = db.SEC_USERS.Where(a => (a.USER_TYPE_NO == (long)UserType.Zonal_Manager) || (a.USER_TYPE_NO == (long)UserType.Agents)).OrderBy(a => a.USER_NAME).ToList();
            ViewBag.USER_NO = new SelectList(user_list, "USER_NO", "USER_NAME");

            ViewBag.fromDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.toDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
            DCRSearch dcrSearch = (Session["DCRSearch"] == null) ? new DCRSearch()
                : (DCRSearch)Session["DCRSearch"];

            if (Request.QueryString.HasKeys() && !page.HasValue)
            {
                Tuple<DCRSearch, IQueryable<TRN_DCR>> tmp = setSearch(dcrs, dcrSearch,false);
                dcrs = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && dcrSearch.dirty)
            {
                dcrSearch = (DCRSearch)Session["DCRSearch"];
                Tuple<DCRSearch, IQueryable<TRN_DCR>> tmp = setSearch(dcrs, dcrSearch,true);
                dcrs = tmp.Item2;
                dcrSearch = tmp.Item1;
            }
            else if (page.HasValue && Session["DCRSearch"] == null)
            {
                dcrSearch = new DCRSearch();

            }
            /*else
                dcrSearch.isactive = "all";*/
            Session["DCRSearch"] = dcrSearch;
            pagedSpecimens = dcrs.OrderByDescending(m => m.INSERT_TIME).ToPagedList(pageIndex, pageSize);
            ViewBag.dcrSearch = dcrSearch;
            return View(pagedSpecimens);
        }

        //
        // GET: /DCR/Details/5

        public ActionResult Details(decimal id = 0)
        {
            TRN_DCR trn_dcr = db.TRN_DCR.Single(t => t.DCR_NO == id);
            if (trn_dcr == null)
            {
                return HttpNotFound();
            }
            ViewBag.jsmsg = "<script type='text/javascript'>$(document).ready(function () {setEdit(" + id + ");}); </script>";
            return View(trn_dcr);
        }

        //
        // GET: /DCR/Create

        /*public ActionResult Create()
        {
            ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "INSTITUTE_NAME");
            ViewBag.DCR_TYPE_NO = new SelectList(db.GEN_DCR_TYPE, "DCR_TYPE_NO", "DCR_TYPE");
            ViewBag.TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "TRANS_TYPE_NAME");
            return View();
        }*/
        public ActionResult Create(decimal? id)
        {
            TRN_DCR trnDcr =(id != null)? db.TRN_DCR.Where(s => s.DCR_NO == id).First():new TRN_DCR();

            /*ViewBag.jsmsg = (id != null) ? "<script type='text/javascript' language='javascript'>setEdit(" + id + "," + trnDcr.GEN_DCR_TYPE.DCR_TYPE_NO + "," +
                    trnDcr.SEC_USERS.USER_NAME + "," + trnDcr.USER_NO + ","
                    + trnDcr.IS_REF_ZM + "," + ((trnDcr.REF_ZM_MOBILE == null) ? "0" : trnDcr.REF_ZM_MOBILE) + "," + trnDcr.TRN_DCR_DATE.ToString("yyyy-MM-dd") +
                    "," + trnDcr.WORK_AREA_FROM_NAME + "," + trnDcr.WORK_AREA_TO_NAME + "," + Convert.ToDateTime(trnDcr.TIME_FROM).ToString("HH-mm") +
                    "," + Convert.ToDateTime(trnDcr.TIME_TO).ToString("HH-mm") + "," + trnDcr.TRANS_TYPE_NO + "," + trnDcr.FARE_AMT + "," + trnDcr.SET_INSTITUTE.INSTITUTE_NAME +
                    "," + trnDcr.INSTITUTE_NO + "); </script>" : "";*/
            List<string> lstTmp = new List<string>(); 
            foreach (string crntSession in Session)
                if (crntSession.Contains("DCREntry"))
                    lstTmp.Add(crntSession);
            foreach (string tmp in lstTmp)
                Session.Remove(tmp);
            ViewBag.jsmsg = (id != null) ? "<script type='text/javascript' language='javascript'>setEdit(" + id+ "); </script>"
                : "";
            ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "INSTITUTE_NAME");
            ViewBag.DCR_TYPE_NO = new SelectList(db.GEN_DCR_TYPE, "DCR_TYPE_NO", "DCR_TYPE");
            ViewBag.TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "TRANS_TYPE_NAME");
            return View();
        }
        //
        // POST: /DCR/Create

        [HttpPost]
        public ActionResult Create(TRN_DCR trn_dcr)
        {
            if (ModelState.IsValid)
            {
                trn_dcr.TIME_FROM = DateTime.ParseExact(trn_dcr.TRN_DCR_DATE.Year + "-" + trn_dcr.TRN_DCR_DATE.Month + "-" + trn_dcr.TRN_DCR_DATE.Day + " " + trn_dcr.FROM_TIME + ":00,000",
                    "yyyy-M-dd H:m:ss,fff",System.Globalization.CultureInfo.InvariantCulture);

                trn_dcr.TIME_TO = DateTime.ParseExact(trn_dcr.TRN_DCR_DATE.Year + "-" + trn_dcr.TRN_DCR_DATE.Month + "-" + trn_dcr.TRN_DCR_DATE.Day + " " + trn_dcr.TO_TIME + ":00,000",
                    "yyyy-M-dd H:m:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
                TimeSpan ts= DateTime.Parse( trn_dcr.TIME_TO.ToString()).Subtract(DateTime.Parse(trn_dcr.TIME_FROM.ToString()));
                if (trn_dcr.edit_ID == 0){
                   decimal trn_dcr_no = db.TRN_DCR_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                        trn_dcr.INSERT_OFFLINE_TIME,
                        null,null,trn_dcr.DCR_TYPE_NO,trn_dcr.user_ID,
                        null, trn_dcr.IS_REF_ZM, trn_dcr.user_ID, trn_dcr.REF_ZM_MOBILE, null,
                        trn_dcr.WORK_AREA_FROM_LAT,trn_dcr.WORK_AREA_FROM_LON,trn_dcr.WORK_AREA_FROM_NAME,
                        trn_dcr.WORK_AREA_TO_LAT,trn_dcr.WORK_AREA_TO_LON,trn_dcr.WORK_AREA_TO_NAME,
                        trn_dcr.TIME_FROM,trn_dcr.TIME_TO,Convert.ToDecimal(ts.Hours),trn_dcr.DIVISION_NO,
                        trn_dcr.ZONE_NO,trn_dcr.ZILLA_NO,trn_dcr.THANA_NO,trn_dcr.INSTITUTE_NO,
                        trn_dcr.TRANS_TYPE_NO,trn_dcr.FARE_AMT,trn_dcr.TRN_DCR_DATE,1,
                        null,trn_dcr.COMMENTS).FirstOrDefault().Value;
                        trn_dcr.DCR_NO = trn_dcr_no;
                }
                else
                {
                    db.TRN_DCR_UPDATE(trn_dcr.edit_ID,
                        decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()),trn_dcr.UPDATE_OFFLINE_TIME,
                        trn_dcr.UPDATE_OFFLINE_SYNC, 1, trn_dcr.DCR_TYPE_NO, trn_dcr.user_ID,
                        null,trn_dcr.IS_REF_ZM,null,trn_dcr.REF_ZM_MOBILE,
                        null,trn_dcr.WORK_AREA_FROM_LAT,trn_dcr.WORK_AREA_FROM_LON,trn_dcr.WORK_AREA_FROM_NAME,
                        trn_dcr.WORK_AREA_TO_LAT,trn_dcr.WORK_AREA_TO_LON,trn_dcr.WORK_AREA_TO_NAME,
                        trn_dcr.TIME_FROM,trn_dcr.TIME_TO,Convert.ToDecimal(ts.Hours),trn_dcr.DIVISION_NO,
                        trn_dcr.ZONE_NO,trn_dcr.ZILLA_NO,trn_dcr.THANA_NO,trn_dcr.INSTITUTE_NO,
                        trn_dcr.TRANS_TYPE_NO,trn_dcr.FARE_AMT,trn_dcr.APPROVE_TRANS_TYPE_NO,
                        trn_dcr.APPROVE_FARE_AMT,trn_dcr.TRN_DCR_DATE,1,null,trn_dcr.COMMENTS);
                }
                List<string> lstTmp = new List<string>();
                foreach (string curSession in Session) {
                    if (curSession.Contains("DCREntry")) {
                        lstTmp.Add(curSession);
                        temp_TRN_DCR_DET temp_sess_det = (temp_TRN_DCR_DET)Session[curSession];
                        TRN_DCR_DET det = (temp_sess_det.editID != 0) ? db.TRN_DCR_DET.Where(s=>s.DCR_DET_NO==temp_sess_det.editID).FirstOrDefault()
                            :db.TRN_DCR_DET.Where(s => s.DCR_NO == ((trn_dcr.DCR_NO == 0) ? trn_dcr.edit_ID : trn_dcr.DCR_NO) &&
                                (s.CLIENT_MOBILE == temp_sess_det.client_mobile || s.TEACHER_MOBILE == temp_sess_det.teacher_mobile)).FirstOrDefault();
                        if (!temp_sess_det.set_deleted)
                        {
                            if (det == null)
                                db.TRN_DCR_DET_INSERT(decimal.Parse(Session["sess_USER_NO"].ToString()),
                                decimal.Parse(Session["sess_LOGON_NO"].ToString()),
                                    DateTime.Now, trn_dcr.INSERT_OFFLINE_SYNC,
                                    1, ((trn_dcr.DCR_NO == 0) ? trn_dcr.edit_ID : trn_dcr.DCR_NO), (temp_sess_det.RcvType.ToLower() == "teacher") ? 1 : 0, temp_sess_det.teacher_no,
                                    temp_sess_det.teacher_mobile, temp_sess_det.is_behalf, temp_sess_det.BEHALF_MOBILE,
                                    (temp_sess_det.type == "specimen") ? temp_sess_det.type_id : null, (temp_sess_det.type == "specimen") ? temp_sess_det.qty : null,
                                    ((temp_sess_det.RcvType.ToLower() == "teacher") ? 0 : 1), temp_sess_det.client_no, temp_sess_det.client_mobile,
                                    (temp_sess_det.type == "specimen") ? temp_sess_det.type_id : null, (temp_sess_det.type == "specimen") ? temp_sess_det.qty : null,
                                    null, null);
                            else
                                db.TRN_DCR_DET_UPDATE(det.DCR_DET_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                                    decimal.Parse(Session["sess_LOGON_NO"].ToString()), DateTime.Now,
                                    det.UPDATE_OFFLINE_SYNC, det.UPDATE_IS_OFFLINE, det.DCR_NO, ((temp_sess_det.RcvType == "client") ? 0 : 1),
                                     temp_sess_det.teacher_no, temp_sess_det.teacher_mobile, temp_sess_det.is_behalf,
                                    temp_sess_det.BEHALF_MOBILE, (temp_sess_det.type == "specimen") ? temp_sess_det.type_id : null, 
                                    (temp_sess_det.type == "specimen") ? temp_sess_det.qty : null,
                                    ((temp_sess_det.RcvType == "client") ? 1 : 0), det.APPROVE_CLIENT_NO, det.APPROVE_CLIENT_MOBILE,
                                    (temp_sess_det.type == "specimen") ? temp_sess_det.type_id : null, (temp_sess_det.type == "specimen") ? temp_sess_det.qty : null, null, null);
                        }
                        else {
                            if(det!=null)
                                db.TRN_DCR_DET_DELETE(det.DCR_DET_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                                    decimal.Parse(Session["sess_LOGON_NO"].ToString()));
                        }
                    }
                }
                foreach (string del in lstTmp)
                    Session.Remove(del);
                return RedirectToAction("Index");
            }
            ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "INSTITUTE_NAME");
            ViewBag.DCR_TYPE_NO = new SelectList(db.GEN_DCR_TYPE, "DCR_TYPE_NO", "DCR_TYPE");
            ViewBag.TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "TRANS_TYPE_NAME");
            return View(trn_dcr);
        }

        //
        // GET: /DCR/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            TRN_DCR trn_dcr = db.TRN_DCR.Single(t => t.DCR_NO == id);
            if (trn_dcr == null)
            {
                return HttpNotFound();
            }
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", trn_dcr.APPROVE_TYPE_NO);
            ViewBag.DCR_TYPE_NO = new SelectList(db.GEN_DCR_TYPE, "DCR_TYPE_NO", "DCR_TYPE", trn_dcr.DCR_TYPE_NO);
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_dcr.USER_NO);
            ViewBag.AGENT_USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_dcr.AGENT_USER_NO);
            ViewBag.REF_ZM_USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_dcr.REF_ZM_USER_NO);
            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "LAST_ACTION", trn_dcr.DIVISION_NO);
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION", trn_dcr.FY_NO);
            ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "LAST_ACTION", trn_dcr.INSTITUTE_NO);
            ViewBag.THANA_NO = new SelectList(db.SET_THANA, "THANA_NO", "LAST_ACTION", trn_dcr.THANA_NO);
            ViewBag.WORK_PUR_NO = new SelectList(db.SET_WORK_PURPOSE, "WORK_PUR_NO", "LAST_ACTION", trn_dcr.WORK_PUR_NO);
            ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA, "ZILLA_NO", "LAST_ACTION", trn_dcr.ZILLA_NO);
            ViewBag.ZONE_NO = new SelectList(db.SET_ZONE, "ZONE_NO", "LAST_ACTION", trn_dcr.ZONE_NO);
            ViewBag.TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "LAST_ACTION", trn_dcr.TRANS_TYPE_NO);
            ViewBag.APPROVE_TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "LAST_ACTION", trn_dcr.APPROVE_TRANS_TYPE_NO);
            return View(trn_dcr);
        }

        //
        // POST: /DCR/Edit/5

        [HttpPost]
        public ActionResult Edit(TRN_DCR trn_dcr)
        {
            if (ModelState.IsValid)
            {
                db.TRN_DCR.Attach(trn_dcr);
                db.ObjectStateManager.ChangeObjectState(trn_dcr, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.APPROVE_TYPE_NO = new SelectList(db.GEN_APPROVE_TYPE, "APPROVE_TYPE_NO", "APPROVE_TYPE", trn_dcr.APPROVE_TYPE_NO);
            ViewBag.DCR_TYPE_NO = new SelectList(db.GEN_DCR_TYPE, "DCR_TYPE_NO", "DCR_TYPE", trn_dcr.DCR_TYPE_NO);
            ViewBag.USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_dcr.USER_NO);
            ViewBag.AGENT_USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_dcr.AGENT_USER_NO);
            ViewBag.REF_ZM_USER_NO = new SelectList(db.SEC_USERS, "USER_NO", "LAST_ACTION", trn_dcr.REF_ZM_USER_NO);
            ViewBag.DIVISION_NO = new SelectList(db.SET_DIVISION, "DIVISION_NO", "LAST_ACTION", trn_dcr.DIVISION_NO);
            ViewBag.FY_NO = new SelectList(db.SET_FISCAL_YEAR, "FY_NO", "LAST_ACTION", trn_dcr.FY_NO);
            ViewBag.INSTITUTE_NO = new SelectList(db.SET_INSTITUTE, "INSTITUTE_NO", "LAST_ACTION", trn_dcr.INSTITUTE_NO);
            ViewBag.THANA_NO = new SelectList(db.SET_THANA, "THANA_NO", "LAST_ACTION", trn_dcr.THANA_NO);
            ViewBag.WORK_PUR_NO = new SelectList(db.SET_WORK_PURPOSE, "WORK_PUR_NO", "LAST_ACTION", trn_dcr.WORK_PUR_NO);
            ViewBag.ZILLA_NO = new SelectList(db.SET_ZILLA, "ZILLA_NO", "LAST_ACTION", trn_dcr.ZILLA_NO);
            ViewBag.ZONE_NO = new SelectList(db.SET_ZONE, "ZONE_NO", "LAST_ACTION", trn_dcr.ZONE_NO);
            ViewBag.TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "LAST_ACTION", trn_dcr.TRANS_TYPE_NO);
            ViewBag.APPROVE_TRANS_TYPE_NO = new SelectList(db.SET_TRANSPORT_TYPE, "TRANS_TYPE_NO", "LAST_ACTION", trn_dcr.APPROVE_TRANS_TYPE_NO);
            return View(trn_dcr);
        }

        //
        // GET: /DCR/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            TRN_DCR trn_dcr = db.TRN_DCR.Single(t => t.DCR_NO == id);
            if (trn_dcr == null)
            {
                return HttpNotFound();
            }
            ViewBag.jsmsg = (id != null) ? "<script type='text/javascript' language='javascript'>setEdit(" + id + "); </script>"
               : "";
            return View(trn_dcr);
        }

        //
        // POST: /DCR/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(decimal id)
        {
            TRN_DCR trn_dcr = db.TRN_DCR.Single(t => t.DCR_NO == id);
            IList<TRN_DCR_DET> dcr_dets = db.TRN_DCR_DET.Where(x => x.DCR_NO == trn_dcr.DCR_NO).ToList();
            foreach (TRN_DCR_DET dt in dcr_dets)
                db.TRN_DCR_DET_DELETE(dt.DCR_DET_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()));
            db.TRN_DCR_DELETE(trn_dcr.DCR_NO, decimal.Parse(Session["sess_USER_NO"].ToString()),
                        decimal.Parse(Session["sess_LOGON_NO"].ToString()));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        /**
         * WServices starting 
         **/
        public JsonResult getOnePrevData(int? id)
        {
            TRN_DCR trndcr = db.TRN_DCR.Single(t => t.DCR_NO == id);
            /*JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(trn_user_specimen);*/
            return Json(new
            {
                dcrNO = trndcr.DCR_NO,
                typeNo = trndcr.GEN_DCR_TYPE.DCR_TYPE_NO,
                userName = trndcr.SEC_USERS.USER_NAME,
                userNo = trndcr.SEC_USERS.USER_NO,
                ref_zm_mobile = trndcr.REF_ZM_MOBILE,
                is_ref = trndcr.IS_REF_ZM,
                date = trndcr.TRN_DCR_DATE.ToString("yyyy-MM-dd"),
                startLoc = trndcr.WORK_AREA_FROM_NAME,
                endLoc = trndcr.WORK_AREA_TO_NAME,
                startTime = Convert.ToDateTime( trndcr.TIME_FROM).ToString("HH:mm"),
                endTime = Convert.ToDateTime(trndcr.TIME_TO).ToString("HH:mm"),
                transport = trndcr.TRANS_TYPE_NO,
                cost = trndcr.FARE_AMT,
                instituteNo = (trndcr.INSTITUTE_NO!=null)?trndcr.INSTITUTE_NO:0,
                institute =(trndcr.INSTITUTE_NO!=null)? trndcr.SET_INSTITUTE.INSTITUTE_NAME:""
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchInstitution(string term)
        {
            if (term != null)
            {
                decimal id;
                if (!decimal.TryParse(term, out id))
                {
                    return Json((from data in db.SET_INSTITUTE
                                where data.INSTITUTE_NAME.Contains(term)
                                || data.INSTITUTE_NO.ToString().StartsWith(term)
                                select new
                                {
                                    INSTITUTE_NO = data.INSTITUTE_NO,
                                    INSTITUTE_NAME = data.INSTITUTE_NAME,
                                    F_INSTITUTION_DB_ID = data.F_INSTITUTION_DB_ID,
                                    EIIN_NUMBER = data.EIIN_NUMBER
                                }).Distinct().OrderByDescending(m => m.INSTITUTE_NO).Take(200), JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json((from data in db.SET_INSTITUTE
                                 where SqlFunctions.StringConvert(data.INSTITUTE_NO)
                                .TrimStart().StartsWith(term)
                                 select new
                                 {
                                     INSTITUTE_NO = data.INSTITUTE_NO,
                                     INSTITUTE_NAME = data.INSTITUTE_NAME,
                                     F_INSTITUTION_DB_ID = data.F_INSTITUTION_DB_ID,
                                     EIIN_NUMBER = data.EIIN_NUMBER
                                 }).Distinct().OrderByDescending(m => m.INSTITUTE_NO).Take(200), JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchArea(string area)
        {
            if (area != null)
            {
                var spec = (from data in db.TRN_DCR
                           where data.WORK_AREA_TO_NAME.StartsWith(area) ||
                           data.WORK_AREA_FROM_NAME.StartsWith(area)
                           select new
                           {
                               AreaName = data.WORK_AREA_FROM_NAME.StartsWith(area) ?
                                               data.WORK_AREA_FROM_NAME :
                                               data.WORK_AREA_TO_NAME
                           }).Distinct().OrderByDescending(m=>m.AreaName);
                return Json(spec, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchUser(string cell)
        {
            if (cell != null)
            {
                IList<TABULAR_SEARCH_ZONAL_GET_Result> result = db.TABULAR_SEARCH_ZONAL_GET(
                    decimal.Parse(Session["sess_USER_NO"].ToString()),null,null,null,cell).ToList();
                var user = (from data in result
                            /*where data.USER_MOBILE.StartsWith(cell)
                            || data.USER_NAME.StartsWith(cell)*/
                            //&& data..SHORT_NAME == "ZM"
                            select new
                           {
                               id = data.USER_NO,
                               username = data.USER_NAME,
                               fullname= data.USER_FULL_NAME,
                               division = (data.DIVISION_NAME!=null)?data.DIVISION_NAME:" ",
                               zilla = (data.ZILLA_NAME!=null)?data.ZILLA_NAME:" ",
                               thana = (data.THANA_NAME != null) ? data.THANA_NAME : " ",
                               cell = data.USER_MOBILE,
                               designation = (data.DESIG_NAME!=null)?data.DESIG_NAME:" ", 
                               institute = (data.INSTITUTE_NAME != null) ? data.INSTITUTE_NAME : " "
                           }).ToList();
                //
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchContact(string type, string cell)
        {
            if (cell != null)
            {
                if (type == "teacher")
                    return Json((from data in db.SET_TEACHER_INFO
                                where data.TEACHER_MOBILE.Contains(cell)
                                select new
                                {
                                    TEACHER_MOBILE = data.TEACHER_MOBILE,
                                    TEACHER_NAME = data.TEACHER_NAME,
                                    TEACHER_NO = data.TEACHER_NO
                                }), JsonRequestBehavior.AllowGet);
                else
                    return Json(from data in db.SET_CLIENT_INFO
                                where data.CLIENT_MOBILE.Contains(cell)
                                select new
                                {
                                    CLIENT_MOBILE = data.CLIENT_MOBILE,
                                    CLIENT_NAME = data.CLIENT_NAME,
                                    CLIENT_NO = data.CLIENT_NO
                                }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchDistribution(string type, string code)
        {
            if (code != null)
            {
                if(type == "specimen") 
                    return Json(( from data in db.SET_SPECIMEN
                                                  where data.SPECIMEN_CODE.ToLower().Contains(code.ToLower())
                                                  || data.SPECIMEN_NAME.ToLower().Contains(code.ToLower())
                                                  select new
                                                  {
                                                      SPECIMEN_NAME = data.SPECIMEN_NAME,
                                                      SPECIMEN_CODE = data.SPECIMEN_CODE,
                                                      SPECIMEN_NO = data.SPECIMEN_NO
                                                  }).OrderByDescending(m => m.SPECIMEN_NAME).Take(10), JsonRequestBehavior.AllowGet);
                else
                    return Json((from data in db.SET_PROMO_ITEM
                                                   where data.PROMO_ITEM_CODE.StartsWith(code)
                                                   select new
                                                   {
                                                       PROMO_ITEM_NAME = data.PROMO_ITEM_NAME,
                                                       PROMO_ITEM_CODE = data.PROMO_ITEM_CODE,
                                                       PROMO_ITEM_NO = data.PROMO_ITEM_NO
                                                   }).OrderByDescending(m => m.PROMO_ITEM_NO),JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDCROnSession(decimal? id, decimal? qty, string type,decimal? teacher_no,
            decimal? is_behalf, string teacher_mobile, string BEHALF_MOBILE, string rcvType, decimal detID)
        {
            Session["DCREntry" + type +"-" + id] = new temp_TRN_DCR_DET(id, type, qty, teacher_no, is_behalf, teacher_mobile, BEHALF_MOBILE,rcvType,detID);
            return Json(new { dt = type + id });
        }
        public JsonResult SaveDCROthersOnSession( string type, decimal? teacher_no, string teacher_mobile,string rcvType,decimal detID )
        {
            Session["DCREntry" + type +"-" +teacher_mobile] = new temp_TRN_DCR_DET(null, type, null, teacher_no, null, teacher_mobile, null, rcvType,detID);
            return Json(new { dt = type +"-" + teacher_mobile });
        }
        public JsonResult changeDeleteDCRFromSession(string id)
        {
            temp_TRN_DCR_DET tmpdet = (temp_TRN_DCR_DET)Session["DCREntry" + id];

            if (tmpdet != null)
            {
                tmpdet.setdeleted();
                Session["DCREntry" + id+"del"] = tmpdet;
                TRN_DCR_DET trn_dcr_det= db.TRN_DCR_DET.Where(t=>t.DCR_DET_NO==tmpdet.editID).Single();
                
                return Json(new { dt = id, html = getDetailFieldsINT(trn_dcr_det.TRN_DCR.DCR_TYPE_NO,tmpdet.editID)});
            }
            return Json((new { dt = "Not Found"}));
        }
        public JsonResult getDetails(decimal typeID)
        {
            string ret = "";
            
            TRN_DCR dcr = db.TRN_DCR.Where(s => s.DCR_NO == typeID).First();
            ret += (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD" || dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "MP") ?
                "<thead id=\"theadsdmp\"><tr><td>Item</td><td>Qty</td><td>Whom?</td><td>Contact</td><td>On Behalf?</td><td>Mobile</td></tr></thead>" :
                "<thead id=\"theadsdmp\"><tr><td>Contact No</td><td>Type</td></tr></thead>";
            IList<TRN_DCR_DET> dets = db.TRN_DCR_DET.Where(s => s.DCR_NO == typeID).ToList();
            foreach (TRN_DCR_DET dt in dets) {
                if (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "TC" || dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "LC")
                {
                    string tmCell = ((dt.IS_FOR_TEACHER==1) ? dt.TEACHER_MOBILE : dt.CLIENT_MOBILE);
                    ret += "<tr id='" + dcr.GEN_DCR_TYPE.DCR_TYPE_CODE.ToLower() + "-" + tmCell
                        + "'><td>" + tmCell + "</td><td>" + ((dt.IS_FOR_TEACHER == 1) ? "Teacher" : "Client") + "</td>"
                        +"<td><input value=\"Remove\" id=\"btnRemove\" class=\"btn btn-danger\" onclick=\"removeDCRDet($(this).closest('tr').attr('id'));$(this).closest('tr').remove();\" type=\"button\"></td></tr>";
                    SaveDCROthersOnSession(dcr.GEN_DCR_TYPE.DCR_TYPE_CODE.ToLower() , (dt.IS_FOR_TEACHER == 1) ? dt.TEACHER_NO : dt.CLIENT_NO,
                        tmCell,(dt.IS_FOR_TEACHER==1)?"teacher":"client",dt.DCR_DET_NO);
                }
                else if (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD" || dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "MP")
                {
                    decimal? tpID = ((dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ? dt.SPECIMEN_NO : dt.PROMO_ITEM_NO);
                    string forWhom = (dt.IS_FOR_TEACHER == 1)?"Teacher":"Client";
                    string tmName = (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ?
                        ((tpID != null) ? db.SET_SPECIMEN.Where(s => s.SPECIMEN_NO == tpID).Single().SPECIMEN_CODE : "") :
                        ((tpID != null) ? db.SET_PROMO_ITEM.Where(s => s.PROMO_ITEM_NO == tpID).Single().PROMO_ITEM_CODE : "");
                    decimal? Qty = ((dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ? dt.SPECIMEN_QTY : dt.PROMO_ITEM_QTY);
                    string WhomCell = ((forWhom=="Teacher")?dt.TEACHER_MOBILE:dt.CLIENT_MOBILE);
                    ret += "<tr id='" + ((dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ? "specimen" : "promotion") + "-"+tpID+
                        "'><td>"+tmName+"</td><td>"+Qty+"<td></td>"+forWhom+"</td><td>"+WhomCell+"</td><td>"+
                        ((dt.IS_ON_BEHALF==1)?"Yes":"No")+"</td><td>"+((dt.IS_ON_BEHALF==1)?dt.BEHALF_MOBILE:"")+"</td><td>"+
                        "<input value=\"Edit\" id=\"btnEdit\" class=\"btn btn-warning\" onclick=\"editDCRDET($(this).closest('tr').attr('id'));\" type=\"button\">"+
                        "<input value=\"Remove\" id=\"btnRemove\" class=\"btn btn-danger\" onclick=\"removeDCRDet($(this).closest('tr').attr('id'));\" type=\"button\"></td></tr>";
                    SaveDCROnSession(tpID, Qty, ((dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ? "specimen" : "promotion"), dt.TEACHER_NO, dt.IS_ON_BEHALF, WhomCell, dt.BEHALF_MOBILE, forWhom,dt.DCR_DET_NO);
                }
            }
            return Json(new { html = ret }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDetailsWithoutButtons(decimal typeID)
        {
            string ret = "";
            TRN_DCR dcr = db.TRN_DCR.Where(s => s.DCR_NO == typeID).First();
            ret += (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD" || dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "MP") ?
                "<thead id=\"theadsdmp\"><tr><td>Item</td><td>Qty</td><td>Whom?</td><td>Contact</td><td>On Behalf?</td><td>Mobile</td></tr></thead>" :
                "<thead id=\"theadsdmp\"><tr><td>Contact No</td><td>Type</td></tr></thead>";
            IList<TRN_DCR_DET> dets = db.TRN_DCR_DET.Where(s => s.DCR_NO == typeID).ToList();
            foreach (TRN_DCR_DET dt in dets)
            {
                if (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "TC" || dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "LC")
                {
                    string tmCell = ((dt.IS_FOR_TEACHER == 1) ? dt.TEACHER_MOBILE : dt.CLIENT_MOBILE);
                    ret += "<tr id='" + dcr.GEN_DCR_TYPE.DCR_TYPE_CODE.ToLower() + "-" + tmCell
                        + "'><td>" + tmCell + "</td><td>" + ((dt.IS_FOR_TEACHER == 1) ? "Teacher" : "Client") + "</td>"
                        + "<td></td></tr>";
                    
                }
                else if (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD" || dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "MP")
                {
                    decimal? tpID = ((dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ? dt.SPECIMEN_NO : dt.PROMO_ITEM_NO);
                    string forWhom = (dt.IS_FOR_TEACHER == 1) ? "Teacher" : "Client";
                    string tmName = (dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ?
                        ((tpID != null) ? db.SET_SPECIMEN.Where(s => s.SPECIMEN_NO == tpID).Single().SPECIMEN_CODE : "") :
                        ((tpID != null) ? db.SET_PROMO_ITEM.Where(s => s.PROMO_ITEM_NO == tpID).Single().PROMO_ITEM_CODE : "");
                    decimal? Qty = ((dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ? dt.SPECIMEN_QTY : dt.PROMO_ITEM_QTY);
                    string WhomCell = ((forWhom == "Teacher") ? dt.TEACHER_MOBILE : dt.CLIENT_MOBILE);
                    ret += "<tr id='" + ((dcr.GEN_DCR_TYPE.DCR_TYPE_CODE == "SD") ? "specimen" : "promotion") + "-" + tpID +
                        "'><td>" + tmName + "</td><td>" + Qty + "<td></td>" + forWhom + "</td><td>" + WhomCell + "</td><td>" +
                        ((dt.IS_ON_BEHALF == 1) ? "Yes" : "No") + "</td><td>" + ((dt.IS_ON_BEHALF == 1) ? dt.BEHALF_MOBILE : "") + "</td><td></td></tr>";
                   
                }
            }
            return Json(new { html = ret }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDetailFields(decimal typeID, string editID)
        {
            string ret = "";
            string js = "<script>";
            GEN_DCR_TYPE type = db.GEN_DCR_TYPE.Where(x => x.DCR_TYPE_NO == (typeID)).First();
            switch (type.DCR_TYPE_CODE)
            {
                case "TC":
                    ret += "<tr><td>Teacher Mobile No:</td><td><input id='contactDetail' name='contactDetail' tmp='teacher' type=\"text\" class=\"form-control\" ></td><td></td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='tc' id='btnAdd' class=\"btn btn-success\" /></td></tr>";
                    ret += "<input type='hidden' name='FOR_WHOM' value='teacher'/>";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    break;
                case "LC":
                    ret += "<tr><td>Library Mobile No: </td><td><input id='contactDetail' name='contactDetail' tmp='library' type=\"text\" class=\"form-control\"></td><td></td></tr>";
                    ret += "<tr><td>For Whom</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"teacher\">Teacher</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"client\">Client</td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='lc' id='btnAdd' class=\"btn btn-success\" /></td></tr>";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    break;
                case "SD":
                    ret += "<tr><td>Specimen No: </td><td><input id='promoID'  name='promoID' type=\"text\" class=\"promotion form-control\" tmp='specimen'></td><td></td>";
                    ret += "<td>QTY</td><td><input id='QTY' name='QTY' type=\"text\" class=\"form-control\"></td></tr>";
                    ret += "<tr><td>Mobile No: </td><td><input id='contactDetail' tmp='teacher' name='TEACHER_MOBILE' type=\"text\" class=\"form-control\"></td><td></td>";
                    ret += "<td><label><input id='chkBehalf' type=\"checkbox\" class=\"form-control\">On Behalf</label></td>";
                    ret += "<td ><input id='txtBehalf' name='BEHALF_MOBILE' style='display:none'  type=\"text\" class=\"form-control\"></td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='specimen' id='btnAdd' class=\"btn btn-success\" /></td>";
                    ret += "</tr>";
                    ret += "<input id=\"hdnDetCode\" name='SPECIMEN_NO' type=\"hidden\" value=\"0\" />";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    ret += "<input type='hidden' name='FOR_WHOM' value='teacher'/>";
                    break;
                case "MP":
                    ret += "<tr><td>Promotional Item No: </td><td><input id='promoID' name='promoID' type=\"text\" class=\"promotion form-control\" tmp='promotion'></td><td></td>";
                    ret += "<td>QTY</td><td><input id='QTY' name='QTY' type=\"text\" class=\"form-control\"></td></tr>";
                    ret += "<tr><td>For Whom</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"Teacher\">Teacher</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"Client\">Client</td></tr>";
                    ret += "<tr><td>Mobile No: </td><td><input id='contactDetail' tmp='teacher' name='TEACHER_MOBILE' type=\"text\" class=\"form-control\"></td><td></td>";
                    ret += "<td><label><input id='chkBehalf' type=\"checkbox\" class=\"form-control\">On Behalf</label></td>";
                    ret += "<td ><input style='display:none' name='BEHALF_MOBILE' id='txtBehalf' type=\"text\" class=\"form-control\"></td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='promotion' id='btnAdd' class=\"btn btn-success\" /></td>";
                    ret += "</tr>";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    ret += "<input id=\"hdnDetCode\" name='PROMO_ITEM_NO' type=\"hidden\" value=\"0\" />";
                    break;
                default:
                    ret = "";
                    break;
            }
            List<string> lstTmp = new List<string>();
            foreach (string curSession in Session)
                if (curSession.Contains("DCREntry"))
                    lstTmp.Add(curSession);
            foreach (string del in lstTmp)
                Session.Remove(del);
            return Json(new { html = ret, js = js }, JsonRequestBehavior.AllowGet);
        }
        /***
         * Internal call to this function
         * here this will generate the table 
         * for the edit dcr_det
         * 
         * */
        public string getDetailFieldsINT(decimal typeID, decimal editID)
        {
            string ret = "";
            GEN_DCR_TYPE type = db.GEN_DCR_TYPE.Where(x => x.DCR_TYPE_NO == (typeID)).First();
            TRN_DCR_DET dcr_det = db.TRN_DCR_DET.Where(x => x.DCR_DET_NO == editID).Single();
            string cd = (dcr_det.IS_FOR_CLIENT==1)?dcr_det.CLIENT_MOBILE:dcr_det.TEACHER_MOBILE;
            switch (type.DCR_TYPE_CODE)
            {
                case "TC":
                    ret += "<tr><td>Teacher Mobile No:</td><td><input id='contactDetail' name='contactDetail' tmp='teacher' type=\"text\" class=\"form-control\" value="+cd+" ></td><td></td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='tc' id='btnAdd' class=\"btn btn-success\" /></td></tr>";
                    ret += "<input type='hidden' name='FOR_WHOM' value='teacher'/>";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    break;
                case "LC":
                    ret += "<tr><td>Library Mobile No: </td><td><input id='contactDetail' name='contactDetail' tmp='library' value='" + cd + "'  type=\"text\" class=\"form-control\"></td><td></td></tr>";
                    ret += "<tr><td>For Whom</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"teacher\">Teacher</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"client\">Client</td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='lc' id='btnAdd' class=\"btn btn-success\" /></td></tr>";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    break;
                case "SD":
                    ret += "<tr><td>Specimen No: </td><td><input id='promoID'  name='promoID' type=\"text\" class=\"promotion form-control\" value='"+
                        (( dcr_det.SET_SPECIMEN!=null)?dcr_det.SET_SPECIMEN.SPECIMEN_CODE:"")+
                        "' tmp='specimen' /></td><td></td>";
                    ret += "<td>QTY</td><td><input id='QTY' name='QTY' type=\"text\" class=\"form-control\" value='" + dcr_det.SPECIMEN_QTY + "' /></td></tr>";
                    ret += "<tr><td>Mobile No: </td><td><input id='contactDetail' tmp='teacher' name='TEACHER_MOBILE' type=\"text\" value='"+cd+"' class=\"form-control\"></td><td></td>";
                    ret += "<td><label><input id='chkBehalf' type=\"checkbox\" class=\"form-control\">On Behalf</label></td>";
                    ret += "<td ><input id='txtBehalf' name='BEHALF_MOBILE' style='display:none'  type=\"text\" class=\"form-control\" value='"+dcr_det.BEHALF_MOBILE+"'></td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='specimen' id='btnAdd'  class=\"btn btn-success\" /></td>";
                    ret += "</tr>";
                    ret += "<input id=\"hdnDetCode\" name='SPECIMEN_NO' type=\"hidden\" value='"+dcr_det.SPECIMEN_NO+"' />";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    ret += "<input type='hidden' name='FOR_WHOM' value='teacher'/>";
                    break;
                case "MP":
                    ret += "<tr><td>Promotional Item No: </td><td><input id='promoID' name='promoID' type=\"text\" class=\"promotion form-control\" tmp='promotion' value='"+
                        (( dcr_det.SET_PROMO_ITEM!=null) ? dcr_det.SET_PROMO_ITEM.PROMO_ITEM_CODE : "") +
                        "'></td><td></td>";
                    ret += "<td>QTY</td><td><input id='QTY' name='QTY' type=\"text\" class=\"form-control\" value='"+dcr_det.PROMO_ITEM_QTY+"' /></td></tr>";
                    ret += "<tr><td>For Whom</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"Teacher\">Teacher</td><td> <input type=\"radio\" name=\"FOR_WHOM\" value=\"Client\">Client</td></tr>";
                    ret += "<tr><td>Mobile No: </td><td><input id='contactDetail' tmp='teacher' name='TEACHER_MOBILE' value='"+cd+"' type=\"text\" class=\"form-control\"></td><td></td>";
                    ret += "<td><label><input id='chkBehalf' type=\"checkbox\" class=\"form-control\">On Behalf</label></td>";
                    ret += "<td ><input style='display:none' name='BEHALF_MOBILE' id='txtBehalf' type=\"text\" class=\"form-control\" value='"+dcr_det.BEHALF_MOBILE+"'></td>";
                    ret += "<td><input type=\"button\" value=\"Add\" tmp='promotion' id='btnAdd' class=\"btn btn-success\" /></td>";
                    ret += "</tr>";
                    ret += "<input type='hidden' name='hdnContactNo' value='0'/>";
                    ret += "<input id=\"hdnDetCode\" name='PROMO_ITEM_NO' type=\"hidden\" value='" + dcr_det.PROMO_ITEM_NO + "' />";
                    break;
                default:
                    ret = "";
                    break;
            }
            List<string> lstTmp = new List<string>();
            foreach (string curSession in Session)
                if (curSession.Contains("DCREntry"))
                    lstTmp.Add(curSession);
            foreach (string del in lstTmp)
                Session.Remove(del);
            return ret;
        }
    }
}
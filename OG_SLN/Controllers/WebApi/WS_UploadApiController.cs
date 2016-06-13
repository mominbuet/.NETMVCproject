using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using OG_SLN.Models;
using System.Transactions;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using OG_SLN.Controllers.WebApi;


namespace OG_SLN.Controllers
{
    public class WS_UploadApiController : Controller
    {

        private OGDBEntities db = new OGDBEntities();

        SEC_USERS_LOGIN_Result1 sess_sec_users = null;
        decimal? sess_USER_NO = null;
        decimal? sess_LOGON_NO = null;
        decimal? sess_entry_user_no = null;
        String sess_entry_user_name = null;
        decimal? sess_zm_user_no = null;
        decimal? sess_agent_user_no = null;
        static string cont_name = "WS__UPLOADAPI";

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        private void GetLoggedInInfo()
        {
            this.sess_sec_users = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;
            this.sess_LOGON_NO = Session["sess_LOGON_NO"] as decimal?;
            this.sess_USER_NO = Session["sess_USER_NO"] as decimal?;

            this.sess_entry_user_no = Session["sess_entry_user_no"] as decimal?;
            this.sess_zm_user_no = Session["sess_zm_user_no"] as decimal?;

            this.sess_agent_user_no = Session["sess_agent_user_no"] as decimal?;


            if (this.sess_LOGON_NO == null)
            {
                this.sess_LOGON_NO = 1;
            }

            if (this.sess_USER_NO == null)
            {
                this.sess_USER_NO = 45;
            }
            if (this.sess_entry_user_no == null)
            {
                this.sess_entry_user_no = 45;
            }
            if (this.sess_zm_user_no == null)
            {
                this.sess_zm_user_no = 45;
            }

            /*
            if (sess_sec_users == null) {
                throw new Exception("Invalid Login");
            } else if (sess_USER_NO == null) {
                throw new Exception("Invalid Login");
            } else if (sess_LOGON_NO == null) {
                throw new Exception("Invalid Login");
            }
            */
        }

        private void GetUserInfo(USER_INFO user_info)
        {
            if ((user_info != null) && (user_info.USER_NAME != null))
            {
                var user_result = db.SEC_USERS_ZM_LOGIN_GET(user_info.USER_NAME, 1, null, null).FirstOrDefault();

                if (user_result.USER_TYPE_NO == (decimal)EUserTypes.Agent)
                {
                    this.sess_entry_user_no = user_result.USER_NO;
                    this.sess_zm_user_no = user_result.USER_PARENT_NO;
                    this.sess_agent_user_no = user_result.USER_NO;
                }
                else
                {
                    this.sess_entry_user_no = user_result.USER_NO;
                    this.sess_zm_user_no = user_result.USER_NO;
                    this.sess_agent_user_no = null;
                }
                this.sess_entry_user_name = user_result.USER_NAME;
            }

            if (this.sess_LOGON_NO == null)
            {
                this.sess_LOGON_NO = 0;
            }

        }


        [HttpPost]
        public JsonResult Upload_Movement(TRN_USER_MOVEMENTS_MASTER master_obj)
        {

            //this.GetLoggedInInfo();
            using (OGDBEntities db = new OGDBEntities())
            {
                bool is_success = false;
                string msg = @"";
                if (master_obj != null)
                {
                    if (master_obj.USER_INFO == null)
                    {
                        is_success = false;
                        msg = "User Information is required.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(master_obj.USER_INFO.USER_NAME) || (string.IsNullOrWhiteSpace(master_obj.USER_INFO.USER_NAME)))
                        {
                            is_success = false;
                            msg = "User Name is required.";
                        }
                        else
                        {

                            this.GetUserInfo(master_obj.USER_INFO);

                            try
                            {
                                decimal USER_NO = this.sess_entry_user_no.Value; //sess_sec_users.USER_NO.Value;

                                if (master_obj.TRN_USER_MOVEMENTS_UP != null)
                                {
                                    foreach (var move in master_obj.TRN_USER_MOVEMENTS_UP)
                                    {
                                        try
                                        {
                                            if (move != null)
                                            {
                                                if (move.MOVE_DATE != null && move.MOVE_TIME != null)
                                                {
                                                    TRN_USER_MOVEMENTS item = new TRN_USER_MOVEMENTS();
                                                    //item.USER_NO = USER_NO;
                                                    item.USER_NO = move.USER_NO.Value;
                                                    item.MOVE_DATE = move.MOVE_DATE.Value;
                                                    item.MOVE_TIME = move.MOVE_TIME.Value;
                                                    item.LAT_VAL = move.LAT_VAL;
                                                    item.LON_VAL = move.LON_VAL;
                                                    item.INSERT_OFFLINE_TIME = move.MOVE_TIME.Value;
                                                    item.BATT_PCT = move.BATT_PCT;

                                                    db.TRN_USER_MOVEMENTS_INSERT(item.USER_NO, sess_LOGON_NO, item.INSERT_OFFLINE_TIME, item.INSERT_OFFLINE_SYNC, (decimal)ENTRY_MODE.OFFLINE, item.USER_NO, item.MOVE_DATE, item.MOVE_TIME, item.LAT_VAL, item.LON_VAL, item.LOCATION_NAME, item.BATT_PCT);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            DbErrorTracker db_error = new DbErrorTracker();
                                            db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                        }
                                    }

                                    try
                                    {
                                        db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "USER MOVEMENT");
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                is_success = true;
                                msg = "saved successfully";

                            }
                            catch (Exception ex)
                            {
                                is_success = false;
                                msg = "failed to save";

                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                        }
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public void call_only_local(TRN_DCR_MASTER master)
        {
            try
            {
                MobileLogger.logDefault(master.TRN_DCR_UP.Count.ToString(), "debug_mode");
                MobileLogger.logDefault(master.USER_INFO.USER_NAME, "debug_mode");
            }
            catch (Exception ex)
            {
                MobileLogger.logDefault(ex.Message, "debug_mode");
            }
        }
        [HttpPost]
        public JsonResult Upload_DCR(TRN_DCR_MASTER master_obj)
        {
            using (OGDBEntities db = new OGDBEntities())
            {
                /*MobileLogger.logDefault(" master_obj.USER_INFO = " + master_obj.USER_INFO.USER_NAME + " content length " +
                    Request.ContentLength + " ipaddress " + Request.UserHostAddress + "Upload dcr called", "Upload_DCR");*/
                //this.GetLoggedInInfo();
                //call_only_local(master_obj);
                TRN_DCR_UP dcr_up_log = null;
                bool is_success = false;
                decimal DCR_NO = 0, DCR_DET_NO = 0;
                string msg = @"";

                if (master_obj != null)
                {
                    if (master_obj.USER_INFO == null)
                    {
                        is_success = false;
                        msg = "User Information is required.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(master_obj.USER_INFO.USER_NAME) || (string.IsNullOrWhiteSpace(master_obj.USER_INFO.USER_NAME)))
                        {
                            is_success = false;
                            msg = "User Name is required.";
                        }
                        else
                        {

                            this.GetUserInfo(master_obj.USER_INFO);

                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                            {
                                try
                                {

                                    if (master_obj.TRN_DCR_UP != null)
                                    {
                                        try
                                        {
                                            foreach (TRN_DCR_UP dcr_up in master_obj.TRN_DCR_UP)
                                            {
                                                if (dcr_up.TRN_DCR_DATE != null)
                                                {

                                                    //decimal DCR_NO = db.TRN_DCR_OFF_SAVE(this.sess_entry_user_no, this.sess_LOGON_NO, dcr_up.ACTION_OFFLINE_TIME, null, (decimal) ENTRY_MODE.OFFLINE, dcr_up.DCR_TYPE_NO, this.sess_zm_user_no, this.sess_agent_user_no, dcr_up.IS_REF_ZM, dcr_up.REF_ZM_USER_NO, dcr_up.REF_ZM_MOBILE, dcr_up.WORK_PUR_NO, dcr_up.WORK_AREA_FROM_LAT, dcr_up.WORK_AREA_FROM_LON, dcr_up.WORK_AREA_FROM_NAME, dcr_up.WORK_AREA_TO_LAT, dcr_up.WORK_AREA_TO_LON, dcr_up.WORK_AREA_TO_NAME, dcr_up.TIME_FROM, dcr_up.TIME_TO, dcr_up.TIME_GAP, dcr_up.DIVISION_NO, dcr_up.ZONE_NO, dcr_up.ZILLA_NO, dcr_up.THANA_NO, dcr_up.INSTITUTE_NO, dcr_up.TRANS_TYPE_NO, dcr_up.FARE_AMT, dcr_up.TRN_DCR_DATE, dcr_up.IS_MANUAL_ENTRY, dcr_up.OFFLINE_DCR_NO, dcr_up.COMMENTS, dcr_up.ENTRY_STATE).First().Value;
                                                    DCR_NO = db.TRN_DCR_OFF_SAVE(dcr_up.USER_NO.Value, this.sess_LOGON_NO, dcr_up.ACTION_OFFLINE_TIME, null, (decimal)ENTRY_MODE.OFFLINE, dcr_up.DCR_TYPE_NO, dcr_up.USER_NO, null, dcr_up.IS_REF_ZM, dcr_up.REF_ZM_USER_NO, dcr_up.REF_ZM_MOBILE, dcr_up.WORK_PUR_NO, dcr_up.WORK_AREA_FROM_LAT, dcr_up.WORK_AREA_FROM_LON, dcr_up.WORK_AREA_FROM_NAME, dcr_up.WORK_AREA_TO_LAT, dcr_up.WORK_AREA_TO_LON, dcr_up.WORK_AREA_TO_NAME, dcr_up.TIME_FROM, dcr_up.TIME_TO, dcr_up.TIME_GAP, dcr_up.DIVISION_NO, dcr_up.ZONE_NO, dcr_up.ZILLA_NO, dcr_up.THANA_NO, dcr_up.INSTITUTE_NO, dcr_up.TRANS_TYPE_NO, dcr_up.FARE_AMT, dcr_up.TRN_DCR_DATE, dcr_up.IS_MANUAL_ENTRY, dcr_up.OFFLINE_DCR_NO, dcr_up.COMMENTS, dcr_up.ENTRY_STATE).First().Value;
                                                    is_success = DCR_NO != 0;
                                                    //if (dcr_up.USER_NO == 287)
                                                    //{
                                                    //    dcr_up_log = dcr_up;
                                                    //    MobileLogger.logDefault(" User " + dcr_up.USER_NO + " OFFLINE_DCR_NO " + dcr_up.OFFLINE_DCR_NO + " entryState " + dcr_up.ENTRY_STATE.ToString() + " dcrno " + DCR_NO, "OnlyMaster");
                                                    //}
                                                    //if (dcr_up.DCR_TYPE_NO != 5 && dcr_up.TRN_DCR_DET_UP.Count == 0)
                                                    //{
                                                    //    MobileLogger.logDefault(" User " + dcr_up.USER_NO + " OFFLINE_DCR_NO " + dcr_up.OFFLINE_DCR_NO + " entryState " + dcr_up.ENTRY_STATE.ToString() + " dcrno " + DCR_NO, "OnlyMaster");
                                                    //    //is_success = false;
                                                    //}
                                                    //if (DCR_NO == 0) MobileLogger.logDefault(" User " + dcr_up.USER_NO + " OFFLINE_DCR_NO " + dcr_up.OFFLINE_DCR_NO + " entryState " + dcr_up.ENTRY_STATE.ToString() + " dcrno " + DCR_NO, "dcrtest");
                                                    if (dcr_up.TRN_DCR_DET_UP != null && DCR_NO > 0)
                                                    {
                                                        //try
                                                        //{
                                                        foreach (TRN_DCR_DET_UP item in dcr_up.TRN_DCR_DET_UP)
                                                        {
                                                            item.DCR_NO = DCR_NO;

                                                            //decimal DCR_DET_NO = db.TRN_DCR_DET_OFF_SAVE(this.sess_entry_user_no, this.sess_LOGON_NO, item.ACTION_OFFLINE_TIME, null, (decimal) ENTRY_MODE.OFFLINE, item.DCR_NO, item.IS_FOR_TEACHER, item.TEACHER_NO, item.TEACHER_MOBILE, item.IS_ON_BEHALF, item.BEHALF_MOBILE, item.SPECIMEN_NO, item.SPECIMEN_QTY, item.IS_FOR_CLIENT, item.CLIENT_NO, item.CLIENT_MOBILE, item.PROMO_ITEM_NO, item.PROMO_ITEM_QTY, item.OFFLINE_DCR_NO, item.OFFLINE_DCR_DET_NO, item.ENTRY_STATE).First().Value;
                                                            //if (dcr_up.USER_NO == 287)
                                                            //    MobileLogger.logDefault(" Success for detail" + DCR_DET_NO + " User " + dcr_up.USER_NO + " OFFLINE_DCR_NO " + dcr_up.OFFLINE_DCR_NO + " entryState " + dcr_up.ENTRY_STATE.ToString() + " dcrno " + DCR_NO, "OnlyMaster");
                                                            DCR_DET_NO = db.TRN_DCR_DET_OFF_SAVE(dcr_up.USER_NO.Value, this.sess_LOGON_NO,
                                                                item.ACTION_OFFLINE_TIME, null, (decimal)ENTRY_MODE.OFFLINE, item.DCR_NO, item.IS_FOR_TEACHER,
                                                                item.TEACHER_NO, item.TEACHER_MOBILE, item.IS_ON_BEHALF, item.BEHALF_MOBILE, item.SPECIMEN_NO,
                                                                item.SPECIMEN_QTY, item.IS_FOR_CLIENT, item.CLIENT_NO, item.CLIENT_MOBILE, item.PROMO_ITEM_NO, item.PROMO_ITEM_QTY, item.OFFLINE_DCR_NO, item.OFFLINE_DCR_DET_NO,
                                                                item.ENTRY_STATE, item.BEHALF_NICK, item.TEACHER_NICK).First().Value;

                                                            // det_insert =(DCR_DET_NO == 0)? false:true;

                                                            //      if (DCR_DET_NO == 0) MobileLogger.logDefault("dcr_no " + item.DCR_NO + " offline dcr_no " + item.OFFLINE_DCR_DET_NO + " offline_dcr_detNo " + item.OFFLINE_DCR_DET_NO + " spec qty " + item.SPECIMEN_QTY + " entryState " + item.ENTRY_STATE.ToString() + " dcrdetno " + DCR_DET_NO, "dcrtest");
                                                            //scope.Complete();
                                                        }

                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //    scope.Dispose();
                                                        //    msg = "An Error Occurred during Saving one Dcr Data, Error Code - 888, Please Try Again."; //ex.Message;
                                                        //    is_success = false;
                                                        //    msg = ex.Message;
                                                        //    DbErrorTracker db_error = new DbErrorTracker();
                                                        //    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                                        //}

                                                    }

                                                }
                                            }
                                            db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "DCR ENTYRY");
                                            if (is_success)
                                            {
                                                //if (dcr_up_log != null)
                                                //    if (dcr_up_log.USER_NO == 287)
                                                //        MobileLogger.logDefault(" Success " + dcr_up_log.TRN_DCR_DET_UP.Count + " User " + dcr_up_log.USER_NO + " OFFLINE_DCR_NO " + dcr_up_log.OFFLINE_DCR_NO + " entryState " + dcr_up_log.ENTRY_STATE.ToString() + " dcrno " + DCR_NO, "OnlyMaster");
                                                scope.Complete();

                                                /*if (DCR_NO != 0 || DCR_DET_NO != 0)
                                                {*/
                                                //is_success = true;
                                                msg = @"saved successfully";

                                            }
                                            else
                                            {
                                                //if (dcr_up_log != null)
                                                //    if (dcr_up_log.USER_NO == 287)
                                                //        MobileLogger.logDefault(" Failure " + dcr_up_log.TRN_DCR_DET_UP.Count + " User " + dcr_up_log.USER_NO + " OFFLINE_DCR_NO " + dcr_up_log.OFFLINE_DCR_NO + " entryState " + dcr_up_log.ENTRY_STATE.ToString() + " dcrno " + DCR_NO, "OnlyMaster");
                                                scope.Dispose();
                                                msg = "An Error Occurred during Saving one Dcr Data, Error Code - 888, Please Try Again."; //ex.Message;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            /*if (dcr_up_log != null)
                                                if (dcr_up_log.USER_NO == 287)*/
                                            //MobileLogger.logDefault(" user_no "+sess_entry_user_name+" \n"+ex.Message + " \n inner \n" + ex.InnerException.Message, "OnlyMaster");
                                            scope.Dispose();
                                            msg = "An Error Occurred during Saving one Dcr Data, Error Code - 888, Please Try Again."; //ex.Message;
                                            is_success = false;
                                            msg = ex.Message;
                                            DbErrorTracker db_error = new DbErrorTracker();
                                            db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                        }
                                    }

                                    /*try
                                    {
                                    
                                        db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "DCR ENTYRY");
                                        scope.Complete();
                                        is_success = true;
                                        msg = @"saved successfully";
                                    }
                                    catch (Exception ex)
                                    {
                                        scope.Dispose();
                                        msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }*/

                                }
                                catch (DbException ex)
                                {
                                    scope.Dispose();
                                    msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                                catch (EntitySqlException ex)
                                {
                                    scope.Dispose();
                                    msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                                catch (DbUpdateException ex)
                                {
                                    scope.Dispose();
                                    msg = msg = ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                                catch (Exception ex)
                                {
                                    scope.Dispose();
                                    msg = ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                            }
                        }
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg };
                /*MobileLogger.logDefault("master_obj.USER_INFO = " + master_obj.USER_INFO.USER_NAME + " content length " +
                Request.ContentLength + " ipaddress " + Request.UserHostAddress + "Upload dcr finished", "Upload_DCR");*/
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        public JsonResult Upload_DCR_DET(TRN_DCR_MASTER master_obj)
        {
            //MobileLogger.logDefault("here", "debug_mode2");
            //this.GetLoggedInInfo();
            using (OGDBEntities db = new OGDBEntities())
            {
                bool is_success = false;
                string msg = @"";

                if (master_obj != null)
                {
                    if (master_obj.USER_INFO == null)
                    {
                        is_success = false;
                        msg = "User Information is required.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(master_obj.USER_INFO.USER_NAME) || (string.IsNullOrWhiteSpace(master_obj.USER_INFO.USER_NAME)))
                        {
                            is_success = false;
                            msg = "User Name is required.";
                        }
                        else
                        {

                            this.GetUserInfo(master_obj.USER_INFO);
                            //call_only_local(master_obj);
                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                            {
                                try
                                {
                                    if (master_obj.TRN_DCR_UP != null)
                                    {
                                        try
                                        {
                                            foreach (TRN_DCR_UP dcr_up in master_obj.TRN_DCR_UP)
                                            {
                                                if (dcr_up.TRN_DCR_DATE != null)
                                                {
                                                    //decimal DCR_NO = db.TRN_DCR_OFF_SAVE(this.sess_entry_user_no, this.sess_LOGON_NO, dcr_up.ACTION_OFFLINE_TIME, null, (decimal) ENTRY_MODE.OFFLINE, dcr_up.DCR_TYPE_NO, this.sess_zm_user_no, this.sess_agent_user_no, dcr_up.IS_REF_ZM, dcr_up.REF_ZM_USER_NO, dcr_up.REF_ZM_MOBILE, dcr_up.WORK_PUR_NO, dcr_up.WORK_AREA_FROM_LAT, dcr_up.WORK_AREA_FROM_LON, dcr_up.WORK_AREA_FROM_NAME, dcr_up.WORK_AREA_TO_LAT, dcr_up.WORK_AREA_TO_LON, dcr_up.WORK_AREA_TO_NAME, dcr_up.TIME_FROM, dcr_up.TIME_TO, dcr_up.TIME_GAP, dcr_up.DIVISION_NO, dcr_up.ZONE_NO, dcr_up.ZILLA_NO, dcr_up.THANA_NO, dcr_up.INSTITUTE_NO, dcr_up.TRANS_TYPE_NO, dcr_up.FARE_AMT, dcr_up.TRN_DCR_DATE, dcr_up.IS_MANUAL_ENTRY, dcr_up.OFFLINE_DCR_NO, dcr_up.COMMENTS, dcr_up.ENTRY_STATE).First().Value;
                                                    decimal DCR_NO = db.TRN_DCR_GET_DCR_NO(dcr_up.USER_NO, dcr_up.OFFLINE_DCR_NO).First().Value;
                                                    if (dcr_up.TRN_DCR_DET_UP != null && DCR_NO > 0)
                                                    {
                                                        try
                                                        {
                                                            foreach (TRN_DCR_DET_UP item in dcr_up.TRN_DCR_DET_UP)
                                                            {
                                                                item.DCR_NO = DCR_NO;
                                                                //decimal DCR_DET_NO = db.TRN_DCR_DET_OFF_SAVE(this.sess_entry_user_no, this.sess_LOGON_NO, item.ACTION_OFFLINE_TIME, null, (decimal) ENTRY_MODE.OFFLINE, item.DCR_NO, item.IS_FOR_TEACHER, item.TEACHER_NO, item.TEACHER_MOBILE, item.IS_ON_BEHALF, item.BEHALF_MOBILE, item.SPECIMEN_NO, item.SPECIMEN_QTY, item.IS_FOR_CLIENT, item.CLIENT_NO, item.CLIENT_MOBILE, item.PROMO_ITEM_NO, item.PROMO_ITEM_QTY, item.OFFLINE_DCR_NO, item.OFFLINE_DCR_DET_NO, item.ENTRY_STATE).First().Value;
                                                                decimal DCR_DET_NO = db.TRN_DCR_DET_OFF_SAVE(dcr_up.USER_NO.Value, this.sess_LOGON_NO,
                                                                    item.ACTION_OFFLINE_TIME, null, (decimal)ENTRY_MODE.OFFLINE, item.DCR_NO, item.IS_FOR_TEACHER,
                                                                    item.TEACHER_NO, item.TEACHER_MOBILE, item.IS_ON_BEHALF, item.BEHALF_MOBILE, item.SPECIMEN_NO,
                                                                    item.SPECIMEN_QTY, item.IS_FOR_CLIENT, item.CLIENT_NO, item.CLIENT_MOBILE, item.PROMO_ITEM_NO,
                                                                    item.PROMO_ITEM_QTY, item.OFFLINE_DCR_NO, item.OFFLINE_DCR_DET_NO, item.ENTRY_STATE, item.BEHALF_NICK, item.TEACHER_NICK).First().Value;
                                                                //scope.Complete();
                                                            }
                                                            /* Added later on may 27,2015 */
                                                            is_success = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            scope.Dispose();
                                                            msg = "An Error Occurred during Saving one Dcr Data, Error Code - 888, Please Try Again."; //ex.Message;
                                                            is_success = false;
                                                            msg = ex.Message;
                                                            DbErrorTracker db_error = new DbErrorTracker();
                                                            db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                                        }
                                                    }
                                                }
                                            }
                                            db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "DCR ENTYRY");
                                            if (is_success)
                                            {
                                                scope.Complete();
                                                msg = @"saved successfully";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            scope.Dispose();
                                            msg = "An Error Occurred during Saving one Dcr Data, Error Code - 888, Please Try Again."; //ex.Message;
                                            is_success = false;
                                            msg = ex.Message;
                                            DbErrorTracker db_error = new DbErrorTracker();
                                            db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                        }
                                    }


                                }
                                catch (DbException ex)
                                {
                                    scope.Dispose();
                                    msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                                catch (EntitySqlException ex)
                                {
                                    scope.Dispose();
                                    msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                                catch (DbUpdateException ex)
                                {
                                    scope.Dispose();
                                    msg = msg = ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                                catch (Exception ex)
                                {
                                    scope.Dispose();
                                    msg = ex.Message;
                                    is_success = false;
                                    msg = ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                            }
                        }
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Upload_Expense_DET(TRN_EXPENSE_UP_MASTER master_obj /*TRN_EXPENSE_UP trn_expense_up*/)
        {
            using (OGDBEntities db = new OGDBEntities())
            {
                bool is_success = false;
                string msg = @"";
                decimal EXP_NO = 0, det = 0;
                if (master_obj != null)
                {
                    if (master_obj.USER_INFO == null)
                    {
                        is_success = false;
                        msg = "User Information is required.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(master_obj.USER_INFO.USER_NAME) || (string.IsNullOrWhiteSpace(master_obj.USER_INFO.USER_NAME)))
                        {
                            is_success = false;
                            msg = "User Name is required.";
                        }
                        else
                        {

                            this.GetUserInfo(master_obj.USER_INFO);

                            if (master_obj.TRN_EXPENSE_UP != null)
                            {
                                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                                {
                                    try
                                    {

                                        foreach (TRN_EXPENSE_UP trn_expense_up in master_obj.TRN_EXPENSE_UP)
                                        {
                                            if (trn_expense_up.TRN_EXP_DATE != null)
                                            {
                                                TRN_EXPENSE trn_expense = new TRN_EXPENSE();
                                                try
                                                {
                                                    if (trn_expense_up != null)
                                                    {
                                                        trn_expense.USER_NO = trn_expense_up.USER_NO.Value;
                                                        trn_expense.LAST_ACTION_OFFLINE_TIME = trn_expense_up.ACTION_OFFLINE_TIME;
                                                        trn_expense.LAST_ACTION_OFFLINE_SYNC = trn_expense_up.ACTION_OFFLINE_SYNC;
                                                        trn_expense.LAST_IS_OFFLINE = trn_expense_up.ACTION_IS_OFFLINE;
                                                        trn_expense.TRN_EXP_DATE = trn_expense_up.TRN_EXP_DATE.Value;
                                                        trn_expense.IS_MANUAL_ENTRY = 1; // trn_expense_up.IS_MANUAL_ENTRY;
                                                        trn_expense.OFFLINE_EXP_NO = trn_expense_up.OFFLINE_EXP_NO;
                                                        trn_expense.ENTRY_STATE = trn_expense_up.ENTRY_STATE.Value;
                                                    }

                                                    EXP_NO = decimal.Parse(db.TRN_EXPENSE_GET_EXP_NO(trn_expense.USER_NO, decimal.Parse(trn_expense.OFFLINE_EXP_NO.ToString())).First().EXP_NO.ToString());
                                                    is_success = EXP_NO != 0;
                                                    if (trn_expense_up.TRN_EXPENSE_DET_UP != null)
                                                    {
                                                        foreach (var item in trn_expense_up.TRN_EXPENSE_DET_UP)
                                                        {
                                                            TRN_EXPENSE_DET det_item = new TRN_EXPENSE_DET();
                                                            det_item.EXP_NO = EXP_NO;
                                                            det_item.LAST_ACTION_OFFLINE_TIME = item.ACTION_OFFLINE_TIME;
                                                            det_item.LAST_ACTION_OFFLINE_SYNC = item.ACTION_OFFLINE_SYNC;
                                                            det_item.EXP_TYPE_NO = item.EXP_TYPE_NO.Value;
                                                            det_item.EXP_AMT = item.EXP_AMT.Value;
                                                            det_item.OFFLINE_EXP_NO = item.OFFLINE_EXP_NO;
                                                            det_item.OFFLINE_EXP_DET_NO = item.OFFLINE_EXP_DET_NO;
                                                            det_item.ENTRY_STATE = item.ENTRY_STATE.Value;
                                                            det_item.LAT_VAL = item.LAT_VAL;
                                                            det_item.LON_VAL = item.LON_VAL;
                                                            det_item.VENDOR = item.VENDOR;
                                                            det_item.COMMENTS = item.COMMENTS;
                                                            det = db.TRN_EXPENSE_DET_OFF_SAVE(trn_expense.USER_NO, this.sess_LOGON_NO, det_item.LAST_ACTION_OFFLINE_TIME, det_item.LAST_ACTION_OFFLINE_SYNC, (decimal)ENTRY_MODE.OFFLINE, det_item.EXP_NO, det_item.EXP_TYPE_NO, det_item.EXP_AMT, det_item.OFFLINE_EXP_NO, det_item.OFFLINE_EXP_DET_NO, det_item.ENTRY_STATE, det_item.LAT_VAL, det_item.LON_VAL, det_item.VENDOR, det_item.COMMENTS).First().Value;
                                                        }
                                                    }
                                                }
                                                catch (Exception exSave)
                                                {
                                                    scope.Dispose();
                                                    msg = "An Error Occurred during Expense Data, Error Code - 888, Please Try Again."; //ex.Message;
                                                    if (EXP_NO != 0 || det != 0)
                                                    {
                                                        is_success = false;
                                                        msg = exSave.Message;
                                                    }
                                                    DbErrorTracker db_error = new DbErrorTracker();
                                                    db_error.WriteErrorLog(db_error.GetErrorMessage(exSave, true), exSave, sess_entry_user_name);
                                                }
                                            }
                                        }
                                        db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "OTHER EXPENSE");
                                        if (is_success)
                                        {
                                            scope.Complete();
                                            //is_success = (det==0)?false:true;
                                            msg = @"saved successfully";
                                        }
                                        else
                                        {

                                            scope.Dispose();
                                            //is_success = (det==0)?false:true;
                                            //is_success = false;
                                            msg = @"Primary key not found";
                                        }
                                    }
                                    catch (DbException ex)
                                    {
                                        scope.Dispose();
                                        msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                    catch (EntitySqlException ex)
                                    {
                                        scope.Dispose();
                                        msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                    catch (DbUpdateException ex)
                                    {
                                        scope.Dispose();
                                        msg = msg = ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                    catch (Exception ex)
                                    {
                                        scope.Dispose();
                                        msg = ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                }
                            }
                            else
                            {
                                is_success = true;
                                msg = "Success with null/empty values";
                            }
                        }
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg /*, @trn_expense_up = trn_expense_up */};

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Upload_Expense(TRN_EXPENSE_UP_MASTER master_obj /*TRN_EXPENSE_UP trn_expense_up*/)
        {
            //this.GetLoggedInInfo();
            using (OGDBEntities db = new OGDBEntities())
            {
                bool is_success = false;
                decimal det = 0, user_calling = 0;
                decimal EXP_NO = 0;
                string msg = @"";
                if (master_obj != null)
                {
                    if (master_obj.USER_INFO == null)
                    {
                        is_success = false;
                        msg = "User Information is required.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(master_obj.USER_INFO.USER_NAME) || (string.IsNullOrWhiteSpace(master_obj.USER_INFO.USER_NAME)))
                        {
                            is_success = false;
                            msg = "User Name is required.";
                        }
                        else
                        {

                            this.GetUserInfo(master_obj.USER_INFO);

                            if (master_obj.TRN_EXPENSE_UP != null)
                            {
                                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                                {
                                    try
                                    {
                                        foreach (TRN_EXPENSE_UP trn_expense_up in master_obj.TRN_EXPENSE_UP)
                                        {
                                            if (trn_expense_up.TRN_EXP_DATE != null)
                                            {
                                                TRN_EXPENSE trn_expense = new TRN_EXPENSE();
                                                try
                                                {
                                                    if (trn_expense_up != null)
                                                    {
                                                        trn_expense.USER_NO = trn_expense_up.USER_NO.Value;
                                                        trn_expense.LAST_ACTION_OFFLINE_TIME = trn_expense_up.ACTION_OFFLINE_TIME;
                                                        trn_expense.LAST_ACTION_OFFLINE_SYNC = trn_expense_up.ACTION_OFFLINE_SYNC;
                                                        trn_expense.LAST_IS_OFFLINE = trn_expense_up.ACTION_IS_OFFLINE;
                                                        trn_expense.TRN_EXP_DATE = trn_expense_up.TRN_EXP_DATE.Value;
                                                        trn_expense.IS_MANUAL_ENTRY = 1; // trn_expense_up.IS_MANUAL_ENTRY;
                                                        trn_expense.OFFLINE_EXP_NO = trn_expense_up.OFFLINE_EXP_NO;
                                                        trn_expense.ENTRY_STATE = trn_expense_up.ENTRY_STATE.Value;
                                                    }
                                                    user_calling = trn_expense.USER_NO;

                                                    //decimal EXP_NO = db.TRN_EXPENSE_OFF_SAVE(this.sess_entry_user_no, this.sess_LOGON_NO, trn_expense.LAST_ACTION_OFFLINE_TIME, trn_expense.LAST_ACTION_OFFLINE_SYNC, (decimal) ENTRY_MODE.OFFLINE, this.sess_zm_user_no, this.sess_agent_user_no, trn_expense.TRN_EXP_DATE, trn_expense.IS_MANUAL_ENTRY, trn_expense.OFFLINE_EXP_NO, trn_expense.ENTRY_STATE).First().Value;
                                                    EXP_NO = db.TRN_EXPENSE_OFF_SAVE(trn_expense.USER_NO, this.sess_LOGON_NO, trn_expense.LAST_ACTION_OFFLINE_TIME, trn_expense.LAST_ACTION_OFFLINE_SYNC, (decimal)ENTRY_MODE.OFFLINE, this.sess_zm_user_no, this.sess_agent_user_no, trn_expense.TRN_EXP_DATE, trn_expense.IS_MANUAL_ENTRY, trn_expense.OFFLINE_EXP_NO, trn_expense.ENTRY_STATE).First().Value;
                                                    is_success = EXP_NO != 0;//&& trn_expense_up.TRN_EXPENSE_DET_UP.Count!=0;

                                                    /*if (EXP_NO == 0)
                                                        MobileLogger.logDefault("Master User_no " + user_calling + " Exp no " + EXP_NO + " offline no " + trn_expense.OFFLINE_EXP_NO, "exptest");
                                                    */
                                                    if (trn_expense_up.TRN_EXPENSE_DET_UP != null && EXP_NO > 0)
                                                    {
                                                        foreach (var item in trn_expense_up.TRN_EXPENSE_DET_UP)
                                                        {
                                                            TRN_EXPENSE_DET det_item = new TRN_EXPENSE_DET();
                                                            det_item.EXP_NO = EXP_NO;
                                                            det_item.LAST_ACTION_OFFLINE_TIME = item.ACTION_OFFLINE_TIME;
                                                            det_item.LAST_ACTION_OFFLINE_SYNC = item.ACTION_OFFLINE_SYNC;
                                                            det_item.EXP_TYPE_NO = item.EXP_TYPE_NO.Value;
                                                            det_item.EXP_AMT = item.EXP_AMT.Value;
                                                            det_item.OFFLINE_EXP_NO = item.OFFLINE_EXP_NO;
                                                            det_item.OFFLINE_EXP_DET_NO = item.OFFLINE_EXP_DET_NO;
                                                            det_item.ENTRY_STATE = item.ENTRY_STATE.Value;
                                                            det_item.LAT_VAL = item.LAT_VAL;
                                                            det_item.LON_VAL = item.LON_VAL;
                                                            det_item.VENDOR = item.VENDOR;
                                                            det_item.COMMENTS = item.COMMENTS;

                                                            //db.TRN_EXPENSE_DET_OFF_SAVE(this.sess_entry_user_no, this.sess_LOGON_NO, det_item.LAST_ACTION_OFFLINE_TIME, det_item.LAST_ACTION_OFFLINE_SYNC, (decimal) ENTRY_MODE.OFFLINE, det_item.EXP_NO, det_item.EXP_TYPE_NO, det_item.EXP_AMT, det_item.OFFLINE_EXP_NO, det_item.OFFLINE_EXP_DET_NO, det_item.ENTRY_STATE, det_item.LAT_VAL, det_item.LON_VAL, det_item.VENDOR, det_item.COMMENTS);
                                                            det = db.TRN_EXPENSE_DET_OFF_SAVE(trn_expense.USER_NO, this.sess_LOGON_NO, det_item.LAST_ACTION_OFFLINE_TIME, det_item.LAST_ACTION_OFFLINE_SYNC, (decimal)ENTRY_MODE.OFFLINE, det_item.EXP_NO, det_item.EXP_TYPE_NO, det_item.EXP_AMT, det_item.OFFLINE_EXP_NO, det_item.OFFLINE_EXP_DET_NO, det_item.ENTRY_STATE, det_item.LAT_VAL, det_item.LON_VAL, det_item.VENDOR, det_item.COMMENTS).First().Value;

                                                            /* if (det == 0)
                                                                 MobileLogger.logDefault("DET user_no " + user_calling + " Exp no " + EXP_NO + " entry state " + item.ENTRY_STATE + " amount " + item.EXP_AMT +
                                                             " off_exp_no " + item.OFFLINE_EXP_NO + " off_exp_det_no " + item.OFFLINE_EXP_DET_NO, "exptest");*/

                                                        }
                                                    }
                                                }
                                                catch (Exception exSave)
                                                {
                                                    //scope.Dispose();
                                                    msg = "An Error Occurred during Expense Data, Error Code - 888, Please Try Again."; //ex.Message;
                                                    //if (EXP_NO != 0 || det != 0)
                                                    //{
                                                    is_success = false;
                                                    msg = exSave.Message;
                                                    //}
                                                    DbErrorTracker db_error = new DbErrorTracker();
                                                    db_error.WriteErrorLog(db_error.GetErrorMessage(exSave, true), exSave, sess_entry_user_name);
                                                }

                                            }

                                        }
                                        db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "OTHER EXPENSE");
                                        if (is_success)
                                        {
                                            scope.Complete();
                                            //is_success = (det==0)?false:true;
                                            msg = @"saved successfully";
                                        }
                                        else
                                        {

                                            scope.Dispose();
                                            //is_success = (det==0)?false:true;
                                            //is_success = false;
                                            msg = @"Primary key not found";
                                        }
                                    }
                                    catch (DbException ex)
                                    {
                                        scope.Dispose();
                                        msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                    catch (EntitySqlException ex)
                                    {
                                        scope.Dispose();
                                        msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                    catch (DbUpdateException ex)
                                    {
                                        scope.Dispose();
                                        msg = msg = ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                    catch (Exception ex)
                                    {
                                        scope.Dispose();
                                        msg = ex.Message;
                                        is_success = false;
                                        msg = ex.Message;
                                        DbErrorTracker db_error = new DbErrorTracker();
                                        db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                    }
                                }
                            }
                            else
                            {
                                is_success = true;
                                msg = "Success with null/empty values";
                            }
                        }
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg /*, @trn_expense_up = trn_expense_up */};

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Upload_UserLocation(TRN_USER_LOCATION_MASTER master_obj)
        {

            //this.GetLoggedInInfo();
            using (OGDBEntities db = new OGDBEntities())
            {
                bool is_success = false;
                string msg = @"";

                if (master_obj != null)
                {

                    if (master_obj.USER_INFO == null)
                    {
                        is_success = false;
                        msg = "User Information is required.";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(master_obj.USER_INFO.USER_NAME) || (string.IsNullOrWhiteSpace(master_obj.USER_INFO.USER_NAME)))
                        {
                            is_success = false;
                            msg = "User Name is required.";
                        }
                        else
                        {

                            this.GetUserInfo(master_obj.USER_INFO);

                            if (master_obj.TRN_USER_LOCATION_UP != null)
                            {
                                foreach (var loc in master_obj.TRN_USER_LOCATION_UP)
                                {
                                    if (loc.LOCATION_NAME != null)
                                    {
                                        TRN_USER_LOCATION user_loc = new TRN_USER_LOCATION();
                                        user_loc.USER_NO = loc.USER_NO.Value;
                                        user_loc.LAST_ACTION_OFFLINE_TIME = loc.ACTION_OFFLINE_TIME;
                                        user_loc.LAT_VAL = loc.LAT_VAL.Value;
                                        user_loc.LON_VAL = loc.LON_VAL.Value;
                                        user_loc.LOCATION_NAME = loc.LOCATION_NAME;
                                        user_loc.OFFLINE_LOC_NO = loc.OFFLINE_LOC_NO;
                                        user_loc.ENTRY_STATE = loc.ENTRY_STATE;

                                        //decimal LOC_NO = db.TRN_USER_LOCATION_OFF_SAVE(this.sess_entry_user_no, this.sess_LOGON_NO, user_loc.LAST_ACTION_OFFLINE_TIME, user_loc.LAST_ACTION_OFFLINE_SYNC, (decimal) ENTRY_MODE.OFFLINE, this.sess_zm_user_no, user_loc.LAT_VAL, user_loc.LON_VAL, user_loc.LOCATION_NAME, user_loc.OFFLINE_LOC_NO, user_loc.ENTRY_STATE).First().Value;
                                        try
                                        {
                                            decimal LOC_NO = db.TRN_USER_LOCATION_OFF_SAVE(user_loc.USER_NO, this.sess_LOGON_NO, user_loc.LAST_ACTION_OFFLINE_TIME, user_loc.LAST_ACTION_OFFLINE_SYNC, (decimal)ENTRY_MODE.OFFLINE, this.sess_zm_user_no, user_loc.LAT_VAL, user_loc.LON_VAL, user_loc.LOCATION_NAME, user_loc.OFFLINE_LOC_NO, user_loc.ENTRY_STATE).First().Value;
                                        }
                                        catch (Exception ex)
                                        {
                                            DbErrorTracker db_error = new DbErrorTracker();
                                            db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                        }
                                    }
                                }

                                try
                                {
                                    db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "USER LOCATION");
                                }
                                catch (Exception ex)
                                {
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }

                            }

                            is_success = true;
                            msg = "Record saved successfully.";
                        }
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Upload_Feedback(TRN_USER_FEED_BACK_MASTER master_obj)
        {
            using (OGDBEntities db = new OGDBEntities())
            {
                bool is_success = false;
                string msg = @"";

                if (master_obj != null)
                {
                    this.GetUserInfo(master_obj.USER_INFO);
                    if (master_obj.TRN_USER_FEED_BACK_UP != null)
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                        {
                            try
                            {

                                foreach (TRN_USER_FEED_BACK_UP item in master_obj.TRN_USER_FEED_BACK_UP)
                                {
                                    db.TRN_USER_FEED_BACK_OFF_SAVE(item.USER_NO, this.sess_LOGON_NO, item.ACTION_OFFLINE_TIME, item.ACTION_OFFLINE_SYNC, (decimal)ENTRY_MODE.OFFLINE, item.USER_NO, item.FEEDBACK_TYPE_NO, item.MESSAGE, item.OFFLINE_FEEDBACK_NO, item.ENTRY_STATE);
                                }

                                try
                                {
                                    db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "FEEDBACK");
                                    scope.Complete();
                                    is_success = true;
                                    msg = @"saved successfully";
                                }
                                catch (Exception ex)
                                {
                                    scope.Dispose();
                                }




                            }
                            catch (DbException ex)
                            {
                                scope.Dispose();
                                msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                            catch (EntitySqlException ex)
                            {
                                scope.Dispose();
                                msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                            catch (DbUpdateException ex)
                            {
                                scope.Dispose();
                                msg = msg = ex.Message;
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                msg = ex.Message;
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                        }
                    }
                    else
                    {
                        is_success = true;
                        msg = "Success with null/empty values";
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Upload_Login(TRN_LOG_INFO_BACK_MASTER master_obj)
        {
            //MobileLogger.logDefault("Upload Login Length" + Request.ContentLength.ToString(), "UPLOAD_LOGGER");

            using (OGDBEntities db = new OGDBEntities())
            {
                bool is_success = false;
                string msg = @"";
                if (master_obj != null)
                {
                    //this.GetUserInfo(master_obj.USER_INFO);

                    if (master_obj.TRN_LOG_INFO_BACK_UP != null)
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                        {
                            try
                            {
                                foreach (TRN_LOG_INFO_BACK_UP item in master_obj.TRN_LOG_INFO_BACK_UP)
                                {
                                    if (item.USER_NO != null)
                                        db.TRN_LOG_INFO_SAVE(item.USER_NO, item.INSERT_LOGON_NO, item.LOG_INFO_NO, item.LOGOUT_TYPE_NO, item.USER_NO, item.LOG_IN_LOCATION_NAME, item.LOG_IN_LAT, item.LOG_IN_LONG, item.LOG_IN_TIME, item.LOG_OUT_LOCATION_NAME, item.LOG_OUT_LAT, item.LOG_OUT_LONG, item.LOG_OUT_TIME, item.LOG_OUT_MESSAGE);
                                }

                                try
                                {
                                    //if (this.sess_entry_user_no != null)
                                    //{
                                        //db.TRN_UPLOAD_LOG_INSERT(this.sess_entry_user_no, null, null, Server.MachineName, null, null, "DEVICE LOGIN");
                                        scope.Complete();

                                        is_success = true;
                                        msg = @"saved successfully";
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    scope.Dispose();
                                    msg = "Generic Error : ";
                                    is_success = false;
                                    msg += ex.Message;
                                    DbErrorTracker db_error = new DbErrorTracker();
                                    db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                                }
                            }
                            catch (DbException ex)
                            {
                                scope.Dispose();
                                msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                            catch (EntitySqlException ex)
                            {
                                scope.Dispose();
                                msg = msg = "An Error Occurred during Saving Data, Error Code - 999, Please Try Again."; //ex.Message;
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                            catch (DbUpdateException ex)
                            {
                                scope.Dispose();
                                msg = "Error Occured";
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                msg = "Error Occured";
                                is_success = false;
                                msg = ex.Message;
                                DbErrorTracker db_error = new DbErrorTracker();
                                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                            }
                        }
                    }
                    else
                    {
                        is_success = true;
                        msg = "Success with null/empty values";
                    }
                }
                else
                {
                    is_success = true;
                    msg = "Success with null/empty values";
                }

                var result = new { @is_success = is_success, @msg = msg };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}

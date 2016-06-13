using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using OG_SLN.Models;
using OG_SLN.Controllers.WebApi;

namespace OG_SLN.Controllers {
    public class WS_SEC_USERSController : Controller {

        private OGDBEntities db = new OGDBEntities();
        SEC_USERS_LOGIN_Result1 sess_sec_users = null;
        decimal? sess_USER_NO = null;
        decimal? sess_LOGON_NO = null;
        decimal? sess_entry_user_no = null;
        String sess_entry_user_name = null;
        decimal? sess_zm_user_no = null;
        decimal? sess_agent_user_no = null;

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult Login(SEC_USERS sec_users) {

            //MobileLogger.logDefault( "called", "test");
            //return Request.CreateResponse(HttpStatusCode.OK, sec_users);
            
            bool is_success = false;
            string msg = @"";
            decimal login_msg_type = (decimal) LOGIN_MSG_TYPE.NOTHING;
            string down_link = string.Empty;
            string version_number = string.Empty;
            decimal? USER_NO = null;
            decimal? ENTRY_USER_NO = null;
            decimal? ZM_USER_NO = null;
            decimal? AGENT_USER_NO = null;
            string SESSION_ID = string.Empty;
            DateTime? SERVER_TIME = null;
            decimal? LAST_PK_VAL = null;


            string APP_VERSION = string.Empty;
            try
            {
                var gen_app_version = db.GEN_APP_VERSION.Where(a => a.IS_CURR_VER == 1).FirstOrDefault();
                if (gen_app_version != null)
                {
                    APP_VERSION = gen_app_version.APP_VERSION;
                }

                if (string.IsNullOrEmpty(sec_users.APP_VERSION) || string.IsNullOrWhiteSpace(sec_users.APP_VERSION))
                {
                    msg = "APP VERSION is required";
                    login_msg_type = (decimal)LOGIN_MSG_TYPE.INVALID_APP_VERSION;
                    //var result = new { @SESSION_ID = "", @SERVER_TIME = "", @msg = msg };
                    //return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    if (APP_VERSION.Trim().ToUpper() != sec_users.APP_VERSION.Trim().ToUpper())
                    {
                        msg = @"Application Version Mismatch, Please download the updated version from https://dl.dropboxusercontent.com/u/67899212/Omicon.apk";
                        login_msg_type = (decimal)LOGIN_MSG_TYPE.INVALID_APP_VERSION;
                        down_link = gen_app_version.DOWN_LINK;
                        version_number = gen_app_version.APP_VERSION;
                        //var result = new { @SESSION_ID = "", @SERVER_TIME = "", @msg = msg };
                        //return Json(result, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        var res_obj = db.SEC_USERS_LOGIN(sec_users.USER_NAME, sec_users.USER_PWD, null).FirstOrDefault();

                        if (res_obj != null)
                        {
                            if (res_obj.USER_NO > 0)
                            {
                                //                
                                Session["sess_sec_users"] = res_obj;
                                Session["sess_USER_NO"] = res_obj.USER_NO;

                                USER_NO = res_obj.USER_NO;
                                SERVER_TIME = res_obj.SERVER_TIME;

                                if (res_obj.USER_TYPE_NO == (decimal)UserType.Agents)
                                {
                                    Session["sess_entry_user_no"] = res_obj.USER_NO;
                                    Session["sess_zm_user_no"] = res_obj.USER_PARENT_NO;
                                    Session["sess_agent_user_no"] = res_obj.USER_NO;

                                    ENTRY_USER_NO = res_obj.USER_NO;
                                    ZM_USER_NO = res_obj.USER_PARENT_NO;
                                    AGENT_USER_NO = res_obj.USER_NO;

                                }
                                else
                                {
                                    Session["sess_entry_user_no"] = res_obj.USER_NO;
                                    Session["sess_zm_user_no"] = res_obj.USER_NO;
                                    Session["sess_agent_user_no"] = null;

                                    ENTRY_USER_NO = res_obj.USER_NO;
                                    ZM_USER_NO = res_obj.USER_PARENT_NO;
                                    AGENT_USER_NO = res_obj.USER_NO;

                                }

                                string sess_id = Session.SessionID;
                                string ip_addr = CustomValidator.GetRequestIpAddress();
                                string device_id = CustomValidator.GetDeviceId();
                                string login_mobile = sec_users.USER_MOBILE;
                                string ws_id = CustomValidator.GetWebServerId();
                                string app_version = sec_users.APP_VERSION;

                                SESSION_ID = sess_id;

                                decimal LOGON_NO = db.SEC_USER_LOGONS_INSERT(res_obj.USER_NO, ip_addr, device_id, null, null, null, (decimal)ApproveType.Approved, null, null, sess_id, login_mobile, ws_id, app_version, login_mobile).First().Value;

                                Session["sess_LOGON_NO"] = LOGON_NO;

                                LAST_PK_VAL = db.TRN_OFFLINE_LAST_PK_GET(res_obj.USER_NO).FirstOrDefault();
                                
                                is_success = true;
                                if (sec_users.USER_NAME == "testuser")
                                    MobileLogger.logDefault("found him testuser " +USER_NO+" msg "+is_success.ToString(), "test");
                                //var result = new { @SESSION_ID = sess_id, @SERVER_TIME = res_obj.SERVER_TIME, @msg = "" };
                                //return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                msg = "Invalid Username or password";
                                login_msg_type = (decimal)LOGIN_MSG_TYPE.INVALID_LOGIN;
                                //var result = new { @SESSION_ID = "", @SERVER_TIME = "", @msg = msg };
                                //return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            //MobileLogger.logDefault((res_obj.USER_NO == null) ? "NULL user no in Login LN:143 WS_SEC_USERS Controller" : res_obj.USER_NO.ToString(),"username");

                        }
                        else
                        {
                            msg = "Invalid Username or password";
                            login_msg_type = (decimal)LOGIN_MSG_TYPE.INVALID_LOGIN;
                            //var result = new { @SESSION_ID = "", @SERVER_TIME = "", @msg = msg };
                            //return Json(result, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                if (sec_users.USER_NAME == "testuser" && !is_success)
                    MobileLogger.logDefault(msg, "test");
                var result_object = new
                {
                    @SESSION_ID = SESSION_ID,
                    @SERVER_TIME = SERVER_TIME,
                    @msg = msg,
                    @is_success = is_success,
                    @USER_NO = USER_NO,
                    @ENTRY_USER_NO = ENTRY_USER_NO,
                    @ZM_USER_NO = ZM_USER_NO,
                    @AGENT_USER_NO = AGENT_USER_NO,
                    @login_msg_type = login_msg_type,
                    @down_link = down_link,
                    @version_number = version_number,
                    @LAST_PK_VAL = LAST_PK_VAL,
                };

                return Json(result_object, JsonRequestBehavior.AllowGet);

                //return null;
            }
            catch (Exception ex)
            {
                
                DbErrorTracker db_error = new DbErrorTracker();
                db_error.WriteErrorLog(db_error.GetErrorMessage(ex, true), ex, sess_entry_user_name);
                return null;
            }
        }


        [HttpGet]
        public JsonResult Logout(string USER_NAME) {
            bool is_success = false;
            string msg = "";

            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            try {

                if (LOGON_NO == null) {

                    if (!string.IsNullOrEmpty(USER_NAME) && !string.IsNullOrWhiteSpace(USER_NAME)) {
                        db.SEC_USERS_LOGOUT_APPS(USER_NAME);
                    }

                } else {

                    db.SEC_USERS_LOGOUT(LOGON_NO);

                    Session.Clear();
                    //Session.RemoveAll();
                    Session.Abandon();

                    is_success = true;
                    msg = "Logout successfully.";


                }
            } catch (Exception ex) {

            }


            var result = new { @is_success = is_success, @msg = msg };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ChangePassword(SEC_USER_CHANGE_PASSWORD user_info) {
            bool is_success = false;
            string msg = "";
            decimal ret_val = 0;
            try {
                ret_val = db.SEC_USER_CHANGE_PASS(user_info.USER_NAME, user_info.OLD_PASSWORD, user_info.NEW_PASSWORD).FirstOrDefault().Value;
            } catch (Exception ex) {
                is_success = false;
                msg = "Failed to Change Password.";
            }

            if (ret_val > 0) {
                is_success = true;
                msg = "Password Changed successfully.";
            } else {
                is_success = false;
                msg = "Failed to Change Password.";
            }

            var result = new { @is_success = is_success, @msg = msg };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckVersion(string APP_VERSION)
        {
            bool is_success = false;
            string msg = @"";
            //MobileLogger.logDefault(APP_VERSION, "check_version"); 
            string CURR_VERSION = string.Empty;
            //MobileLogger.logDefault(Session["sess_USER_NO"].ToString(),"loginDetails");
            decimal login_msg_type = (decimal)LOGIN_MSG_TYPE.NOTHING;
            string down_link = string.Empty;
            string version_number = string.Empty;
            /*if (decimal.Parse(Session["sess_USER_NO"].ToString()) != 182 && decimal.Parse(Session["sess_USER_NO"].ToString())!=691)
            {*/
                var gen_app_version = db.GEN_APP_VERSION.Where(a => a.IS_CURR_VER == 1).FirstOrDefault();
                if (gen_app_version != null)
                {
                    CURR_VERSION = gen_app_version.APP_VERSION;
                }

                if (CURR_VERSION.Trim().ToUpper() != APP_VERSION.Trim().ToUpper())
                {
                    
                    is_success = false;
                    msg = @"Version updated, Please download the updated version";
                    //MobileLogger.logDefault(msg, "check_version"); 
                    login_msg_type = (decimal)LOGIN_MSG_TYPE.INVALID_APP_VERSION;
                    down_link = gen_app_version.DOWN_LINK;
                    version_number = gen_app_version.APP_VERSION;
                }
                else
                {
                    is_success = true;
                    msg = @"";
                }
            /*}
           
            else
            {
                MobileLogger.logDefault("found user 182 for login,changing version", "182-691-user");
                is_success = false;
                msg = @"Application Version Mismatch, Please download the updated version from http://202.4.119.42:6061/Omicon_new.apk";
                login_msg_type = (decimal)LOGIN_MSG_TYPE.INVALID_APP_VERSION;
                down_link = "http://202.4.119.42:6061/Omicon_new.apk";
                version_number = "3.5";
            }*/

            var result_object = new
            {
                @msg = msg,
                @is_success = is_success,
                @login_msg_type = login_msg_type,
                @down_link = down_link,
                @version_number = version_number,
            };

            return Json(result_object, JsonRequestBehavior.AllowGet);

        }
        
        }
}

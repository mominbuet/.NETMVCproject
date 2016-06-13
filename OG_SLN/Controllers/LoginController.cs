using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

using OG_SLN.Models;
using System.Data.Objects;

namespace OG_SLN.Controllers
{
    public class LoginController : Controller
    {

        private OGDBEntities db = new OGDBEntities();

        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SEC_USERS sec_user)
        {
            SEC_USERS_LOGIN_Result1 user = db.SEC_USERS_LOGIN(sec_user.USER_NAME, sec_user.USER_PWD, null).FirstOrDefault();
            if (user != null && user.USER_NO > 0)
            {
                Session["sess_sec_users"] = user;
                Session["sess_USER_NO"] = user.USER_NO;

                if (user.USER_TYPE_NO == (decimal)EUserTypes.Agent)
                {
                    Session["sess_entry_user_no"] = user.USER_NO;
                    Session["sess_zm_user_no"] = user.USER_PARENT_NO;
                    Session["sess_agent_user_no"] = user.USER_NO;
                }
                else
                {
                    Session["sess_entry_user_no"] = user.USER_NO;
                    Session["sess_zm_user_no"] = user.USER_NO;
                    Session["sess_agent_user_no"] = null;
                }


                bool Is_ZonalOrAgent = ((decimal)user.USER_TYPE_NO == (decimal)EUserTypes.ZonalManager
                                    || (decimal)user.USER_TYPE_NO == (decimal)EUserTypes.Agent);
                Session["sess_Is_ZonalOrAgent"] = Is_ZonalOrAgent;

                

                string sess_id = Session.SessionID;
                string ip_addr = CustomValidator.GetRequestIpAddress();
                string device_id = CustomValidator.GetDeviceId();
                string login_mobile = sec_user.USER_MOBILE;
                string ws_id = CustomValidator.GetWebServerId();
                string app_version = sec_user.APP_VERSION;

                decimal LOGON_NO = db.SEC_USER_LOGONS_INSERT(user.USER_NO, ip_addr, device_id, null, null, null, 
                                                        (decimal)ApproveType.Approved, null, null, sess_id, 
                                                        login_mobile, ws_id, app_version, login_mobile).First().Value;


                Session["sess_LOGON_NO"] = LOGON_NO;

                if (user.USER_TYPE_NO == (decimal)EUserTypes.GeneralUser)
                {
                    List<SET_USER_ACTION> per_list = new List<SET_USER_ACTION>();

                    per_list = db.SET_USER_ACTION.Include(a => a.GEN_CONTROLLER_ACTION)
                                .Where(a => (a.USER_NO == user.USER_NO) && (a.IS_ACTIVE == 1))
                                .ToList();
                    List<GEN_CONTROLLER_ACTION> menu_list = (from c in per_list
                                                             where (c.GEN_CONTROLLER_ACTION.IS_MENU == 1  
                                                                && c.GEN_CONTROLLER_ACTION.IS_ACTIVE == 1 
                                                             )
                                                             orderby c.GEN_CONTROLLER_ACTION.SL_NUM
                                                             select c.GEN_CONTROLLER_ACTION).ToList();

                    Session["sess_MENU_LIST"] = menu_list;

                    Session["sess_PERMISSION_LIST"] = per_list;
                    if (menu_list != null)
                    {
                        GEN_CONTROLLER_ACTION redirect_action = menu_list.First();

                        return RedirectToAction(redirect_action.ACTION_NAME, redirect_action.CONTROLLER_NAME);
                    }
                }
                else
                {
                    SET_ROLE role = db.SET_ROLE.Where(r => r.USER_TYPE_NO == user.USER_TYPE_NO).FirstOrDefault();

                    decimal? ROLE_NO = null;

                    if (role != null)
                    {
                        ROLE_NO = role.ROLE_NO;
                        Session["ROLE_NO"] = role.ROLE_NO;
                    }

                    //ROLE_NO = decimal.Parse(Session["ROLE_NO"].ToString());

                    List<SET_ROLE_ACTION> per_list = new List<SET_ROLE_ACTION>();

                    if (ROLE_NO.HasValue)
                    {
                        per_list = db.SET_ROLE_ACTION.Include(a => a.GEN_CONTROLLER_ACTION)
                                .Where(a => (a.ROLE_NO == ROLE_NO.Value) && (a.IS_ACTIVE == 1))
                                .ToList();
                        List<GEN_CONTROLLER_ACTION> menu_list = (from c in per_list
                                                                 where (c.GEN_CONTROLLER_ACTION.IS_MENU == 1
                                                                    && c.GEN_CONTROLLER_ACTION.IS_ACTIVE == 1
                                                                 )
                                                                 orderby c.GEN_CONTROLLER_ACTION.SL_NUM
                                                                 select c.GEN_CONTROLLER_ACTION).ToList();

                        Session["sess_MENU_LIST"] = menu_list;

                        Session["sess_PERMISSION_LIST"] = per_list;
                        if (menu_list != null)
                        {
                            GEN_CONTROLLER_ACTION redirect_action = menu_list.First();

                            return RedirectToAction(redirect_action.ACTION_NAME, redirect_action.CONTROLLER_NAME);
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            decimal? USER_NO = Session["sess_USER_NO"] as decimal?;
            decimal? LOGON_NO = Session["sess_LOGON_NO"] as decimal?;

            SEC_USERS_LOGIN_Result1 user = Session["sess_sec_users"] as SEC_USERS_LOGIN_Result1;

            if (user.USER_NO > 0 && model.OLD_PASS == user.USER_PWD && model.NEW_PASS == model.RE_NEW_PASS)
            {
                /*
                db.SEC_USERS_UPDATE(user.USER_MOBILE, user.USER_ADDR, user.USER_EMAIL, user.USER_NAME,
                    model.NEW_PASS, user.USER_PARENT_NO, user.IS_ACTIVE, user.ACTIVE_FROM, user.ACTIVE_TO,
                    user.USER_NO, USER_NO, LOGON_NO, user.USER_TYPE_NO, user.DEPT_NO, user.DESIG_NO,
                    user.HR_EMP_ID, user.USER_FULL_NAME, user.USER_NICK_NAME, user.USER_CONTACT, 
                    user.COMP_NO, user.THANA_NO);
                */
                db.SEC_USERS_CHANGE_PASSWORD(user.USER_NO, model.NEW_PASS, USER_NO, LOGON_NO);
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return View("Login");
        }

        public ActionResult Home()
        {
            decimal USER_NO = decimal.Parse(Session["sess_USER_NO"].ToString());

            bool Is_ZonalOrAgent = (bool) Session["sess_Is_ZonalOrAgent"];

            ViewBag.Is_ZonalOrAgent = Is_ZonalOrAgent;

            decimal? dcr_approve_count = db.TRN_DCR_APPROVAL_COUNT(USER_NO).FirstOrDefault();
            ViewBag.dcr_approve_count = dcr_approve_count;

            decimal? dcr_offline_count = db.TRN_DCR_OFF_VERIFY_COUNT(USER_NO).FirstOrDefault();
            ViewBag.dcr_offline_count = dcr_offline_count;

            //decimal? expense_approve_count = db.TRN_EXPENSE_APPROVAL_COUNT(USER_NO).FirstOrDefault();
            //ViewBag.expense_approve_count = expense_approve_count;

            decimal? expense_offline_count = db.TRN_EXPENSE_VERIFY_COUNT(USER_NO).FirstOrDefault().CNT;
            ViewBag.expense_offline_count = expense_offline_count;

            return View();
        }

        [HttpPost]
        public JsonResult PendingJob()
        {
            decimal USER_NO = decimal.Parse(Session["sess_USER_NO"].ToString());

            bool Is_ZonalOrAgent = (bool) Session["sess_Is_ZonalOrAgent"];

            decimal? dcr_approve_count = db.TRN_DCR_APPROVAL_COUNT(USER_NO).FirstOrDefault();
            ViewBag.dcr_approve_count = dcr_approve_count;

            decimal? dcr_offline_count = db.TRN_DCR_OFF_VERIFY_COUNT(USER_NO).FirstOrDefault();
            ViewBag.dcr_offline_count = dcr_offline_count;

            //decimal? expense_approve_count = db.TRN_EXPENSE_APPROVAL_COUNT(USER_NO).FirstOrDefault();
            //ViewBag.expense_approve_count = expense_approve_count;

            decimal? expense_offline_count = db.TRN_EXPENSE_VERIFY_COUNT(USER_NO).FirstOrDefault().CNT;
            ViewBag.expense_offline_count = expense_offline_count;

            return Json(new { 
                dcr_approve = dcr_approve_count, 
                dcr_offline = dcr_offline_count,
                //expense_approve = expense_approve_count,
                expense_offline = expense_offline_count,
                is_zonal_or_agent = Is_ZonalOrAgent
            });
        }
    }
}

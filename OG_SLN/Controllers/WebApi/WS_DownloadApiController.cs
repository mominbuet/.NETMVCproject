using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using OG_SLN.Controllers.WebApi;
using System.Configuration;


namespace OG_SLN.Controllers
{
    public class WS_DownloadApiController : Controller
    {

        DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private OGDBEntities db = new OGDBEntities();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private string GetTime(DateTime date_val)
        {
            return (date_val - Jan1st1970).TotalMilliseconds.ToString();
        }


        [HttpGet]
        public JsonResult DownloadData(string DEVICE_ID, string IS_ALL)
        {

            //var MAX_LAST_DOWN_TIME = db.DOWN_MAX_GET(DEVICE_ID).First().Value;

            //List<DOWN_SEC_USERS_OBJ> DOWN_SEC_USERS_OBJ_List = new List<DOWN_SEC_USERS_OBJ>();
            //MobileLogger.logDefault("DEVICE_ID " + DEVICE_ID, "Download Data");
            DateTime MAX_LAST_DOWN_TIME;

            if (string.IsNullOrEmpty(IS_ALL) || string.IsNullOrWhiteSpace(IS_ALL) || (IS_ALL == "0") || (IS_ALL.ToUpper() == "FALSE"))
            {
                MAX_LAST_DOWN_TIME = db.DOWN_MAX_GET(DEVICE_ID).First().Value;
            }
            else
            {
                //MAX_LAST_DOWN_TIME = db.DOWN_MAX_GET(DEVICE_ID).First().Value;
                MAX_LAST_DOWN_TIME = DateTime.Today.AddDays(-5000);
            }

            List<object> DOWN_SET_LOGOUT_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_LOGOUT_TYPE(MAX_LAST_DOWN_TIME, string.Empty))
            {
                DOWN_SET_LOGOUT_TYPE_OBJ obj = new DOWN_SET_LOGOUT_TYPE_OBJ();
                obj.LOGOUT_TYPE_NO = (item.LOGOUT_TYPE_NO.HasValue ? item.LOGOUT_TYPE_NO.Value.ToString() : string.Empty);
                obj.LOGOUT_TYPE_NAME = item.LOGOUT_TYPE_NAME;

                DOWN_SET_LOGOUT_TYPE_OBJ_List.Add(new
                {
                    LOGOUT_TYPE_NO = obj.LOGOUT_TYPE_NO,
                    LOGOUT_TYPE_NAME = obj.LOGOUT_TYPE_NAME
                });
            }

            List<object> DOWN_SET_FEEDBACK_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_FEEDBACK_TYPE(MAX_LAST_DOWN_TIME, string.Empty))
            {
                DOWN_SET_FEEDBACK_TYPE_OBJ obj = new DOWN_SET_FEEDBACK_TYPE_OBJ();
                obj.FEEDBACK_TYPE_NO = (item.FEEDBACK_TYPE_NO.HasValue ? item.FEEDBACK_TYPE_NO.Value.ToString() : string.Empty);
                obj.FEEDBACK_NAME = item.FEEDBACK_NAME;
                obj.FEEDBACK_CODE = item.FEEDBACK_CODE;
                obj.FEEDBACK_DESC = item.FEEDBACK_DESC;
                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_FEEDBACK_TYPE_OBJ_List.Add(new
                {
                    FEEDBACK_TYPE_NO = obj.FEEDBACK_TYPE_NO,
                    FEEDBACK_NAME = obj.FEEDBACK_NAME,
                    FEEDBACK_CODE = obj.FEEDBACK_CODE,
                    FEEDBACK_DESC = obj.FEEDBACK_DESC,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO
                });
            }

            List<object> DOWN_SEC_USERS_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SEC_USERS(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SEC_USERS_OBJ obj = new DOWN_SEC_USERS_OBJ();
                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);
                obj.USER_TYPE_NO = (item.USER_TYPE_NO.HasValue ? item.USER_TYPE_NO.Value.ToString() : string.Empty);
                obj.HR_EMP_ID = item.HR_EMP_ID;
                obj.USER_FULL_NAME = item.USER_FULL_NAME;
                obj.USER_MOBILE = item.USER_MOBILE;

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SEC_USERS_OBJ_List.Add(new
                {
                    USER_NO = obj.USER_NO,
                    USER_TYPE_NO = obj.USER_TYPE_NO,
                    HR_EMP_ID = obj.HR_EMP_ID,
                    USER_FULL_NAME = obj.USER_FULL_NAME,
                    USER_MOBILE = obj.USER_MOBILE,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SEC_USER_THANA_OBJ> DOWN_SEC_USER_THANA_OBJ_List = new List<DOWN_SEC_USER_THANA_OBJ>();
            List<object> DOWN_SEC_USER_THANA_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SEC_USER_THANA(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SEC_USER_THANA_OBJ obj = new DOWN_SEC_USER_THANA_OBJ();
                obj.USER_THANA_NO = (item.USER_THANA_NO.HasValue ? item.USER_THANA_NO.Value.ToString() : string.Empty);
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);
                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);
                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SEC_USER_THANA_OBJ_List.Add(new
                {
                    USER_THANA_NO = obj.USER_THANA_NO,
                    THANA_NO = obj.THANA_NO,
                    USER_NO = obj.USER_NO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_CLASS_OBJ> DOWN_SET_CLASS_OBJ_List = new List<DOWN_SET_CLASS_OBJ>();
            List<object> DOWN_SET_CLASS_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_CLASS(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_CLASS_OBJ obj = new DOWN_SET_CLASS_OBJ();
                obj.CLASS_NO = (item.CLASS_NO.HasValue ? item.CLASS_NO.Value.ToString() : string.Empty);

                obj.CLASS_NAME = item.CLASS_NAME;
                obj.CLASS_NAME_BNG = item.CLASS_NAME_BNG;
                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_CLASS_OBJ_List.Add(new
                {
                    CLASS_NO = obj.CLASS_NO,
                    CLASS_NAME = obj.CLASS_NAME,
                    CLASS_NAME_BNG = obj.CLASS_NAME_BNG,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_CLIENT_INFO_OBJ> DOWN_SET_CLIENT_INFO_OBJ_List = new List<DOWN_SET_CLIENT_INFO_OBJ>();
            List<object> DOWN_SET_CLIENT_INFO_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_CLIENT_INFO(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_CLIENT_INFO_OBJ obj = new DOWN_SET_CLIENT_INFO_OBJ();
                obj.CLIENT_NO = (item.CLIENT_NO.HasValue ? item.CLIENT_NO.Value.ToString() : string.Empty);

                obj.CLIENT_NAME = item.CLIENT_NAME;
                obj.CLIENT_NAME_BNG = item.CLIENT_NAME_BNG;
                obj.CLIENT_NICK_NAME = item.CLIENT_NICK_NAME;
                obj.CLIENT_MOBILE = item.CLIENT_MOBILE;

                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);
                obj.ZONE_NO = (item.ZONE_NO.HasValue ? item.ZONE_NO.Value.ToString() : string.Empty);
                obj.ZILLA_NO = (item.ZILLA_NO.HasValue ? item.ZILLA_NO.Value.ToString() : string.Empty);
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_CLIENT_INFO_OBJ_List.Add(new
                {
                    CLIENT_NO = obj.CLIENT_NO,
                    CLIENT_NAME = obj.CLIENT_NAME,
                    CLIENT_NAME_BNG = obj.CLIENT_NAME_BNG,
                    CLIENT_NICK_NAME = obj.CLIENT_NICK_NAME,
                    CLIENT_MOBILE = obj.CLIENT_MOBILE,
                    obj.DIVISION_NO,
                    ZONE_NO = obj.ZONE_NO,
                    ZILLA_NO = obj.ZILLA_NO,
                    THANA_NO = obj.THANA_NO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_DIVISION_OBJ> DOWN_SET_DIVISION_OBJ_List = new List<DOWN_SET_DIVISION_OBJ>();
            List<object> DOWN_SET_DIVISION_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_DIVISION(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_DIVISION_OBJ obj = new DOWN_SET_DIVISION_OBJ();
                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);

                obj.DIVISION_NAME = item.DIVISION_NAME;
                obj.DIVISION_NAME_BNG = item.DIVISION_NAME_BNG;
                obj.DIVISION_CODE = item.DIVISION_CODE;

                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_DIVISION_OBJ_List.Add(new
                {
                    DIVISION_NO = obj.DIVISION_NO,
                    DIVISION_NAME = obj.DIVISION_NAME,
                    DIVISION_NAME_BNG = obj.DIVISION_NAME_BNG,
                    DIVISION_CODE = obj.DIVISION_CODE,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_EXP_TYPE_OBJ> DOWN_SET_EXP_TYPE_OBJ_List = new List<DOWN_SET_EXP_TYPE_OBJ>();
            List<object> DOWN_SET_EXP_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_EXP_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_EXP_TYPE_OBJ obj = new DOWN_SET_EXP_TYPE_OBJ();
                obj.EXP_TYPE_NO = (item.EXP_TYPE_NO.HasValue ? item.EXP_TYPE_NO.Value.ToString() : string.Empty);

                obj.EXP_TYPE_CODE = item.EXP_TYPE_CODE;
                obj.EXP_TYPE_NAME = item.EXP_TYPE_NAME;
                obj.EXP_TYPE_DETAILS = item.EXP_TYPE_DETAILS;
                obj.EXP_TYPE_DESC = item.EXP_TYPE_DESC;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_EXP_TYPE_OBJ_List.Add(new
                {
                    EXP_TYPE_NO = obj.EXP_TYPE_NO,
                    EXP_TYPE_NAME = obj.EXP_TYPE_NAME,
                    EXP_TYPE_CODE = obj.EXP_TYPE_CODE,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_INSTITUTE_OBJ> DOWN_SET_INSTITUTE_OBJ_List = new List<DOWN_SET_INSTITUTE_OBJ>();
            List<object> DOWN_SET_INSTITUTE_OBJ_List = new List<object>();


            foreach (var item in db.DOWN_GET_SET_INSTITUTE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_INSTITUTE_OBJ obj = new DOWN_SET_INSTITUTE_OBJ();
                obj.INSTITUTE_NO = (item.INSTITUTE_NO.HasValue ? item.INSTITUTE_NO.Value.ToString() : string.Empty);

                obj.INSTITUTE_NAME = item.INSTITUTE_NAME;
                obj.INSTITUTE_NAME_BNG = item.INSTITUTE_NAME_BNG;
                obj.INST_TYPE_NO = (item.INST_TYPE_NO.HasValue ? item.INST_TYPE_NO.Value.ToString() : string.Empty);
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);
                obj.F_INSTITUTION_DB_ID = item.F_INSTITUTION_DB_ID;
                obj.EIIN_NUMBER = item.EIIN_NUMBER;
                obj.MEET_PERSON_NAME = item.MEET_PERSON_NAME;
                obj.CONTACT_NUMBER = item.CONTACT_NUMBER;

                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_INSTITUTE_OBJ_List.Add(new
                {
                    INSTITUTE_NO = obj.INSTITUTE_NO,
                    INSTITUTE_NAME = obj.INSTITUTE_NAME,
                    INSTITUTE_NAME_BNG = obj.INSTITUTE_NAME_BNG,
                    INST_TYPE_NO = obj.INST_TYPE_NO,
                    THANA_NO = obj.THANA_NO,
                    F_INSTITUTION_DB_ID = obj.F_INSTITUTION_DB_ID,
                    EIIN_NUMBER = obj.EIIN_NUMBER,
                    MEET_PERSON_NAME = obj.MEET_PERSON_NAME,
                    CONTACT_NUMBER = obj.CONTACT_NUMBER,
                    ACTUAL_LATI_VAL = obj.ACTUAL_LATI_VAL,
                    ACTUAL_LONG_VAL = obj.ACTUAL_LONG_VAL,
                    FROM_LATI_VAL = obj.FROM_LATI_VAL,
                    FROM_LONG_VAL = obj.FROM_LONG_VAL,
                    TO_LATI_VAL = obj.TO_LATI_VAL,
                    TO_LONG_VAL = obj.TO_LONG_VAL,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_INST_TYPE_OBJ> DOWN_SET_INST_TYPE_OBJ_List = new List<DOWN_SET_INST_TYPE_OBJ>();
            List<object> DOWN_SET_INST_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_INST_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_INST_TYPE_OBJ obj = new DOWN_SET_INST_TYPE_OBJ();
                obj.INST_TYPE_NO = (item.INST_TYPE_NO.HasValue ? item.INST_TYPE_NO.Value.ToString() : string.Empty);

                obj.INST_TYPE_NAME = item.INST_TYPE_NAME;
                obj.INST_TYPE_CODE = item.INST_TYPE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_INST_TYPE_OBJ_List.Add(new
                {
                    INST_TYPE_NO = obj.INST_TYPE_NO,
                    INST_TYPE_NAME = obj.INST_TYPE_NAME,
                    INST_TYPE_CODE = obj.INST_TYPE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_ORG_TYPE_OBJ> DOWN_SET_ORG_TYPE_OBJ_List = new List<DOWN_SET_ORG_TYPE_OBJ>();
            List<object> DOWN_SET_ORG_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_ORG_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_ORG_TYPE_OBJ obj = new DOWN_SET_ORG_TYPE_OBJ();
                obj.ORG_TYPE_NO = (item.ORG_TYPE_NO.HasValue ? item.ORG_TYPE_NO.Value.ToString() : string.Empty);

                obj.ORG_TYPE_NAME = item.ORG_TYPE_NAME;
                obj.ORG_TYPE_CODE = item.ORG_TYPE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_ORG_TYPE_OBJ_List.Add(new
                {
                    ORG_TYPE_NO = obj.ORG_TYPE_NO,
                    ORG_TYPE_NAME = obj.ORG_TYPE_NAME,
                    ORG_TYPE_CODE = obj.ORG_TYPE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_PROMO_ITEM_OBJ> DOWN_SET_PROMO_ITEM_OBJ_List = new List<DOWN_SET_PROMO_ITEM_OBJ>();
            List<object> DOWN_SET_PROMO_ITEM_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_PROMO_ITEM(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_PROMO_ITEM_OBJ obj = new DOWN_SET_PROMO_ITEM_OBJ();
                obj.PROMO_ITEM_NO = (item.PROMO_ITEM_NO.HasValue ? item.PROMO_ITEM_NO.Value.ToString() : string.Empty);

                obj.PROMO_ITEM_NAME = item.PROMO_ITEM_NAME;
                obj.PROMO_ITEM_CODE = item.PROMO_ITEM_CODE;

                obj.PROMO_ITEM_TYPE_NO = (item.PROMO_ITEM_TYPE_NO.HasValue ? item.PROMO_ITEM_TYPE_NO.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_PROMO_ITEM_OBJ_List.Add(new
                {
                    PROMO_ITEM_NO = obj.PROMO_ITEM_NO,
                    PROMO_ITEM_NAME = obj.PROMO_ITEM_NAME,
                    PROMO_ITEM_CODE = obj.PROMO_ITEM_CODE,
                    PROMO_ITEM_TYPE_NO = obj.PROMO_ITEM_TYPE_NO,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_SPECIMEN_OBJ> DOWN_SET_SPECIMEN_OBJ_List = new List<DOWN_SET_SPECIMEN_OBJ>();
            List<object> DOWN_SET_SPECIMEN_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_SPECIMEN(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_SPECIMEN_OBJ obj = new DOWN_SET_SPECIMEN_OBJ();
                obj.SPECIMEN_NO = (item.SPECIMEN_NO.HasValue ? item.SPECIMEN_NO.Value.ToString() : string.Empty);

                obj.SPECIMEN_NAME = item.SPECIMEN_NAME;
                obj.SPECIMEN_NAME_BNG = item.SPECIMEN_NAME_BNG;
                obj.SPECIMEN_CODE = item.SPECIMEN_CODE;

                obj.BOOK_TYPE_NO = (item.BOOK_TYPE_NO.HasValue ? item.BOOK_TYPE_NO.Value.ToString() : string.Empty);
                obj.BOOK_UNIQUE_CODE = item.BOOK_UNIQUE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_SPECIMEN_OBJ_List.Add(new
                {
                    SPECIMEN_NO = obj.SPECIMEN_NO,
                    SPECIMEN_NAME = obj.SPECIMEN_NAME,
                    SPECIMEN_NAME_BNG = obj.SPECIMEN_NAME_BNG,
                    SPECIMEN_CODE = obj.SPECIMEN_CODE,
                    BOOK_TYPE_NO = obj.BOOK_TYPE_NO,
                    BOOK_UNIQUE_CODE = obj.BOOK_UNIQUE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
               // MobileLogger.logDefault("Device id " + DEVICE_ID + " SPECIMEN_NO " + obj.SPECIMEN_NO + " SPECIMEN_NAME "
                //    + obj.SPECIMEN_NAME, "SpecimenDownload");
            }
            //MobileLogger.logDefault("Device id " + DEVICE_ID + " SPECIMEN_SIZE " + DOWN_SET_SPECIMEN_OBJ_List.Count, "SpecimenDownload");
            //List<DOWN_SET_SUBJECT_OBJ> DOWN_SET_SUBJECT_OBJ_List = new List<DOWN_SET_SUBJECT_OBJ>();
            List<object> DOWN_SET_SUBJECT_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_SUBJECT(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_SUBJECT_OBJ obj = new DOWN_SET_SUBJECT_OBJ();
                obj.SUBJECT_NO = (item.SUBJECT_NO.HasValue ? item.SUBJECT_NO.Value.ToString() : string.Empty);

                obj.SUBJECT_NAME = item.SUBJECT_NAME;
                obj.SUBJECT_NAME_BNG = item.SUBJECT_NAME_BNG;
                obj.SUBJECT_CODE = item.SUBJECT_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_SUBJECT_OBJ_List.Add(new
                {
                    SUBJECT_NO = obj.SUBJECT_NO,
                    SUBJECT_NAME = obj.SUBJECT_NAME,
                    SUBJECT_NAME_BNG = obj.SUBJECT_NAME_BNG,
                    SUBJECT_CODE = obj.SUBJECT_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_TEACHER_DESIG_OBJ> DOWN_SET_TEACHER_DESIG_OBJ_List = new List<DOWN_SET_TEACHER_DESIG_OBJ>();
            List<object> DOWN_SET_TEACHER_DESIG_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_TEACHER_DESIG(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_TEACHER_DESIG_OBJ obj = new DOWN_SET_TEACHER_DESIG_OBJ();
                obj.TEACH_DESIG_NO = (item.TEACH_DESIG_NO.HasValue ? item.TEACH_DESIG_NO.Value.ToString() : string.Empty);

                obj.TEACHER_DESIG_NAME = item.TEACHER_DESIG_NAME;
                obj.TEACHER_DESIG_NAME_BNG = item.TEACHER_DESIG_NAME_BNG;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_TEACHER_DESIG_OBJ_List.Add(new
                {
                    TEACH_DESIG_NO = obj.TEACH_DESIG_NO,
                    TEACHER_DESIG_NAME = obj.TEACHER_DESIG_NAME,
                    TEACHER_DESIG_NAME_BNG = obj.TEACHER_DESIG_NAME_BNG,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_TEACHER_INFO_OBJ> DOWN_SET_TEACHER_INFO_OBJ_List = new List<DOWN_SET_TEACHER_INFO_OBJ>();
            List<object> DOWN_SET_TEACHER_INFO_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_TEACHER_INFO(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_TEACHER_INFO_OBJ obj = new DOWN_SET_TEACHER_INFO_OBJ();
                obj.TEACHER_NO = (item.TEACHER_NO.HasValue ? item.TEACHER_NO.Value.ToString() : string.Empty);

                obj.TEACHER_NAME = item.TEACHER_NAME;
                obj.TEACHER_NAME_BNG = item.TEACHER_NAME_BNG;
                obj.TEACHER_NICK_NAME = item.TEACHER_NICK_NAME;
                obj.TEACHER_MOBILE = item.TEACHER_MOBILE;
                obj.INSTITUTE_NO = (item.INSTITUTE_NO.HasValue ? item.INSTITUTE_NO.Value.ToString() : string.Empty);
                obj.TEACH_DESIG_NO = (item.TEACH_DESIG_NO.HasValue ? item.TEACH_DESIG_NO.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_TEACHER_INFO_OBJ_List.Add(new
                {
                    TEACHER_NO = obj.TEACHER_NO,
                    TEACHER_NAME = obj.TEACHER_NAME,
                    TEACHER_NAME_BNG = obj.TEACHER_NAME_BNG,
                    TEACHER_NICK_NAME = obj.TEACHER_NICK_NAME,
                    TEACHER_MOBILE = obj.TEACHER_MOBILE,
                    INSTITUTE_NO = obj.INSTITUTE_NO,
                    TEACH_DESIG_NO = obj.TEACH_DESIG_NO,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_THANA_OBJ> DOWN_SET_THANA_OBJ_List = new List<DOWN_SET_THANA_OBJ>();
            List<object> DOWN_SET_THANA_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_THANA(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_THANA_OBJ obj = new DOWN_SET_THANA_OBJ();
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);
                obj.ZONE_NO = (item.ZONE_NO.HasValue ? item.ZONE_NO.Value.ToString() : string.Empty);
                obj.ZILLA_NO = (item.ZILLA_NO.HasValue ? item.ZILLA_NO.Value.ToString() : string.Empty);

                obj.THANA_NAME = item.THANA_NAME;
                obj.THANA_NAME_BNG = item.THANA_NAME_BNG;
                obj.THANA_CODE = item.THANA_CODE;


                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_THANA_OBJ_List.Add(new
                {
                    THANA_NO = obj.THANA_NO,
                    ZONE_NO = obj.ZONE_NO,
                    ZILLA_NO = obj.ZILLA_NO,
                    THANA_NAME = obj.THANA_NAME,
                    THANA_NAME_BNG = obj.THANA_NAME_BNG,
                    THANA_CODE = obj.THANA_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_TRANSPORT_TYPE_OBJ> DOWN_SET_TRANSPORT_TYPE_OBJ_List = new List<DOWN_SET_TRANSPORT_TYPE_OBJ>();
            List<object> DOWN_SET_TRANSPORT_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_TRANSPORT_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_TRANSPORT_TYPE_OBJ obj = new DOWN_SET_TRANSPORT_TYPE_OBJ();
                obj.TRANS_TYPE_NO = (item.TRANS_TYPE_NO.HasValue ? item.TRANS_TYPE_NO.Value.ToString() : string.Empty);

                obj.TRANS_TYPE_NAME = item.TRANS_TYPE_NAME;

                obj.TRANS_TYPE_CODE = item.TRANS_TYPE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_TRANSPORT_TYPE_OBJ_List.Add(new
                {
                    TRANS_TYPE_NO = obj.TRANS_TYPE_NO,
                    TRANS_TYPE_NAME = obj.TRANS_TYPE_NAME,
                    TRANS_TYPE_CODE = obj.TRANS_TYPE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_WORK_PURPOSE_OBJ> DOWN_SET_WORK_PURPOSE_OBJ_List = new List<DOWN_SET_WORK_PURPOSE_OBJ>();
            List<object> DOWN_SET_WORK_PURPOSE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_WORK_PURPOSE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_WORK_PURPOSE_OBJ obj = new DOWN_SET_WORK_PURPOSE_OBJ();
                obj.WORK_PUR_NO = (item.WORK_PUR_NO.HasValue ? item.WORK_PUR_NO.Value.ToString() : string.Empty);

                obj.WORK_PUR_NAME = item.WORK_PUR_NAME;
                obj.WORK_PUR_CODE = item.WORK_PUR_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_WORK_PURPOSE_OBJ_List.Add(new
                {
                    WORK_PUR_NO = obj.WORK_PUR_NO,
                    WORK_PUR_NAME = obj.WORK_PUR_NAME,
                    WORK_PUR_CODE = obj.WORK_PUR_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_ZILLA_OBJ> DOWN_SET_ZILLA_OBJ_List = new List<DOWN_SET_ZILLA_OBJ>();
            List<object> DOWN_SET_ZILLA_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_ZILLA(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_ZILLA_OBJ obj = new DOWN_SET_ZILLA_OBJ();
                obj.ZILLA_NO = (item.ZILLA_NO.HasValue ? item.ZILLA_NO.Value.ToString() : string.Empty);

                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);

                obj.ZILLA_NAME = item.ZILLA_NAME;
                obj.ZILLA_NAME_BNG = item.ZILLA_NAME_BNG;
                obj.ZILLA_CODE = item.ZILLA_CODE;

                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_ZILLA_OBJ_List.Add(new
                {
                    ZILLA_NO = obj.ZILLA_NO,
                    DIVISION_NO = obj.DIVISION_NO,
                    ZILLA_NAME = obj.ZILLA_NAME,
                    ZILLA_NAME_BNG = obj.ZILLA_NAME_BNG,
                    ZILLA_CODE = obj.ZILLA_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_ZONE_OBJ> DOWN_SET_ZONE_OBJ_List = new List<DOWN_SET_ZONE_OBJ>();
            List<object> DOWN_SET_ZONE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_ZONE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_ZONE_OBJ obj = new DOWN_SET_ZONE_OBJ();
                obj.ZONE_NO = (item.ZONE_NO.HasValue ? item.ZONE_NO.Value.ToString() : string.Empty);

                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);

                obj.ZONE_NAME = item.ZONE_NAME;
                obj.ZONE_NAME_BNG = item.ZONE_NAME_BNG;
                obj.ZONE_CODE = item.ZONE_CODE;
                obj.ZONE_GRADE = item.ZONE_GRADE;

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_ZONE_OBJ_List.Add(new
                {
                    ZONE_NO = obj.ZONE_NO,
                    DIVISION_NO = obj.DIVISION_NO,
                    ZONE_NAME = obj.ZONE_NAME,
                    ZONE_NAME_BNG = obj.ZONE_NAME_BNG,
                    ZONE_CODE = obj.ZONE_CODE,
                    ZONE_GRADE = obj.ZONE_GRADE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_TRN_MSG_OBJ> DOWN_TRN_MSG_OBJ_List = new List<DOWN_TRN_MSG_OBJ>();
            List<object> DOWN_TRN_MSG_OBJ_List = new List<object>();
            // MobileLogger.logDefault("MAX_LAST_DOWN_TIME " + MAX_LAST_DOWN_TIME + " DEVICE_ID  " + DEVICE_ID, "msgLog");
            foreach (var item in db.DOWN_GET_TRN_MSG(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_MSG_OBJ obj = new DOWN_TRN_MSG_OBJ();
                obj.MSG_NO = (item.MSG_NO.HasValue ? item.MSG_NO.Value.ToString() : string.Empty);

                obj.FROM_USER_NO = (item.FROM_USER_NO.HasValue ? item.FROM_USER_NO.Value.ToString() : string.Empty);
                obj.USER_TYPE_NO = (item.USER_TYPE_NO.HasValue ? item.USER_TYPE_NO.Value.ToString() : string.Empty);

                obj.USER_FULL_NAME = item.USER_FULL_NAME;
                obj.USER_MOBILE = item.USER_MOBILE;
                obj.USER_TYPE = item.USER_TYPE;
                obj.MSG_SUBJECT = item.MSG_SUBJECT;
                obj.MSG_BODY = item.MSG_BODY;
                obj.IS_REPLY_ALLOW = (item.IS_REPLY_ALLOW.HasValue ? item.IS_REPLY_ALLOW.Value.ToString() : string.Empty);
                obj.REF_MSG_NO = (item.REF_MSG_NO.HasValue ? item.REF_MSG_NO.Value.ToString() : string.Empty);
                obj.IS_REPLY = (item.IS_REPLY.HasValue ? item.IS_REPLY.Value.ToString() : string.Empty);
                obj.MSG_REC_NO = (item.MSG_REC_NO.HasValue ? item.MSG_REC_NO.Value.ToString() : string.Empty);
                obj.REC_USER_NO = (item.REC_USER_NO.HasValue ? item.REC_USER_NO.Value.ToString() : string.Empty);
                obj.IS_READ = (item.IS_READ.HasValue ? item.IS_READ.Value.ToString() : string.Empty);
                obj.INSERT_TIME = (item.INSERT_TIME.HasValue ? this.GetTime(item.INSERT_TIME.Value.ToUniversalTime()) : string.Empty);

                DOWN_TRN_MSG_OBJ_List.Add(new
                {
                    MSG_NO = obj.MSG_NO,
                    FROM_USER_NO = obj.FROM_USER_NO,
                    USER_TYPE_NO = obj.USER_TYPE_NO,
                    USER_FULL_NAME = obj.USER_FULL_NAME,
                    USER_MOBILE = obj.USER_MOBILE,
                    USER_TYPE = obj.USER_TYPE,
                    MSG_SUBJECT = obj.MSG_SUBJECT,
                    MSG_BODY = obj.MSG_BODY,
                    IS_REPLY_ALLOW = obj.IS_REPLY_ALLOW,
                    REF_MSG_NO = obj.REF_MSG_NO,
                    IS_REPLY = obj.IS_REPLY,
                    MSG_REC_NO = obj.MSG_REC_NO,
                    REC_USER_NO = obj.REC_USER_NO,
                    IS_READ = obj.IS_READ,
                    INSERT_TIME = obj.INSERT_TIME
                });
                //MobileLogger.logDefault("name " + obj.USER_FULL_NAME+ " sub  "+obj.MSG_SUBJECT, "msgLog");
            }

            //List<DOWN_TRN_USER_PROMO_ITEM_OBJ> DOWN_TRN_USER_PROMO_ITEM_OBJ_List = new List<DOWN_TRN_USER_PROMO_ITEM_OBJ>();
            List<object> DOWN_TRN_USER_PROMO_ITEM_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_PROMO_ITEM(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_PROMO_ITEM_OBJ obj = new DOWN_TRN_USER_PROMO_ITEM_OBJ();
                obj.USER_PROMO_NO = (item.USER_PROMO_NO.HasValue ? item.USER_PROMO_NO.Value.ToString() : string.Empty);

                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);

                obj.ASSIGN_DATE = (item.ASSIGN_DATE.HasValue ? this.GetTime(item.ASSIGN_DATE.Value.ToUniversalTime()) : string.Empty);
                obj.TARGET_DATE_FROM = (item.TARGET_DATE_FROM.HasValue ? this.GetTime(item.TARGET_DATE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.TARGET_DATE_TO = (item.TARGET_DATE_TO.HasValue ? this.GetTime(item.TARGET_DATE_TO.Value.ToUniversalTime()) : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);


                DOWN_TRN_USER_PROMO_ITEM_OBJ_List.Add(new
                {
                    USER_PROMO_NO = obj.USER_PROMO_NO,
                    USER_NO = obj.USER_NO,
                    ASSIGN_DATE = obj.ASSIGN_DATE,
                    TARGET_DATE_FROM = obj.TARGET_DATE_FROM,
                    TARGET_DATE_TO = obj.TARGET_DATE_TO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            //List<DOWN_TRN_USER_PROMO_DET_OBJ> DOWN_TRN_USER_PROMO_DET_OBJ_List = new List<DOWN_TRN_USER_PROMO_DET_OBJ>();
            List<object> DOWN_TRN_USER_PROMO_DET_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_PROMO_DET(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_PROMO_DET_OBJ obj = new DOWN_TRN_USER_PROMO_DET_OBJ();
                obj.USER_PROMO_DET_NO = (item.USER_PROMO_DET_NO.HasValue ? item.USER_PROMO_DET_NO.Value.ToString() : string.Empty);

                obj.USER_PROMO_NO = (item.USER_PROMO_NO.HasValue ? item.USER_PROMO_NO.Value.ToString() : string.Empty);
                obj.PROMO_ITEM_CODE = item.PROMO_ITEM_CODE;
                obj.PROMO_ITEM_NAME = item.PROMO_ITEM_NAME;
                obj.PROMO_ITEM_NO = (item.PROMO_ITEM_NO.HasValue ? item.PROMO_ITEM_NO.Value.ToString() : string.Empty);
                obj.QTY = (item.QTY.HasValue ? item.QTY.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);


                DOWN_TRN_USER_PROMO_DET_OBJ_List.Add(new
                {
                    USER_PROMO_DET_NO = obj.USER_PROMO_DET_NO,
                    USER_PROMO_NO = obj.USER_PROMO_NO,
                    PROMO_ITEM_CODE = obj.PROMO_ITEM_CODE,
                    PROMO_ITEM_NAME = obj.PROMO_ITEM_NAME,
                    PROMO_ITEM_NO = obj.PROMO_ITEM_NO,
                    QTY = obj.QTY,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            //List<DOWN_TRN_USER_SPECIMEN_OBJ> DOWN_TRN_USER_SPECIMEN_OBJ_List = new List<DOWN_TRN_USER_SPECIMEN_OBJ>();
            List<object> DOWN_TRN_USER_SPECIMEN_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_SPECIMEN(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_SPECIMEN_OBJ obj = new DOWN_TRN_USER_SPECIMEN_OBJ();
                obj.USER_SPECIMEN_NO = (item.USER_SPECIMEN_NO.HasValue ? item.USER_SPECIMEN_NO.Value.ToString() : string.Empty);

                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);

                obj.ASSIGN_DATE = (item.ASSIGN_DATE.HasValue ? item.ASSIGN_DATE.Value.Ticks.ToString() : string.Empty);
                obj.TARGET_DATE_FROM = (item.TARGET_DATE_FROM.HasValue ? this.GetTime(item.TARGET_DATE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.TARGET_DATE_TO = (item.TARGET_DATE_TO.HasValue ? this.GetTime(item.TARGET_DATE_TO.Value.ToUniversalTime()) : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);


                DOWN_TRN_USER_SPECIMEN_OBJ_List.Add(new
                {
                    USER_SPECIMEN_NO = obj.USER_SPECIMEN_NO,
                    USER_NO = obj.USER_NO,
                    ASSIGN_DATE = obj.ASSIGN_DATE,
                    TARGET_DATE_FROM = obj.TARGET_DATE_FROM,
                    TARGET_DATE_TO = obj.TARGET_DATE_TO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            //List<DOWN_TRN_USER_SPECIMEN_DET_OBJ> DOWN_TRN_USER_SPECIMEN_DET_OBJ_List = new List<DOWN_TRN_USER_SPECIMEN_DET_OBJ>();
            List<object> DOWN_TRN_USER_SPECIMEN_DET_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_SPECIMEN_DET(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_SPECIMEN_DET_OBJ obj = new DOWN_TRN_USER_SPECIMEN_DET_OBJ();
                obj.SPECIMEN_DET_NO = (item.SPECIMEN_DET_NO.HasValue ? item.SPECIMEN_DET_NO.Value.ToString() : string.Empty);

                obj.USER_SPECIMEN_NO = (item.USER_SPECIMEN_NO.HasValue ? item.USER_SPECIMEN_NO.Value.ToString() : string.Empty);
                obj.SPECIMEN_CODE = item.SPECIMEN_CODE;
                obj.SPECIMEN_NAME = item.SPECIMEN_NAME;
                obj.SPECIMEN_NAME_BNG = item.SPECIMEN_NAME_BNG;
                obj.SPECIMEN_NO = (item.SPECIMEN_NO.HasValue ? item.SPECIMEN_NO.Value.ToString() : string.Empty);
                obj.QTY = (item.QTY.HasValue ? item.QTY.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);

                DOWN_TRN_USER_SPECIMEN_DET_OBJ_List.Add(new
                {
                    SPECIMEN_DET_NO = obj.SPECIMEN_DET_NO,
                    USER_SPECIMEN_NO = obj.USER_SPECIMEN_NO,
                    SPECIMEN_CODE = obj.SPECIMEN_CODE,
                    SPECIMEN_NAME = obj.SPECIMEN_NAME,
                    SPECIMEN_NAME_BNG = obj.SPECIMEN_NAME_BNG,
                    SPECIMEN_NO = obj.SPECIMEN_NO,
                    QTY = obj.QTY,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            /*
            var result = new {
                SEC_USERS = DOWN_SEC_USERS, SEC_USER_THANA = DOWN_SEC_USER_THANA,
                SET_CLASS = DOWN_SET_CLASS, SET_CLIENT_INFO = DOWN_SET_CLIENT_INFO, SET_DIVISION = DOWN_SET_DIVISION,
                SET_EXP_TYPE = DOWN_SET_EXP_TYPE, SET_INST_TYPE = DOWN_SET_INST_TYPE, SET_INSTITUTE = DOWN_SET_INSTITUTE,
                SET_ORG_TYPE = DOWN_SET_ORG_TYPE, SET_PROMO_ITEM = DOWN_SET_PROMO_ITEM, SET_SPECIMEN = DOWN_SET_SPECIMEN,
                SET_SUBJECT = DOWN_SET_SUBJECT, SET_TEACHER_DESIG = DOWN_SET_TEACHER_DESIG, SET_TEACHER_INFO = DOWN_SET_TEACHER_INFO,
                SET_THANA = DOWN_SET_THANA, SET_TRANSPORT_TYPE = DOWN_SET_TRANSPORT_TYPE, SET_WORK_PURPOSE = DOWN_SET_WORK_PURPOSE,
                SET_ZILLA = DOWN_SET_ZILLA, SET_ZONE = DOWN_SET_ZONE, TRN_MSG = DOWN_TRN_MSG, TRN_USER_PROMO_DET = DOWN_TRN_USER_PROMO_DET,
                TRN_USER_PROMO_ITEM = DOWN_TRN_USER_PROMO_ITEM, TRN_USER_SPECIMEN = DOWN_TRN_USER_SPECIMEN,
                TRN_USER_SPECIMEN_DET = DOWN_TRN_USER_SPECIMEN_DET,
           };
            */


            var result = new
            {
                SET_FEEDBACK_TYPE = DOWN_SET_FEEDBACK_TYPE_OBJ_List,
                SET_LOGOUT_TYPE = DOWN_SET_LOGOUT_TYPE_OBJ_List,
                SEC_USERS = DOWN_SEC_USERS_OBJ_List,
                SEC_USER_THANA = DOWN_SEC_USER_THANA_OBJ_List,
                SET_CLASS = DOWN_SET_CLASS_OBJ_List,
                SET_CLIENT_INFO = DOWN_SET_CLIENT_INFO_OBJ_List,
                SET_DIVISION = DOWN_SET_DIVISION_OBJ_List,
                SET_EXP_TYPE = DOWN_SET_EXP_TYPE_OBJ_List,
                SET_INST_TYPE = DOWN_SET_INST_TYPE_OBJ_List,
                SET_INSTITUTE = DOWN_SET_INSTITUTE_OBJ_List,
                SET_ORG_TYPE = DOWN_SET_ORG_TYPE_OBJ_List,
                SET_PROMO_ITEM = DOWN_SET_PROMO_ITEM_OBJ_List,
                SET_SPECIMEN = DOWN_SET_SPECIMEN_OBJ_List,
                SET_SUBJECT = DOWN_SET_SUBJECT_OBJ_List,
                SET_TEACHER_DESIG = DOWN_SET_TEACHER_DESIG_OBJ_List,
                SET_TEACHER_INFO = DOWN_SET_TEACHER_INFO_OBJ_List,
                SET_THANA = DOWN_SET_THANA_OBJ_List,
                SET_TRANSPORT_TYPE = DOWN_SET_TRANSPORT_TYPE_OBJ_List,
                SET_WORK_PURPOSE = DOWN_SET_WORK_PURPOSE_OBJ_List,
                SET_ZILLA = DOWN_SET_ZILLA_OBJ_List,
                SET_ZONE = DOWN_SET_ZONE_OBJ_List,
                TRN_MSG = DOWN_TRN_MSG_OBJ_List,
                TRN_USER_PROMO_DET = DOWN_TRN_USER_PROMO_DET_OBJ_List,
                TRN_USER_PROMO_ITEM = DOWN_TRN_USER_PROMO_ITEM_OBJ_List,
                TRN_USER_SPECIMEN = DOWN_TRN_USER_SPECIMEN_DET_OBJ_List,
                TRN_USER_SPECIMEN_DET = DOWN_TRN_USER_SPECIMEN_DET_OBJ_List,
            };

            /*
            if (DEVICE_ID == "042009267")
                MobileLogger.logDefault(new JsonResult()
                {
                    Data = result,
                    ContentType = "application/json",
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                }.ToString(), "042009267");
            */
            //var master_obj = new { @result = result };
            DOWN_MASTER master_obj = new DOWN_MASTER();


            //return Json(master_obj, JsonRequestBehavior.AllowGet);
            return new JsonResult()
            {
                Data = result,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DownloadData_4_0(string DEVICE_ID, string IS_ALL)
        {

            //var MAX_LAST_DOWN_TIME = db.DOWN_MAX_GET(DEVICE_ID).First().Value;

            //List<DOWN_SEC_USERS_OBJ> DOWN_SEC_USERS_OBJ_List = new List<DOWN_SEC_USERS_OBJ>();
            //MobileLogger.logDefault("DEVICE_ID " + DEVICE_ID, "Download Data");
            DateTime MAX_LAST_DOWN_TIME;

            if (string.IsNullOrEmpty(IS_ALL) || string.IsNullOrWhiteSpace(IS_ALL) || (IS_ALL == "0") || (IS_ALL.ToUpper() == "FALSE"))
            {
                MAX_LAST_DOWN_TIME = db.DOWN_MAX_GET(DEVICE_ID).First().Value;
            }
            else
            {
                //MAX_LAST_DOWN_TIME = db.DOWN_MAX_GET(DEVICE_ID).First().Value;
                MAX_LAST_DOWN_TIME = DateTime.Today.AddDays(-5000);
            }

            List<object> DOWN_SET_LOGOUT_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_LOGOUT_TYPE(MAX_LAST_DOWN_TIME, string.Empty))
            {
                DOWN_SET_LOGOUT_TYPE_OBJ obj = new DOWN_SET_LOGOUT_TYPE_OBJ();
                obj.LOGOUT_TYPE_NO = (item.LOGOUT_TYPE_NO.HasValue ? item.LOGOUT_TYPE_NO.Value.ToString() : string.Empty);
                obj.LOGOUT_TYPE_NAME = item.LOGOUT_TYPE_NAME;

                DOWN_SET_LOGOUT_TYPE_OBJ_List.Add(new
                {
                    LOGOUT_TYPE_NO = obj.LOGOUT_TYPE_NO,
                    LOGOUT_TYPE_NAME = obj.LOGOUT_TYPE_NAME
                });
            }

            List<object> DOWN_SET_FEEDBACK_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_FEEDBACK_TYPE(MAX_LAST_DOWN_TIME, string.Empty))
            {
                DOWN_SET_FEEDBACK_TYPE_OBJ obj = new DOWN_SET_FEEDBACK_TYPE_OBJ();
                obj.FEEDBACK_TYPE_NO = (item.FEEDBACK_TYPE_NO.HasValue ? item.FEEDBACK_TYPE_NO.Value.ToString() : string.Empty);
                obj.FEEDBACK_NAME = item.FEEDBACK_NAME;
                obj.FEEDBACK_CODE = item.FEEDBACK_CODE;
                obj.FEEDBACK_DESC = item.FEEDBACK_DESC;
                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_FEEDBACK_TYPE_OBJ_List.Add(new
                {
                    FEEDBACK_TYPE_NO = obj.FEEDBACK_TYPE_NO,
                    FEEDBACK_NAME = obj.FEEDBACK_NAME,
                    FEEDBACK_CODE = obj.FEEDBACK_CODE,
                    FEEDBACK_DESC = obj.FEEDBACK_DESC,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO
                });
            }

            List<object> DOWN_SEC_USERS_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SEC_USERS(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SEC_USERS_OBJ obj = new DOWN_SEC_USERS_OBJ();
                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);
                obj.USER_TYPE_NO = (item.USER_TYPE_NO.HasValue ? item.USER_TYPE_NO.Value.ToString() : string.Empty);
                obj.HR_EMP_ID = item.HR_EMP_ID;
                obj.USER_FULL_NAME = item.USER_FULL_NAME;
                obj.USER_MOBILE = item.USER_MOBILE;

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SEC_USERS_OBJ_List.Add(new
                {
                    USER_NO = obj.USER_NO,
                    USER_TYPE_NO = obj.USER_TYPE_NO,
                    HR_EMP_ID = obj.HR_EMP_ID,
                    USER_FULL_NAME = obj.USER_FULL_NAME,
                    USER_MOBILE = obj.USER_MOBILE,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SEC_USER_THANA_OBJ> DOWN_SEC_USER_THANA_OBJ_List = new List<DOWN_SEC_USER_THANA_OBJ>();
            List<object> DOWN_SEC_USER_THANA_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SEC_USER_THANA(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SEC_USER_THANA_OBJ obj = new DOWN_SEC_USER_THANA_OBJ();
                obj.USER_THANA_NO = (item.USER_THANA_NO.HasValue ? item.USER_THANA_NO.Value.ToString() : string.Empty);
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);
                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);
                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SEC_USER_THANA_OBJ_List.Add(new
                {
                    USER_THANA_NO = obj.USER_THANA_NO,
                    THANA_NO = obj.THANA_NO,
                    USER_NO = obj.USER_NO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_CLASS_OBJ> DOWN_SET_CLASS_OBJ_List = new List<DOWN_SET_CLASS_OBJ>();
            List<object> DOWN_SET_CLASS_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_CLASS(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_CLASS_OBJ obj = new DOWN_SET_CLASS_OBJ();
                obj.CLASS_NO = (item.CLASS_NO.HasValue ? item.CLASS_NO.Value.ToString() : string.Empty);

                obj.CLASS_NAME = item.CLASS_NAME;
                obj.CLASS_NAME_BNG = item.CLASS_NAME_BNG;
                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_CLASS_OBJ_List.Add(new
                {
                    CLASS_NO = obj.CLASS_NO,
                    CLASS_NAME = obj.CLASS_NAME,
                    CLASS_NAME_BNG = obj.CLASS_NAME_BNG,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_CLIENT_INFO_OBJ> DOWN_SET_CLIENT_INFO_OBJ_List = new List<DOWN_SET_CLIENT_INFO_OBJ>();
            List<object> DOWN_SET_CLIENT_INFO_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_CLIENT_INFO(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_CLIENT_INFO_OBJ obj = new DOWN_SET_CLIENT_INFO_OBJ();
                obj.CLIENT_NO = (item.CLIENT_NO.HasValue ? item.CLIENT_NO.Value.ToString() : string.Empty);

                obj.CLIENT_NAME = item.CLIENT_NAME;
                obj.CLIENT_NAME_BNG = item.CLIENT_NAME_BNG;
                obj.CLIENT_NICK_NAME = item.CLIENT_NICK_NAME;
                obj.CLIENT_MOBILE = item.CLIENT_MOBILE;

                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);
                obj.ZONE_NO = (item.ZONE_NO.HasValue ? item.ZONE_NO.Value.ToString() : string.Empty);
                obj.ZILLA_NO = (item.ZILLA_NO.HasValue ? item.ZILLA_NO.Value.ToString() : string.Empty);
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_CLIENT_INFO_OBJ_List.Add(new
                {
                    CLIENT_NO = obj.CLIENT_NO,
                    CLIENT_NAME = obj.CLIENT_NAME,
                    CLIENT_NAME_BNG = obj.CLIENT_NAME_BNG,
                    CLIENT_NICK_NAME = obj.CLIENT_NICK_NAME,
                    CLIENT_MOBILE = obj.CLIENT_MOBILE,
                    obj.DIVISION_NO,
                    ZONE_NO = obj.ZONE_NO,
                    ZILLA_NO = obj.ZILLA_NO,
                    THANA_NO = obj.THANA_NO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_DIVISION_OBJ> DOWN_SET_DIVISION_OBJ_List = new List<DOWN_SET_DIVISION_OBJ>();
            List<object> DOWN_SET_DIVISION_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_DIVISION(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_DIVISION_OBJ obj = new DOWN_SET_DIVISION_OBJ();
                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);

                obj.DIVISION_NAME = item.DIVISION_NAME;
                obj.DIVISION_NAME_BNG = item.DIVISION_NAME_BNG;
                obj.DIVISION_CODE = item.DIVISION_CODE;

                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_DIVISION_OBJ_List.Add(new
                {
                    DIVISION_NO = obj.DIVISION_NO,
                    DIVISION_NAME = obj.DIVISION_NAME,
                    DIVISION_NAME_BNG = obj.DIVISION_NAME_BNG,
                    DIVISION_CODE = obj.DIVISION_CODE,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_EXP_TYPE_OBJ> DOWN_SET_EXP_TYPE_OBJ_List = new List<DOWN_SET_EXP_TYPE_OBJ>();
            List<object> DOWN_SET_EXP_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_EXP_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_EXP_TYPE_OBJ obj = new DOWN_SET_EXP_TYPE_OBJ();
                obj.EXP_TYPE_NO = (item.EXP_TYPE_NO.HasValue ? item.EXP_TYPE_NO.Value.ToString() : string.Empty);

                obj.EXP_TYPE_CODE = item.EXP_TYPE_CODE;
                obj.EXP_TYPE_NAME = item.EXP_TYPE_NAME;
                obj.EXP_TYPE_DETAILS = item.EXP_TYPE_DETAILS;
                obj.EXP_TYPE_DESC = item.EXP_TYPE_DESC;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_EXP_TYPE_OBJ_List.Add(new
                {
                    EXP_TYPE_NO = obj.EXP_TYPE_NO,
                    EXP_TYPE_NAME = obj.EXP_TYPE_NAME,
                    EXP_TYPE_CODE = obj.EXP_TYPE_CODE,
                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_INSTITUTE_OBJ> DOWN_SET_INSTITUTE_OBJ_List = new List<DOWN_SET_INSTITUTE_OBJ>();
            List<object> DOWN_SET_INSTITUTE_OBJ_List = new List<object>();


            foreach (var item in db.DOWN_GET_SET_INSTITUTE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_INSTITUTE_OBJ obj = new DOWN_SET_INSTITUTE_OBJ();
                obj.INSTITUTE_NO = (item.INSTITUTE_NO.HasValue ? item.INSTITUTE_NO.Value.ToString() : string.Empty);

                obj.INSTITUTE_NAME = item.INSTITUTE_NAME;
                obj.INSTITUTE_NAME_BNG = item.INSTITUTE_NAME_BNG;
                obj.INST_TYPE_NO = (item.INST_TYPE_NO.HasValue ? item.INST_TYPE_NO.Value.ToString() : string.Empty);
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);
                obj.F_INSTITUTION_DB_ID = item.F_INSTITUTION_DB_ID;
                obj.EIIN_NUMBER = item.EIIN_NUMBER;
                obj.MEET_PERSON_NAME = item.MEET_PERSON_NAME;
                obj.CONTACT_NUMBER = item.CONTACT_NUMBER;

                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_INSTITUTE_OBJ_List.Add(new
                {
                    INSTITUTE_NO = obj.INSTITUTE_NO,
                    INSTITUTE_NAME = obj.INSTITUTE_NAME,
                    INSTITUTE_NAME_BNG = obj.INSTITUTE_NAME_BNG,
                    INST_TYPE_NO = obj.INST_TYPE_NO,
                    THANA_NO = obj.THANA_NO,
                    F_INSTITUTION_DB_ID = obj.F_INSTITUTION_DB_ID,
                    EIIN_NUMBER = obj.EIIN_NUMBER,
                    MEET_PERSON_NAME = obj.MEET_PERSON_NAME,
                    CONTACT_NUMBER = obj.CONTACT_NUMBER,
                    ACTUAL_LATI_VAL = obj.ACTUAL_LATI_VAL,
                    ACTUAL_LONG_VAL = obj.ACTUAL_LONG_VAL,
                    FROM_LATI_VAL = obj.FROM_LATI_VAL,
                    FROM_LONG_VAL = obj.FROM_LONG_VAL,
                    TO_LATI_VAL = obj.TO_LATI_VAL,
                    TO_LONG_VAL = obj.TO_LONG_VAL,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_INST_TYPE_OBJ> DOWN_SET_INST_TYPE_OBJ_List = new List<DOWN_SET_INST_TYPE_OBJ>();
            List<object> DOWN_SET_INST_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_INST_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_INST_TYPE_OBJ obj = new DOWN_SET_INST_TYPE_OBJ();
                obj.INST_TYPE_NO = (item.INST_TYPE_NO.HasValue ? item.INST_TYPE_NO.Value.ToString() : string.Empty);

                obj.INST_TYPE_NAME = item.INST_TYPE_NAME;
                obj.INST_TYPE_CODE = item.INST_TYPE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_INST_TYPE_OBJ_List.Add(new
                {
                    INST_TYPE_NO = obj.INST_TYPE_NO,
                    INST_TYPE_NAME = obj.INST_TYPE_NAME,
                    INST_TYPE_CODE = obj.INST_TYPE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_ORG_TYPE_OBJ> DOWN_SET_ORG_TYPE_OBJ_List = new List<DOWN_SET_ORG_TYPE_OBJ>();
            List<object> DOWN_SET_ORG_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_ORG_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_ORG_TYPE_OBJ obj = new DOWN_SET_ORG_TYPE_OBJ();
                obj.ORG_TYPE_NO = (item.ORG_TYPE_NO.HasValue ? item.ORG_TYPE_NO.Value.ToString() : string.Empty);

                obj.ORG_TYPE_NAME = item.ORG_TYPE_NAME;
                obj.ORG_TYPE_CODE = item.ORG_TYPE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_ORG_TYPE_OBJ_List.Add(new
                {
                    ORG_TYPE_NO = obj.ORG_TYPE_NO,
                    ORG_TYPE_NAME = obj.ORG_TYPE_NAME,
                    ORG_TYPE_CODE = obj.ORG_TYPE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_PROMO_ITEM_OBJ> DOWN_SET_PROMO_ITEM_OBJ_List = new List<DOWN_SET_PROMO_ITEM_OBJ>();
            List<object> DOWN_SET_PROMO_ITEM_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_PROMO_ITEM(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_PROMO_ITEM_OBJ obj = new DOWN_SET_PROMO_ITEM_OBJ();
                obj.PROMO_ITEM_NO = (item.PROMO_ITEM_NO.HasValue ? item.PROMO_ITEM_NO.Value.ToString() : string.Empty);

                obj.PROMO_ITEM_NAME = item.PROMO_ITEM_NAME;
                obj.PROMO_ITEM_CODE = item.PROMO_ITEM_CODE;

                obj.PROMO_ITEM_TYPE_NO = (item.PROMO_ITEM_TYPE_NO.HasValue ? item.PROMO_ITEM_TYPE_NO.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_PROMO_ITEM_OBJ_List.Add(new
                {
                    PROMO_ITEM_NO = obj.PROMO_ITEM_NO,
                    PROMO_ITEM_NAME = obj.PROMO_ITEM_NAME,
                    PROMO_ITEM_CODE = obj.PROMO_ITEM_CODE,
                    PROMO_ITEM_TYPE_NO = obj.PROMO_ITEM_TYPE_NO,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_SPECIMEN_OBJ> DOWN_SET_SPECIMEN_OBJ_List = new List<DOWN_SET_SPECIMEN_OBJ>();
            List<object> DOWN_SET_SPECIMEN_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_SPECIMEN(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_SPECIMEN_OBJ obj = new DOWN_SET_SPECIMEN_OBJ();
                obj.SPECIMEN_NO = (item.SPECIMEN_NO.HasValue ? item.SPECIMEN_NO.Value.ToString() : string.Empty);

                obj.SPECIMEN_NAME = item.SPECIMEN_NAME;
                obj.SPECIMEN_NAME_BNG = item.SPECIMEN_NAME_BNG;
                obj.SPECIMEN_CODE = item.SPECIMEN_CODE;

                obj.BOOK_TYPE_NO = (item.BOOK_TYPE_NO.HasValue ? item.BOOK_TYPE_NO.Value.ToString() : string.Empty);
                obj.BOOK_UNIQUE_CODE = item.BOOK_UNIQUE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_SPECIMEN_OBJ_List.Add(new
                {
                    SPECIMEN_NO = obj.SPECIMEN_NO,
                    SPECIMEN_NAME = obj.SPECIMEN_NAME,
                    SPECIMEN_NAME_BNG = obj.SPECIMEN_NAME_BNG,
                    SPECIMEN_CODE = obj.SPECIMEN_CODE,
                    BOOK_TYPE_NO = obj.BOOK_TYPE_NO,
                    BOOK_UNIQUE_CODE = obj.BOOK_UNIQUE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
               // MobileLogger.logDefault("Device id " + DEVICE_ID + " SPECIMEN_NO " + obj.SPECIMEN_NO + " SPECIMEN_NAME "
                 //   + obj.SPECIMEN_NAME, "SpecimenDownload");
            }
            //MobileLogger.logDefault("Device id " + DEVICE_ID + " SPECIMEN_SIZE " + DOWN_SET_SPECIMEN_OBJ_List.Count, "SpecimenDownload");
            //List<DOWN_SET_SUBJECT_OBJ> DOWN_SET_SUBJECT_OBJ_List = new List<DOWN_SET_SUBJECT_OBJ>();
            List<object> DOWN_SET_SUBJECT_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_SUBJECT(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_SUBJECT_OBJ obj = new DOWN_SET_SUBJECT_OBJ();
                obj.SUBJECT_NO = (item.SUBJECT_NO.HasValue ? item.SUBJECT_NO.Value.ToString() : string.Empty);

                obj.SUBJECT_NAME = item.SUBJECT_NAME;
                obj.SUBJECT_NAME_BNG = item.SUBJECT_NAME_BNG;
                obj.SUBJECT_CODE = item.SUBJECT_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_SUBJECT_OBJ_List.Add(new
                {
                    SUBJECT_NO = obj.SUBJECT_NO,
                    SUBJECT_NAME = obj.SUBJECT_NAME,
                    SUBJECT_NAME_BNG = obj.SUBJECT_NAME_BNG,
                    SUBJECT_CODE = obj.SUBJECT_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_TEACHER_DESIG_OBJ> DOWN_SET_TEACHER_DESIG_OBJ_List = new List<DOWN_SET_TEACHER_DESIG_OBJ>();
            List<object> DOWN_SET_TEACHER_DESIG_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_TEACHER_DESIG(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_TEACHER_DESIG_OBJ obj = new DOWN_SET_TEACHER_DESIG_OBJ();
                obj.TEACH_DESIG_NO = (item.TEACH_DESIG_NO.HasValue ? item.TEACH_DESIG_NO.Value.ToString() : string.Empty);

                obj.TEACHER_DESIG_NAME = item.TEACHER_DESIG_NAME;
                obj.TEACHER_DESIG_NAME_BNG = item.TEACHER_DESIG_NAME_BNG;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_TEACHER_DESIG_OBJ_List.Add(new
                {
                    TEACH_DESIG_NO = obj.TEACH_DESIG_NO,
                    TEACHER_DESIG_NAME = obj.TEACHER_DESIG_NAME,
                    TEACHER_DESIG_NAME_BNG = obj.TEACHER_DESIG_NAME_BNG,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_TEACHER_INFO_OBJ> DOWN_SET_TEACHER_INFO_OBJ_List = new List<DOWN_SET_TEACHER_INFO_OBJ>();
            List<object> DOWN_SET_TEACHER_INFO_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_TEACHER_INFO(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_TEACHER_INFO_OBJ obj = new DOWN_SET_TEACHER_INFO_OBJ();
                obj.TEACHER_NO = (item.TEACHER_NO.HasValue ? item.TEACHER_NO.Value.ToString() : string.Empty);

                obj.TEACHER_NAME = item.TEACHER_NAME;
                obj.TEACHER_NAME_BNG = item.TEACHER_NAME_BNG;
                obj.TEACHER_NICK_NAME = item.TEACHER_NICK_NAME;
                obj.TEACHER_MOBILE = item.TEACHER_MOBILE;
                obj.INSTITUTE_NO = (item.INSTITUTE_NO.HasValue ? item.INSTITUTE_NO.Value.ToString() : string.Empty);
                obj.TEACH_DESIG_NO = (item.TEACH_DESIG_NO.HasValue ? item.TEACH_DESIG_NO.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_TEACHER_INFO_OBJ_List.Add(new
                {
                    TEACHER_NO = obj.TEACHER_NO,
                    TEACHER_NAME = obj.TEACHER_NAME,
                    TEACHER_NAME_BNG = obj.TEACHER_NAME_BNG,
                    TEACHER_NICK_NAME = obj.TEACHER_NICK_NAME,
                    TEACHER_MOBILE = obj.TEACHER_MOBILE,
                    INSTITUTE_NO = obj.INSTITUTE_NO,
                    TEACH_DESIG_NO = obj.TEACH_DESIG_NO,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_THANA_OBJ> DOWN_SET_THANA_OBJ_List = new List<DOWN_SET_THANA_OBJ>();
            List<object> DOWN_SET_THANA_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_THANA(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_THANA_OBJ obj = new DOWN_SET_THANA_OBJ();
                obj.THANA_NO = (item.THANA_NO.HasValue ? item.THANA_NO.Value.ToString() : string.Empty);
                obj.ZONE_NO = (item.ZONE_NO.HasValue ? item.ZONE_NO.Value.ToString() : string.Empty);
                obj.ZILLA_NO = (item.ZILLA_NO.HasValue ? item.ZILLA_NO.Value.ToString() : string.Empty);

                obj.THANA_NAME = item.THANA_NAME;
                obj.THANA_NAME_BNG = item.THANA_NAME_BNG;
                obj.THANA_CODE = item.THANA_CODE;


                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_THANA_OBJ_List.Add(new
                {
                    THANA_NO = obj.THANA_NO,
                    ZONE_NO = obj.ZONE_NO,
                    ZILLA_NO = obj.ZILLA_NO,
                    THANA_NAME = obj.THANA_NAME,
                    THANA_NAME_BNG = obj.THANA_NAME_BNG,
                    THANA_CODE = obj.THANA_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_TRANSPORT_TYPE_OBJ> DOWN_SET_TRANSPORT_TYPE_OBJ_List = new List<DOWN_SET_TRANSPORT_TYPE_OBJ>();
            List<object> DOWN_SET_TRANSPORT_TYPE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_TRANSPORT_TYPE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_TRANSPORT_TYPE_OBJ obj = new DOWN_SET_TRANSPORT_TYPE_OBJ();
                obj.TRANS_TYPE_NO = (item.TRANS_TYPE_NO.HasValue ? item.TRANS_TYPE_NO.Value.ToString() : string.Empty);

                obj.TRANS_TYPE_NAME = item.TRANS_TYPE_NAME;

                obj.TRANS_TYPE_CODE = item.TRANS_TYPE_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_TRANSPORT_TYPE_OBJ_List.Add(new
                {
                    TRANS_TYPE_NO = obj.TRANS_TYPE_NO,
                    TRANS_TYPE_NAME = obj.TRANS_TYPE_NAME,
                    TRANS_TYPE_CODE = obj.TRANS_TYPE_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_WORK_PURPOSE_OBJ> DOWN_SET_WORK_PURPOSE_OBJ_List = new List<DOWN_SET_WORK_PURPOSE_OBJ>();
            List<object> DOWN_SET_WORK_PURPOSE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_WORK_PURPOSE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_WORK_PURPOSE_OBJ obj = new DOWN_SET_WORK_PURPOSE_OBJ();
                obj.WORK_PUR_NO = (item.WORK_PUR_NO.HasValue ? item.WORK_PUR_NO.Value.ToString() : string.Empty);

                obj.WORK_PUR_NAME = item.WORK_PUR_NAME;
                obj.WORK_PUR_CODE = item.WORK_PUR_CODE;

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_WORK_PURPOSE_OBJ_List.Add(new
                {
                    WORK_PUR_NO = obj.WORK_PUR_NO,
                    WORK_PUR_NAME = obj.WORK_PUR_NAME,
                    WORK_PUR_CODE = obj.WORK_PUR_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_ZILLA_OBJ> DOWN_SET_ZILLA_OBJ_List = new List<DOWN_SET_ZILLA_OBJ>();
            List<object> DOWN_SET_ZILLA_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_ZILLA(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_ZILLA_OBJ obj = new DOWN_SET_ZILLA_OBJ();
                obj.ZILLA_NO = (item.ZILLA_NO.HasValue ? item.ZILLA_NO.Value.ToString() : string.Empty);

                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);

                obj.ZILLA_NAME = item.ZILLA_NAME;
                obj.ZILLA_NAME_BNG = item.ZILLA_NAME_BNG;
                obj.ZILLA_CODE = item.ZILLA_CODE;

                obj.ACTUAL_LATI_VAL = (item.ACTUAL_LATI_VAL.HasValue ? item.ACTUAL_LATI_VAL.Value.ToString() : string.Empty);
                obj.ACTUAL_LONG_VAL = (item.ACTUAL_LONG_VAL.HasValue ? item.ACTUAL_LONG_VAL.Value.ToString() : string.Empty);
                obj.FROM_LATI_VAL = (item.FROM_LATI_VAL.HasValue ? item.FROM_LATI_VAL.Value.ToString() : string.Empty);
                obj.FROM_LONG_VAL = (item.FROM_LONG_VAL.HasValue ? item.FROM_LONG_VAL.Value.ToString() : string.Empty);
                obj.TO_LATI_VAL = (item.TO_LATI_VAL.HasValue ? item.TO_LATI_VAL.Value.ToString() : string.Empty);
                obj.TO_LONG_VAL = (item.TO_LONG_VAL.HasValue ? item.TO_LONG_VAL.Value.ToString() : string.Empty);

                obj.SL_NUM = (item.SL_NUM.HasValue ? item.SL_NUM.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_ZILLA_OBJ_List.Add(new
                {
                    ZILLA_NO = obj.ZILLA_NO,
                    DIVISION_NO = obj.DIVISION_NO,
                    ZILLA_NAME = obj.ZILLA_NAME,
                    ZILLA_NAME_BNG = obj.ZILLA_NAME_BNG,
                    ZILLA_CODE = obj.ZILLA_CODE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_SET_ZONE_OBJ> DOWN_SET_ZONE_OBJ_List = new List<DOWN_SET_ZONE_OBJ>();
            List<object> DOWN_SET_ZONE_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_SET_ZONE(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_SET_ZONE_OBJ obj = new DOWN_SET_ZONE_OBJ();
                obj.ZONE_NO = (item.ZONE_NO.HasValue ? item.ZONE_NO.Value.ToString() : string.Empty);

                obj.DIVISION_NO = (item.DIVISION_NO.HasValue ? item.DIVISION_NO.Value.ToString() : string.Empty);

                obj.ZONE_NAME = item.ZONE_NAME;
                obj.ZONE_NAME_BNG = item.ZONE_NAME_BNG;
                obj.ZONE_CODE = item.ZONE_CODE;
                obj.ZONE_GRADE = item.ZONE_GRADE;

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);
                obj.ACTIVE_FROM = (item.ACTIVE_FROM.HasValue ? this.GetTime(item.ACTIVE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.ACTIVE_TO = (item.ACTIVE_TO.HasValue ? this.GetTime(item.ACTIVE_TO.Value.ToUniversalTime()) : string.Empty);

                DOWN_SET_ZONE_OBJ_List.Add(new
                {
                    ZONE_NO = obj.ZONE_NO,
                    DIVISION_NO = obj.DIVISION_NO,
                    ZONE_NAME = obj.ZONE_NAME,
                    ZONE_NAME_BNG = obj.ZONE_NAME_BNG,
                    ZONE_CODE = obj.ZONE_CODE,
                    ZONE_GRADE = obj.ZONE_GRADE,

                    IS_ACTIVE = obj.IS_ACTIVE,
                    ACTIVE_FROM = obj.ACTIVE_FROM,
                    ACTIVE_TO = obj.ACTIVE_TO,
                });
            }

            //List<DOWN_TRN_MSG_OBJ> DOWN_TRN_MSG_OBJ_List = new List<DOWN_TRN_MSG_OBJ>();
            List<object> DOWN_TRN_MSG_OBJ_List = new List<object>();
            // MobileLogger.logDefault("MAX_LAST_DOWN_TIME " + MAX_LAST_DOWN_TIME + " DEVICE_ID  " + DEVICE_ID, "msgLog");
            foreach (var item in db.DOWN_GET_TRN_MSG(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_MSG_OBJ obj = new DOWN_TRN_MSG_OBJ();
                obj.MSG_NO = (item.MSG_NO.HasValue ? item.MSG_NO.Value.ToString() : string.Empty);

                obj.FROM_USER_NO = (item.FROM_USER_NO.HasValue ? item.FROM_USER_NO.Value.ToString() : string.Empty);
                obj.USER_TYPE_NO = (item.USER_TYPE_NO.HasValue ? item.USER_TYPE_NO.Value.ToString() : string.Empty);

                obj.USER_FULL_NAME = item.USER_FULL_NAME;
                obj.USER_MOBILE = item.USER_MOBILE;
                obj.USER_TYPE = item.USER_TYPE;
                obj.MSG_SUBJECT = item.MSG_SUBJECT;
                obj.MSG_BODY = item.MSG_BODY;
                obj.IS_REPLY_ALLOW = (item.IS_REPLY_ALLOW.HasValue ? item.IS_REPLY_ALLOW.Value.ToString() : string.Empty);
                obj.REF_MSG_NO = (item.REF_MSG_NO.HasValue ? item.REF_MSG_NO.Value.ToString() : string.Empty);
                obj.IS_REPLY = (item.IS_REPLY.HasValue ? item.IS_REPLY.Value.ToString() : string.Empty);
                obj.MSG_REC_NO = (item.MSG_REC_NO.HasValue ? item.MSG_REC_NO.Value.ToString() : string.Empty);
                obj.REC_USER_NO = (item.REC_USER_NO.HasValue ? item.REC_USER_NO.Value.ToString() : string.Empty);
                obj.IS_READ = (item.IS_READ.HasValue ? item.IS_READ.Value.ToString() : string.Empty);
                obj.INSERT_TIME = (item.INSERT_TIME.HasValue ? this.GetTime(item.INSERT_TIME.Value.ToUniversalTime()) : string.Empty);

                DOWN_TRN_MSG_OBJ_List.Add(new
                {
                    MSG_NO = obj.MSG_NO,
                    FROM_USER_NO = obj.FROM_USER_NO,
                    USER_TYPE_NO = obj.USER_TYPE_NO,
                    USER_FULL_NAME = obj.USER_FULL_NAME,
                    USER_MOBILE = obj.USER_MOBILE,
                    USER_TYPE = obj.USER_TYPE,
                    MSG_SUBJECT = obj.MSG_SUBJECT,
                    MSG_BODY = obj.MSG_BODY,
                    IS_REPLY_ALLOW = obj.IS_REPLY_ALLOW,
                    REF_MSG_NO = obj.REF_MSG_NO,
                    IS_REPLY = obj.IS_REPLY,
                    MSG_REC_NO = obj.MSG_REC_NO,
                    REC_USER_NO = obj.REC_USER_NO,
                    IS_READ = obj.IS_READ,
                    INSERT_TIME = obj.INSERT_TIME
                });
                //MobileLogger.logDefault("name " + obj.USER_FULL_NAME+ " sub  "+obj.MSG_SUBJECT, "msgLog");
            }

            //List<DOWN_TRN_USER_PROMO_ITEM_OBJ> DOWN_TRN_USER_PROMO_ITEM_OBJ_List = new List<DOWN_TRN_USER_PROMO_ITEM_OBJ>();
            List<object> DOWN_TRN_USER_PROMO_ITEM_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_PROMO_ITEM(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_PROMO_ITEM_OBJ obj = new DOWN_TRN_USER_PROMO_ITEM_OBJ();
                obj.USER_PROMO_NO = (item.USER_PROMO_NO.HasValue ? item.USER_PROMO_NO.Value.ToString() : string.Empty);

                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);

                obj.ASSIGN_DATE = (item.ASSIGN_DATE.HasValue ? this.GetTime(item.ASSIGN_DATE.Value.ToUniversalTime()) : string.Empty);
                obj.TARGET_DATE_FROM = (item.TARGET_DATE_FROM.HasValue ? this.GetTime(item.TARGET_DATE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.TARGET_DATE_TO = (item.TARGET_DATE_TO.HasValue ? this.GetTime(item.TARGET_DATE_TO.Value.ToUniversalTime()) : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);


                DOWN_TRN_USER_PROMO_ITEM_OBJ_List.Add(new
                {
                    USER_PROMO_NO = obj.USER_PROMO_NO,
                    USER_NO = obj.USER_NO,
                    ASSIGN_DATE = obj.ASSIGN_DATE,
                    TARGET_DATE_FROM = obj.TARGET_DATE_FROM,
                    TARGET_DATE_TO = obj.TARGET_DATE_TO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            //List<DOWN_TRN_USER_PROMO_DET_OBJ> DOWN_TRN_USER_PROMO_DET_OBJ_List = new List<DOWN_TRN_USER_PROMO_DET_OBJ>();
            List<object> DOWN_TRN_USER_PROMO_DET_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_PROMO_DET(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_PROMO_DET_OBJ obj = new DOWN_TRN_USER_PROMO_DET_OBJ();
                obj.USER_PROMO_DET_NO = (item.USER_PROMO_DET_NO.HasValue ? item.USER_PROMO_DET_NO.Value.ToString() : string.Empty);

                obj.USER_PROMO_NO = (item.USER_PROMO_NO.HasValue ? item.USER_PROMO_NO.Value.ToString() : string.Empty);
                obj.PROMO_ITEM_CODE = item.PROMO_ITEM_CODE;
                obj.PROMO_ITEM_NAME = item.PROMO_ITEM_NAME;
                obj.PROMO_ITEM_NO = (item.PROMO_ITEM_NO.HasValue ? item.PROMO_ITEM_NO.Value.ToString() : string.Empty);
                obj.QTY = (item.QTY.HasValue ? item.QTY.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);


                DOWN_TRN_USER_PROMO_DET_OBJ_List.Add(new
                {
                    USER_PROMO_DET_NO = obj.USER_PROMO_DET_NO,
                    USER_PROMO_NO = obj.USER_PROMO_NO,
                    PROMO_ITEM_CODE = obj.PROMO_ITEM_CODE,
                    PROMO_ITEM_NAME = obj.PROMO_ITEM_NAME,
                    PROMO_ITEM_NO = obj.PROMO_ITEM_NO,
                    QTY = obj.QTY,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            //List<DOWN_TRN_USER_SPECIMEN_OBJ> DOWN_TRN_USER_SPECIMEN_OBJ_List = new List<DOWN_TRN_USER_SPECIMEN_OBJ>();
            List<object> DOWN_TRN_USER_SPECIMEN_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_SPECIMEN(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_SPECIMEN_OBJ obj = new DOWN_TRN_USER_SPECIMEN_OBJ();
                obj.USER_SPECIMEN_NO = (item.USER_SPECIMEN_NO.HasValue ? item.USER_SPECIMEN_NO.Value.ToString() : string.Empty);

                obj.USER_NO = (item.USER_NO.HasValue ? item.USER_NO.Value.ToString() : string.Empty);

                obj.ASSIGN_DATE = (item.ASSIGN_DATE.HasValue ? item.ASSIGN_DATE.Value.Ticks.ToString() : string.Empty);
                obj.TARGET_DATE_FROM = (item.TARGET_DATE_FROM.HasValue ? this.GetTime(item.TARGET_DATE_FROM.Value.ToUniversalTime()) : string.Empty);
                obj.TARGET_DATE_TO = (item.TARGET_DATE_TO.HasValue ? this.GetTime(item.TARGET_DATE_TO.Value.ToUniversalTime()) : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);


                DOWN_TRN_USER_SPECIMEN_OBJ_List.Add(new
                {
                    USER_SPECIMEN_NO = obj.USER_SPECIMEN_NO,
                    USER_NO = obj.USER_NO,
                    ASSIGN_DATE = obj.ASSIGN_DATE,
                    TARGET_DATE_FROM = obj.TARGET_DATE_FROM,
                    TARGET_DATE_TO = obj.TARGET_DATE_TO,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            //List<DOWN_TRN_USER_SPECIMEN_DET_OBJ> DOWN_TRN_USER_SPECIMEN_DET_OBJ_List = new List<DOWN_TRN_USER_SPECIMEN_DET_OBJ>();
            List<object> DOWN_TRN_USER_SPECIMEN_DET_OBJ_List = new List<object>();
            foreach (var item in db.DOWN_GET_TRN_USER_SPECIMEN_DET(MAX_LAST_DOWN_TIME, DEVICE_ID))
            {
                DOWN_TRN_USER_SPECIMEN_DET_OBJ obj = new DOWN_TRN_USER_SPECIMEN_DET_OBJ();
                obj.SPECIMEN_DET_NO = (item.SPECIMEN_DET_NO.HasValue ? item.SPECIMEN_DET_NO.Value.ToString() : string.Empty);

                obj.USER_SPECIMEN_NO = (item.USER_SPECIMEN_NO.HasValue ? item.USER_SPECIMEN_NO.Value.ToString() : string.Empty);
                obj.SPECIMEN_CODE = item.SPECIMEN_CODE;
                obj.SPECIMEN_NAME = item.SPECIMEN_NAME;
                obj.SPECIMEN_NAME_BNG = item.SPECIMEN_NAME_BNG;
                obj.SPECIMEN_NO = (item.SPECIMEN_NO.HasValue ? item.SPECIMEN_NO.Value.ToString() : string.Empty);
                obj.QTY = (item.QTY.HasValue ? item.QTY.Value.ToString() : string.Empty);

                obj.IS_ACTIVE = (item.IS_ACTIVE.HasValue ? item.IS_ACTIVE.Value.ToString() : string.Empty);

                DOWN_TRN_USER_SPECIMEN_DET_OBJ_List.Add(new
                {
                    SPECIMEN_DET_NO = obj.SPECIMEN_DET_NO,
                    USER_SPECIMEN_NO = obj.USER_SPECIMEN_NO,
                    SPECIMEN_CODE = obj.SPECIMEN_CODE,
                    SPECIMEN_NAME = obj.SPECIMEN_NAME,
                    SPECIMEN_NAME_BNG = obj.SPECIMEN_NAME_BNG,
                    SPECIMEN_NO = obj.SPECIMEN_NO,
                    QTY = obj.QTY,
                    IS_ACTIVE = obj.IS_ACTIVE,
                });
            }

            /*
            var result = new {
                SEC_USERS = DOWN_SEC_USERS, SEC_USER_THANA = DOWN_SEC_USER_THANA,
                SET_CLASS = DOWN_SET_CLASS, SET_CLIENT_INFO = DOWN_SET_CLIENT_INFO, SET_DIVISION = DOWN_SET_DIVISION,
                SET_EXP_TYPE = DOWN_SET_EXP_TYPE, SET_INST_TYPE = DOWN_SET_INST_TYPE, SET_INSTITUTE = DOWN_SET_INSTITUTE,
                SET_ORG_TYPE = DOWN_SET_ORG_TYPE, SET_PROMO_ITEM = DOWN_SET_PROMO_ITEM, SET_SPECIMEN = DOWN_SET_SPECIMEN,
                SET_SUBJECT = DOWN_SET_SUBJECT, SET_TEACHER_DESIG = DOWN_SET_TEACHER_DESIG, SET_TEACHER_INFO = DOWN_SET_TEACHER_INFO,
                SET_THANA = DOWN_SET_THANA, SET_TRANSPORT_TYPE = DOWN_SET_TRANSPORT_TYPE, SET_WORK_PURPOSE = DOWN_SET_WORK_PURPOSE,
                SET_ZILLA = DOWN_SET_ZILLA, SET_ZONE = DOWN_SET_ZONE, TRN_MSG = DOWN_TRN_MSG, TRN_USER_PROMO_DET = DOWN_TRN_USER_PROMO_DET,
                TRN_USER_PROMO_ITEM = DOWN_TRN_USER_PROMO_ITEM, TRN_USER_SPECIMEN = DOWN_TRN_USER_SPECIMEN,
                TRN_USER_SPECIMEN_DET = DOWN_TRN_USER_SPECIMEN_DET,
           };
            */
            decimal? DOWN_NO = null;
            try
            {
                DOWN_NO = db.TRN_DEVICE_DOWNLOAD_INSERT_4_0(DEVICE_ID).First().Value;
            }
            catch (Exception ex)
            {

            }

            var download_info = new { DOWN_NO = DOWN_NO.ToString() };


            var result = new
            {
                download_info,
                SET_FEEDBACK_TYPE = DOWN_SET_FEEDBACK_TYPE_OBJ_List,
                SET_LOGOUT_TYPE = DOWN_SET_LOGOUT_TYPE_OBJ_List,
                SEC_USERS = DOWN_SEC_USERS_OBJ_List,
                SEC_USER_THANA = DOWN_SEC_USER_THANA_OBJ_List,
                SET_CLASS = DOWN_SET_CLASS_OBJ_List,
                SET_CLIENT_INFO = DOWN_SET_CLIENT_INFO_OBJ_List,
                SET_DIVISION = DOWN_SET_DIVISION_OBJ_List,
                SET_EXP_TYPE = DOWN_SET_EXP_TYPE_OBJ_List,
                SET_INST_TYPE = DOWN_SET_INST_TYPE_OBJ_List,
                SET_INSTITUTE = DOWN_SET_INSTITUTE_OBJ_List,
                SET_ORG_TYPE = DOWN_SET_ORG_TYPE_OBJ_List,
                SET_PROMO_ITEM = DOWN_SET_PROMO_ITEM_OBJ_List,
                SET_SPECIMEN = DOWN_SET_SPECIMEN_OBJ_List,
                SET_SUBJECT = DOWN_SET_SUBJECT_OBJ_List,
                SET_TEACHER_DESIG = DOWN_SET_TEACHER_DESIG_OBJ_List,
                SET_TEACHER_INFO = DOWN_SET_TEACHER_INFO_OBJ_List,
                SET_THANA = DOWN_SET_THANA_OBJ_List,
                SET_TRANSPORT_TYPE = DOWN_SET_TRANSPORT_TYPE_OBJ_List,
                SET_WORK_PURPOSE = DOWN_SET_WORK_PURPOSE_OBJ_List,
                SET_ZILLA = DOWN_SET_ZILLA_OBJ_List,
                SET_ZONE = DOWN_SET_ZONE_OBJ_List,
                TRN_MSG = DOWN_TRN_MSG_OBJ_List,
                TRN_USER_PROMO_DET = DOWN_TRN_USER_PROMO_DET_OBJ_List,
                TRN_USER_PROMO_ITEM = DOWN_TRN_USER_PROMO_ITEM_OBJ_List,
                TRN_USER_SPECIMEN = DOWN_TRN_USER_SPECIMEN_DET_OBJ_List,
                TRN_USER_SPECIMEN_DET = DOWN_TRN_USER_SPECIMEN_DET_OBJ_List,
                
            };

            /*
            if (DEVICE_ID == "042009267")
                MobileLogger.logDefault(new JsonResult() {
                    Data = result,
                    ContentType = "application/json",
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                }.ToString(), "042009267");
            //var master_obj = new { @result = result };
            */

            DOWN_MASTER master_obj = new DOWN_MASTER();


            //return Json(master_obj, JsonRequestBehavior.AllowGet);
            return new JsonResult()
            {
                Data = result,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
            //return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public String set_downloaded(string DOWN_NO,string user)
        {
            try
            {
                //db.TRN_DEVICE_DOWNLOAD_INSERT(device_id);
                if (string.IsNullOrEmpty(DOWN_NO) || string.IsNullOrWhiteSpace(DOWN_NO))
                {
                    MobileLogger.logDefault("null in down no","dwonload_ack");
                }
                else
                {
                    
                    db.TRN_DEVICE_DOWNLOAD_UPDATE_4_0(decimal.Parse(DOWN_NO.Trim()));
                   // MobileLogger.logDefault(user + " user downloaded ", "dwonload_ack");
                }
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        [HttpGet]
        public JsonResult LastDownTime(string DEVICE_ID)
        {
            var data = db.DOWN_MAX_GET(DEVICE_ID).First().Value;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AllSecUsers(DateTime MAX_LAST_DOWN_TIME, string DEVICE_ID)
        {
            var data = db.DOWN_GET_SEC_USERS(MAX_LAST_DOWN_TIME, DEVICE_ID);
            var result = (from d in data
                          select new
                          {
                              USER_NO = d.USER_NO,
                              USER_TYPE_NO = d.USER_TYPE_NO,
                              HR_EMP_ID = d.HR_EMP_ID,
                              USER_FULL_NAME = d.USER_FULL_NAME,
                              USER_MOBILE = d.USER_MOBILE,
                              IS_ACTIVE = d.IS_ACTIVE,
                              ACTIVE_FROM = d.ACTIVE_FROM,
                              ACTIVE_TO = d.ACTIVE_TO
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);

            //return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public JsonResult AllSpecimen(string DEVICE_ID)
        {
            var MAX_LAST_DOWN_TIME = db.DOWN_MAX_GET(DEVICE_ID).First().Value;
            var DOWN_SET_SPECIMEN = (from s in db.DOWN_GET_SET_SPECIMEN(MAX_LAST_DOWN_TIME, DEVICE_ID)
                                     select new
                                     {
                                         SPECIMEN_NO = s.SPECIMEN_NO,
                                     }
                                     ).ToList();



            return Json(DOWN_SET_SPECIMEN, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAddTime(double addTime)
        {
            DateTime time = DateTime.Today.AddDays(addTime);
            return Json(new { @time = time }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCurrTime()
        {
            //DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            //string time = DateTime.Now.ToLongTimeString();
            //string time = (DateTime.UtcNow - Jan1st1970).TotalMilliseconds.ToString();
            //string time = (DateTime.Now - Jan1st1970).TotalMilliseconds.ToString();           
            string time = (DateTime.UtcNow - Jan1st1970).TotalMilliseconds.ToString();
            return Json(new { @time = time }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMovement(string USER_NAME, string USER_PWD, string ZM_LOGIN_NAME, DateTime? DATE_FROM, DateTime? DATE_TO)
        {

            string WS_USER_NAME = ConfigurationManager.AppSettings["WS_USER_NAME"].Trim();
            string WS_USER_PWD = ConfigurationManager.AppSettings["WS_USER_PWD"].Trim();

            if ((USER_NAME.Trim() == WS_USER_NAME) && (USER_PWD.Trim() == WS_USER_PWD))
            {
                if (string.IsNullOrEmpty(ZM_LOGIN_NAME) || string.IsNullOrWhiteSpace(ZM_LOGIN_NAME))
                {
                    throw new Exception("Zonal Login Name is required");
                }
                else
                {
                    if ((DATE_FROM != null) && (DATE_TO != null))
                    {
                        var result = (from w in db.WS_TRN_USER_MOVEMENT(ZM_LOGIN_NAME.Trim(), DATE_FROM.Value, DATE_TO.Value)
                                      select new
                                      {
                                          w.MOVE_NO,
                                          w.MOVE_DATE,
                                          w.MOVE_TIME,
                                          w.LAT_VAL,
                                          w.LON_VAL,
                                          w.BATT_PCT
                                      }
                                      ).ToList();
                        return new JsonResult()
                        {
                            //Data = result,
                            Data = result,
                            ContentType = "application/json",
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            MaxJsonLength = Int32.MaxValue
                        };
                    }
                    else
                    {
                        throw new Exception("Date From and Date To required");
                    }
                }
            }
            else
            {
                throw new Exception("Invalid user name or password");
            }



        }
    }
}

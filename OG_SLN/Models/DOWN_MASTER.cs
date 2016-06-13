using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;

namespace OG_SLN {
    public class DOWN_MASTER {
        List<Object> _objList = new List<object>();

        public List<Object> ObjList {
            get { return _objList; }
            set { _objList = value; }
        }
    }

    public class DOWN_SET_FEEDBACK_TYPE_OBJ {

        string _FEEDBACK_TYPE_NO;

        public string FEEDBACK_TYPE_NO {
            get { return _FEEDBACK_TYPE_NO; }
            set { _FEEDBACK_TYPE_NO = value; }
        }

        string _FEEDBACK_NAME;

        public string FEEDBACK_NAME {
            get { return _FEEDBACK_NAME; }
            set { _FEEDBACK_NAME = value; }
        }

        string _FEEDBACK_CODE;

        public string FEEDBACK_CODE {
            get { return _FEEDBACK_CODE; }
            set { _FEEDBACK_CODE = value; }
        }

        string _FEEDBACK_DESC;

        public string FEEDBACK_DESC {
            get { return _FEEDBACK_DESC; }
            set { _FEEDBACK_DESC = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }

    }

    public class DOWN_SEC_USERS_OBJ {
        string _USER_NO;

        public string USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        string _USER_TYPE_NO;

        public string USER_TYPE_NO {
            get { return _USER_TYPE_NO; }
            set { _USER_TYPE_NO = value; }
        }

        string _HR_EMP_ID;

        public string HR_EMP_ID {
            get { return _HR_EMP_ID; }
            set { _HR_EMP_ID = value; }
        }

        string _USER_FULL_NAME;

        public string USER_FULL_NAME {
            get { return _USER_FULL_NAME; }
            set { _USER_FULL_NAME = value; }
        }

        string _USER_MOBILE;

        public string USER_MOBILE {
            get { return _USER_MOBILE; }
            set { _USER_MOBILE = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }

    }

    public class DOWN_SEC_USER_THANA_OBJ {
        string _USER_THANA_NO;

        public string USER_THANA_NO {
            get { return _USER_THANA_NO; }
            set { _USER_THANA_NO = value; }
        }

        string _THANA_NO;

        public string THANA_NO {
            get { return _THANA_NO; }
            set { _THANA_NO = value; }
        }

        string _USER_NO;

        public string USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }


        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_CLASS_OBJ {
        string _CLASS_NO;

        public string CLASS_NO {
            get { return _CLASS_NO; }
            set { _CLASS_NO = value; }
        }

        string _CLASS_NAME;

        public string CLASS_NAME {
            get { return _CLASS_NAME; }
            set { _CLASS_NAME = value; }
        }

        string _CLASS_NAME_BNG;

        public string CLASS_NAME_BNG {
            get { return _CLASS_NAME_BNG; }
            set { _CLASS_NAME_BNG = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_CLIENT_INFO_OBJ {       

        string _CLIENT_NO;

        public string CLIENT_NO {
            get { return _CLIENT_NO; }
            set { _CLIENT_NO = value; }
        }

        string _CLIENT_NAME;

        public string CLIENT_NAME {
            get { return _CLIENT_NAME; }
            set { _CLIENT_NAME = value; }
        }

        string _CLIENT_NAME_BNG;

        public string CLIENT_NAME_BNG {
            get { return _CLIENT_NAME_BNG; }
            set { _CLIENT_NAME_BNG = value; }
        }

        string _CLIENT_NICK_NAME;

        public string CLIENT_NICK_NAME {
            get { return _CLIENT_NICK_NAME; }
            set { _CLIENT_NICK_NAME = value; }
        }

        string _CLIENT_MOBILE;

        public string CLIENT_MOBILE {
            get { return _CLIENT_MOBILE; }
            set { _CLIENT_MOBILE = value; }
        }

        string _DIVISION_NO;

        public string DIVISION_NO {
            get { return _DIVISION_NO; }
            set { _DIVISION_NO = value; }
        }

        string _ZONE_NO;

        public string ZONE_NO {
            get { return _ZONE_NO; }
            set { _ZONE_NO = value; }
        }

        string _ZILLA_NO;

        public string ZILLA_NO {
            get { return _ZILLA_NO; }
            set { _ZILLA_NO = value; }
        }

        string _THANA_NO;

        public string THANA_NO {
            get { return _THANA_NO; }
            set { _THANA_NO = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }

    }

    public class DOWN_SET_DIVISION_OBJ {

        string _DIVISION_NO;

        public string DIVISION_NO {
            get { return _DIVISION_NO; }
            set { _DIVISION_NO = value; }
        }

        string _DIVISION_NAME;

        public string DIVISION_NAME {
            get { return _DIVISION_NAME; }
            set { _DIVISION_NAME = value; }
        }

        string _DIVISION_NAME_BNG;

        public string DIVISION_NAME_BNG {
            get { return _DIVISION_NAME_BNG; }
            set { _DIVISION_NAME_BNG = value; }
        }

        string _DIVISION_CODE;

        public string DIVISION_CODE {
            get { return _DIVISION_CODE; }
            set { _DIVISION_CODE = value; }
        }

        string _ACTUAL_LATI_VAL;

        public string ACTUAL_LATI_VAL {
            get { return _ACTUAL_LATI_VAL; }
            set { _ACTUAL_LATI_VAL = value; }
        }

        string _ACTUAL_LONG_VAL;

        public string ACTUAL_LONG_VAL {
            get { return _ACTUAL_LONG_VAL; }
            set { _ACTUAL_LONG_VAL = value; }
        }

        string _FROM_LATI_VAL;

        public string FROM_LATI_VAL {
            get { return _FROM_LATI_VAL; }
            set { _FROM_LATI_VAL = value; }
        }

        string _FROM_LONG_VAL;

        public string FROM_LONG_VAL {
            get { return _FROM_LONG_VAL; }
            set { _FROM_LONG_VAL = value; }
        }

        string _TO_LATI_VAL;

        public string TO_LATI_VAL {
            get { return _TO_LATI_VAL; }
            set { _TO_LATI_VAL = value; }
        }

        string _TO_LONG_VAL;

        public string TO_LONG_VAL {
            get { return _TO_LONG_VAL; }
            set { _TO_LONG_VAL = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }

    }

    public class DOWN_SET_EXP_TYPE_OBJ {
        string _EXP_TYPE_NO;

        public string EXP_TYPE_NO {
            get { return _EXP_TYPE_NO; }
            set { _EXP_TYPE_NO = value; }
        }

        string _EXP_TYPE_CODE;

        public string EXP_TYPE_CODE {
            get { return _EXP_TYPE_CODE; }
            set { _EXP_TYPE_CODE = value; }
        }

        string _EXP_TYPE_NAME;

        public string EXP_TYPE_NAME {
            get { return _EXP_TYPE_NAME; }
            set { _EXP_TYPE_NAME = value; }
        }

        string _EXP_TYPE_DETAILS;

        public string EXP_TYPE_DETAILS {
            get { return _EXP_TYPE_DETAILS; }
            set { _EXP_TYPE_DETAILS = value; }
        }

        string _EXP_TYPE_DESC;

        public string EXP_TYPE_DESC {
            get { return _EXP_TYPE_DESC; }
            set { _EXP_TYPE_DESC = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_INSTITUTE_OBJ {
        string _INSTITUTE_NO;

        public string INSTITUTE_NO {
            get { return _INSTITUTE_NO; }
            set { _INSTITUTE_NO = value; }
        }

        string _INSTITUTE_NAME;

        public string INSTITUTE_NAME {
            get { return _INSTITUTE_NAME; }
            set { _INSTITUTE_NAME = value; }
        }

        string _INSTITUTE_NAME_BNG;

        public string INSTITUTE_NAME_BNG {
            get { return _INSTITUTE_NAME_BNG; }
            set { _INSTITUTE_NAME_BNG = value; }
        }

        string _INST_TYPE_NO;

        public string INST_TYPE_NO {
            get { return _INST_TYPE_NO; }
            set { _INST_TYPE_NO = value; }
        }

        string _THANA_NO;

        public string THANA_NO {
            get { return _THANA_NO; }
            set { _THANA_NO = value; }
        }

        string _F_INSTITUTION_DB_ID;

        public string F_INSTITUTION_DB_ID {
            get { return _F_INSTITUTION_DB_ID; }
            set { _F_INSTITUTION_DB_ID = value; }
        }

        string _EIIN_NUMBER;

        public string EIIN_NUMBER {
            get { return _EIIN_NUMBER; }
            set { _EIIN_NUMBER = value; }
        }

        string _MEET_PERSON_NAME;

        public string MEET_PERSON_NAME {
            get { return _MEET_PERSON_NAME; }
            set { _MEET_PERSON_NAME = value; }
        }

        string _CONTACT_NUMBER;

        public string CONTACT_NUMBER {
            get { return _CONTACT_NUMBER; }
            set { _CONTACT_NUMBER = value; }
        }

        string _ACTUAL_LATI_VAL;

        public string ACTUAL_LATI_VAL {
            get { return _ACTUAL_LATI_VAL; }
            set { _ACTUAL_LATI_VAL = value; }
        }

        string _ACTUAL_LONG_VAL;

        public string ACTUAL_LONG_VAL {
            get { return _ACTUAL_LONG_VAL; }
            set { _ACTUAL_LONG_VAL = value; }
        }

        string _FROM_LATI_VAL;

        public string FROM_LATI_VAL {
            get { return _FROM_LATI_VAL; }
            set { _FROM_LATI_VAL = value; }
        }

        string _FROM_LONG_VAL;

        public string FROM_LONG_VAL {
            get { return _FROM_LONG_VAL; }
            set { _FROM_LONG_VAL = value; }
        }

        string _TO_LATI_VAL;

        public string TO_LATI_VAL {
            get { return _TO_LATI_VAL; }
            set { _TO_LATI_VAL = value; }
        }

        string _TO_LONG_VAL;

        public string TO_LONG_VAL {
            get { return _TO_LONG_VAL; }
            set { _TO_LONG_VAL = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_INST_TYPE_OBJ {
        string _INST_TYPE_NO;

        public string INST_TYPE_NO {
            get { return _INST_TYPE_NO; }
            set { _INST_TYPE_NO = value; }
        }

        string _INST_TYPE_NAME;

        public string INST_TYPE_NAME {
            get { return _INST_TYPE_NAME; }
            set { _INST_TYPE_NAME = value; }
        }

        string _INST_TYPE_CODE;

        public string INST_TYPE_CODE {
            get { return _INST_TYPE_CODE; }
            set { _INST_TYPE_CODE = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }    
    }

    public class DOWN_SET_ORG_TYPE_OBJ {
        string _ORG_TYPE_NO;

        public string ORG_TYPE_NO {
            get { return _ORG_TYPE_NO; }
            set { _ORG_TYPE_NO = value; }
        }

        string _ORG_TYPE_NAME;

        public string ORG_TYPE_NAME {
            get { return _ORG_TYPE_NAME; }
            set { _ORG_TYPE_NAME = value; }
        }

        string _ORG_TYPE_CODE;

        public string ORG_TYPE_CODE {
            get { return _ORG_TYPE_CODE; }
            set { _ORG_TYPE_CODE = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_PROMO_ITEM_OBJ {
        string _PROMO_ITEM_NO;

        public string PROMO_ITEM_NO {
            get { return _PROMO_ITEM_NO; }
            set { _PROMO_ITEM_NO = value; }
        }

        string _PROMO_ITEM_NAME;

        public string PROMO_ITEM_NAME {
            get { return _PROMO_ITEM_NAME; }
            set { _PROMO_ITEM_NAME = value; }
        }

        string _PROMO_ITEM_CODE;

        public string PROMO_ITEM_CODE {
            get { return _PROMO_ITEM_CODE; }
            set { _PROMO_ITEM_CODE = value; }
        }

        string _PROMO_ITEM_TYPE_NO;

        public string PROMO_ITEM_TYPE_NO {
            get { return _PROMO_ITEM_TYPE_NO; }
            set { _PROMO_ITEM_TYPE_NO = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }  
    }

    public class DOWN_SET_SPECIMEN_OBJ {
        string _SPECIMEN_NO;

        public string SPECIMEN_NO {
            get { return _SPECIMEN_NO; }
            set { _SPECIMEN_NO = value; }
        }

        string _SPECIMEN_NAME;

        public string SPECIMEN_NAME {
            get { return _SPECIMEN_NAME; }
            set { _SPECIMEN_NAME = value; }
        }

        string _SPECIMEN_NAME_BNG;

        public string SPECIMEN_NAME_BNG {
            get { return _SPECIMEN_NAME_BNG; }
            set { _SPECIMEN_NAME_BNG = value; }
        }

        string _SPECIMEN_CODE;

        public string SPECIMEN_CODE {
            get { return _SPECIMEN_CODE; }
            set { _SPECIMEN_CODE = value; }
        }

        string _BOOK_TYPE_NO;

        public string BOOK_TYPE_NO {
            get { return _BOOK_TYPE_NO; }
            set { _BOOK_TYPE_NO = value; }
        }

        string _BOOK_UNIQUE_CODE;

        public string BOOK_UNIQUE_CODE {
            get { return _BOOK_UNIQUE_CODE; }
            set { _BOOK_UNIQUE_CODE = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        } 
    }

    public class DOWN_SET_SUBJECT_OBJ {
        string _SUBJECT_NO;

        public string SUBJECT_NO {
            get { return _SUBJECT_NO; }
            set { _SUBJECT_NO = value; }
        }

        string _SUBJECT_NAME;

        public string SUBJECT_NAME {
            get { return _SUBJECT_NAME; }
            set { _SUBJECT_NAME = value; }
        }

        string _SUBJECT_NAME_BNG;

        public string SUBJECT_NAME_BNG {
            get { return _SUBJECT_NAME_BNG; }
            set { _SUBJECT_NAME_BNG = value; }
        }

        string _SUBJECT_CODE;

        public string SUBJECT_CODE {
            get { return _SUBJECT_CODE; }
            set { _SUBJECT_CODE = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }   
    }

    public class DOWN_SET_TEACHER_DESIG_OBJ {
        string _TEACH_DESIG_NO;

        public string TEACH_DESIG_NO {
            get { return _TEACH_DESIG_NO; }
            set { _TEACH_DESIG_NO = value; }
        }

        string _TEACHER_DESIG_NAME;

        public string TEACHER_DESIG_NAME {
            get { return _TEACHER_DESIG_NAME; }
            set { _TEACHER_DESIG_NAME = value; }
        }

        string _TEACHER_DESIG_NAME_BNG;

        public string TEACHER_DESIG_NAME_BNG {
            get { return _TEACHER_DESIG_NAME_BNG; }
            set { _TEACHER_DESIG_NAME_BNG = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_TEACHER_INFO_OBJ {
        string _TEACHER_NO;

        public string TEACHER_NO {
            get { return _TEACHER_NO; }
            set { _TEACHER_NO = value; }
        }

        string _TEACHER_NAME;

        public string TEACHER_NAME {
            get { return _TEACHER_NAME; }
            set { _TEACHER_NAME = value; }
        }

        string _TEACHER_NAME_BNG;

        public string TEACHER_NAME_BNG {
            get { return _TEACHER_NAME_BNG; }
            set { _TEACHER_NAME_BNG = value; }
        }

        string _TEACHER_NICK_NAME;

        public string TEACHER_NICK_NAME {
            get { return _TEACHER_NICK_NAME; }
            set { _TEACHER_NICK_NAME = value; }
        }

        string _TEACHER_MOBILE;

        public string TEACHER_MOBILE {
            get { return _TEACHER_MOBILE; }
            set { _TEACHER_MOBILE = value; }
        }

        string _INSTITUTE_NO;

        public string INSTITUTE_NO {
            get { return _INSTITUTE_NO; }
            set { _INSTITUTE_NO = value; }
        }

        string _TEACH_DESIG_NO;

        public string TEACH_DESIG_NO {
            get { return _TEACH_DESIG_NO; }
            set { _TEACH_DESIG_NO = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        } 
    }

    public class DOWN_SET_THANA_OBJ {
        string _THANA_NO;

        public string THANA_NO {
            get { return _THANA_NO; }
            set { _THANA_NO = value; }
        }

        string _ZONE_NO;

        public string ZONE_NO {
            get { return _ZONE_NO; }
            set { _ZONE_NO = value; }
        }

        string _ZILLA_NO;

        public string ZILLA_NO {
            get { return _ZILLA_NO; }
            set { _ZILLA_NO = value; }
        }

        string _THANA_NAME;

        public string THANA_NAME {
            get { return _THANA_NAME; }
            set { _THANA_NAME = value; }
        }

        string _THANA_NAME_BNG;

        public string THANA_NAME_BNG {
            get { return _THANA_NAME_BNG; }
            set { _THANA_NAME_BNG = value; }
        }

        string _THANA_CODE;

        public string THANA_CODE {
            get { return _THANA_CODE; }
            set { _THANA_CODE = value; }
        }

        string _ACTUAL_LATI_VAL;

        public string ACTUAL_LATI_VAL {
            get { return _ACTUAL_LATI_VAL; }
            set { _ACTUAL_LATI_VAL = value; }
        }

        string _ACTUAL_LONG_VAL;

        public string ACTUAL_LONG_VAL {
            get { return _ACTUAL_LONG_VAL; }
            set { _ACTUAL_LONG_VAL = value; }
        }

        string _FROM_LATI_VAL;

        public string FROM_LATI_VAL {
            get { return _FROM_LATI_VAL; }
            set { _FROM_LATI_VAL = value; }
        }

        string _FROM_LONG_VAL;

        public string FROM_LONG_VAL {
            get { return _FROM_LONG_VAL; }
            set { _FROM_LONG_VAL = value; }
        }

        string _TO_LATI_VAL;

        public string TO_LATI_VAL {
            get { return _TO_LATI_VAL; }
            set { _TO_LATI_VAL = value; }
        }

        string _TO_LONG_VAL;

        public string TO_LONG_VAL {
            get { return _TO_LONG_VAL; }
            set { _TO_LONG_VAL = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        } 

    }

    public class DOWN_SET_TRANSPORT_TYPE_OBJ {
        string _TRANS_TYPE_NO;

        public string TRANS_TYPE_NO {
            get { return _TRANS_TYPE_NO; }
            set { _TRANS_TYPE_NO = value; }
        }

        string _TRANS_TYPE_NAME;

        public string TRANS_TYPE_NAME {
            get { return _TRANS_TYPE_NAME; }
            set { _TRANS_TYPE_NAME = value; }
        }

        string _TRANS_TYPE_CODE;

        public string TRANS_TYPE_CODE {
            get { return _TRANS_TYPE_CODE; }
            set { _TRANS_TYPE_CODE = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_WORK_PURPOSE_OBJ {
        string _WORK_PUR_NO;

        public string WORK_PUR_NO {
            get { return _WORK_PUR_NO; }
            set { _WORK_PUR_NO = value; }
        }

        string _WORK_PUR_NAME;

        public string WORK_PUR_NAME {
            get { return _WORK_PUR_NAME; }
            set { _WORK_PUR_NAME = value; }
        }

        string _WORK_PUR_CODE;

        public string WORK_PUR_CODE {
            get { return _WORK_PUR_CODE; }
            set { _WORK_PUR_CODE = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        } 
    }

    public class DOWN_SET_ZILLA_OBJ {
        string _ZILLA_NO;

        public string ZILLA_NO {
            get { return _ZILLA_NO; }
            set { _ZILLA_NO = value; }
        }

        string _DIVISION_NO;

        public string DIVISION_NO {
            get { return _DIVISION_NO; }
            set { _DIVISION_NO = value; }
        }

        string _ZILLA_NAME;

        public string ZILLA_NAME {
            get { return _ZILLA_NAME; }
            set { _ZILLA_NAME = value; }
        }

        string _ZILLA_NAME_BNG;

        public string ZILLA_NAME_BNG {
            get { return _ZILLA_NAME_BNG; }
            set { _ZILLA_NAME_BNG = value; }
        }

        string _ZILLA_CODE;

        public string ZILLA_CODE {
            get { return _ZILLA_CODE; }
            set { _ZILLA_CODE = value; }
        }

        string _ACTUAL_LATI_VAL;

        public string ACTUAL_LATI_VAL {
            get { return _ACTUAL_LATI_VAL; }
            set { _ACTUAL_LATI_VAL = value; }
        }

        string _ACTUAL_LONG_VAL;

        public string ACTUAL_LONG_VAL {
            get { return _ACTUAL_LONG_VAL; }
            set { _ACTUAL_LONG_VAL = value; }
        }

        string _FROM_LATI_VAL;

        public string FROM_LATI_VAL {
            get { return _FROM_LATI_VAL; }
            set { _FROM_LATI_VAL = value; }
        }

        string _FROM_LONG_VAL;

        public string FROM_LONG_VAL {
            get { return _FROM_LONG_VAL; }
            set { _FROM_LONG_VAL = value; }
        }

        string _TO_LATI_VAL;

        public string TO_LATI_VAL {
            get { return _TO_LATI_VAL; }
            set { _TO_LATI_VAL = value; }
        }

        string _TO_LONG_VAL;

        public string TO_LONG_VAL {
            get { return _TO_LONG_VAL; }
            set { _TO_LONG_VAL = value; }
        }

        string _SL_NUM;

        public string SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }  
    }

    public class DOWN_SET_ZONE_OBJ {
        string _ZONE_NO;

        public string ZONE_NO {
            get { return _ZONE_NO; }
            set { _ZONE_NO = value; }
        }

        string _DIVISION_NO;

        public string DIVISION_NO {
            get { return _DIVISION_NO; }
            set { _DIVISION_NO = value; }
        }

        string _ZONE_NAME;

        public string ZONE_NAME {
            get { return _ZONE_NAME; }
            set { _ZONE_NAME = value; }
        }

        string _ZONE_NAME_BNG;

        public string ZONE_NAME_BNG {
            get { return _ZONE_NAME_BNG; }
            set { _ZONE_NAME_BNG = value; }
        }

        string _ZONE_CODE;

        public string ZONE_CODE {
            get { return _ZONE_CODE; }
            set { _ZONE_CODE = value; }
        }

        string _ZONE_GRADE;

        public string ZONE_GRADE {
            get { return _ZONE_GRADE; }
            set { _ZONE_GRADE = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        string _ACTIVE_FROM;

        public string ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        string _ACTIVE_TO;

        public string ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }
    }

    public class DOWN_SET_LOGOUT_TYPE_OBJ {

        string _LOGOUT_TYPE_NO;

        public string LOGOUT_TYPE_NO {
            get { return _LOGOUT_TYPE_NO; }
            set { _LOGOUT_TYPE_NO = value; }
        }

        string _LOGOUT_TYPE_NAME;

        public string LOGOUT_TYPE_NAME {
            get { return _LOGOUT_TYPE_NAME; }
            set { _LOGOUT_TYPE_NAME = value; }
        }

    }

    public class DOWN_TRN_MSG_OBJ {
        string _MSG_NO;

        public string MSG_NO {
            get { return _MSG_NO; }
            set { _MSG_NO = value; }
        }

        string _FROM_USER_NO;

        public string FROM_USER_NO {
            get { return _FROM_USER_NO; }
            set { _FROM_USER_NO = value; }
        }

        string _USER_TYPE_NO;

        public string USER_TYPE_NO {
            get { return _USER_TYPE_NO; }
            set { _USER_TYPE_NO = value; }
        }

        string _USER_FULL_NAME;

        public string USER_FULL_NAME {
            get { return _USER_FULL_NAME; }
            set { _USER_FULL_NAME = value; }
        }

        string _USER_MOBILE;

        public string USER_MOBILE {
            get { return _USER_MOBILE; }
            set { _USER_MOBILE = value; }
        }

        string _USER_TYPE;

        public string USER_TYPE {
            get { return _USER_TYPE; }
            set { _USER_TYPE = value; }
        }

        string _MSG_SUBJECT;

        public string MSG_SUBJECT {
            get { return _MSG_SUBJECT; }
            set { _MSG_SUBJECT = value; }
        }

        string _MSG_BODY;

        public string MSG_BODY {
            get { return _MSG_BODY; }
            set { _MSG_BODY = value; }
        }

        string _IS_REPLY_ALLOW;

        public string IS_REPLY_ALLOW {
            get { return _IS_REPLY_ALLOW; }
            set { _IS_REPLY_ALLOW = value; }
        }

        string _REF_MSG_NO;

        public string REF_MSG_NO {
            get { return _REF_MSG_NO; }
            set { _REF_MSG_NO = value; }
        }

        string _IS_REPLY;

        public string IS_REPLY {
            get { return _IS_REPLY; }
            set { _IS_REPLY = value; }
        }

        string _MSG_REC_NO;

        public string MSG_REC_NO {
            get { return _MSG_REC_NO; }
            set { _MSG_REC_NO = value; }
        }

        string _REC_USER_NO;

        public string REC_USER_NO {
            get { return _REC_USER_NO; }
            set { _REC_USER_NO = value; }
        }

        string _IS_READ;

        public string IS_READ {
            get { return _IS_READ; }
            set { _IS_READ = value; }
        }

        string _INSERT_TIME;

        public string INSERT_TIME {
            get { return _INSERT_TIME; }
            set { _INSERT_TIME = value; }
        }
    }

    public class DOWN_TRN_USER_PROMO_DET_OBJ {
        string _USER_PROMO_DET_NO;

        public string USER_PROMO_DET_NO {
            get { return _USER_PROMO_DET_NO; }
            set { _USER_PROMO_DET_NO = value; }
        }

        string _USER_PROMO_NO;

        public string USER_PROMO_NO {
            get { return _USER_PROMO_NO; }
            set { _USER_PROMO_NO = value; }
        }

        string _PROMO_ITEM_CODE;

        public string PROMO_ITEM_CODE {
            get { return _PROMO_ITEM_CODE; }
            set { _PROMO_ITEM_CODE = value; }
        }

        string _PROMO_ITEM_NAME;

        public string PROMO_ITEM_NAME {
            get { return _PROMO_ITEM_NAME; }
            set { _PROMO_ITEM_NAME = value; }
        }

        string _PROMO_ITEM_NO;

        public string PROMO_ITEM_NO {
            get { return _PROMO_ITEM_NO; }
            set { _PROMO_ITEM_NO = value; }
        }

        string _QTY;

        public string QTY {
            get { return _QTY; }
            set { _QTY = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }
    }

    public class DOWN_TRN_USER_PROMO_ITEM_OBJ {
        string _USER_PROMO_NO;

        public string USER_PROMO_NO {
            get { return _USER_PROMO_NO; }
            set { _USER_PROMO_NO = value; }
        }

        string _USER_NO;

        public string USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        string _ASSIGN_DATE;

        public string ASSIGN_DATE {
            get { return _ASSIGN_DATE; }
            set { _ASSIGN_DATE = value; }
        }

        string _TARGET_DATE_FROM;

        public string TARGET_DATE_FROM {
            get { return _TARGET_DATE_FROM; }
            set { _TARGET_DATE_FROM = value; }
        }

        string _TARGET_DATE_TO;

        public string TARGET_DATE_TO {
            get { return _TARGET_DATE_TO; }
            set { _TARGET_DATE_TO = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }
    }

    public class DOWN_TRN_USER_SPECIMEN_OBJ {
        string _USER_SPECIMEN_NO;

        public string USER_SPECIMEN_NO {
            get { return _USER_SPECIMEN_NO; }
            set { _USER_SPECIMEN_NO = value; }
        }

        string _USER_NO;

        public string USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        string _ASSIGN_DATE;

        public string ASSIGN_DATE {
            get { return _ASSIGN_DATE; }
            set { _ASSIGN_DATE = value; }
        }

        string _TARGET_DATE_FROM;

        public string TARGET_DATE_FROM {
            get { return _TARGET_DATE_FROM; }
            set { _TARGET_DATE_FROM = value; }
        }

        string _TARGET_DATE_TO;

        public string TARGET_DATE_TO {
            get { return _TARGET_DATE_TO; }
            set { _TARGET_DATE_TO = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        } 
    }

    public class DOWN_TRN_USER_SPECIMEN_DET_OBJ {
        string _SPECIMEN_DET_NO;

        public string SPECIMEN_DET_NO {
            get { return _SPECIMEN_DET_NO; }
            set { _SPECIMEN_DET_NO = value; }
        }

        string _USER_SPECIMEN_NO;

        public string USER_SPECIMEN_NO {
            get { return _USER_SPECIMEN_NO; }
            set { _USER_SPECIMEN_NO = value; }
        }

        string _SPECIMEN_CODE;

        public string SPECIMEN_CODE {
            get { return _SPECIMEN_CODE; }
            set { _SPECIMEN_CODE = value; }
        }

        string _SPECIMEN_NAME;

        public string SPECIMEN_NAME {
            get { return _SPECIMEN_NAME; }
            set { _SPECIMEN_NAME = value; }
        }

        string _SPECIMEN_NAME_BNG;

        public string SPECIMEN_NAME_BNG {
            get { return _SPECIMEN_NAME_BNG; }
            set { _SPECIMEN_NAME_BNG = value; }
        }

        string _SPECIMEN_NO;

        public string SPECIMEN_NO {
            get { return _SPECIMEN_NO; }
            set { _SPECIMEN_NO = value; }
        }

        string _QTY;

        public string QTY {
            get { return _QTY; }
            set { _QTY = value; }
        }

        string _IS_ACTIVE;

        public string IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

    }


}
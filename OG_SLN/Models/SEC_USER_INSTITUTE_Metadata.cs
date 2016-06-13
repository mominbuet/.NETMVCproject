using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN {
    public class SEC_USER_INSTITUTE_Metadata {
    }

    [Serializable]
    public partial class SecUserInstituteSave {

        decimal? _INSERT_USER_NO;

        public decimal? INSERT_USER_NO {
            get { return _INSERT_USER_NO; }
            set { _INSERT_USER_NO = value; }
        }

        decimal? _INSERT_LOGON_NO;

        public decimal? INSERT_LOGON_NO {
            get { return _INSERT_LOGON_NO; }
            set { _INSERT_LOGON_NO = value; }
        }

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _INSTITUTE_NO;

        public decimal? INSTITUTE_NO {
            get { return _INSTITUTE_NO; }
            set { _INSTITUTE_NO = value; }
        }

        decimal? _IS_ACTIVE;

        public decimal? IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

        DateTime? _ACTIVE_FROM;

        public DateTime? ACTIVE_FROM {
            get { return _ACTIVE_FROM; }
            set { _ACTIVE_FROM = value; }
        }

        DateTime? _ACTIVE_TO;

        public DateTime? ACTIVE_TO {
            get { return _ACTIVE_TO; }
            set { _ACTIVE_TO = value; }
        }

        decimal? _SL_NUM;

        public decimal? SL_NUM {
            get { return _SL_NUM; }
            set { _SL_NUM = value; }
        }

        List<decimal?> _INSTITUE_LIST = new List<decimal?>();

        public List<decimal?> INSTITUE_LIST {
            get { return _INSTITUE_LIST; }
            set { _INSTITUE_LIST = value; }
        }

    }

    [Serializable]
    public partial class InstituteSearch {

        decimal? _INSTITUTE_NO;

        public decimal? INSTITUTE_NO {
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

        decimal? _INST_TYPE_NO;

        public decimal? INST_TYPE_NO {
            get { return _INST_TYPE_NO; }
            set { _INST_TYPE_NO = value; }
        }

        decimal _THANA_NO;

        public decimal THANA_NO {
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

        decimal _USER_NO;

        public decimal USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

    }

    [Serializable]
    public partial class SecUserInstituteSearch {

        decimal? _USER_INSTITUTE_NO;

        public decimal? USER_INSTITUTE_NO {
            get { return _USER_INSTITUTE_NO; }
            set { _USER_INSTITUTE_NO = value; }
        }

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _INSTITUTE_NO;

        public decimal? INSTITUTE_NO {
            get { return _INSTITUTE_NO; }
            set { _INSTITUTE_NO = value; }
        }

        decimal? _IS_ACTIVE;

        public decimal? IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }

    }
    
}
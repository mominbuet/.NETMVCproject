using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN {
    
    public class TRN_LOG_INFO_Metadata {
    }

    [Serializable]
    public class TRN_LOG_INFO_BACK_MASTER {
        List<TRN_LOG_INFO_BACK_UP> _TRN_LOG_INFO_BACK_UP;

        public List<TRN_LOG_INFO_BACK_UP> TRN_LOG_INFO_BACK_UP {
            get { return _TRN_LOG_INFO_BACK_UP; }
            set { _TRN_LOG_INFO_BACK_UP = value; }
        }

        USER_INFO _USER_INFO;

        public USER_INFO USER_INFO {
            get { return _USER_INFO; }
            set { _USER_INFO = value; }
        }
    }


    [Serializable]
    public class TRN_LOG_INFO_BACK_UP {

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

        decimal? _LOG_INFO_NO;

        public decimal? LOG_INFO_NO {
            get { return _LOG_INFO_NO; }
            set { _LOG_INFO_NO = value; }
        }

        decimal? _LOGOUT_TYPE_NO;

        public decimal? LOGOUT_TYPE_NO {
            get { return _LOGOUT_TYPE_NO; }
            set { _LOGOUT_TYPE_NO = value; }
        }

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        string _LOG_IN_LOCATION_NAME;

        public string LOG_IN_LOCATION_NAME {
            get { return _LOG_IN_LOCATION_NAME; }
            set { _LOG_IN_LOCATION_NAME = value; }
        }

        decimal? _LOG_IN_LAT;

        public decimal? LOG_IN_LAT {
            get { return _LOG_IN_LAT; }
            set { _LOG_IN_LAT = value; }
        }

        decimal? _LOG_IN_LONG;

        public decimal? LOG_IN_LONG {
            get { return _LOG_IN_LONG; }
            set { _LOG_IN_LONG = value; }
        }

        DateTime? _LOG_IN_TIME;

        public DateTime? LOG_IN_TIME {
            get { return _LOG_IN_TIME; }
            set { _LOG_IN_TIME = value; }
        }

        string _LOG_OUT_LOCATION_NAME;

        public string LOG_OUT_LOCATION_NAME {
            get { return _LOG_OUT_LOCATION_NAME; }
            set { _LOG_OUT_LOCATION_NAME = value; }
        }

        decimal? _LOG_OUT_LAT;

        public decimal? LOG_OUT_LAT {
            get { return _LOG_OUT_LAT; }
            set { _LOG_OUT_LAT = value; }
        }

        decimal? _LOG_OUT_LONG;

        public decimal? LOG_OUT_LONG {
            get { return _LOG_OUT_LONG; }
            set { _LOG_OUT_LONG = value; }
        }

        DateTime? _LOG_OUT_TIME;

        public DateTime? LOG_OUT_TIME {
            get { return _LOG_OUT_TIME; }
            set { _LOG_OUT_TIME = value; }
        }

        string _LOG_OUT_MESSAGE;

        public string LOG_OUT_MESSAGE {
            get { return _LOG_OUT_MESSAGE; }
            set { _LOG_OUT_MESSAGE = value; }
        }

    }
}
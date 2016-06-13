using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;


namespace OG_SLN {

    [MetadataType(typeof(TRN_USER_LOCATION_Metadata))]
    public partial class TRN_USER_LOCATION {
        private decimal _ENTRY_STATE;

        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }
    }


    public class TRN_USER_LOCATION_Metadata {
    }

    [Serializable]
    public partial class TRN_USER_LOCATION_MASTER {
        List<TRN_USER_LOCATION_UP> _TRN_USER_LOCATION_UP;

        public List<TRN_USER_LOCATION_UP> TRN_USER_LOCATION_UP {
            get { return _TRN_USER_LOCATION_UP; }
            set { _TRN_USER_LOCATION_UP = value; }
        }

        USER_INFO _USER_INFO;

        public USER_INFO USER_INFO {
            get { return _USER_INFO; }
            set { _USER_INFO = value; }
        }
    } 

    [Serializable]
    public partial class TRN_USER_LOCATION_UP {

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        DateTime _ACTION_OFFLINE_TIME;

        public DateTime ACTION_OFFLINE_TIME {
            get { return _ACTION_OFFLINE_TIME; }
            set { _ACTION_OFFLINE_TIME = value; }
        }

        decimal? _LAT_VAL;

        public decimal? LAT_VAL {
            get { return _LAT_VAL; }
            set { _LAT_VAL = value; }
        }

        decimal? _LON_VAL;

        public decimal? LON_VAL {
            get { return _LON_VAL; }
            set { _LON_VAL = value; }
        }

        string _LOCATION_NAME;

        public string LOCATION_NAME {
            get { return _LOCATION_NAME; }
            set { _LOCATION_NAME = value; }
        }

        decimal? _OFFLINE_LOC_NO;

        public decimal? OFFLINE_LOC_NO {
            get { return _OFFLINE_LOC_NO; }
            set { _OFFLINE_LOC_NO = value; }
        }

        private decimal _ENTRY_STATE;

        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;


namespace OG_SLN {

    [MetadataType(typeof(TRN_USER_MOVEMENTS_Metadata))]
    public partial class TRN_USER_MOVEMENTS {
    }

    public class TRN_USER_MOVEMENTS_Metadata {
    }

    [Serializable]
    public partial class TRN_USER_MOVEMENTS_UP {

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        DateTime? _MOVE_DATE;

        public DateTime? MOVE_DATE {
            get { return _MOVE_DATE; }
            set { _MOVE_DATE = value; }
        }

        DateTime? _MOVE_TIME;

        public DateTime? MOVE_TIME {
            get { return _MOVE_TIME; }
            set { _MOVE_TIME = value; }
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

        decimal? _BATT_PCT;

        public decimal? BATT_PCT {
            get { return _BATT_PCT; }
            set { _BATT_PCT = value; }
        }
    }

    [Serializable]
    public partial class TRN_USER_MOVEMENTS_MASTER {
        List<TRN_USER_MOVEMENTS_UP> _TRN_USER_MOVEMENTS_UP;

        public List<TRN_USER_MOVEMENTS_UP> TRN_USER_MOVEMENTS_UP {
            get { return _TRN_USER_MOVEMENTS_UP; }
            set { _TRN_USER_MOVEMENTS_UP = value; }
        }

        USER_INFO _USER_INFO;

        public USER_INFO USER_INFO {
            get { return _USER_INFO; }
            set { _USER_INFO = value; }
        }
    }
}
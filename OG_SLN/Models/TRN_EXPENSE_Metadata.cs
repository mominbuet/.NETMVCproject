using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;


namespace OG_SLN {

    [MetadataType(typeof(TRN_EXPENSE_Metadata))]
    public partial class TRN_EXPENSE {
        private decimal _ENTRY_STATE;

        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }
        public string Remarks_desc
        {
            get
            {
                return REAMRKS;
            }
            set
            {
                REAMRKS = value;
            }
        }
    }

    public class TRN_EXPENSE_Metadata {
    }

    [MetadataType(typeof(TRN_EXPENSE_DET_Metadata))]
    public partial class TRN_EXPENSE_DET {
        private decimal _ENTRY_STATE;

        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }
        [Required]
        [Display(Name = "Remarks ")]
        [DataType(DataType.MultilineText)]
        public string Remarks_desc { get; set; }
    }

    public class TRN_EXPENSE_DET_Metadata {
    }

    [Serializable]
    public partial class TRN_EXPENSE_UP_MASTER {
        List<TRN_EXPENSE_UP> _TRN_EXPENSE_UP;

        public List<TRN_EXPENSE_UP> TRN_EXPENSE_UP {
            get { return _TRN_EXPENSE_UP; }
            set { _TRN_EXPENSE_UP = value; }
        }

        USER_INFO _USER_INFO;

        public USER_INFO USER_INFO {
            get { return _USER_INFO; }
            set { _USER_INFO = value; }
        }
    }

    [Serializable]
    public partial class TRN_EXPENSE_UP {

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        DateTime? _ACTION_OFFLINE_TIME;

        public DateTime? ACTION_OFFLINE_TIME {
            get { return _ACTION_OFFLINE_TIME; }
            set { _ACTION_OFFLINE_TIME = value; }
        }

        decimal? _ACTION_OFFLINE_SYNC;

        public decimal? ACTION_OFFLINE_SYNC {
            get { return _ACTION_OFFLINE_SYNC; }
            set { _ACTION_OFFLINE_SYNC = value; }
        }

        decimal? _ACTION_IS_OFFLINE;

        public decimal? ACTION_IS_OFFLINE {
            get { return _ACTION_IS_OFFLINE; }
            set { _ACTION_IS_OFFLINE = value; }
        }

        DateTime? _TRN_EXP_DATE;

        public DateTime? TRN_EXP_DATE {
            get { return _TRN_EXP_DATE; }
            set { _TRN_EXP_DATE = value; }
        }

        decimal? _IS_MANUAL_ENTRY;

        public decimal? IS_MANUAL_ENTRY {
            get { return _IS_MANUAL_ENTRY; }
            set { _IS_MANUAL_ENTRY = value; }
        }

        decimal? _OFFLINE_EXP_NO;

        public decimal? OFFLINE_EXP_NO {
            get { return _OFFLINE_EXP_NO; }
            set { _OFFLINE_EXP_NO = value; }
        }

        decimal? _ENTRY_STATE;

        public decimal? ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }

        List<TRN_EXPENSE_DET_UP> _TRN_EXPENSE_DET_UP;

        public List<TRN_EXPENSE_DET_UP> TRN_EXPENSE_DET_UP {
            get { return _TRN_EXPENSE_DET_UP; }
            set { _TRN_EXPENSE_DET_UP = value; }
        }

    }

    [Serializable]
    public partial class TRN_EXPENSE_DET_UP {
        DateTime? _ACTION_OFFLINE_TIME;

        public DateTime? ACTION_OFFLINE_TIME {
            get { return _ACTION_OFFLINE_TIME; }
            set { _ACTION_OFFLINE_TIME = value; }
        }

        decimal? _ACTION_OFFLINE_SYNC;

        public decimal? ACTION_OFFLINE_SYNC {
            get { return _ACTION_OFFLINE_SYNC; }
            set { _ACTION_OFFLINE_SYNC = value; }
        }

        decimal? _ACTION_IS_OFFLINE;

        public decimal? ACTION_IS_OFFLINE {
            get { return _ACTION_IS_OFFLINE; }
            set { _ACTION_IS_OFFLINE = value; }
        }

        decimal? _EXP_TYPE_NO;

        public decimal? EXP_TYPE_NO {
            get { return _EXP_TYPE_NO; }
            set { _EXP_TYPE_NO = value; }
        }

        decimal? _EXP_AMT;

        public decimal? EXP_AMT {
            get { return _EXP_AMT; }
            set { _EXP_AMT = value; }
        }        

        decimal? _OFFLINE_EXP_NO;

        public decimal? OFFLINE_EXP_NO {
            get { return _OFFLINE_EXP_NO; }
            set { _OFFLINE_EXP_NO = value; }
        }

        decimal? _OFFLINE_EXP_DET_NO;

        public decimal? OFFLINE_EXP_DET_NO {
            get { return _OFFLINE_EXP_DET_NO; }
            set { _OFFLINE_EXP_DET_NO = value; }
        }

        private decimal? _LAT_VAL;

        public decimal? LAT_VAL {
            get { return _LAT_VAL; }
            set { _LAT_VAL = value; }
        }

        private decimal? _LON_VAL;

        public decimal? LON_VAL {
            get { return _LON_VAL; }
            set { _LON_VAL = value; }
        }

        private string _VENDOR;

        public string VENDOR {
            get { return _VENDOR; }
            set { _VENDOR = value; }
        }

        private string _COMMENTS;

        public string COMMENTS {
            get { return _COMMENTS; }
            set { _COMMENTS = value; }
        }

        decimal? _ENTRY_STATE;

        public decimal? ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }

    }

}
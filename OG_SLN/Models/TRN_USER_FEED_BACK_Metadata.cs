using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN {

    [MetadataType(typeof(TRN_USER_FEED_BACK_Metadata))]
    public partial class TRN_USER_FEED_BACK
    {
        public bool READ_STATUS
        {
            get
            {
                return IS_READ == 1;
            }
            set
            {
                IS_READ = value ? 1 : 0;
            }
        }
    }

    public class TRN_USER_FEED_BACK_Metadata {
        [Display(Name = "Message")]
        public string MESSAGE { get; set; }

        [Display(Name = "Is Read")]
        public decimal READ_STATUS { get; set; }
    }

    [Serializable]
    public class TRN_USER_FEED_BACK_MASTER {
        List<TRN_USER_FEED_BACK_UP> _TRN_USER_FEED_BACK_UP;

        public List<TRN_USER_FEED_BACK_UP> TRN_USER_FEED_BACK_UP {
            get { return _TRN_USER_FEED_BACK_UP; }
            set { _TRN_USER_FEED_BACK_UP = value; }
        }

        USER_INFO _USER_INFO;

        public USER_INFO USER_INFO {
            get { return _USER_INFO; }
            set { _USER_INFO = value; }
        }
    }

    [Serializable]
    public class TRN_USER_FEED_BACK_UP {

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _FEED_BACK_NO;

        public decimal? FEED_BACK_NO {
            get { return _FEED_BACK_NO; }
            set { _FEED_BACK_NO = value; }
        }

        DateTime _ACTION_OFFLINE_TIME;

        public DateTime ACTION_OFFLINE_TIME {
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

        decimal? _FEEDBACK_TYPE_NO;

        public decimal? FEEDBACK_TYPE_NO {
            get { return _FEEDBACK_TYPE_NO; }
            set { _FEEDBACK_TYPE_NO = value; }
        }

        string _MESSAGE;

        public string MESSAGE {
            get { return _MESSAGE; }
            set { _MESSAGE = value; }
        }

        decimal? _OFFLINE_FEEDBACK_NO;

        public decimal? OFFLINE_FEEDBACK_NO {
            get { return _OFFLINE_FEEDBACK_NO; }
            set { _OFFLINE_FEEDBACK_NO = value; }
        }


        decimal? _ENTRY_STATE;

        public decimal? ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }

    }
}
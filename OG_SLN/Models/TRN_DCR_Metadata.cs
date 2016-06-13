using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;

namespace OG_SLN {

    [MetadataType(typeof(TRN_DCR_Metadata))]
    public partial class TRN_DCR {
        public decimal edit_ID { get; set; }

        public decimal user_ID { get; set; }
        public string txtINS { get; set; }
        private decimal _ENTRY_STATE;

        public string FROM_TIME { get; set; }

        public string TO_TIME { get; set; }
        public bool ZM_MOBILE_IS
        {
            get
            {
                return IS_REF_ZM == 1;
            }
            set
            {
                IS_REF_ZM = value ? 1 : 0;
            }
        }
        public bool MANUAL_ENTRY_IS
        {
            get
            {
                return IS_MANUAL_ENTRY == 1;
            }
            set
            {
                IS_MANUAL_ENTRY = value ? 1 : 0;
            }
        }
        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }
    }

    public class TRN_DCR_Metadata {
        [Display(Name = "Edit Mode")]
        public decimal edit_ID { get; set; }
        [Display(Name = "USER ID")]
        public decimal user_ID { get; set; }
        [Display(Name = "From time")]
        public string FROM_TIME { get; set; }
        [Display(Name = "To time")]
        public string TO_TIME { get; set; }
        [Display(Name = "Ref Zonal Manager")]
        public bool ZM_MOBILE_IS { get; set; }
        [Display(Name = "Manual Entry")]
        public bool MANUAL_ENTRY_IS { get; set; }
        [Required]
        [Display(Name = "Institute name is required")]
        public bool txtINS { get; set; }
    }

    [MetadataType(typeof(TRN_DCR_DET_Metadata))]
    public partial class TRN_DCR_DET {
        private decimal _ENTRY_STATE;
        public bool FOR_TEACHER
        {
            get
            {
                return IS_FOR_TEACHER == 1;
            }
            set
            {
                IS_FOR_TEACHER = value ? 1 : 0;
            }
        }
        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }
    }

    public class TRN_DCR_DET_Metadata {
        [Required]
        [Display(Name = "For Teacher")]
        public bool FOR_TEACHER { get; set; }

    }


    [Serializable]
    public partial class TRN_DCR_MASTER {
        List<TRN_DCR_UP> _TRN_DCR_UP;

        public List<TRN_DCR_UP> TRN_DCR_UP {
            get { return _TRN_DCR_UP; }
            set { _TRN_DCR_UP = value; }
        }

        USER_INFO _USER_INFO;

        public USER_INFO USER_INFO {
            get { return _USER_INFO; }
            set { _USER_INFO = value; }
        }
    }

    [Serializable]
    public partial class TRN_DCR_DET_MASTER {
        List<TRN_DCR_DET_UP> _TRN_DCR_DET_UP;

        public List<TRN_DCR_DET_UP> TRN_DCR_DET_UP {
            get { return _TRN_DCR_DET_UP; }
            set { _TRN_DCR_DET_UP = value; }
        }


        USER_INFO _USER_INFO;

        public USER_INFO USER_INFO {
            get { return _USER_INFO; }
            set { _USER_INFO = value; }
        }
    }

    [Serializable]
    public partial class TRN_DCR_UP {

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _DCR_NO;

        public decimal? DCR_NO {
            get { return _DCR_NO; }
            set { _DCR_NO = value; }
        }

        DateTime _ACTION_OFFLINE_TIME;

        public DateTime ACTION_OFFLINE_TIME {
            get { return _ACTION_OFFLINE_TIME; }
            set { _ACTION_OFFLINE_TIME = value; }
        }

        decimal _DCR_TYPE_NO;

        public decimal DCR_TYPE_NO {
            get { return _DCR_TYPE_NO; }
            set { _DCR_TYPE_NO = value; }
        }

        decimal? _IS_REF_ZM;

        public decimal? IS_REF_ZM {
            get { return _IS_REF_ZM; }
            set { _IS_REF_ZM = value; }
        }

        decimal? _REF_ZM_USER_NO;

        public decimal? REF_ZM_USER_NO {
            get { return _REF_ZM_USER_NO; }
            set { _REF_ZM_USER_NO = value; }
        }

        string _REF_ZM_MOBILE;

        public string REF_ZM_MOBILE {
            get { return _REF_ZM_MOBILE; }
            set { _REF_ZM_MOBILE = value; }
        }

        decimal? _WORK_AREA_FROM_LAT;

        public decimal? WORK_AREA_FROM_LAT {
            get { return _WORK_AREA_FROM_LAT; }
            set { _WORK_AREA_FROM_LAT = value; }
        }

        decimal? _WORK_AREA_FROM_LON;

        public decimal? WORK_AREA_FROM_LON {
            get { return _WORK_AREA_FROM_LON; }
            set { _WORK_AREA_FROM_LON = value; }
        }

        string _WORK_AREA_FROM_NAME;

        public string WORK_AREA_FROM_NAME {
            get { return _WORK_AREA_FROM_NAME; }
            set { _WORK_AREA_FROM_NAME = value; }
        }

        decimal? _WORK_AREA_TO_LAT;

        public decimal? WORK_AREA_TO_LAT {
            get { return _WORK_AREA_TO_LAT; }
            set { _WORK_AREA_TO_LAT = value; }
        }

        decimal? _WORK_AREA_TO_LON;

        public decimal? WORK_AREA_TO_LON {
            get { return _WORK_AREA_TO_LON; }
            set { _WORK_AREA_TO_LON = value; }
        }

        string _WORK_AREA_TO_NAME;

        public string WORK_AREA_TO_NAME {
            get { return _WORK_AREA_TO_NAME; }
            set { _WORK_AREA_TO_NAME = value; }
        }

        DateTime? _TIME_FROM;

        public DateTime? TIME_FROM {
            get { return _TIME_FROM; }
            set { _TIME_FROM = value; }
        }

        DateTime? _TIME_TO;

        public DateTime? TIME_TO {
            get { return _TIME_TO; }
            set { _TIME_TO = value; }
        }

        decimal? _TIME_GAP;

        public decimal? TIME_GAP {
            get { return _TIME_GAP; }
            set { _TIME_GAP = value; }
        }

        decimal? _INSTITUTE_NO;

        public decimal? INSTITUTE_NO {
            get { return _INSTITUTE_NO; }
            set { _INSTITUTE_NO = value; }
        }

        decimal? _TRANS_TYPE_NO;

        public decimal? TRANS_TYPE_NO {
            get { return _TRANS_TYPE_NO; }
            set { _TRANS_TYPE_NO = value; }
        }

        decimal? _FARE_AMT;

        public decimal? FARE_AMT {
            get { return _FARE_AMT; }
            set { _FARE_AMT = value; }
        }

        DateTime? _TRN_DCR_DATE;

        public DateTime? TRN_DCR_DATE {
            get { return _TRN_DCR_DATE; }
            set { _TRN_DCR_DATE = value; }
        }

        decimal? _IS_MANUAL_ENTRY;

        public decimal? IS_MANUAL_ENTRY {
            get { return _IS_MANUAL_ENTRY; }
            set { _IS_MANUAL_ENTRY = value; }
        }

        decimal? _WORK_PUR_NO;

        public decimal? WORK_PUR_NO {
            get { return _WORK_PUR_NO; }
            set { _WORK_PUR_NO = value; }
        }

        decimal? _DIVISION_NO;

        public decimal? DIVISION_NO {
            get { return _DIVISION_NO; }
            set { _DIVISION_NO = value; }
        }


        decimal? _ZONE_NO;

        public decimal? ZONE_NO {
            get { return _ZONE_NO; }
            set { _ZONE_NO = value; }
        }


        decimal? _ZILLA_NO;

        public decimal? ZILLA_NO {
            get { return _ZILLA_NO; }
            set { _ZILLA_NO = value; }
        }


        decimal? _THANA_NO;

        public decimal? THANA_NO {
            get { return _THANA_NO; }
            set { _THANA_NO = value; }
        }

        decimal _OFFLINE_DCR_NO;

        public decimal OFFLINE_DCR_NO {
            get { return _OFFLINE_DCR_NO; }
            set { _OFFLINE_DCR_NO = value; }
        }

        string _COMMENTS;

        public string COMMENTS {
            get { return _COMMENTS; }
            set { _COMMENTS = value; }
        }

        decimal _ENTRY_STATE;

        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }

        List<TRN_DCR_DET_UP> _TRN_DCR_DET_UP;

        public List<TRN_DCR_DET_UP> TRN_DCR_DET_UP {
            get { return _TRN_DCR_DET_UP; }
            set { _TRN_DCR_DET_UP = value; }
        }
    }

    [Serializable]
    public partial class TRN_DCR_DET_UP {

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _DCR_DET_NO;

        public decimal? DCR_DET_NO {
            get { return _DCR_DET_NO; }
            set { _DCR_DET_NO = value; }
        }

        decimal? _DCR_NO;

        public decimal? DCR_NO {
            get { return _DCR_NO; }
            set { _DCR_NO = value; }
        }

        DateTime _ACTION_OFFLINE_TIME;

        public DateTime ACTION_OFFLINE_TIME {
            get { return _ACTION_OFFLINE_TIME; }
            set { _ACTION_OFFLINE_TIME = value; }
        }

        decimal? _IS_FOR_TEACHER;

        public decimal? IS_FOR_TEACHER {
            get { return _IS_FOR_TEACHER; }
            set { _IS_FOR_TEACHER = value; }
        }

        decimal? _TEACHER_NO;

        public decimal? TEACHER_NO {
            get { return _TEACHER_NO; }
            set { _TEACHER_NO = value; }
        }


        string _TEACHER_MOBILE;

        public string TEACHER_MOBILE {
            get { return _TEACHER_MOBILE; }
            set { _TEACHER_MOBILE = value; }
        }
        string _TEACHER_NICK;

        public string TEACHER_NICK
        {
            get { return _TEACHER_NICK; }
            set { _TEACHER_NICK = value; }
        }
 
        decimal? _IS_ON_BEHALF;

        public decimal? IS_ON_BEHALF {
            get { return _IS_ON_BEHALF; }
            set { _IS_ON_BEHALF = value; }
        }

        string _BEHALF_MOBILE;

        public string BEHALF_MOBILE {
            get { return _BEHALF_MOBILE; }
            set { _BEHALF_MOBILE = value; }
        }
        string _BEHALF_NICK;

        public string BEHALF_NICK
        {
            get { return _BEHALF_NICK; }
            set { _BEHALF_NICK = value; }
        }
        decimal? _SPECIMEN_NO;

        public decimal? SPECIMEN_NO {
            get { return _SPECIMEN_NO; }
            set { _SPECIMEN_NO = value; }
        }

        decimal? _SPECIMEN_QTY;

        public decimal? SPECIMEN_QTY {
            get { return _SPECIMEN_QTY; }
            set { _SPECIMEN_QTY = value; }
        }

        decimal? _IS_FOR_CLIENT;

        public decimal? IS_FOR_CLIENT {
            get { return _IS_FOR_CLIENT; }
            set { _IS_FOR_CLIENT = value; }
        }

        decimal? _CLIENT_NO;

        public decimal? CLIENT_NO {
            get { return _CLIENT_NO; }
            set { _CLIENT_NO = value; }
        }

        string _CLIENT_MOBILE;

        public string CLIENT_MOBILE {
            get { return _CLIENT_MOBILE; }
            set { _CLIENT_MOBILE = value; }
        }

        decimal? _PROMO_ITEM_NO;

        public decimal? PROMO_ITEM_NO {
            get { return _PROMO_ITEM_NO; }
            set { _PROMO_ITEM_NO = value; }
        }

        decimal? _PROMO_ITEM_QTY;

        public decimal? PROMO_ITEM_QTY {
            get { return _PROMO_ITEM_QTY; }
            set { _PROMO_ITEM_QTY = value; }
        }        

        decimal _OFFLINE_DCR_NO;

        public decimal OFFLINE_DCR_NO {
            get { return _OFFLINE_DCR_NO; }
            set { _OFFLINE_DCR_NO = value; }
        }

        decimal _OFFLINE_DCR_DET_NO;

        public decimal OFFLINE_DCR_DET_NO {
            get { return _OFFLINE_DCR_DET_NO; }
            set { _OFFLINE_DCR_DET_NO = value; }
        }

        decimal _ENTRY_STATE;

        public decimal ENTRY_STATE {
            get { return _ENTRY_STATE; }
            set { _ENTRY_STATE = value; }
        }
    }
}
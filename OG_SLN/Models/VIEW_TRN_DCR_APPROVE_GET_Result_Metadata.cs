using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN {

    /*[MetadataType(typeof(SEC_USERS_Metadata))]*/
    public partial class VIEW_TRN_DCR_APPROVE_GET_Result2 {

        string _IS_AUTO_APPROVE_NAME = string.Empty;

        [Display(Name = "Auto Approve Type")]
        public string IS_AUTO_APPROVE_NAME {
            get {
                if (this.IS_AUTO_APPROVE.HasValue) {
                    if (this._IS_AUTO_APPROVE.Value == 1) {
                        this._IS_AUTO_APPROVE_NAME = "Yes";
                    } else {
                        this._IS_AUTO_APPROVE_NAME = "No";
                    }
                } else {
                    this._IS_AUTO_APPROVE_NAME = "No";
                }
                return this._IS_AUTO_APPROVE_NAME;
            }
            set {
            }
        }
    }

    public class VIEW_TRN_DCR_APPROVE_GET_Result2_Metadata {
        
    }

    public partial class VIEW_TRN_DCR_APPROVE_GET_Result2 {
        List<VIEW_TRN_DCR_DET_APRROVE_GET_Result1> _VIEW_TRN_DCR_DET_APRROVE_GET_Result;

        public List<VIEW_TRN_DCR_DET_APRROVE_GET_Result1> VIEW_TRN_DCR_DET_APRROVE_GET_Result {
            get { return _VIEW_TRN_DCR_DET_APRROVE_GET_Result; }
            set { _VIEW_TRN_DCR_DET_APRROVE_GET_Result = value; }
        }
    }

    public class TRN_DCR_DO_APPROVE {
        decimal? _DCR_NO;

        public decimal? DCR_NO {
            get { return _DCR_NO; }
            set { _DCR_NO = value; }
        }

        decimal? _APPROVE_TYPE_NO;

        public decimal? APPROVE_TYPE_NO {
            get { return _APPROVE_TYPE_NO; }
            set { _APPROVE_TYPE_NO = value; }
        }

        decimal? _APPROVE_USER_NO;

        public decimal? APPROVE_USER_NO {
            get { return _APPROVE_USER_NO; }
            set { _APPROVE_USER_NO = value; }
        }

        decimal? _APPROVE_LOGON_NO;

        public decimal? APPROVE_LOGON_NO {
            get { return _APPROVE_LOGON_NO; }
            set { _APPROVE_LOGON_NO = value; }
        }

        decimal? _REASON_NO;

        public decimal? REASON_NO {
            get { return _REASON_NO; }
            set { _REASON_NO = value; }
        }

        string _REAMRKS;

        public string REAMRKS {
            get { return _REAMRKS; }
            set { _REAMRKS = value; }
        }

        decimal? _APPROVE_TRANS_TYPE_NO;

        public decimal? APPROVE_TRANS_TYPE_NO {
            get { return _APPROVE_TRANS_TYPE_NO; }
            set { _APPROVE_TRANS_TYPE_NO = value; }
        }

        decimal? _APPROVE_FARE_AMT;

        public decimal? APPROVE_FARE_AMT {
            get { return _APPROVE_FARE_AMT; }
            set { _APPROVE_FARE_AMT = value; }
        }

    }

    public class TRN_EXPENSE_DO_APPROVE {
        decimal? _EXP_NO;

        public decimal? EXP_NO {
            get { return _EXP_NO; }
            set { _EXP_NO = value; }
        }

        decimal? _FY_NO;

        public decimal? FY_NO {
            get { return _FY_NO; }
            set { _FY_NO = value; }
        }

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _APPROVE_TYPE_NO;

        public decimal? APPROVE_TYPE_NO {
            get { return _APPROVE_TYPE_NO; }
            set { _APPROVE_TYPE_NO = value; }
        }

        decimal? _APPROVE_USER_NO;

        public decimal? APPROVE_USER_NO {
            get { return _APPROVE_USER_NO; }
            set { _APPROVE_USER_NO = value; }
        }

        decimal? _APPROVE_LOGON_NO;

        public decimal? APPROVE_LOGON_NO {
            get { return _APPROVE_LOGON_NO; }
            set { _APPROVE_LOGON_NO = value; }
        }

        decimal? _REASON_NO;

        public decimal? REASON_NO {
            get { return _REASON_NO; }
            set { _REASON_NO = value; }
        }

        string _REAMRKS;

        public string REAMRKS {
            get { return _REAMRKS; }
            set { _REAMRKS = value; }
        }

        
    }

    public partial class VIEW_TRN_EXPENSE_APPROVE_GET_Result {
        List<VIEW_TRN_EXPENSE_DET_APPR_GET_Result> _VIEW_TRN_EXPENSE_DET_APPR_GET_Result;

        public List<VIEW_TRN_EXPENSE_DET_APPR_GET_Result> VIEW_TRN_EXPENSE_DET_APPR_GET_Result {
            get { return _VIEW_TRN_EXPENSE_DET_APPR_GET_Result; }
            set { _VIEW_TRN_EXPENSE_DET_APPR_GET_Result = value; }
        }
    }

    public partial class VIEW_DCR_SHEET_Search {

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        DateTime? _TRN_DCR_DATE;

        public DateTime? TRN_DCR_DATE {
            get { return _TRN_DCR_DATE; }
            set { _TRN_DCR_DATE = value; }
        }
    }

    public partial class TRN_DCR_SHEET_SUM_Result {

        List<TRN_DCR_SHEET_DET_Result> _TRN_DCR_SHEET_DET_List = new List<TRN_DCR_SHEET_DET_Result>();

        public List<TRN_DCR_SHEET_DET_Result> TRN_DCR_SHEET_DET_List {
            get { return _TRN_DCR_SHEET_DET_List; }
            set { _TRN_DCR_SHEET_DET_List = value; }
        }
    }

    public partial class TRN_DCR_SUM_SEARCH {

        decimal? _DEPT_NO;

        public decimal? DEPT_NO {
            get { return _DEPT_NO; }
            set { _DEPT_NO = value; }
        }

        decimal? _DIVISION_NO;

        public decimal? DIVISION_NO {
            get { return _DIVISION_NO; }
            set { _DIVISION_NO = value; }
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

        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _MON_NO;

        public decimal? MON_NO {
            get { return _MON_NO; }
            set { _MON_NO = value; }
        }

        decimal? _YEAR;

        public decimal? YEAR {
            get { return _YEAR; }
            set { _YEAR = value; }
        }

        DateTime? _TRN_DCR_DATE_FROM;

        public DateTime? TRN_DCR_DATE_FROM {
            get { return _TRN_DCR_DATE_FROM; }
            set { _TRN_DCR_DATE_FROM = value; }
        }

        DateTime? _TRN_DCR_DATE_TO;

        public DateTime? TRN_DCR_DATE_TO {
            get { return _TRN_DCR_DATE_TO; }
            set { _TRN_DCR_DATE_TO = value; }
        }

        decimal? _DCR_TYPE_NO;

        public decimal? DCR_TYPE_NO {
            get { return _DCR_TYPE_NO; }
            set { _DCR_TYPE_NO = value; }
        }

    }

}
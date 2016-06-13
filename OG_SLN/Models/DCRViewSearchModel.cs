using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN  {
    [Serializable]
    public class DCRViewSearchModel {
        decimal? _DCR_NO;

        public decimal? DCR_NO {
            get { return _DCR_NO; }
            set { _DCR_NO = value; }
        }

        decimal? _INSERT_IS_OFFLINE;

        public decimal? INSERT_IS_OFFLINE {
            get { return _INSERT_IS_OFFLINE; }
            set { _INSERT_IS_OFFLINE = value; }
        }

        decimal? _DCR_TYPE_NO;

        public decimal? DCR_TYPE_NO {
            get { return _DCR_TYPE_NO; }
            set { _DCR_TYPE_NO = value; }
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

        decimal? _IS_REF_ZM;

        public decimal? IS_REF_ZM {
            get { return _IS_REF_ZM; }
            set { _IS_REF_ZM = value; }
        }

        decimal? _INSTITUTE_NO;

        public decimal? INSTITUTE_NO {
            get { return _INSTITUTE_NO; }
            set { _INSTITUTE_NO = value; }
        }

        decimal? _INST_TYPE_NO;

        public decimal? INST_TYPE_NO {
            get { return _INST_TYPE_NO; }
            set { _INST_TYPE_NO = value; }
        }

        decimal? _TRANS_TYPE_NO;

        public decimal? TRANS_TYPE_NO {
            get { return _TRANS_TYPE_NO; }
            set { _TRANS_TYPE_NO = value; }
        }

        decimal? _APPROVE_TRANS_TYPE_NO;

        public decimal? APPROVE_TRANS_TYPE_NO {
            get { return _APPROVE_TRANS_TYPE_NO; }
            set { _APPROVE_TRANS_TYPE_NO = value; }
        }

        decimal? _APPROVE_TYPE_NO;

        public decimal? APPROVE_TYPE_NO {
            get { return _APPROVE_TYPE_NO; }
            set { _APPROVE_TYPE_NO = value; }
        }

        decimal? _IS_MANUAL_ENTRY;

        public decimal? IS_MANUAL_ENTRY {
            get { return _IS_MANUAL_ENTRY; }
            set { _IS_MANUAL_ENTRY = value; }
        }

        decimal? _IS_TRN_LOCKED;

        public decimal? IS_TRN_LOCKED {
            get { return _IS_TRN_LOCKED; }
            set { _IS_TRN_LOCKED = value; }
        }

        decimal? _IS_LOCK_BY_SYSTEM;

        public decimal? IS_LOCK_BY_SYSTEM {
            get { return _IS_LOCK_BY_SYSTEM; }
            set { _IS_LOCK_BY_SYSTEM = value; }
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

        decimal? _IS_VERIFY;

        public decimal? IS_VERIFY {
            get { return _IS_VERIFY; }
            set { _IS_VERIFY = value; }
        }

    }

    [Serializable]
    public class DERViewSearchModel {
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

        DateTime? _TRN_EXP_DATE_FROM;

        public DateTime? TRN_EXP_DATE_FROM {
            get { return _TRN_EXP_DATE_FROM; }
            set { _TRN_EXP_DATE_FROM = value; }
        }

        DateTime? _TRN_EXP_DATE_TO;

        public DateTime? TRN_EXP_DATE_TO {
            get { return _TRN_EXP_DATE_TO; }
            set { _TRN_EXP_DATE_TO = value; }
        }
    }

}
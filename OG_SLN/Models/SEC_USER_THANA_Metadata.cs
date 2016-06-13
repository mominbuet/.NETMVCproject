using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN {
    public class SEC_USER_THANA_Metadata {
    }

    [Serializable]
    public partial class SecUserThanaSearch {
        decimal? _USER_NO;

        public decimal? USER_NO {
            get { return _USER_NO; }
            set { _USER_NO = value; }
        }

        decimal? _THANA_NO;

        public decimal? THANA_NO {
            get { return _THANA_NO; }
            set { _THANA_NO = value; }
        }

        decimal? _IS_ACTIVE;

        public decimal? IS_ACTIVE {
            get { return _IS_ACTIVE; }
            set { _IS_ACTIVE = value; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN {

    [Serializable]
    public class DcrVerifyApproveSearch {

        public decimal? dcrType {get; set;}

        public decimal? USER_NO { get; set; }

        public decimal? ddldivision { get; set; }

        public decimal? ddlZillas { get; set; }

        public decimal? ddlThanas { get; set; }

        public decimal? zmusers { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal? verify_status { get; set; }

        public decimal? fare_cost { get; set; }

    }
}
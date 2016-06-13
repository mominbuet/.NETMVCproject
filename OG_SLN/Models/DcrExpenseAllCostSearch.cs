using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN {
    public class DcrExpenseAllCostSearch {
        public decimal? USER_NO { get; set;}
        public decimal? USER_PARENT_NO { get; set; }
        public decimal? ddldivision { get; set; }
        public decimal? ddlZillas { get; set; }
        public decimal? ddlThanas { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? zmusers { get; set; }
    }
}
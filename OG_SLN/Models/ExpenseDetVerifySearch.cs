using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    [Serializable]
    public class ExpenseDetVerifySearch
    {
        public decimal? USER_NO { get; set; }

        public decimal? ddldivision { get; set; }

        public decimal? ddlZillas { get; set; }

        public decimal? ddlThanas { get; set; }

        public decimal? zmusers { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        //public decimal? uptype { get; set; } 

        public decimal? verify_status { get; set; } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class DashboardSearchModel
    {
        public decimal? Search_ZM_User { get; set; }
        public decimal? Search_AM_User { get; set; }

        public DateTime? Search_Date_From { get; set; }
        public DateTime? Search_Date_To { get; set; }
    }
}
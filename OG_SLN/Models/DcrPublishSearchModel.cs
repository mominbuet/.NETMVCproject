using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class DcrPublishSearchModel
    {
        public decimal? Search_User { get; set; }
        public string User_list { get; set; }
        public decimal? Search_Division { get; set; }
        public decimal? Search_Zilla { get; set; }
        public decimal? Search_Thana { get; set; }
        public decimal? Search_pubno { get; set; }
        public DateTime? Search_Date_From { get; set; }
        public DateTime? Search_Date_To { get; set; }
        public DateTime? Dcr_Date_From { get; set; }
        public DateTime? Dcr_Date_To { get; set; }

        public decimal? Dcr_Amt { get; set; }
        public decimal? Der_Amt { get; set; }
        public decimal? Sd_Cnt { get; set; }
        public string Comments { get; set; }


        public string Filter_Type { get; set; }
    }
}
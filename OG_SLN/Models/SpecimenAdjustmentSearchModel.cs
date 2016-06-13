using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class SpecimenAdjustmentSearchModel
    {
        public decimal? Search_User { get; set; }

        public decimal? Search_Division { get; set; }
        public decimal? Search_Zilla { get; set; }
        public decimal? Search_Thana { get; set; }

        public DateTime? Search_Date_From { get; set; }
        public DateTime? Search_Date_To { get; set; }

        public decimal? Search_Specimen { get; set; }
    }
}
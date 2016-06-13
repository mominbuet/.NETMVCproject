using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class FeedbackSearchViewModel
    {
        public decimal? Search_Feedback_Type { get; set; }
        public decimal? Search_User { get; set; }
        public DateTime? Search_Date_From { get; set; }
        public DateTime? Search_Date_To { get; set; }
    }
}
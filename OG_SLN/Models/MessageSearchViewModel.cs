using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class MessageSearchViewModel
    {
        public decimal? Search_From_User { get; set; }
        public decimal? Search_To_User { get; set; }
        public string Search_Subject { get; set; }
        public DateTime? Search_Date_From { get; set; }
        public DateTime? Search_Date_To { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    public class UpDownSearchModel
    {
        public decimal? Search_Division { get; set; }
        public decimal? Search_Zilla { get; set; }
        public decimal? Search_Thana { get; set; }
        public decimal? Search_User { get; set; }

        public DateTime? Search_From { get; set; }
        public DateTime? Search_To { get; set; }
    }
}
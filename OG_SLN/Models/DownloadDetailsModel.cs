using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class DownloadDetailsSearchModel
    {
        public decimal? User_No { get; set; }
        
        public string User_Name { get; set; }
        public string Division_Name { get; set; }
        public string Zilla_Name { get; set; }
        public string Thana_Name { get; set; }

        public DateTime? Date_From { get; set; }
        public DateTime? Date_To { get; set; }
    }

    public class DownloadDetailsViewModel
    {
        public string User_Name { get; set; }
        public string Division_Name { get; set; }
        public string Zilla_Name { get; set; }
        public string Thana_Name { get; set; }
        public DateTime? Date { get; set; }

        public List<DOWNLOAD_USERS_DET_GET_Result> details { get; set; }
    }
}
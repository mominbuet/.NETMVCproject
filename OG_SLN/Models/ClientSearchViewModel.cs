using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class ClientSearchViewModel
    {
        public string Search_Name { get; set; }
        public string Search_Mobile { get; set; }
        public decimal? Search_Division { get; set; }
        public decimal? Search_Zilla { get; set; }
        public decimal? Search_Thana { get; set; }
        public bool? Search_Active { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class InstituteSearchViewModel
    {
        public decimal? Institute_Type { get; set; }

        public string Institute_Dbid { get; set; }

        public string Institute_Name { get; set; }

        public string Institute_Eiin { get; set; }

        public decimal? Institute_Code { get; set; }

        public decimal? DIVISION_NO { get; set; }

        public decimal? ZILLA_NO { get; set; }

        public decimal? THANA_NO { get; set; }

        public decimal? PROJECT_NO { get; set; }

        public decimal? USER_NO { get; set; }
    }
}
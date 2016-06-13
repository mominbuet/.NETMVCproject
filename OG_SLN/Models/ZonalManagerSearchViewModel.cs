using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class ZonalManagerSearchViewModel
    {
        public string Zonal_Name { get; set; }

        public string Zonal_Hrid { get; set; }

        public string Zonal_Username { get; set; }

        public string Zonal_Mobile { get; set; }

        public string Zonal_Email { get; set; }

        public decimal? Zonal_Dept { get; set; }

        public decimal? Zonal_Desig { get; set; }

        public decimal? Zonal_Parent { get; set; }

        public decimal? Zonal_User_Type { get; set; }

        public decimal? Zonal_Thana { get; set; }

        public decimal? Zilla_No { get; set; }

        public decimal? Division_No { get; set; }
    }
}
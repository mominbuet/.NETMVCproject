using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class TeacherSearchViewModel
    {
        public string Search_Name { get; set; }
        public string Search_Mobile { get; set; }
        public decimal? Search_Institute { get; set; }
        public decimal? Search_Designation { get; set; }
        public bool? Search_Active { get; set; }
        public bool? Search_Approved { get; set; }
    }

    public class TeacherApproveSearchViewModel
    {
        public decimal? Search_Insert_User { get; set; }
        public decimal? Search_Institute { get; set; }
        public decimal? Search_Designation { get; set; }
        public bool? Search_Active { get; set; }
        public DateTime? Search_Date_From { get; set; }
        public DateTime? Search_Date_To { get; set; }
        public decimal? Search_Approved { get; set; }
    }
}
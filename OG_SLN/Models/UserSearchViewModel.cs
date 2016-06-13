using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class UserSearchViewModel
    {
        public decimal? usertype { get; set; }
        public decimal? companyno { get; set; }
        public decimal? deptno { get; set; }
        public decimal? designo { get; set; }
        public string hrid { get; set; }
        public string fullname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public bool? active { get; set; }
    }

    public class AllUserSearchViewModel
    {
        public decimal? Search_User_Type { get; set; }
        public decimal? Search_Company { get; set; }
        public decimal? Search_Department { get; set; }
        public decimal? Search_Designation { get; set; }
        public string Search_Hrid { get; set; }
        public string Search_Fullname { get; set; }
        public string Search_Mobile { get; set; }
        public string Search_Email { get; set; }
        public string Search_Username { get; set; }
        public bool? Search_Active { get; set; }
    }
}
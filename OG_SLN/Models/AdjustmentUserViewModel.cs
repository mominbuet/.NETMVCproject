using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class AdjustmentUserSearchModel
    {
        public decimal? Specimen_No { get; set; }
        public string Specimen_Name { get; set; }
        public string Specimen_Name_Bng { get; set; }
        public string Specimen_Code { get; set; }
        public DateTime? Date_From { get; set; }
        public DateTime? Date_To { get; set; }
    }

    public class AdjustmentUserRecordModel
    {
        public decimal? Specimen_No { get; set; }
        public string Specimen_Name { get; set; }
        public string Specimen_Name_Bng { get; set; }
        public string Specimen_Code { get; set; }
        public DateTime? Date_From { get; set; }
        public DateTime? Date_To { get; set; }

        public List<SPECIMEN_ADJUST_USERWISE_GET_Result> Specimen_Users { get; set; }
    }
    
    public class AdjustmentApprovedDetailsSearchModel
    {
        public decimal? User_No { get; set; }
        public string User_Name { get; set; }

        public decimal? Specimen_No { get; set; }
        public string Specimen_Name { get; set; }
        public string Specimen_Name_Bng { get; set; }
        public string Specimen_Code { get; set; }
        public DateTime? Date_From { get; set; }
        public DateTime? Date_To { get; set; }
    }

    public class AdjustmentApprovedDetailsViewModel
    {
        public decimal? User_No { get; set; }
        public string User_Name { get; set; }

        public decimal? Specimen_No { get; set; }
        public string Specimen_Name { get; set; }
        public string Specimen_Name_Bng { get; set; }
        public string Specimen_Code { get; set; }
        public DateTime? Date_From { get; set; }
        public DateTime? Date_To { get; set; }

        public List<SPECIMEN_ADJUST_DIST_GET_Result> Approved_Details { get; set; }
    }
}
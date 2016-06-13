using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;

namespace OG_SLN
{
    [MetadataType(typeof(TRN_CHALLAN_UPLOAD_Metadata))]
    public partial class TRN_CHALLAN_UPLOAD
    {
        public HttpPostedFileBase uploadExcel { get; set; }

        public string ERROR_MSG { get; set; }

        public string UPLOAD_TYPE
        {
            get
            {
                if (UPLOAD_TYPE_NUM == (decimal)EChallanTypes.Challan)
                    return "Challan";
                else
                    return "Return";
            }
        }
    }

    public class TRN_CHALLAN_UPLOAD_Metadata
    {
        [Display(Name = "Upload File")]
        public string FILE_NAME { get; set; }

        [Display(Name = "File Type")]
        public string FILE_TYPE { get; set; }

        [Display(Name = "File Ext")]
        public string FILE_EXT { get; set; }

        [Display(Name = "Insert Time")]
        public DateTime? INSERT_TIME { get; set; }

        [Display(Name = "Total Records")]
        public decimal? TOTAL_RECORDS { get; set; }

        [Display(Name = "Total HR Count")]
        public decimal? TOTAL_HR_COUNT { get; set; }

        [Display(Name = "Total Book Count")]
        public decimal? TOTAL_BOOK_COUNT { get; set; }

        [Display(Name = "Failed HR Count")]
        public decimal? FAILED_HR_COUNT { get; set; }

        [Display(Name = "Failed Book Count")]
        public decimal? FAILED_BOOK_COUNT { get; set; }

        [Display(Name = "Upload Type")]
        public decimal? UPLOAD_TYPE_NUM { get; set; }
    }
}
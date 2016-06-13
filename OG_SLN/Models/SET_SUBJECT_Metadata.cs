using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{

    [MetadataType(typeof(SET_SUBJECT_Metadata))]
    public partial class SET_SUBJECT
    {
        public bool ACTIVE_STATUS
        {
            get
            {
                return IS_ACTIVE == 1;
            }
            set
            {
                IS_ACTIVE = value ? 1 : 0;
            }
        }

        public decimal CLASS_NO { get; set; }
    }

    public class SET_SUBJECT_Metadata
    {
        [Required]
        [Display(Name = "Subject Name")]
        public string SUBJECT_NAME { get; set; }

        [Display(Name = "Subject Name (Bangla)")]
        public string SUBJECT_NAME_BNG { get; set; }

        [Display(Name = "Subject Code")]
        public string SUBJECT_CODE { get; set; }

        [Display(Name = "Subject Description")]
        public string SUBJECT_DESC { get; set; }

        [Display(Name = "Active")]
        public decimal ACTIVE_STATUS { get; set; }

        [Display(Name = "Active From")]
        public decimal ACTIVE_FROM { get; set; }

        [Display(Name = "Active To")]
        public decimal ACTIVE_TO { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_INSTITUTE_Metadata))]
    public partial class SET_INSTITUTE
    {
        public bool ACTIVE_INSTITUTE
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
        public decimal ZILLA_NO{get;set;}
        public string DESC_INS
        {
            get
            {
                return INSTITUTE_DESC;
            }
            set
            {
                INSTITUTE_DESC = value;
            }
        }
        
    }

    public class SET_INSTITUTE_Metadata
    {
        //[Required]
        //[Display(Name = "Institute")]
        //public string INSTITUTE_NAME { get; set; }
        [Required]
        [Display(Name = "Zilla")]
        public decimal ZILLA_NO { get; set; }
        [Required]
        [Display(Name = "Active Institute")]
        public bool ACTIVE_INSTITUTE { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of Institute")]
        public string DESC_INS { get; set; }
    }
}
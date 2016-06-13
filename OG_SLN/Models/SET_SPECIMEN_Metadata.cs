using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
     [MetadataType(typeof(SET_SPECIMEN_Metadata))]
    public partial class SET_SPECIMEN
    {
        public bool ACTIVE_SPECIMEN
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
        public string DESC_SPECIMEN
        {
            get
            {
                return SPECIMEN_DESC;
            }
            set
            {
                SPECIMEN_DESC = value;
            }
        }
        public string SPEC_CODE
        {
            get
            {
                return SPECIMEN_CODE;
            }
            set
            {
                SPECIMEN_CODE = value;
            }
        }
    }
    public class SET_SPECIMEN_Metadata
    {
        [Required]
        [Display(Name = "Active Specimen")]
        public bool ACTIVE_SPECIMEN { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of Specimen")]
        public decimal DESC_SPECIMEN { get; set; }
        [Required]
        [Display(Name = "Specimen Code")]
        public string SPEC_CODE { get; set; }
    }
}
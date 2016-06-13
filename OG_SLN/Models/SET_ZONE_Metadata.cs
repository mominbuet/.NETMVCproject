using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_ZONE_Metadata))]
    public partial class SET_ZONE
    {
        public bool ACTIVE_ZONE
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
        public string DESC_ZONE
        {
            get
            {
                return ZONE_DESC;
            }
            set
            {
                ZONE_DESC = value;
            }
        }
        public string DETAILS_ZONE
        {
            get
            {
                return ZONE_DETAILS;
            }
            set
            {
                ZONE_DETAILS = value;
            }
        }
    }
    public class SET_ZONE_Metadata
    {
        [Required]
        [Display(Name = "Active Zone")]
        public bool ACTIVE_ZONE { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of Zone")]
        public string DESC_ZONE { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Details of Zone")]
        [Required]
        public string DETAILS_ZONE { get; set; }
    }

}
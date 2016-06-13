using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{

    [MetadataType(typeof(TRN_USER_SPECIMEN_DET_Metadata))]
    public partial class TRN_USER_PROMO_DET
    {
        public bool ACTIVE_SPEC
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
    }
    public class TRN_USER_PROMO_DET_Metadata
    {
        [Required]
        [Display(Name = "Active promotional item")]
        public bool ACTIVE_SPEC { get; set; }
    }
}
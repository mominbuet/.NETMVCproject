using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_FEEDBACK_TYPE_Metadata))]
    public partial class SET_FEEDBACK_TYPE
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

    public class SET_FEEDBACK_TYPE_Metadata
    {
        [Display(Name = "Active")]
        public decimal ACTIVE_STATUS { get; set; }

        [Display(Name = "Active From")]
        public decimal ACTIVE_FROM { get; set; }

        [Display(Name = "Active To")]
        public decimal ACTIVE_TO { get; set; }
    }
}
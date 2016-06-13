using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(TRN_USER_SPECIMEN_Metadata))]
    public partial class TRN_USER_PROMO_ITEM
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
        public decimal edit_ID { get; set; }
    }
    public class TRN_USER_PROMO_ITEM_Metadata
    {
        [Required]
        [Display(Name = "Active User")]
        public bool ACTIVE_STATUS { get; set; }
        [Required]
        [Display(Name = "Edit Mode")]
        public decimal edit_ID { get; set; }
    }
}
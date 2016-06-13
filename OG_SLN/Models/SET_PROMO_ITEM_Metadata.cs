using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_PROMO_ITEM_Metadata))]
    public partial class SET_PROMO_ITEM
    {
        public bool ACTIVE_PROMO_ITEM
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
        public string desc_created
        {
            get
            {
                return PROMO_ITEM_DESC;
            }
            set
            {
                PROMO_ITEM_DESC = value;
            }
        }
        
    }
    public class SET_PROMO_ITEM_Metadata
    {
        [Required]
        [Display(Name = "Active Promotional Item")]
        public bool ACTIVE_PROMO_ITEM { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Description of Item Type")]
        public string desc_created { get; set; }
    }
}
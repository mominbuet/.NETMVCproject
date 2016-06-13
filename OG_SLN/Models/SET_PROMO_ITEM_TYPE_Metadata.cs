using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_PROMO_ITEM_TYPE_Metadata))]
    public partial class SET_PROMO_ITEM_TYPE
    {
        public bool ACTIVE_ITEM_TYPE
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
                return PROMO_ITEM_TYPE_DESC;
            }
            set
            {
                PROMO_ITEM_TYPE_DESC = value ;
            }
        }
    }
    public class SET_PROMO_ITEM_TYPE_Metadata
    {
        [Required]
        [Display(Name = "Active Promotional Item Type")]
        public bool ACTIVE_ITEM_TYPE { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Description of Item Type")]
        public string desc_created { get; set; }
    }
}
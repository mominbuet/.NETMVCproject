using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_BOOK_BRAND_TYPE_Metadata))]
    public partial class SET_BOOK_BRAND_TYPE
    {
        public bool ACTIVE_BRAND
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
        public string DESC_BOOK_BRAND
        {
            get
            {
                return BOOK_BRAND_TYPE_DESC;
            }
            set
            {
                BOOK_BRAND_TYPE_DESC = value;
            }
        }
    }
    public class SET_BOOK_BRAND_TYPE_Metadata
    {
        [Required]
        [Display(Name = "Active Brand")]
        public bool ACTIVE_BRAND { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Description of Brand")]
        public decimal DESC_BOOK_BRAND { get; set; }
    }
}
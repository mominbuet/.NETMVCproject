using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_BOOK_GROUP_Metadata))]
    public partial class SET_BOOK_GROUP
    {
        public bool ACTIVE_GROUP
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
        public string DESC_BOOK_GROUP
        {
            get
            {
                return BOOK_GROUP_DESC;
            }
            set
            {
                BOOK_GROUP_DESC = value;
            }
        }
    }
    public partial class SET_BOOK_GROUP_Metadata
    { 
        [Required]
        [Display(Name = "Active Group")]
        public bool ACTIVE_GROUP { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Description of Book Group")]
        public decimal DESC_BOOK_GROUP { get; set; }
    }
}

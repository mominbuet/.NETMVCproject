using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_BOOK_TYPE_Metadata))]
    public partial class SET_BOOK_TYPE
    {
        public bool ACTIVE_BOOK_TYPE
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
        public string DESC_BOOK_TYPE
        {
            get
            {
                return BOOK_TYPE_DESC;
            }
            set
            {
                BOOK_TYPE_DESC = value;
            }
        }
    }
    public class SET_BOOK_TYPE_Metadata
    {
        [Required]
        [Display(Name = "Active Book type")]
        public bool ACTIVE_BOOK_TYPE { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of Book Type")]
        public decimal DESC_BOOK_TYPE { get; set; }
    }
}
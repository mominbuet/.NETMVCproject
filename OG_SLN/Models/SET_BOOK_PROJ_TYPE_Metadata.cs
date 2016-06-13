using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_BOOK_PROJ_TYPE_Metadata))]
    public partial class SET_BOOK_PROJ_TYPE
    {
        public bool ACTIVE_PROJ_TYPE
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
        public string DESC_BOOK_PROJ
        {
            get
            {
                return BOOK_PROJ_TYPE_DESC;
            }
            set
            {
                BOOK_PROJ_TYPE_DESC = value;
            }
        }
    }
    public class SET_BOOK_PROJ_TYPE_Metadata
    {
        [Required]
        [Display(Name = "Active Project type")]
        public bool ACTIVE_PROJ_TYPE { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of Project type")]
        public decimal DESC_BOOK_PROJ { get; set; }
    }
    
}
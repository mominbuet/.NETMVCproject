using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_FISCAL_YEAR_Metadata))]
    public partial class SET_FISCAL_YEAR {
        public bool ACTIVE_Year
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
        public bool ACTIVE_Current_Year
        {
            get
            {
                return IS_CURRENT_YEAR == 1;
            }
            set
            {
                IS_CURRENT_YEAR = value ? 1 : 0;
            }
        }
    }
    public class SET_FISCAL_YEAR_Metadata
    {
        [Required]
        [Display(Name = "Active Fiscal Year")]
        public bool ACTIVE_Year { get; set; }
        [Required]
        [Display(Name = "Active Current Year")]
        public bool ACTIVE_Current_Year { get; set; }
    }
}
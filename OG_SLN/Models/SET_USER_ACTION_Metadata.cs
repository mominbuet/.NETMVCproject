using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_USER_ACTION_Metadata))]
    public partial class SET_USER_ACTION
    {
        public string RETYPE_PASSWORD { get; set; }

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
    }
    public class SET_USER_ACTION_Metadata
    {
        [Display(Name = "Active")]
        public decimal ACTIVE_STATUS { get; set; }
    }
}
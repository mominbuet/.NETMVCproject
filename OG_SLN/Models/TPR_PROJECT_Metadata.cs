using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(TPR_PROJECT_Metadata))]
    public partial class TPR_PROJECT
    {
        public bool ACTIVE_STATUS
        {
            get 
            {
                return IS_ACTIVE == 1 ;
            }
            set 
            {
                IS_ACTIVE = value ? 1 : 0;
            }
        }

        public string DATE_DECLARE
        {
            get
            {
                if (DECLARE_DATE.HasValue)
                    return DECLARE_DATE.Value.ToString("yyyy-MM-dd");
                else
                    return "";
            }
            set
            {
                DECLARE_DATE = DateTime.Parse(value);
            }
        }

        public string DATE_DEADLINE
        {
            get
            {
                if (DEADLINE_DATE.HasValue)
                    return DEADLINE_DATE.Value.ToString("yyyy-MM-dd");
                else
                    return "";
            }
            set
            {
                DEADLINE_DATE = DateTime.Parse(value);
            }
        }
    }
    public class TPR_PROJECT_Metadata
    {
        [Display(Name="Project Name")]
        public string PROJECT_NAME { get; set; }

        [Display(Name = "Declare Date")]
        public DateTime? DECLARE_DATE { get; set; }

        [Display(Name = "Deadline Date")]
        public DateTime? DEADLINE_DATE { get; set; }
        
        [Display(Name="Status")]
        public string ACTIVE_STATUS { get; set; }
    }
}
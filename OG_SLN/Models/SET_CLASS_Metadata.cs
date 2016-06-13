using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_CLASS_Metadata))]
    public partial class SET_CLASS
    {
        public bool ACTIVE_CLASS
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
        public string DESC_CLASS
        {
            get
            {
                return CLASS_DESC;
            }
            set
            {
                CLASS_DESC = value;
            }
        }

        public List<Class_Subject> Subjects { get; set; }
    }
    public class SET_CLASS_Metadata
    {
        [Required]
        [Display(Name = "Active Class")]
        public bool ACTIVE_CLASS { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of Class")]
        public decimal DESC_CLASS { get; set; }

        [Display(Name = "Class")]
        public string CLASS_NAME { get; set; }
    }
}
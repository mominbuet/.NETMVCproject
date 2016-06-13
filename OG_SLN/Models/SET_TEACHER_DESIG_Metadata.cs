using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_TEACHER_DESIG_Metadata))]
    public partial class SET_TEACHER_DESIG
    {
    }
    public class SET_TEACHER_DESIG_Metadata
    {
        [Required]
        [Display(Name = "Designation")]
        public string TEACHER_DESIG_NAME { get; set; }
    }
}
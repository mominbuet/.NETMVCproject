using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_DIVISION_Metadata))]
    public partial class SET_DIVISION
    { }

    public class SET_DIVISION_Metadata
    {
        [Required]
        [Display(Name = "Division")]
        public string DIVISION_NAME { get; set; }
    }
}
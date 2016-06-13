using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_THANA_Metadata))]
    public partial class SET_THANA
    { }

    public class SET_THANA_Metadata
    {
        [Required]
        [Display(Name = "Thana")]
        public string THANA_NAME { get; set; }
    }
}
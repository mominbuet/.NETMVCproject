using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_ZILLA_Metadata))]
    public partial class SET_ZILLA
    { }

    public class SET_ZILLA_Metadata
    {
        [Required]
        [Display(Name = "Zilla")]
        public string ZILLA_NAME { get; set; }
    }
}
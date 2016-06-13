using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_DESIGNATION_Metadata))]
    public partial class SET_DESIGNATION
    { }

    public class SET_DESIGNATION_Metadata
    {
        [Display(Name = "Designation")]
        public string DESIG_NAME { set; get; }
    }
}
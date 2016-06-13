using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_COMPANY_Metadata))]
    public partial class SET_COMPANY
    { }

    public class SET_COMPANY_Metadata
    {
        [Display(Name = "Company")]
        public string COMP_NAME { set; get; }
    }
}
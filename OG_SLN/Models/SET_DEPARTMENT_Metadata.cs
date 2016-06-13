using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_DEPARTMENT_Metadata))]
    public partial class SET_DEPARTMENT
    { }

    public class SET_DEPARTMENT_Metadata
    {
        [Display(Name = "Department")]
        public string DEPT_NAME { set; get; }
    }
}
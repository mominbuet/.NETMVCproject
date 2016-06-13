using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(TRN_MSG_Metadata))]
    public partial class TRN_MSG
    {
        public string MSG_BODY_ML
        {
            get
            {
                return MSG_BODY;
            }
            set
            {
                MSG_BODY = value;
            }
        }

        public IList<ZonalManagerViewModel> ZonalManagers { get; set; }
    }
    public class TRN_MSG_Metadata
    {
        [Required]
        [Display(Name = "Subject")]
        public string MSG_SUBJECT { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name = "Message Body")]
        public string MSG_BODY_ML { get; set; }
    }
}
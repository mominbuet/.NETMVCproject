using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(TPR_UPLOAD_LIST_Metadata))]
    public partial class TPR_UPLOAD_LIST
    {
        public HttpPostedFileBase QuestionFile { get; set; }
    }

    public class TPR_UPLOAD_LIST_Metadata
    {
    }
}
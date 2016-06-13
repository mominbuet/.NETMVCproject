using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    public class Institute_Subject
    {
        public decimal SUBJECT_NO { get; set; }

        public string SUBJECT_NAME { get; set; }

        public string SUBJECT_NAME_BNG { get; set; }

        public DateTime? TARGET_DATE { get; set; }

        public bool IS_ACTIVE { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    public partial class TPR_PROJECT_INSTITUTE
    {
        public IList<Institute_Subject> Subjects { get; set; }

        public DateTime? Target_Date { get; set; }

        public string Subject_List
        {
            get
            {
                if (Subjects.Count > 0)
                {
                    List<decimal> subjects = Subjects
                        .Where(s => s.IS_ACTIVE == true)
                        .Select(s => s.SUBJECT_NO).ToList();
                    string subject_list = string.Join(",", subjects);

                    if (string.IsNullOrEmpty(subject_list))
                        subject_list = null;
                    return subject_list;
                }
                return null;
            }
        }

        public decimal? Is_Collected { get; set; }
        public decimal? Is_New { get; set; }
        public decimal? Proj_Ins_No { get; set; }
    }
}
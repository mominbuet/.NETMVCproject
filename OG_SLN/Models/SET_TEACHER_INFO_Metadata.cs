using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;

namespace OG_SLN
{
    [MetadataType(typeof(SET_TEACHER_INFO_Metadata))]
    public partial class SET_TEACHER_INFO
    {
        public bool ACTIVE_STATUS
        {
            get
            {
                return IS_ACTIVE == 1;
            }
            set
            {
                IS_ACTIVE = value ? 1 : 0;
            }
        }

        public string BIRTH_DATE
        {
            get
            {
                if (TEACHER_DOB.HasValue)
                    return TEACHER_DOB.Value.ToString("yyyy-MM-dd");
                else
                    return String.Empty;
            }
            set
            {
                TEACHER_DOB = DateTime.Parse(value);
            }
        }

        public string MARRIAGE_DATE
        {
            get
            {
                if (TEACHER_MARRIAGE_DATE.HasValue)
                    return TEACHER_MARRIAGE_DATE.Value.ToString("yyyy-MM-dd");
                else
                    return String.Empty;
            }
            set
            {
                TEACHER_MARRIAGE_DATE = DateTime.Parse(value);
            }
        }

        public string DATE_FROM
        {
            get
            {
                if (ACTIVE_FROM.HasValue)
                    return ACTIVE_FROM.Value.ToString("yyyy-MM-dd");
                else
                    return String.Empty;
            }
            set
            {
                ACTIVE_FROM = DateTime.Parse(value);
            }
        }

        public string DATE_TO
        {
            get
            {
                if (ACTIVE_TO.HasValue)
                    return ACTIVE_TO.Value.ToString("yyyy-MM-dd");
                else
                    return String.Empty;
            }
            set
            {
                ACTIVE_TO = DateTime.Parse(value);
            }
        }

        public string INSERT_TIME_STR
        {
            get
            {
                if (INSERT_TIME.HasValue)
                    return INSERT_TIME.Value.ToString("yyyy-MM-dd");
                else
                    return String.Empty;
            }
            set
            {
                INSERT_TIME = DateTime.Parse(value);
            }
        }

        public string APPROVED_STATUS
        {
            get
            {
                if (IS_APPROVED.HasValue)
                {
                    return Enum.GetName(typeof(ApproveType), (int)IS_APPROVED.Value);
                }
                else
                    return String.Empty;
            }
            set
            {
                IS_APPROVED = (decimal)Enum.Parse(typeof(ApproveType), value);
            }
        }

        public IList<Teacher_Subject> TeacherSubjects { get; set; }
    }

    public class SET_TEACHER_INFO_Metadata
    {
        [Required]
        [Display(Name = "Teacher Name")]
        public string TEACHER_NAME { get; set; }

        [Display(Name = "Teacher Name (Bangla)")]
        public string TEACHER_NAME_BNG { get; set; }

        [Display(Name = "Nick Name")]
        public string TEACHER_NICK_NAME { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        public string TEACHER_MOBILE { get; set; }

        [Display(Name = "Date of Birth")]
        public string BIRTH_DATE { get; set; }

        [Display(Name = "Marriage Date")]
        public string MARRIAGE_DATE { get; set; }

        [Display(Name = "Address")]
        public string TEACHER_ADDR { get; set; }
        
        [Display(Name = "Institute")]
        public string INSTITUTE_NO { get; set; }

        [Display(Name = "Designation")]
        public string TEACH_DESIG_NO { get; set; }
        
        [Display(Name = "Active From")]
        public string DATE_FROM { get; set; }

        [Display(Name = "Active To")]
        public string DATE_TO { get; set; }

        [Display(Name = "Active")]
        public decimal ACTIVE_STATUS { get; set; }

    }
}
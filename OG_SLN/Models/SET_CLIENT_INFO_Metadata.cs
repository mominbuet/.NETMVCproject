using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SET_CLIENT_INFO_Metadata))]
    public partial class SET_CLIENT_INFO
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
                if (CLIENT_DOB.HasValue)
                    return CLIENT_DOB.Value.ToString("yyyy-MM-dd");
                else
                    return String.Empty;
            }
            set
            {
                CLIENT_DOB = DateTime.Parse(value);
            }
        }

        public string MARRIAGE_DATE
        {
            get
            {
                if (CLIENT_MARRIAGE_DATE.HasValue)
                    return CLIENT_MARRIAGE_DATE.Value.ToString("yyyy-MM-dd");
                else
                    return String.Empty;
            }
            set
            {
                CLIENT_MARRIAGE_DATE = DateTime.Parse(value);
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
    }


    public class SET_CLIENT_INFO_Metadata
    {
        [Required]
        [Display(Name = "Client Name")]
        public string CLIENT_NAME { get; set; }

        [Display(Name = "Client Name (Bangla)")]
        public string CLIENT_NAME_BNG { get; set; }

        [Display(Name = "Nick Name")]
        public string CLIENT_NICK_NAME { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        public string CLIENT_MOBILE { get; set; }

        [Display(Name = "Date of Birth")]
        public string BIRTH_DATE { get; set; }

        [Display(Name = "Marriage Date")]
        public string MARRIAGE_DATE { get; set; }

        [Display(Name = "Address")]
        public string CLIENT_ADDR { get; set; }
        
        [Display(Name = "Comments")]
        public string CLIENT_COMENTS { get; set; }

        [Display(Name = "Division")]
        public string DIVISION_NO { get; set; }

        [Display(Name = "Zilla")]
        public string ZILLA_NO { get; set; }

        [Display(Name = "Thana")]
        public string THANA_NO { get; set; }

        [Display(Name = "Active From")]
        public string DATE_FROM { get; set; }

        [Display(Name = "Active To")]
        public string DATE_TO { get; set; }

        [Display(Name = "Active")]
        public decimal ACTIVE_STATUS { get; set; }
    }
}
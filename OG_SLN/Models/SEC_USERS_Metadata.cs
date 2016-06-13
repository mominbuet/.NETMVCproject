using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    [MetadataType(typeof(SEC_USERS_Metadata))]
    public partial class SEC_USERS
    {
        public string RETYPE_PASSWORD { get; set; }

        public bool ACTIVE_STATUS {
            get {
                return IS_ACTIVE == 1;
            }
            set {
                IS_ACTIVE = value ? 1 : 0;
            }
        }

        private string _APP_VERSION;

        public string APP_VERSION {
            get { return _APP_VERSION; }
            set { _APP_VERSION = value; }
        }
        

        private DateTime _SERVER_TIME;

        public DateTime SERVER_TIME {
            get { return _SERVER_TIME; }
            set { _SERVER_TIME = value; }
        }
        
        public string DATE_FROM {
            get {
                if (ACTIVE_FROM.HasValue)
                    return ACTIVE_FROM.Value.ToString("yyyy-MM-dd");
                else
                    return "";
            }
            set {
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
                    return "";
            }
            set
            {
                ACTIVE_TO = DateTime.Parse(value);
            }
        }

        public string User_ViewType { get; set; }

        public decimal? DIVISION_NO { get; set; }

        public decimal? ZILLA_NO { get; set; }
    }

    public class SEC_USERS_Metadata
    {
        [Display(Name = "User Type")]
        public decimal USER_TYPE_NO { get; set; }

        [Display(Name = "Department")]
        public decimal? DEPT_NO { get; set; }

        [Display(Name = "Designation")]
        public decimal? DESIG_NO { get; set; }

        [Required]
        [Display(Name = "HR ID")]
        public string HR_EMP_ID { get; set; }

        [Display(Name = "Full Name")]
        public string USER_FULL_NAME { get; set; }

        [Display(Name = "Nick Name")]
        public string USER_NICK_NAME { get; set; }

        [Display(Name = "Contact")]
        public string USER_CONTACT { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        public string USER_MOBILE { get; set; }

        [Display(Name = "Address")]
        public string USER_ADDR { get; set; }

        [Display(Name = "Email")]
        public string USER_EMAIL { get; set; }
        
        [Required]
        [Display(Name = "User Name")]
        public string USER_NAME { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string USER_PWD { get; set; }

        [Required]
        [Display(Name = "Retype Password")]
        public string RETYPE_PASSWORD { get; set; }

        [Display(Name = "Reporting Authority")]
        public string USER_PARENT_NO { get; set; }

        [Display(Name = "Active")]
        public decimal ACTIVE_STATUS { get; set; }

        [Display(Name = "User Active From")]
        public string DATE_FROM { get; set; }

        [Display(Name = "User Active To")]
        public DateTime DATE_TO { get; set; }

        [Display(Name = "Company")]
        public DateTime COMP_NO { get; set; }

        [Display(Name = "Thana")]
        public decimal? THANA_NO { get; set; }
        
    }

    public class SEC_USERS_UP {
        string _USER_NAME;

        public string USER_NAME {
            get { return _USER_NAME; }
            set { _USER_NAME = value; }
        }


        string _USER_PWD;

        public string USER_PWD {
            get { return _USER_PWD; }
            set { _USER_PWD = value; }
        }

        string _USER_MOBILE;

        public string USER_MOBILE {
            get { return _USER_MOBILE; }
            set { _USER_MOBILE = value; }
        }

        string _APP_VERSION;

        public string APP_VERSION {
            get { return _APP_VERSION; }
            set { _APP_VERSION = value; }
        }
    }

    public class SEC_USER_CHANGE_PASSWORD {

        string _USER_NAME;

        public string USER_NAME {
            get { return _USER_NAME; }
            set { _USER_NAME = value; }
        }

        string _OLD_PASSWORD;

        public string OLD_PASSWORD {
            get { return _OLD_PASSWORD; }
            set { _OLD_PASSWORD = value; }
        }

        string _NEW_PASSWORD;

        public string NEW_PASSWORD {
            get { return _NEW_PASSWORD; }
            set { _NEW_PASSWORD = value; }
        }

        string _RETYPE_PASSWORD;

        public string RETYPE_PASSWORD {
            get { return _RETYPE_PASSWORD; }
            set { _RETYPE_PASSWORD = value; }
        }

    }

}
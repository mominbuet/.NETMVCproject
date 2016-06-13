using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OG_SLN
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name="Old Password")]
        public string OLD_PASS { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NEW_PASS { get; set; }

        [Required]
        [Display(Name = "Retype New Password")]
        public string RE_NEW_PASS { get; set; }
    }
}
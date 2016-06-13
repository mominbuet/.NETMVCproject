using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace OG_SLN
{
    [MetadataType(typeof(TRN_USER_SPECIMEN_Metadata))]
    public partial class TRN_USER_SPECIMEN
    {
        public bool ACTIVE_STATUS {
            get {
                return IS_ACTIVE == 1;
            }
            set {
                IS_ACTIVE = value ? 1 : 0;
            }
        }
        public decimal edit_ID { get; set; }
        /*public  string uname
        {
            get
            {
                return new OGDBEntities().SEC_USERS.Where(u => u.USER_NO == USER_NO).Single().USER_NAME;
            }
            set {
                new OGDBEntities().SEC_USERS.Where(u => u.USER_NO == USER_NO).Single().USER_NAME = value;
            }
        }*/
        //public SpecimenSearch spec_search { get; set; }
    }

    public class TRN_USER_SPECIMEN_Metadata
    {
        [Required]
        [Display(Name = "Active User")]
        public bool ACTIVE_STATUS { get; set; }
        [Required]
        [Display(Name = "Edit Mode")]
        public decimal edit_ID { get; set; }
        /*[Display(Name = "User name")]
        public string uname { get; set; }*/

       /* [Display(Name = "Specimen Search")]
        public SpecimenSearch spec_search { get; set; }*/
    }
   
}
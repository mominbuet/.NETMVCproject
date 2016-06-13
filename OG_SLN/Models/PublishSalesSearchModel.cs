using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace OG_SLN.Models
{
    public class PublishSalesSearchModel
    {

       public decimal? USER_NO { get; set; }
       public decimal? USER_PARENT_NO { get; set; }
       public decimal? PUBLISH_NO { get; set; }
       public decimal? IS_CONFIRM { get; set; }
       public DateTime? Search_Date_From { get; set; }
       public DateTime? Search_Date_To { get; set; }
       public string user_not_confirmed { get; set; }

       public string Comments { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    public class temp_TRN_USER_PROMO_ITEM
    {
        public decimal user_spec_no;
        public string active;
        public DateTime assignDate;
        public DateTime? targetDateFrom, targetDateTo;
        public decimal userNo;
        public string username;
        private OGDBEntities db = new OGDBEntities();
        public temp_TRN_USER_PROMO_ITEM(decimal user_spec_no, decimal? active, DateTime assignDate, DateTime? targetDateFrom, DateTime? targetDateTo, decimal userNo)
        {
            this.user_spec_no = user_spec_no;
            this.active = (active==1) ? "Active" : "Inactive";
            this.assignDate = assignDate;
            this.targetDateFrom = targetDateFrom;
            this.targetDateTo = targetDateTo;
            this.userNo = userNo;
            this.username = db.SEC_USERS.Single(t=>t.USER_NO==userNo).USER_NAME;
        }
    }
}
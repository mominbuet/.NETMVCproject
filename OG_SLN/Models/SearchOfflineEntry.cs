using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    public class SearchOfflineEntry
    {
        public decimal? dcrType, USER_NO, verify;
        public string userName, userContact;
        public DateTime? dateOffFrom, dateOffTo;
        public bool dirty = true;
        public SearchOfflineEntry()
        {
            dateOffFrom = DateTime.Now; dateOffTo = DateTime.Now;
            dirty = false;
        }
    }
    public class SearchOfflineExpense
    {
        public decimal? dcrType, USER_NO, verify;
        public string userName, userContact,uptype;
        public DateTime? dateOffFrom, dateOffTo;
        public bool dirty = true;
        public SearchOfflineExpense()
        {
            dateOffFrom = DateTime.Now; dateOffTo = DateTime.Now;
            dirty = false; uptype = "all";
        }
    }
    
    public class SearchDCRCostApproval
    {
        public decimal? dcrType, USER_NO, verify;
        public string userName, userContact;
        public DateTime? dateOffFrom, dateOffTo;
        public bool dirty = true;
        public decimal? fare_cost;
        public SearchDCRCostApproval()
        {
            dateOffFrom = DateTime.Now; dateOffTo = DateTime.Now;
            dirty = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    [Serializable]
    public class SearchExpense
    {
        public decimal? userNo;
        public string userName, userContact;
        public DateTime? dateExpFrom, dateExpTo;
         public bool dirty=true;
         public SearchExpense()
        {
            dirty = false;
        }
    }
}
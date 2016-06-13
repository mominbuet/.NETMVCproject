using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
[Serializable]
    public class PromotionalItemSearch
    {
    public decimal? userNo;
 public decimal? userno;
        public string isactive;
        public decimal? promotional_item_no;
        public string user_name;
        public DateTime? AssignDateTo;
        public DateTime? AssignDateFrom;
        public bool dirty=true;
        public PromotionalItemSearch()
        {
            dirty = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    [Serializable]
    public class DCRSearch
    {
        public decimal? ZMNo,userNo;
        public string ZMName, institution, RefZMName;
        public DateTime? DateTo;
        public DateTime? DateFrom;
        public bool dirty=true;
        public DCRSearch()
        {
            dirty = false;
        }
    }
}
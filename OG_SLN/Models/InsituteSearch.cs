using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    [Serializable]
    public class InsituteSearch
    {
        public string insName,  F_INSTITUTION_DB_ID, EIIN_NUMBER;
        public decimal? insID = null;
        public bool dirty=true;
        public InsituteSearch()
        {
            dirty = false;
        }
    }
}
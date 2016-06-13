using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    [Serializable]
    public class SpecimenDistSearch
    {
        public decimal? userno, USER_NO;
        public string isactive;
        public decimal? specimen_no;
        public string user_name;
        public DateTime? AssignDateTo;
        public DateTime? AssignDateFrom;
        public bool dirty=true;
        public SpecimenDistSearch()
        {
            dirty = false;
        }
    }
    [Serializable]
    public class SpecimenSearch
    {
        public string sname;
        public decimal? sno;
        public string scode;
        public decimal? bookType;
        public DateTime? activeTo;
        public DateTime? activeFrom;
        public bool dirty = true;
        public SpecimenSearch()
        {
            dirty = false;
        }
    }
}
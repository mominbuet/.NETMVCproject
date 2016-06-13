using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    [Serializable]
    public class temp_TRN_USER_SPECIMEN_DET
    {
        public decimal specimenid;
        public bool active;
        public decimal qty;
        public bool deleted;
        public temp_TRN_USER_SPECIMEN_DET(decimal specid, bool active_param, decimal qty)
        {
            this.specimenid = specid;
            this.active = active_param;
            this.qty = qty;
            this.deleted = false;
        }
        public void set_deleted() {
            this.deleted = true;
        }
    }
}
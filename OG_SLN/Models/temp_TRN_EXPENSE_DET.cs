using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    [Serializable]
    public class temp_TRN_EXPENSE_DET
    {
        public decimal exp_type, amount;
        public string vendor, comments,sessionName;
        public bool set_deleted;
        public decimal? editID;
        public temp_TRN_EXPENSE_DET(decimal exp_type, decimal amount, string vendor, string comments)
        {
            this.amount = amount; this.comments = comments;
            this.exp_type = exp_type; this.vendor = vendor;
            this.sessionName = exp_type + "-" + amount + "-" + vendor;
            this.set_deleted = false;
        }
        public temp_TRN_EXPENSE_DET(decimal exp_type, decimal amount, string vendor, string comments,decimal? editID)
        {
            this.amount = amount; this.comments = comments;
            this.exp_type = exp_type; this.vendor = vendor;
            this.set_deleted = false;
            this.sessionName = exp_type + "-" + amount + "-" + vendor; 
            this.editID = editID;
        }
        public void setdeleted()
        {
            this.set_deleted = true;
        }
    }
}
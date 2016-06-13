using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    public class temp_TRN_DCR_DET
    {
        public decimal? type_id;
        public decimal editID;
        public string type;
        public string RcvType;
        public decimal? qty;
        public decimal? teacher_no;
        public string teacher_mobile;
        public decimal? client_no;
        public string client_mobile;
        public decimal?  is_behalf;
        public string BEHALF_MOBILE;
        public bool set_deleted;
        public temp_TRN_DCR_DET(decimal? typeid,string type,decimal? qty,decimal? teacherno,decimal? isbehalf,
            string teachermobile, string BEHALFMOBILE, string RcvType,decimal editID)
        {
            this.editID = editID;
            type_id = typeid; this.type = type; this.qty = qty; 
            is_behalf = isbehalf; BEHALF_MOBILE = BEHALFMOBILE;
            this.RcvType = RcvType;
            teacherno = (teacherno == 0) ? null : teacherno;
            if (RcvType == "teacher")
            {
                teacher_mobile = teachermobile; teacher_no = teacherno;
                client_no = null; client_mobile = "";
            }
            else {

                client_no = teacherno; client_mobile = teachermobile;
                teacher_mobile = ""; teacher_no = null;
            }
            this.set_deleted = false;
        }
        public void setdeleted()
        {
            this.set_deleted = true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace OG_SLN.Models {

    #region enum types
    public enum ApproveType {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
    }

    public enum UserType { 
        Super_Adminsitrator = 1,
        Administrator = 2, 
        Senior_Manager = 3, 
        Area_Manager = 4, 
        Zonal_Manager = 5,
        Agents = 6,
        General_User = 7,
        Divisional_Manager = 8,
        Marketing_Head = 9
    }

    public enum EntryState { 
        Added = 1, 
        Updated = 2, 
        Deleted = 3,
    }

    public enum ENTRY_MODE { 
        OFFLINE = 1,
        ONLINE = 0,
    }

    public enum LOGIN_MSG_TYPE { 
        NOTHING = 0, 
        INVALID_LOGIN = 1, 
        INVALID_APP_VERSION = 2,
    }

    #endregion

    public class CustomValidator {

        public static string GetRequestIpAddress() {
            string ip;
            string ret_ip;
            ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip)) {
                string[] ipRange = ip.Split(',');
                string trueIP = ipRange[0].Trim();
                ret_ip = trueIP;
            } else {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                ret_ip = ip;
            }
            return ret_ip;
        }

        public static string GetDeviceId() {
            string device_id = string.Empty;
            device_id = HttpContext.Current.Request.ServerVariables["x-avantgo-deviceid"];
            return device_id;
        }

        public static string GetWebServerId() {
            string ws_id = string.Empty;
            ws_id = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            return ws_id;
        }
        
    }
}
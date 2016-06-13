using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OG_SLN.Models;

namespace OG_SLN {

    [Serializable]
    public class USER_INFO {
        string _USER_NAME;

        public string USER_NAME {
            get { return _USER_NAME; }
            set { _USER_NAME = value; }
        }
    }
}
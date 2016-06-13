using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN
{
    public class CrashReport
    {

        public string _DEVICE_ID
        {
            get { return _DEVICE_ID; }
            set { _DEVICE_ID = value; }
        }

        public string _STACKTRACE
        {
            get { return _STACKTRACE; }
            set { _STACKTRACE = value; }
        }
        
    }
}
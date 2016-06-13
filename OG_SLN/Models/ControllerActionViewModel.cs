using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OG_SLN.Models
{
    public class ControllerActionViewModel
    {
        public string controllerName { get; set; }
        public string actionName { get; set; }

        public bool IsAutoInclude { get; set; }
        public bool IsActive { get; set; }

        public bool IsPublic { get; set; }

        public bool IsMenu { get; set; }
        public string menuName { get; set; }
        public string details { get; set; }
    }
}
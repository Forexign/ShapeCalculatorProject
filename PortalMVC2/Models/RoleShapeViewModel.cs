using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalMVC2.Models
{
    public class RoleShapeViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool CanCalculateDikdortgen { get; set; }
        public bool CanCalculateKare { get; set; }
        public bool CanCalculateDaire { get; set; }
        public bool CanCalculateSilindir { get; set; }
        public bool CanCalculateKup { get; set; }
    }

}
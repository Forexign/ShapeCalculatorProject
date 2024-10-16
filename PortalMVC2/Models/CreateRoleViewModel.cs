using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalMVC2.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
        public RoleShapeViewModel ShapePermissions { get; set; }
    }

}
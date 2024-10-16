using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class RoleShapePermission
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ShapeType { get; set; }

        public virtual IdentityRole Role { get; set; }
    }
}

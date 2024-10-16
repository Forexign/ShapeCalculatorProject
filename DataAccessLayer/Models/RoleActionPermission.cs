using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class RoleActionPermission
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }

}

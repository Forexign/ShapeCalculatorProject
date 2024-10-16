using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ShapeResult
    {
        public int Id { get; set; }
        public string ShapeType { get; set; }
        public double Area { get; set; }
        public double Volume { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public int InputsId { get; set; }
    }
}

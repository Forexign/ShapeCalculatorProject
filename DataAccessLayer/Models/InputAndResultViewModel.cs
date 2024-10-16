using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class InputAndResultViewModel
    {
        public int Id { get; set; }
        public string ShapeType { get; set; }
        public Nullable<int> Kenar { get; set; }
        [KisaKenarUzunKenarValidation]
        public Nullable<int> KisaKenar { get; set; }
        public Nullable<int> UzunKenar { get; set; }
        public Nullable<int> Yukseklik { get; set; }
        public Nullable<int> Yaricap { get; set; }
        public Nullable<bool> HesaplandiMi { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public Nullable<double> Area { get; set; }
        public Nullable<double> Volume { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
    }
}

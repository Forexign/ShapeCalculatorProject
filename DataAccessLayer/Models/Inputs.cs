using System;

namespace DataAccessLayer.Models
{
    public class Inputs
    {
        public int Id { get; set; }
        public string ShapeType { get; set; }
        public Nullable<int> Kenar { get; set; }
        public Nullable<int> KisaKenar { get; set; }
        public Nullable<int> UzunKenar { get; set; }
        public Nullable<int> Yukseklik { get; set; }
        public Nullable<int> Yaricap { get; set; }
        public Nullable<bool> HesaplandiMi { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Guid UserId { get; set; }
    }
}

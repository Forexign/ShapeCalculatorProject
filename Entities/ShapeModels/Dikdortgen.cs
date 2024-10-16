using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.ShapeModels
{
    public class Dikdortgen : Shape
    {
        public Dikdortgen(int kisaKenar, int uzunKenar)
        {
            ShapeType = "Dikdörtgen";
            KisaKenar = kisaKenar;
            UzunKenar = uzunKenar;
        }

        public int KisaKenar { get; set; }
        public int UzunKenar { get; set; }

        public override double CalculateArea()
        {

            return KisaKenar * UzunKenar;
        }

        public override double CalculateVolume()
        {
            return 0;
        }
    }
}
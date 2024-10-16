using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Entities.ShapeModels
{
    public class Kare : Shape
    {
        public Kare(int kenar)
        {
            ShapeType = "Kare";
            Kenar = kenar;
        }

        public int Kenar { get; set; }

        public override double CalculateArea()
        {

            return Kenar * Kenar;
        }

        public override double CalculateVolume()
        {
            return 0;
        }
    }
}
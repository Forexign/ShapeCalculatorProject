using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.ShapeModels
{
    public class Kup : Shape
    {
        public Kup(int kenar)
        {
            ShapeType = "Küp";
            Kenar = kenar;
        }

        public int Kenar { get; set; }

        public override double CalculateArea()
        {
            return 6 * Math.Pow(Kenar, 2);
        }

        public override double CalculateVolume()
        {
            return Math.Pow(Kenar, 3);
        }
    }
}
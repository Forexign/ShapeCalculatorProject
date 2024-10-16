using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ShapeModels
{
    public class Daire : Shape
    {
        public Daire(int yaricap)
        {
            ShapeType = "Daire";
            Yaricap = yaricap;
        }

        public int Yaricap { get; set; }

        public override double CalculateArea()
        {

            return Math.PI * Math.Pow(Yaricap, 2);
        }

        public override double CalculateVolume()
        {
            return 0;
        }
    }
}

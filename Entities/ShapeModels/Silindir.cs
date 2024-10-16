using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.ShapeModels
{
    public class Silindir : Shape
    {
        public Silindir(int yaricap, int yukseklik)
        {
            ShapeType = "Silindir";
            Yaricap = yaricap;
            Yukseklik = yukseklik;
        }

        public int Yaricap { get; set; }
        public int Yukseklik { get; set; }

        public override double CalculateArea()
        {

            return 2 * Math.PI * Yaricap * (Yaricap + Yukseklik);
        }

        public override double CalculateVolume()
        {

            return Math.PI * Math.Pow(Yaricap, 2) * Yukseklik;
        }
    }
}
using DataAccessLayer.Models;
using Entities.ShapeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondApi.Controllers
{
    public static class ShapeFactory
    {
        public static Shape CreateShape(Inputs shapeInput)
        {
            switch (shapeInput.ShapeType.ToLower())
            {
                case "kare":
                    return new Kare((int)shapeInput.Kenar);
                case "dikdortgen":
                    return new Dikdortgen((int)shapeInput.KisaKenar, (int)shapeInput.UzunKenar);
                case "daire":
                    return new Daire((int)shapeInput.Yaricap);
                case "kup":
                    return new Kup((int)shapeInput.Kenar);
                case "silindir":
                    return new Silindir((int)shapeInput.Yaricap, (int)shapeInput.Yukseklik);
                default:
                    throw new ArgumentException("Geçersiz şekil türü");
            }
        }
    }

}
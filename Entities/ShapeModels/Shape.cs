using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.ShapeModels
{
    public abstract class Shape
    {
        public int Id { get; set; }
        public string ShapeType { get; set; }
        abstract public double CalculateArea();
        abstract public double CalculateVolume();
    }
}
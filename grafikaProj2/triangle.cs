using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektGrafika
{
    public struct Triangle
    {
        public VectorMethods[] points;
        public Color color;
        public float zValue;

        public Triangle(int n)
        {
            points = new VectorMethods[n];
            color = Color.Black;
            zValue = 0;
        }
    }
}

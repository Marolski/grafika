using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektGrafika
{
    public class Draw
    {
        public void DrawTriangle(Triangle triangle, Bitmap bitmap, Color color)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            SolidBrush contourBrush = new SolidBrush(Color.White);
            SolidBrush fillBrush = new SolidBrush(color);
            Pen pen = new Pen(contourBrush);

            Point[] points = new Point[3];

            for (int i = 0; i < triangle.points.Length; i++)
            {
                int x = (int)Math.Round(triangle.points[i].x);
                int y = (int)Math.Round(triangle.points[i].y);
                Point p = new Point(x, y);
                points[i] = p;

            }
            //rysowanie trojkątów i wypełananie za pomoca fillPolygen
            graphics.FillPolygon(fillBrush,points);
            graphics.DrawLine(pen, triangle.points[0].x, triangle.points[0].y, triangle.points[1].x, triangle.points[1].y);
            graphics.DrawLine(pen, triangle.points[1].x, triangle.points[1].y, triangle.points[2].x, triangle.points[2].y);
            graphics.DrawLine(pen, triangle.points[2].x, triangle.points[2].y, triangle.points[0].x, triangle.points[0].y);


        }
        //zmiana kolory w zależności od współczynika dp
        public Color ChangeColor(Color color, float colorChanging)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            red = (255 - red) * colorChanging + red;
            green = (255 - green) * colorChanging + green;
            blue = (255 - blue) * colorChanging + blue;

            return Color.FromArgb((int)red, (int)green, (int)blue);
        }
    }
}

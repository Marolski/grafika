using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace projektGrafika
{
    public struct GetMesh
    {
        public List<Triangle> triangles;

        public GetMesh(string fileName)
        {
            List<VectorMethods> vectors = new List<VectorMethods>();
            triangles = new List<Triangle>();

            StreamReader sr = new StreamReader(fileName);
            string line = sr.ReadLine();

            while (String.IsNullOrEmpty(line) ==false)
            {
                //sprawdzenie połozenia pojedyńczego wektora 
                if (line[0] == 'v')
                {
                    VectorMethods v = new VectorMethods();

                    string[] sections = line.Split(' ');

                    v.x = float.Parse(sections[1], CultureInfo.InvariantCulture.NumberFormat);
                    v.y = float.Parse(sections[2], CultureInfo.InvariantCulture.NumberFormat);
                    v.z = float.Parse(sections[3], CultureInfo.InvariantCulture.NumberFormat);

                    vectors.Add(v);
                }
                //wektory z przodu (indeksy punktów znajdujących się na froncie)
                if (line[0]=='f')
                {
                    int[] f = new int[3];
                    string[] sections = line.Split(' ');

                    f[0] = Convert.ToInt32(sections[1]);
                    f[1] = Convert.ToInt32(sections[2]);
                    f[2] = Convert.ToInt32(sections[3]);

                    Triangle t = new Triangle
                    {
                        points = new VectorMethods[]
                        {
                            vectors[f[0]-1],
                            vectors[f[1]-1],
                            vectors[f[2]-1]
                        }
                    };
                    triangles.Add(t);
                }
                line = sr.ReadLine();
            }
        }
    }
}

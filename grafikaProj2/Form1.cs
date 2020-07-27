using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projektGrafika
{
    public partial class Form1 : Form
    {

        //Budowanie sceny za pomoca pliku .obj
        GetMesh scene = new GetMesh("siatka.obj");

        public Bitmap bitmap;
        int forward = 0;

        MatrixMethods matProjection = new MatrixMethods(4);
        MatrixMethods matRotationZ = new MatrixMethods(4);
        MatrixMethods matRotationX = new MatrixMethods(4);
        MatrixMethods matTranslation = new MatrixMethods(4);
        MatrixMethods matWorld = new MatrixMethods(4);

        public VectorMethods vCamera = new VectorMethods();
        public VectorMethods vForward = new VectorMethods();
        float rotZ = 21;
        float rotX = 21;
        float angleZ = 90;
        float angleX = 90;


        Draw draw = new Draw();
        float zPosition=7.0f;


        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
            Scene();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Scene()
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            SolidBrush brush = new SolidBrush(Color.Black);
            graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);

            //Tworzenie macierzy rzutowania na 2d
            float nearDistance = 0.1f;
            float farDistance = 1000.0f;
            float angle = 90.0f; //kąt widzenia 
            float aspectRatio = (float)bitmap.Height / (float)bitmap.Width; // wspołczynnik proporcji wyświetlanego ekranu (bitmapy)

            matProjection = MatrixMethods.Projection(angle, aspectRatio, nearDistance, farDistance);


            //Tworzenie macierzy rotacji wkierunkach Z, X
            matRotationZ = MatrixMethods.RotationZ(angleZ);
            matRotationX = MatrixMethods.RotationX(angleX);

            List<Triangle> trianglesToRaster = new List<Triangle>();


            matTranslation = MatrixMethods.Translation(0.0f, 0.0f, zPosition);

            //nowa pozycja po rotacji
            matWorld = MatrixMethods.MatrixIdentity();
            matWorld = MatrixMethods.MultiplyMatrix(matRotationZ, matRotationX);
            matWorld = MatrixMethods.MultiplyMatrix(matWorld, matTranslation);

            //Vector kierunku poruszania się kamery.
            VectorMethods vLookDiraction = new VectorMethods();

            VectorMethods vUp = new VectorMethods { x = 0, y = 1, z = 0 };
            VectorMethods vTarget = new VectorMethods { x = 0, y = 0, z = 5 };

            MatrixMethods matCameraRot = MatrixMethods.RotationY(0);
            vLookDiraction = MatrixMethods.Multiply(matCameraRot, vTarget);
            vTarget = VectorMethods.AddVectors(vCamera, vLookDiraction);

            MatrixMethods matCamera = MatrixMethods.CalculatePoint(vCamera, vTarget, vUp);

            //Tworzenie macierzy widoku kamery
            MatrixMethods matView = MatrixMethods.InverseMatrix(matCamera);



            foreach (var triangle in scene.triangles)
            {

                Triangle projectedTriangle, transformedTriangle, viewedTriangle;

                projectedTriangle = new Triangle(3);
                transformedTriangle = new Triangle(3);
                viewedTriangle = new Triangle(3);
                //Tworzenie vektorów transormacji
                for (int i = 0; i < 3; i++)
                {
                    transformedTriangle.points[i] = MatrixMethods.Multiply(matWorld, triangle.points[i]);

                }

                //Wygląd 'trojkatow' od frontu, z widoku kamery
                VectorMethods normal, edge1, edge2;

                edge1 = VectorMethods.SubVectors(transformedTriangle.points[1], transformedTriangle.points[0]);
                edge2 = VectorMethods.SubVectors(transformedTriangle.points[2], transformedTriangle.points[0]);

                normal = VectorMethods.CrossProduct(edge1, edge2);
                normal = VectorMethods.VectorNormalise(normal);


                // stworzenie wektora z promieniem kamery
                VectorMethods cameraRay = VectorMethods.SubVectors(transformedTriangle.points[0], vCamera);

                if (VectorMethods.ScalarProduct(normal, cameraRay) < 0.0f)
                {

                    //Kierunek światła i cienowanie
                    VectorMethods lightDirection = new VectorMethods
                    {
                        x = 0.0f,
                        y = 1.0f,
                        z = -1.0f
                    };
                    lightDirection = VectorMethods.VectorNormalise(lightDirection);

                    //wspołczynnik cieniowania
                    float dp = (float)Math.Max(0.1f, VectorMethods.ScalarProduct(lightDirection, normal));

                    Color c = draw.ChangeColor(Color.Black, dp);

                    //zmiana wektora transormacji korzystając z widoku kamery
                    viewedTriangle.points[0] = MatrixMethods.Multiply(matView, transformedTriangle.points[0]);
                    viewedTriangle.points[1] = MatrixMethods.Multiply(matView, transformedTriangle.points[1]);
                    viewedTriangle.points[2] = MatrixMethods.Multiply(matView, transformedTriangle.points[2]);


                    //rzutowanie wektorow(trojkątów) na 2d
                    for (int i = 0; i < 3; i++)
                    {
                        projectedTriangle.points[i] = MatrixMethods.Multiply(matProjection, viewedTriangle.points[i]);

                    }
                    for (int i = 0; i < 3; i++)
                    {
                        projectedTriangle.points[i] = VectorMethods.DivideByNumber(projectedTriangle.points[i], projectedTriangle.points[i].w);

                    }
                    projectedTriangle.color = c;


                    //przeskalowanie figur na ekranie(współrzedne x,y)
                    VectorMethods vectorOffSetView = new VectorMethods { x = 1, y = 1, z = 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        projectedTriangle.points[i] = VectorMethods.AddVectors(projectedTriangle.points[i], vectorOffSetView);

                    }

                    //skalowanie do wielkości wyświetlanej bitmapy
                    projectedTriangle.points[0].x *= 0.5f * (float)bitmap.Width;
                    projectedTriangle.points[0].y *= 0.5f * (float)bitmap.Height;
                    projectedTriangle.points[1].x *= 0.5f * (float)bitmap.Width;
                    projectedTriangle.points[1].y *= 0.5f * (float)bitmap.Height;
                    projectedTriangle.points[2].x *= 0.5f * (float)bitmap.Width;
                    projectedTriangle.points[2].y *= 0.5f * (float)bitmap.Height;

                    //Tworzeni listy do sortowania trojkątów
                    projectedTriangle.zValue = (projectedTriangle.points[0].z + projectedTriangle.points[1].z + projectedTriangle.points[2].z) / 3.0f;
                    trianglesToRaster.Add(projectedTriangle);

                }


            }
            //sortowanie trojkatów względem wspołrzednej z
            trianglesToRaster.Sort((a, b) => b.zValue.CompareTo(a.zValue));
            //rysowanie trojkatow
            foreach (var triangle in trianglesToRaster)
            {
                draw.DrawTriangle(triangle, bitmap, triangle.color);
                pictureBox1.Image = bitmap;
            }
        }
       

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                vCamera.y -= 0.05f;
                Scene();
            }
            if (e.KeyCode == Keys.Down)
            {
                vCamera.y += 0.05f;
                Scene();

            }
            if (e.KeyCode == Keys.Right)
            {
                vCamera.x += 0.05f;
                Scene();

            }
            if (e.KeyCode == Keys.Left)
            {
                vCamera.x -= 0.05f;
                Scene();

            }
            if (e.KeyCode == Keys.Q)
            {
                angleZ =0.1f*rotZ;
                rotZ++;
                Scene();

            }
            if (e.KeyCode == Keys.W)
            {
                angleX = 0.1f * rotX;
                rotX++;
                Scene();

            }
            if (e.KeyCode == Keys.P)
            {
                if (forward<6)
                {
                    zPosition = zPosition - 0.5f;
                    Scene();
                    forward++;
                }
            }

            if (e.KeyCode == Keys.O)
            {
                zPosition = zPosition + 0.5f;
                Scene();
                forward--;
            }
        }
    }
}

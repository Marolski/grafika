using System;

namespace projektGrafika
{
    struct MatrixMethods
    {
        public float[,] matrix;

        public MatrixMethods(int n)
        {
            matrix = new float[n, n];
        }

        public static MatrixMethods MatrixIdentity()
        {
            MatrixMethods newIdentity = new MatrixMethods(4);
            for (int i = 0; i < 4; i++)
            {
                newIdentity.matrix[i, i] = 1.0f;

            }
            return newIdentity;

        }

        public static MatrixMethods RotationX(float angle)
        {
            MatrixMethods rotationX = new MatrixMethods(4);
            rotationX.matrix[0, 0] = 1;
            rotationX.matrix[1, 1] = (float)Math.Cos(angle);
            rotationX.matrix[1, 2] = (float)Math.Sin(angle);
            rotationX.matrix[2, 1] = (float)(Math.Sin(angle) * -1);
            rotationX.matrix[2, 2] = (float)Math.Cos(angle);
            rotationX.matrix[3, 3] = 1;
            return rotationX;
        }

        public static MatrixMethods RotationY(float angle)
        {
            MatrixMethods rotationY = new MatrixMethods(4);
            rotationY.matrix[0, 0] = (float)Math.Cos(angle);
            rotationY.matrix[0, 2] = (float)Math.Sin(angle);
            rotationY.matrix[2, 0] = (float)(Math.Sin(angle) * -1);
            rotationY.matrix[1, 1] = 1.0F;
            rotationY.matrix[2, 2] = (float)Math.Cos(angle * 0.5F);
            rotationY.matrix[3, 3] = 1.0F;
            return rotationY;
        }

        public static MatrixMethods RotationZ(float angle)
        {
            MatrixMethods rotationZ = new MatrixMethods(4);

            rotationZ.matrix[0, 0] = (float)Math.Cos(angle);
            rotationZ.matrix[0, 1] = (float)Math.Sin(angle);
            rotationZ.matrix[1, 0] = (float)(Math.Sin(angle) * -1);
            rotationZ.matrix[1, 1] = (float)Math.Cos(angle);
            rotationZ.matrix[2, 2] = 1;
            rotationZ.matrix[3, 3] = 1;
            return rotationZ;
        }
        public static MatrixMethods Projection(float angle, float aspectRatio, float nearDistance, float farDistance)
        {
            float fFovRad = 1.0f / (float)Math.Tan(angle * 0.5f / 180.0f * 3.14159f);

            MatrixMethods projected = new MatrixMethods(4);
            projected.matrix[0, 0] = aspectRatio * fFovRad;
            projected.matrix[1, 1] = fFovRad;
            projected.matrix[2, 2] = farDistance / (farDistance - nearDistance);
            projected.matrix[3, 2] = (-farDistance * nearDistance) / (farDistance - nearDistance);
            projected.matrix[2, 3] = 1.0f;
            projected.matrix[3, 3] = 0.0f;
            return projected;
        }

        public static MatrixMethods Translation(float x, float y, float z)
        {
            MatrixMethods translated = new MatrixMethods(4);

            translated.matrix[0, 0] = 1.0f;
            translated.matrix[1, 1] = 1.0f;
            translated.matrix[2, 2] = 1.0f;
            translated.matrix[3, 3] = 1.0f;
            translated.matrix[3, 0] = x;
            translated.matrix[3, 1] = y;
            translated.matrix[3, 2] = z;
            return translated;
        }

        public static VectorMethods Multiply(MatrixMethods m, VectorMethods v)
        {
            VectorMethods result = new VectorMethods();
            result.x = v.x * m.matrix[0, 0] + v.y * m.matrix[1, 0] + v.z * m.matrix[2, 0] + v.w * m.matrix[3, 0];
            result.y = v.x * m.matrix[0, 1] + v.y * m.matrix[1, 1] + v.z * m.matrix[2, 1] + v.w * m.matrix[3, 1];
            result.z = v.x * m.matrix[0, 2] + v.y * m.matrix[1, 2] + v.z * m.matrix[2, 2] + v.w * m.matrix[3, 2];
            result.w = v.x * m.matrix[0, 3] + v.y * m.matrix[1, 3] + v.z * m.matrix[2, 3] + v.w * m.matrix[3, 3];

            return result;

        }

        public static MatrixMethods MultiplyMatrix(MatrixMethods m1, MatrixMethods m2)
        {
            MatrixMethods multiplied = new MatrixMethods(4);
            for (int c = 0; c < 4; c++)
                for (int r = 0; r < 4; r++)
                    multiplied.matrix[r, c] = m1.matrix[r, 0] * m2.matrix[0, c] + m1.matrix[r, 1] * m2.matrix[1, c] + m1.matrix[r, 2] * m2.matrix[2, c] + m1.matrix[r, 3] * m2.matrix[3, c];
            return multiplied;
        }

        public static MatrixMethods CalculatePoint(VectorMethods actualVector, VectorMethods targetVector, VectorMethods upVector)
        {
            //nowy kierunek w przód
            VectorMethods newForward = VectorMethods.SubVectors(targetVector, actualVector);
            newForward = VectorMethods.VectorNormalise(newForward);

            //nowy kierunek w góre
            VectorMethods tmp = VectorMethods.MultiplyByNumber(newForward, VectorMethods.ScalarProduct(upVector, newForward));
            VectorMethods newUp = VectorMethods.SubVectors(upVector, tmp);
            newUp = VectorMethods.VectorNormalise(newUp);

            //nowy  w prawo
            VectorMethods newRight = VectorMethods.CrossProduct(newUp, newForward);

            //tworzenie macierzy przekształcenia 
            MatrixMethods translated = new MatrixMethods(4);
            translated.matrix[0, 0] = newRight.x;
            translated.matrix[0, 1] = newRight.y;
            translated.matrix[0, 2] = newRight.z;
            translated.matrix[0, 3] = 0.0f;

            translated.matrix[1, 0] = newUp.x;
            translated.matrix[1, 1] = newUp.y;
            translated.matrix[1, 2] = newUp.z;
            translated.matrix[1, 3] = 0.0f;

            translated.matrix[2, 0] = newForward.x;
            translated.matrix[2, 1] = newForward.y;
            translated.matrix[2, 2] = newForward.z;
            translated.matrix[2, 3] = 0.0f;

            translated.matrix[3, 0] = actualVector.x;
            translated.matrix[3, 1] = actualVector.y;
            translated.matrix[3, 2] = actualVector.z;
            translated.matrix[3, 3] = 1.0f;

            return translated;


        }
        //macierz odwrocona
        public static MatrixMethods InverseMatrix(MatrixMethods m)
        {
            MatrixMethods inverseMat = new MatrixMethods(4);
            inverseMat.matrix[0, 0] = m.matrix[0, 0];
            inverseMat.matrix[0, 1] = m.matrix[1, 0];
            inverseMat.matrix[0, 2] = m.matrix[2, 0];
            inverseMat.matrix[0, 3] = 0.0f;

            inverseMat.matrix[1, 0] = m.matrix[0, 1];
            inverseMat.matrix[1, 1] = m.matrix[1, 1];
            inverseMat.matrix[1, 2] = m.matrix[2, 1];
            inverseMat.matrix[1, 3] = 0.0f;

            inverseMat.matrix[2, 0] = m.matrix[0, 2];
            inverseMat.matrix[2, 1] = m.matrix[1, 2];
            inverseMat.matrix[2, 2] = m.matrix[2, 2];
            inverseMat.matrix[2, 3] = 0.0f;

            inverseMat.matrix[3, 0] = -(m.matrix[3, 0] * inverseMat.matrix[0, 0] + m.matrix[3, 1] * inverseMat.matrix[1, 0] + m.matrix[3, 2] * inverseMat.matrix[2, 0]);
            inverseMat.matrix[3, 1] = -(m.matrix[3, 0] * inverseMat.matrix[0, 1] + m.matrix[3, 1] * inverseMat.matrix[1, 1] + m.matrix[3, 2] * inverseMat.matrix[2, 1]);
            inverseMat.matrix[3, 2] = -(m.matrix[3, 0] * inverseMat.matrix[0, 2] + m.matrix[3, 1] * inverseMat.matrix[1, 2] + m.matrix[3, 2] * inverseMat.matrix[2, 2]);
            inverseMat.matrix[3, 3] = 1.0f;

            return inverseMat;



        }

    }
}

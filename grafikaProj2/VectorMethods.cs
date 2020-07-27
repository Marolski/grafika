using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektGrafika
{
    public class VectorMethods
    {
        public float x, y, z;
        public float w = 1;

        public static float LengthOfVector(VectorMethods v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static VectorMethods VectorNormalise(VectorMethods v)
        {
            float length = LengthOfVector(v);
            VectorMethods normal = new VectorMethods
            {
                x = v.x / length,
                y = v.y / length,
                z = v.z / length
            };
            return normal;
        }
        
        public static VectorMethods DivideByNumber(VectorMethods v, float a)
        {
            VectorMethods vec = new VectorMethods
            {
                x = v.x / a,
                y = v.y / a,
                z = v.z / a

            };
            return vec;
        }

        public static  VectorMethods MultiplyByNumber(VectorMethods v, float a)
        {
            VectorMethods vec = new VectorMethods
            {
                x = v.x * a,
                y = v.y * a,
                z = v.z * a

            };
            return vec;
        }

        public static VectorMethods AddVectors(VectorMethods v1, VectorMethods v2)
        {
            VectorMethods vec = new VectorMethods
            {
                x = v1.x + v2.x,
                y = v1.y + v2.y,
                z = v1.z + v2.z

            };
            return vec;
        }

        public static VectorMethods SubVectors(VectorMethods v1, VectorMethods v2)
        {
            VectorMethods vec = new VectorMethods
            {
                x = v1.x - v2.x,
                y = v1.y - v2.y,
                z = v1.z - v2.z

            };
            return vec;
        }

        public static float ScalarProduct(VectorMethods v1, VectorMethods v2)
        {
            float dot = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
            return dot;
        }

        public static VectorMethods CrossProduct(VectorMethods v1, VectorMethods v2)
        {
            VectorMethods vec = new VectorMethods();

            vec.x = v1.y * v2.z - v1.z * v2.y;
            vec.y = v1.z * v2.x - v1.x * v2.z;
            vec.z = v1.x * v2.y - v1.y * v2.x;

            return vec;
        }
    }
}

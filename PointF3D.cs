using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr3grphics
{
    public struct PointF3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public PointF3D(float x = 0,float y = 0,float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static bool operator ==(PointF3D p1,PointF3D p2)
        {
            return (p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z);
        }

        public static bool operator !=(PointF3D p1, PointF3D p2)
        {
            return (p1.X != p2.X || p1.Y != p2.Y || p1.Z != p2.Z);
        }
    }
}

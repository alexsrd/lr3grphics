using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr3grphics
{
    public struct Polygon3D
    {
        public PointF3D[] polygons { get; set; }

        public Polygon3D(PointF3D[] polygons)
        {
            this.polygons = polygons;
        }

        public PointF[] ProjectIsometric()
        {
            return ProjectionAffinity.projectionIsometric(polygons);
        }

        public PointF[] ProjectXY()
        {
            return ProjectionAffinity.projectionXY(polygons);
        }

        public PointF[] ProjectYZ()
        {
            return ProjectionAffinity.projectionYZ(polygons);
        }

        public PointF[] ProjectZX()
        {
            return ProjectionAffinity.projectionXZ(polygons);
        }

        public PointF3D[] Translate(PointF3D translation)
        {
            return ProjectionAffinity.Translate3D(polygons, translation.X, translation.Y, translation.Z);
        }

        public PointF3D[] RotateX(float Angle, PointF3D RotCentre)
        {
            return ProjectionAffinity.RotateX3D(polygons, Angle, RotCentre);
        }

        public PointF3D[] RotateY(float Angle, PointF3D RotCentre)
        {
            return ProjectionAffinity.RotateY3D(polygons, Angle, RotCentre);
        }

        public PointF3D[] RotateZ(float Angle, PointF3D RotCentre)
        {
            return ProjectionAffinity.RotateZ3D(polygons, Angle, RotCentre);
        }

        public PointF3D[] Scale(PointF3D ScalVal, PointF3D ScalCentre)
        {
            return ProjectionAffinity.Scale3D(polygons, ScalVal.X, ScalVal.Y, ScalVal.Z, ScalCentre);
        }
    }
}

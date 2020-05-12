using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr3grphics
{
    public struct Polygon2D
    {
        public PointF[] polygon { get; set; }

        public Polygon2D(PointF[] polygon)
        {
            this.polygon = polygon;
        }

        public PointF[] GetNormalizedPolygon()
        {
            PointF[] normalizedPolygon = new PointF[polygon.Length];

            for (int i = 0; i < polygon.Length; i++)
            {
                normalizedPolygon[i].X = Normalize.NormalizeX(polygon[i].X);
                normalizedPolygon[i].Y = Normalize.NormalizeY(polygon[i].Y);
            }

            return normalizedPolygon;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace lr3grphics
{
    public static class Normalize
    {
        public static float WinHeight;
        public static float WinWidth;
        public static float NormalizeX(float x)
        {
            return (x - DecartCoord.minBorder) * WinWidth / (DecartCoord.maxBorder - DecartCoord.minBorder);
        }
        public static float NormalizeY(float y)
        {
            return WinHeight - (y - DecartCoord.minBorder) * WinHeight / (DecartCoord.maxBorder - DecartCoord.minBorder);
        }

        public static float DenormalizeX(float x)
        {
            return x * (DecartCoord.maxBorder - DecartCoord.minBorder) / WinWidth + DecartCoord.minBorder;
        }

        public static float DenormalizeY(float y)
        {
            return DecartCoord.minBorder - (y - WinHeight) * (DecartCoord.maxBorder - DecartCoord.minBorder) / WinHeight;
        }
        public static PointF NormalizePoint(PointF p)
        {
            PointF result = new PointF();
            result.X = (p.X - DecartCoord.minBorder) * WinWidth / (DecartCoord.maxBorder - DecartCoord.minBorder);
            result.Y = WinHeight - (p.Y - DecartCoord.minBorder) * WinHeight / (DecartCoord.maxBorder - DecartCoord.minBorder);
            return result;
        }
    }
}

using MatrixLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr3grphics
{
    public static class ProjectionAffinity
    {
        public static PointF[] projectionIsometric(PointF3D[] obj)
        {
            PointF[] projected2D = new PointF[obj.Length];

            float radian30 = (float)(30 * Math.PI / 180);
            for (int i = 0; i < obj.Length; i++)
            {
                projected2D[i].X = (-obj[i].X + obj[i].Z) * (float)Math.Cos(radian30);
                projected2D[i].Y = obj[i].Y + (-obj[i].X - obj[i].Z) * (float)Math.Sin(radian30);
            }

            return projected2D;
        }

        public static PointF[] projectionXY(PointF3D[] obj)
        {
            PointF[] projected2D = new PointF[obj.Length];

            for (int i = 0; i < obj.Length; i++)
            {
                projected2D[i].X = obj[i].X;
                projected2D[i].Y = obj[i].Y;

            }
            return projected2D;
        }
        public static PointF[] projectionXZ(PointF3D[] obj)
        {
            PointF[] projected2D = new PointF[obj.Length];

            for (int i = 0; i < obj.Length; i++)
            {
                projected2D[i].X = obj[i].Z;
                projected2D[i].Y = obj[i].X;

            }
            return projected2D;
        }
        public static PointF[] projectionYZ(PointF3D[] obj)
        {
            PointF[] projected2D = new PointF[obj.Length];

            for (int i = 0; i < obj.Length; i++)
            {
                projected2D[i].X = obj[i].Y;
                projected2D[i].Y = obj[i].Z;

            }
            return projected2D;
        }

        public static Matrix Get3DTranslationMatrix(double x, double y, double z)
        {
            return new Matrix(new double[,]
            {
                {1f,    0f,    0f,  x },
                {0f,    1f,    0f,  y },
                {0f,    0f,    1f,  z },
                {0f,    0f,    0f,  1f }
            }
            );
        }

        public static PointF3D[] Translate3D(PointF3D[] obj, double x, double y, double z)
        {
            PointF3D[] TranslatedtedObj = new PointF3D[obj.Length];
            obj.CopyTo(TranslatedtedObj, 0);
            Matrix CurPoint;
            for (int i = 0; i < TranslatedtedObj.Length; i++)
            {
                CurPoint = Form3DCoordsMatrix(obj[i].X, obj[i].Y, obj[i].Z);
                CurPoint = Get3DTranslationMatrix(x, y, z).Multiply(CurPoint);
                TranslatedtedObj[i].X = (float)CurPoint.GetElementValue(0, 0);
                TranslatedtedObj[i].Y = (float)CurPoint.GetElementValue(1, 0);
                TranslatedtedObj[i].Z = (float)CurPoint.GetElementValue(2, 0);
            }

            return TranslatedtedObj;
        }

        public static Matrix Form3DCoordsMatrix(double x, double y, double z)
        {
            return new Matrix(new double[,] { { x }, { y }, { z }, { 1 } });
        }

        public static PointF3D[] RotateX3D(PointF3D[] obj, double angle, PointF3D RotationCentre)
        {
            PointF3D[] RotatedObj = new PointF3D[obj.Length];
            obj.CopyTo(RotatedObj, 0);

            RotatedObj = Translate3D(RotatedObj, -RotationCentre.X, -RotationCentre.Y, -RotationCentre.Z);

            Matrix CurPoint;
            for (int i = 0; i < RotatedObj.Length; i++)
            {
                CurPoint = Form3DCoordsMatrix(RotatedObj[i].X, RotatedObj[i].Y, RotatedObj[i].Z);
                CurPoint = GetXRotation3DMatrix(angle).Multiply(CurPoint);
                RotatedObj[i].X = (float)CurPoint.GetElementValue(0, 0);
                RotatedObj[i].Y = (float)CurPoint.GetElementValue(1, 0);
                RotatedObj[i].Z = (float)CurPoint.GetElementValue(2, 0);
            }

            RotatedObj = Translate3D(RotatedObj, RotationCentre.X, RotationCentre.Y, RotationCentre.Z);
            return RotatedObj;
        }

        public static PointF3D[] RotateY3D(PointF3D[] obj, double angle, PointF3D RotationCentre)
        {
            PointF3D[] RotatedObj = new PointF3D[obj.Length];
            obj.CopyTo(RotatedObj, 0);

            RotatedObj = Translate3D(RotatedObj, -RotationCentre.X, -RotationCentre.Y, -RotationCentre.Z);

            Matrix CurPoint;
            for (int i = 0; i < RotatedObj.Length; i++)
            {
                CurPoint = Form3DCoordsMatrix(RotatedObj[i].X, RotatedObj[i].Y, RotatedObj[i].Z);
                CurPoint = GetYRotation3DMatrix(angle).Multiply(CurPoint);
                RotatedObj[i].X = (float)CurPoint.GetElementValue(0, 0);
                RotatedObj[i].Y = (float)CurPoint.GetElementValue(1, 0);
                RotatedObj[i].Z = (float)CurPoint.GetElementValue(2, 0);
            }

            RotatedObj = Translate3D(RotatedObj, RotationCentre.X, RotationCentre.Y, RotationCentre.Z);
            return RotatedObj;
        }

        public static PointF3D[] RotateZ3D(PointF3D[] obj, double angle, PointF3D RotationCentre)
        {
            PointF3D[] RotatedObj = new PointF3D[obj.Length];
            obj.CopyTo(RotatedObj, 0);

            RotatedObj = Translate3D(RotatedObj, -RotationCentre.X, -RotationCentre.Y, -RotationCentre.Z);

            Matrix CurPoint;
            for (int i = 0; i < RotatedObj.Length; i++)
            {
                CurPoint = Form3DCoordsMatrix(RotatedObj[i].X, RotatedObj[i].Y, RotatedObj[i].Z);
                CurPoint = GetZRotation3DMatrix(angle).Multiply(CurPoint);
                RotatedObj[i].X = (float)CurPoint.GetElementValue(0, 0);
                RotatedObj[i].Y = (float)CurPoint.GetElementValue(1, 0);
                RotatedObj[i].Z = (float)CurPoint.GetElementValue(2, 0);
            }

            RotatedObj = Translate3D(RotatedObj, RotationCentre.X, RotationCentre.Y, RotationCentre.Z);
            return RotatedObj;
        }

        public static Matrix GetXRotation3DMatrix(double angle)
        {
            double AngleInRadian = angle * Math.PI / 180;
            double AngCos = Math.Cos(AngleInRadian);
            double AngSin = Math.Sin(AngleInRadian);
            return new Matrix(new double[,]
            {
                {1f,0f,        0f,         0f },
                {0f,AngCos,    -AngSin,    0f },
                {0f,AngSin,    AngCos,     0f},
                {0f,0f,        0f,         1f }
            }
            );
        }

        public static Matrix GetYRotation3DMatrix(double angle)
        {
            double AngleInRadian = angle * Math.PI / 180;
            double AngCos = Math.Cos(AngleInRadian);
            double AngSin = Math.Sin(AngleInRadian);
            return new Matrix(new double[,]
            {
                {AngCos,    0f, -AngSin,    0f },
                {0f,1f,        0f,         0f },
                {AngSin,    0f, AngCos,    0f},
                {0f,0f,        0f,         1f }
            }
            );
        }

        public static Matrix GetZRotation3DMatrix(double angle)
        {
            double AngleInRadian = angle * Math.PI / 180;
            double AngCos = Math.Cos(AngleInRadian);
            double AngSin = Math.Sin(AngleInRadian);
            return new Matrix(new double[,]
            {
                {AngCos,    -AngSin,0f,    0f },
                {AngSin,    AngCos, 0f,    0f},
                {0f,0f,        1f,         0f },
                {0f,0f,        0f,         1f }
            }
            );
        }

        public static Matrix Get3DScalingMatrix(double xScal, double yScal, double zScal)
        {
            return new Matrix(new double[,]
            {
                {xScal,  0f,             0f,             0f},
                {0f,            yScal,   0f,             0f},
                {0f,            0f,             zScal,   0f },
                {0f,            0f,             0f,             1f }
            }
            );
        }

        public static PointF3D[] Scale3D(PointF3D[] obj, double xScal, double yScal, double zScal, PointF3D ScalingCentre)
        {
            PointF3D[] ScaledtedObj = new PointF3D[obj.Length];
            obj.CopyTo(ScaledtedObj, 0);

            ScaledtedObj = Translate3D(ScaledtedObj, -ScalingCentre.X, -ScalingCentre.Y, -ScalingCentre.Z);

            Matrix CurPoint;
            for (int i = 0; i < ScaledtedObj.Length; i++)
            {
                CurPoint = Form3DCoordsMatrix(ScaledtedObj[i].X, ScaledtedObj[i].Y, ScaledtedObj[i].Z);
                CurPoint = Get3DScalingMatrix(xScal, yScal, zScal).Multiply(CurPoint);
                ScaledtedObj[i].X = (float)CurPoint.GetElementValue(0, 0);
                ScaledtedObj[i].Y = (float)CurPoint.GetElementValue(1, 0);
                ScaledtedObj[i].Z = (float)CurPoint.GetElementValue(2, 0);
            }

            ScaledtedObj = Translate3D(ScaledtedObj, ScalingCentre.X, ScalingCentre.Y, ScalingCentre.Z);

            return ScaledtedObj;
        }
    }
}

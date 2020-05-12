using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lr3grphics
{
    public partial class Form1 : Form
    {
        Polygon3D[] DefaultPyramid;
        Polygon3D[] pyramid;
        PointF[] normalizedIzometricAxes,
            normalizedXYAxes,
            normalizedXZAxes,
            normalizedYZAxes;
        PointF3D oldTransVal, TransVal;

        PointF3D DefaultObjCentre;
        PointF3D objCentre;

        float XRotValue, OldXRotValue,
            YRotValue, OldYRotValue,
            ZRotValue, OldZRotValue;
        float ScalVal = 1, OldScalVal = 1;

        PointF3D[] coordinateLines =
        {
            new PointF3D(0f,0f,0f),
            new PointF3D(15f, 0f, 0f),
            new PointF3D(0f, 15f, 0f),
            new PointF3D(0f, 0f, 15f)
        };

        static PointF3D[,] DefaultControlPoints = new PointF3D[4, 4]
            {
                { new PointF3D(0, 0, 0), new PointF3D(0, 20, 0), new PointF3D(0, 0, 0), new PointF3D(0, 0, 0) },
                { new PointF3D(20, 0, 0), new PointF3D(20, 20, 0), new PointF3D(0, 0, 0), new PointF3D(0, 0, 0) },
                { new PointF3D(0, 0, 0), new PointF3D(0, 0, 0), new PointF3D(0, 0, 0), new PointF3D(0, 0, 0) },
                { new PointF3D(0, 0, 0), new PointF3D(0, 0, 0), new PointF3D(0, 0, 0), new PointF3D(0, 0, 0) }
            };
        Surface surface = new Surface(DefaultControlPoints, 0.2);
        PointF3D DefaultSurfaceCentre = new PointF3D(1.5f, 0, 1.5f);
        PointF3D surfaceCentre;


        public Form1()
        {
            InitializeComponent();
            Normalize.WinHeight = isometrixBox.ClientSize.Height;
            Normalize.WinWidth = isometrixBox.ClientSize.Width;

            //Base
            PointF3D base1 = new PointF3D(2, 0, 0);
            PointF3D base2 = new PointF3D(0, 0, 5);
            PointF3D base3 = new PointF3D(15f, 0, 8f);
            //Height
            PointF3D height = new PointF3D(2, 10, 0);

            DefaultPyramid = new Polygon3D[]
            {
                //base
                new Polygon3D(new PointF3D[]{base1,base2,base3}),
                //connections lines to the height
                new Polygon3D(new PointF3D[]{base1,height}),
                new Polygon3D(new PointF3D[]{base2,height}),
                new Polygon3D(new PointF3D[]{base3,height}),
                //height
                new Polygon3D(new PointF3D[]{height,height}),
            };
            pyramid = (Polygon3D[])DefaultPyramid.Clone();

            DefaultObjCentre = new PointF3D(2, 0, 2);
            objCentre = DefaultObjCentre;
            surfaceCentre = DefaultSurfaceCentre;


            DefaultTransformVals();
            PointF[] IzometricAxes = ProjectionAffinity.projectionIsometric(coordinateLines);
            PointF[] XYAxes = ProjectionAffinity.projectionXY(coordinateLines);
            PointF[] XZAxes = ProjectionAffinity.projectionXZ(coordinateLines);
            PointF[] YZAxes = ProjectionAffinity.projectionYZ(coordinateLines);
            normalizedIzometricAxes = new PointF[IzometricAxes.Length];
            normalizedXYAxes = new PointF[XYAxes.Length];
            normalizedXZAxes = new PointF[XZAxes.Length];
            normalizedYZAxes = new PointF[YZAxes.Length];

            for (int i = 0; i < coordinateLines.Length; i++)
            {
                normalizedIzometricAxes[i].X = Normalize.NormalizeX(IzometricAxes[i].X);
                normalizedIzometricAxes[i].Y = Normalize.NormalizeY(IzometricAxes[i].Y);
                normalizedXYAxes[i].X = Normalize.NormalizeX(XYAxes[i].X);
                normalizedXYAxes[i].Y = Normalize.NormalizeY(XYAxes[i].Y);
                normalizedXZAxes[i].X = Normalize.NormalizeX(XZAxes[i].X);
                normalizedXZAxes[i].Y = Normalize.NormalizeY(XZAxes[i].Y);
                normalizedYZAxes[i].X = Normalize.NormalizeX(YZAxes[i].X);
                normalizedYZAxes[i].Y = Normalize.NormalizeY(YZAxes[i].Y);
            }
        }

        private void xyBox_Paint(object sender, PaintEventArgs e)
        {

            DrawXY(e.Graphics);
            if (PyramidCheck.Checked)
                DrawPyramid(e.Graphics, 1);
            else
                DrawSurface(e.Graphics, 1);
        }

        private void xzBox_Paint(object sender, PaintEventArgs e)
        {
            DrawXZ(e.Graphics);
            if (PyramidCheck.Checked)
                DrawPyramid(e.Graphics, 2);
            else
                DrawSurface(e.Graphics, 3);
        }

        private void Scale_ValueChanged(object sender, EventArgs e)
        {
            if (OldScalVal != (float)ScaleBox.Value)
            {
                ScalVal = (float)ScaleBox.Value;
                isometrixBox.Refresh();
                xyBox.Refresh();
                xzBox.Refresh();
                yzBox.Refresh();
            }
        }

        private void yzBox_Paint(object sender, PaintEventArgs e)
        {
            DrawYZ(e.Graphics);
            if (PyramidCheck.Checked)
                DrawPyramid(e.Graphics, 3);
            else
                DrawSurface(e.Graphics, 2);
        }


        private void isometrixBox_Paint(object sender, PaintEventArgs e)
        {
            TransformObject();
            DrawIsometric(e.Graphics);
            if (PyramidCheck.Checked)
                DrawPyramid(e.Graphics, 0);
            else
                DrawSurface(e.Graphics, 0);
        }
        private void Surface_CheckedChanged(object sender, EventArgs e)
        {
            if (SurfaceCheck.Checked)
            {
                DefaultTransformVals();
                surfaceGB.Enabled = true;
            }
            isometrixBox.Refresh();
            xyBox.Refresh();
            xzBox.Refresh();
            yzBox.Refresh();
        }

        private void PyramidCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (PyramidCheck.Checked)
            {
                if (!wholeSurfaceButton.Checked) wholeSurfaceButton.Checked = true;
                surfaceGB.Enabled = false;
                DefaultTransformVals();
            }
            isometrixBox.Refresh();
            xyBox.Refresh();
            xzBox.Refresh();
            yzBox.Refresh();
        }

        private void wholeSurfaceButton_CheckedChanged(object sender, EventArgs e)
        {
            if(wholeSurfaceButton.Checked)
            {
                rotateX.Enabled = rotateY.Enabled = rotateZ.Enabled = ScaleBox.Enabled = false;
                transX.Value = (decimal)TransVal.X;
                transY.Value = (decimal)TransVal.Y;
                transZ.Value = (decimal)TransVal.Z;
                transX.Maximum = transY.Maximum = transZ.Maximum = 20;
                transX.Minimum = transY.Minimum = transZ.Minimum = -20;
            }
            else
            {
                rotateX.Enabled = rotateY.Enabled = rotateZ.Enabled = ScaleBox.Enabled = false;

                transX.Maximum = transY.Maximum = transZ.Maximum = 1000;
                transX.Minimum = transY.Minimum = transZ.Minimum = -1000;
            }
        }
        Point chgSurfaceIndex;
        private void points_CheckedChanged(object sender, EventArgs e)
        {
            chgSurfaceIndex.X = int.Parse(((RadioButton)(sender)).Name[1].ToString())-1;
            chgSurfaceIndex.Y = int.Parse(((RadioButton)(sender)).Name[2].ToString())-1;
            transX.Value = (decimal)surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].X;
            transY.Value = (decimal)surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Y;
            transZ.Value = (decimal)surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Z;
        }

        private void transX_ValueChanged(object sender, EventArgs e)
        {
            if (wholeSurfaceButton.Checked)
            {
                if ((decimal)TransVal.X != transX.Value)
                {
                    TransVal.X = (float)transX.Value;
                    isometrixBox.Refresh();
                    xyBox.Refresh();
                    xzBox.Refresh();
                    yzBox.Refresh();
                }
            }
            else
            {
                if ((float)transX.Value != surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].X)
                {
                    surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].X += (float)transX.Value - surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].X;
                    surface.CountDrawPoints();
                    isometrixBox.Refresh();
                    xyBox.Refresh();
                    xzBox.Refresh();
                    yzBox.Refresh();
                }

            }
            
        }

        private void transY_ValueChanged(object sender, EventArgs e)
        {
            if (wholeSurfaceButton.Checked)
            {
                if ((decimal)TransVal.Y != transY.Value)
                {
                    TransVal.Y = (float)transY.Value;
                    isometrixBox.Refresh();
                    xyBox.Refresh();
                    xzBox.Refresh();
                    yzBox.Refresh();
                }
            }
            else
            {
                if ((float)transY.Value != surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Y)
                {
                    surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Y += (float)transY.Value - surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Y;
                    surface.CountDrawPoints();
                    isometrixBox.Refresh();
                    xyBox.Refresh();
                    xzBox.Refresh();
                    yzBox.Refresh();
                }

            }
        }

        private void transZ_ValueChanged(object sender, EventArgs e)
        {
            if (wholeSurfaceButton.Checked)
            {
                if ((decimal)TransVal.Z != transZ.Value)
                {
                    TransVal.Z = (float)transZ.Value;
                    isometrixBox.Refresh();
                    xyBox.Refresh();
                    xzBox.Refresh();
                    yzBox.Refresh();
                }
            }
            else
            {
                if ((float)transZ.Value != surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Z)
                {
                    surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Z += (float)transZ.Value - surface.ControlPoints[chgSurfaceIndex.X, chgSurfaceIndex.Y].Z;
                    surface.CountDrawPoints();
                    isometrixBox.Refresh();
                    xyBox.Refresh();
                    xzBox.Refresh();
                    yzBox.Refresh();
                }

            }
        }

        private void TSbox_ValueChanged(object sender, EventArgs e)
        {
            surface.DrawIncrement = (double)TSbox.Value;
            isometrixBox.Refresh();
            xyBox.Refresh();
            xzBox.Refresh();
            yzBox.Refresh();
        }

        void DrawIsometric(Graphics e)
        {
            e.Clear(Color.White);
            Pen gray = new Pen(Color.DarkGray);
            e.DrawLine(gray, normalizedIzometricAxes[0], normalizedIzometricAxes[1]);
            e.DrawLine(gray, normalizedIzometricAxes[0], normalizedIzometricAxes[2]);
            e.DrawLine(gray, normalizedIzometricAxes[0], normalizedIzometricAxes[3]);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.DrawString("X", Font, brush, normalizedIzometricAxes[1]);
            e.DrawString("Y", Font, brush, normalizedIzometricAxes[2]);
            e.DrawString("Z", Font, brush, normalizedIzometricAxes[3]);

        }
        void DrawXY(Graphics e)
        {

            Pen gray = new Pen(Color.DarkGray);
            e.Clear(Color.White);
            e.DrawLine(gray, normalizedXYAxes[0], normalizedXYAxes[1]);
            e.DrawLine(gray, normalizedXYAxes[0], normalizedXYAxes[2]);
            e.DrawLine(gray, normalizedXYAxes[0], normalizedXYAxes[3]);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.DrawString("X", Font, brush, normalizedXYAxes[1]);
            e.DrawString("Y", Font, brush, normalizedXYAxes[2]);
            e.DrawString("Z", Font, brush, normalizedXYAxes[3]);
        }
        void DrawXZ(Graphics e)
        {

            Pen gray = new Pen(Color.DarkGray);
            e.Clear(Color.White);
            e.DrawLine(gray, normalizedXZAxes[0], normalizedXZAxes[1]);
            e.DrawLine(gray, normalizedXZAxes[0], normalizedXZAxes[2]);
            e.DrawLine(gray, normalizedXZAxes[0], normalizedXZAxes[3]);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.DrawString("X", Font, brush, normalizedXZAxes[1]);
            e.DrawString("Y", Font, brush, normalizedXZAxes[2]);
            e.DrawString("Z", Font, brush, normalizedXZAxes[3]);
        }

        void DrawYZ(Graphics e)
        {

            Pen gray = new Pen(Color.DarkGray);
            e.Clear(Color.White);
            e.DrawLine(gray, normalizedYZAxes[0], normalizedYZAxes[1]);
            e.DrawLine(gray, normalizedYZAxes[0], normalizedYZAxes[2]);
            e.DrawLine(gray, normalizedYZAxes[0], normalizedYZAxes[3]);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.DrawString("X", Font, brush, normalizedYZAxes[1]);
            e.DrawString("Y", Font, brush, normalizedYZAxes[2]);
            e.DrawString("Z", Font, brush, normalizedYZAxes[3]);
        }

        void DrawPyramid(Graphics e, int drawTo)
        {
            Polygon2D[] obj = new Polygon2D[pyramid.Length];
            switch (drawTo)
            {
                case 0:
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        obj[i].polygon = pyramid[i].ProjectIsometric();
                        obj[i].polygon = obj[i].GetNormalizedPolygon();
                    }
                    break;
                case 1:
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        obj[i].polygon = pyramid[i].ProjectXY();
                        obj[i].polygon = obj[i].GetNormalizedPolygon();
                    }
                    break;
                case 2:
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        obj[i].polygon = pyramid[i].ProjectZX();
                        obj[i].polygon = obj[i].GetNormalizedPolygon();
                    }
                    break;
                case 3:
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        obj[i].polygon = pyramid[i].ProjectYZ();
                        obj[i].polygon = obj[i].GetNormalizedPolygon();
                    }
                    break;
            }
            Pen pen = new Pen(Color.Blue);
            foreach (Polygon2D p in obj)
            {
                e.DrawPolygon(pen, p.polygon);
            }
        }

        void TransformObject()
        {
            if (oldTransVal != TransVal)
            {
                if (PyramidCheck.Checked)
                {
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        pyramid[i].polygons = pyramid[i].Translate(new PointF3D(TransVal.X - oldTransVal.X, TransVal.Y - oldTransVal.Y, TransVal.Z - oldTransVal.Z));
                    }
                    objCentre.X += TransVal.X - oldTransVal.X;
                    objCentre.Y += TransVal.Y - oldTransVal.Y;
                    objCentre.Z += TransVal.Z - oldTransVal.Z;
                }
                else
                {
                    surface.ControlPoints = surface.Translate(new PointF3D(TransVal.X - oldTransVal.X, TransVal.Y - oldTransVal.Y, TransVal.Z - oldTransVal.Z));
                    surfaceCentre.X += TransVal.X - oldTransVal.X;
                    surfaceCentre.Y += TransVal.Y - oldTransVal.Y;
                    surfaceCentre.Z += TransVal.Z - oldTransVal.Z;
                }
                oldTransVal = TransVal;
            }

            if (OldXRotValue != XRotValue)
            {
                if (PyramidCheck.Checked)
                {
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        pyramid[i].polygons = pyramid[i].RotateX(XRotValue - OldXRotValue, objCentre);
                    }
                }
                else
                {
                    surface.ControlPoints = surface.RotateX(XRotValue - OldXRotValue, surfaceCentre);
                }
                OldXRotValue = XRotValue;
            }
            if (OldYRotValue != YRotValue)
            {
                if (PyramidCheck.Checked)
                {
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        pyramid[i].polygons = pyramid[i].RotateY(YRotValue - OldYRotValue, objCentre);
                    }
                }
                else
                {
                    surface.ControlPoints = surface.RotateY(YRotValue - OldYRotValue, surfaceCentre);
                }
                OldYRotValue = YRotValue;
            }
            if (OldZRotValue != ZRotValue)
            {
                if (PyramidCheck.Checked)
                {
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        pyramid[i].polygons = pyramid[i].RotateZ(ZRotValue - OldZRotValue, objCentre);
                    }
                }
                else
                {
                    surface.ControlPoints = surface.RotateZ(ZRotValue - OldZRotValue, surfaceCentre);
                }
                OldZRotValue = ZRotValue;
            }

            if (OldScalVal != ScalVal)
            {
                float curscalval = ScalVal / OldScalVal;
                if (PyramidCheck.Checked)
                {
                    for (int i = 0; i < pyramid.Length; i++)
                    {
                        pyramid[i].polygons = pyramid[i].Scale(new PointF3D(curscalval, curscalval, curscalval), objCentre);
                    }
                }
                else
                {
                    surface.ControlPoints = surface.Scale(new PointF3D(curscalval, curscalval, curscalval), surfaceCentre);
                }
                OldScalVal = ScalVal;
            }
        }

        private void Rotate_ValueChanged(object sender, EventArgs e)
        {
            if (((NumericUpDown)(sender)).Value == 361) ((NumericUpDown)(sender)).Value = 0;
            if (((NumericUpDown)(sender)).Value == -1) ((NumericUpDown)(sender)).Value = 360;
            XRotValue = (float)rotateX.Value;
            YRotValue = (float)rotateY.Value;
            ZRotValue = (float)rotateZ.Value;
            isometrixBox.Refresh();
            xyBox.Refresh();
            xzBox.Refresh();
            yzBox.Refresh();
        }

        void DrawSurface(Graphics e, int drawTo)
        {
            int count = surface.DrawPoints.GetLength(0);
            PointF[] surfacePoints = new PointF[count * count];
            switch (drawTo)
            {
                case 0:
                    surfacePoints = surface.ProjectDimetric();
                    for (int i = 0; i < surfacePoints.Length; i++)
                    {
                        surfacePoints[i].X = Normalize.NormalizeX(surfacePoints[i].X);
                        surfacePoints[i].Y = Normalize.NormalizeY(surfacePoints[i].Y);
                    }
                    break;
                case 1:
                    surfacePoints = surface.ProjectXY();
                    for (int i = 0; i < surfacePoints.Length; i++)
                    {
                        surfacePoints[i].X = Normalize.NormalizeX(surfacePoints[i].X);
                        surfacePoints[i].Y = Normalize.NormalizeY(surfacePoints[i].Y);
                    }
                    break;
                case 2:
                    surfacePoints = surface.ProjectYZ();
                    for (int i = 0; i < surfacePoints.Length; i++)
                    {
                        surfacePoints[i].X = Normalize.NormalizeX(surfacePoints[i].X);
                        surfacePoints[i].Y = Normalize.NormalizeY(surfacePoints[i].Y);
                    }
                    break;
                case 3:
                    surfacePoints = surface.ProjectXZ();
                    for (int i = 0; i < surfacePoints.Length; i++)
                    {
                        surfacePoints[i].X = Normalize.NormalizeX(surfacePoints[i].X);
                        surfacePoints[i].Y = Normalize.NormalizeY(surfacePoints[i].Y);
                    }
                    break;
            }

            PointF[,] IsometricSurface = new PointF[count, count];
            for (int r = 0; r < count; r++)
            {
                for (int c = 0; c < count; c++)
                {
                    IsometricSurface[r, c] = surfacePoints[r * count + c];
                }
            }

            Pen pen = new Pen(Color.Red);
            for (int r = 0; r < count - 1; r++)
                for (int c = 0; c < count - 1; c++)
                {
                    e.DrawLine(pen, IsometricSurface[r, c], IsometricSurface[r, c + 1]);
                    e.DrawLine(pen, IsometricSurface[r, c], IsometricSurface[r + 1, c]);
                }
            for (int i = 0; i < count - 1; i++)
            {
                e.DrawLine(pen, IsometricSurface[i, count - 1], IsometricSurface[i + 1, count - 1]);
                e.DrawLine(pen, IsometricSurface[count - 1, i], IsometricSurface[count - 1, i + 1]);
            }

        }

        private void DefaultTransformVals()
        {
            objCentre = DefaultObjCentre;
            surfaceCentre = DefaultSurfaceCentre;
            OldScalVal = ScalVal = 1;
            ScaleBox.Value = 1;
            if (PyramidCheck.Checked)
            {
                oldTransVal.X = TransVal.X = objCentre.X;
                oldTransVal.Y = TransVal.Y = objCentre.Y;
                oldTransVal.Z = TransVal.Z = objCentre.Z;
            }
            else
            {
                oldTransVal.X = TransVal.X = surfaceCentre.X;
                oldTransVal.Y = TransVal.Y = surfaceCentre.Y;
                oldTransVal.Z = TransVal.Z = surfaceCentre.Z;
            }
            transX.Value = (decimal)oldTransVal.X;
            transY.Value = (decimal)oldTransVal.Y;
            transZ.Value = (decimal)oldTransVal.Z;

            XRotValue = YRotValue = ZRotValue = OldXRotValue = OldYRotValue = OldZRotValue = 0;
            rotateX.Value = rotateY.Value = rotateZ.Value = 0;
            pyramid = (Polygon3D[])DefaultPyramid.Clone();
            surface.ControlPoints = (PointF3D[,])DefaultControlPoints.Clone();
        }
    }
}

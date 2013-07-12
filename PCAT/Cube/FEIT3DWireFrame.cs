using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DTools;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Controls;

namespace FiveElementsIntTest.Cube
{
    class FEIT3DWireFrame
    {
        public List<ScreenSpaceLines3D> mLines;

        public FEIT3DWireFrame()
        {
            mLines = new List<ScreenSpaceLines3D>();

            for (int i = 0; i < 15; i++)
            {
                ScreenSpaceLines3D Line = new ScreenSpaceLines3D();
                Line.Thickness = 2;
                Line.Color = Color.FromRgb(255, 255, 255);
                mLines.Add(Line);
            }

            mLines[0].Points.Add(new Point3D(-2, 2, -2));
            mLines[0].Points.Add(new Point3D(2, 2, -2));

            mLines[1].Points.Add(new Point3D(2, 2, -2));
            mLines[1].Points.Add(new Point3D(2, 2, 2));

            mLines[2].Points.Add(new Point3D(2, 2, 2));
            mLines[2].Points.Add(new Point3D(-2, 2, 2));

            mLines[3].Points.Add(new Point3D(-2, 2, 2));
            mLines[3].Points.Add(new Point3D(-2, 2, -2));

            mLines[4].Points.Add(new Point3D(-2, 2, 0));
            mLines[4].Points.Add(new Point3D(2, 2, 0));

            mLines[5].Points.Add(new Point3D(0, 2, -2));
            mLines[5].Points.Add(new Point3D(0, 2, 2));

            mLines[6].Points.Add(new Point3D(-2, 2, 2));
            mLines[6].Points.Add(new Point3D(-2, -2, 2));

            mLines[7].Points.Add(new Point3D(2, 2, 2));
            mLines[7].Points.Add(new Point3D(2, -2, 2));

            mLines[8].Points.Add(new Point3D(0, 2, 2));
            mLines[8].Points.Add(new Point3D(0, -2, 2));

            mLines[9].Points.Add(new Point3D(-2, 0, 2));
            mLines[9].Points.Add(new Point3D(2, 0, 2));

            mLines[10].Points.Add(new Point3D(-2, -2, 2));
            mLines[10].Points.Add(new Point3D(2, -2, 2));

            mLines[11].Points.Add(new Point3D(2, -2, 2));
            mLines[11].Points.Add(new Point3D(2, -2, -2));

            mLines[12].Points.Add(new Point3D(2, 2, 0));
            mLines[12].Points.Add(new Point3D(2, -2, 0));

            mLines[13].Points.Add(new Point3D(2, 0, 2));
            mLines[13].Points.Add(new Point3D(2, 0, -2));

            mLines[14].Points.Add(new Point3D(2, 2, -2));
            mLines[14].Points.Add(new Point3D(2, -2, -2));

            //mLines[0].Points.Add(new Point3D(0, 0, 0));
            //mLines[0].Points.Add(new Point3D(0, 4, 0));

            //mLines[2].Points.Add(new Point3D(-2, 2.2, -2));
            //mLines[2].Points.Add(new Point3D(2, 2.2, -2));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Windows;
using _3DTools;

namespace FiveElementsIntTest.Cube
{
    public class FEIT3DCubeBox
    {
        public List<GeometryModel3D> mModels;
        public List<MeshGeometry3D> mMeshes;
        private static int[] mV = { 1, -1, 1, 1, 1, -1, 1, 1, 1, 1, -1, 1, 1, -1, -1, 1, 1, -1, 
                            -1, -1, -1, -1, 1, 1, -1, 1, -1, -1, -1, -1, -1, -1, 1, -1, 1, 1, 
                             1, -1, -1, -1, 1, -1, 1, 1, -1, 1, -1, -1, -1, -1, -1, -1, 1, -1, 
                            -1, -1, 1, 1, 1, 1, -1, 1, 1, -1, -1, 1, 1, -1, 1, 1, 1, 1, 
                            -1, 1, 1, 1, 1, -1, -1, 1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, 
                            -1, -1, -1, 1, -1, 1, -1, -1, 1, -1, -1, -1, 1, -1, -1, 1, -1, 1 };

        public FEIT3DCubeBox()
        {
            mModels = new List<GeometryModel3D>();
            mMeshes = new List<MeshGeometry3D>();

            for (int i = 0; i < 6; i++)
            {
                mModels.Add(new GeometryModel3D());
                mMeshes.Add(new MeshGeometry3D());
                mModels[i].Geometry = mMeshes[i];
                GenSurface(i);
                mModels[i].Material = new DiffuseMaterial(Brushes.OrangeRed);
            }
        }

        public void SetTexture(int dstSur, System.Drawing.Bitmap graph)
        {
            ImageBrush ib = new ImageBrush(
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                graph.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(graph.Width, graph.Height)));

            mModels[dstSur].Material = new DiffuseMaterial(ib);

            PointCollection pc = new PointCollection();

            //texture coordinates
            switch (dstSur)
            {
                case 0:
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 0));
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 1));
                    pc.Add(new Point(1, 0));
                    break;
                case 1:
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 0));
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 1));
                    pc.Add(new Point(1, 0));
                    break;
                case 2:
                    pc.Add(new Point(1, 1));
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(1, 0));
                    pc.Add(new Point(1, 1));
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(0, 0));
                    break;
                case 3:
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 0));
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 1));
                    pc.Add(new Point(1, 0));
                    break;
                case 4:
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(1, 1));
                    pc.Add(new Point(1, 0));
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 1));
                    break;
                case 5:
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(1, 1));
                    pc.Add(new Point(1, 0));
                    pc.Add(new Point(0, 0));
                    pc.Add(new Point(0, 1));
                    pc.Add(new Point(1, 1));
                    break;
            }

            mMeshes[dstSur].TextureCoordinates = pc;
        }

        public void GenSurface(int i)
        {

            MeshGeometry3D mesh = mMeshes[i];
            int baseOff = i * 18;
            int vEachOff = 3;

            for (int j = 0; j < 6; j++)
            {
                mesh.Positions.Add(
                    new Point3D(mV[baseOff + j * vEachOff], 
                    mV[baseOff + j * vEachOff + 1], 
                    mV[baseOff + j * vEachOff + 2]));
            }
        }
    }
}

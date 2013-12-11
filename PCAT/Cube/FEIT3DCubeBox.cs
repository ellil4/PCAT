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
using System.Runtime.InteropServices;
namespace FiveElementsIntTest.Cube
{
    public class FEIT3DCubeBox
    {
        public List<GeometryModel3D> mModels;//魔方动作控制
        public List<MeshGeometry3D> mMeshes;//用于生成3-D形状的三角形基元
        private static int[] mV = { 1, -1, 1, 1, 1, -1, 1, 1, 1, 1, -1, 1, 1, -1, -1, 1, 1, -1, 
                            -1, -1, -1, -1, 1, 1, -1, 1, -1, -1, -1, -1, -1, -1, 1, -1, 1, 1, 
                             1, -1, -1, -1, 1, -1, 1, 1, -1, 1, -1, -1, -1, -1, -1, -1, 1, -1, 
                            -1, -1, 1, 1, 1, 1, -1, 1, 1, -1, -1, 1, 1, -1, 1, 1, 1, 1, 
                            -1, 1, 1, 1, 1, -1, -1, 1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, 
                            -1, -1, -1, 1, -1, 1, -1, -1, 1, -1, -1, -1, 1, -1, -1, 1, -1, 1 };

        IntPtr[] mIptrs;

        public FEIT3DCubeBox()
        {
            mModels = new List<GeometryModel3D>();
            mMeshes = new List<MeshGeometry3D>();
            mIptrs = new IntPtr[6];
            for (int i = 0; i < 6; i++)
            {
                mIptrs[i] = IntPtr.Zero;
                mModels.Add(new GeometryModel3D());
                mMeshes.Add(new MeshGeometry3D());
                mModels[i].Geometry = mMeshes[i];//几何3d形状
                GenSurface(i);
                mModels[i].Material = new DiffuseMaterial(Brushes.OrangeRed);
                // 材料              //2D-3D转换                       //绘制图形对象的对象
            }
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
        
        public void SetTexture( int dstSur, System.Drawing.Bitmap graph)
        {           //质地    

            if (mIptrs[dstSur] != IntPtr.Zero)
            {
                DeleteObject(mIptrs[dstSur]);
                mIptrs[dstSur] = IntPtr.Zero;
            }

            mIptrs[dstSur] = graph.GetHbitmap();

            ImageBrush ib = new ImageBrush(
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                mIptrs[dstSur], IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(graph.Width, graph.Height)));

            mModels[dstSur].Material = new DiffuseMaterial(ib);

            PointCollection pc = new PointCollection();

            //texture coordinates
            switch (dstSur)
            {
                case 0:
                    pc.Add(new Point(0, 1));//坐标
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
                    pc.Add(new  Point(1, 1));
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

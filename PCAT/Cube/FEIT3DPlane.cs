using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace FiveElementsIntTest.Cube
{
    public class FEIT3DPlane
    {
        public GeometryModel3D mModel;

        public FEIT3DPlane(int size, int pos)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mModel = new GeometryModel3D();

            int eachOff = 3;
            int[] pvs = GenPoints(size / 2, pos);

            for (int i = 0; i < 6; i++)
            {
                mesh.Positions.Add(new Point3D(
                    pvs[i * eachOff], pvs[i * eachOff + 1], pvs[i * eachOff + 2]));
            }

            mModel.Geometry = mesh;
            mModel.Material = new DiffuseMaterial(Brushes.Black);
        }

        private int[] GenPoints(int halfSize, int pos)
        {
            //-1, 1, 1, 1, 1, -1, -1, 1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, 
            int[] retval = { -1 * halfSize, pos, 1 * halfSize, 
                               1 * halfSize, pos, -1 * halfSize, 
                               -1 * halfSize, pos, -1 * halfSize, 
                               -1 * halfSize, pos, 1 * halfSize, 
                               1 * halfSize, pos, 1 * halfSize, 
                               1 * halfSize, pos, -1 * halfSize};

            return retval;
        }

    }
}

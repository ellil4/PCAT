using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace FiveElementsIntTest.Cube
{
    public class FEITBoxesGear
    {
        public Dictionary<int, FEIT3DCubeBox> Boxes;
        public Dictionary<int, Transform3DGroup> Transforms;

        public FEITBoxesGear()
        {
            Boxes = new Dictionary<int, FEIT3DCubeBox>();
            Transforms = new Dictionary<int, Transform3DGroup>();
        }

        public void AddBox(int index, ref FEIT3DCubeBox box)
        {
            Boxes.Add(index, box);
            Transforms.Add(index, new Transform3DGroup());
        }

        public void AddTransform(int index, Transform3D trans)
        {
            Transforms[index].Children.Add(trans);
        }

        public Transform3DGroup GetTransform(int index)
        {
            return Transforms[index];
        }
    }
}

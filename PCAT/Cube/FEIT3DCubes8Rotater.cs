using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FiveElementsIntTest.Cube
{
    public class FEIT3DCubes8Rotater //暂时不看

    {
        //public Dictionary<int, FEIT3DCubeBox> mBoxes;
        public FEITBoxesGear mBoxesGear;

        //keys are in anticlockwise order
        private void switchElements<TYPENAME>(int[] keys, bool anticlockwise, 
            ref Dictionary<int, TYPENAME> dict)
        {
            List<TYPENAME> elem4 = new List<TYPENAME>();
            
            for (int i = 0; i < 4; i++)
            {
                elem4.Add(dict[keys[i]]);
                dict.Remove(keys[i]);
            }

            if (anticlockwise)
            {
                dict.Add(keys[1], elem4[0]);
                dict.Add(keys[2], elem4[1]);
                dict.Add(keys[3], elem4[2]);
                dict.Add(keys[0], elem4[3]);
            }
            else
            {
                dict.Add(keys[3], elem4[0]);
                dict.Add(keys[0], elem4[1]);
                dict.Add(keys[1], elem4[2]);
                dict.Add(keys[2], elem4[3]);
            }
        }

        private void switchBoxesGear(int[] keys, bool anticlockwise)
        {
            switchElements<FEIT3DCubeBox>(keys, anticlockwise, ref mBoxesGear.Boxes);
            switchElements<Transform3DGroup>(keys, anticlockwise, ref mBoxesGear.Transforms);
        }

        public FEIT3DCubes8Rotater(ref FEITBoxesGear boxGear)
        {
            mBoxesGear = boxGear;
        }

        public void swicthLeft(bool anticlockwise)
        {
            switchBoxesGear(new int[] { 0, 1, 5, 4 }, anticlockwise);
        }

        public void switchRight(bool anticlockwise)
        {
            switchBoxesGear(new int[] { 3, 2, 6, 7 }, anticlockwise);
        }

        public void switchTop(bool anticlockwise)
        {
            switchBoxesGear(new int[] { 0, 3, 2, 1 }, anticlockwise);
        }

        public void switchBottom(bool anticlockwise)
        {
            switchBoxesGear(new int[] { 4, 7, 6, 5 }, anticlockwise);
        }

        public void switchFront(bool anticlockwise)
        {
            switchBoxesGear(new int[] { 4, 7, 3, 0 }, anticlockwise);
        }

        public void switchBack(bool anticlockwise)
        {
            switchBoxesGear(new int[] { 1, 5, 6, 2 }, anticlockwise);
        }

        public void rotateLeft(double Angle)
        {
            Vector3D vec = new Vector3D(0, 0, -1);
            rotateBox(0, vec, Angle);
            rotateBox(1, vec, Angle);
            rotateBox(4, vec, Angle);
            rotateBox(5, vec, Angle);
        }

        public void rotateRight(double Angle)
        {
            Vector3D vec = new Vector3D(0, 0, 1);
            rotateBox(2, vec, Angle);
            rotateBox(3, vec, Angle);
            rotateBox(6, vec, Angle);
            rotateBox(7, vec, Angle);
        }

        public void rotateTop(double Angle)
        {
            Vector3D vec = new Vector3D(0, -1, 0);
            rotateBox(0, vec, Angle);
            rotateBox(1, vec, Angle);
            rotateBox(2, vec, Angle);
            rotateBox(3, vec, Angle);
        }

        public void rotateBottom(double Angle)
        {
            Vector3D vec = new Vector3D(0, 1, 0);
            rotateBox(4, vec, Angle);
            rotateBox(5, vec, Angle);
            rotateBox(6, vec, Angle);
            rotateBox(7, vec, Angle);
        }

        public void rotateFront(double Angle)
        {
            Vector3D vec = new Vector3D(-1, 0, 0);
            rotateBox(0, vec, Angle);
            rotateBox(3, vec, Angle);
            rotateBox(4, vec, Angle);
            rotateBox(7, vec, Angle);
        }

        public void rotateBack(double Angle)
        {
            Vector3D vec = new Vector3D(1, 0, 0);
            rotateBox(1, vec, Angle);
            rotateBox(2, vec, Angle);
            rotateBox(5, vec, Angle);
            rotateBox(6, vec, Angle);
        }

        public void rotateBox(int index, Vector3D vec, double angle)
        {
            AxisAngleRotation3D rotation = new AxisAngleRotation3D();
            rotation.Axis = vec;
            rotation.Angle = angle;
            RotateTransform3D trans = new RotateTransform3D();
            trans.Rotation = rotation;

            mBoxesGear.AddTransform(index, trans);
            

            for (int i = 0; i < 6; i++)
            {
                mBoxesGear.Boxes[index].mModels[i].Transform = mBoxesGear.GetTransform(index);
            }
        }
    }
}

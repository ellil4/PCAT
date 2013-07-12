using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;

namespace FiveElementsIntTest.Cube
{
    public enum LIGHT_MODE
    {
        ALL_FACES, TOP_PRIORITY
    }

    public class FEIT3DCubes8Sight
    {
        public Model3DGroup mGroup;
        public Viewport3D mVp3d;

        //for transforms
        public FEITBoxesGear mBoxesGear;

        

        //constructor
        public FEIT3DCubes8Sight(double width, double height, LIGHT_MODE mode, bool bWire)
        {
            mGroup = new Model3DGroup();
            if (mode == LIGHT_MODE.TOP_PRIORITY)
            {

                mGroup.Children.Add(genLight(LIGHT_POS.TOP));//light
                mGroup.Children.Add(genLight(LIGHT_POS.PERSPECTIVE));
            }
            else if (mode == LIGHT_MODE.ALL_FACES)
            {
                mGroup.Children.Add(genLight(LIGHT_POS.RIGHT));//light
                mGroup.Children.Add(genLight(LIGHT_POS.FRONT));//light
                mGroup.Children.Add(genLight(LIGHT_POS.TOP));
            }

            mVp3d = new Viewport3D();
            mVp3d.Height = height;
            mVp3d.Width = width;

            mVp3d.Camera = genPersCamera();
            mVp3d.Children.Add(genScene(ref mGroup));//scene
            mBoxesGear = new FEITBoxesGear();

            if (bWire)
                GenWireFrame();
            
        }

        public void GenWireFrame()
        {
            FEIT3DWireFrame wire = new FEIT3DWireFrame();
            for (int i = 0; i < wire.mLines.Count; i++)
            {
                mVp3d.Children.Add(wire.mLines[i]);
            }
        }


        public void AddBox(ref FEIT3DCubeBox box, int index)
        {
            //six surfaces
            //eight boxes
            mBoxesGear.AddBox(index, ref box);

            for (int i = 0; i < 6; i++)
            {
                mGroup.Children.Add(box.mModels[i]);
            }

            placeBox(ref box, index);
        }

        public void AddPlane(ref FEIT3DPlane plane)
        {
            mGroup.Children.Add(plane.mModel);
        }

        private void placeBox(ref FEIT3DCubeBox box, int index)
        {
            switch (index)
            {
                case 0:
                    moveBox(index, ref box, new Vector3D(1, 1, 1));
                    break;
                case 1:
                    moveBox(index, ref box, new Vector3D(-1, 1, 1));
                    break;
                case 2:
                    moveBox(index, ref box, new Vector3D(-1, 1, -1));
                    break;
                case 3:
                    moveBox(index, ref box, new Vector3D(1, 1, -1));
                    break;
                case 4:
                    moveBox(index, ref box, new Vector3D(1, -1, 1));
                    break;
                case 5:
                    moveBox(index, ref box, new Vector3D(-1, -1, 1));
                    break;
                case 6:
                    moveBox(index, ref box, new Vector3D(-1, -1, -1));
                    break;
                case 7:
                    moveBox(index, ref box, new Vector3D(1, -1, -1));
                    break;
            }
        }

        private void moveBox(int index, ref FEIT3DCubeBox box, Vector3D vec)
        {
            TranslateTransform3D trans = new TranslateTransform3D(vec);

            mBoxesGear.AddTransform(index, trans);

            for (int i = 0; i < 6; i++)
                box.mModels[i].Transform = mBoxesGear.GetTransform(index);
        }

        private static ModelVisual3D genScene(ref Model3DGroup group)
        {
            ModelVisual3D ret = new ModelVisual3D();
            ret.Content = group;
            return ret;
        }

        private enum LIGHT_POS
        {
            TOP, FRONT, RIGHT, PERSPECTIVE
        }

        private static PointLight genLight(LIGHT_POS pos)
        {
            PointLight light = new PointLight();

            switch (pos)
            {
                case LIGHT_POS.TOP:
                    light.Position = new Point3D(0, 10, 0);
                    break;
                case LIGHT_POS.FRONT:
                    light.Position = new Point3D(0, 0, 10);
                    break;
                case LIGHT_POS.RIGHT:
                    light.Position = new Point3D(10, 0, 0);
                    break;
                case LIGHT_POS.PERSPECTIVE:
                    light.Position = new Point3D(8, 10, 8);
                    break;
            }

            light.Color = Colors.White;
            light.Range = 150;
            light.LinearAttenuation = 0.01;
            light.QuadraticAttenuation = 0.01;
            light.ConstantAttenuation = 0.01;

            return light;
        }

        private static OrthographicCamera genOrthoCam()
        {
            OrthographicCamera cam = new OrthographicCamera();
            cam.Position = new Point3D(4, 4, 4);
            cam.LookDirection = new Vector3D(-1, -1, -1);
            cam.UpDirection = new Vector3D(0, 1, 0);
            cam.Width = 8;
            return cam;
        }

        private static PerspectiveCamera genPersCamera()
        {
            PerspectiveCamera cam = new PerspectiveCamera();
            double distance = 25;
            cam.Position = new Point3D(distance, distance, distance);
            cam.LookDirection = new Vector3D(-1, -1, -1);
            cam.UpDirection = new Vector3D(0, 1, 0);
            cam.FieldOfView = 10;
            //cam.
            return cam;
        }
    }
}

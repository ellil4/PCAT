using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FiveElementsIntTest.Cube;
using System.Windows.Interop;
using System.Runtime.InteropServices;
namespace FiveElementsIntTest
{
    /// <summary>
    /// CompCubeDisplay.xaml 的互動邏輯
    /// </summary>
    public partial class CompCubeDisplay : UserControl
    {
        public FEIT3DCubes8Sight mSight;
        public List<FEIT3DCubeBox> mCubeBoxes;
        public Image mOverlapImage;
        public FEIT3DCubes8Rotater mRotater;

        public void Show(bool enable)
        {
            if (enable)
                this.Visibility = System.Windows.Visibility.Visible;
            else
                this.Visibility = System.Windows.Visibility.Hidden;
        }

        public CompCubeDisplay(int size, LIGHT_MODE light_mode, bool bWire)
        {
            InitializeComponent();

            mSight = new FEIT3DCubes8Sight(size, size, light_mode, bWire);
            mCubeBoxes = new List<FEIT3DCubeBox>();
            mOverlapImage = new Image();
            mRotater = new FEIT3DCubes8Rotater(ref mSight.mBoxesGear);

            System.Drawing.Bitmap bmp = FiveElementsIntTest.Properties.Resources.BLANK_TEX;

            for (int i = 0; i < 8; i++)
            {
                FEIT3DCubeBox b = new FEIT3DCubeBox();
                mSight.AddBox(ref b, i);
                mCubeBoxes.Add(b);

                for (int j = 0; j < 6; j++)
                {
                    b.SetTexture(j, bmp);
                }
            }

            FEIT3DPlane plane = new FEIT3DPlane(32, -10);
            mSight.AddPlane(ref plane);

            Width = size;
            Height = size;

            amGrid.Width = size;
            amGrid.Height = size;

            mOverlapImage.Width = size;
            mOverlapImage.Height = size;

            amGrid.Children.Add(mSight.mVp3d);
            amGrid.Children.Add(mOverlapImage);
        }

        public void SetTexture(int cubeFaceIndex,
            int cubeIndex, System.Drawing.Bitmap texture)
        {
            mCubeBoxes[cubeIndex].SetTexture(cubeFaceIndex, texture);
        }

        public void SetTextureWithCode(String code, System.Drawing.Bitmap texture)
        {
            switch (code)
            {
                case "A":
                    SetTexture(4, 0, texture);
                    break;
                case "B":
                    SetTexture(4, 3, texture);
                    break;
                case "C":
                    SetTexture(4, 2, texture);
                    break;
                case "D":
                    SetTexture(4, 1, texture);
                    break;
                case "E":
                    SetTexture(3, 0, texture);
                    break;
                case "F":
                    SetTexture(3, 4, texture);
                    break;
                case "G":
                    SetTexture(3, 5, texture);
                    break;
                case "H":
                    SetTexture(3, 1, texture);
                    break;
                case "I":
                    SetTexture(0, 0, texture);
                    break;
                case "J":
                    SetTexture(0, 3, texture);
                    break;
                case "K":
                    SetTexture(0, 7, texture);
                    break;
                case "L":
                    SetTexture(0, 4, texture);
                    break;
            }
        }

        public String mLastTurnPhase = null;
        public bool mLastTurnWise = false;


        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
        IntPtr mArrowPtr = IntPtr.Zero;

        private void setSemiTurn(String partName, bool clockWise)
        {
            int rotAngle = 0;

            if (clockWise)
            {
                rotAngle = 30;
            }
            else
            {
                rotAngle = -30;
            }
            System.Drawing.Bitmap texture = null;

            if (partName.Equals("top"))
            {
                mRotater.rotateTop(rotAngle);
                if (clockWise)
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowUpClock;
                }
                else
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowUpAnti;
                }

            }
            else if (partName.Equals("bottom"))
            {
                mRotater.rotateBottom(rotAngle);
                if (clockWise)
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowDownClock;
                }
                else
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowDownAnti;
                }
            }
            else if (partName.Equals("left"))
            {
                mRotater.rotateLeft(rotAngle);
                if (clockWise)
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowLeftClock;
                }
                else
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowLeftAnti;
                }
            }
            else if (partName.Equals("right"))
            {
                mRotater.rotateRight(rotAngle);
                if (clockWise)
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowRightClock;
                }
                else
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowRightAnti;
                }
            }
            else if (partName.Equals("front"))
            {
                mRotater.rotateFront(rotAngle);
                if (clockWise)
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowFrontClock;
                }
                else
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowFrontAnti;
                }
            }
            else if (partName.Equals("back"))
            {
                mRotater.rotateBack(rotAngle);
                if (clockWise)
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowBackClock;
                }
                else
                {
                    texture =
                        FiveElementsIntTest.Properties.Resources.CubeArrowBackAnti;
                }
            }


            //here to add the arrow
            if (texture != null)
            {
                if (mArrowPtr != IntPtr.Zero)
                {
                    DeleteObject(mArrowPtr);
                    mArrowPtr = IntPtr.Zero;
                }

                mArrowPtr = texture.GetHbitmap();

                mOverlapImage.Source =
                       Imaging.CreateBitmapSourceFromHBitmap(
                       mArrowPtr, IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight((int)mOverlapImage.Width, (int)mOverlapImage.Height));
            }
        }

        public void SetSemiTurn(String partName, bool clockWise)
        {
            if (mLastTurnPhase != null)
            {
                setSemiTurn(mLastTurnPhase, !mLastTurnWise);
            }

            setSemiTurn(partName, clockWise);
            mLastTurnPhase = partName;
            mLastTurnWise = clockWise;
        }


    }
}

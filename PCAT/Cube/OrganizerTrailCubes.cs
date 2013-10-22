using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace FiveElementsIntTest.Cube
{
    public class OrganizerTrailCubes
    {
        private List<String> mOriginCubeLink;//12
        private List<List<String>> mSelectionCubezLink;//8 * 4\

        private CompCubeDisplay mOriginGraph;
        private CompCubeDisplay mRotatingGraph;
        private List<CompCubeDisplay> mSelGraphs;

        private List<Border> mSelBorders;

        private PageCube mPage;

        public OrganizerTrailCubes(PageCube pgCube)
        {
            mPage = pgCube;

            mOriginCubeLink = new List<String>();
            mSelectionCubezLink = new List<List<String>>();
            mSelGraphs = new List<CompCubeDisplay>();
            mSelBorders = new List<Border>();


            mOriginGraph = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, true);
            mOriginGraph.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            mPage.mBaseCanvas.Children.Add(mOriginGraph);
            Canvas.SetLeft(mOriginGraph, FEITStandard.PAGE_BEG_X + 146);
            Canvas.SetTop(mOriginGraph, FEITStandard.PAGE_BEG_Y);

            mRotatingGraph = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, false);
            mRotatingGraph.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            mPage.mBaseCanvas.Children.Add(mRotatingGraph);
            Canvas.SetLeft(mRotatingGraph, FEITStandard.PAGE_BEG_X + 146 + 180 + 146);
            Canvas.SetTop(mRotatingGraph, FEITStandard.PAGE_BEG_Y);

            //Console.Out.WriteLine(mRotatingGraph.mCubeBoxes.

            for (int i = 0; i < 8; i++)
            {
                CompCubeDisplay g = new CompCubeDisplay(180, LIGHT_MODE.TOP_PRIORITY, true);
                mSelGraphs.Add(g);
                mPage.mBaseCanvas.Children.Add(g);
                //g.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                Border b = GenBorder(180);
                mSelBorders.Add(b);
                mPage.mBaseCanvas.Children.Add(b);
                b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                b.BorderThickness = new Thickness(1.0);
                b.Visibility = Visibility.Hidden;

                g.MouseEnter += new System.Windows.Input.MouseEventHandler(g_MouseEnter);
                g.MouseLeave += new System.Windows.Input.MouseEventHandler(g_MouseLeave);

                if (i < 4)
                {
                    Canvas.SetTop(g, FEITStandard.PAGE_BEG_Y + 240);
                    Canvas.SetLeft(g, FEITStandard.PAGE_BEG_X + 16 * (i + 1) + i * 180);
                    Canvas.SetTop(b, FEITStandard.PAGE_BEG_Y + 240);
                    Canvas.SetLeft(b, FEITStandard.PAGE_BEG_X + 16 * (i + 1) + i * 180);
                }
                else
                {
                    Canvas.SetTop(g, FEITStandard.PAGE_BEG_Y + 420);
                    Canvas.SetLeft(g, FEITStandard.PAGE_BEG_X + 16 * (i - 3) + (i - 4) * 180);
                    Canvas.SetTop(b, FEITStandard.PAGE_BEG_Y + 420);
                    Canvas.SetLeft(b, FEITStandard.PAGE_BEG_X + 16 * (i - 3) + (i - 4) * 180);
                }
            }
            
        }

        public void SetFirstGraphTexture(char faceCode, System.Drawing.Bitmap texture)
        {
            mOriginGraph.SetTextureWithCode(faceCode, texture);
        }

        public void SetRotatingGraph(String partName, bool clockWise)
        {
            mRotatingGraph.SetSemiTurn(partName, clockWise);
        }

        public void SetSelectionGraph(int index, char faceCode, System.Drawing.Bitmap texture)
        {
            if (faceCode == 'A' || faceCode == 'B' || 
                faceCode == 'C' || faceCode == 'D')
            {
                if (index < 8)
                {
                    mSelGraphs[index].SetTextureWithCode(faceCode, texture);
                }
            }
        }

        private void g_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (mSelGraphs[i].Equals(sender))
                {
                    mSelBorders[i].Visibility = Visibility.Hidden;
                }
            }
        }

        private void g_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                if (mSelGraphs[i].Equals(sender))
                {
                    mSelBorders[i].Visibility = Visibility.Visible;
                }
            }
        }

        public Border GenBorder(int size)
        {
            Border retval = new Border();
            retval.Height = 180;
            retval.Width = 180;
            return retval;
        }
    }
}

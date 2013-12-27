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
        private CompCubeDisplay mRotatingGraph1;
        private CompCubeDisplay mRotatingGraph2;
        
        private List<CompCubeDisplay> mSelGraphs;
        private List<Border> mSelBorders;
        private Border mSave = null;
        private Border mSave1 = null;
        private Label herL;//分割线
        public Label Qnum;//题号
        
        public int ansCount = 0;
        public Boolean choose_buttom=true;
        
         public String Ans = "  ";
        CompCubeDisplay g;//选项方块
        Border b;//选项方框
        private PageCube mPage;

        private static int left_layout = ((int)System.Windows.SystemParameters.PrimaryScreenWidth)/2 - 482;
      //  private System.Windows.Input.MouseButtonEventHandler g_MouseDown1;
          int cube_lay_centr ;


          public bool optEnable = true;
        public OrganizerTrailCubes(PageCube pgCube)
        {
            mPage = pgCube;

            mOriginCubeLink = new List<String>();
            mSelectionCubezLink = new List<List<String>>();
            mSelGraphs = new List<CompCubeDisplay>();
            mSelBorders = new List<Border>();
            
            

             //Console.Out.WriteLine(mRotatingGraph.mCubeBoxes.
            if (mPage.Surface.Count < 50)
            {
           //     cube_lay_centre = 48 + 180 + 90;
                //3.----第一个图
                mOriginGraph = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, true);
                mOriginGraph.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mOriginGraph);
                //Canvas.SetLeft(mOriginGraph, FEITStandard.PAGE_BEG_X + cube_lay_centr);
                //Canvas.SetTop(mOriginGraph, FEITStandard.PAGE_BEG_Y +20);//3.----第一个图

                //4.---第一个示意图 带旋转的
                mRotatingGraph = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, false);
                mRotatingGraph.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mRotatingGraph);
                //Canvas.SetLeft(mRotatingGraph, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180);
                //Canvas.SetTop(mRotatingGraph, FEITStandard.PAGE_BEG_Y + 20);//4.-----第一个示意图 带旋转的

                mRotatingGraph1 = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, false);
                mRotatingGraph1.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mRotatingGraph1);
                //Canvas.SetLeft(mRotatingGraph1, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180 + 180);
                //Canvas.SetTop(mRotatingGraph1, FEITStandard.PAGE_BEG_Y + 20);// .-----第二个示意图 带旋转的

                mRotatingGraph2 = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, false);
                mRotatingGraph2.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mRotatingGraph2);
                //Canvas.SetLeft(mRotatingGraph2, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180 + 180 + 180);
                //Canvas.SetTop(mRotatingGraph2, FEITStandard.PAGE_BEG_Y + 20);//.-----第三个示意图 带旋转的
                herL = new Label();
                herL.Height = 2;
                herL.Width =965;
                herL.Background = Brushes.White;
                mPage.mBaseCanvas.Children.Add(herL);//水平分割线
                Canvas.SetLeft(herL, left_layout);
                Canvas.SetTop(herL, FEITStandard.PAGE_BEG_Y + 214);
                for (int i = 0; i < 5; i++)//1.---输出5个选项
                {
                    g = new CompCubeDisplay(180, LIGHT_MODE.TOP_PRIORITY, true);
                    mSelGraphs.Add(g);
                    mPage.mBaseCanvas.Children.Add(g);
                    //g.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                    b = GenBorder(180);
                    mSelBorders.Add(b);
                    mPage.mBaseCanvas.Children.Add(b);
                    b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    b.BorderThickness = new Thickness(2.0);
                    b.Visibility = Visibility.Hidden;

                    g.MouseEnter += new System.Windows.Input.MouseEventHandler(g_MouseEnter);
                    g.MouseLeave += new System.Windows.Input.MouseEventHandler(g_MouseLeave);
                    g.MouseDown += new System.Windows.Input.MouseButtonEventHandler(g_MouseDown);

                    //if (i < 4)
                    //{
                        Canvas.SetTop(g, FEITStandard.PAGE_BEG_Y + 280);
                        Canvas.SetLeft(g, left_layout + 16 * (i + 1) + i * 180);
                        Canvas.SetTop(b, FEITStandard.PAGE_BEG_Y + 280);
                        Canvas.SetLeft(b, left_layout + 16 * (i + 1) + i * 180);
                        
                    //}
                    //else
                    //{
                    //    Canvas.SetTop(g, FEITStandard.PAGE_BEG_Y + 420);
                    //    Canvas.SetLeft(g, FEITStandard.PAGE_BEG_X + 16 * (i - 3) + (i - 4) * 180);
                    //    Canvas.SetTop(b, FEITStandard.PAGE_BEG_Y + 420);
                    //    Canvas.SetLeft(b, FEITStandard.PAGE_BEG_X + 16 * (i - 3) + (i - 4) * 180);
                    //}
                } //1.----输出5个选项
            }
            else
            {
                //3.----第一个图
                mOriginGraph = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, true);
                mOriginGraph.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mOriginGraph);
                //Canvas.SetLeft(mOriginGraph, FEITStandard.PAGE_BEG_X + cube_lay_centr);
                //Canvas.SetTop(mOriginGraph, FEITStandard.PAGE_BEG_Y);//3.----第一个图

                //4.---第一个示意图 带旋转的
                mRotatingGraph = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, false);
                mRotatingGraph.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mRotatingGraph);
                //Canvas.SetLeft(mRotatingGraph, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180);
                //Canvas.SetTop(mRotatingGraph, FEITStandard.PAGE_BEG_Y);//4.-----第一个示意图 带旋转的

                mRotatingGraph1 = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, false);
                mRotatingGraph1.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mRotatingGraph1);
                //Canvas.SetLeft(mRotatingGraph1, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180 + 180);
                //Canvas.SetTop(mRotatingGraph1, FEITStandard.PAGE_BEG_Y);// .-----第二个示意图 带旋转的

                mRotatingGraph2 = new CompCubeDisplay(180, LIGHT_MODE.ALL_FACES, false);
                mRotatingGraph2.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                mPage.mBaseCanvas.Children.Add(mRotatingGraph2);
                //Canvas.SetLeft(mRotatingGraph2, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180 + 180 + 180);
                //Canvas.SetTop(mRotatingGraph2, FEITStandard.PAGE_BEG_Y);//.-----第三个示意图 带旋转的
                herL = new Label();
                herL.Height = 2;
                herL.Width = 775;
                herL.Background = Brushes.White;
                mPage.mBaseCanvas.Children.Add(herL);
                Canvas.SetLeft(herL, FEITStandard.PAGE_BEG_X);
                Canvas.SetTop(herL, FEITStandard.PAGE_BEG_Y + 214);
                for (int i = 0; i < 8; i++)//1.---输出8个选项
                {
                    g = new CompCubeDisplay(180, LIGHT_MODE.TOP_PRIORITY, true);
                    mSelGraphs.Add(g);
                    mPage.mBaseCanvas.Children.Add(g);
                    //g.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                    b = GenBorder(180);
                    mSelBorders.Add(b);
                    mPage.mBaseCanvas.Children.Add(b);
                    b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    b.BorderThickness = new Thickness(2.0);
                    b.Visibility = Visibility.Hidden;

                    g.MouseEnter += new System.Windows.Input.MouseEventHandler(g_MouseEnter);
                    g.MouseLeave += new System.Windows.Input.MouseEventHandler(g_MouseLeave);
                    g.MouseDown += new System.Windows.Input.MouseButtonEventHandler(g_MouseDown);

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
                } //1.----输出8个选项
            }
            Qnum = new Label();
            Qnum.Height = 38;
            Qnum.Width = 200;
            Qnum.Background = Brushes.Black;
            
            Qnum.FontSize = 26.0;
            
            mPage.mBaseCanvas.Children.Add(Qnum);
            Canvas.SetLeft(Qnum, left_layout + 32 + 360 + 86);
            Canvas.SetTop(Qnum, FEITStandard.PAGE_BEG_Y -25);

            

          //  nextQuestion.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(nextQuestion_MouseLeftButtonDown);
        }

        
        
        //private  void nextQuestion_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
            
        //}

        public void CubeLayout()
        {
            //=================================

            if (mPage.Surface.Count < 50)
            {
                
                //3.----第一个图

                Canvas.SetLeft(mOriginGraph, left_layout + cube_lay_centr);
                Canvas.SetTop(mOriginGraph, FEITStandard.PAGE_BEG_Y + 20);//3.----第一个图

                //4.---第一个示意图 带旋转的

                Canvas.SetLeft(mRotatingGraph, left_layout + cube_lay_centr + 180);
                Canvas.SetTop(mRotatingGraph, FEITStandard.PAGE_BEG_Y + 20);//4.-----第一个示意图 带旋转的


                Canvas.SetLeft(mRotatingGraph1, left_layout + cube_lay_centr + 180 + 180);
                Canvas.SetTop(mRotatingGraph1, FEITStandard.PAGE_BEG_Y + 20);// .-----第二个示意图 带旋转的


                Canvas.SetLeft(mRotatingGraph2, left_layout + cube_lay_centr + 180 + 180 + 180);
                Canvas.SetTop(mRotatingGraph2, FEITStandard.PAGE_BEG_Y + 20);//.-----第三个示意图 带旋转的
                
            }
            else
            {
                //3.----第一个图
                
                Canvas.SetLeft(mOriginGraph, FEITStandard.PAGE_BEG_X + cube_lay_centr);
                Canvas.SetTop(mOriginGraph, FEITStandard.PAGE_BEG_Y);//3.----第一个图

                //4.---第一个示意图 带旋转的
                
                Canvas.SetLeft(mRotatingGraph, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180);
                Canvas.SetTop(mRotatingGraph, FEITStandard.PAGE_BEG_Y);//4.-----第一个示意图 带旋转的

               
                Canvas.SetLeft(mRotatingGraph1, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180 + 180);
                Canvas.SetTop(mRotatingGraph1, FEITStandard.PAGE_BEG_Y);// .-----第二个示意图 带旋转的

              
                Canvas.SetLeft(mRotatingGraph2, FEITStandard.PAGE_BEG_X + cube_lay_centr + 180 + 180 + 180);
                Canvas.SetTop(mRotatingGraph2, FEITStandard.PAGE_BEG_Y);//.-----第三个示意图 带旋转的
                
      
            }

            //===================================
        }

        //2.----示意图的旋转
        public void SetFirstGraphTexture(String faceCode, System.Drawing.Bitmap texture)
        {
           
            mOriginGraph.SetTextureWithCode(faceCode, texture);
        }
        public void SetRotatingGraph(String partName, bool clockWise)
        {
             mRotatingGraph.Show(true);
            mRotatingGraph.SetSemiTurn(partName, clockWise);
        }

        public void SetRotatingGraph1(String partName, bool clockWise)
        {
             mRotatingGraph1.Show(true);
            mRotatingGraph1.SetSemiTurn(partName, clockWise);
        }

        public void SetRotatingGraph2(String partName, bool clockWise)
        {

             mRotatingGraph2.Show(true);
            mRotatingGraph2.SetSemiTurn(partName, clockWise);
        }
        public void ClearRotGraph()
        {
            mRotatingGraph.Show(false);
        }
        public void ClearRotGraph1()
        {
            mRotatingGraph1.Show(false);
        }
       
        public void ClearRotGraph2()
        {
            mRotatingGraph2.Show(false);
        }
      //2.------示意图的旋转

        //新加的方法 用于清除魔方上所有已经设置的贴图
        public void ClearCompCubeTex(CompCubeDisplay cubeDis)
        {
            cubeDis.SetTextureWithCode("A", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("B", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("C", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("D", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("E", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("F", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("G", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("H", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("I", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("J", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("L", FiveElementsIntTest.Properties.Resources.BLANK);
            cubeDis.SetTextureWithCode("K", FiveElementsIntTest.Properties.Resources.BLANK);
        }

        public void ClearAllCubzTex()
        {

            if (mPage.Surface.Count < 50)
            {
                for (int i = 0; i < 5; i++)
                {
                    ClearCompCubeTex(mSelGraphs[i]);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    ClearCompCubeTex(mSelGraphs[i]);
                }
            }
            ClearCompCubeTex(mOriginGraph);
            mSave = null;
        }

        public void SetSelectionGraph(int index, String  faceCode, System.Drawing.Bitmap texture)
        {
            if (faceCode == "A" || faceCode == "B" || 
                faceCode == "C" || faceCode == "D")
            {
                if (index < 8)
                {
                    mSelGraphs[index].SetTextureWithCode(faceCode, texture);
                }
            }
        }
        // 
        private void g_MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
        {

            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
              choose_buttom = false;
              mPage._Control_choose = false;
              if (optEnable)
              {
                if (mPage.Surface.Count < 50)
                {
                    for (int i = 0; i < 5; i++)//显示五个选项
                    {

                        if (mSelGraphs[i].Equals(sender))
                        {
                            if (mSave != null)
                                mSave.Visibility = Visibility.Hidden;

                            if (mPage.line_num_count == 0)
                            {
                                mSelBorders[i].Visibility = Visibility.Visible;
                            }
                            else if (mPage.line_num_count > 0 && mPage.line_num_count < mPage.line_num)
                            {
                                
                                mSelBorders[i].Visibility = Visibility.Visible;
                                mSave = mSelBorders[i];
                                Ans = (i + 1).ToString();
                                if (mPage.line_num_count > 0 && mPage.line_num_count < 6)
                                {

                                    if (!mPage._isDisplayhide)
                                    {
                                         if (mPage.Anstandard[mPage.Anstandard.Count - 1] == Ans)
                                        {
                                            //mPage._flash_Display.Stop();
                                            //mPage.t_Display.Close();
                                             if (mPage.tip_display.Visibility == System.Windows.Visibility.Hidden)
                                                {
                                                mPage.tip_display.Visibility = System.Windows.Visibility.Visible;
                                                }

                                            mPage.tip_display.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                            mPage.tip_display.Content = "正  确";
                                            optEnable = false;
                                        

                                         }
                                        else
                                         {
                                            //mPage._flash_Display.Stop();
                                            //mPage.t_Display.Stop();
                                            if (mPage.tip_display.Visibility == System.Windows.Visibility.Hidden)
                                            {
                                                mPage.tip_display.Visibility = System.Windows.Visibility.Visible;
                                            }
                                            mPage.tip_display.Foreground = Brushes.Red;
                                            mPage.tip_display.Content = "选择错误，正确答案为：" + mPage.Anstandard[mPage.Anstandard.Count - 1];
                                            optEnable = false;
                                        
                                        }
                                    }
                                    else if(mPage._isDisplayhide)
                                    {
                                        if (mPage.Anstandard[mPage.hide_count - 1] == Ans)
                                        {
                                            if (mPage.tip_display.Visibility == System.Windows.Visibility.Hidden)
                                            {
                                                mPage.tip_display.Visibility = System.Windows.Visibility.Visible;
                                            }
                                            mPage.tip_display.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                            mPage.tip_display.Content = "正  确";
                                            optEnable = false;
                                            //mPage.t_Display.Close();

                                        }
                                        else
                                        {
                                            if (mPage.tip_display.Visibility == System.Windows.Visibility.Hidden)
                                            {
                                                mPage.tip_display.Visibility = System.Windows.Visibility.Visible;
                                            }
                                            mPage.tip_display.Foreground = Brushes.Red;
                                            mPage.tip_display.Content = "选择错误，正确答案为：" + mPage.Anstandard[mPage.hide_count - 1];
                                            optEnable = false;
                                            //mPage.t_Display.Stop();
                                        }
                                    }

                                    //mPage.t_Display.Stop();
                                    //mPage._flash_Display.Stop();

                                }
                                else//做完前面的6道
                                {
                                    if (mPage.tip_display.Visibility == System.Windows.Visibility.Visible)
                                    {
                                        mPage.tip_display.Visibility = System.Windows.Visibility.Hidden;
                                    }
                                    

                                    mPage.tip_display.Foreground = Brushes.Black;

                                    //mPage.t_Display.Stop();

                                    mPage.tip_display.Foreground = Brushes.Red;
                                }
                            }
                            else break;
                        }

                    }//-----for

                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {

                        if (mSelGraphs[i].Equals(sender))
                        {
                            if (mSave != null)
                                mSave.Visibility = Visibility.Hidden;

                            if (mPage.line_num_count == 0)
                            {
                                mSelBorders[i].Visibility = Visibility.Visible;
                            }
                            else if (mPage.line_num_count > 0 && mPage.line_num_count < mPage.line_num)
                            {

                                mSelBorders[i].Visibility = Visibility.Visible;
                                mSave = mSelBorders[i];
                                Ans = (i + 1).ToString();



                            }
                            else break;
                        }

                    }//-----for

                }

               
            }
        }
            choose_buttom = true;
         //   mPage.mBaseCanvas.IsEnabled = false;
        }
        private void g_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mPage.Surface.Count < 50)
            {
                if (mSave == null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (mSelGraphs[i].Equals(sender))
                        {

                            mSelBorders[i].Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
            else
            {
                if (mSave == null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (mSelGraphs[i].Equals(sender))
                        {

                            mSelBorders[i].Visibility = Visibility.Hidden;
                        }
                    }
                }
            }

            
            //else
            //{

            //}
        }
        private void g_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mPage.Surface.Count < 50)
            {
                if (mSave == null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (mSelGraphs[i].Equals(sender))
                        {
                            mSelBorders[i].Visibility = Visibility.Visible;
                        }
                    }

                }
            }
            else
            {
                if (mSave == null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (mSelGraphs[i].Equals(sender))
                        {
                            mSelBorders[i].Visibility = Visibility.Visible;
                        }
                    }

                }
            }
          
            //else
            //{
            //    for (int i = 0; i < 8; i++)
            //    {
            //        if (mSelGraphs[i].Equals(sender))
            //        {
            //            mSelBorders[i].Visibility = Visibility.Hidden;
            //        }
            //    }
            //}

        }

        public void borderVis(int seq)
        {
            mSelBorders[seq].Visibility = Visibility.Visible;
            mSave1 = mSelBorders[seq];
            mPage.mBaseCanvas.IsEnabled = false;
        }

        public void borderHide()
        {
            mSave1.Visibility = Visibility.Hidden;
            mPage.mBaseCanvas.IsEnabled = true;
        }
        public void clearSeSelGraphs()
        {
            if (mPage.Surface.Count < 50)
            {
                for (int i = 0; i <5; i++)
                {

                    mSelBorders[i].Visibility = Visibility.Hidden;
                    mSave1 = mSelBorders[i];
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {

                    mSelBorders[i].Visibility = Visibility.Hidden;
                    mSave1 = mSelBorders[i];
                }
            }
        }
        public Border GenBorder(int size)
        {
            if (mPage.Surface.Count < 50)
            {
                Border retval = new Border();
                retval.Height = 180;
                retval.Width = 180;
                return retval;
            }
            else
            {
                Border retval = new Border();
                retval.Height = 180;
                retval.Width = 180;
                return retval;
            }
            
        }

        public void canvEnable()
        {
            mPage.mBaseCanvas.IsEnabled = true;

        }

        public Brush Silver { get; set; }

        public int cube_Layout(int temp)
        {
            cube_lay_centr = temp;
            return cube_lay_centr;
        }
        //public void ClearCube()//清除页面所有方块
        //{
        //    mPage.mBaseCanvas.Children.Remove(mOriginGraph);
        //    mPage.mBaseCanvas.Children.Remove(mRotatingGraph);
        //    mPage.mBaseCanvas.Children.Remove(mRotatingGraph1);
        //    mPage.mBaseCanvas.Children.Remove(mRotatingGraph2);
        //    mPage.mBaseCanvas.Children.Remove(herL);
        //    mPage.mBaseCanvas.Children.Remove(g);
        //    mPage.mBaseCanvas.Children.Remove(b);

        //}

       

    }
}

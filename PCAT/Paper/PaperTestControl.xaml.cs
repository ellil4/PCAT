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

namespace FiveElementsIntTest.Paper
{
    /// <summary>
    /// PaperTestControl.xaml 的互動邏輯
    /// </summary>
    public partial class PaperTestControl : UserControl
    {
        //读文件
       // PaperReadFile vPaperReadFile;
      //  private String _temporary;//图片名 中间变量

        //窗口控制
        private static int actual_x = 0;//((int)System.Windows.SystemParameters.PrimaryScreenWidth) / 240
        private static int actual_y = 100;//- 50;

        // border 布局
        PaperBorderControl mbordercontrol;
        private int bordertop;                                                                      // = 258;
        private int borderleft ;                                                                   //= 95;
        private int borderadd ;                                                                   //= 179;
        private Border b ;
        private List<Border> mBorders;
        //第二行border
        private int bordertop2;                                                                 // = 408;
        private int borderleft2 ;                                                              //= 95;


        // image 布局 
        PaperImageControl mImagecontrol;
        private Image him;//题目图片及图片参数
        private List<Image> mhImages;
        private int himagetop;                                                                         // = 68;
        private int himageleft;                                                                       // = 105;
        private int himageadd ;                                                                      //= 179;
        




        //选项图片及选项参数
        private Image im;
        private List<Image> vImages;
        private int imagetop ;                                                                         //= 268;
        private int imageleft ;                                                                       //= 105;
        private int imageadd;                                                                        // = 179;

        //第二行图片
        private int imagetop2;                                                                     // = 418;
        private int imageleft2 ;                                                                  //= 105;

        private Border cut_line;

        //操作控制
        private Boolean _control ; //控制border 显示 隐藏

        private Border _full;//控制多次选择答案

        private List<String> _inputOpt;//只有选项的List

        private Label Qnum;//显示题号

        public String opt = " ";//选择的答案

        private int  layleft = 0;

        //前五题答案提示

        PagePaper mPagePaper;

        

        public PaperTestControl(List<String> _input, int head_num, int _linenum, int _linenumcount,PagePaper pap)
        {
            InitializeComponent();//先写

            mPagePaper = pap;

            _layoutPaper(_input, head_num, _linenum, _linenumcount);

            cutLine();
            
        }

        public void _layoutPaper(List<String> _input, int head_num, int _linenum, int _linenumcount)//整张测试题
        {
            PapertestCanvas.Children.Clear();
            PapertestCanvas.IsEnabled = true;
            
            _control = false;
             bordertop = 258;
             borderleft = 95  ;
             borderadd = 172;
             
            //第二行border
             bordertop2 = 408;
             borderleft2 = 95 ;

            // image 布局 
             himagetop = 68;
             himageleft = 105;
             himageadd = 172;
        
            //选项图片及选项参数
             imagetop = 268;
             imageleft = 105  ;
             imageadd = 172;

            //第二行图片
             imagetop2 = 418;
             imageleft2 = 105  ;

           //操作
             _numshow();
              Qnum.Foreground = Brushes.White;
              if (!mPagePaper.recede_control)
                  Qnum.Content = "(" + (_linenumcount - 2) + ")";
              else
                  Qnum.Content = "(" + "+" +(_linenumcount + 1) + ")";
              
             if (_input.Count < 10)//选项非8个
             {
                 layoutTestFive(_input, head_num);
                 layoutOptionFive(_inputOpt);
             }
             else
             {
                
                 layoutTest(_input, head_num);//载入题目
                 layoutOption(_inputOpt);//载入选项
             }


                _input.Clear();
                _inputOpt.Clear();

                
       
        }

        


        private void layoutTestFive(List<String> _temporary, int item_num)//题目
        {


            mImagecontrol = new PaperImageControl();

            mhImages = new List<Image>();

           

            //载入题目
            if (item_num == 2 ) //偶数个题目
            {
                himageleft = 105 + layleft + (330 - (180 - 82) / 2) - 82; //330 是分割线的长度1/2   85是分割线位移  82图宽 180图片间距

            }
            else if (item_num == 4)
            {
                himageleft = 105 + layleft + (330 - (180 - 82) / 2) - 82 - 180;
            }
            else//奇数个题目
            {
                himageleft = 105 + layleft + (330 - 41) - 180; //330 是分割线的长度1/2   85是分割线位移

            }
            for (int i = 0; i < item_num; i++)//item_num 题目的个数
            {
                  him = new Image();
              //  him = mImagecontrol.GetImage(82, 74);
                him.Stretch = Stretch.Fill;

                System.Drawing.Image pic = System.Drawing.Image.FromFile("Paper\\PaperRes\\Test\\Pa\\" + _temporary[0] + ".bmp");
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(pic);

                IntPtr hBitmap = bmp.GetHbitmap();
                System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(82, 74));//FromEmptyOptions() 源图像大小
                him.Source = WpfBitmap;
                //BitmapImage bitim = new BitmapImage();
                //bitim.BeginInit();
                //bitim.UriSource = new Uri("\\PCAT;component\\Images\\" + _temporary[0] + ".bmp", UriKind.Relative);
                //bitim.EndInit();
                //him.Stretch = Stretch.Fill;
                //him.Source = bitim;
                mhImages.Add(him);
                PapertestCanvas.Children.Add(him);

                Canvas.SetTop(him, actual_y + himagetop);//
             
                Canvas.SetLeft(him, actual_x + himageleft);//
                himageleft = himageleft + himageadd;

                _temporary.RemoveAt(0);
            }
            _inputOpt = _temporary;

        }
        
        private void layoutTest(List<String> _temporary, int item_num)//题目
        {


            mImagecontrol = new PaperImageControl();

            mhImages = new List<Image>();


            //载入题目

            for (int i = 0; i < item_num; i++)
            {
                him = new Image();
                him = mImagecontrol.GetImage(82, 74);
                BitmapImage bitim = new BitmapImage();
                bitim.BeginInit();
           //     bitim.UriSource = new Uri("\\PCAT;component\\Images\\" + _temporary[0] + ".bmp", UriKind.Relative);
                bitim.EndInit();
                him.Stretch = Stretch.Fill;
                him.Source = bitim;
                mhImages.Add(him);
                PapertestCanvas.Children.Add(him);
                Canvas.SetTop(him, actual_y + himagetop);//
                Canvas.SetLeft(him, actual_x + himageleft);// 
                himageleft = himageleft + himageadd;

                _temporary.RemoveAt(0);
        }
            _inputOpt = _temporary;

     }

        private void layoutOptionFive(List<String> _temporary)
        {
            mbordercontrol = new PaperBorderControl();

            mBorders = new List<Border>();

            mImagecontrol = new PaperImageControl();

            vImages = new List<Image>();

            cutLine();

            cut_line.Width = 660 + 180; //分割线加长

            imageleft = 130 - 85;

            borderleft = 120 - 85;
            // 载入选项
            for (int i = 0; i < _temporary.Count; i++)// 8 是选项 ps：在内部就可以显示border 放外部循环就不行
            {
                //border 
                b = mbordercontrol.GenBorder(102, 94);
                mBorders.Add(b);
                PapertestCanvas.Children.Add(b);


                //image
                im = new Image();

                im.Stretch = Stretch.Fill;

                System.Drawing.Image picc = System.Drawing.Image.FromFile("Paper\\PaperRes\\Test\\PaS\\" + _temporary[i] + ".bmp");
                System.Drawing.Bitmap bmpc = new System.Drawing.Bitmap(picc);

                IntPtr hBitmapc = bmpc.GetHbitmap();
                System.Windows.Media.ImageSource WpfBitmapc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmapc, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(82, 74));//FromEmptyOptions() 源图像大小
                im.Source = WpfBitmapc;

                //im = mImagecontrol.GetImage(82, 74);
                //BitmapImage bitim = new BitmapImage();
                //bitim.BeginInit();
                //bitim.UriSource = new Uri("\\PCAT;component\\Images\\" + _temporary[i] + ".bmp", UriKind.Relative);
                //bitim.EndInit();
                //im.Stretch = Stretch.Fill;
                //im.Source = bitim;
                vImages.Add(im);
                PapertestCanvas.Children.Add(im);

                //Event

                im.MouseDown += new MouseButtonEventHandler(im_MouseDown);
                im.MouseEnter += new MouseEventHandler(im_MouseEnter);
                im.MouseLeave += new MouseEventHandler(im_MouseLeave);

                
                    Canvas.SetTop(b, actual_y + bordertop);//
                    Canvas.SetLeft(b, actual_x + borderleft);// 
                    borderleft = borderleft + borderadd;

                    Canvas.SetTop(im, actual_y + imagetop);//
                    Canvas.SetLeft(im, actual_x + imageleft);//
                    imageleft = imageleft + imageadd;



                
            }//---for
        }

        private void layoutOption(List<String> _temporary)//选项 8个
        {
            mbordercontrol = new PaperBorderControl();
           
            mBorders = new List<Border>();

            mImagecontrol = new PaperImageControl();

            vImages = new List<Image>();

            cutLine();


            // 载入选项
            for (int i = 0; i < _temporary.Count; i++)// 8 是选项 ps：在内部就可以显示border 放外部循环就不行
            {
                //border 
                b = mbordercontrol.GenBorder(102, 94);
                mBorders.Add(b);
                PapertestCanvas.Children.Add(b);
                

                //image
                im = new Image();
                im = mImagecontrol.GetImage(82, 74);
                BitmapImage bitim = new BitmapImage();
                bitim.BeginInit();
               // bitim.UriSource = new Uri("\\PCAT;component\\Images\\" + _temporary[i] + ".bmp", UriKind.Relative);
                bitim.EndInit();
                im.Stretch = Stretch.Fill;
                im.Source = bitim;
                vImages.Add(im);
                PapertestCanvas.Children.Add(im);
               
                //Event

                im.MouseDown += new MouseButtonEventHandler(im_MouseDown);
                im.MouseEnter += new MouseEventHandler(im_MouseEnter);
                im.MouseLeave += new MouseEventHandler(im_MouseLeave);

                if (i < (_temporary.Count / 2))
                {

                    Canvas.SetTop(b, actual_y + bordertop);//
                    Canvas.SetLeft(b, actual_x + borderleft);// 
                    borderleft = borderleft + borderadd;

                    Canvas.SetTop(im, actual_y + imagetop);//
                    Canvas.SetLeft(im, actual_x + imageleft);//
                    imageleft = imageleft + imageadd;
                    
                    
                    
                }
                else 
                {
                    Canvas.SetTop(b, actual_y + bordertop2);// 
                    Canvas.SetLeft(b, actual_x + borderleft2);// 
                    borderleft2 = borderleft2 + borderadd;

                    Canvas.SetTop(im, actual_y + imagetop2);//
                    Canvas.SetLeft(im, actual_x + imageleft2);//
                    imageleft2 = imageleft2 + imageadd;
                    
                }
             }//---for

        }

        private void im_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_control)
            {
                for (int i = 0; i < vImages.Count; i++)
                {
                    if (vImages[i].Equals(sender))
                    {
                        mBorders[i].Visibility = Visibility.Hidden;
                    }
                }
            }

        }

        private void im_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_control)
            {

                for (int i = 0; i < vImages.Count; i++)
                {
                    if (vImages[i].Equals(sender))
                    {
                        mBorders[i].Visibility = Visibility.Visible;


                    }
                }

            }
        }

        private void im_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mPagePaper._Control_press = true;

                for (int i = 0; i < vImages.Count; i++)
                {
                    if (vImages[i].Equals(sender))
                    {

                        if (mPagePaper.line_num_count < 6 ) //前五道题答案提示
                        {


                            //mPagePaper.t_Display.Stop();
                            mPagePaper.tip_display.Visibility = System.Windows.Visibility.Visible;

                            if (_full != null) _full.Visibility =  Visibility.Hidden;

                            _full = mBorders[i];
                        

                            mBorders[i].Visibility = Visibility.Visible; //显示通用
                            _control = true; //显示通用
                   
                            opt = (i + 1).ToString();//被试选择答案

                           

                            PapertestCanvas.IsEnabled = false;
                            
                            if (mPagePaper.correct_ans != opt )
                            {
                                mPagePaper.tip_display.Foreground = Brushes.Red;
                                mPagePaper.tip_display.Content = "选择错误，正确答案为：" + mPagePaper.correct_ans;
                                if (mPagePaper.re_count_num<2) mPagePaper.recede_control = true;
                               
                            }
                            else
                            {
                                //mPagePaper.t_Display.Stop();
                                mPagePaper.tip_display.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                mPagePaper.tip_display.Content = "正  确";
                            }

                            //mPagePaper.t_Display.Stop();
                            //mPagePaper._flash_Display.Stop();
                        
                      }
                        else if (mPagePaper.line_num_count >= 6)
                        {

                            mPagePaper.tip_display.Foreground = Brushes.Black;

                            //mPagePaper.t_Display.Stop();

                            mPagePaper.tip_display.Foreground = Brushes.Red;
                        /*******************用于选择多次****************************/
                        if (_full != null) _full.Visibility =  Visibility.Hidden;

                        _full = mBorders[i];
                        /***********************************************/

                        mBorders[i].Visibility = Visibility.Visible; //显示通用
                        _control = true; //显示通用
                   



                        /******************************用于写文件*********************************/

                        opt = (i + 1).ToString();
                       
                        /*************************************************************************/

                            ///////////////////////////实验开始/////////////
                        //if (mPagePaper.correct_ans != opt)
                        //{
                        //    mPagePaper.tip_display.Content = "选择错误，正确答案为：" + mPagePaper.correct_ans;
                        //    if (mPagePaper.re_count_num < 2) mPagePaper.recede_control = true;

                        //}
                        //else
                        //{
                        //    mPagePaper.tip_display.Content = "恭喜你，答对了！";
                        //}

                            ////////////////////实验结束//////////////
                      }
                    }
                }
            }
        }

        public void clearBorder()
        {
            for (int i = 0; i < vImages.Count; i++)
            {
               
                    mBorders[i].Visibility = Visibility.Hidden;
                    opt = " ";
            }
        }

        private void cutLine()
        {
            cut_line = new Border();
            cut_line.Width = 660;
            cut_line.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            cut_line.BorderThickness = new Thickness(3.0);
            Canvas.SetTop(cut_line, actual_y + 228);//
            Canvas.SetLeft(cut_line, actual_x + layleft);
            PapertestCanvas.Children.Add(cut_line);
        }

        private void _numshow()
        {
            Qnum = new Label();
            Qnum.Height = 38;
            Qnum.Width = 200;
            Qnum.Background = Brushes.Black;
            Qnum.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            Qnum.FontSize = 26.0;

            PapertestCanvas.Children.Add(Qnum);
            Canvas.SetLeft(Qnum, 406);
            Canvas.SetTop(Qnum, 70);
        }

    }






}
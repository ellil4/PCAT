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
    /// ExamplePage.xaml 的互動邏輯
    /// </summary>
    public partial class ExamplePage : UserControl
    {
       
        //窗口控制
        private int actual_x = 0;
        private int actual_y = 0 ;

        // border 布局
        PaperBorderControl mbordercontrol;
        private int bordertop = 358;
        private int borderleft = 95;
        private int borderadd = 129;
        private Border b;
        private List<Border> mBorders;
        // image 布局 
        PaperImageControl mImagecontrol;
        private Image im;
        private List<Image> mImages;
        private int imagetop = 368;
        private int imageleft = 105;
        private int imageadd = 129;
        //操作控制
        private Boolean _control = false;

        //题目布局
        private LayoutInstruction mLayoutInstruction;
        private Image im_item1;
        private Image im_item2;
       public ExamplePage()
        {
            InitializeComponent();
            BaseCanvas.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            BaseCanvas.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            mbordercontrol = new PaperBorderControl();

            mBorders = new List<Border>();

            mImagecontrol = new PaperImageControl();

            mImages = new List<Image>();

            mLayoutInstruction = new LayoutInstruction(ref BaseCanvas);

            mLayoutInstruction.addInstruction(-FEITStandard.PAGE_BEG_Y, -FEITStandard.PAGE_BEG_X, FEITStandard.PAGE_WIDTH / 100 * 81, 140,
                "    横线上方呈现了折纸和扎孔（扎透了所有的层）的过程，横线下方给出了５种纸被展开后的情况，其中只有一个是正确的答案。"
             , "Kaiti_GB2312", 30, Color.FromRgb(255, 255, 255));

            //窗口布局

            

            Canvas.SetLeft(border1, actual_x + 55);
            Canvas.SetTop(border1, actual_y + 320);

            Canvas.SetLeft(label1, actual_x + 609);//正确答案
            Canvas.SetTop(label1, actual_y + 595);

            Canvas.SetLeft(label2, actual_x + 214);//请选择正确答案
            Canvas.SetTop(label2, actual_y + 500);

            Canvas.SetLeft(label3, actual_x + 294);//正确答案
            Canvas.SetTop(label3, actual_y + 270);

            Canvas.SetLeft(label4, actual_x + 433);//正确答案
            Canvas.SetTop(label4, actual_y + 270);


            additem();//练习题目
            
            
            
            //
            for (int i = 0; i < 5; i++)
            {
                //border 
                b = mbordercontrol.GenBorder(102, 94);
                mBorders.Add(b);
                this.BaseCanvas.Children.Add(b);
                Canvas.SetTop(b, actual_y + bordertop);
                Canvas.SetLeft(b, actual_x + borderleft);
                borderleft = borderleft + borderadd;

                //image
                string temp = (i + 1).ToString();
                im = new Image();
                im.Stretch = Stretch.Fill;

                System.Drawing.Image pie = System.Drawing.Image.FromFile("Paper\\PaperRes\\Example\\O" + temp + ".bmp");
                System.Drawing.Bitmap bmpe = new System.Drawing.Bitmap(pie);

                IntPtr hBitmape = bmpe.GetHbitmap();
                System.Windows.Media.ImageSource WpfBitmape = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmape, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(82, 74));//FromEmptyOptions() 源图像大小
                im.Source = WpfBitmape;
                //im = mImagecontrol.GetImage(82,74);
                //BitmapImage bitim = new BitmapImage();
                //bitim.BeginInit();
                //bitim.UriSource = new Uri("/PCAT;component/Images/O" + temp + ".bmp", UriKind.Relative);
                //bitim.EndInit();
                //im.Stretch = Stretch.Fill;
                //im.Source = bitim;
                mImages.Add(im);
                this.BaseCanvas.Children.Add(im);
                Canvas.SetTop(im, actual_y + imagetop);
                Canvas.SetLeft(im, actual_x + imageleft);
                imageleft = imageleft + imageadd;
                //Event

                im.MouseDown += new MouseButtonEventHandler(im_MouseDown);
                im.MouseEnter += new MouseEventHandler(im_MouseEnter);
                im.MouseLeave += new MouseEventHandler(im_MouseLeave);
            
            }
        
        }

       private void im_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_control)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (mImages[i].Equals(sender))
                    {
                        mBorders[i].Visibility = Visibility.Hidden;
                    }
                }
            }
      
        
        }

      private  void im_MouseEnter(object sender, MouseEventArgs e)
        {

            if (!_control)
            {

                    for (int i = 0; i < 5; i++)
                    {
                        if (mImages[i].Equals(sender))
                        {
                            mBorders[i].Visibility = Visibility.Visible;


                        }
                    }

            }
           
        }

      private  void im_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_control)
            {

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (mImages[i].Equals(sender))
                        {
                           // BaseCanvas.IsEnabled = false;
                            label2.FontFamily = new FontFamily("KaiTi_GB2312");
                            if (i == 2)
                            {
                                label2.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                label2.Content = "正  确";
                            }

                            else
                            {
                                label2.Foreground = Brushes.Red;
                                label2.Content = "错误，正确答案：3";
                            }
                            mBorders[i].Visibility = Visibility.Visible;
                            _control = true;
                        }
                    }
                }
            }
        }

      private void additem()
      {
                  im_item1 = mImagecontrol.GetImage(82, 74);
                  BitmapImage bitim1 = new BitmapImage();
                  bitim1.BeginInit();
                  bitim1.UriSource = new Uri(FEITStandard.GetExePath() + "Paper\\PaperRes\\Example\\im_item1.bmp");
                  bitim1.EndInit();
                  im_item1.Stretch = Stretch.Fill;
                  im_item1.Source = bitim1;
                  
                  this.BaseCanvas.Children.Add(im_item1);
                  Canvas.SetTop(im_item1, actual_y + 185);
                  Canvas.SetLeft(im_item1, actual_x + 290);

                  im_item2 = mImagecontrol.GetImage(82, 74);
                  BitmapImage bitim2 = new BitmapImage();
                  bitim2.BeginInit();
                  bitim2.UriSource = new Uri(FEITStandard.GetExePath() + "Paper\\PaperRes\\Example\\im_item2.bmp");
                  bitim2.EndInit();
                  im_item2.Stretch = Stretch.Fill;
                  im_item2.Source = bitim2;
                  
                  this.BaseCanvas.Children.Add(im_item2);
                  Canvas.SetTop(im_item2, actual_y + 185);
                  Canvas.SetLeft(im_item2, actual_x + 429);
      }
 
        
    }
}

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

namespace FiveElementsIntTest.Cube
{
    /// <summary>
    /// ExerciseTwoPage.xaml 的互動邏輯
    /// </summary>
    public partial class ExerciseTwoPage : UserControl
    {
        //窗口控制
        private int actual_x = 0;
        private int actual_y = 0 ;

        // border 布局
        bordercontrol mbordercontrol;
        private int bordertop = 322;
        private int borderleft = 64;
        private int borderadd = 138;
        private Border b;
        private List<Border> mBorders;
        // image 布局 
        Imagecontrol mImagecontrol;
        private Image im;
        private List<Image> mImages;
        private int imagetop = 342;
        private int imageleft = 78;
        private int imageadd = 138;
        //操作控制
        private Boolean _control = false;

        public ExerciseTwoPage()
        {
            InitializeComponent();

            mbordercontrol = new bordercontrol();

            mBorders = new List<Border>();

            mImagecontrol = new Imagecontrol();

            mImages = new List<Image>();

            //窗口布局

            BitmapImage bit = new BitmapImage();
            bit.BeginInit();
            bit.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-2-0.bmp");
            bit.EndInit();
            image1.Source = bit;

            Canvas.SetLeft(image1, actual_x + 27);
            Canvas.SetTop(image1, actual_y + 26);

            Canvas.SetLeft(border1, actual_x + 38);
            Canvas.SetTop(border1, actual_y + 285);

            Canvas.SetLeft(label1, actual_x + 190);
            Canvas.SetTop(label1, actual_y + 485);

           
            
            
            
            
            
            
            
            
            //
            for (int i = 0; i < 5; i++)
            {
                //border 
                b = mbordercontrol.GenBorder(120, 130);
                mBorders.Add(b);
                this.canvas1.Children.Add(b);
                Canvas.SetTop(b, actual_y + bordertop);
                Canvas.SetLeft(b, actual_x + borderleft);
                borderleft = borderleft + borderadd;

                //image
                string temp = (i + 1).ToString();
                im = mImagecontrol.GetImage(93, 100);
                BitmapImage bitim = new BitmapImage();
                bitim.BeginInit();
                bitim.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-2-" + temp + ".bmp");
                bitim.EndInit();
                im.Stretch = Stretch.Fill;
                im.Source = bitim;
                mImages.Add(im);
                this.canvas1.Children.Add(im);
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (mImages[i].Equals(sender))
                    {
                        canvas1.IsEnabled = false;

                        if (i == 4)
                        {
                            label1.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            label1.Content = "正  确";
                        }
                        else
                        {
                            label1.Foreground = Brushes.Red;
                            label1.Content = "错误，正确答案：5";
                        }
                        mBorders[i].Visibility = Visibility.Visible;
                        _control = true;
                    }
                }
            }
        }
 
    
    
    }//--------类
}


/* <Image Canvas.Left="68" Canvas.Top="340" Height="100" Name="image2" Stretch="Fill" Width="93" />
        <Image Canvas.Left="210" Canvas.Top="340" Height="100" Name="image3" Stretch="Fill" Width="93" />
        <Image Canvas.Left="342" Canvas.Top="340" Height="100" Name="image4" Stretch="Fill" Width="93" />
        <Image Canvas.Left="480" Canvas.Top="340" Height="100" Name="image5" Stretch="Fill" Width="93" />
        <Image Canvas.Left="615" Canvas.Top="337" Height="103" Name="image6" Stretch="Fill" Width="93" />*/
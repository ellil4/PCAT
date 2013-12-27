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
    /// ExerciseOnePage.xaml 的互動邏輯
    /// </summary>
    public partial class ExerciseOnePage : UserControl
    {
        private Boolean border_control = false;
        private int actual_x = 0;
        private int actual_y = 0;
        public ExerciseOnePage()
        {
            InitializeComponent();

            BitmapImage bitim = new BitmapImage();
            bitim.BeginInit();
            bitim.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-1-0.bmp");
            bitim.EndInit();
            image1.Stretch = Stretch.Fill;
            image1.Source = bitim;

            Canvas.SetLeft(image1, actual_x + 27);
            Canvas.SetTop(image1, actual_y + 26);

            Canvas.SetLeft(label1, actual_x + 190);
            Canvas.SetTop(label1, actual_y + 527);

            Canvas.SetLeft(border1, actual_x + 59);
            Canvas.SetTop(border1, actual_y + 306);

            BitmapImage bitim2 = new BitmapImage();
            bitim2.BeginInit();
            bitim2.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-1.bmp");
            bitim2.EndInit();
            image2.Stretch = Stretch.Fill;
            image2.Source = bitim2;

            Canvas.SetLeft(image2, actual_x + 76);
            Canvas.SetTop(image2, actual_y + 356);

            BitmapImage bitim3 = new BitmapImage();
            bitim3.BeginInit();
            bitim3.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-2.bmp");
            bitim3.EndInit();
            image3.Stretch = Stretch.Fill;
            image3.Source = bitim3;

            Canvas.SetLeft(image3, actual_x + 210);
            Canvas.SetTop(image3, actual_y + 356);

            BitmapImage bitim4 = new BitmapImage();
            bitim4.BeginInit();
            bitim4.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-3.bmp");
            bitim4.EndInit();
            image4.Stretch = Stretch.Fill;
            image4.Source = bitim4;

            Canvas.SetLeft(image4, actual_x + 349);
            Canvas.SetTop(image4, actual_y + 356);

            BitmapImage bitim5 = new BitmapImage();
            bitim5.BeginInit();
            bitim5.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-3.bmp");
            bitim5.EndInit();
            image5.Stretch = Stretch.Fill;
            image5.Source = bitim5;

            Canvas.SetLeft(image5, actual_x + 487);
            Canvas.SetTop(image5, actual_y + 356);

            BitmapImage bitim6 = new BitmapImage();
            bitim6.BeginInit();
            bitim6.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer-3.bmp");
            bitim6.EndInit();
            image6.Stretch = Stretch.Fill;
            image6.Source = bitim6;

            Canvas.SetLeft(image6, actual_x + 626);
            Canvas.SetTop(image6, actual_y + 356);

            Canvas.SetLeft(border2, actual_x + 64);
            Canvas.SetTop(border2, actual_y + 342);

            Canvas.SetLeft(border3, actual_x + 199);
            Canvas.SetTop(border3, actual_y + 342);

            Canvas.SetLeft(border4, actual_x + 336);
            Canvas.SetTop(border4, actual_y + 342);

            Canvas.SetLeft(border5, actual_x + 474);
            Canvas.SetTop(border5, actual_y + 342);

            Canvas.SetLeft(border6, actual_x + 612);
            Canvas.SetTop(border6, actual_y + 342);

        }

      
        private void image2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Canvas.IsEnabled = false;
                border2.Visibility = Visibility.Visible;
                border_control = true;
                label1.Foreground = new SolidColorBrush(Color.FromRgb(0,255, 0));
                label1.Content = "正  确";
               
            }
        }

        private void image3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                border3.Visibility = Visibility.Visible;
                Canvas.IsEnabled = false;
                border_control = true;
                label1.Foreground = Brushes.Red;
                label1.Content = "错误，正确答案：1";
            }
        }

        private void image4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                border4.Visibility = Visibility.Visible;
                Canvas.IsEnabled = false;
                border_control = true;
                label1.Foreground = Brushes.Red;
                label1.Content = "错误，正确答案：1";
            }
        }

        private void image5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
               border5.Visibility = Visibility.Visible;
               Canvas.IsEnabled = false;
               border_control = true;
               label1.Foreground = Brushes.Red;
               label1.Content = "错误，正确答案：1";
            }
        }

        private void image6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
               border6.Visibility = Visibility.Visible;
               Canvas.IsEnabled = false;
               border_control = true;
               label1.Foreground = Brushes.Red;
               label1.Content = "错误，正确答案：1";
            }
        }

        private void image2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!border_control)
            border2.Visibility = Visibility.Hidden;
        }

        private void image2_MouseEnter(object sender, MouseEventArgs e)
        {
            border2.Visibility = Visibility.Visible;
        }

        private void image3_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!border_control)
            border3.Visibility = Visibility.Hidden;
        }

        private void image3_MouseEnter(object sender, MouseEventArgs e)
        {
            border3.Visibility = Visibility.Visible;
        }

        private void image4_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!border_control)
            border4.Visibility = Visibility.Hidden;
        }

        private void image4_MouseEnter(object sender, MouseEventArgs e)
        {
            border4.Visibility = Visibility.Visible;
        }

        private void image5_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!border_control)
            border5.Visibility = Visibility.Hidden;
        }

        private void image5_MouseEnter(object sender, MouseEventArgs e)
        {
           
            border5.Visibility = Visibility.Visible;
        }

        private void image6_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!border_control)
            border6.Visibility = Visibility.Hidden;
        }

        private void image6_MouseEnter(object sender, MouseEventArgs e)
        {
            border6.Visibility = Visibility.Visible;
        }

    

      
      
    }//类
}//命名空间

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
            Canvas.SetLeft(image1, actual_x + 27);
            Canvas.SetTop(image1, actual_y + 26);

            Canvas.SetLeft(label1, actual_x + 190);
            Canvas.SetTop(label1, actual_y + 527);

            Canvas.SetLeft(border1, actual_x + 59);
            Canvas.SetTop(border1, actual_y + 306);

            Canvas.SetLeft(image2, actual_x + 76);
            Canvas.SetTop(image2, actual_y + 356);

            Canvas.SetLeft(image3, actual_x + 210);
            Canvas.SetTop(image3, actual_y + 356);

            Canvas.SetLeft(image4, actual_x + 349);
            Canvas.SetTop(image4, actual_y + 356);

            Canvas.SetLeft(image5, actual_x + 487);
            Canvas.SetTop(image5, actual_y + 356);

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
                label1.Content = "恭喜你，答对了！";
               
            }
        }

        private void image3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                border3.Visibility = Visibility.Visible;
                Canvas.IsEnabled = false;
                border_control = true;
                label1.Content = "选择错误，正确答案第一个";
            }
        }

        private void image4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                border4.Visibility = Visibility.Visible;
                Canvas.IsEnabled = false;
                border_control = true;
                label1.Content = "选择错误，正确答案第一个";
            }
        }

        private void image5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
               border5.Visibility = Visibility.Visible;
               Canvas.IsEnabled = false;
               border_control = true;
               label1.Content = "选择错误，正确答案第一个";
            }
        }

        private void image6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
               border6.Visibility = Visibility.Visible;
               Canvas.IsEnabled = false;
               border_control = true;
               label1.Content = "选择错误，正确答案第一个";
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

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
    /// ExerciseTwoPage_Ins.xaml 的互動邏輯
    /// </summary>
    public partial class ExerciseTwoPage_Ins : UserControl
    {
        //private int actual_x = FEITStandard.PAGE_BEG_X - 127;
        //private int actual_y = FEITStandard.PAGE_BEG_Y;

        public ExerciseTwoPage_Ins()
        {
            InitializeComponent();

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer2_ins.bmp");
            bi.EndInit();
            image1.Source = bi; 

            Canvas.SetLeft(image1, 0);// actual_x + 12
            Canvas.SetTop(image1,0);// actual_y + 26
        }
    }
}

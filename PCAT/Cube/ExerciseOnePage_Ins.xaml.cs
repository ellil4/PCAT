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
    /// ExerciseOnePage_Ins.xaml 的互動邏輯
    /// </summary>
    public partial class ExerciseOnePage_Ins : UserControl
    {
        //private int actual_x = FEITStandard.PAGE_BEG_X - 127;
        //private int actual_y = FEITStandard.PAGE_BEG_Y;

        public ExerciseOnePage_Ins()
        {
            InitializeComponent();

            BitmapImage bit = new BitmapImage();
            bit.BeginInit();
            bit.UriSource = new Uri(FEITStandard.GetExePath() + "Cube\\CubeRes\\cubinsExer1_ins.bmp");
            bit.EndInit();
            image_ins1.Source = bit; 

            Canvas.SetLeft(image_ins1, 0);
            Canvas.SetTop(image_ins1, 0);
        }
    }
}

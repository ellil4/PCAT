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
    /// PaperInsUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class PaperInsUserControl : UserControl
    {
        private int actual_x = FEITStandard.PAGE_BEG_X ;
        private int actual_y = 0 ;
       
        public PaperInsUserControl()
        {
            InitializeComponent();
            Canvas.SetLeft(image1, 0);
            Canvas.SetTop(image1, actual_y + 26);
            BitmapImage bitim = new BitmapImage();
            bitim.BeginInit();
            bitim.UriSource = new Uri(FEITStandard.GetExePath() + "Paper\\PaperRes\\Example\\paperins.jpg");
            bitim.EndInit();
            image1.Stretch = Stretch.Fill;
            image1.Source = bitim;
        }
    }
}

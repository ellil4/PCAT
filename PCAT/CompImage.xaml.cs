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
using System.Drawing;

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompImage.xaml 的互動邏輯
    /// </summary>
    public partial class CompImage : UserControl
    {
        public bool isSelected = false;
        public static int SIZE = 166;
        public int mId = -1;

        public CompImage(int id)
        {
            InitializeComponent();
            BorderVisiable(false);
            mId = id;
        }

        public void BorderVisiable(bool visiable)
        {
            if (visiable)
            {
                amBorder.Visibility = Visibility.Visible;
            }
            else
            {
                amBorder.Visibility = Visibility.Hidden;
            }
        }

        public void SetGraph(Bitmap src)
        {
            amImage.Source = BitmapSourceFactory.GetBitmapSource(src);
        }
    }
}

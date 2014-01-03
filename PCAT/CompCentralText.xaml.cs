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
using FiveElementsIntTest.OpSpan;
using System.Threading;

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompMathStimu.xaml 的互動邏輯
    /// </summary>
    public partial class CompCentralText : UserControl
    {
        public int OUTWIDTH = 796;
        public int OUTHEIGHT = 100;

        public CompCentralText()
        {
            InitializeComponent();
            mText.IsReadOnly = true;
        }

        private void setText(String text, String fontFamily, double size, Color color)
        {
            mText.FontFamily = new FontFamily(fontFamily);
            mText.Text = text;
            mText.FontSize = size;
            mText.Foreground = new SolidColorBrush(color);
        }

        public void PutTextToCentralScreen(String content, String fontFamily, 
            double size, ref Canvas canvas, int yOff, Color color)
        {
            setText(content, fontFamily, size, color);
            canvas.Children.Add(this);

            Canvas.SetLeft(this,
                FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - OUTWIDTH) / 2);

            Canvas.SetTop(this,
                FEITStandard.PAGE_BEG_Y + FEITStandard.PAGE_HEIGHT / 2
                - OUTHEIGHT / 2 + yOff);
        }
    }
}

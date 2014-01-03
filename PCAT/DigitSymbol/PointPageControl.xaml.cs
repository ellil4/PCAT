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

namespace FiveElementsIntTest.DigitSymbol
{
    /// <summary>
    /// PointPageControl.xaml 的交互逻辑
    /// </summary>
    public partial class PointPageControl : UserControl
    {
        protected LayoutInstruction _LayoutInstruction;

        public PointPageControl()
        {
            InitializeComponent();

            Digitpointone.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            Digitpointone.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;


            _LayoutInstruction = new LayoutInstruction(ref Digitpointone);

        }
        private void layoutPointOut()
        {
            _LayoutInstruction.addInstruction(200, 0, FEITStandard.PAGE_WIDTH / 100 * 98, 100,
                "    请在保证正确的前提下尽快按键,/n/r"+"中途按错键不用修改，继续往下做。", "Kaiti", 35, Color.FromRgb(255, 255, 255));
            _LayoutInstruction.addTitle(300, 0, "(空格键继续）", "KaiTi", 48, Color.FromRgb(255, 255, 255));
        }









    }//class
}//namespace

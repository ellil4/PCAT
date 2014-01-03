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
    /// DigiPointPageOneControl.xaml 的交互逻辑
    /// </summary>
    public partial class DigiPointPageOneControl : UserControl
    {
        protected LayoutInstruction _LayoutInstruction;

        public DigiPointPageOneControl()
        {
            InitializeComponent();
            Digitpointone.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            Digitpointone.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;


            _LayoutInstruction = new LayoutInstruction(ref Digitpointone);

            layoutPointOut();

        }
        private void layoutPointOut()
        {
            _LayoutInstruction.addInstruction(250, 80, FEITStandard.PAGE_WIDTH / 100 * 98, 200,
                "             请又好又快地完成任务,\n\r" + "   如果按错键，不能修改，继续往下做。", "Kaiti", 35, Color.FromRgb(255, 255, 255));
            _LayoutInstruction.addTitle(500, 20, "(空格键继续）", "KaiTi", 36, Color.FromRgb(255, 255, 255));
        }




    }//class
}

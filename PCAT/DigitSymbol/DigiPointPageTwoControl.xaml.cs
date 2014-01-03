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
    /// DigiPointPageTwoControl.xaml 的交互逻辑
    /// </summary>
    public partial class DigiPointPageTwoControl : UserControl
    {
        protected LayoutInstruction _LayoutInstruction;

        public DigiPointPageTwoControl()
        {
            InitializeComponent();
            Digitpointtwo.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            Digitpointtwo.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;


            _LayoutInstruction = new LayoutInstruction(ref Digitpointtwo);
            layoutPointOut();
        }
        private void layoutPointOut()
        {
            _LayoutInstruction.addInstruction(250, 80, FEITStandard.PAGE_WIDTH / 100 * 98, 300,
                "        请将手放在" + " \"√\"" + ", " + "\"X\"" + ", " + "\"—\"" + " 键上,\n\r" + "", "Kaiti", 35, Color.FromRgb(255, 255, 255));
            _LayoutInstruction.addTitle(450, 20, "（按空格键开始正式测验）", "KaiTi", 36, Color.FromRgb(255, 255, 255));
        }

    }
}

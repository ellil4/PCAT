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

namespace FiveElementsIntTest.CtSpan
{
    /// <summary>
    /// CompResultReport.xaml 的互動邏輯
    /// </summary>
    public partial class CompResultReport : UserControl
    {
        public CompResultReport(int totalCount, int correctCount)
        {
            InitializeComponent();

            amTextBlock1.Text = "这组题（共" + totalCount + "道）中，您";
            amTextBlock2.Text = "记对了" + correctCount + "道";
        }
    }
}

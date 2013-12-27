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

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// BoardSymmInst1.xaml 的互動邏輯
    /// </summary>
    public partial class BoardSymmInst0 : UserControl
    {
        BasePage mBasePage;

        public BoardSymmInst0(BasePage bp)
        {
            InitializeComponent();
            mBasePage = bp;
        }

        private void amNextBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mBasePage.ShowSymmInst1();
        }
    }
}

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
    /// BoardInstructionComprehPrac.xaml 的互動邏輯
    /// </summary>
    public partial class BoardInstructionComprehPrac : UserControl
    {
        BasePage mBasePage;
        public BoardInstructionComprehPrac(BasePage bp)
        {
            InitializeComponent();
            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                label2.Content = "请在做对称判断的同时";
                label3.Content = "记住随后出现的红点位置";
            }
            mBasePage = bp;
        }

        private void amStartBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mBasePage.ShowGroupTitle();
        }
    }
}

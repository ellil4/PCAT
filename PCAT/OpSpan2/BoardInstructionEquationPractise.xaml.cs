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
    /// BoardInstructionEquationPractise.xaml 的互動邏輯
    /// </summary>
    public partial class BoardInstructionEquationPractise : UserControl
    {
        public BasePage mBasePage;
        public BoardInstructionEquationPractise(BasePage bp)
        {
            InitializeComponent();
            mBasePage = bp;

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                label1.Content = "下面开始练习判断图形是否对称";
                label2.Content = "得出答案后请尽快点击";
                label2.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                label4.Content = "然后判断给出的图形是否对称";
                label4.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                //label3.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void label5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //go to equation exe
            mBasePage.ShowEquationPage();
        }
    }
}

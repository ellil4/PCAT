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
    public partial class BoardSymmInst2 : UserControl
    {
        BasePage mBasePage;

        public BoardSymmInst2(BasePage bp)
        {
            InitializeComponent();
            mBasePage = bp;
            amBtnNext.MouseUp += new MouseButtonEventHandler(amBtnNext_MouseUp);
            amBtnPrev.MouseUp += new MouseButtonEventHandler(amBtnPrev_MouseUp);
        }

        void amBtnPrev_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mBasePage.ShowSymmInst1();            
        }

        void amBtnNext_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BoardInstructionEquationPractise biep =
                new BoardInstructionEquationPractise(mBasePage);

            biep.label3.Content = "看好了";
            mBasePage.ShowBoard(biep);
        }
    }
}

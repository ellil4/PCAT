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
    /// BoardEquation.xaml 的互動邏輯
    /// </summary>
    public partial class BoardEquation : UserControl
    {
        public BasePage mBasePage;
        public BoardEquation(BasePage bp)
        {
            InitializeComponent();
            mBasePage = bp;

            switch (mBasePage.mStage)
            {
                case Stage.EquationPrac:
                    amEquation.Content = mBasePage.mEquationPrac[mBasePage.mCurInGrpAt];
                    break;
                case Stage.ComprehPrac:
                    break;
                case Stage.Formal:
                    break;
            }

            amEquation.Content += "?";
            
        }

        private void amOKBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //show judge page
            mBasePage.SHowEquationJudgePage(null);
        }
    }
}

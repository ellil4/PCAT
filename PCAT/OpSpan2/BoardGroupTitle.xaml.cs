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
using System.Timers;

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// BoardGroupTitle.xaml 的互動邏輯
    /// </summary>
    public partial class BoardGroupTitle : UserControl
    {
        public BasePage mBasePage;

        public BoardGroupTitle(BasePage bp)
        {
            InitializeComponent();
            mBasePage = bp;
        }

        public void Run()
        {
            switch(mBasePage.mStage)
            {
                case Stage.ComprehPrac:
                    amLabNum.Content = "[" + BasePage.mPracScheme[mBasePage.mCurSchemeAt] + 
                        "-" + (mBasePage.GetGroupAtInSpan(mBasePage.mCurSchemeAt, BasePage.mPracScheme) + 1) + "]";
                    break;
                case Stage.Formal:
                    amLabNum.Content = "[" + BasePage.mTestScheme[mBasePage.mCurSchemeAt] +
                        "-" + (mBasePage.GetGroupAtInSpan(mBasePage.mCurSchemeAt, BasePage.mTestScheme) + 1) + "]";
                    break;
            }       
     
            Timer tCP = new Timer();
            tCP.Interval = 1500;
            tCP.AutoReset = false;
            tCP.Elapsed += new ElapsedEventHandler(tCP_Elapsed);
            tCP.Enabled = true;

        }

        delegate void TimeDele();
        delegate void TimeDele2(object obj);

        //blank mask
        void tCP_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ClearAll));
            Timer tOut = new Timer();
            tOut.Interval = 500;
            tOut.AutoReset = false;
            tOut.Elapsed += new ElapsedEventHandler(tOut_Elapsed);
            tOut.Enabled = true;
        }

        void tOut_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowEquationPage));
        }
    }
}

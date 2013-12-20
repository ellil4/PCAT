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
using System.Windows.Threading;

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// BoardAnimal.xaml 的互動邏輯
    /// </summary>
    public partial class BoardAnimal : UserControl
    {
        string mContent;
        BasePage mBasePage;

        public delegate void TimeDele();
        public delegate void TimeDeleP(object obj = null);

        public BoardAnimal(BasePage page)
        {
            InitializeComponent();
            mBasePage = page;
            //mContent = content;
            switch (mBasePage.mStage)
            {
                case Stage.AnimalPrac:
                    mContent =
                        mBasePage.mAnimalPrac[mBasePage.mCurSchemeAt][mBasePage.mCurInGrpAt];
                    break;
                case Stage.ComprehPrac:
                    mContent =
                        mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget;
                    break;
                case Stage.Formal:
                    mContent =
                        mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget;
                    break;
            }
            label1.Content = mContent;
        }

        public void Run()
        {
            Timer t = new Timer();
            t.Interval = 1000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(nexBlankMask));
        }

        void nexBlankMask()
        {
            mBasePage.ClearAll();
            Timer tb = new Timer();
            tb.Interval = 250;
            tb.AutoReset = false;
            tb.Elapsed += new ElapsedEventHandler(tb_Elapsed_go_out);
            tb.Enabled = true;
        }

        void tb_Elapsed_go_out(object sender, ElapsedEventArgs e)
        {
            if(mBasePage.mStage == Stage.ComprehPrac || mBasePage.mStage == Stage.Formal)
                mBasePage.mRecorder.inGroupNum.Add(mBasePage.mCurInGrpAt);

            mBasePage.DoCursorIteration();
            
            switch (mBasePage.mStage)
            {
                case Stage.AnimalPrac:
                    if (!mBasePage.SchemeIterated())//
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowBoardAnimal));
                    }
                    else//Scheme Iterated
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowOrderSelectPage));
                    }
                    break;
                case Stage.ComprehPrac:

                    if (!mBasePage.SchemeIterated())//
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowEquationPage));
                    }
                    else//Scheme Iterated
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowOrderSelectPage));
                    }
                    break;
                case Stage.Formal:

                    if (!mBasePage.SchemeIterated())//
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowEquationPage));
                    }
                    else//Scheme Iterated
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowOrderSelectPage));
                    }
                    break;
            }
        }
    }
}

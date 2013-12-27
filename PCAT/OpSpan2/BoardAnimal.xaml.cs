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
        BoardSubChess mChess;

        public delegate void TimeDele();
        public delegate void TimeDeleP(object obj = null);

        public BoardAnimal(BasePage page)
        {
            InitializeComponent();
            mBasePage = page;

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                label1.Visibility = System.Windows.Visibility.Visible;
            }
            else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                label1.Visibility = System.Windows.Visibility.Hidden;

                mChess = new BoardSubChess();
                amGrid.Children.Add(mChess);
                mChess.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                mChess.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                mChess.mEditable = false;
            }

            switch (mBasePage.mStage)
            {
                case Stage.MemPrac:
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        mContent =
                            mBasePage.mMemPrac[mBasePage.mCurSchemeAt][mBasePage.mCurInGrpAt];
                        label1.Content = mContent;
                    }
                    else
                    {
                        mChess.ShowDot(
                            Int32.Parse(
                            mBasePage.mMemPrac[mBasePage.mCurSchemeAt][mBasePage.mCurInGrpAt]));
                    }
                    break;
                case Stage.ComprehPrac:
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        mContent =
                            mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget;
                        label1.Content = mContent;
                    }
                    else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    {
                        mChess.ShowDot(
                           Int32.Parse(
                           mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget));
                    }
                    break;
                case Stage.Formal:
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        mContent =
                            mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget;
                        label1.Content = mContent;
                    }
                    else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    {
                        mChess.ShowDot(
                          Int32.Parse(
                          mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget));
                    }
                    break;
            }
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

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                mChess.ClearAllShown();
                amGrid.Children.Remove(mChess);
                mBasePage.amBaseCanvas.Children.Add(mChess);
                Canvas.SetTop(mChess, FEITStandard.PAGE_BEG_Y + ((FEITStandard.PAGE_HEIGHT - 268) / 2));
                Canvas.SetLeft(mChess, FEITStandard.PAGE_BEG_X + ((FEITStandard.PAGE_WIDTH - 268) / 2));
            }

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
                case Stage.MemPrac:
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

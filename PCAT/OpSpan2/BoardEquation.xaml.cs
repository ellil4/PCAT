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
using System.Drawing;

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// BoardEquation.xaml 的互動邏輯
    /// </summary>
    public partial class BoardEquation : UserControl
    {
        public BasePage mBasePage;
        public Timer mLimit;
        IntPtr mpPic = IntPtr.Zero;
        public BoardEquation(BasePage bp)
        {
            InitializeComponent();

            amLabelOvertime.Visibility = System.Windows.Visibility.Hidden;

            mBasePage = bp;

            mLimit = new Timer();
            mLimit.AutoReset = false;
            mLimit.Interval = mBasePage.mInterTimeLimit;
            mLimit.Elapsed += new ElapsedEventHandler(mLimit_Elapsed);

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                amOKBtn.Content = "看好了";
                amEquation.Visibility = System.Windows.Visibility.Hidden;

                amLabelOvertime.Margin = new Thickness(432, 586, 432, 126);
                amOKBtn.Margin = new Thickness(0, 400, 0, 0);
            }

            switch (mBasePage.mStage)
            {
                case Stage.InterPrac:
                    //show
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        amEquation.Content = mBasePage.mInterPrac[mBasePage.mCurInGrpAt].Equation;
                    }
                    else
                    {
                        Bitmap bmp = new Bitmap(
                            FEITStandard.GetExePath() + "SYMM\\" + 
                            mBasePage.mInterPrac[mBasePage.mCurInGrpAt].Equation + ".bmp");

                        amImage.Source = BitmapSourceFactory.GetBitmapSource(bmp, out mpPic);
                    }
                    //rec
                    mBasePage.mRecorder.mathPracEquations.Add(mBasePage.mInterPrac[mBasePage.mCurInGrpAt]);
                    //show 
                    break;
                case Stage.ComprehPrac:
                    //show
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        amEquation.Content =
                            mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equation;
                    }
                    else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    {
                        Bitmap bmp = new Bitmap(
                           FEITStandard.GetExePath() + "SYMM\\" +
                           mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equation + 
                           ".bmp");

                        amImage.Source = BitmapSourceFactory.GetBitmapSource(bmp, out mpPic);
                    }
                    //limit
                    mLimit.Enabled = true;
                    //rec
                    mBasePage.mRecorder.mathExpression.Add(
                        mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equation);
                    mBasePage.mRecorder.isPractise.Add(true);
                    mBasePage.mRecorder.isExtra.Add(mBasePage.mSecondComprehPrac);
                    mBasePage.mRecorder.displayedAnswer.Add(
                        mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].result);
                    mBasePage.mRecorder.animal.Add(
                        mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget);
                    mBasePage.mRecorder.correctness.Add(
                        mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].correctness);
                    mBasePage.mRecorder.equaLv.Add(
                        mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equationLevel);
                    break;
                case Stage.Formal:
                    //show
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        amEquation.Content =
                            mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equation;
                    }
                    else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    {
                        Bitmap bmp = new Bitmap(
                          FEITStandard.GetExePath() + "SYMM\\" +
                          mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equation +
                          ".bmp");

                        amImage.Source = BitmapSourceFactory.GetBitmapSource(bmp, out mpPic);
                    }
                    //limit
                    mLimit.Enabled = true;
                    //rec
                    mBasePage.mRecorder.mathExpression.Add(
                        mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equation);
                    mBasePage.mRecorder.isPractise.Add(false);
                    mBasePage.mRecorder.isExtra.Add(mBasePage.mSecondFormal);
                    mBasePage.mRecorder.displayedAnswer.Add(
                        mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].result);
                    mBasePage.mRecorder.animal.Add(
                        mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].memTarget);
                    mBasePage.mRecorder.correctness.Add(
                        mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].correctness);
                    mBasePage.mRecorder.equaLv.Add(
                        mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].equationLevel);
                    break;
            }

            amEquation.Content += "?";
            

        }

        void switch2OvertimeUI()
        {
            amOKBtn.Visibility = System.Windows.Visibility.Hidden;
            amLabelOvertime.Visibility = System.Windows.Visibility.Visible;
        }

        void mLimit_Elapsed(object sender, ElapsedEventArgs e)
        {
            //go animal
            mBasePage.Dispatcher.Invoke(new TimeDele(switch2OvertimeUI));

            Timer timerGoOut = new Timer();
            timerGoOut.Interval = 500;
            timerGoOut.AutoReset = false;
            timerGoOut.Elapsed += new ElapsedEventHandler(timerGoOut_Elapsed);
            timerGoOut.Enabled = true;
            mBasePage.mRecorder.isOvertime.Add(true);
        }

        void timerGoOut_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(goOut));
        }

        void goOut()
        {
            long offTime = mBasePage.mTimeline.ElapsedMilliseconds;
            mLimit.Enabled = false;
            switch (mBasePage.mStage)
            {
                case Stage.InterPrac:
                    mBasePage.mRecorder.mathPracOff.Add(offTime);
                    mBasePage.mRecorder.mathPracRTs.Add(
                        offTime - mBasePage.mRecorder.mathPracOn[
                        mBasePage.mRecorder.mathPracOn.Count - 1]);

                    break;
                case Stage.ComprehPrac:
                    mBasePage.mRecorder.mathOff.Add(offTime);
                    mBasePage.mRecorder.mathDure.Add(
                        offTime - mBasePage.mRecorder.mathOn[
                        mBasePage.mRecorder.mathOn.Count - 1]);
                    break;
                case Stage.Formal:
                    mBasePage.mRecorder.mathOff.Add(offTime);
                    mBasePage.mRecorder.mathDure.Add(
                        offTime - mBasePage.mRecorder.mathOn[
                        mBasePage.mRecorder.mathOn.Count - 1]);
                    break;
            }
            //blank mask and
            //show judge page
            mBasePage.ClearAll();
            Timer t = new Timer();
            t.Interval = 250;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        private void amOKBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mBasePage.mStage == Stage.ComprehPrac || mBasePage.mStage == Stage.Formal)
            {
                mBasePage.mRecorder.isOvertime.Add(false);
            }
            goOut();
        }

        delegate void TimeDele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowEquationJudgePage));
            BitmapSourceFactory.DeleteObject(mpPic);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (mBasePage.mStage)
            {
                case Stage.InterPrac:
                    mBasePage.mRecorder.mathPracOn.Add(
                        mBasePage.mTimeline.ElapsedMilliseconds);
                    break;
                case Stage.ComprehPrac:
                    mBasePage.mRecorder.mathOn.Add(
                        mBasePage.mTimeline.ElapsedMilliseconds);

                    mBasePage.mRecorder.spanWidth.Add(
                        BasePage.mPracScheme[mBasePage.mCurSchemeAt]);

                    mBasePage.mRecorder.groupNum.Add(
                        mBasePage.GetGroupAtInSpan(
                        mBasePage.mCurSchemeAt, BasePage.mPracScheme));
                    break;
                case Stage.Formal:
                    mBasePage.mRecorder.mathOn.Add(
                        mBasePage.mTimeline.ElapsedMilliseconds);

                    mBasePage.mRecorder.spanWidth.Add(
                        BasePage.mTestScheme[mBasePage.mCurSchemeAt]);

                    mBasePage.mRecorder.groupNum.Add(
                        mBasePage.GetGroupAtInSpan(
                        mBasePage.mCurSchemeAt, BasePage.mTestScheme));
                    break;
            }
        }

    }
}

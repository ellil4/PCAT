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
    /// BoardEquationJudge.xaml 的互動邏輯
    /// </summary>
    public partial class BoardEquationJudge : UserControl
    {
        public BasePage mBasePage;
        bool mOriginalAns;
        //bool mIsPractise = false;

        public BoardEquationJudge(BasePage bp)
        {
            InitializeComponent();

            mBasePage = bp;

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                amTrueBtn.Content = "是";
                amFalseBtn.Content = "否";

                amTrueBtn.Margin = new Thickness(300, 510, 0, 0);
                amFalseBtn.Margin = new Thickness(614, 510, 0, 0);
                amCorrectness.Margin = new Thickness(430, 508, 434, 304);
            }

            switch (mBasePage.mStage)
            {
                case Stage.InterPrac:
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        amAnswerShow.Content = mBasePage.mInterPrac[mBasePage.mCurInGrpAt].Result;
                    }
                    else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    {
                        amAnswerShow.Content = "是否对称?";
                    }
                    //get ans
                    mOriginalAns = mBasePage.mInterPrac[mBasePage.mCurInGrpAt].Answer;

                    break;
                case Stage.ComprehPrac:
                    //show
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        amAnswerShow.Content =
                            mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].result;
                    }
                    else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    {
                        amAnswerShow.Content = "是否对称?";
                    }
                    //get ans
                    mOriginalAns =
                        mBasePage.mComprehPrac[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].correctness;
                    break;
                case Stage.Formal:
                    //show
                    if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                    {
                        amAnswerShow.Content =
                            mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].result;
                    }
                    else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    {
                        amAnswerShow.Content = "是否对称?";
                    }
                    //get ans
                    mOriginalAns =
                        mBasePage.mTest[mBasePage.mCurSchemeAt].mTrails[mBasePage.mCurInGrpAt].correctness;
                    break;
            }

            amCorrectness.Visibility = System.Windows.Visibility.Hidden;
        }

        private void goCorrect()
        {
            /*if (mBasePage.mStage == Stage.InterPrac)
            {
                amCorrectness.Content = "正确";
                amCorrectness.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                amCorrectness.Visibility = System.Windows.Visibility.Visible;
            }*/


            /*Timer t_correct = new Timer();
            t_correct.Interval = 500;
            t_correct.AutoReset = false;
            t_correct.Elapsed += new ElapsedEventHandler(t_afterShowCorrectness_Elapsed);
            t_correct.Enabled = true;*/
            t_afterShowCorrectness_Elapsed(null, null);
        }

        delegate void TimeDele();
        delegate void TimeDele2(object obj = null);

        private void showWrong()
        {
            amCorrectness.Visibility = System.Windows.Visibility.Visible;

            Timer t_wrong = new Timer();
            t_wrong.Interval = 500;
            t_wrong.AutoReset = false;
            t_wrong.Elapsed += new ElapsedEventHandler(t_afterShowCorrectness_Elapsed);
            t_wrong.Enabled = true;
        }

        void t_afterShowCorrectness_Elapsed(object sender, ElapsedEventArgs e)
        {
            //blank mask
            mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ClearAll));
            Timer t_mask = new Timer();
            t_mask.Interval = 500;
            t_mask.AutoReset = false;
            t_mask.Elapsed += new ElapsedEventHandler(t_mask_Elapsed);
            t_mask.Enabled = true;
        }

        void t_mask_Elapsed(object sender, ElapsedEventArgs e)
        {
            switch (mBasePage.mStage)
            {
                case Stage.InterPrac:
                    //iter & go out
                    mBasePage.DoCursorIteration();
                    if (mBasePage.mCurInGrpAt != mBasePage.mInterPrac.Count)
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowEquationPage));
                    }
                    else
                    {
                        //go to compreh practise
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowInstructionComprehPrac));
                    }
                    break;
                case Stage.ComprehPrac:
                    if (mBasePage.SchemeReturned())//end of span && end of scheme
                    {
                        //go to order UI
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowOrderSelectPage));
                    }
                    else
                    {
                        //go to next animal
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowBoardAnimal));
                    }
                    break;
                case Stage.Formal:
                    if (mBasePage.SchemeReturned() || mBasePage.SchemeIterated())//end of span || end of scheme
                    {
                        //go to order UI
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowOrderSelectPage));
                    }
                    else
                    {
                        //go to next animal
                        mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowBoardAnimal));
                    }
                    break;
            }
        }

        void addChoiceMadeAndDureTime(long pressTime)
        {
            mBasePage.mRecorder.choiceMadeTime.Add(pressTime);

            mBasePage.mRecorder.choiceDure.Add(
                pressTime - mBasePage.mRecorder.choiceShowTime[
                mBasePage.mRecorder.choiceShowTime.Count - 1]);
        }

        private void amTrueBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            long pressTime = mBasePage.mTimeline.ElapsedMilliseconds;
            switch (mBasePage.mStage)
            {
                case Stage.InterPrac:
                    mBasePage.mRecorder.mathPracAnswers.Add("True");
                    break;
                case Stage.ComprehPrac:
                    addChoiceMadeAndDureTime(pressTime);
                    mBasePage.mRecorder.choice.Add("True");
                    break;
                case Stage.Formal:
                    addChoiceMadeAndDureTime(pressTime);
                    mBasePage.mRecorder.choice.Add("True");
                    break;
            }
            
            if (mOriginalAns)
            {
                goCorrect();
            }
            else
            {
                showWrong();
            }
        }

        private void amFalseBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            long pressTime = mBasePage.mTimeline.ElapsedMilliseconds;
            switch (mBasePage.mStage)
            {
                case Stage.InterPrac:
                    mBasePage.mRecorder.mathPracAnswers.Add("False");
                    break;
                case Stage.ComprehPrac:
                    addChoiceMadeAndDureTime(pressTime);
                    mBasePage.mRecorder.choice.Add("False");
                    break;
                case Stage.Formal:
                    addChoiceMadeAndDureTime(pressTime);
                    mBasePage.mRecorder.choice.Add("False");
                    break;
            }
            
            if (mOriginalAns)
            {
                showWrong();
            }
            else
            {
                goCorrect();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (mBasePage.mStage == Stage.Formal || mBasePage.mStage == Stage.ComprehPrac)
            {
                mBasePage.mRecorder.choiceShowTime.Add(
                    mBasePage.mTimeline.ElapsedMilliseconds);
            }
        }
    }
}

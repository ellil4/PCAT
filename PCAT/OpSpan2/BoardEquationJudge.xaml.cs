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
        public StEquation mEquation;
        bool mOriginalAns;

        public BoardEquationJudge(BasePage bp)
        {
            InitializeComponent();

            mBasePage = bp;

            switch(mBasePage.mStage)
            {
                case Stage.EquationPrac:
                    //rec
                    mBasePage.mRecorder.mMathPracEquations.Add(mBasePage.mEquationPrac[mBasePage.mCurInGrpAt]);
                    //show 
                    amAnswerShow.Content = mBasePage.mEquationPrac[mBasePage.mCurInGrpAt].Result;
                    //get ans
                    mOriginalAns = mBasePage.mEquationPrac[mBasePage.mCurInGrpAt].Answer;
                    break;
            }

            amCorrectness.Visibility = System.Windows.Visibility.Hidden;
        }

        private void showCorrect()
        {
            amCorrectness.Content = "正确";
            amCorrectness.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            amCorrectness.Visibility = System.Windows.Visibility.Visible;

            Timer t_correct = new Timer();
            t_correct.Interval = 500;
            t_correct.AutoReset = false;
            t_correct.Elapsed += new ElapsedEventHandler(t_afterShowCorrectness_Elapsed);
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
        }

        void t_afterShowCorrectness_Elapsed(object sender, ElapsedEventArgs e)
        {
            //blank mask
            mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ClearAll));
            Timer t_mask = new Timer();
            t_mask.Interval = 500;
            t_mask.AutoReset = false;
            t_mask.Elapsed += new ElapsedEventHandler(t_mask_Elapsed);
        }

        void t_mask_Elapsed(object sender, ElapsedEventArgs e)
        {
            //iter & go out
            mBasePage.DoCursorIteration();
            switch (mBasePage.mStage)
            {
                case Stage.EquationPrac:
                    if (mBasePage.mCurInGrpAt != mBasePage.mEquationPrac.Count)
                    {
                        mBasePage.Dispatcher.Invoke(new TimeDele2(mBasePage.ShowEquationPage));
                    }
                    else
                    {
                        //go to compreh practise
                    }
                    break;
            }
        }

        private void amTrueBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mBasePage.mRecorder.mMathPracAnswers.Add(true);
            if (mOriginalAns)
            {
                showCorrect();
            }
            else
            {
                showWrong();
            }
        }

        private void amFalseBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mBasePage.mRecorder.mMathPracAnswers.Add(false);
            if (mOriginalAns)
            {
                showWrong();
            }
            else
            {
                showCorrect();
            }
        }
    }
}

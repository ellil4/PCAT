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
    /// BoardOrderOP.xaml 的互動邏輯
    /// </summary>
    public partial class BoardOrderOP : UserControl
    {
        List<Label> mAnimalButtons;
        List<Label> mAnswersBoxes;

        public BasePage mBasePage;
        public bool mIsExtra;
        int mCountDownNum = 45;
        public int mLen;
        public int mCur = 0;
        BoardSubChess mChess;

        public BoardOrderOP(BasePage bp, bool isExtra)
        {
            InitializeComponent();
            mAnimalButtons = new List<Label>();

            mBasePage = bp;
            mIsExtra = isExtra;
            
            amQuesLabel.Visibility = System.Windows.Visibility.Hidden;
            amTBNotice.Visibility = System.Windows.Visibility.Hidden;

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                initAnimal();
                registerAnimal();
            }
            else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                //manipulate
                amAniQues.Visibility = System.Windows.Visibility.Hidden;
                label1.Content = "请按顺序回忆红点出现过的位置";
                rectangle1.Visibility = System.Windows.Visibility.Hidden;

                commonInit();
                for (int i = 0; i < mAnimalButtons.Count; i++)
                {
                    mAnimalButtons[i].Visibility = System.Windows.Visibility.Hidden;
                }
                //add chess
                mChess = new BoardSubChess();
                mChess.mBoardOrder = this;
                mChess.mEditable = true;
                amGrid.Children.Add(mChess);
                mChess.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                mChess.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }
        }

        void commonInit()
        {
            mAnimalButtons.Add(amAni1);
            mAnimalButtons.Add(amAni2);
            mAnimalButtons.Add(amAni3);
            mAnimalButtons.Add(amAni4);
            mAnimalButtons.Add(amAni5);
            mAnimalButtons.Add(amAni6);
            mAnimalButtons.Add(amAni7);
            mAnimalButtons.Add(amAni8);
            mAnimalButtons.Add(amAni9);
            mAnimalButtons.Add(amAni10);
            mAnimalButtons.Add(amAni11);
            mAnimalButtons.Add(amAni12);

            switch (mBasePage.mStage)
            {
                case Stage.MemPrac:
                    mLen = BasePage.mMemPracScheme[getSchemeID2Check()];
                    break;
                case Stage.ComprehPrac:
                    mLen = BasePage.mPracScheme[getSchemeID2Check()];
                    break;
                case Stage.Formal:
                    mLen = BasePage.mTestScheme[getSchemeID2Check()];
                    break;
            }
        }

        //OPSPAN ONLY
        void initAnimal()
        {

            commonInit();
            //amTBAnswer.Text = "";
            //boxes control
            mAnswersBoxes = new List<Label>();

            for (int i = 0; i < mLen; i++)
            {
                Label lb = new Label();
                lb.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                lb.Width = 55;
                lb.Height = 55;
                lb.BorderThickness = new Thickness(2);
                lb.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                amOrderCanvas.Children.Add(lb);
                Canvas.SetTop(lb, 215);
                Canvas.SetLeft(lb, (1024 - mLen * 55) / 2 + i * 53);
                lb.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                lb.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                lb.FontFamily = new FontFamily("SimHei");
                lb.FontSize = 40;
                mAnswersBoxes.Add(lb);
            }
        }

        //OPSPAN ONLY
        bool boxesContains(string tx)
        {
            bool retval = false;
            for (int i = 0; i < mAnswersBoxes.Count; i++)
            {
                if (mAnswersBoxes[i].Content != null && mAnswersBoxes[i].Equals(tx))
                {
                    retval = true;
                    break;
                }
            }

            return retval;
        }

        //OPSPAN ONLY
        void registerAnimal()
        {
            for (int i = 0; i < mAnimalButtons.Count; i++)
            {
                mAnimalButtons[i].MouseUp += new MouseButtonEventHandler(Animal_MouseUp);
            }

            amAniQues.MouseUp += new MouseButtonEventHandler(amAniQues_MouseUp);
        }

        //OPSPAN ONLY
        void amAniQues_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mCur < mLen)
            {
                mAnswersBoxes[mCur].Content = ((Label)sender).Content;
                mCur++;
            }
        }

        //OPSPAN ONLY
        void Animal_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mCur < mLen && !boxesContains(((Label)sender).Content.ToString()))
            {
                ((Label)sender).BorderBrush =
                    new SolidColorBrush(Color.FromRgb(255, 119, 0));
                mAnswersBoxes[mCur].Content = ((Label)sender).Content;
                mCur++;
            }
        }

        private void amBtnClear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mCur > 0)
            {
                if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
                {
                    string tar = mAnswersBoxes[mCur - 1].Content.ToString();

                    for (int i = 0; i < mAnimalButtons.Count; i++)
                    {
                        if (mAnimalButtons[i].Content.Equals(tar))
                        {
                            mAnimalButtons[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        }
                    }

                    mAnswersBoxes[mCur - 1].Content = "";
                }
                else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                {
                    mChess.ClearOne();
                }

                mCur--;
            }
        }

        //OPSPAN ONLY
        private void amAniQues_MouseEnter(object sender, MouseEventArgs e)
        {
            amQuesLabel.Visibility = System.Windows.Visibility.Visible;
        }
        //OPSPAN ONLY
        private void amAniQues_MouseLeave(object sender, MouseEventArgs e)
        {
            amQuesLabel.Visibility = System.Windows.Visibility.Hidden;
        }

        void showRight()
        {
            amTBNotice.Text = "正确";
            amTBNotice.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            amTBNotice.Visibility = System.Windows.Visibility.Visible;
        }

        void showWrong()
        {
            amTBNotice.Visibility = System.Windows.Visibility.Visible;
            switch (mBasePage.mStage)
            {
                case Stage.MemPrac:
                    if (mBasePage.mSecondMemPrac)
                    {
                        amTBNotice.Text = "错误";
                    }
                    else
                    {
                        amTBNotice.Text = "错误，请再看一遍";
                    }
                    break;
                case Stage.ComprehPrac:
                    if (mBasePage.mSecondComprehPrac)
                    {
                        amTBNotice.Text = "错误";
                    }
                    else
                    {
                        amTBNotice.Text = "错误，请再看一遍";
                    }
                    break;
                case Stage.Formal:
                    amTBNotice.Visibility = System.Windows.Visibility.Hidden;
                    break;

            }
        }

        void saveComprehForamlOutTime(long offtime)
        {
            mBasePage.mRecorder.orderOff.Add(offtime);
            mBasePage.mRecorder.orderDure.Add(
                offtime - mBasePage.mRecorder.orderOn[mBasePage.mRecorder.orderOn.Count - 1]);
        }

        private void comprehPracGoOut(string userOrder, long offtime)
        {
            saveComprehForamlOutTime(offtime);
            string realOrder = mBasePage.GetComprehPracAnimalRealOrder();
            mBasePage.mRecorder.rightOrder.Add(realOrder);
            mBasePage.mRecorder.userInputOrder.Add(userOrder);
            mBasePage.mRecorder.isExtraG.Add(mIsExtra);
            mBasePage.mRecorder.isPractiseG.Add(true);
            mBasePage.mExeDidCount++;

            if (realOrder.Equals(userOrder))
            {
                showRight();
                //go formal
                Timer outAsInstructionFormal = new Timer();
                outAsInstructionFormal.Interval = 500;
                outAsInstructionFormal.AutoReset = false;
                outAsInstructionFormal.Elapsed +=
                    new ElapsedEventHandler(outAsComprhPracRight_Elapsed);
                outAsInstructionFormal.Enabled = true;

            }
            else
            {
                showWrong();
                //go agian or formal

                Timer outAsComprhPracWrong = new Timer();
                outAsComprhPracWrong.Interval = 1500;
                outAsComprhPracWrong.AutoReset = false;
                outAsComprhPracWrong.Elapsed +=
                    new ElapsedEventHandler(outAsComprhPracWrong_Elapsed);
                outAsComprhPracWrong.Enabled = true;
            }
        }

        private void fillJumpedAllOK(int schemeIDX)
        {
            for (int i = 0; i < BasePage.mTestScheme[schemeIDX]; i++)
            {
                mBasePage.mRecorder.mathExpression.Add(mBasePage.mTest[schemeIDX].mTrails[i].equation);
                mBasePage.mRecorder.mathOn.Add(0);
                mBasePage.mRecorder.mathOff.Add(0);
                mBasePage.mRecorder.mathDure.Add(0);
                mBasePage.mRecorder.displayedAnswer.Add(mBasePage.mTest[schemeIDX].mTrails[i].result);
                mBasePage.mRecorder.choice.Add("pass");
                mBasePage.mRecorder.correctness.Add(true);
                mBasePage.mRecorder.choiceShowTime.Add(0);
                mBasePage.mRecorder.choiceMadeTime.Add(0);
                mBasePage.mRecorder.choiceDure.Add(0);
                mBasePage.mRecorder.animal.Add(mBasePage.mTest[schemeIDX].mTrails[i].memTarget);
                mBasePage.mRecorder.isExtra.Add(true);
                mBasePage.mRecorder.isPractise.Add(false);
                mBasePage.mRecorder.spanWidth.Add(BasePage.mTestScheme[schemeIDX]);
                mBasePage.mRecorder.groupNum.Add(mBasePage.GetGroupAtInSpan(schemeIDX, BasePage.mTestScheme));
                mBasePage.mRecorder.isOvertime.Add(false);
                mBasePage.mRecorder.inGroupNum.Add(mBasePage.mCurInGrpAt);
                mBasePage.mRecorder.equaLv.Add(mBasePage.mTest[schemeIDX].mTrails[i].equationLevel);
            }

            mBasePage.mRecorder.spanWidthG.Add(BasePage.mTestScheme[schemeIDX]);
            mBasePage.mRecorder.groupNumG.Add(mBasePage.GetGroupAtInSpan(schemeIDX, BasePage.mTestScheme));
            mBasePage.mRecorder.isExtraG.Add(true);
            mBasePage.mRecorder.isPractiseG.Add(false);
            mBasePage.mRecorder.orderOn.Add(0);
            mBasePage.mRecorder.orderOff.Add(0);
            mBasePage.mRecorder.orderDure.Add(0);
            string rightOrder = "";
            for (int i = 0; i < mBasePage.mTest[schemeIDX].mTrails.Count; i++)
            {
                rightOrder += mBasePage.mTest[schemeIDX].mTrails[i].memTarget;
            }
            mBasePage.mRecorder.rightOrder.Add(rightOrder);
            mBasePage.mRecorder.userInputOrder.Add("pass");
        }

        private void formalGoOut(int schemeID2Check, string userOrder, long offtime)
        {
            mBasePage.ResetSchemeIterationStatus();
            saveComprehForamlOutTime(offtime);
            string realOrder = mBasePage.GetFormalAnimalRealOrder(schemeID2Check);
            mBasePage.mRecorder.rightOrder.Add(realOrder);
            mBasePage.mRecorder.userInputOrder.Add(userOrder);
            mBasePage.mRecorder.isExtraG.Add(mIsExtra);
            mBasePage.mRecorder.isPractiseG.Add(false);

            if (mBasePage.IfGroupPassed(getSchemeID2Check(), BasePage.mTestScheme))
            {
                if (!mIsExtra || mBasePage.mCurSchemeAt == 2)
                {
                    if (getSchemeID2Check() != BasePage.mTestScheme.Length - 2)//not the last span
                    {
                        fillJumpedAllOK(mBasePage.mCurSchemeAt);
                        mBasePage.mCurSchemeAt++;//jump

                        if (mBasePage.mCurSchemeAt == 2)
                        {
                            fillJumpedAllOK(mBasePage.mCurSchemeAt);
                            mBasePage.mCurSchemeAt++;//jump over 222
                        }

                        mBasePage.ShowGroupTitle();
                    }
                    else//is last span and pass
                    {
                        //finish
                        mBasePage.mSecondFormal = false;
                        mBasePage.ShowFinishPage(mBasePage);
                    }
                }
                else//is extra
                {
                    if (!mBasePage.SchemeReturned())
                    {
                        mBasePage.ShowGroupTitle();
                    }
                    else
                    {
                        //finish
                        mBasePage.mSecondFormal = false;
                        mBasePage.ShowFinishPage(mBasePage);
                    }
                }

                mBasePage.mSecondFormal = false;
                
            }
            else//not pass
            {
                if (!mIsExtra)//one more chance
                {
                    mBasePage.mSecondFormal = true;
                    mBasePage.ShowGroupTitle();
                }
                else
                {
                    if (mBasePage.mCurSchemeAt != 2)
                    {
                        //output, save, 
                        //quit
                        mBasePage.mSecondFormal = false;
                        mBasePage.ShowFinishPage(mBasePage);
                    }
                    else//==2
                    {
                        //one more chance
                        mBasePage.mSecondFormal = true;
                        mBasePage.ShowGroupTitle();
                    }
                }
            }
        }

        int getSchemeID2Check()
        {
            int schemeID2Check = -4;

            if (mBasePage.SchemeReturned())
            {
                if (mBasePage.mStage == Stage.MemPrac)
                {
                    schemeID2Check = BasePage.mMemPracScheme.Length - 1;
                }
                else if (mBasePage.mStage == Stage.ComprehPrac)
                {
                    schemeID2Check = BasePage.mPracScheme.Length - 1;
                }
                else if (mBasePage.mStage == Stage.Formal)
                {
                    schemeID2Check = BasePage.mTestScheme.Length - 1;
                }
            }
            else
            {
                schemeID2Check = mBasePage.mCurSchemeAt - 1;
            }

            return schemeID2Check;
        }

        private void amBtnConfirm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            long offtime = mBasePage.mTimeline.ElapsedMilliseconds;
            string realOrder;

            string userOrder = "";

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                for (int i = 0; i < mAnswersBoxes.Count; i++)
                {
                    if (mAnswersBoxes[i].Content != null)
                    {
                        userOrder += mAnswersBoxes[i].Content.ToString();
                    }
                }
            }
            else if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                userOrder = mChess.GetAnswerStr();
            }

            if (String.IsNullOrEmpty(userOrder))
                userOrder = "not set";

            mCountDowner.Enabled = false;

            int schemeID2Check = getSchemeID2Check();


            switch (mBasePage.mStage)
            {
                case Stage.MemPrac:
                    mBasePage.ResetSchemeIterationStatus();
                    mBasePage.mRecorder.orderPracOff.Add(
                        offtime);
                    mBasePage.mRecorder.orderPracRTs.Add(
                        offtime - mBasePage.mRecorder.orderPracOn[mBasePage.mRecorder.orderPracOn.Count - 1]);


                    realOrder = mBasePage.GetAnimalPracRealOrder(schemeID2Check);

                    mBasePage.mRecorder.orderPracRealOrder.Add(realOrder);
                    mBasePage.mRecorder.orderPracAnswers.Add(userOrder);

                    if (realOrder.Equals(userOrder))
                    {
                        mBasePage.mRecorder.orderPracCorrectness.Add(true);
                        showRight();

                        Timer out2Equa = new Timer();
                        out2Equa.Interval = 500;
                        out2Equa.AutoReset = false;
                        out2Equa.Elapsed +=
                            new ElapsedEventHandler(outAsAnimalPracRight_Elapsed);
                        out2Equa.Enabled = true;
                    }
                    else
                    {
                        mBasePage.mRecorder.orderPracCorrectness.Add(false);
                        showWrong();

                        Timer outAsWrong = new Timer();
                        outAsWrong.Interval = 1500;
                        outAsWrong.AutoReset = false;
                        outAsWrong.Elapsed += 
                            new ElapsedEventHandler(outAsAnimalPracWrong_Elapsed);
                        outAsWrong.Enabled = true;
                    }
                    break;
                case Stage.ComprehPrac:
                    comprehPracGoOut(userOrder, offtime);
                    break;
                case Stage.Formal:
                    formalGoOut(schemeID2Check, userOrder, offtime);
                    break;
            }
        }

        void outAsComprhPracRight_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (mBasePage.IfGroupPassed(0, BasePage.mPracScheme))
            {
                mBasePage.Dispatcher.Invoke(
                   new TimeDele(mBasePage.ShowInstructionFormal));
            }
            else//order passed but mem failed
            {
                if (!mBasePage.mSecondComprehPrac)
                {
                    mBasePage.ProgressReturn();
                    mBasePage.mSecondComprehPrac = true;
                    mBasePage.Dispatcher.Invoke(
                        new TimeDele(mBasePage.ShowEquationPage));
                }
                else
                {
                    mBasePage.Dispatcher.Invoke(
                        new TimeDele(mBasePage.ShowInstructionFormal));
                }
            }
        }

        void outAsComprhPracWrong_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!mBasePage.mSecondComprehPrac)
            {
                mBasePage.ProgressReturn();
                mBasePage.mSecondComprehPrac = true;
                mBasePage.Dispatcher.Invoke(
                    new TimeDele(mBasePage.ShowEquationPage));
            }
            else
            {
                mBasePage.Dispatcher.Invoke(
                    new TimeDele(mBasePage.ShowInstructionFormal));
            }
        }

        delegate void TimeDele();

        void outAsAnimalPracRight_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.mSecondMemPrac = false;
            if(!mBasePage.SchemeReturned())
            {
                mBasePage.Dispatcher.Invoke(
                    new TimeDele(mBasePage.ShowBoardAnimal));
            }
            else
            {
                //go base line test
                mBasePage.Dispatcher.Invoke(
                    new TimeDele(mBasePage.ShowInstructionEquationPrac));
            }
        }

        void outAsAnimalPracWrong_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!mBasePage.mSecondMemPrac)
            {
                mBasePage.mSecondMemPrac = true;
                if (!mBasePage.SchemeReturned())
                {
                    mBasePage.mCurSchemeAt--;
                }
                else
                {
                    mBasePage.mCurSchemeAt = BasePage.mMemPracScheme.Length - 1;
                }
                 mBasePage.Dispatcher.Invoke(
                    new TimeDele(mBasePage.ShowBoardAnimal));
            }
            else
            {
                mBasePage.mSecondMemPrac = false;
                if (!mBasePage.SchemeReturned())
                {
                    mBasePage.Dispatcher.Invoke(
                    new TimeDele(mBasePage.ShowBoardAnimal));
                }
                else
                {
                    //go base line test
                    mBasePage.Dispatcher.Invoke(
                        new TimeDele(mBasePage.ShowInstructionEquationPrac));
                }
            }
        }

        Timer mCountDowner;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mCountDowner = new Timer();
            mCountDowner.AutoReset = true;
            mCountDowner.Interval = 1000;
            mCountDowner.Elapsed += new ElapsedEventHandler(cd_Elapsed);

            if (mBasePage.mStage == Stage.ComprehPrac ||
                mBasePage.mStage == Stage.Formal)
            {
                mCountDowner.Enabled = true;

                mBasePage.mRecorder.orderOn.Add(
                    mBasePage.mTimeline.ElapsedMilliseconds);
                //-1 for it is already iterated
                mBasePage.mRecorder.spanWidthG.Add(
                    BasePage.mTestScheme[getSchemeID2Check()]);
                
                mBasePage.mRecorder.groupNumG.Add(
                mBasePage.GetGroupAtInSpan(getSchemeID2Check(),
                    BasePage.mTestScheme));
            }
            else
            {
                //single practise does not limit time
                mCountDowner.Enabled = false;
                amLabelCountDown.Visibility = System.Windows.Visibility.Hidden;

                mBasePage.mRecorder.orderPracOn.Add(
                    mBasePage.mTimeline.ElapsedMilliseconds);
            }
        }

        void cd_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(countDown));
        }

        private void countDown()
        {
            mCountDownNum--;
            amLabelCountDown.Content = "剩余时间：" + mCountDownNum + "秒";
            if (mCountDownNum == 0)
            {
                long offtime = mBasePage.mTimeline.ElapsedMilliseconds;
                mCountDowner.Enabled = false;
                switch (mBasePage.mStage)
                {
                    case Stage.ComprehPrac:
                        comprehPracGoOut("overtime", offtime);
                        break;
                    case Stage.Formal:
                        formalGoOut(getSchemeID2Check(), "overtime", offtime);
                        break;
                }
            }
        }
    }
}

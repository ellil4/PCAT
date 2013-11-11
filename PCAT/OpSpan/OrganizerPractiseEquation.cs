using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using FiveElementsIntTest;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Threading;
using LibTabCharter;
using System.Diagnostics;

namespace FiveElementsIntTest.OpSpan
{
    public class OrganizerPractiseEquation : OrganizerPractiseBase
    {
        public OpSpanEquationMaker mOSEM;
        private int mPosMark = 0;
        private Random mRDM;

        public List<StEquation> mEquations;
        public List<long> mRTs;
        public List<bool> mAnswers;

        private int mActualTestLen = 12;

        private Stopwatch mWatch;


        public OrganizerPractiseEquation(PageOpSpan page) : base(page)
        {
            mPage = page;
            mOSEM = new OpSpanEquationMaker();
            mRDM = new Random();
            mfNext = showInstruction;
            mRTs = new List<long>();
            mWatch = new Stopwatch();
            mAnswers = new List<bool>();
            
            if (mPage.mbFixedItemMode)
            {
                mEquations = new List<StEquation>();
                TabFetcher fetcher =
                    new TabFetcher(
                        FEITStandard.GetExePath() + "OP\\opspanbaseline.txt", "\\t");
                fetcher.Open();

                fetcher.GetLineBy();//skip header
                List<String> line;
                while ((line = fetcher.GetLineBy()).Count != 0)
                {
                    StEquation equ = new StEquation();
                    equ.Equation = line[1];
                    equ.Result = Int32.Parse(line[2]);
                    if (Int32.Parse(line[3]) == 1)
                    {
                        equ.Answer = true;
                    }
                    else
                    {
                        equ.Answer = false;
                    }

                    mEquations.Add(equ);
                }

                fetcher.Close();
            }
        }

        private void showInstruction()
        {
            mPage.ClearAll();

            LayoutInstruction li = new LayoutInstruction(ref mPage.mBaseCanvas);
            //li.addTitle(200, 0, "算式练习", "KaiTi", 50, Color.FromRgb(255, 255, 255));
            li.addInstruction(200, 0, 638, 400,
                "下面再来练习一下心算\r\n在完成每一心算题后请尽快单击         \r\n然后判断给出的心算答案是否正确",
                "KaiTi", 40, Color.FromRgb(255, 255, 255));

            CompBtnNextPage btn = new CompBtnNextPage("算好了");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 290, 330);
            btn.mfOnAction = doNothing;

            mfNext = showEquation;

            CompBtnNextPage btnGO = new CompBtnNextPage("开始练习");
            btnGO.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 490);
            btnGO.mfOnAction = oneSecBlackScreen;
        }

        private void doNothing(object obj)
        { }

        private void equationClicked()
        {
            mRTs.Add(mWatch.ElapsedMilliseconds);
            mWatch.Stop();
            mWatch.Reset();
            quaterSecBlackScreen(); 
        }

        private void equationClicked(object obj)
        {
            equationClicked();
        }

        private void doNothing(CompDualDetermine cdd)
        {
        }

        private void positiveChoiceMadeReaction(CompDualDetermine cdd)
        {
            if (mShowAnswerzCorrectness == false)
            {
                cdd.setCorrectness(false);
                mAnswers.Add(false);
            }
            else
            {
                cdd.setCorrectness(true);
                mAnswers.Add(true);
            }

            cdd.mConfirmMethod = doNothing;
            cdd.mDenyMethod = doNothing;
            mPosMark++;

            if (mPosMark < mActualTestLen)
            {
                mfNext = showEquation;
            }
            else
            {
                mfNext = mPage.nextStep;
            }

            cdd.HideCorrecteness(false);
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        private void negativeChoiceMadeReaction(CompDualDetermine cdd)
        {
            if (mShowAnswerzCorrectness == false)
            {
                cdd.setCorrectness(true);
                mAnswers.Add(true);
            }
            else
            {
                cdd.setCorrectness(false);
                mAnswers.Add(false);
            }

            cdd.mConfirmMethod = doNothing;
            cdd.mDenyMethod = doNothing;
            mPosMark++;

            if (mPosMark < 16)
            {
                mfNext = showEquation;
            }
            else
            {
                mfNext = mPage.nextStep;
            }

            cdd.HideCorrecteness(false);
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        private String mExpression = "";
        private int mResult = -1;

        public void showEquation()
        {
            mWatch.Start();
            if (mPage.mbFixedItemMode)
            {
                mExpression = mEquations[mPosMark].Equation;
                mResult = mEquations[mPosMark].Result;
            }
            else
            {

                if (mPosMark >= 0 && mPosMark < 8)
                {
                    mOSEM.GenEquation(ref mExpression, ref mResult, EquationType.NonCarry);
                }
                else if (mPosMark >= 8 && mPosMark < 16)
                {
                    mOSEM.GenEquation(ref mExpression, ref mResult, EquationType.Carry);
                }
            }

            mPage.ClearAll();
            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen(mExpression + " ?",
                "Batang", 74, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            /*Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed2QuaterSecond);
            t.Interval = 2000;
            t.AutoReset = false;
            t.Enabled = true;*/

            //new FEITClickableScreen(ref mPage.mBaseCanvas, equationClicked);

            CompBtnNextPage btn = new CompBtnNextPage("算好了");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 490);
            btn.mfOnAction = equationClicked;
            
            mfNext = showAnswer;
        }

        private bool mShowAnswerzCorrectness = false;

        public void showAnswer()
        {
            mPage.ClearAll();

            mRDM = new Random();
            int falseAnswer = -1;
            CompDualDetermine cdd = new CompDualDetermine();
            cdd.HideCorrecteness(true);

            if (mPage.mbFixedItemMode)
            {
                mShowAnswerzCorrectness = mEquations[mPosMark].Answer;
                cdd.setResult(mResult.ToString());
            }
            else
            {
                if (mRDM.Next(0, 2) == 0)
                {
                    mShowAnswerzCorrectness = false;
                    while ((falseAnswer = mRDM.Next(0, 100)) == mResult)
                    {
                        falseAnswer = mRDM.Next(0, 100);
                    }

                    cdd.setResult(falseAnswer.ToString());
                }
                else
                {
                    mShowAnswerzCorrectness = true;
                    cdd.setResult(mResult.ToString());
                }
            }

            cdd.mConfirmMethod = positiveChoiceMadeReaction;
            cdd.mDenyMethod = negativeChoiceMadeReaction;

            mPage.mBaseCanvas.Children.Add(cdd);
            Canvas.SetTop(cdd, FEITStandard.PAGE_BEG_Y +
                (FEITStandard.PAGE_HEIGHT - CompDualDetermine.OUTHEIGHT) / 2);
            Canvas.SetLeft(cdd, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - CompDualDetermine.OUTWIDTH) / 2);

        }
    }
}

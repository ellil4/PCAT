using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using FiveElementsIntTest;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.OpSpan
{
    public class OrganizerPractiseEquation : OrganizerPractiseBase
    {
        public OpSpanEquationMaker mOSEM;
        private int mPosMark = 0;
        private Random mRDM;

        public OrganizerPractiseEquation(PageOpSpan page) : base(page)
        {
            mPage = page;
            mOSEM = new OpSpanEquationMaker();
            mRDM = new Random();
            mfNext = showInstruction;
        }

        private void showInstruction()
        {
            mPage.ClearAll();

            LayoutInstruction li = new LayoutInstruction(ref mPage.mBaseCanvas);
            //li.addTitle(200, 0, "算式练习", "KaiTi", 50, Color.FromRgb(255, 255, 255));
            li.addInstruction(240, 0, 638, 200, 
                "下面再来练习一下心算，在完成每一心算题后，请尽快单击鼠标左键。然后判断给出的心算答案是否正确。请点击鼠标以开始。",
                "KaiTi", 40, Color.FromRgb(255, 255, 255));

            mfNext = showEquation;
            FEITClickableScreen fcs = new FEITClickableScreen(ref mPage.mBaseCanvas, oneSecBlackScreen);
        }

        private void t_Elapsed2QuaterSecond(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(quaterSecBlackScreen)); 
        }

        private void positiveChoiceMadeReaction(CompDualDetermine cdd)
        {
            if (mShowAnswerzCorrectness == false)
            {
                cdd.setCorrectness(false);
            }
            else
            {
                cdd.setCorrectness(true);
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
            }
            else
            {
                cdd.setCorrectness(false);
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

            if (mPosMark >= 0 && mPosMark < 4)
            {
                mOSEM.GenEquation(ref mExpression, ref mResult, EquationType.NonCarry);
            }
            else if (mPosMark >= 4 && mPosMark < 8)
            {
                mOSEM.GenEquation(ref mExpression, ref mResult, EquationType.Carry);
            }

            mPage.ClearAll();
            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen(mExpression + " = ?",
                "Batang", 74, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed2QuaterSecond);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;

            mfNext = showAnswer;

            mPosMark++;
        }

        private bool mShowAnswerzCorrectness = false;

        public void showAnswer()
        {
            mPage.ClearAll();

            mRDM = new Random();
            int falseAnswer = -1;
            CompDualDetermine cdd = new CompDualDetermine();
            cdd.HideCorrecteness(true);

            if (mPosMark < 8)
            {
                mfNext = showEquation;
            }
            else
            {
                mfNext = mPage.nextStep;
            }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.OpSpan
{
    public class OrganizerTrailOS
    {
        private List<TrailGroupOS> mGroups;
        private PageOpSpan mPage;
        private bool mPractiseMode = false;
        private bool mExtraTrailDone = false;

        private int mCurTypeMax = 0;
        private int mCurTypeAt = 1;

        private int mGrpAt = 0;
        private int mTraAt = 0;

        //about result
        private bool mValid = true;
        private int mSpanOrderErrCount = 0;

        private List<AnswerOpSpan> mAnswerGrp;
        public AnswerOpSpan mAnswer;
        private int mOrderCorrectCount = 0;
        private int mMathCorrectCount = 0;
        private int mMathErrACC2 = 0;



        private int mCurTrailsCount = 0;

        private OpSpanEquationMaker mOSEM;
        private String mEquation;
        private int mEquationResult;

        private delegate bool timedele();
        private delegate void simpleTimedele();



        public OrganizerTrailOS(List<TrailGroupOS> _groups, PageOpSpan _page, bool practise)
        {
            mGroups = _groups;
            mPage = _page;

            mPractiseMode = practise;
            mAnswerGrp = new List<AnswerOpSpan>();
            mAnswer = new AnswerOpSpan();

            mCurTrailsCount = mGroups[mGrpAt].mTrails.Count;
            mCurTypeMax = getCountOfType(mCurTrailsCount);

            mOSEM = new OpSpanEquationMaker();

            mPage.mTimer.Start();
        }

        public delegate bool OrgRoute();
        public OrgRoute route;

        public bool nextStep()
        {
            return route();
        }

        public void nextStep(object obj)
        {
            nextStep();
        }

        private int getCountOfType(int len)
        {
            int retval = 0;

            for (int i = 0; i < mGroups.Count; i++)
            {
                if (mGroups[i].mTrails.Count == len)
                    retval++;
            }

            return retval;
        }

        public bool showTitlePage()
        {
            mPage.ClearAll();

            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("做心算，记属相",
                "KaiTi", 50, ref mPage.mBaseCanvas, 0, Color.FromRgb(0, 255, 0));

            route = showLongBlackPage2Equation;

            //clickable
            //new FEITClickableScreen(ref mPage.mBaseCanvas, mPage.nextStep);

            //delay
            delayAndNext(2000);

            return true;
        }

        private int getOrderCorrectCount()
        {
            int ret = 0;
            for (int i = 0; i < mCurTrailsCount; i++)
            {
                if (i < mAnswer.Order.Count)
                {
                    if (mAnswer.Order[i] != -1)
                    {
                        if (UIGroupNumChecksOS.LUNAR_ANI[mAnswer.Order[i]].Equals(
                            mGroups[mGrpAt].mTrails[i].memTarget))
                        {
                            ret++;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            
            return ret;
        }

        private double getTotalMathCorrectRate()
        {
            double retval = -1;
            int totalCorrect = 0;
            int totalCount = 0;
            for (int i = 0; i < mAnswerGrp.Count; i++)
            {
                for (int j = 0; j < mAnswerGrp[i].Confirm.Count; j++)
                {
                    int grp_i = i;
                    if (i > 1 && mExtraTrailDone)
                        grp_i += 2;

                    if (mGroups[grp_i].mTrails[j].correctness ==
                        mAnswerGrp[i].Confirm[j])
                    {
                        totalCorrect++;
                    }
                    totalCount++;
                }
 
            }

            retval = ((double)totalCorrect / (double)totalCount);

            return retval;
        }

        private int getMathCorrectCount()
        {
            int ret = 0;
            for (int i = 0; i < mCurTrailsCount; i++)
            {
                if (mGroups[mGrpAt].mTrails[i].correctness ==
                    mAnswer.Confirm[i])
                {
                    ret++;
                }
            }
            
            return ret;
        }

        public void groupIterate()
        {
            
            mAnswer = new AnswerOpSpan();
            
            //statistics
            mOrderCorrectCount = 0;

            //cursor
            if (mGrpAt == 1 && mSpanOrderErrCount < 2 && !mPractiseMode)
            {
                mGrpAt += 3;
            }
            else
            {
                mGrpAt++;//go extra
                mExtraTrailDone = true;
            }

            mTraAt = 0;

            if (mGrpAt < mGroups.Count)
            {
                int newTrailCount = mGroups[mGrpAt].mTrails.Count;

                //when span iterates
                if (mCurTrailsCount != newTrailCount)
                {
                    mCurTrailsCount = newTrailCount;
                    mCurTypeMax = getCountOfType(mCurTrailsCount);
                    mCurTypeAt = 1;
                    mSpanOrderErrCount = 0;
                }
                else
                {
                    mCurTypeAt++;
                }
            }
            else if (mGrpAt == mGroups.Count)
            {
                //test quit after iteration
                route = quit;
            }
        }

        private int mathDoneCount()
        {
            int retval = 0;

            for (int i = 0; i <= mGrpAt; i++)
            {
                retval += mGroups[i].mTrails.Count;
            }

            return retval;
        }

        public void groupStatistics()
        {
            mOrderCorrectCount = getOrderCorrectCount();
            mMathCorrectCount = getMathCorrectCount();
            mMathErrACC2 += mCurTrailsCount - mMathCorrectCount;

            //get span correct rate
            if(mGrpAt < mGroups.Count - 1)
            {
                int newTrailCount = mGroups[mGrpAt + 1].mTrails.Count;
            }

            if (mMathErrACC2 >= 2)
            {
                mValid = false;
                mMathErrACC2 = 0;
            }
            else
            {
                mValid = true;
            }

            if (mOrderCorrectCount != mCurTrailsCount)
            {
                mSpanOrderErrCount++;
            }

            if (!mValid && !mPractiseMode)
            {
                route = showWarningPage;
            }

            //quit with error
            mAnswerGrp.Add(mAnswer);

            if (!mPractiseMode &&
                (mSpanOrderErrCount == 2 || getTotalMathCorrectRate() < 0.8) && 
                mGrpAt > 3 &&
                mCurTypeAt == mCurTypeMax)//quit beyond span2
            {
                route = quit;
            }
            else if (!mPractiseMode &&
                (mSpanOrderErrCount == 4) && 
                mGrpAt == 3)//quit in span2
            {
                route = quit;
            }
        }

        public bool showOrderPage()
        {
            SubPageOrderOS subPage = new SubPageOrderOS(ref mPage, this);
            mPage.ClearAll();
            
        //recorder
            //on time
            mPage.mRecorder.orderOn.Add(mPage.mTimer.GetElapsedTime());
            //the right order
            string rightOrderStr = "";
            int len = mGroups[mGrpAt].mTrails.Count;
            for(int i = 0; i < len; i++)
            {
                rightOrderStr += mGroups[mGrpAt].mTrails[i].memTarget;   
            }
            mPage.mRecorder.rightOrder.Add(rightOrderStr);

            subPage.Show();

            if (mPractiseMode)
            {
                route = showReportPage;
            }
            else 
            {
                route = showBlackPage2Title;

            }

            return true;
        }

        public bool showDeterminePage()
        {

            mPage.mRecorder.mathOff.Add(mPage.mTimer.GetElapsedTime());
            mPage.mRecorder.displayedAnswer.Add(Int32.Parse(mGroups[mGrpAt].mTrails[mTraAt].result));

            SubPageDetermine subPage = new SubPageDetermine(ref mPage, this);
            mPage.ClearAll();
            subPage.setResult(mGroups[mGrpAt].mTrails[mTraAt].result);
            subPage.hideCorrectness(true);

            subPage.Show();

            route = showBlackPage2Animal;

            return true;
        }

        public bool showEquationPage()
        {
            mPage.ClearAll();

            if (!mPage.mbFixedItemMode)//if not fixed gen random
            {
                if (mPractiseMode)
                {
                    mOSEM.GenEquation(ref mEquation, ref mEquationResult, EquationType.NonCarry);
                }
                else
                {
                    if (mGrpAt < 2)//non-carry
                    {
                        mOSEM.GenEquation(ref mEquation, ref mEquationResult, EquationType.NonCarry);
                    }
                    else//carry
                    {
                        mOSEM.GenEquation(ref mEquation, ref mEquationResult, EquationType.Carry);
                    }
                }

                Random rdm = new Random();
                if (rdm.Next(0, 2) == 0)//set incorrect to show
                {
                    mGroups[mGrpAt].mTrails[mTraAt].result = rdm.Next(0, 200).ToString();
                    mGroups[mGrpAt].mTrails[mTraAt].correctness = false;
                }
                else//set correct to show
                {
                    mGroups[mGrpAt].mTrails[mTraAt].result = mEquationResult.ToString();
                    mGroups[mGrpAt].mTrails[mTraAt].correctness = true;
                }

                mGroups[mGrpAt].mTrails[mTraAt].equation = mEquation;
            }

            mGroups[mGrpAt].mTrails[mTraAt].equation += " ?";

            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen(mGroups[mGrpAt].mTrails[mTraAt].equation,
                "Batang", 74, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            route = showBlackPage2Determine;

            //record
            Timer t = null;
            //save rec
            mPage.mRecorder.mathOn.Add(mPage.mTimer.GetElapsedTime());
            mPage.mRecorder.mathExpression.Add(mGroups[mGrpAt].mTrails[mTraAt].equation);

            if (!mPractiseMode)
            {
                //overtime!
                t = new Timer();
                t.Elapsed += new ElapsedEventHandler(t_Overtime);
                t.Interval = mPage.mMeanRT * 2;
                t.AutoReset = false;
                t.Enabled = true;
            }

            //new FEITClickableScreen(ref mPage.mBaseCanvas, mPage.nextStep, ref t);
            CompBtnNextPage btn = new CompBtnNextPage("算好了", t);
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = mPage.nextStep;

            return true;
        }

        public bool currentCorrectness()
        {
            return mGroups[mGrpAt].mTrails[mTraAt].correctness;
        }

        public bool practiseMode()
        {
            return mPractiseMode;
        }

        void t_Overtime(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, 
                new simpleTimedele(showBlackPage2Overtime));
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(nextStep));
        }

        private void delayAndNext(long duration)
        {
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = duration;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void showBlackPage2Overtime()
        {
            mPage.ClearAll();
            route = showOvertimePage;
            delayAndNext(250);
        }

        public bool showBlackPage2Order()
        {
            mPage.ClearAll();

            route = showOrderPage;
            //delay
            delayAndNext(500);

            return true;
        }

        public bool showBlackPage2Animal()
        {
            mPage.ClearAll();
            
            route = showAnimalPage;
            //delay
            delayAndNext(250);

            return true;
        }

        public bool showBlackPage2Equation()
        {
            mPage.ClearAll();

            route = showEquationPage;
            //delay
            delayAndNext(250);

            return true;
        }

        public bool showLongBlackPage2Equation()
        {
            mPage.ClearAll();

            route = showEquationPage;
            //delay
            delayAndNext(500);

            return true;
        }

        public bool showBlackPage2Determine()
        {
            mPage.ClearAll();

            route = showDeterminePage;
            //delay
            delayAndNext(250);

            return true;
        }

        public bool showBlackPage2Title()
        {
            mPage.ClearAll();
            route = showTitlePage;
            delayAndNext(1000);

            return true;
        }

        public bool showOvertimePage()
        {
            mPage.mRecorder.mathOff.Add(mPage.mTimer.GetElapsedTime());
            mPage.mRecorder.displayedAnswer.Add(Int32.Parse(mGroups[mGrpAt].mTrails[mTraAt].result));

            route = showBlackPage2Animal;

            mPage.ClearAll();
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("心算超时",
                "Microsoft YaHei", 55, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 0, 0));
            
            //fill a wrong answer and no sense data
            mAnswer.Confirm.Add(!currentCorrectness());
            mPage.mRecorder.choiceShowTime.Add(-1);
            mPage.mRecorder.choiceMadeTime.Add(-1);
            mPage.mRecorder.choice.Add("null");
            mPage.mRecorder.correctness.Add(false);

            delayAndNext(1500);

            return true;
        }

        public bool showAnimalPage()
        {
            mPage.ClearAll();
            CompCentralText ct = new CompCentralText();
            string target = mGroups[mGrpAt].mTrails[mTraAt].memTarget;//GrpAt/////////////////////////////////////////////////////////
            ct.PutTextToCentralScreen(target,
                "Microsoft YaHei", 55, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            mPage.mRecorder.animal.Add(target);
            //mPage.mRecorder.groupID.Add(mCurTrailsCount);
            //mPage.mRecorder.subgroupID.Add(mCurTypeAt);

            if (mTraAt == mCurTrailsCount - 1)
            {
                route = showBlackPage2Order;
            }
            else
            {
                mTraAt++;
                route = showBlackPage2Equation;
            }

            //delay
            delayAndNext(1000);

            return true;
        }


        public bool showReportPage()//only in practise
        {
            mPage.ClearAll();

            CompCentralText ct = new CompCentralText();
            CompCentralText ct2 = new CompCentralText();
            CompCentralText ct3 = new CompCentralText();

            ct.PutTextToCentralScreen("这组题（共" + mCurTrailsCount + "个）中，你",
                "KaiTi", 32, ref mPage.mBaseCanvas, -30, Color.FromRgb(255, 255, 255));
            ct2.PutTextToCentralScreen("记对了" + mOrderCorrectCount + "个属相",
                "KaiTi", 32, ref mPage.mBaseCanvas, 4, Color.FromRgb(255, 255, 255));
            ct3.PutTextToCentralScreen("做对了" + mMathCorrectCount + "个心算",
                "KaiTi", 32, ref mPage.mBaseCanvas, 36, Color.FromRgb(255, 255, 255));

            if (mGrpAt == mGroups.Count - 1)
            {
                //quit practise
                route = quit;
            }
            else
            {
                groupIterate();
                route = showLongBlackPage2Equation;
            }

            //new FEITClickableScreen(ref mPage.mBaseCanvas, mPage.nextStep);
            //delayAndNext(2000);

            CompBtnNextPage btn = new CompBtnNextPage("继续");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = mPage.nextStep;

            return true;
        }

        public bool showWarningPage()
        {
            mPage.ClearAll();

            CompCentralText cct = new CompCentralText();
            CompCentralText cct2 = new CompCentralText();
            cct.PutTextToCentralScreen("请你注意", "KaiTi", 48, 
                ref mPage.mBaseCanvas, -30, Color.FromRgb(255, 0, 0));
            cct2.PutTextToCentralScreen(" 保持心算的正确率！", "KaiTi", 48, 
                ref mPage.mBaseCanvas, 30, Color.FromRgb(255, 0, 0));


            route = showBlackPage2Title;

            delayAndNext(2000);

            return true;
        }

        public bool quit()
        {
            if (mPractiseMode)
            {
                mPage.mCurrentStatus = PageOpSpan.PageAttr.instruction2;
            }
            else
            {
                //record
                mPage.mTimer.Stop();
                mPage.mRecorder.outputReport(
                    FEITStandard.GetRepotOutputPath() + "op\\" + mPage.interFilename,
                    FEITStandard.GetRepotOutputPath() + "op\\" + mPage.orderFilename, 
                    FEITStandard.GetRepotOutputPath() + "op\\" + mPage.pracMathFilename,
                    FEITStandard.GetRepotOutputPath() + "op\\" + mPage.pracOrderFilename);

                mPage.mCurrentStatus = PageOpSpan.PageAttr.finish;
            }

            mPage.nextStep();

            return false;
        }
    }
}

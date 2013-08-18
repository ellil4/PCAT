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

        private int mCurTypeMax = 0;
        private int mCurTypeAt = 1;

        private int mGrpAt = 0;
        private int mTraAt = 0;

        //about result
        private bool mValid = true;
        private int mCurOrderErrCount = 0;

        private List<AnswerOpSpan> mAnswerGrp;
        public AnswerOpSpan mAnswer;
        private int mOrderCorrectCount = 0;

        private int mMathCorrectCount = 0;
        private double mMathCorrectRate = 0.0;

        private int mMathTotalCorrectCount = 0;
        private double mMathTotalCorrectRate = 0.0;

        private int mCurTrailsCount = 0;

        private OpSpanEquationMaker mOSEM;
        private String mEquation;
        private int mEquationResult;

        private delegate bool timedele();



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

            if(!practise)
                mPage.mTimer.Start();
        }

        public delegate bool OrgRoute();
        public OrgRoute route;

        public bool nextStep()
        {
            return route();
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
            ct.PutTextToCentralScreen("***" + mCurTrailsCount + "题第" + mCurTypeAt + "组" + "***",
                "KaiTi", 50, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            route = showLongBlackPage2Equation;

            //clickable
            //new FEITClickableScreen(ref mPage.mBaseCanvas, mPage.nextStep);

            //delay
            delayAndNext(3000);

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
            mAnswerGrp.Add(mAnswer);
            mAnswer = new AnswerOpSpan();
            
            //statistics
            mMathCorrectCount = 0;
            mOrderCorrectCount = 0;

            //cursor
            if (mGrpAt == 2 && mCurOrderErrCount < 2 && !mPractiseMode)
            {
                mGrpAt += 3;
            }
            else
            {
                mGrpAt++;
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
                    mCurOrderErrCount = 0;
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
            mMathCorrectRate = 
                ((double)mMathCorrectCount / (double)mCurTrailsCount);
            
            mMathTotalCorrectCount += mMathCorrectCount;
            mMathTotalCorrectRate = ((double)mMathTotalCorrectCount / (double)mathDoneCount());

            if (mMathTotalCorrectRate < 0.85)
            {
                mValid = false;
            }
            else
            {
                mValid = true;
            }

            if (mOrderCorrectCount != mCurTrailsCount)
            {
                mCurOrderErrCount++;
            }

            if (!mValid && mCurTypeAt == mCurTypeMax && !mPractiseMode)
            {
                route = showWarningPage;
            }

            //quit with error
            if (!mPractiseMode && mCurOrderErrCount > 1 && mGrpAt > 5)//quit beyond span2
            {
                route = quit;
            }
            else if (!mPractiseMode && mCurOrderErrCount > 2 && mGrpAt == 4)//quit in span2
            {
                route = quit;
            }
        }

        public bool showOrderPage()
        {
            SubPageOrderOS subPage = new SubPageOrderOS(ref mPage, this);
            mPage.ClearAll();
            
            //recorder
            if (!mPractiseMode)
            {
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
            }

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
            if (!mPractiseMode)
            {
                mPage.mRecorder.mathOff.Add(mPage.mTimer.GetElapsedTime());
                mPage.mRecorder.displayedAnswer.Add(Int32.Parse(mGroups[mGrpAt].mTrails[mTraAt].result));
            }


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
            if (!mPractiseMode)
            {
                mPage.mRecorder.mathOn.Add(mPage.mTimer.GetElapsedTime());
                mPage.mRecorder.mathExpression.Add(mGroups[mGrpAt].mTrails[mTraAt].equation);
            }

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = mPage.mMeanRT;
            t.AutoReset = false;
            t.Enabled = true;
            new FEITClickableScreen(ref mPage.mBaseCanvas, mPage.nextStep, ref t);

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
            delayAndNext(1000);

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

        public bool showAnimalPage()
        {
            mPage.ClearAll();
            CompCentralText ct = new CompCentralText();
            string target = mGroups[mGrpAt].mTrails[mTraAt].memTarget;//GrpAt/////////////////////////////////////////////////////////
            ct.PutTextToCentralScreen(target,
                "Microsoft YaHei", 55, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            if (!mPractiseMode)
            {
                mPage.mRecorder.animal.Add(target);
                //mPage.mRecorder.groupID.Add(mCurTrailsCount);
                //mPage.mRecorder.subgroupID.Add(mCurTypeAt);
            }

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

            ct.PutTextToCentralScreen("这组题（共" + mCurTrailsCount + "个）中，您",
                "KaiTi", 32, ref mPage.mBaseCanvas, -30, Color.FromRgb(255, 255, 255));
            ct2.PutTextToCentralScreen("记对了" + mOrderCorrectCount + "个属相",
                "KaiTi", 32, ref mPage.mBaseCanvas, 4, Color.FromRgb(255, 255, 255));
            ct3.PutTextToCentralScreen("做对了" + mMathCorrectCount + "个心算",
                "KaiTi", 32, ref mPage.mBaseCanvas, 36, Color.FromRgb(255, 255, 255));
            LayoutInstruction li = new LayoutInstruction(ref mPage.mBaseCanvas);

            Color color;
            if(mMathCorrectRate > 0.8)
            {
                color = Color.FromRgb(15, 255, 15);
            }
            else
            {
                color = Color.FromRgb(255, 15, 15);
            }

            li.addTitle(FEITStandard.PAGE_BEG_Y, FEITStandard.PAGE_WIDTH / 2 - 65, 
                (mMathCorrectRate * 100).ToString("0") + "%", "Batang", 34, color); 

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
            delayAndNext(2000);

            return true;
        }

        public bool showWarningPage()
        {
            mPage.ClearAll();

            CompCentralText cct = new CompCentralText();
            CompCentralText cct2 = new CompCentralText();
            cct.PutTextToCentralScreen("请您注意", "KaiTi", 48, 
                ref mPage.mBaseCanvas, -30, Color.FromRgb(255, 255, 255));
            cct2.PutTextToCentralScreen("保持心算的正确率！", "KaiTi", 48, 
                ref mPage.mBaseCanvas, 30, Color.FromRgb(255, 255, 255));


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
                mPage.mRecorder.outputReport(FEITStandard.GetRepotOutputPath() + "op\\" + PageOpSpan.interFilename,
                    FEITStandard.GetRepotOutputPath() + "op\\" + PageOpSpan.orderFilename);

                mPage.mCurrentStatus = PageOpSpan.PageAttr.finish;
            }

            mPage.nextStep();

            return false;
        }
    }
}

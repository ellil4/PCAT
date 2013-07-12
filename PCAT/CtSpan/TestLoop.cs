using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;

namespace FiveElementsIntTest.CtSpan
{
    public enum InTestStage
    {
        SubTitle, Black, Graph, Count, Result, QuitInfo, DoQuit, Back, Warning
    }

    public class TestLoop
    {
        public InTestStage mStage;
        public bool mbFormal;
        public PageCtSpan mPage;
        public Stopwatch mTicker;
        public Stopwatch mGraphTicker;
        public int mCorrectInGrpCount = 0;
        public int mCorrectBetweenGrpCount = 0;
        public int mCountingIncorrectNum = 0;
        public bool mOnExtra = false;

        //public delegate void Next();
        //public Next mFuncNext;

        public TestLoop(PageCtSpan page)
        {
            mPage = page;
            mStage = InTestStage.SubTitle;
            mPage.mOrganizer.mConfirmBtn.mfDo = OnGraphConfirm;
            mPage.mOrganizer.mPan.mfConfirmOp = OnPanConfirm;
            mTicker = new Stopwatch();
            mGraphTicker = new Stopwatch();
        }

        public void SetFormal(bool flag)
        {
            mbFormal = flag;
        }

        public void StageIteration()
        {
            switch (mStage)
            {
                case InTestStage.SubTitle:
                    mPage.mOrganizer.ShowTrialTitle();
                    mStage = InTestStage.Black;
                    TickJump2(2000);
                    break;
                case InTestStage.Black:
                    mPage.mOrganizer.ShowBlackScreen();
                    mStage = InTestStage.Graph;
                    TickJump2(500);
                    break;
                case InTestStage.Graph:
                    mPage.mOrganizer.mGC.Clear();
                    mPage.mOrganizer.ShowGraph(
                        mPage.mItems[mPage.mTotalItemIndexAt]);
                    mGraphTicker.Reset();
                    mGraphTicker.Start();
                    break;
                case InTestStage.Count:
                    mPage.mOrganizer.mPan.Clear();
                    mPage.mOrganizer.ShowCountingPan();
                    mTicker.Reset();
                    mTicker.Start();
                    break;
                case InTestStage.Result:
                    mPage.mOrganizer.ShowResultBoard(
                        mPage.SpanArrangement[mPage.mTotalSpanIndexAt - 1], mCorrectInGrpCount);
                    if (mPage.mTotalSpanIndexAt == 3)
                    {
                        mStage = InTestStage.Back;
                    }
                    else
                    {
                        mStage = InTestStage.SubTitle;
                    }

                    TickJump2(2000);

                    break;
                case InTestStage.QuitInfo:
                    mPage.mOrganizer.ShowQuitPage();
                    mStage = InTestStage.DoQuit;
                    TickJump2(2000);
                    break;
                case InTestStage.DoQuit:
                    mPage.Quit();
                    break;
                case InTestStage.Back:
                    mPage.mStage = Proccess.Instruct2;
                    mPage.NextStage();
                    break;
                case InTestStage.Warning:
                    mPage.mOrganizer.ShowWarning();

                    if (!mbFormal)
                    {
                        mStage = InTestStage.Result;
                    }
                    else
                    {
                        mStage = InTestStage.SubTitle;
                    }

                    TickJump2(2000);
                    break;
            }
        }

        public void OnPanConfirm()
        {
            //save result
            mTicker.Stop();

            int resultsCount = mPage.mResults.Count;
            int beg = resultsCount - mPage.SpanArrangement[mPage.mTotalSpanIndexAt];
            //ckeck correctness
            bool correct = true;
            mCorrectInGrpCount = 0;

            String stdSerial = "";
            String userSerial = "";
            String userSerialCorrectMask = "";

            for (int o = beg, a = 0; o < resultsCount; o++, a++)
            {
                if (mPage.mOrganizer.mPan.mAnswers.Count == mPage.SpanArrangement[mPage.mTotalSpanIndexAt])
                {
                    if (mPage.mOrganizer.mPan.mAnswers[a] != mPage.mItems[o].TarCount)
                    {
                        correct = false;
                        userSerialCorrectMask += "0";

                    }
                    else
                    {
                        mCorrectInGrpCount++;
                        userSerialCorrectMask += "1";
                    }

                    stdSerial += mPage.mItems[o].TarCount;
                    userSerial += mPage.mOrganizer.mPan.mAnswers[a];
                }
                else
                {
                    correct = false;
                }
            }

            if (correct)
            {
                mCorrectBetweenGrpCount++;
            }

            //save for group
            for (int i = beg; i < resultsCount; i++)
            {
                mPage.mResults[i].Correctness = correct;
                mPage.mResults[i].RT = mTicker.ElapsedMilliseconds;

                mPage.mResults[i].StdSerial = stdSerial;
                mPage.mResults[i].UserSerial = userSerial;
                mPage.mResults[i].UserSerialCorrectCount = mCorrectInGrpCount;
                mPage.mResults[i].UserSerialCorrectMask = userSerialCorrectMask;
            }

            //iter and....
            //judge if exercise or add extra
            if (!mbFormal)
            {
                mStage = InTestStage.Result;
                mPage.mTotalSpanIndexAt++;
                mPage.mGrpAt++;

                if (mPage.mGrpAt == 3)
                {
                    mPage.mGrpAt = 0;
                    mCorrectBetweenGrpCount = 0;
                }
            }
            else
            {
                mStage = InTestStage.SubTitle;
                if ((!mOnExtra && mPage.mGrpAt == 2))//end of span
                {
                    if (mCorrectBetweenGrpCount < 2)//
                    {
                        if (mPage.SpanArrangement[mPage.mTotalSpanIndexAt] == 2)//go extra
                        {
                            //go extra
                            mOnExtra = true;
                            mPage.mTotalItemIndexAt++;
                            mPage.mGrpAt++;
                        }
                        else
                        {
                            //go quit
                            mStage = InTestStage.QuitInfo;
                        }

                    }
                    else//the span passed
                    {
                        mPage.mGrpAt = 0;

                        if (mPage.SpanArrangement[mPage.mTotalSpanIndexAt] == 2)
                        {
                            mPage.mTotalSpanIndexAt += 3;
                        }
                        else
                        {
                            mPage.mTotalSpanIndexAt++;
                        }

                        mCorrectBetweenGrpCount = 0;



                        if (mPage.mTotalSpanIndexAt == 27)//all finish???
                            mStage = InTestStage.QuitInfo;
                    }
                }
                else if (mOnExtra)//on extra
                {
                    if (mPage.mGrpAt == 4)//end of extra(the span passed)
                    {
                        mPage.mGrpAt = 0;
                        mPage.mTotalSpanIndexAt++;

                        if (mCorrectBetweenGrpCount < 3)
                            mStage = InTestStage.QuitInfo;

                        mCorrectBetweenGrpCount = 0;
                        mOnExtra = false;
                    }
                    else
                    {
                        mPage.mGrpAt++;
                        mPage.mTotalItemIndexAt++;
                    }
                }
                else
                {
                    mPage.mGrpAt++;
                    mPage.mTotalSpanIndexAt++;
                }
            }

            if (mStage != InTestStage.QuitInfo && mCountingIncorrectNum >= 2)
            {
                mStage = InTestStage.Warning;
            }
            mCountingIncorrectNum = 0;

            StageIteration();
        }

        public void OnGraphConfirm()
        {
            mGraphTicker.Stop();
            //save result
            StResult result = new StResult();
            result.SpanWidth = mPage.SpanArrangement[mPage.mTotalSpanIndexAt];
            result.GroupNum = mPage.mGrpAt + 1;
            result.GroupSeq = mPage.mSubGrpAt + 1;
            result.ClickOnType = mPage.mOrganizer.mGC.GetTapeTypecall();
            result.ClickOnPosition = mPage.mOrganizer.mGC.GetTapePositioncall();
            result.CountingCorrectness = mPage.mOrganizer.mGC.GetCountingCorrectness();
            if (!result.CountingCorrectness)
            {
                mCountingIncorrectNum++;
            }
            result.PractiseMode = !mbFormal;
            result.GraphRT = mGraphTicker.ElapsedMilliseconds;

            mPage.mResults.Add(result);

            //iter
            mPage.mTotalItemIndexAt++;
            mPage.mSubGrpAt++;
            //judge next step to
            if (mPage.mSubGrpAt == mPage.SpanArrangement[mPage.mTotalSpanIndexAt])
            {
                mPage.mSubGrpAt = 0;
                mStage = InTestStage.Count;
            }

            StageIteration();
        }

        public void TickJump2(long dure)
        {
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = dure;
            t.AutoReset = false;
            t.Enabled = true;
        }

        private delegate void timedele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(
                DispatcherPriority.Normal, new timedele(StageIteration));
        }
    }
}

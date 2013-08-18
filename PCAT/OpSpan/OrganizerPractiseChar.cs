using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Timers;
using System.Windows.Threading;
using LibTabCharter;

namespace FiveElementsIntTest.OpSpan
{
    public class OrganizerPractiseChar : OrganizerPractiseBase
    {
        //UIGroupNumChecksOS

        List<List<int>> mRealOrder;
        public List<int> mAnswer;
        int mGrandGroupAtIndex = 0;
        int mGroupAtIndex = 0;

        public OrganizerPractiseChar(PageOpSpan page) : base(page)
        {
            int[] scheme = { 2, 2, 3, 3 };

            mRealOrder = new List<List<int>>();

            if (!mPage.mbFixedItemMode)
            {
                for (int i = 0; i < scheme.Length; i++)
                {
                    mRealOrder.Add(getNonRepeatArray(scheme[i]));
                }
            }
            else//fixed mode
            {
                TabFetcher fetcher =
                    new TabFetcher(FEITStandard.GetExePath() + "OP\\opspanword.txt", "\\t");

                fetcher.Open();
                fetcher.GetLineBy();//skip header

                for (int i = 0; i < scheme.Length; i++)
                {
                    List<int> group = new List<int>();
                    for (int j = 0; j < scheme[i]; j++)
                    {
                        List<string> line = fetcher.GetLineBy();
                        group.Add(UIGroupNumChecksOS.GetIndexByChar(line[2]));
                    }
                    mRealOrder.Add(group);
                }

                fetcher.Close();
            }

            

            mfNext = showInstruction;
        }

        private int mRdmSeed = 1;

        public List<int> getNonRepeatArray(int count)
        {
            int thisBuf = -1;
            List<int> retval = new List<int>();
            bool breakDead = false;
            

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    breakDead = true;
                    Random rdm = new Random((int)DateTime.Now.ToFileTime() + mRdmSeed);
                    thisBuf = rdm.Next(0, 12);
                    for (int j = 0; j < retval.Count; j++)
                    {
                        if (retval[j] == thisBuf)
                        {
                            breakDead = false;
                            break;
                        }
                    }

                    if (breakDead)
                        break;
                }

                retval.Add(thisBuf);
                mRdmSeed++;
            }

            return retval;
        }

        private void showInstruction()
        {
            mPage.ClearAll();

            LayoutInstruction li = new LayoutInstruction(ref mPage.mBaseCanvas);
            //li.addTitle(200, 0, "", "KaiTi", 50, Color.FromRgb(255, 255, 255));
            li.addInstruction(240, 0, 570, 200, "下面先来练习一下记忆属相，请点击鼠标以开始。", 
                "KaiTi", 40, Color.FromRgb(255, 255, 255));

            mfNext = showAnimal;
            FEITClickableScreen fcs = new FEITClickableScreen(ref mPage.mBaseCanvas, oneSecBlackScreen);
        }

        private void showAnimal()
        {
            mPage.ClearAll();
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen(
                UIGroupNumChecksOS.LUNAR_ANI[mRealOrder[mGrandGroupAtIndex][mGroupAtIndex]],
                "Microsoft YaHei", 55, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            mGroupAtIndex++;

            if (mGroupAtIndex ==
                mRealOrder[mGrandGroupAtIndex].Count)//end of small group
            {
                mfNext = showHMInterface;
                mGroupAtIndex = 0;

                Timer t = new Timer();
                t.Elapsed += new ElapsedEventHandler(t_Elapsed2ASecond);
                t.Interval = 1000;
                t.AutoReset = false;
                t.Enabled = true;
            }
            else
            {
                Timer t = new Timer();
                t.Elapsed += new ElapsedEventHandler(t_Elapsed2QuaterSecond);
                t.Interval = 1000;
                t.AutoReset = false;
                t.Enabled = true;
            }
        }

        private void t_Elapsed2ASecond(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(oneSecBlackScreen));
        }

        private void t_Elapsed2QuaterSecond(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(quaterSecBlackScreen));
        }

        private void showHMInterface()
        {
            SubPageOrderOSPrac subPage = new SubPageOrderOSPrac(mPage, this);
            mPage.ClearAll();
            mfNext = showFeedback;
            subPage.Show();
        }

        private void showFeedback()
        {
            CompCentralText ct = new CompCentralText();
            CompCentralText ct2 = new CompCentralText();
            //bool correctness = false;
            int correctCount = 0;


            for (int i = 0; i < mRealOrder[mGrandGroupAtIndex].Count; i++)
            {
                if(i < mAnswer.Count && mAnswer[i] == mRealOrder[mGrandGroupAtIndex][i])
                    correctCount++;
            }

            ct.PutTextToCentralScreen(
                "这组题（共" + mRealOrder[mGrandGroupAtIndex].Count + "个属相）中，您",
                    "KaiTi", 40, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            ct2.PutTextToCentralScreen(
                "记对了" + correctCount + "个属相",
                    "KaiTi", 40, ref mPage.mBaseCanvas, 42, Color.FromRgb(255, 255, 255));

            /*if (correctness)
            {
                ct.PutTextToCentralScreen(
                    "正确",
                    "KaiTi", 55, ref mPage.mBaseCanvas, 0, Color.FromRgb(0, 255, 0));
            }
            else
            {
                ct.PutTextToCentralScreen(
                    "错误",
                    "KaiTi", 55, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 0, 0));
            }*/

            mGrandGroupAtIndex++;

            if (mGrandGroupAtIndex ==
                mRealOrder.Count)
            {
                mfNext = mPage.nextStep;
            }
            else
            {
                mfNext = showAnimal;
            }

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed2ASecond);
            t.Interval = 2000;
            t.AutoReset = false;
            t.Enabled = true;
        }
    }
}

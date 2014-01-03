using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;

namespace FiveElementsIntTest.PairedAsso
{
    public class OrganizerTest
    {
        public PagePairedAsso mPage;
        public List<StTest> mSource;
        public int mCurAt = 0;
        public String mCharNum;
        private Stopwatch mWatch;
        //private CompOvertimeWarning mWarning;
        //private Timer mtWarn;
        private Timer mtFlipper = null;
        public CompCountDown mCountDown;
        

        public OrganizerTest(PagePairedAsso page, List<StTest> source, String charNum)
        {
            mPage = page;
            mSource = source;
            mCharNum = charNum;
            mWatch = new Stopwatch();

            mCountDown = new CompCountDown();
            mCountDown.Duration = 30;
            mCountDown.FunctionElapsed = next;
        }

        private void showCallingAttentionPage()
        {
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("记忆测试" + mCharNum,
                "KaiTi", 50, ref mPage.amBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            CompBtnNextPage btn = new CompBtnNextPage("开始");
            btn.Add2Page(mPage.amBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = showBlackPage;
        }

        public delegate void timedele();

        void goBlack_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(new timedele(showBlackPage), DispatcherPriority.Normal);
        }

        private void showBlackPage(object obj)
        {
            showBlackPage();
        }

        private void showBlackPage()
        {
            mPage.clearAll();
            mPage.amBaseCanvas.Children.Add(mCountDown);
            Canvas.SetTop(mCountDown, FEITStandard.PAGE_BEG_Y + 360);
            Canvas.SetLeft(mCountDown, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - 300) / 2 - 150);
            mCountDown.Visibility = System.Windows.Visibility.Hidden;

            Timer t = new Timer();
            t.Interval = 1000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(goNext_Elapsed);
            t.Enabled = true;
        }

        void goNext_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }

        public void Begin()
        {
            mCurAt = 0;
            showCallingAttentionPage();
        }

        private void next(object obj)
        {
            next();
        }

        private void next()
        {
            if (!mPage.mFreeze)
            {
                if (mWatch.IsRunning)
                {
                    mPage.mRTs.Add(mWatch.ElapsedMilliseconds);
                    mWatch.Stop();
                    mWatch.Reset();
                }

                mCountDown.Stop();
                mCountDown.Duration = 30;

                if (mtFlipper != null && mtFlipper.Enabled)
                    mtFlipper.Enabled = false;


                if (mCurAt == mSource.Count)
                {
                    mPage.next();
                }
                else
                {
                    show9CellsPad();
                    mCurAt++;
                }
            }
            else
            {
                mWatch.Stop();
            }
        }

        CompChinese9Cells mCompHold = null;

        private void show9CellsPad()
        {
            if(mCompHold != null)
                mPage.amBaseCanvas.Children.Remove(mCompHold);
            mCompHold = new CompChinese9Cells(mPage);

            mCompHold.SetQuest(mSource[mCurAt].Pair.First);
            mCompHold.SetCharas(mSource[mCurAt].Chars9);
            mCompHold.mfConfirm = confirmCheckNext;
            mPage.amBaseCanvas.Children.Add(mCompHold);
            Canvas.SetTop(mCompHold, FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT - 515) / 2 + 60);
            Canvas.SetLeft(mCompHold, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - 800) / 2);

            mPage.mOrders.Add(mCompHold.mSelectedOrder);
            mWatch.Start();

            mCountDown.Visibility = System.Windows.Visibility.Visible;
            
            if(!mPage.mFreeze)
                mCountDown.Start();
        }

        void confirmCheckNext(object obj)
        {
            CompChinese9Cells cc9 = (CompChinese9Cells)obj;
            if (cc9.mSelectedOrder.Count == 2)
            {
                next();
            }
        }

        void mtFlipper_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }
    }
}

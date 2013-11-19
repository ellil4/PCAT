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
    class OrganizerTest
    {
        public PagePairedAsso mPage;
        public List<StTest> mSource;
        public int mCurAt = 0;
        public String mCharNum;
        private Stopwatch mWatch;
        private CompOvertimeWarning mWarning;
        private Timer mtWarn;
        private Timer mtFlipper;
        

        public OrganizerTest(PagePairedAsso page, List<StTest> source, String charNum)
        {
            mPage = page;
            mSource = source;
            mCharNum = charNum;
            mWatch = new Stopwatch();

            mWarning = new CompOvertimeWarning(mPage);
            mPage.amBaseCanvas.Children.Add(mWarning);
            Canvas.SetTop(mWarning, FEITStandard.PAGE_BEG_Y + 500);
            Canvas.SetLeft(mWarning, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - 300) / 2);
        }

        private void showCallingAttentionPage()
        {
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("记忆测试" + mCharNum,
                "KaiTi", 50, ref mPage.amBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            /*Timer t = new Timer();
            t.Interval = 2000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(goBlack_Elapsed);
            t.Enabled = true;*/

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

        private void next()
        {
            if (mWatch.IsRunning)
            {
                mPage.mRTs.Add(mWatch.ElapsedMilliseconds);
                mWatch.Stop();
                mWatch.Reset();
            }

            if (mtWarn != null && mtWarn.Enabled)
                mtWarn.Enabled = false;

            mWarning.Out();

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

        private void show9CellsPad()
        {
            CompChinese9Cells comp = new CompChinese9Cells(mPage);

            comp.SetQuest(mSource[mCurAt].Pair.First);
            comp.SetCharas(mSource[mCurAt].Chars9);
            comp.mfConfirm = next;
            mPage.amBaseCanvas.Children.Add(comp);
            Canvas.SetTop(comp, FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT- 515) / 2);
            Canvas.SetLeft(comp, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - 800) / 2);

            mPage.mOrders.Add(comp.mSelectedOrder);
            mWatch.Start();

            mtWarn = new Timer();
            mtWarn.Interval = 10000;
            mtWarn.Elapsed += new ElapsedEventHandler(tWarn_Elapsed);
            mtWarn.AutoReset = false;
            mtWarn.Enabled = true;

            /*mtFlipper = new Timer();
            mtFlipper.Interval = 15000;
            mtFlipper.Elapsed += new ElapsedEventHandler(mtFlipper_Elapsed);
            mtFlipper.AutoReset = false;
            mtFlipper.Enabled = true;*/
        }

        void mtFlipper_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }

        void tWarn_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(showWarning));
        }

        void showWarning()
        {
            mWarning.Flashing();
        }
    }
}

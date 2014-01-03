using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Input;

namespace FiveElementsIntTest.PairedAsso
{
    public class OrganizerLearning
    {
        public PagePairedAsso mPage;
        List<StPair> mSource;
        public int mItemAt = 0;

        public OrganizerLearning(PagePairedAsso page, List<StPair> source)
        {
            mPage = page;
            mSource = source;
        }

        public void ShowCallingAttentionPage()
        {
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("下面开始呈现词对",
                "KaiTi", 32, ref mPage.amBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            CompCentralText ct2 = new CompCentralText();
            ct2.PutTextToCentralScreen("请注意记",
                "KaiTi", 32, ref mPage.amBaseCanvas, 150, Color.FromRgb(0, 255, 0));

            CompBtnNextPage btn = new CompBtnNextPage("开始");
            btn.Add2Page(mPage.amBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = showBlackPage;
        }

        public void Begin()
        {
            mItemAt = 0;
            ShowCallingAttentionPage();
        }

        private void showPair()
        {
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen(
                mSource[mItemAt].First + " - " + mSource[mItemAt].Second,
                "SimHei", 60, ref mPage.amBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            Timer t = new Timer();
            
            t.Interval = 3000;
            //systest
            //t.Interval = 10;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(showPair_Elapsed);
            t.Enabled = true;
        }

        public delegate void timedele();

        void showPair_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(showBlackPage));
        }

        private void showBlackPage()
        {
            mPage.clearAll();
            Timer t = new Timer();
            t.Interval = 2000;
            //systest
            //t.Interval = 10;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(showBlackPage_Elapsed);
            t.Enabled = true;
        }

        private void showBlackPage(object obj)
        {
            showBlackPage();
        }

        void showBlackPage_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }

        public void next()
        {
            if (mItemAt == mSource.Count)
            {
                mPage.Cursor = Cursors.Hand;
                mPage.next();
            }
            else
            {
                mPage.Cursor = Cursors.None;
                showPair();
                mItemAt++;
            }
        }
    }
}

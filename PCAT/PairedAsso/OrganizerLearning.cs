using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Timers;
using System.Windows.Threading;

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
            please pay attention to remember 2s
        }

        public void Begin()
        {
            mItemAt = 0;
            showPair();
        }

        private void showPair()
        {

            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen(
                mSource[mItemAt].First + "-" + mSource[mItemAt].Second,
                "KaiTi", 50, ref mPage.amBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            mItemAt++;

            Timer t = new Timer();
            t.Interval = 4000;
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
            t.Interval = 1000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(showBlackPage_Elapsed);
            t.Enabled = true;
        }

        void showBlackPage_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(showBlackPage));
        }

        public void next()
        {
            if (mItemAt == mSource.Count)
            {
                mPage.next();
            }
            else
            {
                showPair();
            }
        }
    }
}

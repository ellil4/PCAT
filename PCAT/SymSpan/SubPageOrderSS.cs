using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.SymSpan
{
    class SubPageOrderSS : SubPageOrder
    {
        private PageSymmSpan mPage;
        private OrganizerTrailSS mOrg;
        private static int EDGE_ELEM = CompNumCheckSS.OUTWIDTH - 2;

        public SubPageOrderSS(ref PageSymmSpan _page, OrganizerTrailSS org)
        {
            mPage = _page;
            mOrg = org;
            mCheckComponent = new UIGroupNumChecksSS();
        }

        private void doNothing()
        { }

        private int doNothingi()
        {
            return -1;
        }

        public override void TriBtnConfirm()
        {
            mOrg.groupStatistics();
            Timer tm = new Timer();
            tm.Interval = 500;
            tm.AutoReset = false;
            tm.Elapsed += new ElapsedEventHandler(tm_Elapsed);
            tm.Enabled = true;

            mTriBtns.mConfirmMethod = doNothing;
            mTriBtns.mClearMethod = doNothingi;
            mTriBtns.mBlankMethod = doNothing;
        }

        private delegate void timeDele();

        void tm_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(mOrg.nextStep));
        }
        
        public override void PutTriBtnToScreen(int xOff, int yOff)
        {
            mPage.mBaseCanvas.Children.Add(mTriBtns);
            Canvas.SetLeft(mTriBtns, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - CompTriBtns.OUTWIDTH) / 2 + xOff);
            Canvas.SetTop(mTriBtns, FEITStandard.PAGE_BEG_Y + yOff);
        }

        public override void PutNumCheckToScreen(int xOff, int yOff, 
            int xCount, int yCount, int width, int height)
        {
            for(int i = 0; i < mCheckComponent.mCheckComps.Count; i++)
            {
                mPage.mBaseCanvas.Children.Add(mCheckComponent.mCheckComps[i]);

                Canvas.SetTop(mCheckComponent.mCheckComps[i],
                    EDGE_ELEM * (i / xCount) + yOff + FEITStandard.PAGE_BEG_Y);
                Canvas.SetLeft(mCheckComponent.mCheckComps[i],
                    EDGE_ELEM * (i % xCount) + xOff + FEITStandard.PAGE_BEG_X);
            }
        }

        public override void Show()
        {
            mPage.mLayoutInstruction.addTitle(
                50, 0, "请按顺序回忆红点出现过的位置", "KaiTi", 32, Color.FromRgb(0, 255, 0));
            PutNumCheckToScreen(271, 160, 4, 4, 600, 240);
            PutTriBtnToScreen(0, 450);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public override void TriBtnConfirm()
        {
            mOrg.groupStatistics();
            mOrg.nextStep();
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
                50, 56, "***请按顺序回忆位置***", "KaiTi", 40, Color.FromRgb(255, 255, 255));
            PutNumCheckToScreen(271, 160, 4, 4, 600, 240);
            PutTriBtnToScreen(0, 450);
        }

    }
}

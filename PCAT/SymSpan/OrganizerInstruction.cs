using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FiveElementsIntTest.SymSpan
{
    class OrganizerInstruction
    {
        private delegate void OnShowFunc();
        private OnShowFunc mNextRun;
        private PageSymmSpan mPage;

        public OrganizerInstruction(PageSymmSpan page)
        {
            mPage = page;
            mNextRun = page1;
            next();
            mPage.mStatus = PageSymmSpan.StatusSS.practise;
        }

        private void next()
        {
            mNextRun();
        }

        private void page1()
        {
            mPage.ClearAll();
            LayoutInstruction li = new LayoutInstruction(ref mPage.mBaseCanvas);
            li.addTitle(200, 0, "指导页（测试页面）", "KaiTi", 31, Color.FromRgb(50, 255, 50));
            mNextRun = mPage.nextStep;
            new FEITClickableScreen(ref mPage.mBaseCanvas, next);
        }
    }
}

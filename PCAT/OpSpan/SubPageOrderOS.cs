using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using FiveElementsIntTest;
using System.Windows.Media;

namespace FiveElementsIntTest.OpSpan
{
    public class SubPageOrderOS : SubPageOrder
    {
        
        private PageOpSpan mPage;
        private OrganizerTrailOS mOrg;

        public SubPageOrderOS(ref PageOpSpan _page, OrganizerTrailOS org)
        {
            mPage = _page;
            mOrg = org;
            mCheckComponent = new UIGroupNumChecksOS();
        }

        public override void TriBtnConfirm()
        {
            //save result
            mOrg.mAnswer.Order = mCheckComponent.getAnswer();
            //call finish
            mOrg.groupStatistics();
            //call data iterate
            if (!mOrg.practiseMode())
            {
                //record
                mPage.mRecorder.orderOff.Add(mPage.mTimer.GetElapsedTime());
                string orderStr = "";
                int len = mOrg.mAnswer.Order.Count;
                int aniIdxbuf;
                for (int i = 0; i < len; i++)
                {
                    aniIdxbuf = mOrg.mAnswer.Order[i];

                    if (aniIdxbuf != -1)
                    {
                        orderStr += UIGroupNumChecksOS.LUNAR_ANI[aniIdxbuf];
                    }
                    else
                    {
                        orderStr += "无";
                    }
                }

                mPage.mRecorder.userInputOrder.Add(orderStr);

                mOrg.groupIterate();
            }
            //
            mPage.nextStep();
            //Console.WriteLine("Confirm Clickd");
        }

        public override void PutNumCheckToScreen(int xOff, int yOff,
            int xCount, int yCount, int width, int height)//over
        {
            int gapX =
                (width - CompNumCheckOS.OUTWIDTH * xCount) / (xCount - 1);

            int gapY =
                (height - CompNumCheckOS.OUTHEIGHT * yCount) / (yCount - 1);

            //arrange a layout
            for (int i = 0; i < mCheckComponent.mCheckComps.Count; i++)
            {
                mPage.mBaseCanvas.Children.Add((CompNumCheck)mCheckComponent.mCheckComps[i]);

                Canvas.SetLeft((CompNumCheck)mCheckComponent.mCheckComps[i],
                    FEITStandard.PAGE_BEG_X + (i % xCount) * (gapX + CompNumCheckOS.OUTWIDTH) + xOff);

                Canvas.SetTop((CompNumCheck)mCheckComponent.mCheckComps[i],
                    FEITStandard.PAGE_BEG_Y + (i / xCount) * (gapY + CompNumCheckOS.OUTHEIGHT) + yOff);
            }
        }

        public override void PutTriBtnToScreen(int xOff, int yOff)
        {
            mPage.mBaseCanvas.Children.Add(mTriBtns);
            Canvas.SetLeft(mTriBtns, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - CompTriBtns.OUTWIDTH) / 2 + xOff);
            Canvas.SetTop(mTriBtns, FEITStandard.PAGE_BEG_Y + yOff);
        }

        public override void Show()//over
        {
            mPage.mLayoutInstruction.addTitle(
                50, 63, "***请按顺序回忆属相***", "KaiTi", 40, Color.FromRgb(255, 255, 255));
            PutNumCheckToScreen(0, 160, 4, 3, 800, 240);
            PutTriBtnToScreen(0, 450);
        }
    }
}

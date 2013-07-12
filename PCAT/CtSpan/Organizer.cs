using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FiveElementsIntTest.CtSpan
{
    public class Organizer
    {
        public PageCtSpan mPage;
        public GraphControl mGC;
        public GraphConfirmBtn mConfirmBtn;
        public CompCountRecallPan mPan;

        public Organizer(PageCtSpan page)
        {
            mPage = page;
            mGC = new GraphControl(mPage);
            mPan = new CompCountRecallPan();

            mConfirmBtn = new GraphConfirmBtn();
        }

        public void ShowGraph(StGraphItem item)
        {
            //graph
            mGC.Clear();
            mGC.DrawScene(item);
            //button
            mPage.amBaseCanvas.Children.Add(mConfirmBtn);
            Canvas.SetTop(mConfirmBtn, FEITStandard.PAGE_BEG_Y + 530);
            Canvas.SetLeft(mConfirmBtn, FEITStandard.PAGE_BEG_X + 730);
        }

        public void ShowCountingPan()
        {
            mPage.ClearAll();
            mPan.Clear();
            mPage.amBaseCanvas.Children.Add(mPan);
            Canvas.SetTop(mPan, FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(mPan, FEITStandard.PAGE_BEG_X);
        }

        public void ShowWarning()
        {
            mPage.ClearAll();
            CompWarning cw = new CompWarning();
            mPage.amBaseCanvas.Children.Add(cw);
            Canvas.SetTop(cw, FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(cw, FEITStandard.PAGE_BEG_X);
        }

        public void ShowTrialTitle()
        {
            String text = mPage.SpanArrangement[mPage.mTotalSpanIndexAt] + 
                "题第" + (mPage.mGrpAt + 1).ToString() + "组";

            mPage.ClearAll();
            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen(
                text, "KaiTi", 50, ref mPage.amBaseCanvas, 0,
                System.Windows.Media.Color.FromRgb(0, 255, 0));
        }

        public void ShowQuitPage()
        {
            String text = "本次测试结束，程序将自动退出";

            if (mPage.mTotalSpanIndexAt == 27)
                text = "全做完了？难道你是杜新吗？程序将自动退出。";

            mPage.ClearAll();
            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen(
                text, "KaiTi", 44, ref mPage.amBaseCanvas, 0,
                System.Windows.Media.Color.FromRgb(0, 255, 0));
        }

        public void ShowBlackScreen()
        {
            mPage.ClearAll();
        }

        public void ShowInstruction()
        {
            mPage.ClearAll();
            CompInstruction ci = new CompInstruction();
            ci.amGraphConfirmBtn.mfDo = mPage.NextStage;
            mPage.amBaseCanvas.Children.Add(ci);
            Canvas.SetTop(ci, FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(ci, FEITStandard.PAGE_BEG_X);
        }

        public void ShowInstruction2()
        {
            mPage.ClearAll();
            CompInstruction2 ci = new CompInstruction2();
            ci.amGraphConfirmBtn.mfDo = mPage.NextStage;
            mPage.amBaseCanvas.Children.Add(ci);
            Canvas.SetTop(ci, FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(ci, FEITStandard.PAGE_BEG_X); 
        }

        public void ShowResultBoard(int totalCount, int correctCount)
        {
            mPage.ClearAll();
            CompResultReport crr =
                new CompResultReport(totalCount, correctCount);
            mPage.amBaseCanvas.Children.Add(crr);
            Canvas.SetTop(crr, FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(crr, FEITStandard.PAGE_BEG_X);
        }

        public void ShowTestTitle()
        {
            mPage.ClearAll();
            CompTitle crr =
                new CompTitle();
            crr.amGraphConfirmBtn.mfDo = mPage.NextStage;
            mPage.amBaseCanvas.Children.Add(crr);
            Canvas.SetTop(crr, FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(crr, FEITStandard.PAGE_BEG_X);
        }
    }
}

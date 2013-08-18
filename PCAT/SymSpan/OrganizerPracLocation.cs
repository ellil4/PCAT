using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace FiveElementsIntTest.SymSpan
{
    public class OrganizerPracLocation
    {
        public PageSymmSpan mPage;
        public UIGroupNumChecksSS mComp;
        public CompTriBtns mTriBtns;
        public List<List<int>> mLocations;
        public int mItemAt = 0;
        public int mGrpAt = 0;
        private static int EDGE_ELEM = CompNumCheckSS.OUTWIDTH - 2;
        private Timer mTimer;

        public OrganizerPracLocation(PageSymmSpan page, List<List<int>> locations)
        {
            mPage = page;
            mLocations = locations;
            mTriBtns = new CompTriBtns();
        }

        private void putNumCheckToScreen(int xOff, int yOff,
                int xCount, int yCount, int width, int height)
        {
            for (int i = 0; i < mComp.mCheckComps.Count; i++)
            {
                mPage.mBaseCanvas.Children.Add(mComp.mCheckComps[i]);

                Canvas.SetTop(mComp.mCheckComps[i],
                    EDGE_ELEM * (i / xCount) + yOff + FEITStandard.PAGE_BEG_Y);
                Canvas.SetLeft(mComp.mCheckComps[i],
                    EDGE_ELEM * (i % xCount) + xOff + FEITStandard.PAGE_BEG_X);
            }
        }

        private void putTriBtnToScreen(int xOff, int yOff)
        {
            mPage.mBaseCanvas.Children.Add(mTriBtns);
            Canvas.SetLeft(mTriBtns, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - CompTriBtns.OUTWIDTH) / 2 + xOff);
            Canvas.SetTop(mTriBtns, FEITStandard.PAGE_BEG_Y + yOff);
        }

        private void putPicAtCanvas(string picname)
        {
            System.Windows.Controls.Image img_ctrl = new System.Windows.Controls.Image();

            //uri resource loading
            Uri uriimage = new Uri(LoaderSymmSpan.GetBaseFolder() + "SYMM\\" +
                picname);

            //image 
            BitmapImage img = new BitmapImage(uriimage);

            //set to control
            img_ctrl.Source = img;
            img_ctrl.Width = 600;
            img_ctrl.Height = 450;

            mPage.mBaseCanvas.Children.Add(img_ctrl);
            Canvas.SetTop(img_ctrl, FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT - img_ctrl.Height) / 2);
            Canvas.SetLeft(img_ctrl, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - img_ctrl.Width) / 2);
        }

        void showInstruction1()
        {
            mPage.ClearAll();
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("下面再来练习一下图形对称判断，",
                "KaiTi", 30, ref mPage.mBaseCanvas, -40, Color.FromRgb(255, 255, 255));

            CompCentralText ct2 = new CompCentralText();
            ct2.PutTextToCentralScreen("先了解一下怎样判断本任务中的图形是否对称。",
                "KaiTi", 30, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));
            
            new FEITClickableScreen(ref mPage.mBaseCanvas, showInstruction2);
        }

        void showInstruction2()
        {
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "左右对折后两侧图形可以重合，是对称的。", "KaiTi", 30, ref mPage.mBaseCanvas,
                300, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompCentralText text2 = new CompCentralText();
            text2.PutTextToCentralScreen(
                "点鼠标继续", "KaiTi", 30, ref mPage.mBaseCanvas,
                -210, System.Windows.Media.Color.FromRgb(255, 255, 255));

            putPicAtCanvas("symmInstruction.bmp");
            new FEITClickableScreen(ref mPage.mBaseCanvas, showInstruction3);
        }

        void showInstruction3()
        {
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "左右对折后两侧图形不能重合，是不对称的。", "KaiTi", 30, ref mPage.mBaseCanvas,
                300, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompCentralText text2 = new CompCentralText();
            text2.PutTextToCentralScreen(
                "点鼠标继续", "KaiTi", 30, ref mPage.mBaseCanvas,
                -210, System.Windows.Media.Color.FromRgb(255, 255, 255));

            putPicAtCanvas("insymmInstruction.bmp");
            new FEITClickableScreen(ref mPage.mBaseCanvas, showInstruction4);
        }

        void showInstruction4()
        {
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "下面开始练习判断图形是否对称", "KaiTi", 30, ref mPage.mBaseCanvas,
                0, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompCentralText text3 = new CompCentralText();
            text3.PutTextToCentralScreen(
                "在看完每幅图片后，请尽快单击鼠标左键。", "KaiTi", 30, ref mPage.mBaseCanvas,
                50, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompCentralText text2 = new CompCentralText();
            text2.PutTextToCentralScreen(
                "然后判断给出的图形是否对称。", "KaiTi", 30, ref mPage.mBaseCanvas,
                100, System.Windows.Media.Color.FromRgb(255, 255, 255));

            new FEITClickableScreen(ref mPage.mBaseCanvas, showBlankMask);
        }

        void showBlankMask()
        {
            Timer t = new Timer();
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed2);
        }

        void t_Elapsed2(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(mPage.nextStep));
        }

        public void next()
        {
            //Console.WriteLine("delegate called");
            if (mGrpAt < mLocations.Count && mItemAt < mLocations[mGrpAt].Count)
            {
                ShowPos(mLocations[mGrpAt][mItemAt]);
                mItemAt++;
            }
            else if (mGrpAt < mLocations.Count && mItemAt >= mLocations[mGrpAt].Count)
            {
                mItemAt = 0;
                mGrpAt++;
                ShowOrder();
            }
            else
            {
                showInstruction1();
            }
        }

        public void ShowInform()
        {
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "这组位置（共" + mLocations[mGrpAt - 1].Count + "个）", "KaiTi", 30, ref mPage.mBaseCanvas,
                -25, System.Windows.Media.Color.FromRgb(255, 255, 255));

            //correct count
            int correctCount = 0;
            if (mComp.mOrder.Count == mLocations[mGrpAt - 1].Count)
            {
                for (int i = 0; i < mLocations[mGrpAt - 1].Count; i++)
                {
                    if (mComp.mOrder[i] == mLocations[mGrpAt - 1][i])
                    {
                        correctCount++;
                    }
                }
            }

            CompCentralText text2 = new CompCentralText();
            text2.PutTextToCentralScreen(
                "您记对了" + correctCount + "个", "KaiTi", 30, ref mPage.mBaseCanvas,
                25, System.Windows.Media.Color.FromRgb(255, 255, 255));

            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(mTimer_Elapsed);
            mTimer.AutoReset = false;
            mTimer.Interval = 2000;
            mTimer.Enabled = true;
        }

        private delegate void timedele();

        void mTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }

        public void ShowOrder()
        {
            mComp = new UIGroupNumChecksSS();
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "**请按顺序回忆方块出现过的位置**", "KaiTi", 30, ref mPage.mBaseCanvas,
                -200, System.Windows.Media.Color.FromRgb(255, 255, 255));

            mComp.setPositionMode(false);
            //mComp.setMarked(-1);
            mComp.reset();
            putNumCheckToScreen(280, 160, 4, 4, 600, 240);
            putTriBtnToScreen(0, 450);

            mTriBtns.mBlankMethod = mComp.jumpOver;
            mTriBtns.mClearMethod = mComp.backErase;
            mTriBtns.mConfirmMethod = ShowInform;
        }

        public void ShowPos(int pos)
        {
            mComp = new UIGroupNumChecksSS();
            mPage.ClearAll();
            mComp.setPositionMode(true);
            mComp.setMarked(pos);
            putNumCheckToScreen(280, 160, 4, 4, 600, 240);

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "点鼠标继续", "KaiTi", 30, ref mPage.mBaseCanvas,
                -200, System.Windows.Media.Color.FromRgb(255, 255, 255));

            new FEITClickableScreen(ref mPage.mBaseCanvas, 
                new FEITClickableScreen.clickableAreaReaction(next));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Input;

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
        public Stopwatch mWatch;
        public RecorderSymSpan mRecorder;

        public OrganizerPracLocation(PageSymmSpan page, List<List<int>> locations, RecorderSymSpan rec)
        {
            mPage = page;
            mLocations = locations;
            mTriBtns = new CompTriBtns();
            mWatch = new Stopwatch();
            mRecorder = rec;
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

        void showInstruction1(object obj)
        {
            mPage.ClearAll();
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("下面再来练习一下图形对称判断，",
                "KaiTi", 30, ref mPage.mBaseCanvas, -40, Color.FromRgb(255, 255, 255));

            CompCentralText ct2 = new CompCentralText();
            ct2.PutTextToCentralScreen("先了解一下怎样判断\r\n本任务中的图形是否对称。",
                "KaiTi", 30, ref mPage.mBaseCanvas, 40, Color.FromRgb(255, 255, 255));
            
           // new FEITClickableScreen(ref mPage.mBaseCanvas, showInstruction2);
            CompBtnNextPage btn = new CompBtnNextPage("下一页");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = showInstruction2;
        }

        void showInstruction2(object obj)
        {
            mPage.ClearAll();
            //abc
            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "         后两侧图形可以重合，是对称的。", "KaiTi", 30, ref mPage.mBaseCanvas,
                300, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompCentralText text0 = new CompCentralText();
            text0.PutTextToCentralScreen(
                "左右对折                             ", "KaiTi", 30, ref mPage.mBaseCanvas,
                300, System.Windows.Media.Color.FromRgb(0, 255, 0));

            putPicAtCanvas("symmInstruction.bmp");

            CompBtnNextPage btn = new CompBtnNextPage("上一页");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470, -70);
            btn.mfOnAction = showInstruction1;

            CompBtnNextPage btn2 = new CompBtnNextPage("下一页");
            btn2.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470, 70);
            btn2.mfOnAction = showInstruction3;
        }

        void showInstruction3(object obj)
        {
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "         后两侧图形不能重合，是不对称的。", "KaiTi", 30, ref mPage.mBaseCanvas,
                300, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompCentralText text0 = new CompCentralText();
            text0.PutTextToCentralScreen(
                "左右对折                                ", "KaiTi", 30, ref mPage.mBaseCanvas,
                300, System.Windows.Media.Color.FromRgb(0, 255, 0));

            putPicAtCanvas("insymmInstruction.bmp");
            CompBtnNextPage btn = new CompBtnNextPage("上一页");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470, -70);
            btn.mfOnAction = showInstruction2;

            CompBtnNextPage btn2 = new CompBtnNextPage("下一页");
            btn2.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470, 70);
            btn2.mfOnAction = showInstruction4;
        }

        void showInstruction4(object obj)
        {
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "下面练习判断图形是否对称", "KaiTi", 30, ref mPage.mBaseCanvas,
                -100, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompCentralText text3 = new CompCentralText();
            text3.PutTextToCentralScreen(
                "在看完每幅图片后，请尽快单击           ", "KaiTi", 30, ref mPage.mBaseCanvas,
                -50, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompBtnNextPage btnOKexap = new CompBtnNextPage("看好了");
            btnOKexap.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + FEITStandard.PAGE_HEIGHT / 2
                - CompCentralText.OUTHEIGHT / 2 - 55, 200);
            btnOKexap.mfOnAction = doNothing;

            CompCentralText text2 = new CompCentralText();
            text2.PutTextToCentralScreen(
                "然后判断给出的图形是否对称。", "KaiTi", 30, ref mPage.mBaseCanvas,
                0, System.Windows.Media.Color.FromRgb(255, 255, 255));

            //new FEITClickableScreen(ref mPage.mBaseCanvas, showBlankMask);
            CompBtnNextPage btn2 = new CompBtnNextPage("开始练习");
            btn2.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470, 0);
            btn2.mfOnAction = showBlankMask;
        }

        void doNothing(object obj)
        { }

        void showBlankMask(object obj)
        {
            mPage.ClearAll();
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
                showInstruction1(null);
            }
            
        }

        public void ShowInform()
        {
            mRecorder.pracPosRTs.Add(mWatch.ElapsedMilliseconds);
            mWatch.Stop();
            mWatch.Reset();

            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "这组位置（共" + mLocations[mGrpAt - 1].Count + "个）", "KaiTi", 30, ref mPage.mBaseCanvas,
                -25, System.Windows.Media.Color.FromRgb(255, 255, 255));

            //correct count
            int correctCount = 0;
            string pospos = "";
            string userpos = "";

            for (int x = 0; x < mComp.mOrder.Count; x++)
            {
                userpos += mComp.mOrder[x].ToString();
                if (x != mComp.mOrder.Count - 1)
                    userpos += ",";
            }
            mRecorder.pracPosUserSel.Add(userpos);

            for (int y = 0; y < mLocations[mGrpAt - 1].Count; y++)
            {
                pospos += mLocations[mGrpAt - 1][y].ToString();
                if (y != mLocations[mGrpAt - 1].Count - 1)
                    pospos += ",";
            }
            mRecorder.pracPosPos.Add(pospos);

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
                "你记对了" + correctCount + "个", "KaiTi", 30, ref mPage.mBaseCanvas,
                25, System.Windows.Media.Color.FromRgb(255, 255, 255));

            if (correctCount == mLocations[mGrpAt - 1].Count)
            {
                mRecorder.pracPosCorrect.Add(true);
            }
            else
            {
                mRecorder.pracPosCorrect.Add(false);
            }

            mTimer = new Timer();
            mTimer.Elapsed += new ElapsedEventHandler(resultBlankMask1000);
            mTimer.AutoReset = false;
            mTimer.Interval = 2000;
            mTimer.Enabled = true;
        }

        private delegate void timedele();

        void mTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }

        public void resultBlankMask1000(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(mPage.ClearAll));
            Timer tm = new Timer();
            tm.Elapsed += new ElapsedEventHandler(mTimer_Elapsed);
            tm.AutoReset = false;
            tm.Interval = 1000;
            tm.Enabled = true;
        }

        public void ShowOrder()
        {
            Mouse.OverrideCursor = Cursors.Hand;
            mComp = new UIGroupNumChecksSS();
            mPage.ClearAll();

            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "请按顺序回忆红点出现过的位置", "KaiTi", 32, ref mPage.mBaseCanvas,
                -200, System.Windows.Media.Color.FromRgb(0, 255, 0));

            mComp.setPositionMode(false);
            //mComp.setMarked(-1);
            mComp.reset();
            putNumCheckToScreen(280, 160, 4, 4, 600, 240);
            putTriBtnToScreen(0, 450);

            mTriBtns.mBlankMethod = mComp.jumpOver;
            mTriBtns.mClearMethod = mComp.backErase;
            mTriBtns.mConfirmMethod = ShowInform;

            mWatch.Start();
        }

        public void ShowPos(int pos)
        {
            Mouse.OverrideCursor = Cursors.None;
            mPage.ClearAll();

            mComp = new UIGroupNumChecksSS();
            mComp.mTouchActivated = false;
            mComp.setPositionMode(true);
            mComp.setMarked(pos);
            putNumCheckToScreen(280, 160, 4, 4, 600, 240);

            Timer posTm = new Timer();
            posTm.Interval = 1000;
            posTm.AutoReset = false;
            posTm.Elapsed += new ElapsedEventHandler(posTm_Elapsed);
            posTm.Enabled = true;
        }

        void posTm_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(clickBlankMask500));
        }

        void clickBlankMask500()
        {
            mPage.ClearAll();

            mComp = new UIGroupNumChecksSS();
            mComp.mTouchActivated = false;
            mComp.setPositionMode(true);
            putNumCheckToScreen(280, 160, 4, 4, 600, 240);

            Timer t = new Timer();
            t.Interval = 500;
            t.AutoReset = false;
            t.Enabled = true;
            t.Elapsed += new ElapsedEventHandler(clickt_Elapsed);
        }

        void clickt_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }
    }
}

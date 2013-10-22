using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;

namespace FiveElementsIntTest.SymSpan
{
    public class OrganizerPracSymm
    {
        public PageSymmSpan mPage;
        public List<TrailSS_ST> mSymmItems;
        public int mAt = 0;
        public Stopwatch mWatch;
        public List<long> mRTs;
        public long mCurRT = 0;

        public OrganizerPracSymm(PageSymmSpan page, List<TrailSS_ST> symms)
        {
            mPage = page;
            mSymmItems = symms;
            mWatch = new Stopwatch();
            mRTs = new List<long>();
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

        public void ShowPic()
        {
            mWatch.Stop();
            mWatch.Reset();
            mWatch.Start();
            if (mAt < mSymmItems.Count)
            {
                mPage.ClearAll();

                putPicAtCanvas(mSymmItems[mAt].FileName);

                CompBtnNextPage btn = new CompBtnNextPage("看好了");
                btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
                btn.mfOnAction = blankMask200withShowJudge;
            }
        }

        void blankMask200withShowJudge(object obj)
        {
            mPage.ClearAll();
            Timer tm = new Timer();
            tm.Interval = 200;
            tm.AutoReset = false;
            tm.Elapsed += new ElapsedEventHandler(tm_Elapsed);
            tm.Enabled = true;
        }

        delegate void timeDele();

        void tm_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(ShowJudge));
        }

        public void ShowJudge()
        {
            mCurRT = mWatch.ElapsedMilliseconds;
            mRTs.Add(mCurRT);
            mPage.ClearAll();
            CompDualDetermine dualPad = new CompDualDetermine();

            dualPad.setButtonText("是", "否");
            //dualPad.setCorrectness(mSymmItems[mAt].IsSymm);
            dualPad.setResult("");
            dualPad.HideCorrecteness(true);
            dualPad.mConfirmMethod = confirmPressed;
            dualPad.mDenyMethod = denyPressed;

            //dualPad.BorderThickness = new Thickness(1.0);
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("是否对称", "KaiTi", 45,
                ref mPage.mBaseCanvas, -130, Color.FromRgb(255, 255, 255));

            mPage.mBaseCanvas.Children.Add(dualPad);
            Canvas.SetTop(dualPad, FEITStandard.PAGE_BEG_Y +
                (FEITStandard.PAGE_HEIGHT - CompDualDetermine.OUTHEIGHT) / 2);
            Canvas.SetLeft(dualPad, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - CompDualDetermine.OUTWIDTH) / 2);
        }

        private void doNothing(CompDualDetermine self)
        { }

        private void confirmPressed(CompDualDetermine self)
        {
            self.HideCorrecteness(false);
            if (mSymmItems[mAt].IsSymm)
            {
                self.setCorrectness(true);
            }
            else
            {
                self.setCorrectness(false);
            }

            mAt++;

            Timer t = new Timer();
            t.Interval = 2000;
            t.AutoReset = false;
            t.Enabled = true;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);

            self.mConfirmMethod = doNothing;
            self.mDenyMethod = doNothing;
        }

        private void denyPressed(CompDualDetermine self)
        {
            self.HideCorrecteness(false);
            if (mSymmItems[mAt].IsSymm)
            {
                self.setCorrectness(false);
            }
            else
            {
                self.setCorrectness(true);
            }

            mAt++;

            Timer t = new Timer();
            t.Interval = 2000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
            

            self.mConfirmMethod = doNothing;
            self.mDenyMethod = doNothing;
        }

        private delegate void timedele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(mPage.ClearAll));
            Timer tmNext = new Timer();
            tmNext.Interval = 500;
            tmNext.AutoReset = false;
            tmNext.Elapsed += new ElapsedEventHandler(tmNext_Elapsed);
            tmNext.Enabled = true; 
        }

        void tmNext_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(next));
        }

        void showInstructionPage()
        {
            mPage.ClearAll();
            CompCentralText text = new CompCentralText();
            text.PutTextToCentralScreen(
                "下面练习同时完成这两个任务", "KaiTi", 30, ref mPage.mBaseCanvas,
                -100, System.Windows.Media.Color.FromRgb(255, 255, 255));

            
            CompCentralText text2 = new CompCentralText();
            text2.PutTextToCentralScreen(
                "请在做对称判断的同时", "KaiTi", 30, ref mPage.mBaseCanvas,
                0, System.Windows.Media.Color.FromRgb(255, 255, 255));

            
            CompCentralText text3 = new CompCentralText();
            text3.PutTextToCentralScreen(
                "记住随后出现的红点的位置", "KaiTi", 30, ref mPage.mBaseCanvas,
                100, System.Windows.Media.Color.FromRgb(255, 255, 255));

            CompBtnNextPage btn = new CompBtnNextPage("开始练习");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = go2PageNextStep;
        }

        private void go2PageNextStep(object obj)
        {
            mPage.nextStep();
        }

        public void next()
        {
            if (mAt < mSymmItems.Count)
            {
                ShowPic();
            }
            else
            {
                showInstructionPage(); 
            }
        }
    }
}

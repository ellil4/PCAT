using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FiveElementsIntTest;
using System.Windows.Media;
using System.Windows;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Input;

namespace FiveElementsIntTest.SymSpan
{
    public class OrganizerTrailSS
    {
        private PageSymmSpan mPage;
        public bool mPractise;
        private List<TrailsGroupSS> mContent;
        
        private int mItemAt = 0;
        private int mGrpAt = 0;//systest disabled

        private int mCurSpanGroupsCount = 0;
        private int mCurSpanAt = 0;

        private int mSymmCorrectCount = 0;
        private int mSymmCorrectWarningMark = 0;
        private int mOrderCorrectCount = 0;

        private int mOrderErrorACC = 0;
        private int mSymmErrorACC = 0;

        public delegate void OrgRoute();
        public delegate void timedele();
        public OrgRoute mfRoute;
        private bool mShowWarning = false;

        private List<AnswerSSST> mAnswer;
        private AnswerSSST mCurAnswer;
        private SubPageOrderSS mOrderPage;

        private bool mbDead = false;

        public OrganizerTrailSS(PageSymmSpan _page, bool isPractise, 
            List<TrailsGroupSS> content, ref List<AnswerSSST> answer)
        {
            mContent = content;
            mPage = _page;
            mPractise = isPractise;
            mCurAnswer = new AnswerSSST();

            if (mPractise)
            {
                mfRoute = showSymmPage;
            }
            else
            {
                mfRoute = showTitlePage;
            }

            mAnswer = answer;

            mOrderPage = new SubPageOrderSS(ref mPage, this);

            mCurSpanGroupsCount = getCurSpanGroupsCount();
        }

        private int getCurSpanGroupsCount()
        {
            int retval = 0;
            if (mGrpAt < mContent.Count)
            {
                int size = mContent[mGrpAt].Trails.Count;
                for (int i = 0; i < mContent.Count; i++)
                {
                    if (mContent[i].Trails.Count == size)
                    {
                        retval++;
                    }
                }
            }

            return retval;
        }

        public void groupStatistics()
        {
            //record
            mPage.mRecorder.posOffTime.Add(mPage.mTimer.GetElapsedTime());
            //symm answers` statistics
            int symErrCompare = mSymmErrorACC;
            if(mGrpAt != 2 && mGrpAt != 3)//addtional trails not count in
                mSymmErrorACC += mContent[mGrpAt].Trails.Count - mSymmCorrectCount;

            if (mSymmErrorACC - symErrCompare > 1)
            {
                mShowWarning = true;
            }
            
            //sudden death
            bool everWrong = false;

            List<int> userInputOrder = new List<int>();

            //order answers` statistics            
            for (int i = 0; i < mContent[mGrpAt].Trails.Count; i++)
            {
                //keep record
                if (!mPractise)
                    mCurAnswer.Order.Add(mContent[mGrpAt].Trails[i].Position);

                //correctness
                if (mOrderPage.mCheckComponent.mOrder.Count > i)
                {
                    if (mContent[mGrpAt].Trails[i].Position ==
                        mOrderPage.mCheckComponent.mOrder[i])
                    {
                        if (!mPractise)
                            mCurAnswer.OrderCorrectness.Add(true);

                        mOrderCorrectCount++;
                    }
                    else
                    {
                        if (!mPractise)
                            mCurAnswer.OrderCorrectness.Add(false);

                        everWrong = true;
                    }
                }
                else
                {
                    if (!mPractise)
                        mCurAnswer.OrderCorrectness.Add(false);

                    everWrong = true;
                }
            }

            //sudden death
            if (everWrong)
                mOrderErrorACC++;

            //routing

            if (mSymmCorrectWarningMark > 1)
            {
                mShowWarning = true;
                mSymmCorrectWarningMark = 0;
            }

            //record
            mPage.mRecorder.shownPosition.Add(mCurAnswer.Order);
            List<int> bufferOrder = new List<int>();
            for (int i = 0; i < mOrderPage.mCheckComponent.mOrder.Count; i++)
            {
                bufferOrder.Add(mOrderPage.mCheckComponent.mOrder[i]);
            }
            mPage.mRecorder.userSelPosition.Add(bufferOrder);
            mPage.mRecorder.posCorrectness.Add(!everWrong);

            //routing
            if (mPractise)
            {
                //route
                mfRoute = showReportPage;
            }
            else//not practise
            {
                //mRecorder.elementInArray.Add(mContent[mGrpAt].Trails.Count);

                //routing
                if (mShowWarning)
                {
                    mfRoute = showReportPage;
                }
                else
                {
                    mfRoute = showTitlePage;
                    groupIterate();
                }

                
            }
        }

        public void testEnd()
        {
            mPage.mRecorder.outputReport(FEITStandard.GetRepotOutputPath() + "symm\\" + mPage.interFilename,
                    FEITStandard.GetRepotOutputPath() + "symm\\" + mPage.posFilename,
                    FEITStandard.GetRepotOutputPath() + "symm\\" + mPage.pracSymmFilename,
                    FEITStandard.GetRepotOutputPath() + "symm\\" + mPage.pracPosFilename);
            mfRoute = mPage.finish;
            mPage.nextStep();
        }

        public void groupIterate()
        {
            mGrpAt++;
            mItemAt = 0;
            mSymmCorrectCount = 0;
            mSymmCorrectWarningMark = 0;
            mOrderCorrectCount = 0;

            if (mCurSpanAt == mCurSpanGroupsCount - 1)//end of span
            {
                if (!mPractise)
                {
                    if (mOrderErrorACC > 1 && mGrpAt - 1 >= 4)
                    {
                        mbDead = true;
                    }
                    else if (mOrderErrorACC > 3 && mGrpAt - 1 < 4)
                    {
                        mbDead = true;
                    }

                    if ((((float)mSymmErrorACC /
                        (float)(mCurSpanGroupsCount * mPage.mTestGroupScheme[mGrpAt - 1])) > 0.2) &&
                        mGrpAt - 1 >= 4)
                    {
                        mbDead = true;
                    }
                }

                mCurSpanGroupsCount = getCurSpanGroupsCount();
                mOrderErrorACC = 0;
                //mSymmErrorACC = 0;
                mCurSpanAt = 0;
            }
            else
            {
                mCurSpanAt++;
            }



            //length2 addtional 2 items
            if (mGrpAt == 2 && !mPractise)
            {
                /*int posErrCount = 0;
                for (int i = 0; i < mPage.mRecorder.posCorrectness.Count; i++)
                {
                    if (mPage.mRecorder.posCorrectness[i] == false)
                        posErrCount++;
                }*/

                /*int symmErrCount = 0;
                for (int j = 0; j < mRecorder.symmJudgeCorrectness.Count; j++)
                {
                    if (mRecorder.symmJudgeCorrectness[j] == false)
                        symmErrCount++;
                }*/

                if (mPage.mRecorder.posCorrectness[2] == true ||
                    mPage.mRecorder.posCorrectness[3] == true)//not gonna have extra
                {
                    mGrpAt += 2;
                    mCurSpanGroupsCount = getCurSpanGroupsCount();
                    mCurSpanAt = 0;
                    mOrderErrorACC = 0;
                    //mSymmErrorACC = 0;
                }
            }

            if (mGrpAt == mContent.Count && !mPractise)
            {
                mfRoute = testEnd;
            }
            else if (mGrpAt == mContent.Count && mPractise)
            {
                mfRoute = mPage.nextStep;
            }

            if (mbDead)
                mfRoute = testEnd;

            //new answer struct
            mAnswer.Add(mCurAnswer);
            mCurAnswer = new AnswerSSST();

            mOrderPage.mCheckComponent.reset();
        }

        public void nextStep()
        {
            mfRoute();
        }

        public void nextStep(object obj)
        {
            mfRoute();
        }
        
        public void showOrderPage()
        {
            Mouse.OverrideCursor = Cursors.Hand;
            mPage.ClearAll();
            mOrderPage = new SubPageOrderSS(ref mPage, this);
            mOrderPage.Show();
            mPage.mRecorder.posOnTime.Add(mPage.mTimer.GetElapsedTime());

            //wait for order`s signal
        }

        Timer mSymmPageFlipper;
        //Timer mIKnowBtnShower;

        public void showSymmPage()
        {
            Mouse.OverrideCursor = Cursors.Hand;
            mPage.ClearAll();

            //control component
            System.Windows.Controls.Image img_ctrl = new System.Windows.Controls.Image();

            //uri resource loading
            Uri uriimage = new Uri(LoaderSymmSpan.GetBaseFolder() + "SYMM\\" +
                mContent[mGrpAt].Trails[mItemAt].FileName);

            //image 
            BitmapImage img = new BitmapImage(uriimage);

            //set to control
            img_ctrl.Source = img;
            img_ctrl.Width = 600;
            img_ctrl.Height = 450;

            mPage.mBaseCanvas.Children.Add(img_ctrl);
            Canvas.SetTop(img_ctrl, FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT - img_ctrl.Height) / 2 - 50);
            Canvas.SetLeft(img_ctrl, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - img_ctrl.Width) / 2);

            mfRoute = showDualDeterPage;

            if (!mPractise)
            {
                arrangeAutoIKnowAndAutoFlipperInSymmPage();
            }
            else
            {
                if (mGrpAt == 0)
                {
                    CompBtnNextPage btn = new CompBtnNextPage("看好了");
                    btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 400);
                    btn.mfOnAction = nextStep;
                }
                else
                {
                    arrangeAutoIKnowAndAutoFlipperInSymmPage();
                }
            }

            //record
            mPage.mRecorder.symmOnTime.Add(mPage.mTimer.GetElapsedTime());
        }

        void arrangeAutoIKnowAndAutoFlipperInSymmPage()
        {
            mSymmPageFlipper = new Timer();
            mSymmPageFlipper.Elapsed += new ElapsedEventHandler(symmPage_autoFlipped);
            mSymmPageFlipper.Interval = mPage.mMeanRT * 2;
            mSymmPageFlipper.AutoReset = false;
            mSymmPageFlipper.Enabled = true;

            /*mIKnowBtnShower = new Timer();
            mIKnowBtnShower.Interval = 2000;
            mIKnowBtnShower.AutoReset = false;
            mIKnowBtnShower.Elapsed += new ElapsedEventHandler(btnShower_Elapsed);
            mIKnowBtnShower.Enabled = true;*/
            showIKnowBtn();
        }

        void showIKnowBtn()
        {
            CompBtnNextPage btn = new CompBtnNextPage("看好了");
            btn.Add2Page(mPage.mBaseCanvas, FEITStandard.PAGE_BEG_Y + 400);
            btn.mfOnAction = onIKnowBtn; 
        }

        void symmPage_autoFlipped(object sender, ElapsedEventArgs e)
        {
            /*if (mIKnowBtnShower.Enabled)
                mIKnowBtnShower.Enabled = false;*/

            mfRoute = showOvertimePage;
            t_Elapsed(sender, e);
        }

        void showOvertimePage()
        {
            mfRoute = showBlackPageAndGo2Pos;

            CompCentralText ct0 = new CompCentralText();
            ct0.PutTextToCentralScreen("观察超时",
                "KaiTI", 48, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 0, 0));

            mPage.mRecorder.symmOffTime.Add(mPage.mTimer.GetElapsedTime());
            mPage.mRecorder.choiceShownTime.Add(-1);
            mPage.mRecorder.choiceMadeTime.Add(-1);
            mPage.mRecorder.symmJudgeCorrectness.Add(false);

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 1500;
            t.AutoReset = false;
            t.Enabled = true;
        }

        /*void btnShower_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(showIKnowBtn));
        }*/

        void onIKnowBtn(object obj)
        {
            if (mSymmPageFlipper.Enabled)
                mSymmPageFlipper.Enabled = false;

            nextStep();
        }

        public void doNothing(CompDualDetermine self)
        { }

        public void showDualDeterPage()
        {
            Mouse.OverrideCursor = Cursors.Hand;
            //record
            mPage.mRecorder.symmOffTime.Add(mPage.mTimer.GetElapsedTime());

            mPage.ClearAll();
            CompDualDetermine dualPad = new CompDualDetermine();

            dualPad.setButtonText("是", "否");
            dualPad.setCorrectness(mContent[mGrpAt].Trails[mItemAt].IsSymm);
            dualPad.setResult("");
            dualPad.HideCorrecteness(true);
            dualPad.mConfirmMethod = DualDeterConfirmMethod;
            dualPad.mDenyMethod = DualdeterDenyMethod;

            if(mPage.mMainWindow.mbEngiMode)
                dualPad.BorderBrush = new SolidColorBrush(Color.FromRgb(50, 50, 50));

            dualPad.BorderThickness = new Thickness(1.0);
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("是否对称", "KaiTi", 45, 
                ref mPage.mBaseCanvas, -130, Color.FromRgb(255, 255, 255));

            mPage.mBaseCanvas.Children.Add(dualPad);
            Canvas.SetTop(dualPad, FEITStandard.PAGE_BEG_Y +
                (FEITStandard.PAGE_HEIGHT - CompDualDetermine.OUTHEIGHT) / 2);
            Canvas.SetLeft(dualPad, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - CompDualDetermine.OUTWIDTH) / 2);

            mfRoute = showBlackPageAndGo2Pos;

            //record
            mPage.mRecorder.choiceShownTime.Add(mPage.mTimer.GetElapsedTime());
        }

        private void pressedGoNext()
        {
            mPage.ClearAll();
            Timer pressedT = new Timer();
            pressedT.Interval = 250;
            pressedT.AutoReset = false;
            pressedT.Elapsed += new ElapsedEventHandler(t_ElapsedNext);
            pressedT.Enabled = true;
        }

        private void DualDeterConfirmMethod(CompDualDetermine self)
        {

            mCurAnswer.TF.Add(true);
            mPage.mRecorder.choiceMadeTime.Add(mPage.mTimer.GetElapsedTime());

            self.HideCorrecteness(false);

            if (mContent[mGrpAt].Trails[mItemAt].IsSymm)
            {

                mCurAnswer.TFCorrectness.Add(true);
                mPage.mRecorder.symmJudgeCorrectness.Add(true);
                
                if(mPractise)
                {
                    self.setCorrectness(true);
                }

                mSymmCorrectCount++;
            }
            else
            {
                mCurAnswer.TFCorrectness.Add(false);
                mPage.mRecorder.symmJudgeCorrectness.Add(false);
                
                if(mPractise)
                {
                    self.setCorrectness(false);
                }

                mSymmCorrectWarningMark++;
            }

            if (!mPractise)
            {
                pressedGoNext();
            }
            else
            {
                Timer t = new Timer();
                t.Elapsed += new ElapsedEventHandler(t_Elapsed);
                t.AutoReset = false;
                t.Enabled = true;
                t.Interval = 1000;
            }

            self.mConfirmMethod = doNothing;
            self.mDenyMethod = doNothing;
        }

        private void DualdeterDenyMethod(CompDualDetermine self)
        {

            mCurAnswer.TF.Add(false);
            mPage.mRecorder.choiceMadeTime.Add(mPage.mTimer.GetElapsedTime());

            self.HideCorrecteness(false);

            if (mContent[mGrpAt].Trails[mItemAt].IsSymm)
            {

                mCurAnswer.TFCorrectness.Add(false);
                mPage.mRecorder.symmJudgeCorrectness.Add(false);
                
                if(mPractise)
                {
                    self.setCorrectness(false);
                }

                mSymmCorrectWarningMark++;
            }
            else 
            {
                mCurAnswer.TFCorrectness.Add(true);
                mPage.mRecorder.symmJudgeCorrectness.Add(true);
                
                if(mPractise)
                {
                    self.setCorrectness(true);
                }

                mSymmCorrectCount++;
            }

            if (!mPractise)
            {
                pressedGoNext();
            }
            else
            {
                Timer t = new Timer();
                t.Elapsed += new ElapsedEventHandler(t_Elapsed);
                t.AutoReset = false;
                t.Enabled = true;
                t.Interval = 1000;
            }

            self.mConfirmMethod = doNothing;
            self.mDenyMethod = doNothing;
        }

        public void showPosistionPage()
        {
            Mouse.OverrideCursor = Cursors.None;
            //record
            mPage.mRecorder.inters.Add(mContent[mGrpAt].Trails[mItemAt]);
            //mRecorder.segmentID.Add(mCurSpanAt + 1);

            SubPageOrderSS sp_pos = new SubPageOrderSS(ref mPage, this);
            sp_pos.mCheckComponent.mTouchActivated = false;
            sp_pos.PutNumCheckToScreen(271, 160, 4, 4, 600, 240);
            ((UIGroupNumChecksSS)sp_pos.mCheckComponent).setPositionMode(true);
            ((UIGroupNumChecksSS)sp_pos.mCheckComponent).setMarked(
                mContent[mGrpAt].Trails[mItemAt].Position);
            
            if(mItemAt == mContent[mGrpAt].Trails.Count - 1)
            {
                mfRoute = showOrderPage;
            }
            else
            {
                mfRoute = showSymmPage;
            }

            mItemAt++;

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(showBlackPage);
            if (mPractise && mGrpAt == 0)
            {
                t.Interval = 2000;
            }
            else
            {
                t.Interval = 1000;
            }
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void showReportPage()
        {
            mPage.ClearAll();

            CompCentralText ct0 = new CompCentralText();
            CompCentralText ct = new CompCentralText();
            CompCentralText ct2 = new CompCentralText();
            CompCentralText ctWarn = new CompCentralText();

            if (mPractise)
            {
                ct0.PutTextToCentralScreen("这组题（共" + mContent[mGrpAt].Trails.Count + "个）中，你",
                    "KaiTI", 32, ref mPage.mBaseCanvas, -40, Color.FromRgb(255, 255, 255));
                ct.PutTextToCentralScreen("记对了" + mOrderCorrectCount + "个位置",
                    "KaiTi", 32, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));
                ct2.PutTextToCentralScreen("做对了" + mSymmCorrectCount + "个判断",
                    "KaiTi", 32, ref mPage.mBaseCanvas, 40, Color.FromRgb(255, 255, 255));
                //LayoutInstruction li = new LayoutInstruction(ref mPage.mBaseCanvas);
            }

            if (mShowWarning)
            {
                ctWarn.PutTextToCentralScreen("请你注意判断的正确率！",
                    "KaiTi", 30, ref mPage.mBaseCanvas, 110, Color.FromRgb(255, 0, 0));
                mShowWarning = false;
            }

            if (mPractise)
            {
                mfRoute = showSymmPage;
            }
            else
            {
                mfRoute = showTitlePage;
            }

            groupIterate();

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void showTitlePage()
        {
            mPage.ClearAll();

            CompCentralText ct0 = new CompCentralText();
            ct0.PutTextToCentralScreen("判对称，记位置",
                "KaiTi", 50, ref mPage.mBaseCanvas, 0, Color.FromRgb(0, 255, 0));

            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("[" + mContent[mGrpAt].Trails.Count + "-" + (mCurSpanAt + 1) + "]",
                "KaiTi", 50, ref mPage.mBaseCanvas, 100, Color.FromRgb(0, 255, 0));

            mfRoute = showSymmPage;

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(tx_Elapsed);
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        void showBlackPage(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(
                DispatcherPriority.Normal, new timedele(showBlackPage));
        }

        void putEmptyCellOn()
        {
            SubPageOrderSS sp_pos = new SubPageOrderSS(ref mPage, this);
            sp_pos.mCheckComponent.mTouchActivated = false;
            sp_pos.PutNumCheckToScreen(271, 160, 4, 4, 600, 240);
            ((UIGroupNumChecksSS)sp_pos.mCheckComponent).setPositionMode(true);
        }

        void showBlackPage()
        {
            mPage.ClearAll();
            
            if (mfRoute == showOrderPage)
            {
                putEmptyCellOn();
            }
            //delay
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 500;
            t.AutoReset = false;
            t.Enabled = true;
        }

        void showBlackPageAndGo2Pos()
        {
            mPage.ClearAll();

            mfRoute = showPosistionPage;
            //delay
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 250;
            t.AutoReset = false;
            t.Enabled = true;
        }

        /*void t_ElapsedBlackPage(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(mPage.ClearAll));
            Timer tx = new Timer();
            tx.Interval = 1000;
            tx.AutoReset = false;
            tx.Elapsed += new ElapsedEventHandler(tx_Elapsed);
            tx.Enabled = true;
        }*/

        void tx_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(showBlackPage));
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(mPage.ClearAll));
            
            if (mfRoute == showOrderPage)
            {
                mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(putEmptyCellOn));
            }

            Timer tt = new Timer();
            tt.Interval = 250;
            tt.AutoReset = false;
            tt.Elapsed += new ElapsedEventHandler(t_ElapsedNext);
            tt.Enabled = true;
        }

        void t_ElapsedNext(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(nextStep));
        }
    }
}

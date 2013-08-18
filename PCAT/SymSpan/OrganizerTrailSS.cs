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

namespace FiveElementsIntTest.SymSpan
{
    public class OrganizerTrailSS
    {
        private PageSymmSpan mPage;
        public bool mPractise;
        private List<TrailsGroupSS> mContent;
        
        private int mItemAt = 0;
        private int mGrpAt = 0;

        private int mCurSpanGroupsCount = 0;
        private int mCurSpanAt = 0;

        private int mSymmCorrectCount = 0;
        private double mSymmGrpCorrectRate = 0.0;
        private int mOrderCorrectCount = 0;

        private int mOrderErrorACC = 0;

        public delegate void OrgRoute();
        public delegate void timedele();
        public OrgRoute mfRoute;

        private List<AnswerSSST> mAnswer;
        private AnswerSSST mCurAnswer;
        private SubPageOrderSS mOrderPage;

        public RecorderSymSpan mRecorder;

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
                mAnswer = answer;
            }

            mOrderPage = new SubPageOrderSS(ref mPage, this);

            mCurSpanGroupsCount = getCurSpanGroupsCount();

            mRecorder = new RecorderSymSpan(mPage);
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
            mRecorder.posOffTime.Add(mPage.mTimer.GetElapsedTime());
            //symm answers` statistics
            mSymmGrpCorrectRate = ((double)mSymmCorrectCount) / ((double)mContent[mGrpAt].Trails.Count);
            
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
            if (mPractise)
            {
                mfRoute = showReportPage;
            }
            else
            {
                //record
                mRecorder.shownPosition.Add(mCurAnswer.Order);

                List<int> bufferOrder = new List<int>();
                for(int i = 0 ; i < mOrderPage.mCheckComponent.mOrder.Count; i++)
                {
                    bufferOrder.Add(mOrderPage.mCheckComponent.mOrder[i]);
                }
                mRecorder.userSelPosition.Add(bufferOrder);

                mRecorder.posCorrectness.Add(!everWrong);
                //mRecorder.elementInArray.Add(mContent[mGrpAt].Trails.Count);

                //sta. & routing
                if (getGroupSymmCorrectRate() < 0.85)
                {
                    mfRoute = showWarningPage;
                }
                else 
                {
                    mfRoute = showTitlePage;
                }

                if (mOrderErrorACC > 1 && mGrpAt >= 5)
                    mfRoute = testEnd;

                if(mOrderErrorACC >2 && mGrpAt < 5)
                    mfRoute = testEnd;

                groupIterate();
            }
        }

        public void testEnd()
        {
            mRecorder.outputReport(FEITStandard.GetRepotOutputPath() + "symm\\" + PageSymmSpan.interFilename,
                    FEITStandard.GetRepotOutputPath() + "symm\\" + PageSymmSpan.posFilename);
            mfRoute = mPage.finish;
            mPage.nextStep();
        }

        public void groupIterate()
        {
            mGrpAt++;
            mItemAt = 0;
            mSymmCorrectCount = 0;
            mOrderCorrectCount = 0;

            if (mCurSpanAt == mCurSpanGroupsCount - 1)
            {
                mCurSpanGroupsCount = getCurSpanGroupsCount();
                mOrderErrorACC = 0;
                mCurSpanAt = 0;
            }
            else
            {
                mCurSpanAt++;
            }

            //length2 addtional items
            if (mGrpAt == 3 && !mPractise)
            {
                int posErrCount = 0;
                for (int i = 0; i < mRecorder.posCorrectness.Count; i++)
                {
                    if (mRecorder.posCorrectness[i] == false)
                        posErrCount++;
                }

                if (posErrCount < 2)
                {
                    mGrpAt += 2;
                    mCurSpanAt = 0;
                    mOrderErrorACC = 0;
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

            //new answer struct
            if (!mPractise)
            {
                mAnswer.Add(mCurAnswer);
                mCurAnswer = new AnswerSSST();
            }

            mOrderPage.mCheckComponent.reset();
        }

        public double getGroupSymmCorrectRate()
        {
            return mSymmGrpCorrectRate;
        }

        public void nextStep()
        {
            mfRoute();
        }
        
        public void showOrderPage()
        {
            mPage.ClearAll();
            mOrderPage.Show();
            mRecorder.posOnTime.Add(mPage.mTimer.GetElapsedTime());

            //wait for order`s signal
        }

        public void showSymmPage()
        {
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
            Canvas.SetTop(img_ctrl, FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT - img_ctrl.Height) / 2);
            Canvas.SetLeft(img_ctrl, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - img_ctrl.Width) / 2);

            mfRoute = showDualDeterPage;

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = mPage.mMeanRT;
            t.AutoReset = false;
            t.Enabled = true;
            new FEITClickableScreen(ref mPage.mBaseCanvas, nextStep, ref t);

            //record
            mRecorder.symmOnTime.Add(mPage.mTimer.GetElapsedTime());
        }

        public void doNothing(CompDualDetermine self)
        { }

        public void showDualDeterPage()
        {
            //record
            mRecorder.symmOffTime.Add(mPage.mTimer.GetElapsedTime());

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
            mRecorder.choiceShownTime.Add(mPage.mTimer.GetElapsedTime());
        }

        private void DualDeterConfirmMethod(CompDualDetermine self)
        {
            if (!mPractise)
            {
                mCurAnswer.TF.Add(true);
                mRecorder.choiceMadeTime.Add(mPage.mTimer.GetElapsedTime());
            }

            self.HideCorrecteness(false);

            if (mContent[mGrpAt].Trails[mItemAt].IsSymm)
            {
                if (!mPractise)
                {
                    mCurAnswer.TFCorrectness.Add(true);
                    mRecorder.symmJudgeCorrectness.Add(true);
                }
                else
                {
                    self.setCorrectness(true);
                }

                mSymmCorrectCount++;
            }
            else
            {
                if (!mPractise)
                {
                    mCurAnswer.TFCorrectness.Add(false);
                    mRecorder.symmJudgeCorrectness.Add(false);
                }
                else
                {
                    self.setCorrectness(false);
                }
            }

            if (!mPractise)
            {
                nextStep();
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
            if (!mPractise)
            {
                mCurAnswer.TF.Add(false);
                mRecorder.choiceMadeTime.Add(mPage.mTimer.GetElapsedTime());
            }

            self.HideCorrecteness(false);

            if (mContent[mGrpAt].Trails[mItemAt].IsSymm)
            {
                if (!mPractise)
                {
                    mCurAnswer.TFCorrectness.Add(false);
                    mRecorder.symmJudgeCorrectness.Add(false);
                }
                else
                {
                    self.setCorrectness(false);
                }
            }
            else 
            {
                if (!mPractise)
                {
                    mCurAnswer.TFCorrectness.Add(true);
                    mRecorder.symmJudgeCorrectness.Add(true);
                }
                else
                {
                    self.setCorrectness(true);
                }

                mSymmCorrectCount++;
            }

            if (!mPractise)
            {
                nextStep();
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
            //record
            if (!mPractise)
            {
                mRecorder.inters.Add(mContent[mGrpAt].Trails[mItemAt]);
                //mRecorder.segmentID.Add(mCurSpanAt + 1);
            }

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
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void showReportPage()
        {
            mPage.ClearAll();

            CompCentralText ct = new CompCentralText();
            CompCentralText ct2 = new CompCentralText();

            ct.PutTextToCentralScreen(mContent[mGrpAt].Trails.Count + "个位置记忆， 您对了" + mOrderCorrectCount + "个",
                "KaiTi", 32, ref mPage.mBaseCanvas, -30, Color.FromRgb(255, 255, 255));
            ct2.PutTextToCentralScreen(mContent[mGrpAt].Trails.Count + "个对称判断， 您对了" + mSymmCorrectCount + "个",
                "KaiTi", 30, ref mPage.mBaseCanvas, 60, Color.FromRgb(255, 255, 255));
            LayoutInstruction li = new LayoutInstruction(ref mPage.mBaseCanvas);

            Color color;
            if (mSymmGrpCorrectRate > 0.8)
            {
                color = Color.FromRgb(15, 255, 15);
            }
            else
            {
                color = Color.FromRgb(255, 15, 15);
            }

            li.addTitle(FEITStandard.PAGE_BEG_Y, FEITStandard.PAGE_WIDTH / 2 - 65,
                (mSymmGrpCorrectRate * 100).ToString("0") + "%", "Batang", 34, color); 


            mfRoute = showSymmPage;

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

            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("***" + mContent[mGrpAt].Trails.Count + "题第" + (mCurSpanAt + 1) + "组" + "***",
                "KaiTi", 50, ref mPage.mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            mfRoute = showSymmPage;

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_ElapsedBlackPage);
            t.Interval = 2000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void showWarningPage()
        {
            mPage.ClearAll();

            CompCentralText cct = new CompCentralText();
            CompCentralText cct2 = new CompCentralText();
            cct.PutTextToCentralScreen("请您注意", "KaiTi", 48,
                ref mPage.mBaseCanvas, -30, Color.FromRgb(255, 255, 255));
            cct2.PutTextToCentralScreen("对称判断的正确性", "KaiTi", 48,
                ref mPage.mBaseCanvas, 30, Color.FromRgb(255, 255, 255));

            mfRoute = showTitlePage;

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 2000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        void showBlackPage()
        {
            mPage.ClearAll();
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
            t.Interval = 500;
            t.AutoReset = false;
            t.Enabled = true;
        }

        void t_ElapsedBlackPage(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(showBlackPage));
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(nextStep));
        }
    }
}

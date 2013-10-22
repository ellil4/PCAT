using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
using System.Timers;
using PCATData;


namespace FiveElementsIntTest.ITFigure
{
    /// <summary>
    /// PageITFigure.xaml 的互動邏輯
    /// </summary>
    /// 

    public enum ITF_STATES
    {
        STA_NULL, STA_INSTRUCTION, STA_FREEPRAC, STA_READY4INTENSE, 
        STA_INTENSEPRAC, STA_READY4TEST,STA_REALTEST, STA_REPORT
    };


    public partial class PageITFigure : Page
    {
        private MainWindow mMainWindow;
        public FEITCentralFlipper mFlipper;
        private bool mIsListening;
        private ITF_STATES mCurStage = ITF_STATES.STA_NULL;
        private FEITTimer mTimer;

        private ITFFreePrac mItFreePrac;
        private ITFPrac8 mItPrac8;
        private ITFRealTest mItRealTest;
        private ITFInstruction mItInstruction;

        private bool mOnBreak = false;

        private int mItPrac8TotalCorrect = 0;

        public PageITFigure(MainWindow _mainwindow)
        {
            InitializeComponent();
            mMainWindow = _mainwindow;
            PageCommon.InitCommonPageElements(ref amBaseCanvas);
            screenReady();
            mFlipper = new FEITCentralFlipper(ref amBaseCanvas, this);

            mItFreePrac = new ITFFreePrac(this);
            mItPrac8 = new ITFPrac8(this);
            mItRealTest = new ITFRealTest(this);
            mItInstruction = new ITFInstruction(this);

            mTimer = new FEITTimer();

            //undefined
            //mCurStage = ITF_STATES.STA_FREEPRAC;

           // if(mMainWindow.mDB.TableExists(Names.))

            this.Focus();
            nextStage();
        }

        public void nextStage()
        {
            mCurStage++;

            switch (mCurStage)
            {
                case ITF_STATES.STA_INSTRUCTION:
                    screenReady();
                    mOnBreak = true;
                    mIsListening = true;
                    mItInstruction.Show();
                    break;
                case ITF_STATES.STA_FREEPRAC://free prac
                    mItFreePrac.NextItem();
                    break;
                case ITF_STATES.STA_READY4INTENSE:
                    breakScreen("下面连续做对8次才能正式进入测验，空格键继续");
                    break;
                case ITF_STATES.STA_INTENSEPRAC://intense prac
                    mItPrac8.NextItem();
                    break;
                case ITF_STATES.STA_READY4TEST:
                    breakScreen("下面开始正式测验，按空格键继续");
                    break;
                case ITF_STATES.STA_REALTEST://test
                    mItRealTest.NextItem();
                    break;
                case ITF_STATES.STA_REPORT://report
                    //Console.Out.WriteLine("stage3");
                    breakScreen("测试结束，你的反应时是" + mItRealTest.GetRT() + "毫秒， 空格键退出");
                    break;
            }
        }

        public void breakScreen(string content)
        {
            screenReady();
            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen(content,
                "Microsoft YaHei", 36, ref amBaseCanvas,
                 18,
                System.Windows.Media.Color.FromRgb(220, 220, 220));

            mOnBreak = true;
            mIsListening = true;
        }

        private List<long> genInterval(long coreDuration)
        {
            List<long> retval = new List<long>();
            retval.Add(500);//fix cross
            retval.Add(360);//black blank
            retval.Add(coreDuration);//stimulation
            retval.Add(360);//mask
            retval.Add(10);//black blank
            return retval;
        }

        private List<Bitmap> genBitmap(bool left)
        {
            List<Bitmap> retval = new List<Bitmap>();

            retval.Add(FiveElementsIntTest.Properties.Resources.CROSS);
            retval.Add(FiveElementsIntTest.Properties.Resources.BLANK);

            if (left)
                retval.Add(FiveElementsIntTest.Properties.Resources.LINELEFT);
            else
                retval.Add(FiveElementsIntTest.Properties.Resources.LINERIGHT);

            retval.Add(FiveElementsIntTest.Properties.Resources.LINEMASK);
            retval.Add(FiveElementsIntTest.Properties.Resources.BLANK);

            return retval;
        }

        public void ShowLeft(long duration)
        {
            List<long> interval = genInterval(duration);
            List<Bitmap> bmps = genBitmap(true);
            mFlipper.SetupStepLink(bmps, interval, OnFinishItem, OnStartShowingItem);
            mFlipper.DoStepLink();
        }

        public void ShowRight(long duration)
        {
            List<long> interval = genInterval(duration);
            List<Bitmap> bmps = genBitmap(false);
            mFlipper.SetupStepLink(bmps, interval, OnFinishItem, OnStartShowingItem);
            mFlipper.DoStepLink();
        }

        public void OnFinishItem()
        {
            //this.Focus();
            mIsListening = true;
            mTimer.Start();
        }

        private void OnStartShowingItem()
        {
            //mIsListening = false;
        }

        //correctness with delayed nextItem()
        private delegate void invokeShell();

        private void screenReady()
        {
            amBaseCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref amBaseCanvas, ref mBdrContentArea, ref mBdrEdge, ref mMainWindow);
            amBaseCanvas.Children.Add(amCentralImage);
        }

        private void delayedNextItem(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new invokeShell(screenReady));
            Dispatcher.Invoke(DispatcherPriority.Normal, new invokeShell(mItFreePrac.NextItem));
        }

        private void ShowCorrectnessNextItemFollow(bool correct)
        {
            screenReady();

            CompCentralText cct = new CompCentralText();
            if (correct)
            {
                cct.PutTextToCentralScreen("正确", 
                    "Microsoft YaHei", 36, ref amBaseCanvas, 
                    18, 
                    System.Windows.Media.Color.FromRgb(22, 233, 44));
            }
            else
            {
                cct.PutTextToCentralScreen("错误",
                    "Microsoft YaHei", 36, ref amBaseCanvas,
                    18,
                    System.Windows.Media.Color.FromRgb(233, 22, 44));
            }

            Timer t = new Timer();
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
            t.Elapsed += new ElapsedEventHandler(delayedNextItem);
        }
        //correctness with delayed nextItem()over

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (mIsListening)
            {
                if (!mOnBreak)
                {
                    //if valid
                    if (e.Key == Key.F || e.Key == Key.J)
                    {
                        mTimer.Stop();

                        bool userChoice = false;

                        if (e.Key == Key.F)
                        {
                            mIsListening = false;
                            userChoice = true;
                            //Console.Out.WriteLine("left");
                        }
                        else if (e.Key == Key.J)
                        {
                            userChoice = false;
                            mIsListening = false;
                            //Console.Out.WriteLine("right");
                        }

                        switch (mCurStage)
                        {
                            case ITF_STATES.STA_FREEPRAC:
                                mItFreePrac.mItems[mItFreePrac.mNowTill].UserChoiceDirection = userChoice;
                                mItFreePrac.mItems[mItFreePrac.mNowTill].RT = mTimer.GetElapsedTime();
                                mTimer.Reset();

                                if (mItFreePrac.mItems[mItFreePrac.mNowTill].UserChoiceDirection ==
                                    mItFreePrac.mItems[mItFreePrac.mNowTill].ItemDirection)
                                {
                                    ShowCorrectnessNextItemFollow(true);
                                }
                                else
                                {
                                    ShowCorrectnessNextItemFollow(false);
                                }
                                //mItFreePrac.NextItem();
                                break;
                            case ITF_STATES.STA_INTENSEPRAC:
                                if (mItPrac8.mLastLeft == userChoice)
                                {
                                    mItPrac8TotalCorrect++;
                                }
                                else
                                {
                                    mItPrac8TotalCorrect = 0;
                                }

                                if (mItPrac8TotalCorrect == 8)
                                {
                                    nextStage();
                                }
                                else
                                {
                                    mItPrac8.NextItem();
                                }
                                break;
                            case ITF_STATES.STA_REALTEST:
                                //save input
                                mItRealTest.mItems[mItRealTest.mItems.Count - 1].UserChoiceDirection = userChoice;
                                mItRealTest.mItems[mItRealTest.mItems.Count - 1].RT = mTimer.GetElapsedTime();
                                mTimer.Reset();
                                //save correctness
                                if (mItRealTest.mLastLeft == userChoice)
                                {
                                    mItRealTest.mLastCorrect = true;
                                }
                                else
                                {
                                    mItRealTest.mLastCorrect = false;
                                }

                                //if stop?
                                if (mItRealTest.mTrackedRTs.Count == 4)//undefined
                                {
                                    nextStage();
                                }
                                else if (mItRealTest.m30Count == 10)//if break undefined
                                {
                                    mItRealTest.m30Count = 0;
                                    breakScreen("休息一会儿吧 按空格键继续");
                                }
                                else
                                {
                                    mItRealTest.NextItem();
                                }
                                break;
                            /*case 3:
                                System.Environment.Exit(0);
                                break;*/
                        }
                    }
                }
                else if (e.Key == Key.Space)
                {
                    if (mCurStage == ITF_STATES.STA_REALTEST)
                    {
                        mIsListening = false;
                        mOnBreak = false;
                        screenReady();
                        mItRealTest.NextItem();
                    }
                    else if (mCurStage == ITF_STATES.STA_REPORT)
                    {
                        System.Environment.Exit(0);
                    }
                    else if (mCurStage == ITF_STATES.STA_INSTRUCTION || 
                        mCurStage == ITF_STATES.STA_READY4INTENSE ||
                        mCurStage == ITF_STATES.STA_READY4TEST)
                    {
                        mIsListening = false;
                        mOnBreak = false;
                        screenReady();
                        nextStage();
                    }
                }
            }

            //Console.Out.WriteLine("pressed");
        }

    }
}

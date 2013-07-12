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
using System.Timers;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Threading;
using PCATData;

namespace FiveElementsIntTest.SybSrh
{
    /// <summary>
    /// PageSybSrh.xaml 的互動邏輯
    /// </summary>
    public partial class PageSybSrh : Page
    {
        private MainWindow mMainWindow;
        //public static int TEST1ITEMTILL = 2;
        //public static int TEST2ITEMTILL = TEST1ITEMTILL * 2;

       // public int mTestEndMark = TEST1ITEMTILL - 1;

        public int SPAN_LEN = 12;
        public List<CompImage> mImages;
        public LayoutSybSrh mLayout;
        public List<SybSrhItem> mItems;
        public static String RESPATH = AppDomain.CurrentDomain.BaseDirectory + "SybSrh\\";
        public static String DSTPATH = AppDomain.CurrentDomain.BaseDirectory + "SybSrh\\output\\";
        public List<SybSrhResult> mResult;
        //public List<SybSrhResult> mSpanResult;

        //public static long SPAN_TIME_LONG = 5000;

        public STATUS mStatus = STATUS.INSTRUCTION;
        public int mCurTillIdx = 0;
        public SybSrhItemGenerator mGen;

        public FEITTimer mTimer;
        public Stopwatch m2Minute;
        private int mDidCount = 0;

        public enum STATUS
        {
            INSTRUCTION, INSTRUCTION2, INSTRUCTION3, TEST, FINISH
        }

        public PageSybSrh(MainWindow mw)
        {
            InitializeComponent();
            mMainWindow = mw;
            mImages = new List<CompImage>();
            mResult = new List<SybSrhResult>();
            //mSpanResult = new List<SybSrhResult>();
            mGen = new SybSrhItemGenerator();
            mTimer = new FEITTimer();

            m2Minute = new Stopwatch();

            if (mMainWindow.mDB != null)
            {
                if (!mMainWindow.mDB.TableExists(Names.SYMBOL_SEARCH_TABLENAME))
                {
                    mMainWindow.mDB.CreateSymbolSearchTable(mItems.Count);
                }
            }

        }

        public void clearAll()
        {
            amCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref amCanvas, ref amBorder800, 
                ref amBorder1024, ref mMainWindow);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amCanvas);
            clearAll();

            for (int i = 0; i < 7; i++)
            {
                CompImage ci = new CompImage(i);

                ci.amBorder.Width = 100;
                ci.amBorder.Height = 100;
                ci.amImage.Width = 100;
                ci.amImage.Height = 100;
                ci.Width = 100;
                ci.Height = 100;

                mImages.Add(ci);
            }

            mLayout = new LayoutSybSrh(this);
            mLayout.SetInstructionLayout();

            this.Focus();

            //test belwow undefined
            //SybSrhItemGenerator ssg = new SybSrhItemGenerator();
            //while (true)
            //{
            //    List<SybSrhItem> items = ssg.Get12Items();
            //    bool ifpass = true;

            //    for(int k = 0; k < items.Count; k++)
            //    {
            //        for (int i = 0; i < items[k].Selection.Length; i++)
            //        {
            //            if (items[k].Selection[i] == null)
            //            {
            //                ifpass = false;
            //            }
            //        }

            //        if (!ifpass)
            //        {
            //            Console.WriteLine("not pass");
            //            ifpass = true;
            //        }
            //    }
                
                
            //}
        }

        public void UpdateSelPageData(int index)
        {
            for (int i = 0; i < mImages.Count; i++)
            {
                if (i < 2)
                {
                    mImages[i].amImage.Source =
                        BitmapSourceFactory.GetBitmapSource(
                        mItems[mCurTillIdx].Target[i].BMP);
                }
                else
                {
                    mImages[i].amImage.Source =
                        BitmapSourceFactory.GetBitmapSource(
                        mItems[mCurTillIdx].Selection[i - 2].BMP);
                }
            }

            mTimer.Stop();
            mTimer.Reset();
            mTimer.Start();
        }

        public static int MAXTRAILCOUNT = 72;

        public void Next()
        {
            if (m2Minute.ElapsedMilliseconds >= 120000 || mDidCount == MAXTRAILCOUNT)
            {
                mStatus = STATUS.FINISH;
            }

            switch (mStatus)
            {
                case STATUS.INSTRUCTION:
                    mLayout.SetInstructionLayout2();
                    mStatus = STATUS.INSTRUCTION2;
                    break;
                case STATUS.INSTRUCTION2:
                    mLayout.SetInstructionLayout3();
                    mStatus = STATUS.INSTRUCTION3;
                    break;
                case STATUS.INSTRUCTION3:
                    preTextNextReaction(STATUS.TEST);
                    break;
                case STATUS.TEST:
                    testNextReaction();
                    break;
                case STATUS.FINISH:
                    writeResult();
                    testEnd();
                    break;
            }
        }

        private delegate void invokeDelegate(int nextEndMark, STATUS nextStage);

        //private void testSpanFinish(int nextEndMark, STATUS nextStage)
        //{
        //    mLayout.SetReport(getMeanRT(), correctnessCalc());

        //    for (int i = 0; i < mSpanResult.Count; i++)
        //    {
        //        mResult.Add(mSpanResult[i]);
        //    }

        //    //add devide mark
        //    mResult.Add(new SybSrhResult(false, -4));
        //    mSpanResult.Clear();

        //    mStatus = nextStage;
        //    mTestEndMark = nextEndMark;
        //    m2Minute.Enabled = false;
        //}

        private void testNextReaction()
        {
            if (mCurTillIdx < SPAN_LEN)
            {
                UpdateSelPageData(mCurTillIdx);
                mCurTillIdx++;
            }
            else
            {
                mCurTillIdx = 0;
                mItems = mGen.Get12Items();
                UpdateSelPageData(mCurTillIdx);
                mCurTillIdx++;
            }
        }

        private void preTextNextReaction(STATUS nextStage)
        {
            mStatus = nextStage;
            mLayout.SetSelectionLayout();
            mCurTillIdx = 0;
            mItems = mGen.Get12Items();
            UpdateSelPageData(mCurTillIdx);
            mCurTillIdx++;
            m2Minute.Start();
            

            /*m2Minute.Elapsed += new ElapsedEventHandler(m1Minute_Elapsed);
            m2Minute.Interval = SPAN_TIME_LONG;
            m2Minute.AutoReset = false;
            m2Minute.Enabled = true;*/
        }

        private void writeResult()
        {
            PCATDataSaveReport();
            SybSrhWriter writer = new SybSrhWriter();
            writer.WriteResults(DSTPATH + FEITStandard.GetStamp() + ".txt", mResult);
        }

        private void appendItemInfo2QRec(ref QRecSymbolSearch rec, SybSrhItem item)
        {
            for (int i = 0; i < rec.TarIdx.Length; i++)
            {
                rec.TarIdx[i] = item.Target[i].Index;
                rec.TarType[i] = item.Target[i].Type;
            }
            rec.TrueTarAt = item.GetTrueTarIdx();

            for (int i = 0; i < rec.SelIdx.Length; i++)
            {
                rec.SelIdx[i] = item.Selection[i].Index;
                rec.SelType[i] = item.Selection[i].Type;
            }
            rec.TrueSelAt = item.GetTrueSelectionIdx();
        }

        private void PCATDataSaveReport()
        {
            List<QRecSymbolSearch> info = new List<QRecSymbolSearch>();
            for (int i = 0; i < mResult.Count; i++)
            {
                QRecSymbolSearch single = new QRecSymbolSearch();
                appendItemInfo2QRec(ref single, mResult[i].Item);
                single.Answer = mResult[i].Answer;
                single.Correctness = mResult[i].Correctness;
                single.RT = mResult[i].RT;

                info.Add(single);
            }

            if(mMainWindow.mDB != null)
                mMainWindow.mDB.AddSymbolSearchRecord(info, mMainWindow.mUserID);
        }

        private void testEnd()
        {
            mLayout.SetEndPage();

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        private delegate void nonParaInvoke();
        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new nonParaInvoke(mMainWindow.TestForward), DispatcherPriority.Normal);
        }

        //private float correctnessCalc()
        //{
        //    float retval = -1f;
        //    int correctCount = 0;
            
        //    for (int i = 0; i < mSpanResult.Count; i++)
        //    {
        //        if (mSpanResult[i].Answer == mItems[i].CorrectAnswer)
        //        {
        //            mSpanResult[i].Correctness = true;
        //            correctCount++;
        //        }
        //        else
        //        {
        //            mSpanResult[i].Correctness = false;
        //        }
        //    }

        //    if (mSpanResult.Count == 0)
        //    {
        //        retval = -1;
        //    }
        //    else
        //    {
        //        retval = ((float)correctCount) / ((float)mSpanResult.Count);
        //    }
            
        //    return retval;
        //}

        //private long getMeanRT()
        //{
        //    long retval = -1;
        //    long totalRT = 0;

        //    for (int i = 0; i < mSpanResult.Count; i++)
        //    {
        //        totalRT += mSpanResult[i].RT;
        //    }

        //    if (mSpanResult.Count == 0)
        //    {
        //        retval = -1;
        //    }
        //    else
        //    {
        //        retval = totalRT / mSpanResult.Count;
        //    }

        //    return retval;
        //}

        //private void Page_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if(mStatus == STATUS.INSTRUCTION)
        //    {
        //        if (e.Key == Key.Space)
        //        {
        //            Next();
        //        }
        //    }
        //    else if (mStatus == STATUS.TEST)
        //    {
        //        mTimer.Stop();
        //        if (e.Key == Key.F)
        //        {
        //            mResult.Add(new SybSrhResult(true, mTimer.GetElapsedTime(), mItems[mCurTillIdx - 1]));
        //            Next();
        //        }
        //        else if(e.Key == Key.J)
        //        {
        //            mResult.Add(new SybSrhResult(false, mTimer.GetElapsedTime(), mItems[mCurTillIdx - 1]));
        //            Next();
        //        }
        //    }
        //}

        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mGen.Get12Items();
                //Console.WriteLine("good");
            }

            if (mStatus == STATUS.INSTRUCTION || 
                mStatus == STATUS.INSTRUCTION2 || 
                mStatus == STATUS.INSTRUCTION3)
            {
                if (e.Key == Key.Space)
                {
                    Next();
                }
            }
            else if (mStatus == STATUS.TEST && mDidCount < MAXTRAILCOUNT)
            {
                mTimer.Stop();
                mDidCount++;
                if (e.Key == Key.O)
                {
                    mResult.Add(new SybSrhResult(true, mTimer.GetElapsedTime(), mItems[mCurTillIdx - 1]));
                    Next();
                }
                else if (e.Key == Key.X)
                {
                    mResult.Add(new SybSrhResult(false, mTimer.GetElapsedTime(), mItems[mCurTillIdx - 1]));
                    Next();
                }
            }
        }


    }
}

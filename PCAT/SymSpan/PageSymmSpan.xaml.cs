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
using PCATData;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.SymSpan
{
    /// <summary>
    /// PageSymmSpan.xaml 的互動邏輯
    /// </summary>
    public partial class PageSymmSpan : Page
    {
        public MainWindow mMainWindow;
        public LayoutInstruction mLayoutInstruction;
        public int[] mTestGroupScheme;
        private int[] mPracGroupScheme;
        private List<TrailsGroupSS> mTest;
        private List<TrailsGroupSS> mPrac;
        public List<List<int>> mPracLocation;
        public List<TrailSS_ST> mPractiseSymm;
        public StatusSS mStatus;
        public long mMeanRT;

        public bool mbFixedItems = true;

        public FEITTimer mTimer;

        private List<AnswerSSST> mGroupsAnswer;//one element for each group

        public string interFilename = "";
        public string posFilename = "";
        public string pracPosFilename = "";
        public string pracSymmFilename = "";
        public RecorderSymSpan mRecorder;

        public enum StatusSS
        {
            singlePos, singleSymm, main_title, instruction, practise, instruction2, test, finish
        }

        public PageSymmSpan(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);
            mTestGroupScheme = new int[] { 2, 2, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };
            mPracGroupScheme = new int[] { 2, 2 };

            interFilename = "inter_" + mMainWindow.mDemography.GenBriefString() + ".txt";
            posFilename = "pos_" + mMainWindow.mDemography.GenBriefString() + ".txt";
            pracPosFilename = "prac_pos_" + mMainWindow.mDemography.GenBriefString() + ".txt";
            pracSymmFilename = "prac_symm_" + mMainWindow.mDemography.GenBriefString() + ".txt";

            LoaderSymmSpan loader = new LoaderSymmSpan("SYMM\\sourceIndex.txt", "SYMM\\fixedItems.txt", "SYMM\\locationExe.txt", "SYMM\\fixedSymmExeBaseline.txt");
            List<TrailSS_ST> rList = loader.GetResourceList();

            if (!mbFixedItems)
            {
                RandomSelector testRanSel = new RandomSelector(rList, mTestGroupScheme);
                mTest = testRanSel.Get();

                RandomSelector pracRanSel = new RandomSelector(rList, mPracGroupScheme);
                mPrac = pracRanSel.Get();
            }
            else
            {
                mTest = loader.GetFixedItemGroups(mTestGroupScheme);
                mPrac = loader.GetFixedComprehExe(mPracGroupScheme);
                mPracLocation = loader.GetFixedLocationExe();
                mPractiseSymm = loader.GetFixedSymmExe();
            }

            mGroupsAnswer = new List<AnswerSSST>();

            mStatus = StatusSS.main_title;//test could be from here

            mTimer = new FEITTimer();
            mTimer.Start();

            mRecorder = new RecorderSymSpan(this);

            /*if (!mMainWindow.mDB.TableExists(Names.SYMSPAN_POS_TABLENAME))
            {
                mMainWindow.mDB.CreateSymSpanPosTable(mTestGroup.Length);
            }

            if (!mMainWindow.mDB.TableExists(Names.SYMSPAN_SYMM_TABLENAME))
            {
                mMainWindow.mDB.CreateSymSpanSymmTable(trails.Count);
            }*/
        }

        //what is this?
        //input: posisiton in total groups` array
        //output: posisiton in span
        //use: UI maybe
        public static int getSubGroupID(int numInArray)
        {
            return (numInArray % 2) + 1;
        }

        public void nextStep()
        {
            switch (mStatus)
            {
                case StatusSS.main_title:
                    loadMainTitle();
                    break;
                case StatusSS.singlePos:
                    loadSinglePos();
                    break;
                case StatusSS.singleSymm:
                    loadSingleSymm();
                    break;
                case StatusSS.practise:
                    loadPractise();
                    break;
                case StatusSS.instruction2:
                    loadInstruction2();
                    break;
                case StatusSS.test:
                    loadTest();
                    break;
                case StatusSS.finish:
                    finish();
                    break;
            }
        }

        private void loadSinglePos()
        {
            OrganizerPracLocation org = new OrganizerPracLocation(this, mPracLocation, mRecorder);
            org.next();
            mStatus = StatusSS.singleSymm;
        }

        private List<long> mRTBaseLine = new List<long>();

        private void loadSingleSymm()
        {
            OrganizerPracSymm org = new OrganizerPracSymm(this, mPractiseSymm, mRecorder);
            org.next();
            mStatus = StatusSS.practise;

            mRTBaseLine = org.mRTs;
        }

        private void loadMainTitle()
        {
            ClearAll();
            mLayoutInstruction.addTitle(70, 0, "对称广度", "SimHei", 52, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(133, 105, "Operation Span", "Batang", 32, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(240, 5, 
                "请你判断图形的对称性，并记住随后出现的红点位置。", 
                "SimHei", 30, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(290, 0,
                "下面先来练习一下记忆红点位置",
                "SimHei", 30, Color.FromRgb(255, 255, 255));

            mStatus = StatusSS.singlePos;
            //new FEITClickableScreen(ref mBaseCanvas, nextStep);
            CompBtnNextPage btn = new CompBtnNextPage("下一页");
            btn.Add2Page(mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = BlankMask1000Next;
        }

        private void BlankMask1000Next(object obj)
        {
            ClearAll();
            Timer t = new Timer();
            t.Interval = 1000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        public delegate void timedele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(nextStep));
        }

        //delete 2 more-than-10-sec records
        //return twice length of the rest`s meanRT
        private long getMeanRT()
        {
            long retval = -1;

            for (int i = 0; i < mRTBaseLine.Count; i++)
            {
                retval += mRTBaseLine[i];
            }

            if (mRTBaseLine.Count != 0)
            {
                retval /= mRTBaseLine.Count;
            }
            else
            {
                retval = 5000;
            }

            return retval;
        }

        private void loadPractise()
        {
            mMeanRT = getMeanRT();


            OrganizerTrailSS ots = 
                new OrganizerTrailSS(this, true, mPrac, ref mGroupsAnswer);
            mStatus = StatusSS.instruction2;
            ots.nextStep();
        }

        private void loadInstruction2()
        {
            ClearAll();
            mLayoutInstruction.addTitle(240, 0, "以下是正式测验,按鼠标键继续", "KaiTi", 30, Color.FromRgb(255, 255, 255));
            
            mStatus = StatusSS.test;
            
            new FEITClickableScreen(ref mBaseCanvas, nextStep);
        }

        private void loadTest()
        {
            OrganizerTrailSS ots =
                new OrganizerTrailSS(this, false, mTest, ref mGroupsAnswer);
            
            mStatus = StatusSS.finish;

            ots.nextStep();
        }

        public void finish()
        {
            mTimer.Stop();
            ClearAll();

            CompCentralText cct = new CompCentralText();
            CompCentralText cct2 = new CompCentralText();
            CompCentralText cct3 = new CompCentralText();

            cct.PutTextToCentralScreen("本次测验结束", "KaiTi",
                50, ref mBaseCanvas, -100, Color.FromRgb(255, 255, 255));

            cct2.PutTextToCentralScreen("请稍作休息进行其他测验", "KaiTi",
                50, ref mBaseCanvas, -35, Color.FromRgb(255, 255, 255));

            cct3.PutTextToCentralScreen("点击鼠标继续", "KaiTi",
                50, ref mBaseCanvas, 30, Color.FromRgb(255, 255, 255));

            new FEITClickableScreen(ref mBaseCanvas, exitSSSpan);
        }

        private void exitSSSpan()
        {
            mMainWindow.TestForward();
        }

        public void ClearAll()
        {
            mBaseCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref mBaseCanvas, ref mAuxBorder, ref mAuxBorder1024, ref mMainWindow);
        }

        private void mBaseCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref mBaseCanvas);
            ClearAll();
            
            
            loadMainTitle();

            //systest disabled
            //loadSingleSymm();
            //mMeanRT = 2000;
            /*OrganizerTrailSS ots =
                new OrganizerTrailSS(this, false, mTest, ref mGroupsAnswer);
            mStatus = StatusSS.finish;
            ots.nextStep();*/
            //loadSinglePos();
            //loadSingleSymm();
            //loadPractise();
            //loadTest();
        }
    }
}

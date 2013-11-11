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
using System.Collections;
using PCATData;
using System.Timers;
using System.Windows.Threading;
using LibTabCharter;

namespace FiveElementsIntTest.OpSpan
{
    /// <summary>
    /// PageOpSpan.xaml 的互動邏輯
    /// </summary>
    public partial class PageOpSpan : Page
    {
        public MainWindow mMainWindow;
        public List<TrailGroupOS> mGroups;
        public List<TrailGroupOS> mGroupsPrac;
        public int[] mGroupArrangement;
        public int[] mGroupArrangementPrac;
        public PageAttr mCurrentStatus = PageAttr.title;

        public LayoutInstruction mLayoutInstruction;

        private OrganizerTrailOS mOrgPrac;
        private OrganizerTrailOS mOrgTest;

        public RecorderOpSpan mRecorder;
        public FEITTimer mTimer;

        public List<long> mRTs;
        public long mMeanRT = 2000;//default value, used for test

        public string interFilename;
        public string orderFilename;
        public string pracMathFilename;
        public string pracOrderFilename;

        public bool mbFixedItemMode = true;

        public enum PageAttr
        {
            title, instructCompreh, practise, practiseChar, practiseEquation, instruction2, test, finish
        };

        public PageOpSpan(MainWindow mainWindow)
        {
            InitializeComponent();
            mMainWindow = mainWindow;
            mGroupArrangement = new int[] { 2, 2, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 };
            mGroupArrangementPrac = new int[] { 2, 2 };

            mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);

            mRecorder = new RecorderOpSpan(this);
            mTimer = new FEITTimer();

            interFilename = "inter_" + mMainWindow.mDemography.GenString() + ".txt";
            orderFilename = "order_" + mMainWindow.mDemography.GenString() + ".txt";
            pracMathFilename = "pracMath_" + mMainWindow.mDemography.GenString() + ".txt";
            pracOrderFilename = "pracOrder_" + mMainWindow.mDemography.GenString() + ".txt";
        }

        public static int getSubGroupID(int numInArray)
        {
            return (numInArray % 3) + 1;
        }

        private void readFixedFromFile(string path,
            ref List<TrailGroupOS> groups, int[] scheme, int begFromLine)
        {
                TabFetcher fet = new TabFetcher(path, "\\t");

                fet.Open();

                for (int k = 0; k < begFromLine; k++ )
                    fet.GetLineBy();//skip lines

                groups = new List<TrailGroupOS>();
                //practise
                for (int i = 0; i < scheme.Length; i++)
                {
                    TrailGroupOS group = new TrailGroupOS();
                    for (int j = 0; j < scheme[i]; j++)
                    {
                        List<String> line = fet.GetLineBy();
                        TrailOS_ST st = new TrailOS_ST();
                        st.equation = line[1];
                        st.result = line[2];
                        if(Int32.Parse(line[3]) == 1)
                        {
                            st.correctness = true;
                        }
                        else
                        {
                            st.correctness = false;
                        }
                        st.memTarget = line[4];
                        group.mTrails.Add(st);
                    }
                    groups.Add(group);
                }
                fet.Close();
        }

        private void mBaseCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref mBaseCanvas);
            ClearAll();

            if (!mbFixedItemMode)
            {
                //prepare data structures
                mGroups = GenGroups((new LoaderOpSpan("OPSPAN.csv")).GetTrails(), mGroupArrangement);

                mGroupsPrac =
                    GenGroups((new LoaderOpSpan("OPSPANPrac.csv")).GetTrails(), mGroupArrangementPrac);
            }
            else//fixed mode
            {
                readFixedFromFile(FEITStandard.GetExePath() + "OP\\opspan.txt",
                    ref mGroupsPrac, mGroupArrangementPrac, 1);
                readFixedFromFile(FEITStandard.GetExePath() + "OP\\opspan.txt",
                    ref mGroups, mGroupArrangement, 7);
            }

            //replace equations here:

            mCurrentStatus = PageAttr.title;

            //systest
            //mCurrentStatus = PageAttr.test;

            //build BD
            int individualUnitCount = 0;
            for (int i = 0; i < mGroupArrangement.Length; i++)
            {
                individualUnitCount += mGroupArrangement[i];
            }

            /*if (!mMainWindow.mDB.TableExists(Names.OPSAPN_EXPRESSION_TABLENAME))
            {
                mMainWindow.mDB.CreateOpSpanExpressionTable(individualUnitCount);
            }

            if (!mMainWindow.mDB.TableExists(Names.OPSPAN_ORDER_TABLENAME))
            {
                mMainWindow.mDB.CreateOPSpanOrderTable(mGroupArrangement.Length);
            }*/
            
            nextStep();
        }

        private List<TrailGroupOS> GenGroups(List<TrailOS_ST> trials, int[] arrangement)
        {
            List<TrailGroupOS> retval = new List<TrailGroupOS>();
            int totalCount = 0;
            bool broken = false;
            for (int i = 0; i < arrangement.Length; i++)
            {
                TrailGroupOS group = new TrailGroupOS();

                for (int j = 0; j < arrangement[i]; j++)
                {
                    if (totalCount < trials.Count)
                    {
                        group.mTrails.Add(trials[totalCount]);
                    }
                    else
                    {
                        broken = true;
                        break;
                    }
                    totalCount++;
                }

                if (broken)
                    break;

                retval.Add(group);
            }
                
            return retval;
        }

        public void ClearAll()
        {
            mBaseCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref mBaseCanvas, 
                ref mAuxBorder, ref mAuxBorder1024, ref mMainWindow);
        }

        private void loadTitlePage()
        {
            ClearAll();
            mLayoutInstruction.addTitle(70, 0, "操作广度", "KaiTi", 52, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(133, 105, "Operation Span", "Batang", 32, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addInstruction(240, 0, 600, 200, 
                "请在做心算题的同时,记住随后出现的属相(十二生肖)", 
                "KaiTi",
                42, Color.FromRgb(255, 255, 255));

            mCurrentStatus = PageAttr.practiseChar;

            //test////////////////////////////////////////////////////for stage control
            //mCurrentStatus = PageAttr.instructCompreh;
            //test above//////////////////////////////////////////////

            //new FEITClickableScreen(ref mBaseCanvas, nextStep);
            CompBtnNextPage btn = new CompBtnNextPage("下一页");
            btn.Add2Page(mBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = nextStep;
        }

        private void loadPractisePage()
        {
            mOrgPrac = new OrganizerTrailOS(mGroupsPrac, this, true);

            ClearAll();
            mOrgPrac.showEquationPage();
        }

        private void loadFinishPage()
        {
            ClearAll();

            CompCentralText cct = new CompCentralText();
            CompCentralText cct2 = new CompCentralText();
            CompCentralText cct3 = new CompCentralText();

            cct.PutTextToCentralScreen("本次测验结束", "KaiTi",
                50, ref mBaseCanvas, -100, Color.FromRgb(255, 255, 255));

            cct2.PutTextToCentralScreen("请稍作休息进行其他测验", "KaiTi",
                50, ref mBaseCanvas, -35, Color.FromRgb(255, 255, 255));

            cct3.PutTextToCentralScreen("点击按钮继续", "KaiTi",
                50, ref mBaseCanvas, 30, Color.FromRgb(255, 255, 255));

            //new FEITClickableScreen(ref mBaseCanvas, exitOpSpan);
            CompBtnNextPage btnGO = new CompBtnNextPage("测验结束");
            btnGO.Add2Page(mBaseCanvas, FEITStandard.PAGE_BEG_Y + 490);
            btnGO.mfOnAction = exitOpSpan;
        }

        private void exitOpSpan(object obj)
        {
            mMainWindow.TestForward();
        }

        private void loadTestPage()
        {
            mOrgTest = new OrganizerTrailOS(mGroups, this, false);

            ClearAll();
            mOrgTest.showTitlePage();
        }

        private void loadPractiseChar()
        {
            OrganizerPractiseChar opc = new OrganizerPractiseChar(this);
            mCurrentStatus = PageAttr.practiseEquation;
            opc.mfNext();

            mRecorder.mPracOrderAnswers = opc.mAnswers;
            mRecorder.mPracOrderCorrectness = opc.mCorrectness;
            mRecorder.mPracOrderRealOrder = opc.mRealOrder;
            mRecorder.mPracOrderRTs = opc.mRTs;
        }

        private void loadPractiseEquation()
        {
            OrganizerPractiseEquation ope = new OrganizerPractiseEquation(this);
            mCurrentStatus = PageAttr.instructCompreh;
            ope.mfNext();
            mRTs = ope.mRTs;

            mRecorder.mMathPracAnswers = ope.mAnswers;
            mRecorder.mMathPracEquations = ope.mEquations;
            mRecorder.mMathPracRTs = ope.mRTs;
        }

        private long calcMeanRt()
        {
            long retval = 2000;

            if (mRTs != null)
            {
                for (int i = 0; i < mRTs.Count; i++)
                {
                    retval += mRTs[i];
                }

                retval /= mRTs.Count;
            }
            
            return retval;
        }

        private void loadInstructionComprehensivePrac()
        {
            ClearAll();
            
            mMeanRT = calcMeanRt();

            mLayoutInstruction.addInstruction(150, 100,
                FEITStandard.PAGE_WIDTH, 400,
                "下面练习一下同时完成这两项任务\r\n      请在做心算题的同时\r\n      记住随后出现的属相",
                "KaiTi", 40, Color.FromRgb(255, 255, 255));

            mCurrentStatus = PageAttr.practise;

            //new FEITClickableScreen(ref mBaseCanvas, blank1000WithNextStep);
            CompBtnNextPage btnGO = new CompBtnNextPage("开始练习");
            btnGO.Add2Page(mBaseCanvas, FEITStandard.PAGE_BEG_Y + 590);
            btnGO.mfOnAction = blank1000WithNextStep;
        }

        private delegate void timedele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(nextStep));
        }

        private void blank1000WithNextStep()
        {
            ClearAll();

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        private void blank1000WithNextStep(object obj)
        {
            blank1000WithNextStep();
        }

        private void loadInstructionPage2()
        {
            ClearAll();

            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen("下面是正式测验", "KaiTi", 
                38, ref mBaseCanvas, -50, Color.FromRgb(255, 255, 255));
            CompCentralText cct2 = new CompCentralText();
            cct2.PutTextToCentralScreen("请在做心算题的同时\r\n记住随后出现的属相", "KaiTi",
                38, ref mBaseCanvas, 50, Color.FromRgb(255, 255, 255));

            mCurrentStatus = PageAttr.test;
            //new FEITClickableScreen(ref mBaseCanvas, nextStep);
            CompBtnNextPage btnGO = new CompBtnNextPage("开始测验");
            btnGO.Add2Page(mBaseCanvas, FEITStandard.PAGE_BEG_Y + 550);
            btnGO.mfOnAction = nextStep;
        }

        public void nextStep()
        {

            if (mCurrentStatus == PageAttr.title)////////////////////////////////////////////////////////////////////////////////////////////////
            {
                loadTitlePage();
            }
            else if (mCurrentStatus == PageAttr.practiseChar)
            {
                loadPractiseChar();
            }
            else if (mCurrentStatus == PageAttr.practiseEquation)
            {
                loadPractiseEquation();
            }
            else if (mCurrentStatus == PageAttr.instructCompreh)
            {
                loadInstructionComprehensivePrac();
            }
            else if (mCurrentStatus == PageAttr.practise)
            {
                if (mOrgPrac == null)
                {
                    loadPractisePage();
                }
                else
                {
                    mOrgPrac.nextStep();
                }
            }
            else if (mCurrentStatus == PageAttr.instruction2)
            {
                loadInstructionPage2();

            }
            else if (mCurrentStatus == PageAttr.test)
            {
                if (mOrgTest == null)
                {
                    loadTestPage();
                }
                else
                {
                    mOrgTest.nextStep();
                }
            }
            else if (mCurrentStatus == PageAttr.finish)
            {
                loadFinishPage();
            }
        }

        public void nextStep(object obj)
        {
            nextStep();
        }
    }
}

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

        public static string interFilename = "OpSpanInterRep.csv";
        public static string orderFilename = "OpSpanOrderRep.csv";

        public enum PageAttr
        {
            title, instruction, instructCompreh, practise, practiseChar, practiseEquation, instruction2, test, finish
        };

        public PageOpSpan(MainWindow mainWindow)
        {
            InitializeComponent();
            mMainWindow = mainWindow;
            mGroupArrangement = new int[] { 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 };
            mGroupArrangementPrac = new int[] { 2, 2, 3, 3 };

            mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);

            mRecorder = new RecorderOpSpan(this);
            mTimer = new FEITTimer();
        }

        public static int getSubGroupID(int numInArray)
        {
            return (numInArray % 3) + 1;
        }

        private void mBaseCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref mBaseCanvas);
            ClearAll();

            //prepare data structures
            mGroups = GenGroups((new LoaderOpSpan("OPSPAN.csv")).GetTrails(), mGroupArrangement);

            mGroupsPrac = 
                GenGroups((new LoaderOpSpan("OPSPANPrac.csv")).GetTrails(), mGroupArrangementPrac);

            //replace equations here:

            mCurrentStatus = PageAttr.title;

            //build BD
            int individualUnitCount = 0;
            for (int i = 0; i < mGroupArrangement.Length; i++)
            {
                individualUnitCount += mGroupArrangement[i];
            }

            if (!mMainWindow.mDB.TableExists(Names.OPSAPN_EXPRESSION_TABLENAME))
            {
                mMainWindow.mDB.CreateOpSpanExpressionTable(individualUnitCount);
            }

            if (!mMainWindow.mDB.TableExists(Names.OPSPAN_ORDER_TABLENAME))
            {
                mMainWindow.mDB.CreateOPSpanOrderTable(mGroupArrangement.Length);
            }
            
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
            mLayoutInstruction.addInstruction(240, 0, 798, 200, 
                "    请在做心算题的同时，记住随后出现的属相（十二生肖）。", 
                "KaiTi",
                38, Color.FromRgb(255, 255, 255));

            mCurrentStatus = PageAttr.instruction;

            //test////////////////////////////////////////////////////for stage control
            //mCurrentStatus = PageAttr.instructCompreh;
            //test above//////////////////////////////////////////////

            new FEITClickableScreen(ref mBaseCanvas, nextStep);
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

            cct3.PutTextToCentralScreen("点击鼠标继续", "KaiTi",
                50, ref mBaseCanvas, 30, Color.FromRgb(255, 255, 255));

            new FEITClickableScreen(ref mBaseCanvas, exitOpSpan);
        }

        private void exitOpSpan()
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
        }

        private void loadPractiseEquation()
        {
            OrganizerPractiseEquation ope = new OrganizerPractiseEquation(this);
            mCurrentStatus = PageAttr.instructCompreh;
            ope.mfNext();
        }

        private void loadInstructionPage()
        {
            ClearAll();

            mLayoutInstruction.addInstruction(240, 0, 
                FEITStandard.PAGE_WIDTH - 200, 300, 
                "下面是单项练习，点击鼠标继续。", 
                "KaiTi", 40, Color.FromRgb(255, 255, 255));

            //CompCentralText cct

            mCurrentStatus = PageAttr.practiseChar;
            new FEITClickableScreen(ref mBaseCanvas, nextStep);
        }

        private void loadInstructionComprehensivePrac()
        {
            ClearAll();
            mLayoutInstruction.addInstruction(240, 0,
                FEITStandard.PAGE_WIDTH - 200, 300,
                "下面是综合练习，将两项任务（记属相，做心算）结合起来练习。请在做心算题的同时，记住随后出现的属相。点击鼠标开始。",
                "KaiTi", 40, Color.FromRgb(255, 225, 255));

            mCurrentStatus = PageAttr.practise;

            new FEITClickableScreen(ref mBaseCanvas, 
                blackAfterInstructionComprehensicePrac);
        }

        private delegate void timedele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(nextStep));
        }

        private void blackAfterInstructionComprehensicePrac()
        {
            ClearAll();

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 1000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        private void loadInstructionPage2()
        {
            ClearAll();

            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen("下面是正式测验，点击鼠标开始。", "KaiTi", 
                50, ref mBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            mCurrentStatus = PageAttr.test;
            new FEITClickableScreen(ref mBaseCanvas, nextStep);
        }

        public void nextStep()
        {

            if (mCurrentStatus == PageAttr.title)////////////////////////////////////////////////////////////////////////////////////////////////
            {
                loadTitlePage();
            }
            else if (mCurrentStatus == PageAttr.instruction)
            {
                loadInstructionPage();
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
    }
}

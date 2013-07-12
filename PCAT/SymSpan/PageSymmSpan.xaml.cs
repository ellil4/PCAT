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

namespace FiveElementsIntTest.SymSpan
{
    /// <summary>
    /// PageSymmSpan.xaml 的互動邏輯
    /// </summary>
    public partial class PageSymmSpan : Page
    {
        public MainWindow mMainWindow;
        public LayoutInstruction mLayoutInstruction;
        public int[] mTestGroup;
        private int[] mPracGroup;
        private List<TrailsGroupSS> mTest;
        private List<TrailsGroupSS> mPrac;
        public StatusSS mStatus;

        public FEITTimer mTimer;

        private List<AnswerSSST> mGroupsAnswer;//one element for each group

        public static string interFilename = "SymmInterRep.csv";
        public static string posFilename = "SymmPosRep.csv";

        public enum StatusSS
        {
            main_title, instruction, practise, instruction2, test, finish
        }

        public PageSymmSpan(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);
            mTestGroup = new int[] { 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8 };
            mPracGroup = new int[] { 2, 2, 2, 3, 3, 3 };

            LoaderSymmSpan loader = new LoaderSymmSpan("SYMM\\SYMM.txt");
            List<TrailSS_ST> trails = loader.GetTrails();
            
            RandomSelector testRanSel = new RandomSelector(trails, mTestGroup);
            mTest = testRanSel.Get();

            RandomSelector pracRanSel = new RandomSelector(trails, mPracGroup);
            mPrac = pracRanSel.Get();

            mGroupsAnswer = new List<AnswerSSST>();

            mStatus = StatusSS.main_title;//test could be from here

            mTimer = new FEITTimer();

            if (!mMainWindow.mDB.TableExists(Names.SYMSPAN_POS_TABLENAME))
            {
                mMainWindow.mDB.CreateSymSpanPosTable(mTestGroup.Length);
            }

            if (!mMainWindow.mDB.TableExists(Names.SYMSPAN_SYMM_TABLENAME))
            {
                mMainWindow.mDB.CreateSymSpanSymmTable(trails.Count);
            }
        }


        public static int getSubGroupID(int numInArray)
        {
            return (numInArray % 3) + 1;
        }

        public void nextStep()
        {
            switch (mStatus)
            {
                case StatusSS.main_title:
                    loadMainTitle();
                    break;
                case StatusSS.instruction:
                    loadInstruction();
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

        private void loadMainTitle()
        {
            ClearAll();
            mLayoutInstruction.addTitle(70, 0, "对称广度", "KaiTi", 52, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(133, 105, "Operation Span", "Batang", 32, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(240, 0, "按鼠标键继续", "KaiTi", 30, Color.FromRgb(255, 255, 255));

            mStatus = StatusSS.instruction;
            new FEITClickableScreen(ref mBaseCanvas, nextStep);
        }

        private void loadInstruction()
        {
            new OrganizerInstruction(this);
        }

        private void loadPractise()
        {
            OrganizerTrailSS ots = 
                new OrganizerTrailSS(this, true, mPrac, ref mGroupsAnswer);
            mStatus = StatusSS.instruction2;
            ots.nextStep();
        }

        private void loadInstruction2()
        {
            ClearAll();
            mLayoutInstruction.addTitle(240, 0, "以下是正式测验,按任意键继续", "KaiTi", 30, Color.FromRgb(255, 255, 255));
            
            mStatus = StatusSS.test;
            
            new FEITClickableScreen(ref mBaseCanvas, nextStep);
        }

        private void loadTest()
        {
            OrganizerTrailSS ots =
                new OrganizerTrailSS(this, false, mTest, ref mGroupsAnswer);
            
            mStatus = StatusSS.finish;

            mTimer.Start();
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

            //undifined

            //loadMainTitle();

            loadMainTitle();

            //ots.showPosistionPage(10);
            //ots.showOrderPage();

            //ots.showReportPage();
            //ots.showWarningPage();
            //ots.showDualDeterPage();
            //ots.showTitlePage();
        }
    }
}

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

namespace FiveElementsIntTest.PairedAsso
{
    /// <summary>
    /// PagePairedAsso.xaml 的互動邏輯
    /// </summary>
    public partial class PagePairedAsso : Page
    {
        public MainWindow mMainWindow;

        public static int mGroupLen = 12;
        public int mGrpAt = 0;
        public int mItemInGrpAt = 0;

        public List<List<StPair>> mLearningItems;
        public List<List<StTest>> mTestItems;
        private PairedAssoStep mStep;

        public List<long> mRTs;
        public List<List<int>> mOrders;

        public PagePairedAsso(MainWindow mw)
        {
            InitializeComponent();
            mMainWindow = mw;

            mLearningItems = ReaderPairedAsso.GetLearningItems();
            mTestItems = ReaderPairedAsso.GetTestItems();

            mStep = PairedAssoStep.title;
            mOrders = new List<List<int>>();
            mRTs = new List<long>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amBaseCanvas);
            
            /*CompChinese9Cells comp = new CompChinese9Cells(this);
            String[] str = new String[] {"人", "口", "大", "小", "汉", "字", "简", "繁", "英"};
            comp.SetCharas(str);
            amBaseCanvas.Children.Add(comp);
            Canvas.SetTop(comp, 100);
            Canvas.SetLeft(comp, 750);*/

            //OrganizerTest ot = new OrganizerTest(this, mTestItems[0], "(一)");
            //ot.Begin();

            //OrganizerLearning ol = new OrganizerLearning(this, mLearningItems[0]);
            //ol.Begin();

            next();

        }

        private void showTitle()
        {
            clearAll();
            Title comp = new PairedAsso.Title();
            amBaseCanvas.Children.Add(comp);
            Canvas.SetTop(comp, FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT - 600) / 2);
            Canvas.SetLeft(comp, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - 800) / 2);

            //new FEITClickableScreen(ref amBaseCanvas, next);
            CompBtnNextPage btn = new CompBtnNextPage("开始");
            btn.Add2Page(amBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = next;
        }

        private void showInstruction()
        {
            clearAll();

            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("如，学习阶段记忆“太阳-月亮”，测试时会出现，",
                "KaiTi", 32, ref amBaseCanvas, -250, Color.FromRgb(255, 255, 255));

            CompChinese9Cells cells = new CompChinese9Cells(this);
            String[] str = new String[] {"行", "地", "气", "热", "星", "月", "亮", "球", "圆"};
            cells.SetCharas(str);
            cells.SetQuest("太阳");
            cells.mfConfirm = instructionInteractionJudge;
            amBaseCanvas.Children.Add(cells);
            Canvas.SetTop(cells, FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT - 515) / 2 + 50);
            Canvas.SetLeft(cells, FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - 800) / 2);

            CompCentralText ct2 = new CompCentralText();
            ct2.PutTextToCentralScreen("此时，用鼠标依次点击“月”“亮”二字即可完成作答。\r\n请点击“月”“亮”， 然后点击“确定”进入下一页。",
                "KaiTi", 32, ref amBaseCanvas, 300, Color.FromRgb(255, 255, 255));
        }

        private void instructionInteractionJudge(object obj)
        {
            CompChinese9Cells compCell = (CompChinese9Cells)obj;
            if (compCell.GetCharas().Equals("月亮"))
            {
                next();
            }
        }

        private void showLearning(int index)
        {
            clearAll();

            OrganizerLearning ol = new OrganizerLearning(this, mLearningItems[index]);
            ol.Begin();
        }

        private void showTest(int index, String charNum)
        {
            clearAll();
            OrganizerTest ot = new OrganizerTest(this, mTestItems[index], charNum);
            ot.Begin();
        }

        private void showEndPage()
        {
            PARecorder rec = new PARecorder(this);
            rec.outputReport(FEITStandard.GetExePath() + "Report\\pa\\" + 
                mMainWindow.mDemography.GenBriefString() + ".txt");
            clearAll();
            CompCentralText ct = new CompCentralText();
            ct.PutTextToCentralScreen("本测验结束",
                "KaiTi", 32, ref amBaseCanvas, 0, Color.FromRgb(255, 255, 255));

            //new FEITClickableScreen(ref amBaseCanvas, close);
            CompBtnNextPage btn = new CompBtnNextPage("结束测验");
            btn.Add2Page(amBaseCanvas, FEITStandard.PAGE_BEG_Y + 470);
            btn.mfOnAction = close;
        }

        private void close(object obj)
        {
            close();
        }

        private void close()
        {
            Environment.Exit(0);
        }

        public void clearAll()
        {
            amBaseCanvas.Children.Clear();
        }

        public void next(object obj)
        {
            next();
        }

        public void next()
        {
            switch (mStep)
            {
                case PairedAssoStep.title:
                    showTitle();
                    break;
                case PairedAssoStep.instruction:
                    showInstruction();
                    break;
                case PairedAssoStep.learning1:
                    showLearning(0);
                    break;
                case PairedAssoStep.test1:
                    showTest(0, "(一)");
                    break;
                case PairedAssoStep.learning2:
                    showLearning(1);
                    break;
                case PairedAssoStep.test2:
                    showTest(1, "(二)");
                    break;
                /*case PairedAssoStep.learning3:
                    showLearning(2);
                    break;
                case PairedAssoStep.test3:
                    showTest(2, "(三)");
                    break;*/
                case PairedAssoStep.end:
                    showEndPage();
                    break;
            }

            mStep++;
        }
    }

    public enum PairedAssoStep
    {
        title, instruction, learning1, test1, learning2, test2, /*learning3, test3,*/ end
    }
}

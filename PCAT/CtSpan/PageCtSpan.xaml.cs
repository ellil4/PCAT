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

namespace FiveElementsIntTest.CtSpan
{
    /// <summary>
    /// PageCtSpan.xaml 的互動邏輯
    /// </summary>
    public partial class PageCtSpan : Page
    {
        private MainWindow mMainWindow;
        private GraphControl mGC;

        public Proccess mStage = Proccess.Title;
        public Organizer mOrganizer;
        public ItemReader mReader;
        public List<StGraphItem> mItems;
        public List<StResult> mResults;
        public TestLoop mTL;

        public int mSubGrpAt = 0;
        public int mGrpAt = 0;
        public int mTotalItemIndexAt = 0;
        public int mTotalSpanIndexAt = 0;

        public int[] SpanArrangement = 
            { 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9 };
        
        public Random mRDM;

        public PageCtSpan(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            mGC = new GraphControl(this);
            mOrganizer = new Organizer(this);
            
            /*mItems = (new ItemReader(
                System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "CtSpan.txt")).mItems;*/

            mRDM = new Random();
            mItems = randomItemListGenerator(140);
            mResults = new List<StResult>();
            mTL = new TestLoop(this);

            //DB Here
            //....
        }

        private static int[] triangleCountArr = { 1, 3, 5, 7, 9};

        public StGraphItem GenOneRandomItem(short distanceTar, short distanceComm)
        {
            StGraphItem retval = new StGraphItem();
            
            retval.DistanceComm = distanceComm;
            retval.DistanceTar = distanceTar;
            retval.TarCount = mRDM.Next(3, 10);
            retval.InterCircleCount = mRDM.Next(1, 6);
            int triangleIdx = mRDM.Next(0, 5);
            retval.InterTriCount = triangleCountArr[triangleIdx];

            return retval;
        }

        private List<StGraphItem> randomItemListGenerator(int count)
        {
            List<StGraphItem> retval = new List<StGraphItem>();
            
            for (int i = 0; i < count; i++)
            {
                retval.Add(GenOneRandomItem(120, 60));
            }
            
            return retval;
        }

        public void ClearAll()
        {
            amBaseCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref amBaseCanvas,
                ref amAuxBorder, ref amAuxBorder1024, ref mMainWindow);

            if (mMainWindow.mbEngiMode)
            {
                amBaseCanvas.Children.Add(amSubareaBorder);
                Canvas.SetTop(amSubareaBorder, GraphControl.SUBAREA_BEG_Y);
                Canvas.SetLeft(amSubareaBorder, GraphControl.SUBAREA_BEG_X);
            }
        }

        public void NextStage()
        {
            switch (mStage)
            {
                case Proccess.Title:
                    mStage = Proccess.Instruct;
                    mOrganizer.ShowTestTitle();
                    break;
                case Proccess.Instruct:
                    mStage = Proccess.Exercise;
                    mOrganizer.ShowInstruction();
                    break;
                case Proccess.Exercise:
                    mStage = Proccess.Instruct2;
                    mTL.SetFormal(false);
                    mTL.StageIteration();
                    break;
                case Proccess.Instruct2:
                    mStage = Proccess.Test;
                    mOrganizer.ShowInstruction2();
                    mTL.mStage = InTestStage.SubTitle;
                    break;
                case Proccess.Test:
                    mTL.SetFormal(true);
                    mTL.StageIteration();
                    break;
            }
        }

        private void amBaseCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amBaseCanvas);
            ClearAll();
            Focus();

            NextStage();

            //test
            /*LayoutInstruction li = new LayoutInstruction(ref amBaseCanvas);
            li.addTitle(70, 0, "计数广度模块已加载", "KaiTi", 52, Color.FromRgb(255, 255, 255));*/

            /*GraphicToken gt = new GraphicToken(TokenType.TRIANGLE);
            amBaseCanvas.Children.Add(gt);
            Canvas.SetLeft(gt, 100);
            Canvas.SetTop(gt, 200);*/

            /*int count = 0;
            while (true)
            {
                mGC.DrawScene(9, 9, 5, 120, 60);
                Console.WriteLine(count.ToString());
                count++;
            }*/
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.Enter)
            {
                mGC.DrawScene(9, 9, 5, 200, 60);
            }

            if (e.Key == Key.Space)
            {
                ClearAll();
                CompCountRecallPan pan = new CompCountRecallPan();
                amBaseCanvas.Children.Add(pan);
                Canvas.SetTop(pan, FEITStandard.PAGE_BEG_Y + 200);
                Canvas.SetLeft(pan, FEITStandard.PAGE_BEG_X);
            }*/
        }

        public void Quit()
        {
            //writer save
            new FormWriter(mItems, mResults);
            //quit
            Environment.Exit(0);
        }
    }
}

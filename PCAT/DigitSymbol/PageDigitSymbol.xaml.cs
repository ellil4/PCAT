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
using System.IO;
using PCATData;
using Network;

namespace FiveElementsIntTest
{
    /// <summary>
    /// PageDigitSymbol.xaml 的互動邏輯
    /// </summary>
    public partial class PageDigitSymbol : Page
    {
        public MainWindow mMainWindow;
        protected LayoutDigitSymbol mLayout;
        protected LayoutInstruction mLayoutInstruction;
        protected ListenerDigitSymbol mListener;
        protected LoaderDigitSymbol mLoader;
        protected ArrayList mLoadedNums;
        
        public ResHolderDigitSymbol mResHolder;
        
        public ArrayList mSymbols;
        public int mElemFocus;
        public bool mElemFocusLeft;

        public DIGIT_SYMBOL_STEP mCurrentStep;

        public FEITTimer mTimer;

        public bool mbExercise = true;


        public enum DIGIT_SYMBOL_STEP
        {
            ready, instruction, excercise, instruction2,
            testPage1, testPage2, testPage3, testPage4, testPage5, report
        };

        private bool IsInputPage(DIGIT_SYMBOL_STEP p)
        {
            if (p == DIGIT_SYMBOL_STEP.excercise || p == DIGIT_SYMBOL_STEP.testPage1 ||
                p == DIGIT_SYMBOL_STEP.testPage2 || p == DIGIT_SYMBOL_STEP.testPage3 ||
                p == DIGIT_SYMBOL_STEP.testPage4 || p == DIGIT_SYMBOL_STEP.testPage5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public PageDigitSymbol(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            mSymbols = new ArrayList();

            mLayout = new LayoutDigitSymbol(this);
            mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);

            this.Focus();

            mElemFocus = 0;
            mElemFocusLeft = true;
            mListener = new ListenerDigitSymbol(this);

            mLoader = new LoaderDigitSymbol();
            mLoadedNums = mLoader.GetContentDigits();

            mResHolder = new ResHolderDigitSymbol();

            mCurrentStep = DIGIT_SYMBOL_STEP.ready;

            mTimer = new FEITTimer();

            //undefined
            //DB
            /*if (!mMainWindow.mDB.TableExists(Names.DIGIT_SYMBOL_TABLENAME))
            {
                mMainWindow.mDB.CreateDigiSymbolTable(LayoutDigitSymbol.ELEMENT_PER_PAGE * 5);
            }*/
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref mBaseCanvas);
            nextStep();
        }

        public void ClearAll()
        {
            mBaseCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref mBaseCanvas, ref mAuxBorder, ref mAuxBorder1024, ref mMainWindow);
        }

        public void nextStep()
        {
            ClearAll();
            mSymbols.Clear();

            switch (mCurrentStep)
            {
                case DIGIT_SYMBOL_STEP.ready:
                    loadInstructionPage();
                    break;
                case DIGIT_SYMBOL_STEP.instruction:
                    loadExcercisePage();
                    break;
                case DIGIT_SYMBOL_STEP.excercise:
                    loadInstructionPage2();
                    break;
                case DIGIT_SYMBOL_STEP.instruction2:
                    mTimer.Start();
                    loadTestPage(0);
                    break;
                case DIGIT_SYMBOL_STEP.testPage1:
                    loadTestPage(1);
                    break;
                case DIGIT_SYMBOL_STEP.testPage2:
                    loadTestPage(2);
                    break;
                case DIGIT_SYMBOL_STEP.testPage3:
                    loadTestPage(3);
                    break;
                case DIGIT_SYMBOL_STEP.testPage4:
                    loadTestPage(4);
                    mTimer.Stop();
                    break;
                case DIGIT_SYMBOL_STEP.testPage5:
                    laodReport();                    
                    break;
                case DIGIT_SYMBOL_STEP.report:
                    exitDigitSynbol();
                    break;
            }


            mCurrentStep++;
        }

        private void loadTestPage(int pageNum)
        {
            mbExercise = false;

            mLayout.addLegend(20, 7);
            mLayout.FillPage(mLoadedNums, 
                pageNum * LayoutDigitSymbol.ELEMENT_PER_PAGE, 
                CompDigiSymbol.OUTHEIGHT + 130);

            ((CompDigiSymbol)mSymbols[0]).SetCursor(CURSOR_STATUS.LEFT);
        }

        private void loadExcercisePage()
        {
            mbExercise = true;

            mLayout.addLegend(20, 7);
            int[] arr = { 4, 2, 9, 6, 1, 3, 7, 8, 5, 3 };
            
            for (int i = 0; i < arr.Length; i++ )
            {
                mLayout.addTestField(arr[i], i, 0, 310, 0);
            }

            ((CompDigiSymbol)mSymbols[0]).SetCursor(CURSOR_STATUS.LEFT);
                
        }

        private void loadInstructionPage()
        {

            mLayoutInstruction.addTitle(50, 0, "数字符号", "KaiTi", 48, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(113, 92, "Digit Symbol", "Batang", 32, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addInstruction(200, 0, FEITStandard.PAGE_WIDTH / 100 * 98, 300,
                "    先熟悉键盘上的反应键。按照屏幕上方给出的数字-符号对的对应关系，在数字下的空格内填入相应的符号对。\n\r    现在开始练习。", "Kaiti", 35, Color.FromRgb(255, 255, 255));
        }

        private void loadInstructionPage2()
        {
            mLayoutInstruction.addTitle(276, 0, "现在开始测试", "Microsoft YaHei", 48, Color.FromRgb(255, 255, 255));
        }

        private void laodReport()
        {
            String showtext = "一共" + mLoadedNums.Count + "道题，答对" + mListener.mEvalue.CorrectCount() +
                "道题,共用时" + mTimer.GetElapsedTime() + "毫秒。 \r\n（这是一个测试页面。）";

            mLayoutInstruction.addInstruction(200, 0, 794, 300, showtext, "KaiTi", 32, Color.FromRgb(34, 177, 76));
        }

        private void PCATDataSaveReport()
        {
            int len = mListener.mEvalue.mTF.Count;
            List<QRecDigiSymbol> info = new List<QRecDigiSymbol>();
            for (int i = 0; i < len; i++)
            {
                QRecDigiSymbol oneInfo = new QRecDigiSymbol();
                
                oneInfo.Stim = (int)mLoadedNums[i];
                oneInfo.Left = mListener.mEvalue.mLeftAnswer[i];
                oneInfo.Right = mListener.mEvalue.mRightAnswer[i];
                oneInfo.Correctness = mListener.mEvalue.mTF[i];

                info.Add(oneInfo);
            }

            //undefined
            //mMainWindow.mDB.AddDigiSymbolRecord(info, mMainWindow.mUserID);
        }

        private void exitDigitSynbol()
        {
            //OutputDigitSymbol ods = new OutputDigitSymbol();
            //if (!Directory.Exists(FEITStandard.GetRepotOutputPath()))
            //    Directory.CreateDirectory(FEITStandard.GetRepotOutputPath());

            //ods.Output(FEITStandard.GetRepotOutputPath() + "DigitSymbolReport.csv", ref mListener.mEvalue, ref mTimer);

            PCATDataSaveReport();
            mMainWindow.TestForward();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsInputPage(mCurrentStep))
            {
                mListener.pressed(e);
            }
            else
            {
                if(e.Key == Key.Space)
                    nextStep();
            }
        }  

    }
}

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
using System.Timers;
using FiveElementsIntTest.DigitSymbol;

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

        DigiPointPageOneControl _DigiPointPageOneControl;
        
        DigiPointPageTwoControl _DigiPointPageTwoControl;
        
        public ResHolderDigitSymbol mResHolder;
        
        public ArrayList mSymbols;
        public int mElemFocus; //小三角焦点位移
        public bool mElemFocusLeft;

        public DIGIT_SYMBOL_STEP mCurrentStep;

        public FEITTimer mTimer;

        public bool mbExercise = true;

        public bool _ControlExercise = true;//练习对比

        //private int exer_num = 3;
        private Timer _time_test;//测试总时间

        public int _second_chage;
        public enum DIGIT_SYMBOL_STEP
        {
            ready, instruction, excercise, instruction2,instruction3,
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

            _DigiPointPageOneControl = new DigiPointPageOneControl();

            _DigiPointPageTwoControl = new DigiPointPageTwoControl();
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
                    _second_chage = 1000;
                     loadExcercisePage();
                    break;
                case DIGIT_SYMBOL_STEP.excercise:
                   // layoutPointTwo();
                    _ControlExercise = false;//关闭练习对比功能
                    layoutPointOne();
                    break;
                case DIGIT_SYMBOL_STEP.instruction2:
                    layoutPointTwo();
                    _second_chage = 10;
                    break;
                case DIGIT_SYMBOL_STEP.instruction3:
                    testTotalTime();
                    _time_test.Start();
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
                    _time_test.Stop();
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

           // ((CompDigiSymbol)mSymbols[0]).SetCursor(CURSOR_STATUS.LEFT);
           
            // 修改指针位置
            ((CompDigiSymbol)mSymbols[0]).FillpiontAnswer(mListener.mEvalue.SetValue(), mListener.mEvalue.SetValueOne());
            mElemFocus = 1;
            ((CompDigiSymbol)mSymbols[1]).FillpiontAnswer(mListener.mEvalue.SetValue(), mListener.mEvalue.SetValueOne());
            mElemFocus = 2;
            ((CompDigiSymbol)mSymbols[2]).SetCursor(CURSOR_STATUS.LEFT);
            
            //鼠标隐藏
            mBaseCanvas.Cursor = Cursors.None;
            //compareTo();
            //_t_Contrast.Start();  //开始练习对比功能
        }

        private void loadInstructionPage()
        {

            mLayoutInstruction.addTitle(50, 0, "数字符号", "KaiTi", 48, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(113, 92, "Digit Symbol", "Batang", 32, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addInstruction(250, 25, FEITStandard.PAGE_WIDTH / 100 * 98, 300,
                "       屏幕上方将呈现一些 “数字-符号” 对，\n请在数字下的空格内填入对应的符号。   ", 
                "Kaiti", 32, Color.FromRgb(255, 255, 255), true);

            mLayoutInstruction.addTitle(500, 15, "(按空格键开始看实例、做练习）", "KaiTi", 32, Color.FromRgb(255, 255, 255));
        
        }

        private void loadInstructionPage2()
        {
            mLayoutInstruction.addTitle(276, 0, "现在开始测试", "Microsoft YaHei", 48, Color.FromRgb(255, 255, 255));
        }

        private void laodReport()
        {
            //显示鼠标

            mBaseCanvas.Cursor = Cursors.AppStarting;


            String showtext = "一共" + mLoadedNums.Count + "道题，答对" + mListener.mEvalue.CorrectCount() +
                "道题,共用时" + mTimer.GetElapsedTime() + "毫秒。 \r\n";

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
            OutputDigitSymbol ods = new OutputDigitSymbol();
            if (!Directory.Exists(FEITStandard.GetRepotOutputPath()))
                Directory.CreateDirectory(FEITStandard.GetRepotOutputPath());

            ods.Output(FEITStandard.GetRepotOutputPath() + "DigitSymbol\\" +
                mMainWindow.mDemography.GenBriefString() + ".txt", 
                ref mListener.mEvalue, ref mTimer);

            //PCATDataSaveReport();
            mMainWindow.TestForward();
          //mMainWindow.TestForward(); 版本四中若要提示测试结束 则隐去
        }

        private void layoutPointOne()
        {
            
            mBaseCanvas.Children.Add(_DigiPointPageOneControl);
            Canvas.SetLeft(_DigiPointPageOneControl, 0);
            Canvas.SetTop(_DigiPointPageOneControl,  0);
        }

        private void layoutPointTwo()
        {
            mBaseCanvas.Children.Add(_DigiPointPageTwoControl);
            Canvas.SetLeft(_DigiPointPageTwoControl, 0);
            Canvas.SetTop(_DigiPointPageTwoControl, 0);
        }


        //=====================================测试总时间==============================

        private void testTotalTime()
        {
            _time_test = new Timer(120000);//120000
            _time_test.AutoReset = false;
            _time_test.Enabled = true;
            _time_test.Elapsed += new ElapsedEventHandler(_time_test_Elapsed);
        
        }

        void _time_test_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBaseCanvas.Dispatcher.Invoke(new displayblanktime(tt_processing));
        }
        
        private delegate void displayblanktime();

        private void tt_processing() //超时跳出测试
        {
            mTimer.Stop();

            exitDigitSynbol();

            //是否要提示
            mBaseCanvas.Children.Clear();


            mLayoutInstruction.addTitle(250, 100, "本 项 测 验 结 束", "KaiTi", 36, Color.FromRgb(255, 255, 255));
       
        }

        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsInputPage(mCurrentStep))
            {
                mListener.pressed(e);
            }
            else
            {
                if (e.Key == Key.Space)
                    nextStep();
            }
        }  




        //private void compareTo()
        //{
        //    _t_Contrast = new Timer();
        //    _t_Contrast.AutoReset = false;
        //    _t_Contrast.Enabled = true;
        //    _t_Contrast.Elapsed +=new ElapsedEventHandler(_t_Contrast_Elapsed);

        //}

        //void  _t_Contrast_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    mBaseCanvas.Dispatcher.Invoke(new displayFouce(df_processing)); 
        //}
        //private delegate void displayFouce();

        //private void df_processing() //超时跳出测试
        //{
        //    if (!mListener.mEvalue.ContrastValue())
        //    {
        //        if(exer_num % 2 !=0)
        //        {
                    
        //        }
        //        else if (exer_num % 2 == 0)
        //        {
                    
        //        }

        //    }
        //    else
        //    {
        //        exer_num++;
        //    }

        //}
    }//class
}

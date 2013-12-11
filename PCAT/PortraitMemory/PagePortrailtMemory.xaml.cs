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

namespace FiveElementsIntTest.PortraitMemory
{
    /// <summary>
    /// PagePortrailtMemory.xaml 的交互逻辑
    /// </summary>
    public partial class PagePortrailtMemory : Page
    {
        public MainWindow mMainWindow;
        protected LayoutInstruction mLayoutInstruction;

        /************************界面元件*****************************/
        private Label _next_page;
        private Label _begin_test;
        private Label _nextQuestion;
        private Label _promptlabel;
        private Label _close_win;
      //  int _width = ((int)System.Windows.SystemParameters.PrimaryScreenWidth) / 2 - 50;
        /******************************提示************************************/

        public Label _tip_display;

        public Timer _t_Display;

        public Timer _flash_Display;
        /*****************************学习*****************************/
        
        MemoryControl mMemoryControl;
        
        private bool _remember = false;

        private Timer _t_Total_Time;
        /****************************开始按钮*********************************/

        private static int _startButtomStep = 0; 


        /******************************测试界面*********************************/
        PortrailtMemoryTestControl mPortrailtMemoryTestControl;
        TestReader mTestReader;

        private int _line_num = 0;

        private Timer _time_blank;

        private int colTestTwo = 0;
        
        /*******************************记录***************************************/
        String _rt = " "; //反应时间
        Stopwatch _rtime;//每道题花费时间
        long _runtime = 0;

        long _accountRT = 0;

        public String _NameAnswer = "";
        public String _JobAnswer = "";
        public String _LikeAnswer = "";

        private List<String> _optionAnswers;

        private List<List<String>> _answers = new List<List<string>>();

        /**************************结果**************************/
        TestResult mTestResult;

        public PagePortrailtMemory(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            this.Focus();
            mLayoutInstruction = new LayoutInstruction(ref PortCanvas);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref PortCanvas);
            // nextInstructionPage();
            loadIniPage();
            
        }

        private void loadIniPage() // 首页
        {

            mLayoutInstruction.addTitle(50, 0, "人像特点联系回忆", "KaiTi_GB2312", 48, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(113, 110, "Portrait Memory", "", 28, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addInstruction(200, 10, FEITStandard.PAGE_WIDTH / 100 * 68, 400,
                "    你将看到一些人像，同时还要记住他姓什么，做什么工作，爱好什么。请将人像和他的三个特点联系起来记忆。 \r    看完全部人像后，会有一个测试，当再看到每一张人像时，请选出他姓什么，做什么工作和有什么爱好。 ", "Kaiti_GB2312", 30, Color.FromRgb(255, 255, 255));
            
            chageNextLabel();

        }

        private void loadStartPage()
        {
            //mLayoutInstruction.addTitle(50, 0, "人像特点联系回忆", "KaiTi_GB2312", 48, Color.FromRgb(255, 255, 255));
            //mLayoutInstruction.addTitle(113, 110, "Portrait Memory", "", 28, Color.FromRgb(255, 255, 255));

            _promptlabel.Visibility = System.Windows.Visibility.Visible;
            _promptlabel.Content = "下 面 请 注 意 记 ！";

            startTestLabel();
        }

        private void loadFirstTest()
        {
            //mLayoutInstruction.addTitle(50, 0, "人像特点联系回忆", "KaiTi_GB2312", 48, Color.FromRgb(255, 255, 255));
            //mLayoutInstruction.addTitle(113, 110, "Portrait Memory", "", 28, Color.FromRgb(255, 255, 255));

            _promptlabel.Visibility = System.Windows.Visibility.Visible;
            _promptlabel.Content = "记 忆 测 试 （一）";

            startTestLabel();

        }
        private void loadSecondPage()
        {
            //mLayoutInstruction.addTitle(50, 0, "人像特点联系回忆", "KaiTi_GB2312", 48, Color.FromRgb(255, 255, 255));
            //mLayoutInstruction.addTitle(113, 110, "Portrait Memory", "", 28, Color.FromRgb(255, 255, 255));
            _promptlabel.Visibility = System.Windows.Visibility.Visible;
            _promptlabel.Content = "下 面 再 记 一 遍 ！";

            startTestLabel();
        }
        private void loadSecondTest()
        {
            //mLayoutInstruction.addTitle(50, 0, "人像特点联系回忆", "KaiTi_GB2312", 48, Color.FromRgb(255, 255, 255));
            //mLayoutInstruction.addTitle(113, 110, "Portrait Memory", "", 28, Color.FromRgb(255, 255, 255));
            _promptlabel.Visibility = System.Windows.Visibility.Visible;
            _promptlabel.Content = "记 忆 测 试（二）";

            startTestLabel();

        }

        private void laodReport()
        {
            

            String showtext = "一共" + mTestReader._TestList.Count + "道题,完整做对" + mTestResult._Account + "道题；共用时" + _accountRT + "毫秒";

            mLayoutInstruction.addInstruction(160, 90, 794, 450, showtext, "result", 32, Color.FromRgb(34, 177, 76));
            _close_win = new Label();
            _close_win.Height = 50;
            _close_win.Width = 250;
            _close_win.Foreground = Brushes.White;
            _close_win.FontSize = 24.0;
            _close_win.FontFamily = new FontFamily("KaiTi_GB2312");
            _close_win.Content = "关闭测试";
            _close_win.MouseLeftButtonDown += new MouseButtonEventHandler(_close_win_MouseLeftButtonDown);
            PortCanvas.Children.Add(_close_win);
            Canvas.SetLeft(_close_win, FEITStandard.PAGE_BEG_X + 522);
            Canvas.SetTop(_close_win, FEITStandard.PAGE_BEG_Y + 300);

            // mMainWindow.Closing = null;
        }
        void _close_win_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mMainWindow.Close();
        }

        

        /**************************界面元素******************************/

        //chageNextPage()
        //startTest()
        //nextQuestionLabel()

        /*********************************************************/
        private void chageNextLabel()
        {
            _next_page = new Label();

            _next_page.Height = 38;
            _next_page.Width = 100;
            _next_page.Foreground = Brushes.White;

            PortCanvas.Children.Add(_next_page);
            _next_page.FontSize = 24.0;
            _next_page.Content = "下一页";
            _next_page.HorizontalContentAlignment = HorizontalAlignment.Center;
            _next_page.BorderThickness = new Thickness(2.0);
            _next_page.BorderBrush = Brushes.White;
            _next_page.FontFamily = new FontFamily("KaiTi_GB2312");
            Canvas.SetLeft(_next_page, FEITStandard.PAGE_BEG_X + 355);
            Canvas.SetTop(_next_page, FEITStandard.PAGE_BEG_Y + 600);
            _next_page.MouseLeftButtonUp += new MouseButtonEventHandler(_next_page_MouseLeftButtonUp);
        }

        void _next_page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            clearAll();

            promptLabel();
            loadStartPage();
        }

        private void startTestLabel()
        {
            _begin_test = new Label();

            _begin_test.Height = 38;
            _begin_test.Width = 120;
            _begin_test.Foreground = Brushes.White;

            PortCanvas.Children.Add(_begin_test);
            _begin_test.FontSize = 24.0;
            _begin_test.FontFamily = new FontFamily("KaiTi_GB2312");
            _begin_test.HorizontalContentAlignment = HorizontalAlignment.Center;
            _begin_test.Content = "开  始";
            _begin_test.BorderThickness = new Thickness(2.0);
            _begin_test.BorderBrush = Brushes.White;
            Canvas.SetLeft(_begin_test, FEITStandard.PAGE_BEG_X + 355);
            Canvas.SetTop(_begin_test, FEITStandard.PAGE_BEG_Y + 600);

            _begin_test.MouseLeftButtonDown += new MouseButtonEventHandler(_begin_test_MouseLeftButtonDown);



        }

        void _begin_test_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            switch (_startButtomStep)
            {
                case 0:
                   studyOne();
                 // testOne();
                    break;
                case 1:
                    testOne();
                    
                    break;
                case 2:
                    studyTwo();
                    break;
                case 3:
                    testTwo();

                    break;

            }



        }

        private void nextQuestionLabel()//横向490
        {
            _nextQuestion = new Label();
            _nextQuestion.Height = 38;
            _nextQuestion.Width = 125;
            //_nextQuestion.Background = Brushes.Black;
            _nextQuestion.Foreground = Brushes.Black;
            _nextQuestion.FontSize = 26.0;
            _nextQuestion.FontFamily = new FontFamily("KaiTi_GB2312");
            _nextQuestion.Content = "下一题";
            _nextQuestion.HorizontalContentAlignment = HorizontalAlignment.Center;
            _nextQuestion.BorderThickness = new Thickness(2.0);
            _nextQuestion.BorderBrush = Brushes.Black;
            PortCanvas.Children.Add(_nextQuestion);
            Canvas.SetLeft(_nextQuestion, FEITStandard.PAGE_BEG_X + 540);
            Canvas.SetTop(_nextQuestion, FEITStandard.PAGE_BEG_Y + 480);



            _nextQuestion.MouseLeftButtonDown += new MouseButtonEventHandler(_nextQuestion_MouseLeftButtonDown);

        }

        void _nextQuestion_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!_remember)
                {
                    _line_num++;
                    if (_line_num < mTestReader._TestList.Count / 2)
                    {
                        if (_line_num == mTestReader._TestList.Count / 2 - 1)
                        {
                            _nextQuestion.Content = "测试(一)完";
                           
                            lastQuestionLabel();

                            _remember = true;
                        }
                        _tip_display.Content = " ";
                        _flash_Display.Stop();
                        _t_Display.Stop();
                       
                        
                        

                        _rtime.Stop();

                        optionAnswerRecord();
                        
                        _NameAnswer = "";
                        _JobAnswer = "";
                        _LikeAnswer = "";
                        _rtime.Reset();
                        mPortrailtMemoryTestControl.ClearLists();

                        mPortrailtMemoryTestControl.LayoutTestPage(mTestReader._TestList[_line_num]);
                       
                        _tip_display.Visibility = System.Windows.Visibility.Hidden;

                        _nextQuestion.Visibility = System.Windows.Visibility.Hidden;
                        
                        mPortrailtMemoryTestControl.HideComponent();
                        

                        testBlankTime();
                       
                        _time_blank.Start();
                    }
                }
                else if (_remember &&_line_num == mTestReader._TestList.Count / 2 - 1)
                {
                    _time_blank.Stop();
                    _tip_display.Content = " ";
                    PortCanvas.Children.Remove(_tip_display);
                    _flash_Display.Stop();
                    _t_Display.Stop();
                    
                    
                    _rtime.Stop();

                    optionAnswerRecord();

                    _NameAnswer = "";
                    _JobAnswer = "";
                    _LikeAnswer = "";
                    _rtime.Reset();
                    mPortrailtMemoryTestControl.ClearLists();
                    _tip_display.Visibility = System.Windows.Visibility.Hidden;

                    _nextQuestion.Visibility = System.Windows.Visibility.Hidden;
                    PortCanvas.Children.Remove(mPortrailtMemoryTestControl);
                    PortCanvas.Background = Brushes.Black;
                    _line_num++;
                    _promptlabel.Visibility = System.Windows.Visibility.Visible;
                   
                    loadSecondPage();
                }
                else if (_remember && _line_num <= mTestReader._TestList.Count - 1 && colTestTwo == 0)
                {
                        _line_num++;
                    
                        if (_line_num == mTestReader._TestList.Count - 1)
                        {

                            _nextQuestion.Content = "测试(二)完";
                            
                            lastQuestionLabel();

                            colTestTwo = -1;

                            
                        }
                        _tip_display.Content = " ";
                        _flash_Display.Stop();
                        _t_Display.Stop();
                       
                        _rtime.Stop();

                        optionAnswerRecord();
                        _NameAnswer = "";
                        _JobAnswer = "";
                        _LikeAnswer = "";
                        _rtime.Reset();
                        mPortrailtMemoryTestControl.ClearLists();
                        mPortrailtMemoryTestControl.LayoutTestPage(mTestReader._TestList[_line_num]);

                        _tip_display.Visibility = System.Windows.Visibility.Hidden;
                        
                        _nextQuestion.Visibility = System.Windows.Visibility.Hidden;

                        mPortrailtMemoryTestControl.HideComponent();


                        testBlankTime();
                        
                        _time_blank.Start();
                    
                }
                else if (_line_num == mTestReader._TestList.Count - 1 && colTestTwo == -1)
                {
                    _time_blank.Stop();
                    _tip_display.Content = " ";
                    _flash_Display.Stop();
                    _t_Display.Stop();
                  
                    _rtime.Stop();

                    optionAnswerRecord();
                    _startButtomStep = 0;
                    clearAll();
                    PortCanvas.Background = Brushes.Black;
                    mTestResult = new TestResult(mTestReader._ResultList, _answers);
                    laodReport();
                }
            }
        }
        private void lastQuestionLabel()
        {
           
            _nextQuestion.Width = 145;
            Canvas.SetLeft(_nextQuestion, FEITStandard.PAGE_BEG_X + 530);
        }

        private void promptLabel()
        {
            _promptlabel = new Label();
            _promptlabel.Height = 58;
            _promptlabel.Width = 340;
            _promptlabel.Background = Brushes.Black;
            _promptlabel.Foreground = Brushes.White;
            _promptlabel.FontSize = 34.0;
            _promptlabel.FontFamily = new FontFamily("KaiTi_GB2312");
            _promptlabel.Content = "";
            _promptlabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            _promptlabel.BorderThickness = new Thickness(2.0);
            _promptlabel.BorderBrush = Brushes.Black;
            _promptlabel.Visibility = System.Windows.Visibility.Hidden;
            PortCanvas.Children.Add(_promptlabel);
            Canvas.SetLeft(_promptlabel, FEITStandard.PAGE_BEG_X + 245);
            Canvas.SetTop(_promptlabel, FEITStandard.PAGE_BEG_Y + 300);
        }

        private void clearAll()
        {
            PortCanvas.Children.Clear();
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////
        /// </summary>

        private void studyOne()
        {
            mMemoryControl = new MemoryControl();
            PortCanvas.Children.Add(mMemoryControl);
            Canvas.SetLeft(mMemoryControl, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mMemoryControl, FEITStandard.PAGE_BEG_Y);
            _begin_test.Visibility = System.Windows.Visibility.Hidden;
            backTextPage();
            mMemoryControl.displayInformation(_remember);
           
            _t_Total_Time.Start();
            PortCanvas.Cursor = Cursors.None;
        }

        private void studyTwo()
        {
            PortCanvas.Children.Remove(mMemoryControl);
            _remember = true;
            _begin_test.Visibility = System.Windows.Visibility.Hidden;
            PortCanvas.Children.Add(mMemoryControl);
            Canvas.SetLeft(mMemoryControl, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mMemoryControl, FEITStandard.PAGE_BEG_Y);
            
            
            
            mMemoryControl.displayInformation(_remember);
            _t_Total_Time.Start();
            PortCanvas.Cursor = Cursors.None;
        }

        private void testOne()
        {
            _begin_test.Visibility = System.Windows.Visibility.Hidden;
            _promptlabel.Visibility = System.Windows.Visibility.Hidden;
            
            mTestReader = new TestReader();

            mPortrailtMemoryTestControl = new PortrailtMemoryTestControl(this);

            _rtime = new Stopwatch();

            mPortrailtMemoryTestControl.LayoutTestPage(mTestReader._TestList[_line_num]);

            

            PortCanvas.Children.Add(mPortrailtMemoryTestControl);
           // PortCanvas.Background = Brushes.White;
            Canvas.SetLeft(mPortrailtMemoryTestControl, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mPortrailtMemoryTestControl, FEITStandard.PAGE_BEG_Y);
           
            
            
            nextQuestionLabel();
            tipDisplay();
            Display_Timer();
            
            _t_Display.Close();
            flash_Time();
            _flash_Display.Start();
            _startButtomStep++;
            
        
            _rtime.Start();
        }

        private void testTwo()
        {
           clearAll();
            
            mPortrailtMemoryTestControl.LayoutTestPage(mTestReader._TestList[_line_num]);

            PortCanvas.Children.Add(mPortrailtMemoryTestControl);
          //  PortCanvas.Background = Brushes.White;
            Canvas.SetLeft(mPortrailtMemoryTestControl, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mPortrailtMemoryTestControl, FEITStandard.PAGE_BEG_Y);
            nextQuestionLabel();
             tipDisplay();


          //  Display_Timer();
             
            _t_Display.Close();
          //  flash_Time();
            _flash_Display.Start();

            _rtime.Start();
        }


        /****************************做题提示********************************/


        private void tipDisplay()//65
        {
            _tip_display = new Label();
            _tip_display.Height = 40;
            _tip_display.Width = 350;
           // _tip_display.Background = Brushes.Black;
            _tip_display.Foreground = Brushes.Red;
            _tip_display.FontSize = 26.0;
            _tip_display.FontFamily = new FontFamily("KaiTi_GB2312");
            _tip_display.Content = " ";
            _tip_display.HorizontalContentAlignment = HorizontalAlignment.Center;
            //_tip_display.BorderThickness = new Thickness(2.0);
            //_tip_display.BorderBrush = Brushes.Black;
            _tip_display.Visibility = System.Windows.Visibility.Hidden;
            PortCanvas.Children.Add(_tip_display);
            Canvas.SetLeft(_tip_display, FEITStandard.PAGE_BEG_X + 25);
            Canvas.SetTop(_tip_display, FEITStandard.PAGE_BEG_Y + 480);
        }
        private void flash_Time()
        {
            _flash_Display = new Timer(10000);
            _flash_Display.AutoReset = false;
            _flash_Display.Enabled = true;
            _flash_Display.Elapsed += new ElapsedEventHandler(_flash_Display_Elapsed);
        }

        void _flash_Display_Elapsed(object sender, ElapsedEventArgs e)
        {
            PortCanvas.Dispatcher.Invoke(new displaytime(fv_processing));
        }
        private void fv_processing() //超时跳出测试
        {
            _flash_Display.Stop();
            
            _t_Display.Start();

        }

        private void Display_Timer()
        {
            _t_Display = new Timer(1000);
            _t_Display.AutoReset = true;
            _t_Display.Enabled = true;
            _t_Display.Elapsed += new ElapsedEventHandler(t_Display_Elapsed);

        }

        void t_Display_Elapsed(object sender, ElapsedEventArgs e)
        {
            PortCanvas.Dispatcher.Invoke(new displaytime(ev_processing));
        }
        private delegate void displaytime();

        private void ev_processing() //超时跳出测试
        {
             _tip_display.Content = "** 请 尽 快 选 择 答 案 **";
            if (_tip_display.Visibility == System.Windows.Visibility.Hidden)
            {
                _tip_display.Visibility = System.Windows.Visibility.Visible;
            }
            else if (_tip_display.Visibility == System.Windows.Visibility.Visible)
            {
                _tip_display.Visibility = System.Windows.Visibility.Hidden;
            }
        }

       
        

        /// <summary>
        /// 回跳
        /// </summary>
        /// 
        private void backTextPage()
        {
            _t_Total_Time = new Timer(51300);
            _t_Total_Time.AutoReset = true;
            _t_Total_Time.Enabled = true;
            _t_Total_Time.Elapsed += new ElapsedEventHandler(_t_Total_Time_Elapsed);

        }

        void _t_Total_Time_Elapsed(object sender, ElapsedEventArgs e)
        {

            PortCanvas.Dispatcher.Invoke(new backTextPageTime(bp_processing));
        }
        private delegate void backTextPageTime();

        private void bp_processing() //超时跳出测试
        {
            PortCanvas.Children.Remove(mMemoryControl);
            _startButtomStep++;
            if (_startButtomStep==1)
            loadFirstTest();
            if (_startButtomStep == 3)
                loadSecondTest();
            _t_Total_Time.Stop();
            PortCanvas.Cursor = Cursors.AppStarting;
            _begin_test.Visibility = System.Windows.Visibility.Visible;
        }


        public void testBlankTime()
        {
            _time_blank = new Timer(500);
            _time_blank.AutoReset = false;
            _time_blank.Enabled = true;
            _time_blank.Elapsed += new ElapsedEventHandler(_time_blank_Elapsed);
        }

        void _time_blank_Elapsed(object sender, ElapsedEventArgs e)
        {

            PortCanvas.Dispatcher.Invoke(new displayblanktime(bl_processing));
        }
        private delegate void displayblanktime();

        private void bl_processing() //超时跳出测试
        {
            
           
            mPortrailtMemoryTestControl.DisplayComponent();
            _tip_display.Visibility = System.Windows.Visibility.Visible;
            _nextQuestion.Visibility = System.Windows.Visibility.Visible;
            //_t_Display.Start();
            _flash_Display.Start();
            _time_blank.Stop();
            _rtime.Start();
        }

      /****************************记录****************************/
        private void rtRecord()
        {

            _runtime = _rtime.ElapsedMilliseconds;//RT

            _accountRT = _accountRT + _runtime;

            _rt = _runtime.ToString(); // 无时间限制
        }

        private void optionAnswerRecord()
        {
            rtRecord();
            _optionAnswers = new List<string>();

            if (_NameAnswer != "") _optionAnswers.Add(_NameAnswer);
            else _optionAnswers.Add("未选");
            
            if (_JobAnswer != "") _optionAnswers.Add(_JobAnswer);
            else _optionAnswers.Add("未选");
            
            if (_LikeAnswer != "") _optionAnswers.Add(_LikeAnswer);
            else _optionAnswers.Add("未选");
            
            try
            {
                _optionAnswers.Add(_rt);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                _rt = "0";
                _optionAnswers.Add(_rt);
            }
            

            _answers.Add(_optionAnswers);

        }




    }//----class
}//namespace

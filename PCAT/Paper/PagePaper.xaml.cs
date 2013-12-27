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
using System.Diagnostics;
using System.Timers;


namespace FiveElementsIntTest.Paper
{
    /// <summary>
    /// Page1.xaml 的互動邏輯
    /// </summary>
    public partial class PagePaper : Page
    {
        public MainWindow mMainWindow;
        protected LayoutInstruction mLayoutInstruction;
        PaperInsUserControl mPaperInsUserControl;
        ExamplePage mExamplePage;

        /********************页面显示**************************************/
        PaperTestControl mPaperTestControl;//试题页面显示控制

        private Label next_page;
        private Label pre_page;
        private Label begin_test;
        private Label nextQuestion;


        private int stepcount_control = 0;//指导语运行步骤

        private Label close_win;//关闭测试

        /*****************************文件操作*************************************/
        public int line_num_count = 0;// 读文件时记录当前运行的第几号题
        public int line_num = 17;//文件总行数 

        private PaperReadFile mPaperReadFile;//读取文件
        private PaperWriteFile mPaperWriteFile;//写测试结果

        /*******************************记录*********************************************/
        String RT = " "; //反应时间
        public List<String> paper_record;
        int _connum = 0; //给写文件类的一个方法做参数 控制下标
        public FEITTimer mTimer;//总共用时
        int rightcount;//答对题数

        //=======时间控制===
        long runtime = 0;
        Stopwatch rtime;//每道题花费时间

        /******************************提示************************************/

        public Label tip_display;

        //public Timer t_Display;

        public String correct_ans;

        //public Timer _flash_Display;

        CompCountDown mCountDown;
        /*************************回退规则*************/
        //用到的方法 recede_ans()  recede_pro()

        public bool recede_control = false;

        List<RecedeVer> reTemp = new List<RecedeVer>();

        //  RecedeVer mRecedeVer;

        List<List<String>> re_Record = new List<List<string>>();

        public int re_count_num = 0;

        private int add_ans_num = 0;

        List<String> compare = new List<string>();

        bool _produrce = true;

        bool re_nextque = false;

        /***********输出答案控制**************/

        public bool wr_thre = false;

        /************************自动翻页 + 时间等级限制 + 破选题控制*******************************/

        //public Timer _t_AutoNextPage;

        private int _level_one = 45000;

        private int _level_two = 75000;

        private int _level_three = 120000;

        private int _level;

        public bool _Control_press = false;

        /*************************************提示各种限制控制**********************************************/

        // public bool _Control_hide = false;

        public PagePaper(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            this.Focus();
            mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);
            mCountDown = new CompCountDown();
            mCountDown.FunctionElapsed = auto_processing;
            tipDisplay();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref mBaseCanvas);
            nextInstructionPage();

        }

        private void loadIniPage() // 首页
        {

            mLayoutInstruction.addTitle(50, 0, "折纸测试", "KaiTi_GB2312", 48, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(113, 95, "Paper Folding", "", 28, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addInstruction(200, 0, FEITStandard.PAGE_WIDTH / 100 * 68, 350,
                "      本测验需要在头脑中进行如下操作：按要求折叠一张正方形的纸，然后用铅笔在叠好的纸上扎一个孔，最后把纸展开。要求你想象展开后的纸上面会有几个孔，以及它们正确的位置。\n\r     （点击 “下一页” 看例题）"
             , "Kaiti_GB2312", 30, Color.FromRgb(255, 255, 255));

            chageNextPage();

        }




        private void loadExercisePage()//练习页
        {
            mExamplePage = new ExamplePage();
            mBaseCanvas.Children.Add(mExamplePage);
            Canvas.SetLeft(mExamplePage, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mExamplePage, FEITStandard.PAGE_BEG_Y);

        }
        private void loadInstructionImagePage()//练习一讲解页
        {
            mPaperInsUserControl = new PaperInsUserControl();
            mBaseCanvas.Children.Add(mPaperInsUserControl);
            Canvas.SetLeft(mPaperInsUserControl, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mPaperInsUserControl, FEITStandard.PAGE_BEG_Y);
        }

        private void loadTestPage()//测试页
        {

            mPaperReadFile.loadTableContent();
            mPaperTestControl._layoutPaper(mPaperReadFile.image_name, mPaperReadFile.item_num, line_num - 1, line_num_count);//8个选项是10个
            rtime.Reset();

            if (line_num_count < line_num - 1)
            {
                correct_ans = mPaperReadFile.line[mPaperReadFile.line.Count - 4];
                tipDisplay();// 提示代码

               // _flash_Display.Start();

                // t_Display.Start();
                nextQuestionLabel();

            }
            else lastQuestionLabel();

            //startCompel();

            rtime.Start();

            setCountDownUITIck();
        }

        private void loadRecedeTest()
        {
            mPaperTestControl._layoutPaper(reTemp[re_count_num].r_text, reTemp[re_count_num].r_item, line_num - 1, re_count_num);
            rtime.Reset();

            correct_ans = re_Record[re_count_num][re_Record[re_count_num].Count - 4];
            tipDisplay();// 提示代码

            //_flash_Display.Start();
            // t_Display.Start();
            nextQuestionLabel();

            //startCompel();

            rtime.Start();

            setCountDownUITIck();
        }

        void nextQuestion_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)// 下一题控制
        {
            if (re_nextque && recede_control && _Control_press)//开始判断回退的2道题 //后添加破选题控制
            {
                if (re_count_num < 2)
                {
                    // _Control_hide = true;
                    rtime.Stop();
                    //t_Display.Stop();
                    tip_display.Visibility = System.Windows.Visibility.Hidden;
                    rtRecord();
                    recede_modifyans();
                    mPaperTestControl.clearBorder();
                    wrongNumCount();
                    re_count_num++;
                    if (add_ans_num >= 1 && re_count_num >= 2)//开关 关闭附加题 开正式测试题
                    {
                        re_nextque = false;
                        recede_control = false;
                        // _Control_hide = false;

                    }

                    if (re_count_num < 2)
                    {
                        nextStep();
                    }
                }
            }
            if (!re_nextque && _Control_press)
            {
                rtime.Stop();
                //t_Display.Stop();
                tip_display.Visibility = System.Windows.Visibility.Hidden;
                if (correct_ans != mPaperTestControl.opt && mPaperTestControl.opt == " " && line_num_count < 6 && line_num_count > 0 && re_count_num < 2) recede_control = true;//



                if (re_count_num != 2)
                {
                    _Answers();
                    wrongNumCount();

                }//
                if (re_count_num == 2) re_count_num++;

                if (compare.Count > 0 && compare[compare.Count - 1] == "F" && line_num_count < 6 && re_count_num == 0)
                {
                    re_nextque = true;
                }
                //if (add_ans_num >= 1 && re_count_num >= 2)//开关 关闭附加题 开正式测试题
                //{
                //    re_nextque = false;
                //    recede_control = false;

                //}

                mPaperTestControl.clearBorder();

                //_t_AutoNextPage.Stop();//强迫机制添加

                nextStep();

            }

            _Control_press = false;
        }


        private void nextStep()//流程
        {

            if (_produrce)
            {
                if (!recede_control)
                {
                    line_num_count++;
                    if (line_num_count < line_num)
                    {

                        loadTestPage();
                    }
                    else
                    {
                        rtime.Stop();
                        mTimer.Stop();
                        //t_Display.Stop();//display 关闭
                        //_t_AutoNextPage.Stop();//强迫机制添加
                        _outputfile();
                        ClearAll();
                        laodReport();
                        rtime.Reset();
                    }
                }
                else
                {
                    loadRecedeTest();
                }
            }

        }


        private void rtRecord()
        {

            runtime = rtime.ElapsedMilliseconds;//RT
            //if (runtime > 0)
            //{


            //    if (runtime < 8000)
            //    {
            //        RT =runtime.ToString();
            //    }
            //    else
            //    {
            //        RT= "8000";
            //    }
            //}
            RT = runtime.ToString(); // 无时间限制
        }

        private void _Answers()
        {

            paper_record = mPaperReadFile.line;
            paper_record.RemoveRange(paper_record.Count - 3, 3);
            rtRecord();

            try
            {
                paper_record.Insert(paper_record.Count, RT);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                RT = "0";
                paper_record.Insert(paper_record.Count, RT);
            }

            paper_record.Insert(paper_record.Count, mPaperTestControl.opt);

            if (paper_record[paper_record.Count - 3] == mPaperTestControl.opt)
            {
                paper_record.Insert(paper_record.Count, "T");
                rightcount++;//答对题数
                compare.Add("T");
            }
            else
            {
                paper_record.Insert(paper_record.Count, "F");
                //rightcount--;
                compare.Add("F");
            }

            mPaperWriteFile.mLines_Record(_connum, paper_record);

            _connum++;



        }

        private void recede_ans()//默认正确答案
        {
            paper_record = mPaperReadFile.line;

            paper_record.RemoveRange(paper_record.Count - 3, 3);

            paper_record.Insert(paper_record.Count, 0 + " ");


            paper_record.Insert(paper_record.Count, paper_record[paper_record.Count - 2]);

            paper_record.Insert(paper_record.Count, "T");

            rightcount++;//答对题数

            mPaperWriteFile.mLines_Record(_connum, paper_record);

            _connum++;
        }

        private void recede_modifyans()//回退后修改默认答案
        {
            List<String> ver = mPaperWriteFile.mLines_paper[re_count_num + 1];
            ver[ver.Count - 3] = RT;
            ver[ver.Count - 2] = mPaperTestControl.opt;


            if (ver[ver.Count - 4] == mPaperTestControl.opt)
            {
                ver[ver.Count - 1] = "T";
                rightcount++;//答对题数
                add_ans_num++;
                compare.Add("T");
            }
            else
            {
                ver[ver.Count - 1] = "F";
                compare.Add("F");
                //rightcount--;

            }
        }


        private delegate void wrongtime();


        void Elapsed(object sender, ElapsedEventArgs e)
        {
            mBaseCanvas.Dispatcher.Invoke(new wrongtime(wrongNumCount));
        }


        private void wrongNumCount() //超时跳出测试
        {
            bool _end = false;

            if (compare[compare.Count - 1] == "T")
            {

                compare.Clear();
            }
            if (compare.Count > 1)
            {
                if (compare[compare.Count - 2] == "F" && compare[compare.Count - 1] == compare[compare.Count - 2])
                {
                    _end = true;
                }


                if (compare.Count == 3 && _end)
                {
                    _produrce = false;
                    wr_thre = true;

                    rtime.Stop();
                    mTimer.Stop();
                    //t_Display.Stop();//display 关闭

                    //_t_AutoNextPage.Stop();//强迫机制添加
                    rtime.Reset();

                    _outputfile();
                    ClearAll();
                    laodReport();
                }

            }


        }
        private void recede_pro()//隐藏2道题
        {
            RecedeVer[] mRecedeVer = new RecedeVer[2];
            for (int i = 0; i < 2; i++)// 
            {
                mRecedeVer[i] = new RecedeVer();

                mPaperReadFile.loadTableContent();

                re_Record.Add(mPaperReadFile.line);

                for (int j = 0; j < mPaperReadFile.image_name.Count; j++)
                {
                    mRecedeVer[i].r_text.Add(mPaperReadFile.image_name[j]);


                }
                mRecedeVer[i].r_item = mPaperReadFile.item_num;

                reTemp.Add(mRecedeVer[i]);

                recede_ans();

                line_num_count++;

            }



        }
        private void laodReport()
        {
            if (rightcount < 0) rightcount = 0;
            if (line_num_count == line_num) line_num_count--;

            //String showtext = "一共" + (line_num - 1) + "道题，已做过" + line_num_count + "道题，" + rightcount + "道题正确；共用时" + mTimer.GetElapsedTime() + "毫秒";

            //mLayoutInstruction.addInstruction(160, 0, 794, 450, showtext, "result", 32, Color.FromRgb(34, 177, 76));
            close_win = new Label();
            close_win.Height = 50;
            close_win.Width = 250;
            close_win.Foreground = Brushes.White;
            close_win.FontSize = 28.0;
            close_win.FontFamily = new FontFamily("KaiTi_GB2312");
            close_win.Content = "本 项 测 验 结 束";
            close_win.MouseLeftButtonDown += new MouseButtonEventHandler(close_win_MouseLeftButtonDown);
            mBaseCanvas.Children.Add(close_win);
            Canvas.SetLeft(close_win, FEITStandard.PAGE_BEG_X + 275);
            Canvas.SetTop(close_win, FEITStandard.PAGE_BEG_Y + 300);

            // mMainWindow.Closing = null;
        }

        void close_win_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mMainWindow.Close();
        }

        private void _outputfile()
        {
            mPaperWriteFile.outPutresult();
        }




        public void ClearAll()
        {
            mBaseCanvas.Children.Clear();//清除属性？
        }







        //---------------------------------------- 以下是指导语修改部分---------------------


        private void chageNextPage()
        {
            next_page = new Label();

            next_page.Height = 38;
            next_page.Width = 100;
            next_page.Foreground = Brushes.Black;
            next_page.Background = Brushes.White;

            mBaseCanvas.Children.Add(next_page);
            next_page.FontSize = 24.0;
            next_page.Content = "下一页";

            next_page.HorizontalContentAlignment = HorizontalAlignment.Center;
            next_page.BorderThickness = new Thickness(2.0);
            next_page.BorderBrush = Brushes.White;
            next_page.FontFamily = new FontFamily("KaiTi_GB2312");
            Canvas.SetLeft(next_page, FEITStandard.PAGE_BEG_X + 650);
            Canvas.SetTop(next_page, FEITStandard.PAGE_BEG_Y + 600);





            next_page.MouseLeftButtonUp += new MouseButtonEventHandler(next_page_MouseLeftButtonUp);

        }

        void next_page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)  //MouseLeftButtonUp="next_page_MouseLeftButtonDown"
        {
            if (stepcount_control < 3)
            {
                stepcount_control++;
                DeleteaddChild();
                nextInstructionPage();

            }

            if (re_count_num >= 2)
            {
                recede_control = false;
            }
        }

        private void chagePrePage()
        {
            pre_page = new Label();

            pre_page.Height = 38;
            pre_page.Width = 100;
            pre_page.Foreground = Brushes.Black;
            pre_page.Background = Brushes.White;

            mBaseCanvas.Children.Add(pre_page);
            pre_page.FontSize = 24.0;
            pre_page.FontFamily = new FontFamily("KaiTi_GB2312");
            pre_page.Content = "上一页";
            pre_page.HorizontalContentAlignment = HorizontalAlignment.Center;
            pre_page.BorderThickness = new Thickness(2.0);
            pre_page.BorderBrush = Brushes.White;
            Canvas.SetLeft(pre_page, FEITStandard.PAGE_BEG_X + 50);
            Canvas.SetTop(pre_page, FEITStandard.PAGE_BEG_Y + 600);




            pre_page.MouseLeftButtonUp += new MouseButtonEventHandler(pre_page_MouseLeftButtonUp);
        }

        void pre_page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


            stepcount_control--;
            DeleteaddChild();
            nextInstructionPage();
        }

        private void startTest()
        {
            begin_test = new Label();

            begin_test.Height = 38;
            begin_test.Width = 120;
            begin_test.Foreground = Brushes.Black;
            begin_test.Background = Brushes.White;

            mBaseCanvas.Children.Add(begin_test);
            begin_test.FontSize = 24.0;
            begin_test.FontFamily = new FontFamily("KaiTi_GB2312");
            begin_test.HorizontalContentAlignment = HorizontalAlignment.Center;
            begin_test.Content = "开始测验";
            begin_test.BorderThickness = new Thickness(2.0);
            begin_test.BorderBrush = Brushes.White;
            Canvas.SetLeft(begin_test, FEITStandard.PAGE_BEG_X + 630);
            Canvas.SetTop(begin_test, FEITStandard.PAGE_BEG_Y + 600);

            begin_test.MouseLeftButtonUp += new MouseButtonEventHandler(begin_test_MouseLeftButtonUp);



        }

        void begin_test_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)//开始测验按钮事件
        {

            ClearAll(); //清除原来的控件
            mPaperReadFile = new PaperReadFile(this);

            mPaperWriteFile = new PaperWriteFile(this, mMainWindow);
            mPaperWriteFile.mLines_Head(mPaperReadFile.Surface);

            recede_pro();//隐藏2道题

            mTimer = new FEITTimer();//整体时间
            rtime = new Stopwatch();//每题时间

            mPaperReadFile.loadTableContent();

            mPaperTestControl = new PaperTestControl(mPaperReadFile.image_name, mPaperReadFile.item_num, line_num - 1, line_num_count, this);


            correct_ans = mPaperReadFile.line[mPaperReadFile.line.Count - 4];
            nextQuestionLabel();
            mBaseCanvas.Children.Add(mPaperTestControl);
            Canvas.SetLeft(mPaperTestControl, FEITStandard.PAGE_BEG_X - 45);
            Canvas.SetTop(mPaperTestControl, FEITStandard.PAGE_BEG_Y - 80);

            //startCompel();

            rtime.Start();
            mTimer.Start();

            tipDisplay();//提示代码 nov.12
            //Display_Timer();
           // t_Display.Stop();
            // flash_Time();
            //_flash_Display.Start();
            // t_Display.Start();

            setCountDownUITIck();
        }

        void setCountDownUITIck()
        {
            mCountDown.Stop();

            if (line_num_count > 0 && line_num_count <= 5)
            {
                mCountDown.Duration = 45;
            }
            else if (line_num_count > 5 && line_num_count <= 13)
            {
                mCountDown.Duration = 75;
            }
            else
            {
                mCountDown.Duration = 120;
            }

            if (!mBaseCanvas.Children.Contains(mCountDown))
            {
                mBaseCanvas.Children.Add(mCountDown);
            }

            Canvas.SetTop(mCountDown, FEITStandard.PAGE_BEG_Y + 550);
            Canvas.SetLeft(mCountDown, (SystemParameters.PrimaryScreenWidth - 300) / 2);

            mCountDown.Start();
        }



        private void nextInstructionPage()
        {
            switch (stepcount_control)
            {
                case 0:
                    ClearAll();
                    loadIniPage();
                    break;
                case 1:
                    loadExerciseOnePage();

                    break;
                case 2:
                    mBaseCanvas.Children.Remove(mExamplePage);
                    loadExerciseOneInsPage();
                    break;
                //case 3:
                //    DeleteaddChild();
                //    nextStep();

                //    break;

            }




        }

        private void loadExerciseOnePage()
        {
            loadExercisePage();
            chageNextPage();
            chagePrePage();
        }
        private void loadExerciseOneInsPage()
        {
            loadInstructionImagePage();
            chagePrePage();
            mBaseCanvas.Children.Remove(next_page);
            startTest();
            //      mTimer.Start();

        }




        private void nextQuestionLabel()
        {
            nextQuestion = new Label();
            nextQuestion.Height = 38;
            nextQuestion.Width = 125;
            nextQuestion.Background = Brushes.White;
            nextQuestion.Foreground = Brushes.Black;
            nextQuestion.FontSize = 26.0;
            nextQuestion.FontFamily = new FontFamily("KaiTi_GB2312");
            nextQuestion.Content = "下一题";
            nextQuestion.HorizontalContentAlignment = HorizontalAlignment.Center;
            nextQuestion.BorderThickness = new Thickness(2.0);
            nextQuestion.BorderBrush = Brushes.White;
            mBaseCanvas.Children.Add(nextQuestion);
            Canvas.SetLeft(nextQuestion, FEITStandard.PAGE_BEG_X + 338);
            Canvas.SetTop(nextQuestion, FEITStandard.PAGE_BEG_Y + 600);



            nextQuestion.MouseLeftButtonUp += new MouseButtonEventHandler(nextQuestion_MouseLeftButtonUp);

        }
        private void lastQuestionLabel()
        {
            nextQuestion.Content = "最后一题";
            nextQuestion.Width = 130;
        }

        private void DeleteaddChild()
        {
            mBaseCanvas.Children.Remove(mExamplePage);
            mBaseCanvas.Children.Remove(mPaperInsUserControl);

            mLayoutInstruction.addInstruction(200, 2, 600, 300, " "
                 , "KaiTi_GB2312", 30, Color.FromRgb(255, 255, 255));
            mBaseCanvas.Children.Remove(begin_test);
            mBaseCanvas.Children.Remove(next_page);
            //mBaseCanvas.Children.Remove(border_adorn);
            //mBaseCanvas.Children.Remove(pre_page); 

        }


        //-----------------------提示--------------------------------
        /*private void flash_Time()
        {
            _flash_Display = new Timer(_level - 15000);
            _flash_Display.AutoReset = false;
            _flash_Display.Enabled = true;
            _flash_Display.Elapsed += new ElapsedEventHandler(_flash_Display_Elapsed);
        }

        void _flash_Display_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBaseCanvas.Dispatcher.Invoke(new displaytime(fv_processing));
        }

        private void fv_processing() 
        {
            _flash_Display.Stop();

            t_Display.Start();

        }



        private void Display_Timer()
        {
            t_Display = new Timer(1000);
            t_Display.AutoReset = true;
            t_Display.Enabled = true;
            t_Display.Elapsed += new ElapsedEventHandler(t_Display_Elapsed);

        }

        void t_Display_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBaseCanvas.Dispatcher.Invoke(new displaytime(ev_processing));
        }
        private delegate void displaytime();

        private void ev_processing() //超时跳出测试
        {
            if (tip_display.Visibility == System.Windows.Visibility.Hidden)
            {
                tip_display.Visibility = System.Windows.Visibility.Visible;
            }
            else if (tip_display.Visibility == System.Windows.Visibility.Visible)
            {
                tip_display.Visibility = System.Windows.Visibility.Hidden;
            }
        }*/

        private void tipDisplay()
        {
            tip_display = new Label();
            tip_display.Height = 46;
            tip_display.Width = 440;
            tip_display.Background = Brushes.Black;
            tip_display.Foreground = Brushes.Red;
            tip_display.FontSize = 32.0;
            tip_display.FontFamily = new FontFamily("KaiTi_GB2312");
            tip_display.Content = "   ";
            tip_display.HorizontalContentAlignment = HorizontalAlignment.Center;
            tip_display.BorderThickness = new Thickness(2.0);
            tip_display.BorderBrush = Brushes.Black;
            tip_display.Visibility = System.Windows.Visibility.Hidden;
            mBaseCanvas.Children.Add(tip_display);
            Canvas.SetLeft(tip_display, FEITStandard.PAGE_BEG_X + 180);
            Canvas.SetTop(tip_display, FEITStandard.PAGE_BEG_Y + 500);
        }


        /*private void CompelNextPage()
        {
            _t_AutoNextPage = new Timer(_level);
            _t_AutoNextPage.AutoReset = false;
            _t_AutoNextPage.Enabled = true;
            _t_AutoNextPage.Elapsed += new ElapsedEventHandler(_t_AutoNextPage_Elapsed);


        }*/

        void _t_AutoNextPage_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBaseCanvas.Dispatcher.Invoke(new autodisplaytime(auto_processing));
        }

        private delegate void autodisplaytime();

        private void auto_processing() //
        {
            if (re_nextque && recede_control)//开始判断回退的2道题
            {
                if (re_count_num < 2)
                {
                    rtime.Stop();
                    //t_Display.Stop();
                    tip_display.Visibility = System.Windows.Visibility.Hidden;
                    rtRecord();
                    recede_modifyans();
                    mPaperTestControl.clearBorder();
                    wrongNumCount();
                    re_count_num++;
                    if (add_ans_num >= 1 && re_count_num >= 2)//开关 关闭附加题 开正式测试题
                    {
                        re_nextque = false;
                        recede_control = false;

                    }
                    if (re_count_num < 2)
                    {

                        nextStep();
                    }
                }
            }
            if (!re_nextque)
            {
                rtime.Stop();
                //t_Display.Stop();
                tip_display.Visibility = System.Windows.Visibility.Hidden;
                if (correct_ans != mPaperTestControl.opt && mPaperTestControl.opt == " " && line_num_count < 6 && line_num_count > 0 && re_count_num < 2) recede_control = true;//



                if (re_count_num != 2)
                {
                    _Answers();
                    wrongNumCount();

                }//
                if (re_count_num == 2) re_count_num++;

                if (compare.Count > 0 && compare[compare.Count - 1] == "F" && line_num_count < 6 && re_count_num == 0)
                {
                    re_nextque = true;
                }
                //if (add_ans_num >= 1 && re_count_num >= 2)//开关 关闭附加题 开正式测试题
                //{
                //    re_nextque = false;
                //    recede_control = false;

                //}

                mPaperTestControl.clearBorder();

                //_t_AutoNextPage.Stop();//强迫机制添加

                nextStep();



            }

            _Control_press = false;
        }

        private void upLevel()
        {
            if (line_num_count < 5)
            {
                _level = _level_one;
            }
            else if (line_num_count >= 5 && line_num_count < line_num - 3)
            {
                _level = _level_two;
            }
            else
            {
                _level = _level_three;
            }

            if (re_count_num == 1)
            {
                _level = _level_one;
            }
        }

        /*private void startCompel()
        {
            upLevel();
            CompelNextPage();
            //_t_AutoNextPage.Start();
            //flash_Time();
        }*/
    }//class
}//namespace

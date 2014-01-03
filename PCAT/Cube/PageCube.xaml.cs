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
using LibTabCharter;
using System.Collections;
using System.Diagnostics;
using System.IO;
namespace FiveElementsIntTest.Cube
{
    /// <summary>
    /// Page1.xaml 的互動邏輯
    /// </summary>
    public partial class PageCube : Page
    {
        public MainWindow mMainWindow;
        protected LayoutInstruction mLayoutInstruction;//
        TabFetcher tf = new TabFetcher("Cube\\Cube_Test_Paper\\CubeR.txt", "\\s");// Cube练习两题.txt   CubeA.txt
        List<String> line;
        public List<String> Surface = new List<string>();
        List<String> Question_test = new List<string>();//中间变量 记录每一道题
        List<String> RT = new List<string>();
        List<String> Question = new List<String>();
        List<String> temporary_list;//开辟空间 写文件
        List<String> recode = new List<string>(); //用于旋转轴
        public List<String> Anstandard = new List<string>();//标准答案
        public List<String> Antest = new List<string>();//测试答案

        List<List<String>> mLines;//存放一套测试结果
        List<String> resline; //为写文件服务
        ExerciseOnePage mExerciseOnePage;
        ExerciseOnePage_Ins mExerciseOnePage_Ins;
        ExerciseTwoPage mExerciseTwoPage;
        ExerciseTwoPage_Ins mExerciseTwoPage_Ins;

        OrganizerTrailCubes orga;
        //  CubeDemonstration mCubeDemonstration;
        //     int reset_time = 1000000000;//重置时间
        //   int nextstepcount = 0;//翻页控制器
        long runtime = 0;
        public int ansstacount = 0;
        public int rightcount;//答对题数
        public FEITTimer mTimer;//总共用时
        public Timer otime;//翻页计时控制器
        public Timer alltime;
        Stopwatch rtime;//每道题花费时间
        public int line_num = 17;//文件行数 
        public int line_num_count = 0;
        TabCharter mTabCharter;//写入硬盘
        String file_Loca; //系统时间文件地址
        string titletext = "题号";
        int anscount = 0;//输出对比答案计数器
        //  private Label ex_ans_Lab;
        private Label close_win;
        private Label next_page;
        private Label pre_page;
        private Label begin_test;
        private Label nextQuestion;
        private Boolean con_result_putout = false;


        //------------回退---------------------------------//
        private int wrong_count = 0;//记录错题个数
        public int hide_count = 0;//回退题的个数
        private String hideans = null;
        private String hiderigthans = null;
        private String hideRT = null;
        private String hidecompare = null;
        public bool _isDisplayhide = false;

        private bool wrong_ans_putout = false;

        private int stepcount_control = 0;

        /******************************提示************************************/

        public Label tip_display;

        //public Timer t_Display;

        //public Timer _flash_Display;

        public CompCountDown mCountDownUI;//count down component in user interface
        /******************************迫选*********************************/
        public bool _Control_choose = true;

        public PageCube(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            this.Focus();
            mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);

            mLines = new List<List<string>>();//存放一套测试结果
            //   mLines.Capacity = line_num;
            resline = new List<string>();

            mTimer = new FEITTimer();
            rtime = new Stopwatch();
            tf.Open();

            //set record location and file name
            file_Loca = "Report\\CubeTest_result_file\\" + mMainWindow.mDemography.GenBriefString() + ".txt";
            mTabCharter = new TabCharter(file_Loca);//写入硬盘

            mCountDownUI = new CompCountDown();
            mCountDownUI.FunctionElapsed = goNextQuestion;
        }

        //~PageCube()
        //{
        //    if (line_num_count >= 0 && line_num_count < line_num - 1) 
        //        File.Delete(file_Loca) ;
        //}


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref mBaseCanvas);
            laodTextFile();
            line_num_count = line_num_count - 1;
            outPutfile();
            nextInstructionPage();


        }


        private void loadInitPage()
        {
            //  mLayoutInstruction = new LayoutInstruction(ref mBaseCanvas);

            mLayoutInstruction.addTitle(50, 0, "魔方旋转", "KaiTi_GB2312", 48, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addTitle(113, 92, "Cube Rotation", "", 28, Color.FromRgb(255, 255, 255));
            mLayoutInstruction.addInstruction(200, 2, 600, 500, "    本测验需要你在头脑中进行如下操作: " + "    按要求对四格魔方进行旋转，要求你想象旋转后魔方上顶面的情况。\n\r        （点击 “下一页” 看例题）"
                , "KaiTi_GB2312", 30, Color.FromRgb(255, 255, 255));


            chageNextPage();
        }

        private bool mFirstTime = true;

        private void nextStep()//页面显示流程
        {

            if (line_num_count < line_num)
            {
                if (mFirstTime)
                {
                    ClearAll();
                    orga = new OrganizerTrailCubes(this);
                    mFirstTime = false;
                }

                if (wrong_count >= 3)
                {
                    wrongTestOver();
                }
                if (wrong_count < 3 && hide_count >= 2)
                {
                    _isDisplayhide = false;
                }
                if (_isDisplayhide && wrong_count < 3)
                {
                    nextQuestionLabel();
                    mBaseCanvas.Children.Remove(tip_display);
                    tipDisplay();
                    orga.optEnable = true;
                    orga.clearSeSelGraphs();
                    Question_test.Clear();
                    hide_count++;
                    loadTest();

                    orga.CubeLayout();
                    //_flash_Display.Start();
                    //  t_Display.Start();
                }
                else if (!_isDisplayhide && wrong_count < 3)
                {
                    nextQuestionLabel();
                    if (line_num_count == line_num - 2)
                    {
                        nextQuestion.Content = "测试结束";
                        nextQuestion.Width = 130;
                    }
                    //keyTimeTrigger();//总体计时器
                    mBaseCanvas.Children.Remove(tip_display);
                    tipDisplay();
                    orga.optEnable = true;


                    orga.clearSeSelGraphs();
                    Question_test.Clear();
                    line_num_count++;
                    loadTest();

                    orga.CubeLayout();
                    //_flash_Display.Start();
                    //  t_Display.Start();
                }
            }
            else
            {
                mTimer.Stop();
                //t_Display.Close();
                outPutresult();
                tf.Close();
                ClearAll();
                ClearArrayList();
                laodReport();

            }

        }



        private void laodReport()
        {

            //if (rightcount == -1) rightcount = 0;

            //String showtext = "一共" + (line_num_count-1) + "道题，答对" + rightcount + "道题,共用时" + mTimer.GetElapsedTime() + "毫秒"; 

            //mLayoutInstruction.addInstruction(160, 90, 794, 450, showtext, "result", 32, Color.FromRgb(34, 177, 76));
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
            mMainWindow.TestForward();
            // mMainWindow.Closing = null;
        }

        void close_win_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mMainWindow.Close();
        }

        private void laodTextFile()//读取表头
        {
            //  line = tf.GetLineBy(); 


            line = tf.GetLineAt(line_num_count);

            if (line_num_count == 0)
            {

                for (int i = 0; i < line.Count; i++)
                {

                    if (line[i] != " ")
                    {
                        Surface.Add(line[i].ToString());
                    }
                }
                line_num_count++;

            }

        }

        private void loadTest()
        {//读取每一行

            if (_isDisplayhide)
            {
                line = tf.GetLineAt(hide_count);

                for (int i = 1; i < (line.Count - 4); i++)
                {
                    Question_test.Add(line[i].ToString());

                }
                hiderigthans = line[line.Count - 4];
                TextPage(Question_test);//显示题
                orga.Qnum.Foreground = Brushes.White;
                orga.Qnum.Content = "(+" + (hide_count) + ")";
                nextQuestion.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(nextQuestion_MouseLeftButtonUp);


            }
            else if (!_isDisplayhide)
            {
                for (; line_num_count < line_num; )
                {

                    line = tf.GetLineAt(line_num_count);

                    if (line_num_count < line_num)
                    {

                        for (int i = 1; i < (line.Count - 4); i++)
                        {
                            Question_test.Add(line[i].ToString());

                        }
                        //   if (line[line.Count - 4] == "#") Anstandard.Add("");
                        //     else 
                        Anstandard.Add(line[line.Count - 4]);//纪录标准答案

                        TextPage(Question_test);//显示题
                        orga.Qnum.Foreground = Brushes.White;
                        orga.Qnum.Content = "(" + (line_num_count - 2) + ")";//"共" + (line_num -1)+ "道题：" + "第" + (line_num_count) + "题";


                        //  nextQuestion.Content = "下一题";
                        nextQuestion.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(nextQuestion_MouseLeftButtonUp);

                        break;
                    }


                    // if (i >= (52 * 3) && i <= (50 + 52 * 3)){ Question2.Add(line[i].ToString());}



                }
            }

            setCountDownUITick();
        }

        void setCountDownUITick()
        {
            mCountDownUI.Stop();

            if (line_num_count > 0 && line_num_count <= 5)
            {
                mCountDownUI.Duration = 45;
            }
            else if (line_num_count > 5 && line_num_count <= 13)
            {
                mCountDownUI.Duration = 75;
            }
            else
            {
                mCountDownUI.Duration = 120;
            }

            if (!mBaseCanvas.Children.Contains(mCountDownUI))
            {
                mBaseCanvas.Children.Add(mCountDownUI);
            }

            Canvas.SetTop(mCountDownUI, FEITStandard.PAGE_BEG_Y + 500);
            Canvas.SetLeft(mCountDownUI, (SystemParameters.PrimaryScreenWidth - 300) / 2);

            mCountDownUI.Start();
        }

        void goNextQuestion()
        {
            _Control_choose = true;
            rtRecord();
            //t_Display.Stop();

            if (line_num_count == 0)
            {
                recode.Clear();

                nextStep();

            }
            else if (line_num_count < line_num)
            {
                if (_isDisplayhide && hide_count <= 2)
                {
                    hideans = orga.Ans;

                    if (line[line.Count - 4] == orga.Ans)
                    {
                        rightcount++;
                        if (wrong_count > 0) wrong_count--;
                    }
                    else
                    {
                        wrong_count++;
                        //   rightcount--;

                    }


                    orga.Ans = "";
                    recode.Clear();
                    putoutDisplayHided();

                    nextStep();
                }
                else if (!_isDisplayhide)
                {
                    Antest.Add(orga.Ans);

                    if (Anstandard[ansstacount] == orga.Ans)
                    {
                        rightcount++;
                        ansstacount++;
                        if (wrong_count > 0) wrong_count--;
                    }
                    else
                    {
                        wrong_count++;
                        //rightcount--;
                        ansstacount++;
                        if (line_num_count >= 3 && line_num_count <= 5 && hide_count < 2)
                        {
                            if (wrong_count > 0)
                            {
                                _isDisplayhide = true;
                            }
                        }

                    }

                    orga.Ans = "";
                    recode.Clear();
                    outPutfile();

                    if (line_num_count == line_num - 1) line_num_count++;
                    nextStep();
                }
            }
        }

        private void nextQuestion_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           if(!_Control_choose)
            goNextQuestion();
        }

        private void outPutfile()//写硬盘
        {
            if (line_num_count == 0)
            {

                mLines.Add(Surface);
                //   mTabCharter.Create(Surface);
            }
            else if (line_num_count < line_num)
            {
                Question_test.Insert(0, line_num_count.ToString());
                for (; anscount < line_num; )
                {
                    Question_test.Add(Anstandard[anscount]);
                    try
                    {
                        Question_test.Add(RT[anscount] + "ms");
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        RT.Add("0");
                        Question_test.Add(RT[anscount] + "ms");
                    }
                    Question_test.Add(Antest[anscount]);
                    if (Anstandard[anscount] == Antest[anscount]) Question_test.Add("T");
                    else Question_test.Add("F");

                    mLines_Record(anscount, Question_test);
                    anscount++;

                    break;
                }
                //   mTabCharter.Append(Question_test);

            }



        }
        private void mLines_Record(int num, List<String> list)
        {


            temporary_list = new List<String>();//开辟空间

            mLines.Add(temporary_list);

            for (int i = 0; i < list.Count; i++)
            {
                mLines[num + 1].Add(list[i]);

            }
        }

        //private void keyTimeTrigger()//时控翻页时间
        //{
        //    otime = new Timer(reset_time);
        //    otime.AutoReset = true;
        //    otime.Enabled = true;
        //    otime.Elapsed += new ElapsedEventHandler(Elapsed);

        //}

        //private delegate void overtime();

        //void Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    mBaseCanvas.Dispatcher.Invoke(new overtime(unaccomplishReport));
        //}
        //private void unaccomplishReport() //超时跳出测试
        //{
        //    if (line_num_count < line_num)
        //    {
        //        rtRecord();

        //        if (orga.choose_buttom) Antest.Add("0");
        //        outPutfile();
        //        nextStep();   

        //    }else{

        //        mTimer.Stop();

        //        if (con_result_putout == false) outPutresult();
        //        tf.Close();
        //        ClearAll();
        //        ClearArrayList();
        //        laodReport();   

        //    }

        //}



        private void TextPage(List<String> timetest)
        {
            Surface.Remove(titletext);
            orga.ClearAllCubzTex();
            String instand1 = " ";
            int ins1 = 0;
            int ins2 = 0;

            for (int j = 0; j < timetest.Count; j++)
            {

                if (j < 12)
                {

                    if (timetest[j].ToString() != "#" && timetest[j].ToString() == "1")
                    {
                        instand1 = Surface[j].ToString();
                        ins1 = 1;
                    }
                    else if (timetest[j].ToString() != "#" && timetest[j].ToString() == "2")
                    {
                        instand1 = Surface[j].ToString();
                        ins1 = 2;
                    }
                    else if (timetest[j].ToString() != "#" && timetest[j].ToString() == "3")
                    {
                        instand1 = Surface[j].ToString();
                        ins1 = 3;
                    }
                    else if (timetest[j].ToString() != "#" && timetest[j].ToString() == "4")
                    {
                        instand1 = Surface[j].ToString();
                        ins1 = 4;
                    }
                    else if (timetest[j].ToString() != "#" && timetest[j].ToString() == "5")
                    {
                        instand1 = Surface[j].ToString();
                        ins1 = 5;
                    }
                    switch (ins1)
                    {
                        case 1:
                            orga.SetFirstGraphTexture(instand1, FiveElementsIntTest.Properties.Resources.ArrowTexUp);

                            break;
                        case 2:
                            orga.SetFirstGraphTexture(instand1, FiveElementsIntTest.Properties.Resources.ArrowTexDown);
                            break;
                        case 3:
                            orga.SetFirstGraphTexture(instand1, FiveElementsIntTest.Properties.Resources.ArrowTexLeft);
                            break;
                        case 4:
                            orga.SetFirstGraphTexture(instand1, FiveElementsIntTest.Properties.Resources.ArrowTexRight);
                            break;
                        case 5:
                            orga.SetFirstGraphTexture(instand1, FiveElementsIntTest.Properties.Resources.CIRCLE);
                            break;

                    }
                }
                if (j >= 12 && j < 15)
                {
                    if (timetest[j].ToString() != "#")
                    {

                        if (Surface[j].ToString() == "RotDir1")
                        {
                            RotDir("RotDir1", timetest[j].ToString(), orga);
                        }

                        if (Surface[j].ToString() == "RotDir2")
                        {
                            RotDir("RotDir2", timetest[j].ToString(), orga);
                        }
                        if (Surface[j].ToString() == "RotDir3")
                        {
                            RotDir("RotDir3", timetest[j].ToString(), orga);
                        }

                    }
                }



                if (j >= 15 && j < 47)
                {
                    if (Surface[j].ToString() == "AS0" && timetest[j].ToString() != "#")
                    {
                        ins2 = 0;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);

                    }
                    else if (Surface[j].ToString() == "BS0" && timetest[j].ToString() != "#")
                    {
                        ins2 = 0;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS0" && timetest[j].ToString() != "#")
                    {
                        ins2 = 0;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS0" && timetest[j].ToString() != "#")
                    {
                        ins2 = 0;
                        instand1 = "D";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "AS1" && timetest[j].ToString() != "#")
                    {
                        ins2 = 1;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "BS1" && timetest[j].ToString() != "#")
                    {
                        ins2 = 1;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS1" && timetest[j].ToString() != "#")
                    {
                        ins2 = 1;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS1" && timetest[j].ToString() != "#")
                    {
                        ins2 = 1;
                        instand1 = "D";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "AS2" && timetest[j].ToString() != "#")
                    {
                        ins2 = 2;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "BS2" && timetest[j].ToString() != "#")
                    {
                        ins2 = 2;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS2" && timetest[j].ToString() != "#")
                    {
                        ins2 = 2;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS2" && timetest[j].ToString() != "#")
                    {
                        ins2 = 2;
                        instand1 = "D";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "AS3" && timetest[j].ToString() != "#")
                    {
                        ins2 = 3;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "BS3" && timetest[j].ToString() != "#")
                    {
                        ins2 = 3;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS3" && timetest[j].ToString() != "#")
                    {
                        ins2 = 3;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS3" && timetest[j].ToString() != "#")
                    {
                        ins2 = 3;
                        instand1 = "D";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "AS4" && timetest[j].ToString() != "#")
                    {
                        ins2 = 4;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "BS4" && timetest[j].ToString() != "#")
                    {
                        ins2 = 4;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS4" && timetest[j].ToString() != "#")
                    {
                        ins2 = 4;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS4" && timetest[j].ToString() != "#")
                    {
                        ins2 = 4;
                        instand1 = "D";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "AS5" && timetest[j].ToString() != "#")
                    {
                        ins2 = 5;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "BS5" && timetest[j].ToString() != "#")
                    {
                        ins2 = 5;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS5" && timetest[j].ToString() != "#")
                    {
                        ins2 = 5;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS5" && timetest[j].ToString() != "#")
                    {
                        ins2 = 5;
                        instand1 = "D";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "AS6" && timetest[j].ToString() != "#")
                    {
                        ins2 = 6;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "BS6" && timetest[j].ToString() != "#")
                    {
                        ins2 = 6;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS6" && timetest[j].ToString() != "#")
                    {
                        ins2 = 6;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS6" && timetest[j].ToString() != "#")
                    {
                        ins2 = 6;
                        instand1 = "D";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "AS7" && timetest[j].ToString() != "#")
                    {
                        ins2 = 7;
                        instand1 = "A";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "BS7" && timetest[j].ToString() != "#")
                    {
                        ins2 = 7;
                        instand1 = "B";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "CS7" && timetest[j].ToString() != "#")
                    {
                        ins2 = 7;
                        instand1 = "C";
                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }
                    else if (Surface[j].ToString() == "DS7" && timetest[j].ToString() != "#")
                    {
                        ins2 = 7;
                        instand1 = "D";

                        optionTest(ins2, instand1, timetest[j].ToString(), orga);
                    }






                }//---------16-47 if


                //   Console.ReadLine();
            }//--------for
            recode.Clear();
            rtime.Reset();
            rtime.Start();

        }
        private void RotDir(String RotDirnum, String s, OrganizerTrailCubes orga)
        {

            switch (s)// 原版 R与bottom back  true变false false 变true
            {
                case "L":
                    RotDisplay(RotDirnum, "left", true);

                    break;
                case "Li":
                    RotDisplay(RotDirnum, "left", false);

                    break;
                case "R":
                    RotDisplay(RotDirnum, "right", false);

                    break;
                case "Ri":
                    RotDisplay(RotDirnum, "right", true);

                    break;
                case "U":
                    RotDisplay(RotDirnum, "top", true);
                    break;
                case "Ui":
                    RotDisplay(RotDirnum, "top", false);
                    break;
                case "D":
                    RotDisplay(RotDirnum, "bottom", false);

                    break;
                case "Di":
                    RotDisplay(RotDirnum, "bottom", true);

                    break;
                case "F":
                    RotDisplay(RotDirnum, "front", true);
                    break;
                case "Fi":
                    RotDisplay(RotDirnum, "front", false);

                    break;
                case "B":
                    RotDisplay(RotDirnum, "back", false);
                    break;
                case "Bi":
                    RotDisplay(RotDirnum, "back", true);
                    break;
            }


        }
        private void RotDisplay(String rot, string fx, Boolean bb)
        {
            recode.Add(rot);
            if (recode.Count == 1)
            {
                orga.cube_Layout(48 + 180 + 90);
                if (recode[0] == "RotDir1")
                {
                    orga.ClearRotGraph1();
                    orga.ClearRotGraph2();
                    orga.SetRotatingGraph(fx, bb);


                }
                else if (recode[0] == "RotDir2")
                {
                    orga.ClearRotGraph();
                    orga.ClearRotGraph2();
                    orga.SetRotatingGraph1(fx, bb);
                }

                else if (recode[0] == "RotDir3")
                {
                    orga.ClearRotGraph1();
                    orga.ClearRotGraph();
                    orga.SetRotatingGraph2(fx, bb);
                }



            }
            else if (recode.Count == 2)
            {

                if (recode[0] == "RotDir1" && recode[1] == "RotDir2")
                {
                    orga.SetRotatingGraph1(fx, bb);
                    orga.ClearRotGraph2();
                    orga.cube_Layout(48 + 180);
                }

            }
            else if (recode.Count == 3)
            {

                orga.SetRotatingGraph2(fx, bb);
                orga.cube_Layout(48 + 90);

            }
        }




        private void optionTest(int ins, String instand, String s2, OrganizerTrailCubes orga)
        {

            switch (s2)
            {
                case "1":
                    orga.SetSelectionGraph(ins, instand, FiveElementsIntTest.Properties.Resources.ArrowTexUp);

                    break;
                case "2":
                    orga.SetSelectionGraph(ins, instand, FiveElementsIntTest.Properties.Resources.ArrowTexDown);
                    break;
                case "3":
                    orga.SetSelectionGraph(ins, instand, FiveElementsIntTest.Properties.Resources.ArrowTexLeft);
                    break;
                case "4":
                    orga.SetSelectionGraph(ins, instand, FiveElementsIntTest.Properties.Resources.ArrowTexRight);
                    break;
                case "5":
                    orga.SetSelectionGraph(ins, instand, FiveElementsIntTest.Properties.Resources.CIRCLE);
                    break;



            }


        }

        public void ClearAll()
        {

            mBaseCanvas.Children.Clear();//清除属性？
            //PageCommon.AddAuxBDR(ref mBaseCanvas, ref mAuxBorder, ref mAuxBorder1024, ref mMainWindow);
            //这个类的作用 

        }
        private void rtRecord()
        {

            runtime = rtime.ElapsedMilliseconds;//RT
            //if (runtime > 0)
            //{
            //    rtime.Stop();

            //    if (runtime < 8000)
            //    {
            //        RT.Add(runtime.ToString());
            //    }
            //    else
            //    {
            //        RT.Add("8000");
            //    }
            //}
            if (line_num_count > 0 && line_num_count < 3 && !_isDisplayhide)
            {
                RT.Add("0");
            }
            else if (line_num_count > 2 && line_num_count < 6 && _isDisplayhide)
            {
                hideRT = runtime.ToString();
            }
            else
            {
                RT.Add(runtime.ToString());
            }
        }
        private void ClearArrayList()
        {
            line.Clear();
            Surface.Clear();
            Question_test.Clear();
            RT.Clear();
            Question.Clear();
            recode.Clear(); //用于旋转轴
            Anstandard.Clear();//标准答案
            Antest.Clear();//测试答案
            mLines.Clear();
            resline.Clear();
        }

        //private void Page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{//MouseLeftButtonUp="Page_MouseLeftButtonUp"
        //    rtRecord();

        //    if (e.LeftButton == System.Windows.Input.MouseButtonState.Released)//e.Key == Key.Space)//
        //    {
        //        if (line_num_count == 0)
        //        {
        //            recode.Clear();

        //            nextStep();

        //        }
        //        else if (line_num_count <= line_num)
        //        {
        //            if (orga.choose_buttom) Antest.Add("0");
        //            recode.Clear();
        //            outPutfile();
        //            nextStep();
        //        }
        //    }
        //}

        private void outPutresult()
        {
            con_result_putout = true;
            Surface.Insert(0, titletext);
            if (line_num_count >= line_num - 1 || wrong_ans_putout)
            {
                for (int i = 0; i < mLines.Count; i++)
                {
                    resline.Clear();

                    for (int j = 0; j < mLines[i].Count; j++)//
                    {
                        resline.Add(mLines[i][j]);
                    }
                    if (i == 0) mTabCharter.Create(resline);
                    else
                        mTabCharter.Append(resline);

                }
            }
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
            if (stepcount_control < 5)
            {
                stepcount_control++;
                addChild();
                nextInstructionPage();


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
            addChild();
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
            begin_test.BorderThickness = new Thickness(2.0);
            begin_test.HorizontalContentAlignment = HorizontalAlignment.Center;
            begin_test.BorderBrush = Brushes.White;
            begin_test.Content = "开始测验";


            Canvas.SetLeft(begin_test, FEITStandard.PAGE_BEG_X + 630);
            Canvas.SetTop(begin_test, FEITStandard.PAGE_BEG_Y + 600);

            begin_test.MouseLeftButtonUp += new MouseButtonEventHandler(begin_test_MouseLeftButtonUp);



        }

        void begin_test_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            // 提示代码
            for (int k = 0; k < 2; k++)
            {
                Question_test.Clear();
                line_num_count++;
                recordHideTest();


            }
            //Display_Timer();
            //t_Display.Close();
            //flash_Time();

            nextStep();
        }

        private void nextInstructionPage()
        {
            switch (stepcount_control)
            {
                case 0:
                    ClearAll();
                    loadInitPage();
                    break;
                case 1:
                    loadExerciseOnePage();

                    break;
                case 2:
                    loadExerciseOneInsPage();
                    break;
                case 3:
                    loadExerciseTwoPage();

                    break;
                case 4:
                    loadExerciseTwoInsPage();

                    break;
            }




        }

        private void loadExerciseOnePage()
        {
            mExerciseOnePage = new ExerciseOnePage();
            mBaseCanvas.Children.Add(mExerciseOnePage);
            Canvas.SetLeft(mExerciseOnePage, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mExerciseOnePage, FEITStandard.PAGE_BEG_Y);
            chageNextPage();
            chagePrePage();
        }
        private void loadExerciseOneInsPage()
        {
            mExerciseOnePage_Ins = new ExerciseOnePage_Ins();
            mBaseCanvas.Children.Add(mExerciseOnePage_Ins);
            Canvas.SetLeft(mExerciseOnePage_Ins, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mExerciseOnePage_Ins, FEITStandard.PAGE_BEG_Y);
            chageNextPage();

        }

        private void loadExerciseTwoPage()
        {
            mExerciseTwoPage = new ExerciseTwoPage();
            mBaseCanvas.Children.Add(mExerciseTwoPage);
            Canvas.SetLeft(mExerciseTwoPage, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mExerciseTwoPage, FEITStandard.PAGE_BEG_Y);
            chageNextPage();
        }



        private void loadExerciseTwoInsPage()
        {
            mExerciseTwoPage_Ins = new ExerciseTwoPage_Ins();
            mBaseCanvas.Children.Add(mExerciseTwoPage_Ins);
            Canvas.SetLeft(mExerciseTwoPage_Ins, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(mExerciseTwoPage_Ins, FEITStandard.PAGE_BEG_Y);


            mBaseCanvas.Children.Remove(next_page);
            startTest();
            mTimer.Start();
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
            Canvas.SetLeft(nextQuestion, FEITStandard.PAGE_BEG_X + 365);
            Canvas.SetTop(nextQuestion, FEITStandard.PAGE_BEG_Y + 600);



        }

        private void addChild()
        {
            mBaseCanvas.Children.Remove(mExerciseOnePage);
            mBaseCanvas.Children.Remove(mExerciseOnePage_Ins);
            mBaseCanvas.Children.Remove(mExerciseTwoPage);
            mBaseCanvas.Children.Remove(mExerciseTwoPage_Ins);
            mLayoutInstruction.addInstruction(200, 2, 600, 300, " "
                 , "KaiTi_GB2312", 30, Color.FromRgb(255, 255, 255));
            mBaseCanvas.Children.Remove(begin_test);
            mBaseCanvas.Children.Remove(next_page);
        }

        ////////////////////////////回退规则修改////////////////////////////////////





        //-----------------------提示--------------------------------

        /*private void flash_Time()
        {
            _flash_Display = new Timer(10000);
            _flash_Display.AutoReset = false;
            _flash_Display.Enabled = true;
            _flash_Display.Elapsed += new ElapsedEventHandler(_flash_Display_Elapsed);
        }

        void _flash_Display_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBaseCanvas.Dispatcher.Invoke(new displaytime(fv_processing));
        }
        private void fv_processing() //超时跳出测试
        {
            _flash_Display.Stop();

            t_Display.Start();

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
            tip_display.Content = "  请 尽 快 选 择 答 案  ";
            tip_display.HorizontalContentAlignment = HorizontalAlignment.Center;
            tip_display.BorderThickness = new Thickness(2.0);
            tip_display.BorderBrush = Brushes.Black;
            tip_display.Visibility = System.Windows.Visibility.Hidden;
            mBaseCanvas.Children.Add(tip_display);
            Canvas.SetLeft(tip_display, FEITStandard.PAGE_BEG_X + 205);
            Canvas.SetTop(tip_display, FEITStandard.PAGE_BEG_Y + 460);
        }

        /*private void Display_Timer()
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
        }

        private void tipDisplayVis()
        {
            tip_display.Content = "  请 尽 快 选 择 答 案  ";
            if (tip_display.Visibility == System.Windows.Visibility.Visible)
            {
                tip_display.Visibility = System.Windows.Visibility.Hidden;
            }
        }*/

        ////////////////////////////////回退规则/////////////////
        private void recordHideTest()
        {
            line = tf.GetLineAt(line_num_count);
            for (int i = 1; i < (line.Count - 4); i++)
            {
                Question_test.Add(line[i].ToString());

            }
            Anstandard.Add(line[line.Count - 4]);
            Antest.Add(line[line.Count - 4]);
            rtRecord();
            rightcount++;
            //标准答案list长度 -1
            recode.Clear();
            putoutRightHideTest();


        }

        private void putoutRightHideTest()
        {
            Question_test.Insert(0, line_num_count.ToString());

            Question_test.Insert(line.Count - 4, Anstandard[anscount]);

            Question_test.Insert(line.Count - 3, "0 ");

            Question_test.Insert(line.Count - 2, Antest[anscount]);

            Question_test.Insert(line.Count - 1, "T");

            mLines_Record(anscount, Question_test);

            anscount++;
            ansstacount++;
        }

        private void putoutDisplayHided()
        {

            int temcount = mLines[hide_count].Count;

            try
            {
                mLines[hide_count][temcount - 3] = hideRT + "";
            }
            catch (System.ArgumentOutOfRangeException)
            {

                mLines[hide_count][temcount - 3] = "0  ";
            }
            mLines[hide_count][temcount - 2] = hideans;
            if (hiderigthans == hideans) hidecompare = "T";
            else hidecompare = "F";
            mLines[hide_count][temcount - 1] = hidecompare;



        }

        private void wrongTestOver()
        {
            wrong_ans_putout = true;
            mTimer.Stop();
            //t_Display.Close();
            outPutresult();
            tf.Close();
            ClearAll();
            ClearArrayList();
            laodReport();

        }







    }//---------类
}//----------空间名

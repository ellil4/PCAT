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
using System.IO;
using PCATData;

namespace FiveElementsIntTest.SeniorWords
{
    /// <summary>
    /// PageSeniorWords.xaml 的互動邏輯
    /// </summary>
    public partial class PageSeniorWords : Page
    {
        public MainWindow mMainWindow;

        public List<CompSeniorWords> mSelections;
        public TextBox mMainLabel;
        OrganizerSeniorWords mOrganizer;
        public Label mNextBtn;

        public List<StSWItem> mItems;
        public List<StSWResult> mResults;

        private int mCurTillIndex = 0;
        private FEITTimer mTimer;

        public int SHEET_LENGTH = 40;

        public static String BASE_FOLDER = 
            System.AppDomain.CurrentDomain.BaseDirectory;

        public static String OUT_FOLDER = "output\\";
        public static String INPUT_FILE = "FEITSWsource.txt";

        private int mContinuousZero = 0;

        public PageSeniorWords(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;

            checkOutputFolder();

            mSelections = new List<CompSeniorWords>();

            mResults = new List<StSWResult>();
            mTimer = new FEITTimer();

            if (!mMainWindow.mDB.TableExists(Names.VOCAB_TABLENAME))
            {
                mMainWindow.mDB.CreateVocabTable(SHEET_LENGTH);
            }
        }

        public int SelectedIdx()
        {
            int retval = -1;
            for (int i = 0; i < 5; i++)
            {
                if (mSelections[i].isSelected)
                {
                    retval = i;
                    break;
                }
                
            }
            return retval;
        }

        private void checkOutputFolder()
        {
            if(!Directory.Exists(BASE_FOLDER + OUT_FOLDER))
            {
                Directory.CreateDirectory(BASE_FOLDER + OUT_FOLDER);
            }
        }

        private void buildElements()
        {
            mMainLabel = new TextBox();
            mMainLabel.Width = 300;
            mMainLabel.Height = 200;
            mMainLabel.Text = "Psych";
            mMainLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            mMainLabel.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            mMainLabel.BorderThickness = new Thickness(0);
            mMainLabel.Focusable = false;
            mMainLabel.TextAlignment = TextAlignment.Center;
            mMainLabel.FontFamily = new FontFamily("Microsoft YaHei");
            mMainLabel.FontSize = 51;

            mNextBtn = new Label();
            mNextBtn.Content = "下一题";
            mNextBtn.Width = 110;
            mNextBtn.Height = 60;
            mNextBtn.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            mNextBtn.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            mNextBtn.BorderThickness = new Thickness(3);
            mNextBtn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            mNextBtn.FontFamily = new FontFamily("Microsoft YaHei");
            mNextBtn.FontSize = 31;
            mNextBtn.MouseDown += new MouseButtonEventHandler(mNextBtn_MouseDown);

            for(int i = 0; i < 5; i++)
            {
                CompSeniorWords csw = new CompSeniorWords();
                mSelections.Add(csw);
                csw.MouseEnter += new MouseEventHandler(csw_MouseEnter);
                csw.MouseLeave += new MouseEventHandler(csw_MouseLeave);
                csw.MouseDown += new MouseButtonEventHandler(csw_MouseDown);
            }
            
        }

        private bool continuousZeroQuit(int selIndex)
        {
            bool retval = false;

            if (selIndex != -1 && mItems[mCurTillIndex - 1].Weights[selIndex] == 0)
            {
                mContinuousZero++;
                if (mContinuousZero == 5)
                {
                    retval = true;
                    testEnd();
                }
            }
            else
            {
                mContinuousZero = 0;
            }

            return retval;
        }

        void mNextBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = SelectedIdx();
            if (selectedIndex != -1)
            {
                //save result
                mTimer.Stop();
                StSWResult result = new StSWResult();
                result.SelectedItemIndex = selectedIndex;
                result.RT = mTimer.GetElapsedTime();
                mResults.Add(result);
                mTimer.Reset();
                //5 errors
                if (!continuousZeroQuit(selectedIndex))
                {
                    if (selectedIndex != -1)
                    {
                        if (mCurTillIndex < SHEET_LENGTH)
                        {
                            //UI Work
                            deselectAll();
                            nextPage();
                        }
                        else
                        {
                            testEnd();
                        }
                    }
                }
            }
        }

        private string getOutputPath()
        {
            return BASE_FOLDER + OUT_FOLDER + FEITStandard.GetStamp() + ".csv";
        }

        public void PCATDataSaveReport()
        {
            List<QRecStdMultiChoice> report = new List<QRecStdMultiChoice>();
            for (int i = 0; i < SHEET_LENGTH; i++)
            {
                QRecStdMultiChoice rec = new QRecStdMultiChoice();
                int selCount = mItems[i].Weights.Count;
                
                rec.SS = mItems[i].Selections;
                rec.Target = mItems[i].Casual;

                for(int j = 0; j < selCount; j++)
                {
                    rec.SWeight.Add(mItems[i].Weights[j].ToString());
                }

                rec.RT = mResults[i].RT;
                rec.UserAnswer = mResults[i].SelectedItemIndex.ToString();
            }

            mMainWindow.mDB.AddVocSimiRecord(report, mMainWindow.mUserID, Names.VOCAB_TABLENAME);
        }

        private void testEnd()
        {
            PCATDataSaveReport();
            //new SWReportWriter(getOutputPath(), mResults, mItems);

            mOrganizer.EndPage();

            //quit timer
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void TestStart()
        {
            clearAll();
            mOrganizer.arrangeLayout();
            nextPage();
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.TestForward();
        }

        private void deselectAll()
        {
            for (int i = 0; i < 5; i++)
            {
                mSelections[i].SetSelected(false);
            }
        }

        void csw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            deselectAll();

            ((CompSeniorWords)sender).SetSelected(true);
        }

        void csw_MouseLeave(object sender, MouseEventArgs e)
        {
            ((CompSeniorWords)sender).MouseOver(false);
        }

        void csw_MouseEnter(object sender, MouseEventArgs e)
        {
            ((CompSeniorWords)sender).MouseOver(true);
        }

        public void clearAll()
        {
            amCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref amCanvas, ref mAuxBorder800, 
                ref mAuxBorder1024, ref mMainWindow);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amCanvas);
            clearAll();
            buildElements();
            mOrganizer = new OrganizerSeniorWords(this);

            SWItemReader rd = new SWItemReader();

            mItems = rd.ReadSheet(BASE_FOLDER + INPUT_FILE);
            

            mOrganizer.StartPage();
        }

        public void nextPage()
        {
            fillPage(mCurTillIndex);
            mCurTillIndex++;
            mTimer.Start();
        }

        private void fillPage(int index)
        {
            mMainLabel.Text = mItems[index].Casual;
            for (int i = 0; i < 5; i++)
            {
                mSelections[i].SetText(mItems[index].Selections[i]);
            }
        }
    }
}

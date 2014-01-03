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

namespace FiveElementsIntTest.VocabCommon
{
    /// <summary>
    /// PageSeniorWords.xaml 的互動邏輯
    /// </summary>
    public partial class PageVocabCommon : Page
    {
        public MainWindow mMainWindow;

        public List<CompSeniorWords> mSelections;
        public TextBox mMainLabel;
        OrganizerVocabCommon mOrganizer;
        public Label mNextBtn;

        public List<StVCItem> mItems;
        public StVCResult[] mResults;
        private int mCurTillIndex = 2;//begin with the third one
        private int mHideCount = 2;//hide 2 items
        private bool mbReturned = false;
        private int mReturnedAt = -1;

        private FEITTimer mTimer;

        public int SHEET_LENGTH = 40;

        public static String BASE_FOLDER = 
            System.AppDomain.CurrentDomain.BaseDirectory;

        public static String OUT_FOLDER_VOC = "Report\\VocabTest\\";
        public static String OUT_FOLDER_COMM = "Report\\CommonAbstraction\\";
        public static String INPUT_FILE_VOC = "FEITSWsource.txt";
        public static String INPUT_FILE_COMM = "FEITCOMMsource.txt";

        //public CompOvertimeWarning mWarning;
        public CompCountDown mCountDown;
        public TestType mTestType;

        private List<int> mSocres8 = new List<int>();

        public PageVocabCommon(MainWindow _mainWindow, TestType type)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
            mTestType = type;

            checkOutputFolder();

            mSelections = new List<CompSeniorWords>();
            mResults = new StVCResult[40];
            prefillExtraItemzResult();
            mTimer = new FEITTimer();

            /*if (!mMainWindow.mDB.TableExists(Names.VOCAB_TABLENAME))
            {
                mMainWindow.mDB.CreateVocabTable(SHEET_LENGTH);
            }*/

            mCountDown = new CompCountDown();
            mCountDown.Duration = 30;
            mCountDown.FunctionElapsed = nextBtnDownCore;
        }

        private void prefillExtraItemzResult()
        {
            for (int i = 0; i < 2; i++)
            {
                mResults[i] = new StVCResult();
                mResults[i].RT = -1;
                mResults[i].SelectedItemIndex = -1;
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
            if (mTestType == TestType.Vocabulary)
            {
                if (!Directory.Exists(BASE_FOLDER + OUT_FOLDER_VOC))
                {
                    Directory.CreateDirectory(BASE_FOLDER + OUT_FOLDER_VOC);
                }
            }
            else if(mTestType == TestType.Similarity)
            {
                if (!Directory.Exists(BASE_FOLDER + OUT_FOLDER_COMM))
                {
                    Directory.CreateDirectory(BASE_FOLDER + OUT_FOLDER_COMM);
                }
            }
        }

        private void buildElements()
        {
            mMainLabel = new TextBox();
            mMainLabel.Width = 800;
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
            mNextBtn.MouseUp += new MouseButtonEventHandler(mNextBtn_MouseUp);

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

            if (selIndex != -1)/* && mItems[mCurTillIndex - 1].Weights[selIndex] != 2)*/
            {
                mSocres8.Add(mItems[mCurTillIndex - 1].Weights[selIndex]);
            }
            else if(selIndex == -1)
            {
                mSocres8.Add(0);
            }

            if(mSocres8.Count > 8)//remove extra scores (more than 8) form head
            {
                int limit = mSocres8.Count - 8;
                for (int i = 0; i < limit; i++)
                {
                    mSocres8.Remove(mSocres8[0]);
                }
            }

            if(mSocres8.Count == 8)
            {
                int sum = 0;
                for(int i = 0 ; i < mSocres8.Count; i++)
                {
                    sum += mSocres8[i];
                }

                if(sum <= 6)
                {
                    retval = true;
                    testEnd();
                }
            }

            return retval;
        }

        void nextBtnDownCore()
        {
            int selectedIndex = SelectedIdx();

                //save result
            mTimer.Stop();
            StVCResult result = new StVCResult();
            result.SelectedItemIndex = selectedIndex;
            result.RT = mTimer.GetElapsedTime();
            mResults[mCurTillIndex - 1] = result;
            mTimer.Reset();
            //mtWarn.Enabled = false;

            //return process
            bool continuousErrQuit = continuousZeroQuit(selectedIndex);
            int thisScore = 0;
            if (selectedIndex != -1)
            {
                thisScore = mItems[mCurTillIndex - 1].Weights[result.SelectedItemIndex];
            }

            if (mCurTillIndex - 1 < 5 && mbReturned == false)//go return
            {
                if (thisScore != 2)
                {
                    mReturnedAt = mCurTillIndex;
                    mCurTillIndex = 0;
                    mbReturned = true;
                }
            }

            if (mCurTillIndex - 1 == 1)//return process finished
            {
                mCurTillIndex = mReturnedAt;
            }

            if (mCurTillIndex - 1 == 5 && !mbReturned)//count the skipped items as correct
            {
                for (int i = 0; i < mHideCount; i++)
                {
                    mSocres8.Add(2);
                }
            }


            //8 items more than 6 socres
            if (!continuousErrQuit)
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

        void mNextBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SelectedIdx() != -1)
            {
                nextBtnDownCore();
            }
        }

        private string getOutputPath()
        {
            if (mTestType == TestType.Vocabulary)
            {
                return BASE_FOLDER + OUT_FOLDER_VOC + 
                    mMainWindow.mDemography.GenBriefString() + ".txt";
            }
            else if (mTestType == TestType.Similarity)
            {
                return BASE_FOLDER + OUT_FOLDER_COMM + 
                    mMainWindow.mDemography.GenBriefString() + ".txt";
            }
            else
            {
                return null;
            }
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
            //PCATDataSaveReport();
            mCountDown.Stop();
            new VCReportWriter(getOutputPath(), mResults, mItems);

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

        public delegate void timeDele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(new timeDele(mMainWindow.TestForward), 
                System.Windows.Threading.DispatcherPriority.Normal);
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
            mOrganizer = new OrganizerVocabCommon(this);

            VCtemReader rd = new VCtemReader();

            if (mTestType == TestType.Vocabulary)
            {
                mItems = rd.ReadSheet(BASE_FOLDER + INPUT_FILE_VOC);
            }
            else if (mTestType == TestType.Similarity)
            {
                mItems = rd.ReadSheet(BASE_FOLDER + INPUT_FILE_COMM);
            }
            
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

            mCountDown.Stop();

            if (!amCanvas.Children.Contains(mCountDown))
            {
                amCanvas.Children.Add(mCountDown);
                Canvas.SetTop(mCountDown, FEITStandard.PAGE_BEG_Y + 580);
                Canvas.SetLeft(mCountDown, FEITStandard.PAGE_BEG_X + 
                    (FEITStandard.PAGE_WIDTH - 300) / 2);//center
            }

            mCountDown.Duration = 30;
            mCountDown.Start();
        }
    }
}

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
using System.Windows.Threading;
using PCATData;

namespace FiveElementsIntTest.GraphicRecog
{
    /// <summary>
    /// PageGraphicRecog.xaml 的互動邏輯
    /// </summary>
    public partial class PageGraphicRecog : Page
    {
        private MainWindow mMainWindow;
        public UIImageGroup mImageGroup;
        public CompImage mLearnImage1, mLearnImage2, mTargetImage;
        public List<String> mLearnList;
        public List<String> mDoList;
        public List<String> mUserAnswer;
        public LayoutGFR mLayout;

        public FEITTimer mTimer;
        public Image mDoubleArrow;

        public static String TEST_FOLDER = "GR\\";

        public String itemFileAddr;
        public String learningFileAddr;

        public bool mStateSwitch = false;

        public enum STATE
        {
           LEARN, SPAN
        };

        public int mCurTillItem = 0;
        public int mCurTillLearn = 0;
        public int mCorrectCount = 0;

        public STATE mCurState = STATE.LEARN;

        private IntPtr mIntPtr = IntPtr.Zero;

        public PageGraphicRecog(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;

            itemFileAddr = FEITStandard.BASE_FOLDER + TEST_FOLDER + "items.csv";
            learningFileAddr = FEITStandard.BASE_FOLDER + TEST_FOLDER + "instruct.csv";

            mLearnList = GRFormReader.GetList(learningFileAddr, GRFormReader.SCRIPT_TYPE.LEARNING);
            mDoList = GRFormReader.GetList(itemFileAddr, GRFormReader.SCRIPT_TYPE.TEST);
            mUserAnswer = new List<String>();

            mLayout = new LayoutGFR(this);

            mImageGroup =
                new UIImageGroup(saveUserSelection, Next);

            mLearnImage1 = new CompImage(0);
            mLearnImage2 = new CompImage(0);
            mTargetImage = new CompImage(0);

            mTimer = new FEITTimer();

            mDoubleArrow = new Image();
            mDoubleArrow.Width = 90;
            mDoubleArrow.Height = 31;
            System.Drawing.Bitmap bmp = FiveElementsIntTest.Properties.Resources.DOUBLE_ARROW;
            mDoubleArrow.Source = BitmapSourceFactory.GetBitmapSource(bmp, out mIntPtr);

            if (!mMainWindow.mDB.TableExists(Names.GRAPH_ASSO_TABLENAME))
            {
                mMainWindow.mDB.CreateGraphAssoTable(GRFormReader.TOTAL_ITEM);
            }
        }

        ~PageGraphicRecog()
        {
            if (mIntPtr != IntPtr.Zero)
                BitmapSourceFactory.DeleteObject(mIntPtr);
        }

        public void TestStart()
        {
            clearAll();
            mLayout.LayLearningPage();
            Next();
        }

        private void testEnd()
        {
            mLayout.LayEndPage();
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed_end);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        void t_Elapsed_end(object sender, ElapsedEventArgs e)
        {
            mMainWindow.TestForward();
        }

        public void PCATDataSaveReport()
        {
            List<QRecStdMultiChoice> report = new List<QRecStdMultiChoice>();

            int indexCursor = 0;

            for (int i = 0; i < mUserAnswer.Count; i++)
            {
                QRecStdMultiChoice single = new QRecStdMultiChoice();

                single.Target = mDoList[indexCursor];
                indexCursor++;

                for (int j = 0; j < 8; j++)
                {
                    single.SS.Add(mDoList[indexCursor]);
                    indexCursor++;
                }

                single.CorrectAnswer = mDoList[indexCursor];
                indexCursor++;

                single.UserAnswer = mUserAnswer[i];

                report.Add(single);
            }

            mMainWindow.mDB.AddAssoRecord(report, 
                mMainWindow.mUserID, Names.GRAPH_ASSO_TABLENAME);
        }

        public void Next()
        {
            if (mCurTillItem == GRFormReader.TOTAL_ITEM)
            {
                //(new GRFormWriter()).Save(this);
                PCATDataSaveReport();
                testEnd();
            }
            else
            {
                if (mStateSwitch)
                {
                    stateSwitch();
                }
                else
                {
                    switch (mCurState)
                    {
                        case STATE.LEARN:
                            learnNext();
                            break;
                        case STATE.SPAN:
                            spanNext();
                            break;
                    }
                }
            }
        }

        private void stateSwitch()
        {
            mStateSwitch = false;
            if (mCurState == STATE.SPAN)
            {
                clearAll();
                mLayout.LayTestTitle();
            }
            else
            {
                clearAll();
                mLayout.LayLearnTitle();
            }
        }

        private void learnNext()
        {
            mLayout.UpdateLearningPage(mCurTillLearn);
            mCurTillLearn++;

            if ((mCurTillLearn) % GRFormReader.LEARNINGSPAN_PAGE_COUNT == 0)
            {
                mCurState = STATE.SPAN;
                mStateSwitch = true;
            }

            Timer t = new Timer();
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed_next);
        }

        public delegate void systemThreadDelegate();

        void t_Elapsed_next(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new systemThreadDelegate(Next));
        }

        private void spanNext()
        {
            mLayout.UpdateSelectionPage(mCurTillItem);
            mTimer.Reset();
            mTimer.Start();

            mCurTillItem++;

            if ((mCurTillItem) % GRFormReader.TESTSPAN_PAGE_COUNT == 0)
            {
                mCurState = STATE.LEARN;
                mStateSwitch = true;
            }
        }

        private void saveUserSelection(int senderID)
        {
            mTimer.Stop();

            mUserAnswer.Add(senderID.ToString());

            if (senderID == getAnswer(mCurTillItem - 1))
            {
                mUserAnswer.Add("T");
                mCorrectCount++;
            }
            else
            {
                mUserAnswer.Add("F");
            }

            mUserAnswer.Add(mTimer.GetElapsedTime() + "ms");

            //count correct item in span
            if ((mCurTillItem) % GRFormReader.TESTSPAN_PAGE_COUNT == 0)
            {
                mUserAnswer.Add(mCorrectCount.ToString());
                mCorrectCount = 0;
            }
        }

        public int getAnswer(int itemIndex)
        {
            int retval = -1;
            retval = Int32.Parse(mDoList[itemIndex * GRFormReader.IN_TEST_PAGE + 9]);
            return retval;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amCanvas);
            clearAll();

            mLayout.LayStartPage();
        }

        public void clearAll()
        {
            amCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref amCanvas, ref amBorder800, 
                ref amBorder1024, ref mMainWindow);
        }
    }
}

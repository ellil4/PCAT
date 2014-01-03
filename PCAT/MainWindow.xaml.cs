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
using FiveElementsIntTest.OpSpan;
using FiveElementsIntTest.SymSpan;
using FiveElementsIntTest.ITFigure;
using FiveElementsIntTest.Cube;
using FiveElementsIntTest.VocabCommon;
using FiveElementsIntTest.GraphicRecog;
using FiveElementsIntTest.SybSrh;
using FiveElementsIntTest.CtSpan;
using FiveElementsIntTest.PairedAsso;
using FiveElementsIntTest.Paper;
using FiveElementsIntTest.PortraitMemory;

using PCATData;
using Network;

namespace FiveElementsIntTest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public bool mbEngiMode = false;
        public PCATTableRoutine mDB;
        public String mServerIPA = "";
        public ConnectionInfo mConnInfo;
        public long mUserID = -1;
        public Client mClient;
        public StDemography mDemography;

        public int mTestAt = -1;
        public List<TestType> mTestList;
        public PCATData.VERSION mVersion;

        public void GoToTest(TestType type)
        {
            switch (type)
            {
                case TestType.DigitSymbol:
                    if(mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("数字符号");

                    NavigationService.Navigate(new PageDigitSymbol(this));
                    break;
                case TestType.SymbolSearch:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("符号搜索");

                    NavigationService.Navigate(new PageSybSrh(this));
                    break;
                case TestType.OpSpan:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("操作广度");

                    NavigationService.Navigate(new PageOpSpan(this));
                    break;
                case TestType.SymSpan:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("对称广度");

                    NavigationService.Navigate(new PageSymmSpan(this));
                    break;

                case TestType.CtSpan:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("计数广度");

                    NavigationService.Navigate(new PageCtSpan(this));
                    break;
                case TestType.VocAsso:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("词对联想");

                    NavigationService.Navigate(new PagePairedAsso(this));
                    break;
                case TestType.GraphAsso:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("图对联想");

                    NavigationService.Navigate(new PageGraphicRecog(this));
                    break;
                case TestType.Paper:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("折纸测验");
                    //////////////////////////////////////////////////////////////////////////////
					NavigationService.Navigate(new PagePaper(this));
                    break;
                case TestType.Cube:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("魔方旋转");

                    NavigationService.Navigate(new PageCube(this));
                    break;
                case TestType.Vocabulary:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("词汇测验");

                    NavigationService.Navigate(new PageVocabCommon(this, TestType.Vocabulary));
                    break;
                case TestType.Similarity:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("类同测验");

                    NavigationService.Navigate(new PageVocabCommon(this, TestType.Similarity));
                    break;
					
				case TestType.PortraitMemory:
                    if (mVersion == VERSION.CLIENT)
                        mClient.SendTestBeginMessage("人像特点联系回忆");
						
                    NavigationService.Navigate(new PagePortrailtMemory(this));
                    break;
                case TestType.OpSpan2:
                    NavigationService.Navigate(new OpSpan2.BasePage(this, SECOND_ARCHI_TYPE.OPSPAN));
                    break;
                case TestType.SymmSpan2:
                    NavigationService.Navigate(new OpSpan2.BasePage(this, SECOND_ARCHI_TYPE.SYMMSPAN));
                    break;
            }
        }

        public void TestForward()
        {
            if (mTestAt + 1 < mTestList.Count)
            {
                mTestAt++;
                GoToTest(mTestList[mTestAt]);
            }
            else
            {
                if (mClient != null)
                    mClient.SendAllEndMessage();

                if (mVersion == VERSION.CLIENT)
                {
                    System.Environment.Exit(0);
                }
                else if (mVersion == VERSION.STANDALONE)
                {
                    this.Close();
                }
            }
        }

        public MainWindow(List<TestType> testList, PCATData.VERSION version)
        {
            InitializeComponent();
            this.Cursor = Cursors.Hand;

            if (!mbEngiMode)
            {
                //disable the UI
                this.ShowsNavigationUI = false;

                //disable the hotkey
                this.CommandBindings.Add(
                    new CommandBinding(NavigationCommands.BrowseBack, OnBrowseBack));
                //disable the hotkey
                this.CommandBindings.Add(
                    new CommandBinding(NavigationCommands.BrowseForward, BrowseForward));
            }

            //get list from different source
            if (mVersion == VERSION.STANDALONE)
            {
                mTestList = testList;
            }
            else if (mVersion == VERSION.MANAGER)
            {
                ////////////////////////////////////////////////////////////////////////////
            }

            mVersion = version;
        }

        //override to disable the hotkey
        void OnBrowseBack(object sender, ExecutedRoutedEventArgs args)
        { }

        //override to disable the hotkey
        void BrowseForward(object sender, ExecutedRoutedEventArgs args)
        { }

        private void FullScreen()
        {
            if (!mbEngiMode)
            {
                this.WindowState = System.Windows.WindowState.Normal;
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
                //this.Topmost = true;

                this.Left = 0;
                this.Top = 0;
                this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
                this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Maximized;
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
            }
        }

        private void NavigationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FullScreen();
            if (!mbEngiMode)
                this.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            //entrance
            if (mVersion == VERSION.CLIENT)
            {
                if (String.IsNullOrEmpty(mServerIPA))
                    (new ServerConfigDialog(this)).ShowDialog();
            }

            if (((mVersion == VERSION.CLIENT) && !(String.IsNullOrEmpty(mServerIPA))))
            {
                mClient = new Client(Methods.GetIPAFromString(mServerIPA));
            }

            //Test code in....
            //undefined <demography pane disabled>
            if (new DemographyPane(this).ShowDialog() == true)
            {
                TestForward();
            }
            //Test code out....
        }

        private void NavigationWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ControlPane cp = new ControlPane(this);
                cp.ShowDialog();
            }

            if (e.Key == Key.F1)
            {
                ServerConfigDialog scd =  new ServerConfigDialog(this);
                scd.ShowDialog();

                if (scd.mbSet)
                {
                    (new DemographyPane(this)).ShowDialog();
                }
            }
        }
    }
}

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
using PCATData;
using Network;
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;

namespace Query
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public VERSION mVersion;
        public PCATTableRoutine mTableManager;
        public ConnectionInfo mConnInfo;
        public bool mbConnected = false;
        private Thread mThreadService;
        public ObservableCollection<SubjectListDataLine> mSubjectCollection;
        public ObservableCollection<RecordListDataLine> mRecordCollection;

        private NetControl mNc;

        public MainWindow(VERSION version)
        {
            InitializeComponent();
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;

            mVersion = version;

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initDataGrids();
            initUIByVersion();
        }

        private void initUIByVersion()
        {
            if (mVersion == VERSION.STANDALONE)
            {
                amTabControl.Items.Remove(amTabItemServer);
                
                //display or not
                amBtnRefresh.Visibility = System.Windows.Visibility.Hidden;
                amBtnDataServer.Visibility = System.Windows.Visibility.Hidden;
                amBtnDisconn.Visibility = System.Windows.Visibility.Hidden;
                
            }
            else if (mVersion == VERSION.MANAGER)
            {
                amBtnDisconn.IsEnabled = false;

                //display or not
                amBtnPassword.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        

        private void initDataGrids()
        {
            mRecordCollection = new ObservableCollection<RecordListDataLine>();
            amDataGridUser.DataContext = mRecordCollection;
            
            if (mVersion == VERSION.MANAGER)
            {
                mSubjectCollection = new ObservableCollection<SubjectListDataLine>();
                amDataGridService.DataContext = mSubjectCollection;
            }
            else if (mVersion == VERSION.STANDALONE)
            {
                mTableManager = new PCATTableRoutine(PCATData.Names.SQLitePath);

                (new PasswordRequestPan(mTableManager, this)).ShowDialog();//password check

                loadDBRecPage();
            }
        }

        private void loadDBRecPage()
        {
            
            List<QRecUser> users = mTableManager.AcquireUserRecords();

            for (int i = 0; i < users.Count; i++)
            {
                addUserRecord(users[i]);
            }
        }

        private void clearUserRecord()
        {
            mRecordCollection.Clear();
            amDataGridUser.Items.Refresh();
        }

        private void addUserRecord(QRecUser info)
        {
            RecordListDataLine rld = new RecordListDataLine(
                info.ID, info.GroupMark, info.Time, info.Name, info.Gender, info.Age);
            mRecordCollection.Add(rld);
        }

        private void removeUserRecord(int index)
        {
            mRecordCollection.RemoveAt(index);
        }

        private static SubjectListDataLine transStrList2SDL(List<String> strArr)
        {
            SubjectListDataLine SLD = 
                new SubjectListDataLine(
                     strArr[0], strArr[1], strArr[2], strArr[3], strArr[4], strArr[5]);
            return SLD;
        }

        public delegate void addRecInvokeDelegate(SubjectListDataLine dataLine);

        public void AddServerSubjectCtrlRecord(List<String> strArr)
        {
           Dispatcher.Invoke(DispatcherPriority.Normal, 
               new addRecInvokeDelegate(addServerSubjectCtrlRecord), (transStrList2SDL(strArr)));
        }

        private void clearServerSubjectCtrlRecord()
        {
            mSubjectCollection.Clear();
            amDataGridService.Items.Refresh();
        }

        private void addServerSubjectCtrlRecord(SubjectListDataLine dataLine)
        {
            mSubjectCollection.Add(dataLine);
        }

        public int GetServerSubjectCtrlRecordIndex(String ipa)
        {
            int retval = -1;
            for (int i = 0; i < mSubjectCollection.Count; i++)
            {
                if (mSubjectCollection[i].IP.Equals(ipa))
                {
                    retval = i;
                    break;
                }
            }

            return retval;
        }

        public delegate void removeRecDelegate(String ipa);

        public void RemoveServerSubjectCtrlRecord(String ipa)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new removeRecDelegate(removeServerSubjectCtrlRecord), ipa);
        }

        private void removeServerSubjectCtrlRecord(String ipa)
        {
            int index = GetServerSubjectCtrlRecordIndex(ipa);
            mSubjectCollection.RemoveAt(index);
        }

        public delegate void changeRecStaDelegate(String ipa, String newStatus);

        public void ChangeServerSubjectCtrlRecordStatus(String ipa, String newStatus)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new changeRecStaDelegate(changeServerSubjectCtrlRecordStatus), ipa, newStatus);
        }

        private void changeServerSubjectCtrlRecordStatus(String ipa, String newStatus)
        {
            int index = GetServerSubjectCtrlRecordIndex(ipa);
            mSubjectCollection[index].Status = newStatus;
            amDataGridService.Items.Refresh();
        }

        //for MySQL
        public void SetupDataBaseServerConnection(ConnectionInfo info)
        {
            try
            {
                mConnInfo = info;
                mTableManager = new PCATTableRoutine(mConnInfo, VERSION.MANAGER);

            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                if (e.Number == 1042)
                    throw e;
            }
        }

        private void amQuitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void amBtnTestConfig_Click(object sender, RoutedEventArgs e)
        {
            (new TestConfig()).ShowDialog();
        }

        //set local ip
        private delegate void SetLocaIPDisplayDelegate(IPAddress ipa);

        private void SetLocalIPDisplay(IPAddress ipa)
        {
            amLabelThisIP.Content = "本机：" + ipa.ToString();
        }

        private void SetLocalIPDisplayInvoke(IPAddress ipa)
        {
            Dispatcher.Invoke(new SetLocaIPDisplayDelegate(SetLocalIPDisplay), ipa);
        }

        private void amBtnDataServer_Click(object sender, RoutedEventArgs e)
        {
            DataServerConfigWnd configWnd = new DataServerConfigWnd(this);
            configWnd.ShowDialog();

            if (mbConnected == true)
            {
                loadDBRecPage();
                amLabDataConnSta.Content =
                    "状态：已连接至" + mConnInfo.Server + ":" + mConnInfo.Port;

                //start register service below
                mNc = new NetControl(mTableManager, 
                    TestConfig.GetTestStrArrayFromFile());

                mNc.mfOnServerStart = SetLocalIPDisplayInvoke;
                mNc.mAddRecFunc = AddServerSubjectCtrlRecord;
                mNc.mRemoveRecFunc = RemoveServerSubjectCtrlRecord;
                mNc.mChangeRecStaFunc = ChangeServerSubjectCtrlRecordStatus;
                mNc.mListener = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), 8500);

                mThreadService = new Thread(mNc.ServerStart);
                mThreadService.Start();
                amBtnTestConfig.IsEnabled = false;
                amBtnDisconn.IsEnabled = true;
            }
        }

        

        private void amBtnDisconn_Click(object sender, RoutedEventArgs e)
        {
            mbConnected = false;
            //stop register service here
            if (mThreadService != null)
            {
                mThreadService.Abort();
                mThreadService = null;
            }

            if (mNc != null && mNc.mListener != null)
            {
                mNc.mListener.Stop();
                mNc.mListener = null;
            }
            //UI
            clearUserRecord();
            amLabDataConnSta.Content = "状态：断开";
            amLabelThisIP.Content = "本机：";
            amBtnTestConfig.IsEnabled = true;
            amBtnDisconn.IsEnabled = false;

        }

        private void fillUserUIInfo(long id)
        {
            QRecUser userInfo = mTableManager.QueryUserTable(id);
            amTBName.Text = userInfo.Name;
            amTBGender.Text = userInfo.Gender;
            amTBAge.Text = userInfo.Age;
            amTBJob.Text = userInfo.Job;
            amTBEduBG.Text = userInfo.Qualif;
            amTBHealth.Text = userInfo.Health;
            amRichNote.Document.Blocks.Clear();
            amRichNote.AppendText(userInfo.Other);
        }

        public RecordListDataLine mRLD;
        public SubjectListDataLine mSLD;

        private void updateDBInfo()
        {
            updateDBUserInfo();
            //update other tables here............................................................
        }

        private void updateDBUserInfo()
        {
            QRecUser user = new QRecUser();
            user.GroupMark = mRLD.GroupMark;
            user.Time = mRLD.TestTime;
            user.ID = mRLD.ID;

            user.Name = amTBName.Text;
            user.Age = amTBAge.Text;
            user.Gender = amTBGender.Text;
            user.Health = amTBHealth.Text;
            user.Qualif = amTBEduBG.Text;
            user.Job = amTBJob.Text;
            TextRange txtRange = new TextRange(amRichNote.Document.ContentStart, amRichNote.Document.ContentEnd);
            user.Other = txtRange.Text;

            mRLD.SubjectName = user.Name;
            mRLD.Age = user.Age;
            mRLD.Gender = user.Gender;

            amDataGridUser.Items.Refresh();
            mTableManager.UpdateUserTable(user);
        }

        private void amDataGridUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update 2 DB
            //update 2 UI
            if (mVersion == VERSION.STANDALONE || 
                (mVersion == VERSION.MANAGER && mbConnected))
            {
                if (mRLD != null)
                {
                    updateDBInfo();
                }

                mRLD = (RecordListDataLine)amDataGridUser.SelectedItem;
                long id = long.Parse(mRLD.ID);
                fillUserUIInfo(id);
            }
        }

        private void amDataGridService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update 2 UI
            if (mVersion == VERSION.STANDALONE ||
                (mVersion == VERSION.MANAGER && mbConnected))
            {
                mSLD = (SubjectListDataLine)amDataGridUser.SelectedItem;
                long id = long.Parse(mSLD.ID);
                fillUserUIInfo(id);
            }
        }

        private void anBtnSetNum_Click(object sender, RoutedEventArgs e)
        {
            (new GroupMarkPan(mTableManager)).ShowDialog();
        }

        private void amBtnPassword_Click(object sender, RoutedEventArgs e)
        {
            (new PasswordPan(mTableManager)).ShowDialog();
        }
    }
}

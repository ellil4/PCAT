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
using System.Windows.Shapes;
using Network;
using System.Text.RegularExpressions;
using PCATData;

namespace FiveElementsIntTest
{
    /// <summary>
    /// DemographyPane.xaml 的互動邏輯
    /// </summary>
    public partial class DemographyPane : Window
    {
        MainWindow mMW;
        bool mbSet = false;
        public DemographyPane(MainWindow mw)
        {
            InitializeComponent();

            mMW = mw;
        }



        private void amBtnStart_Click(object sender, RoutedEventArgs e)
        {
            //register here
            //get test list here
            if (mMW.mVersion == PCATData.VERSION.CLIENT)
            {
                Client client = new Client(Methods.GetIPAFromString(mMW.mServerIPA));
                client.mfDoGrantFunc = callbackStartTest;

                //register client version
                TextRange txtRange = new TextRange(amRichNote.Document.ContentStart, amRichNote.Document.ContentEnd);
                int age = -1;
                if (int.TryParse(amTBAge.Text, out age))
                {
                    client.RegisterRequest(removeQuot(amTBName.Text),
                            removeQuot((String)((ComboBoxItem)amComboGender.SelectedItem).Content),
                            age,
                            removeQuot((String)((ComboBoxItem)amComboHealth.SelectedItem).Content),
                            removeQuot(amTBEduBG.Text),
                            removeQuot((String)((ComboBoxItem)amComboJob.SelectedItem).Content),
                            removeQuot(txtRange.Text));
                }
                else
                {
                    MessageBox.Show(
                        "须输入整数作为年龄", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                    
            }
            else if (mMW.mVersion == PCATData.VERSION.STANDALONE)
            {
                callbackStartTest(null);
            }
        }

        public void callbackStartTest(String message)
        {
            mMW.mTestAt = 0;
            mbSet = true;

            //parse message to keep server information and make a testList
            if (mMW.mVersion == PCATData.VERSION.CLIENT)
            {
                //server
                Regex rex = new Regex("[^\t]+");
                MatchCollection mts = rex.Matches(message);
                mMW.mConnInfo =
                    new PCATData.ConnectionInfo(
                        mts[1].Value, mts[2].Value, mts[3].Value, mts[4].Value, mts[5].Value);

                //user id
                mMW.mUserID = long.Parse(mts[6].Value);

                //testList
                mMW.mTestList = new List<TestType>();
                for (int i = 7; i < mts.Count; i++)
                {
                    mMW.mTestList.Add(
                        (TestType)FEITStandard.GetTestIndexByName(mts[i].Value));
                }
            }

            if (mMW.mVersion == VERSION.STANDALONE)
            {
                mMW.mDB = new PCATTableRoutine(PCATData.Names.SQLitePath);
            }
            else if (mMW.mVersion == VERSION.CLIENT)
            {
                mMW.mDB = new PCATTableRoutine(mMW.mConnInfo, VERSION.CLIENT);
            }

            //register standalone
            if (mMW.mVersion == VERSION.STANDALONE)
            {
                TextRange txtRange = new TextRange(amRichNote.Document.ContentStart, amRichNote.Document.ContentEnd);
                int age = -1;
                if (int.TryParse(amTBAge.Text, out age))
                {
                    StUserRegisterFeedback fb = mMW.mDB.AddUser(removeQuot(amTBName.Text),
                            removeQuot((String)((ComboBoxItem)amComboGender.SelectedItem).Content),
                            age,
                            removeQuot((String)((ComboBoxItem)amComboHealth.SelectedItem).Content),
                            removeQuot(amTBEduBG.Text),
                            removeQuot((String)((ComboBoxItem)amComboJob.SelectedItem).Content),
                            removeQuot(txtRange.Text));

                    mMW.mUserID = fb.id;
                    mMW.GoToTest(mMW.mTestList[mMW.mTestAt]);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "须输入整数作为年龄", "信息", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (mMW.mVersion == VERSION.CLIENT)
            {
                mMW.TestForward();
                this.Close();
            }
        }

        private String removeQuot(String input)
        {
            return input.Replace('\'',' ');
        }

        private void amBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!mbSet)
            {
                mMW.Close();
            }
        }
    }
}

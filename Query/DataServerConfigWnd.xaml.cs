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
using PCATData;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Query
{
    /// <summary>
    /// DataServerConfigWnd.xaml 的互動邏輯
    /// </summary>
    public partial class DataServerConfigWnd : Window
    {
        public MainWindow mMW;
        
        public DataServerConfigWnd(MainWindow mw)
        {
            InitializeComponent();
            mMW = mw;
        }

        private bool isIP(String input)
        {
            Regex rex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return rex.IsMatch(input);
        }

        private bool isNameOrPass(String input)
        {
            Regex rex = new Regex("^[a-zA-Z]\\w{0,17}$");
            return rex.IsMatch(input);
        }

        private bool isPort(String input)
        {
            Regex rex = new Regex("^[0-9]*$");
            return rex.IsMatch(input);
        }

        private void amBtnConnect_Click(object sender, RoutedEventArgs e)
        {
            bool checkOK = true; 

            if (!isIP(amTBServerIPA.Text) && checkOK)
            {
                MessageBox.Show("IP地址输入错误", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                checkOK = false;
            }

            if (!isNameOrPass(amTBDBName.Text) && checkOK)
            {
                MessageBox.Show("数据库名称输入不正确，正确格式为：以字母开头，长度在6~18之间，只能包含字符、数字和下划线", 
                    "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                checkOK = false;
            }

            if (!isNameOrPass(amTBUserName.Text) && checkOK)
            {
                MessageBox.Show("用户名输入不正确，正确格式为：以字母开头，长度在6~18之间，只能包含字符、数字和下划线",
                    "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                checkOK = false;
            }

            if (!isPort(amTBPort.Text) && checkOK)
            {
                MessageBox.Show("端口输入错误，必须是数字", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                checkOK = false;
            }

            try
            {
                if (checkOK)
                {
                    ConnectionInfo ci = new ConnectionInfo();
                    ci.Server = amTBServerIPA.Text;
                    ci.DBName = amTBDBName.Text;
                    ci.UserName = amTBUserName.Text;
                    ci.Port = amTBPort.Text;
                    ci.Password = amTBPass.Password;

                    mMW.SetupDataBaseServerConnection(ci);
                    mMW.mbConnected = true;
                    Close();
                }
            }
            catch (MySqlException err)
            {
                if (err.Number == 1042)
                {
                    MessageBox.Show("无法与数据服务器建立连接",
                        "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void amBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (mMW.mConnInfo != null)
            {
                amTBServerIPA.Text = mMW.mConnInfo.Server;
                amTBDBName.Text = mMW.mConnInfo.DBName;
                amTBUserName.Text = mMW.mConnInfo.UserName;
                amTBPort.Text = mMW.mConnInfo.Port;
                amTBPass.Password = mMW.mConnInfo.Password;
            }
        }
    }
}

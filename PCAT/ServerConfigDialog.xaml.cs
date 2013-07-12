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
using System.Net;
using System.Windows.Threading;

namespace FiveElementsIntTest
{
    /// <summary>
    /// ServerConfigDialog.xaml 的互動邏輯
    /// </summary>
    public partial class ServerConfigDialog : Window
    {
        MainWindow mMW;
        public bool mbSet = false;
        public ServerConfigDialog(MainWindow mw)
        {
            InitializeComponent();
            mMW = mw;
            amTBIPA.Text = mMW.mServerIPA;
        }

        private static bool isIP(String input)
        {
            Regex rex =
                new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

            return rex.IsMatch(input);
        }

        private delegate void UIPingOkDelegate();

        private void PingOK()
        {
            mMW.mServerIPA = amTBIPA.Text;
            mbSet = true;
            Close();
        }

        private void PingNoResonse()
        {
            amLabelLinkSta.Content = "信息：服务器没有响应";
        }

        private void setPingOKStatus()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new UIPingOkDelegate(PingOK));
        }

        public void DoPing(String iap)
        {
            IPAddress ipa = Methods.GetIPAFromString(amTBIPA.Text);
            Client client = new Client(ipa);
            client.mfPingOKFunc = setPingOKStatus;
            amLabelLinkSta.Content = "请等待";

            try
            {
                client.Ping();
            }
            catch (System.Net.Sockets.SocketException)
            {
                PingNoResonse();
            }
        }

        private void amBtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (isIP(amTBIPA.Text))
            {
                DoPing(amTBIPA.Text);
            }
            else
            {
                amLabelLinkSta.Content = "信息：IP地址格式不正确";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(mMW.mServerIPA))
            {
                mMW.Close();
            }
        }
    }
}

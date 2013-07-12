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

namespace Query
{
    /// <summary>
    /// PasswordRequestPan.xaml 的互動邏輯
    /// </summary>
    public partial class PasswordRequestPan : Window
    {
        MainWindow mMW;
        PCATData.PCATTableRoutine mDB;
        bool mPass = false;

        public PasswordRequestPan(PCATData.PCATTableRoutine db, MainWindow mw)
        {
            InitializeComponent();
            mDB = db;
            mMW = mw;
        }

        private void confirm()
        {
            String op = mDB.GetPassword();
            if (op.Equals(amPasswordBox.Password))
            {
                mPass = true;
                Close();
            }
            else
            {
                MessageBox.Show("密码不正确",
                    "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void amBtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            confirm();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!mPass)
                mMW.Close();
        }

        private void amPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                confirm();
            }
        }
    }
}

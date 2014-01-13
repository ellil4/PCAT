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
    /// PasswordPan.xaml 的互動邏輯
    /// </summary>
    public partial class PasswordPan : Window
    {
        public PCATData.PCATTableRoutine mDB;

        public PasswordPan(PCATData.PCATTableRoutine db)
        {
            InitializeComponent();
            mDB = db;
        }

        private void amBtnCommitChange_Click(object sender, RoutedEventArgs e)
        {
            String pwOld = mDB.GetPassword();
            if (pwOld.Equals(amTBOldPassword.Password))
            {
                if (amTBNewPassword.Password.Equals(amTBNewConfirm.Password))
                {
                    mDB.SetPassword(amTBNewPassword.Password);
                    Close();
                }
                else
                {
                    MessageBox.Show("两次输入的新密码不一致", 
                        "提示", MessageBoxButton.OK, MessageBoxImage.Information); 
                }
            }
            else
            {
                MessageBox.Show("旧密码不正确", 
                    "提示", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }
    }
}

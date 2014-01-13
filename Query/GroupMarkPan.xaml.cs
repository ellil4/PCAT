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

namespace Query
{
    /// <summary>
    /// GroupMarkPan.xaml 的互動邏輯
    /// </summary>
    public partial class GroupMarkPan : Window
    {
        PCATTableRoutine mTableManager = null;

        public GroupMarkPan(PCATTableRoutine tableManager)
        {
            InitializeComponent();
            mTableManager = tableManager;
        }

        private void amBtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long newNum = long.MinValue;
                if (Int64.TryParse(amTBGroupMark.Text, out newNum))
                {
                    mTableManager.SetOdometer(newNum);
                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "请输入一个整数", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            long num = mTableManager.QueryOdometer(true);
            amTBGroupMark.Text = num.ToString();
        }
    }
}

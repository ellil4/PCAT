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

namespace Hub
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public static PCATData.VERSION mVersion = PCATData.VERSION.STANDALONE;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
        }

        private void amQuitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void amStartBtn_Click(object sender, RoutedEventArgs e)
        {
            FiveElementsIntTest.MainWindow mw = 
                new FiveElementsIntTest.MainWindow(Query.TestConfig.GetTestArrayFromFile(), mVersion);

            mw.ShowDialog();
        }

        private void amResultBtn_Click(object sender, RoutedEventArgs e)
        {
            (new Query.MainWindow(mVersion)).ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (mVersion)
            {
                case PCATData.VERSION.CLIENT:
                    this.Visibility = System.Windows.Visibility.Hidden;
                    FiveElementsIntTest.MainWindow mw = 
                        new FiveElementsIntTest.MainWindow(
                            Query.TestConfig.GetTestArrayFromFile(), mVersion);
                    mw.ShowDialog();
                    this.Close();
                    break;
                case PCATData.VERSION.MANAGER:
                    this.Visibility = System.Windows.Visibility.Hidden;
                    (new Query.MainWindow(mVersion)).ShowDialog();
                    this.Close();
                    break;
            }
        }
    }
}

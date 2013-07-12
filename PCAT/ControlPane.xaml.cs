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

namespace FiveElementsIntTest
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class ControlPane : Window
    {
        public MainWindow mMW;
        private List<RadioButton> mRBs;

        public ControlPane(MainWindow mw)
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;

            mMW = mw;

            //form shape and RB data
            mRBs = new List<RadioButton>();
            for (int i = 0; i < mMW.mTestList.Count; i++)
            {
                RadioButton rb = new RadioButton();
                rb.Content = FEITStandard.TEST_TITLE[(int)mMW.mTestList[i]] + "        ";

                if (i < 5)
                    amStackPane1.Children.Add(rb);
                else
                    amStackPane2.Children.Add(rb);

                mRBs.Add(rb);
            }

            SetSelected(mMW.mTestAt);
        }

        public int GetSelected()
        {
            int i = -1;
            for (i = 0; i < mRBs.Count; i++)
            {
                if (mRBs[i].IsChecked == true)
                    break;
            }

            return i;
        }

        public void SetSelected(int index)
        {
            for (int i = 0; i < mRBs.Count; i++)
            {
                if(i != index)
                    mRBs[i].IsChecked = false;
                else
                    mRBs[i].IsChecked = true;

            }
        }

        private void amBtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mMW.Close();
        }

        private void amBtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            int TestAt = GetSelected();
            mMW.GoToTest(mMW.mTestList[TestAt]);
            mMW.mTestAt = TestAt;
            this.Close();
        }
    }
}

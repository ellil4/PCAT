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

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompDualDetermine.xaml 的互動邏輯
    /// </summary>
    public partial class CompDualDetermine : UserControl
    {
        
        public DualReaction mConfirmMethod;
        public DualReaction mDenyMethod;

        public static int OUTWIDTH = 440;
        public static int OUTHEIGHT = 250;

        public CompDualDetermine()
        {
            InitializeComponent();
        }

        public void setButtonText(string confirm, string deny)
        {
            textBlock1.Text = confirm;
            textBlock2.Text = deny;
        }

        public void setResult(String result)
        {
            mTextboxResult.Text = result;
        }

        public void setCorrectness(bool correct)
        {
            if (correct)
            {
                mLabelCorrectness.Content = "正确";
                mLabelCorrectness.Foreground = new SolidColorBrush(Color.FromRgb(15, 255, 15));
            }
            else
            {
                mLabelCorrectness.Content = "错误";
                mLabelCorrectness.Foreground = new SolidColorBrush(Color.FromRgb(255, 15, 15));
            }
        }

        public void HideCorrecteness(bool hide)
        {
            //mLabelCorrectness.Content = "";
            if (hide)
                mLabelCorrectness.Visibility = Visibility.Hidden;
            else
                mLabelCorrectness.Visibility = Visibility.Visible;
        }

        public delegate void DualReaction(CompDualDetermine self);

        private void mBdrConfirm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mConfirmMethod(this);
        }

        private void mBdrDeny_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mDenyMethod(this);
        }

        private void textBoxConfirm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}

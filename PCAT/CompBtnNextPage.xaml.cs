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

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompBtnNextPage.xaml 的互動邏輯
    /// </summary>
    public partial class CompBtnNextPage : UserControl
    {
        public delegate void OnAction(object obj);

        public static int mWidht = 123;
        public static int mHeight = 38;

        public OnAction mfOnAction;
        public System.Timers.Timer mTimer = null;

        public CompBtnNextPage(String text, System.Timers.Timer tm = null)
        {
            InitializeComponent();
            amText.Text = text;
            mTimer = tm;
        }

        public void Add2Page(Canvas canvas, int setTop, int setLeftOff=-1)
        {
            canvas.Children.Add(this);

            if (setLeftOff == -1)
            {
                Canvas.SetLeft(this, FEITStandard.PAGE_BEG_X + 
                    (FEITStandard.PAGE_WIDTH - mWidht) / 2);
            }
            else
            {
                Canvas.SetLeft(this, FEITStandard.PAGE_BEG_X + 
                    (FEITStandard.PAGE_WIDTH - mWidht) / 2 + setLeftOff); ;
            }

            Canvas.SetTop(this, setTop);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mTimer != null)
            {
                mTimer.Enabled = false;
            }
            mfOnAction(sender);
        }
    }
}

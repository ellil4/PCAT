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
using System.Timers;

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// BoardTitle.xaml 的互動邏輯
    /// </summary>
    public partial class BoardTitle : UserControl
    {
        public BasePage mBasePage;
        public BoardTitle(BasePage bp)
        {
            InitializeComponent();
            mBasePage = bp;

            if (mBasePage.ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                label1.Content = "对称广度";
                //label1.FontFamily = new FontFamily("KaiTi");
                label2.Content = "Symmetry Span";
                //label2.FontFamily = new FontFamily("KaiTi");
                textBlock1.Text = "    请判断图形的对称性，并记住随后出现的红点位置。\r\n\r\n    下面先来练习一下记红点位置";
            }
        }

        private void label3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mBasePage.ClearAll();
            Timer t = new Timer();
            t.Interval = 1000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        delegate void TimeDele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(mBasePage.ShowBoardAnimal));
        }
    }
}

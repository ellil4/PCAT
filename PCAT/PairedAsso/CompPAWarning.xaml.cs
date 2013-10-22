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
using System.Windows.Threading;

namespace FiveElementsIntTest.PairedAsso
{
    /// <summary>
    /// CompPAWarning.xaml 的互動邏輯
    /// </summary>
    public partial class CompPAWarning : UserControl
    {
        Timer mTm;
        PagePairedAsso mPage;
        public CompPAWarning(PagePairedAsso page)
        {
            InitializeComponent();
            mPage = page;
            mTm = new Timer();
            Flashing();
        }

        public void Flashing()
        {
            mTm.AutoReset = true;
            mTm.Interval = 760;
            mTm.Enabled = true;
            mTm.Elapsed += new ElapsedEventHandler(mTm_Elapsed);
        }

        void switchVisibility()
        {
            if (label1.Visibility == System.Windows.Visibility.Visible)
            {
                label1.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                label1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private delegate void timedele();

        void mTm_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(switchVisibility));
        }
    }
}

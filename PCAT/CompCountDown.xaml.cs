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

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompCountDown.xaml 的互動邏輯
    /// count down GUI component
    /// </summary>
    public partial class CompCountDown : UserControl
    {

        //var to set
        public long Duration = 0;
        public delegate void FuncElapsed();
        public FuncElapsed FunctionElapsed;

        //private 
        private Timer Timer;

        public CompCountDown()
        {
            InitializeComponent();
            FunctionElapsed = doNothing;

            Timer = new Timer();
            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
        }

        public void Start()
        {
            Timer.AutoReset = true;
            Timer.Interval = 1000;
            Timer.Enabled = true;
            amLabel.Content = "剩余时间：" + Duration + "秒";
        }

        private void doNothing()
        { }

        delegate void TimeDele();

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new TimeDele(Refresh));
        }

        public void Refresh()
        {
            Duration--;
            amLabel.Content = "剩余时间：" + Duration + "秒";
            if (Duration < 1)
            {
                Stop();
                Dispatcher.Invoke(new TimeDele(FunctionElapsed));
            }

        }

        public void Stop()
        {
            Timer.AutoReset = false;
            Timer.Enabled = false; 
        }
    }
}

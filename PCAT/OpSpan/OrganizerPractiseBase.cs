using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.OpSpan
{
    public class OrganizerPractiseBase
    {
        public PageOpSpan mPage;
        public delegate void nextDelegate();

        public nextDelegate mfNext;

        public OrganizerPractiseBase(PageOpSpan page)
        {
            mPage = page;
        }

        public delegate void timedele();

        public void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            invokeNext(); 
        }

        public void invokeNext()
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(mfNext)); 
        }

        public void blackScreen(long duration)
        {
            mPage.ClearAll();

            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = duration;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void oneSecBlackScreen(object obj)
        {
            oneSecBlackScreen();
        }

        public void oneSecBlackScreen()
        {
            blackScreen(1000);
        }

        public void t_halfSecBlackScreen(object sender, ElapsedEventArgs e)
        {
             mPage.Dispatcher.Invoke(
                 DispatcherPriority.Normal, new timedele(halfSecBlackScreen));
        }

        public void halfSecBlackScreen()
        {
            blackScreen(500);
        }

        public void quaterSecBlackScreen()
        {
            blackScreen(250);
        }
    }
}

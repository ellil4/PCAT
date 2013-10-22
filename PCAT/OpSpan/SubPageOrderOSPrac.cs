using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.OpSpan
{
    class SubPageOrderOSPrac : SubPageOrderOS
    {
        private OrganizerPractiseChar mOPC;
        PageOpSpan mPage;

        public SubPageOrderOSPrac(PageOpSpan page, OrganizerPractiseChar opc) : base(ref page, null)
        {
            mOPC = opc;
            mPage = page;
        }

        public override void TriBtnConfirm()
        {
            mOPC.mAnswer = mCheckComponent.getAnswer();

            Timer t = new Timer();
            t.Interval = 500;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        private delegate void timeDele();

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(mOPC.halfSecBlackScreen));
        }
    }
}

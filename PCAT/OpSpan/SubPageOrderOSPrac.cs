using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.OpSpan
{
    class SubPageOrderOSPrac : SubPageOrderOS
    {
        private OrganizerPractiseChar mOPC;
        public SubPageOrderOSPrac(PageOpSpan page, OrganizerPractiseChar opc) : base(ref page, null)
        {
            mOPC = opc;
        }

        public override void TriBtnConfirm()
        {
            if(mOPC.mAnswer != null)
                mOPC.mAnswer.Clear();

            mOPC.mAnswer = mCheckComponent.getAnswer();
            mOPC.halfSecBlackScreen();
        }
    }
}

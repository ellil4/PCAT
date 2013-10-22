using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.OpSpan
{
    class CompNumCheckOS : CompNumCheck
    {
        public static int OUTWIDTH = 130;
        public static int OUTHEIGHT = 64;

        public CompNumCheckOS(String txt, UIGroupNumChecks parent, int id) : base(txt, parent, id)
        {
            //nothing need to be done
            amBorder.BorderThickness = new System.Windows.Thickness(2);
        }
    }
}

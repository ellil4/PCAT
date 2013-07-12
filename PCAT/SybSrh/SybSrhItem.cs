using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FiveElementsIntTest.SybSrh
{
    public class SybSrhItem
    {
        //public Bitmap[] Targets;
        //public Bitmap[] Selections;
        //public ItemsInfo Info;
        public SybSrhVisualElem[] Target;//2
        public SybSrhVisualElem[] Selection;//5

        public SybSrhItem()
        {
            Target = new SybSrhVisualElem[2];
            Selection = new SybSrhVisualElem[5];
            //Targets = new Bitmap[2];
            //Selections = new Bitmap[5];
            //Info = new ItemsInfo();
        }

        public int GetTrueTarIdx()
        {
            int retval = -1;
            for (int i = 0; i < Target.Length; i++)
            {
                if (Target[i] != null && Target[i].IfTrue)
                {
                    retval = i;
                    break;
                }
            }

            return retval;
        }

        public int GetTrueSelectionIdx()
        {
            int retval = -1;
            for (int i = 0; i < Selection.Length; i++)
            {
                if (Selection[i] != null && Selection[i].IfTrue)
                {
                    retval = i;
                    break;
                }
            }
            return retval;
        }
    }
}

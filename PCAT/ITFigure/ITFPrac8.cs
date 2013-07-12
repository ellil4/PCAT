using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.ITFigure
{
    class ITFPrac8 : ITFProc
    {
        public bool mLastLeft;
        public ITFPrac8(PageITFigure page)
            : base(page)
        {
            //mNowTill = -1;
            //mItems = GenItems();
        }

        public override void NextItem()
        {
            if (RandomBool())
            {
                mPage.ShowLeft(400);
                mLastLeft = true;
            }
            else
            {
                mPage.ShowRight(400);
                mLastLeft = false;
            }
        }

        /*public override List<ITFigureItem> GenItems()
        {
            List<ITFigureItem> retval = new List<ITFigureItem>();

            return retval;
        }*/
    }
}

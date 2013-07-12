using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.ITFigure
{
    public class ITFProc
    {
        public PageITFigure mPage;
        public List<ITFigureItem> mItems;
        public int mNowTill = 0;
        Random mRdm = new Random();

        public ITFProc(PageITFigure page)
        {
            mPage = page;
            //mItems = new List<ITFigureItem>();
        }

        public virtual void NextItem() 
        { }

        public virtual List<ITFigureItem> GenItems() 
        {
            return null;
        }

        public bool RandomBool()
        {
            if (mRdm.Next(0, 2) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

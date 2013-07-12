using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.ITFigure
{
    public class ITFFreePrac : ITFProc
    {
        

        public ITFFreePrac(PageITFigure page) : base(page)
        {
            mNowTill = -1;
            mItems = GenItems();
        }

        public override void NextItem()
        {
            mNowTill++;
            if (mNowTill < mItems.Count)
            {
                if (mItems[mNowTill].ItemDirection)
                {
                    mPage.ShowLeft(mItems[mNowTill].ShowingTimeSpan);
                }
                else
                {
                    mPage.ShowRight(mItems[mNowTill].ShowingTimeSpan);
                }
            }
            else
            {
                mPage.nextStage();
            }
        }

        public override List<ITFigureItem> GenItems()
        {
            List<ITFigureItem> retval = new List<ITFigureItem>();

            for (int i = 0; i < 10; i++)
            {
                ITFigureItem item = new ITFigureItem();

                item.ItemDirection = RandomBool();
                item.ShowingTimeSpan = 500;

                retval.Add(item);
            }

            return retval;
        }
    }
}

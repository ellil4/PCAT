using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections;
using PCATData;

namespace FiveElementsIntTest
{
    public class LayoutDigitSymbol
    {
        protected PageDigitSymbol mPage;
        public static int ELEMENT_PER_PAGE = 30;

        public LayoutDigitSymbol(PageDigitSymbol _page)
        {
            mPage = _page;
        }

        private void addAt(int x, int y,
            ref UserControl component)
        {
            mPage.mBaseCanvas.Children.Add(component);
            Canvas.SetLeft(component, x);
            Canvas.SetTop(component, y);
        }

        public void addLegend(int yOff, int xOff)
        {
            for (int i = 1; i <= 9; i++)
            {
                CompDigiSymbol inst = new CompDigiSymbol(i.ToString()[0],
                    FEITStandard.SYMBOL_LEFT[i], FEITStandard.SYMBOL_RIGHT[i], mPage);

                UserControl inst_uc = (UserControl)inst;

                //align with the center
                addAt((FEITStandard.PAGE_BEG_X + ((FEITStandard.PAGE_WIDTH - (CompDigiSymbol.OUTWIDTH * 9)) / 2)) +
                    CompDigiSymbol.OUTWIDTH * (i - 1) + xOff,
                      FEITStandard.PAGE_BEG_Y + yOff, ref inst_uc);
            }
        }

        public void addTestField(int num, int cellX, int CellY, int yOff, int lineGap)
        {
            CompDigiSymbol symbol = new CompDigiSymbol(num.ToString()[0], 
                SYMBOL_TYPE.NONE, SYMBOL_TYPE.NONE, mPage);

            UserControl inst_uc = (UserControl)symbol;
            mPage.mSymbols.Add(symbol);
            addAt(FEITStandard.PAGE_BEG_X + cellX * CompDigiSymbol.OUTWIDTH,
                FEITStandard.PAGE_BEG_Y + CellY * (CompDigiSymbol.OUTHEIGHT + lineGap) + yOff,
                ref inst_uc);
        }

        //30 per page
        public void FillPage(ArrayList numbers, int beginPos, int yOff)
        {
            int atX = 0;
            int atY = 0;
            for (int i = beginPos; i < beginPos + ELEMENT_PER_PAGE; i++)
            {
                if(atX == 10)
                {
                    atX = 0;
                    atY++;
                }

                addTestField((int)numbers[i], atX, atY, yOff, 20);

                atX++;
            }
        }
    }
}

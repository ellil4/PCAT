using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PCATData;

namespace FiveElementsIntTest
{
    public class EvalueDigitSymbol
    {
        private PageDigitSymbol mPage;
        public List<bool> mTF;
        public List<SYMBOL_TYPE> mLeftInput;
        public List<SYMBOL_TYPE> mLeftAnswer;
        public List<SYMBOL_TYPE> mRightInput;
        public List<SYMBOL_TYPE> mRightAnswer;

        public EvalueDigitSymbol(PageDigitSymbol _page)
        {
            mPage = _page;
            mTF = new List<bool>();
            mLeftInput = new List<SYMBOL_TYPE>();
            mLeftAnswer = new List<SYMBOL_TYPE>();
            mRightInput = new List<SYMBOL_TYPE>();
            mRightAnswer = new List<SYMBOL_TYPE>();
        }

        public void markThis()
        {
            SYMBOL_TYPE left = ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).GetLeft();
            SYMBOL_TYPE right = ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).GetRight();
            int num = ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).GetNumber();

            mLeftInput.Add(left);
            mLeftAnswer.Add(FEITStandard.SYMBOL_LEFT[num]);
            mRightInput.Add(right);
            mRightAnswer.Add(FEITStandard.SYMBOL_RIGHT[num]);

            if (left == FEITStandard.SYMBOL_LEFT[num] && right == FEITStandard.SYMBOL_RIGHT[num])
            {
                mTF.Add(true);
            }
            else 
            {
                mTF.Add(false);
            }
        }

        public int CorrectCount()
        {
            int retval = 0;

            for (int i = 0; i < mTF.Count; i++)
            {
                if ((bool)mTF[i])
                    retval++;
            }

            return retval;
        }
    }
}

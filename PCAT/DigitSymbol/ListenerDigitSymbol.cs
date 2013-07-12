using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Timers;
using PCATData;

namespace FiveElementsIntTest
{
    public class ListenerDigitSymbol
    {
        private PageDigitSymbol mPage;
        public EvalueDigitSymbol mEvalue;

        public ListenerDigitSymbol(PageDigitSymbol _page)
        {
            mPage = _page;
            mEvalue = new EvalueDigitSymbol(_page);
        }

        private void fPressed()
        {
            ((CompDigiSymbol)mPage.
                mSymbols[mPage.mElemFocus]).
                fillAnswer(SYMBOL_TYPE.X, mPage.mElemFocusLeft);
        }

        private void jPressed()
        {
            ((CompDigiSymbol)mPage.
                mSymbols[mPage.mElemFocus]).
                fillAnswer(SYMBOL_TYPE.O, mPage.mElemFocusLeft);
        }

        private void spacePressed()
        {
            ((CompDigiSymbol)mPage.
                mSymbols[mPage.mElemFocus]).
                fillAnswer(SYMBOL_TYPE.BAR, mPage.mElemFocusLeft);
        }

        public void pressed(KeyEventArgs key)
        {
            if (mPage.mElemFocus < mPage.mSymbols.Count)
            {
                //keeps track of time only when it is not an exercise
                if (!mPage.mbExercise)
                {
                    if (mPage.mElemFocusLeft)
                    {
                        mPage.mTimer.Dot("Left");
                    }
                    else
                    {
                        mPage.mTimer.Dot("Right");
                    }
                }


                bool valid = true;

                if (key.Key == Key.F)
                {
                    fPressed();
                }
                else if (key.Key == Key.J)
                {
                    jPressed();
                }
                else if (key.Key == Key.Space)
                {
                    spacePressed();
                }
                else
                {
                    valid = false;
                }

                if (valid)
                {
                    //focus on the right side
                    //evalue, save, iteration
                    if (mPage.mElemFocusLeft == false)
                    {
                        //keep record and evalue only when it is not an exercise
                        if (!mPage.mbExercise)
                            mEvalue.markThis();

                        if (mPage.mElemFocus < mPage.mSymbols.Count - 1)
                            ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus + 1]).SetCursor(CURSOR_STATUS.LEFT);


                        ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).SetCursor(CURSOR_STATUS.NONE);
                        

                        mPage.mElemFocus++;
                    }
                    else//focus on the left
                    {
                        ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).SetCursor(CURSOR_STATUS.RIGHT);
                    }

                    mPage.mElemFocusLeft = !mPage.mElemFocusLeft;
                }
            }
            else//turn the page
            {
                mPage.mElemFocus = 0;
                mPage.nextStep();
            }
        }

        /*public void Reset()
        {
            mEvalue.mResult.Clear();
        }*/
    }
}

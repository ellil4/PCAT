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

        private void fPressed() //bool validation
        {
            ((CompDigiSymbol)mPage.
                mSymbols[mPage.mElemFocus]).
                fillAnswer(SYMBOL_TYPE.X, mPage.mElemFocusLeft);
        }

        private void jPressed()//bool validation
        {
            ((CompDigiSymbol)mPage.
                mSymbols[mPage.mElemFocus]).
                fillAnswer(SYMBOL_TYPE.O, mPage.mElemFocusLeft);
        }

        private void spacePressed() //bool validation
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
                    if (!ContrastValue() && mPage._ControlExercise) 
                    {
                        ((CompDigiSymbol)mPage.
                             mSymbols[mPage.mElemFocus]).
                                 fillAnswer(SYMBOL_TYPE.NONE, mPage.mElemFocusLeft);
                        valid = false;
                    }
                    else
                    {
                        valid = true;
                    }
                    
                }
                else if (key.Key == Key.J)
                {
                    jPressed();
                    if (!ContrastValue() && mPage._ControlExercise)
                    {
                        ((CompDigiSymbol)mPage.
                            mSymbols[mPage.mElemFocus]).
                                fillAnswer(SYMBOL_TYPE.NONE, mPage.mElemFocusLeft);
                        valid = false;
                    }
                    else
                    {
                        valid = true;
                    }
                    
                }
                else if (key.Key == Key.Space && !key.IsRepeat)
                {
                   
                    spacePressed();
                    if (!ContrastValue() && mPage._ControlExercise)
                    {
                        ((CompDigiSymbol)mPage.
                            mSymbols[mPage.mElemFocus]).
                                fillAnswer(SYMBOL_TYPE.NONE, mPage.mElemFocusLeft);
                        valid = false;
                    }
                    else
                    {
                        valid = true;
                    }
                    
                }
                else
                {
                    valid = false;
                }

                //if (!valid && mPage.mbExercise)
                //{
 

                //}

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

                        if (mPage.mElemFocus == mPage.mSymbols.Count)
                        {
                            Timer tx = new Timer();
                            tx.AutoReset = false;
                            tx.Interval = mPage._second_chage;
                            tx.Elapsed += new ElapsedEventHandler(tx_Elapsed);
                            tx.Enabled = true;
                        }
                    }
                    else//focus on the left
                    {
                        ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).SetCursor(CURSOR_STATUS.RIGHT);
                    }

                    mPage.mElemFocusLeft = !mPage.mElemFocusLeft;
                }

            }


        }

        delegate void TurnPageDele();

        void tx_Elapsed(object sender, ElapsedEventArgs e)
        {
            //turn the page                
            mPage.mElemFocus = 0;
            mPage.Dispatcher.Invoke(new TurnPageDele(mPage.nextStep));
        }
        
        //练习答案对比
        public bool ContrastValue()
        {
            bool IsCorrect= true;//练习答案参数

            int item_num = ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).GetNumber();

            SYMBOL_TYPE left = ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).GetLeft();

            SYMBOL_TYPE right = ((CompDigiSymbol)mPage.mSymbols[mPage.mElemFocus]).GetRight();

            if (left != SYMBOL_TYPE.NONE && left != FEITStandard.SYMBOL_LEFT[item_num])
            {
                IsCorrect = false;
            }

            if (right != SYMBOL_TYPE.NONE && right != FEITStandard.SYMBOL_RIGHT[item_num])
            {
                IsCorrect = false;
            }
            return IsCorrect;
        }

        /*public void Reset()
        {
            mEvalue.mResult.Clear();
        }*/
    }
}

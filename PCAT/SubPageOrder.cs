using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FiveElementsIntTest
{
    public abstract class SubPageOrder
    {
        public CompTriBtns mTriBtns;
        public UIGroupNumChecks mCheckComponent;

        public SubPageOrder()
        {
            mTriBtns = new CompTriBtns();
            mTriBtns.mConfirmMethod = TriBtnConfirm;
            mTriBtns.mClearMethod = TriBtnClear;
            mTriBtns.mBlankMethod = TriBtnBlank;
        }

        public void TriBtnClear()
        {
            mCheckComponent.backErase();
            //Console.WriteLine("Clear Clickd");
        }

        public void TriBtnBlank()
        {
            mCheckComponent.jumpOver();
            //Console.WriteLine("Blank Clickd");
        }

        public abstract void TriBtnConfirm();

        public abstract void PutNumCheckToScreen(int xOff, int yOff,
            int xCount, int yCount, int width, int height);

        public abstract void PutTriBtnToScreen(int xOff, int yOff);

        public abstract void Show();
    }
}

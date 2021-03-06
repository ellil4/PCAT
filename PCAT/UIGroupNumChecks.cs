﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FiveElementsIntTest
{
    public class UIGroupNumChecks
    {
        public List<CompNumCheck> mCheckComps;
        public List<int> mOrder;

        protected int mNowTill = 1;

        public int mElemCount = 0;
        public int mClickLimit = 0;
        public int mJumpLimit = 0;
        public bool mTouchActivated = true;

        public UIGroupNumChecks()
        {
            mCheckComps = new List<CompNumCheck>();
            mOrder = new List<int>();
        }

        /*public void SetTouchFree()
        {
            for (int i = 0; i < mCheckComps.Count; i++)
            {
                mCheckComps[i].IsHitTestVisible = false;
            }
        }

        public void SetTouchable()
        {
            for (int i = 0; i < mCheckComps.Count; i++)
            {
                mCheckComps[i].IsHitTestVisible = true;
            }
        }*/

        public virtual void onAction(int actionCompIndex)
        {
            if (mNowTill < mClickLimit/*10*/ && mTouchActivated)
            {
                CompNumCheck component = (CompNumCheck)mCheckComps[actionCompIndex];
                if (!component.clicked())
                {
                    component.setOderDigit(mNowTill);
                    mOrder.Add(actionCompIndex);
                    mNowTill++;
                    component.setClicked();
                }
            }
        }

        public virtual void reset()
        {
            mNowTill = 1;
            for (int i = 0; i < mElemCount; i++)
            {
                ((CompNumCheck)mCheckComps[i]).amDigiLabel.Content = "";
                mCheckComps[i].setUnClicked();
                mOrder.Clear();
            }
        }

        public virtual int backErase()
        {
            int retval = -1;
            if (mNowTill > 1)
            {
                for (int i = 0; i < mElemCount; i++)
                {
                    CompNumCheck component = mCheckComps[i];

                    if (component.amDigiLabel.Content.Equals((mNowTill - 1).ToString()))
                    {
                        component.amDigiLabel.Content = "";
                        component.setUnClicked();
                        mOrder.RemoveAt(mOrder.Count - 1);
                        retval = i;
                        break;
                    }

                }

                mNowTill--;

                while (mNowTill > 2 && mOrder[mNowTill - 2] == -1)
                {
                    mOrder.RemoveAt(mNowTill - 2);
                    mNowTill--;
                }
            }

            return retval;
        }

        public void jumpOver()
        {
            if (mNowTill < mJumpLimit/*9*/)
            {
                mOrder.Add(-1);
                mNowTill++;
            }
        }

        public List<int> getAnswer()
        {
            return mOrder;
        }

    }
}

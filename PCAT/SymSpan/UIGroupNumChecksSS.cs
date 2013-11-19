using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FiveElementsIntTest.SymSpan
{
    public class UIGroupNumChecksSS : UIGroupNumChecks
    {   
        public UIGroupNumChecksSS()
        {
            mElemCount = 16;
            mJumpLimit = 9;
            mClickLimit = 10;

            for (int i = 0; i < mElemCount; i++)
            {
                CompNumCheck comp = new CompNumCheckSS("", this, i);
                mCheckComps.Add(comp);
            }
        }

        public void setMarked(int index)
        {
            int count = mCheckComps.Count;
            for (int i = 0; i < count; i++)
            {
                if (i == index)
                {
                    mCheckComps[i].amEllipse.Visibility = System.Windows.Visibility.Visible;
                    /*mCheckComps[i].amDigiLabel.Background =
                        new SolidColorBrush(Color.FromRgb(255, 255, 255));*/
                }
                else 
                {
                    mCheckComps[i].amEllipse.Visibility = System.Windows.Visibility.Hidden;
                    /*mCheckComps[i].amDigiLabel.Background =
                        new SolidColorBrush(Color.FromRgb(0, 0, 0));*/
                }
            }
        }

        public void setPositionMode(bool flag)
        {
            if (flag)
            {
                for (int i = 0; i < mCheckComps.Count; i++)
                {
                    mCheckComps[i].amDigiLabel.Margin = 
                        new System.Windows.Thickness(-9, -9, 0, 0);
                }
            }
            else
            {
                for (int j = 0; j < mCheckComps.Count; j++)
                {
                    mCheckComps[j].amDigiLabel.Margin = 
                        CompNumCheckSS.DEF_MARGIN_THICKNESS;
                }
            }
        }

        public override void onAction(int actionCompIndex)
        {
            base.onAction(actionCompIndex);
            if(mTouchActivated)
                mCheckComps[actionCompIndex].amEllipse.Visibility = System.Windows.Visibility.Visible;
        }

        public override void reset()
        {
            base.reset();
            for (int i = 0; i < mElemCount; i++)
            {
                mCheckComps[i].amEllipse.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public override int backErase()
        {
            int retval = -1;
            retval = base.backErase();
            mCheckComps[retval].amEllipse.Visibility = System.Windows.Visibility.Hidden;
            return retval;
        }
    }
}

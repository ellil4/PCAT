using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FiveElementsIntTest.OpSpan
{

    public class UIGroupNumChecksOS : UIGroupNumChecks
    {
        public static String[] LUNAR_ANI = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪"};

        public UIGroupNumChecksOS(/*OrganizerTrail _org*/)
        {
            mElemCount = 12;
            mJumpLimit = 9;
            mClickLimit = 10;

            for (int i = 0; i < mElemCount; i++)
            {
                CompNumCheck comp = new CompNumCheck(LUNAR_ANI[i], this, i);
                mCheckComps.Add(comp);
            }
        }

        public static int GetIndexByChar(string chara)
        {
            int retval = -1;
            for(int i = 0; i < LUNAR_ANI.Length; i++)
            {
                if (chara == LUNAR_ANI[i])
                    retval = i;
            }

            return retval;
        }

    }
}

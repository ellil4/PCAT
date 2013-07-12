using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.SymSpan
{
    class RandomSelector
    {
        List<TrailSS_ST> mSource;
        List<TrailSS_ST> mWorking;
        Random mRandom;
        int[] mGroupArr;

        public RandomSelector(List<TrailSS_ST> pics, int[] groupArr)
        {
            mSource = pics;
            mGroupArr = groupArr;
            mWorking = new List<TrailSS_ST>();
            mRandom = new Random();

            reArrange();
        }

        public List<TrailsGroupSS> Get()
        {
            List<TrailsGroupSS> retval = new List<TrailsGroupSS>();

            //group
            for (int i = 0; i < mGroupArr.Length; i++)
            {

                //trails in group
                TrailsGroupSS group = new TrailsGroupSS();

                //gen random positions
                int[] pos = genRandom1to16Position(mGroupArr[i]);
                
                
                for (int j = 0; j < mGroupArr[i]; j++)
                {
                    int num = mRandom.Next(0, mWorking.Count);
                    //System.Console.Write(num + ",");

                    group.Trails.Add(new TrailSS_ST(mWorking[num].FileName,
                        mWorking[num].IsSymm, pos[j]));
                    mWorking.RemoveAt(num);

                    if (mWorking.Count == 0)
                        reArrange();
                }

                retval.Add(group);
            }
            return retval;
        }

        private int[] genRandom1to16Position(int len)
        {
            //initiation
            int[] ret = new int[len];
            for (int i = 0; i < len; i++)
            {
                ret[i] = -1;
            }
            //no repeat
            for (int j = 0; j < len; j++)
            {
                ret[j] = mRandom.Next(0, 16);

                bool rep = true;
                while (rep)
                {
                    for (int k = 0; k < len; k++)
                    {
                        if (k != j && ret[k] == ret[j])
                        {
                            ret[j] = mRandom.Next(0, 16);
                            break;
                        }

                        if (k == len - 1)
                            rep = false;
                    }
                }

                System.Console.Write(ret[j] + ",");
            }

            return ret;
        }

        private void reArrange()
        {
            //deep copy
            mWorking.Clear();
            for (int i = 0; i < mSource.Count; i++)
            {
                mWorking.Add(new TrailSS_ST(mSource[i]));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.SybSrh
{
    public class ItemsInfo
    {
        public int[] TargetsTypes;//default value = -1
        public int[] TargetsPicIndex;//default value = -1

        public int TrueTargetAt = -1;//A:0, B:1, None:-1
        public int TrueSelectionAt = -1;//0-5, None:-1

        public int[] SelectionTypes;//default value = -1
        public int[] SelectionPicIndex;//default value = -1

        public ItemsInfo()
        {
            SelectionTypes = new int[6];
            TargetsTypes = new int[2];

            for (int i = 0; i < 6; i++)
            {
                SelectionTypes[i] = -1;
                SelectionPicIndex[i] = -1;
            }

            for (int j = 0; j < 2; j++)
            {
                TargetsTypes[j] = -1;
                TargetsPicIndex[j] = -1;
            }
        }
    }
}

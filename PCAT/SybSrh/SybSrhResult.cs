using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.SybSrh
{
    public class SybSrhResult
    {
        public SybSrhItem Item;
        public bool Answer;
        public long RT;
        public bool Correctness;

        public SybSrhResult(bool ans, long rt, SybSrhItem item)
        {
            Answer = ans;
            RT = rt;
            
            for (int i = 0; i < item.Target.Length; i++)
                item.Target[i].BMP = null;

            for (int i = 0; i < item.Selection.Length; i++)
                item.Selection[i].BMP = null;

            Item = item;

            if (ans == SybSrhItemGenerator.hasTrueTarget(item))
                Correctness = true;
            else
                Correctness = false;
        }
    }
}

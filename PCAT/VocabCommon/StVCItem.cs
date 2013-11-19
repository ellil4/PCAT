using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.VocabCommon
{
    public class StVCItem
    {
        public String Casual;
        public List<String> Selections;
        public List<int> Weights;

        public StVCItem()
        {
            Selections = new List<String>();
            Weights = new List<int>();
        }
    }
}

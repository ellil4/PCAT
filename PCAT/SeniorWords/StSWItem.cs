using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.SeniorWords
{
    public class StSWItem
    {
        public String Casual;
        public List<String> Selections;
        public List<int> Weights;

        public StSWItem()
        {
            Selections = new List<String>();
            Weights = new List<int>();
        }
    }
}

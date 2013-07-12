using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.SymSpan
{
    //one subgroup
    class AnswerSSST
    {
        public List<bool> TF;//true or false serial
        public List<bool> TFCorrectness;

        public List<int> Order;//by numbers of cells`
        public List<bool> OrderCorrectness;

        public AnswerSSST()
        {
            TF = new List<bool>();
            TFCorrectness = new List<bool>();
            Order = new List<int>();
            OrderCorrectness = new List<bool>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    public enum SYMBOL_TYPE
    {
        O, X, BAR, NONE
    };

    public class QRecDigiSymbol
    {
        public int Stim;
        public SYMBOL_TYPE Left;
        public long RTLeft;
        public SYMBOL_TYPE Right;
        public long RTRight;
        public bool Correctness;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    public class QRecSymbolSearch
    {
        //public List<String> Stim;//10
        public int[] TarType;
        public int[] TarIdx;
        public int[] SelType;
        public int[] SelIdx;
        public int TrueTarAt = -1;
        public int TrueSelAt = -1;

        public bool Answer;
        public long RT;
        public bool Correctness;

        public QRecSymbolSearch(bool ans, long rt)
        {
            Answer = ans;
            RT = rt;

            TarType = new int[2];
            TarIdx = new int[2];
            SelType = new int[2];
            SelIdx = new int[2];
        }

        public QRecSymbolSearch()
        {
            TarType = new int[2];
            TarIdx = new int[2];
            SelType = new int[2];
            SelIdx = new int[2];
        }
    }
}

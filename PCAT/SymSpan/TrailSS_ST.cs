using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.SymSpan
{
    public class TrailSS_ST
    {
        public string FileName;
        public bool IsSymm;
        public int Position;

        public TrailSS_ST(TrailSS_ST src)
        {
            FileName = src.FileName;
            IsSymm = src.IsSymm;
            Position = src.Position;
        }

        public TrailSS_ST(string file, bool correct, int pos)
        {
            FileName = file;
            IsSymm = correct;
            Position = pos;
        }

        public TrailSS_ST()
        {
 
        }
    }
}

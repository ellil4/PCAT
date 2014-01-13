using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QAIF
{
    public class RWBase
    {
        public long mIndexBeg = -1;

        public long GetIndexPosition(long index)
        {
            return index * QAIF.STRIDE + mIndexBeg;
        }
    }
}

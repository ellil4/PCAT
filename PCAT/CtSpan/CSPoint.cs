﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.CtSpan
{
    public class CSPoint
    {
        public short x = 0;
        public short y = 0;
        public int type;

        public CSPoint(short _x, short _y)
        {
            x = _x;
            y = _y;
            type = -1;
        }

        public CSPoint()
        {
 
        }
    }
}

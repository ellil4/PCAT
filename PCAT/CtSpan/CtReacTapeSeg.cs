using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.CtSpan
{
    public class CtReacTapeSeg
    {
        public int X, Y;
        public TokenType Type;
        public long TimeSpot;

        public CtReacTapeSeg(int x, int y, TokenType type, long spotOfTime)
        {
            X = x;
            Y = y;
            Type = type;
            spotOfTime = TimeSpot;
        }
    }
}

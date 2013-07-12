using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    class StMessage
    {
        public int HeaderLen;
        public int MessageLength;
        public int MessageStartPos;

        public StMessage()
        {
            MessageLength = -1;
            MessageStartPos = -1;
        }

        public bool IsValid()
        {
            if (MessageLength != -1 && MessageStartPos != -1)
                return true;
            else
                return false;
        }
    }
}

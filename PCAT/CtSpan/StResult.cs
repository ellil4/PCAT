using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.CtSpan
{
    public class StResult
    {
        public int SpanWidth = -1;
        public int GroupNum = -1;
        public int GroupSeq = -1;
        public String ClickOnType = "";
        public String ClickOnPosition = "";
        public long GraphRT = -1;

        public bool PractiseMode = false;

        public bool CountingCorrectness = false;

        //of serial
        public String StdSerial = "";
        public String UserSerial = "";
        public int UserSerialCorrectCount = 0;
        public String UserSerialCorrectMask = "";
        public bool Correctness = false;//corectness of Serial
        public long RT = -1;//RT of Serial
    }
}

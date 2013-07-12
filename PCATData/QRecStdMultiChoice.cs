using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    public class QRecStdMultiChoice
    {
        public String Target;
        public List<String> SS;
        public List<String> SWeight;
        public long RT;
        public String CorrectAnswer;
        public String UserAnswer;

        public QRecStdMultiChoice()
        {
            SS = new List<string>();
            SWeight = new List<string>();
        }
    }
}

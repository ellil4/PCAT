using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    //for one sub-group
    public class QRecSymSpan
    {
        //repeat part
        public List<String> SymStim;
        public List<bool> IsSym;
        
        public List<long> SymExposureTime;
        public List<bool> SymCorrectness;
        public List<long> SymRT;

        //position part
        public List<int> PosStim;
        public List<int> UserPos;
        public long PosRT;
        public bool Correctness;


        //share part
        public int GroupID;
        public int SubGroupID;

        public QRecSymSpan()
        { }
    }
}

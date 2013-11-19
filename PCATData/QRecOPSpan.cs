using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    //DB module only
    public class QRecOPSpan
    {
        public String UserOrder;
        public String StimOrder;
        public long OrderRT;
        public bool OrderCorrectness;

        public List<String> AnimalStim;
        public List<String> ExpressionStim;
        public List<String> AnswerStim;
        public List<string> Confirm;
        public List<long> ConfirmRT;
        public List<bool> ConfirmCorrectness;
        public List<long> ExposureTime;

        public int GroupID;
        public int SubGroupID;
        

        public QRecOPSpan()
        {
            AnimalStim = new List<string>();
            ExpressionStim = new List<String>();
            AnswerStim = new List<string>();
            Confirm = new List<string>();
            ConfirmRT = new List<long>();
            ConfirmCorrectness = new List<bool>();
            ExposureTime = new List<long>();
        }
    }
}

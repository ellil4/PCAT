using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PCATData;

namespace FiveElementsIntTest.OpSpan
{
    public class RecorderOpSpan : RecorderBase
    {
        //chart 1
        public List<string> mathExpression;
        public List<long> mathOn;
        public List<long> mathOff;

        public List<int> displayedAnswer;
        public List<long> choiceShowTime;
        public List<long> choiceMadeTime;
        public List<bool> choice;
        public List<bool> correctness;


        public List<string> animal;

        //chart 2
        public List<long> orderOn;
        public List<long> orderOff;
        public List<string> rightOrder;
        public List<string> userInputOrder;

        private static string[] HEADER_INFO_COLLECTION = {"exp", "exp on time", "exp off time",  
                                                      "displayed answer", "choice", "correctness",
                                                    "choice Show time", "choice made time", "animal", "group ID"};

        private static string[] HEADER_ORDER = {"order on", "order off", "right order", "user input order"};

        private static string[] HEADER_PRACMATH = { "Equation", "ShownResult", "RT", "TrueAnswer", "UserAnswer" };
        private static string[] HEADER_PRACORDER = 
            {"RT", "RealOrder", "UserAnswer", "Corectness"};

        //chart 3 practise math
        public List<StEquation> mMathPracEquations;
        public List<long> mMathPracRTs;
        public List<bool> mMathPracAnswers;
        //chart 4 practise order
        public List<List<int>> mPracOrderRealOrder;
        public List<List<int>> mPracOrderAnswers;
        public List<bool> mPracOrderCorrectness;
        public List<long> mPracOrderRTs;

        public PageOpSpan mPage;

        public RecorderOpSpan(PageOpSpan page)
        {
            mPage = page;

            mathExpression = new List<string>();
            mathOn = new List<long>();
            mathOff = new List<long>();
            displayedAnswer = new List<int>();
            choice = new List<bool>();
            correctness = new List<bool>();
            choiceShowTime = new List<long>();
            choiceMadeTime = new List<long>();
            animal = new List<string>();
            //groupID = new List<int>();
            //subgroupID = new List<int>();

            orderOn = new List<long>();
            orderOff = new List<long>();
            rightOrder = new List<string>();
            userInputOrder = new List<string>();
        }

        public void outputReport(string filepathInfoCollection, string filepathOrder, 
            string filepathPracMath, string filepathPracOrder)
        {
            //output info collection
            abstractOut(makeHeaderInfoCollection, fillInfoCollection, filepathInfoCollection);
            //output order collection
            abstractOut(makeHeaderOrder, fillOrder, filepathOrder);
            //output prac math info
            abstractOut(makeHeaderPracMath, writePracMathContent, filepathPracMath);
            //output prac order info
            abstractOut(makeHeaderPracOrder, writePracOrderContent, filepathPracOrder);
            //PCATDataSaveReport();
        }

        public void PCATDataSaveReport()
        {
            List<QRecOPSpan> rec = new List<QRecOPSpan>();
            int thisCursor1 = 0;

            for (int i = 0; i < mPage.mGroupArrangement.Length; i++)
            {
                QRecOPSpan subGroupRec = new QRecOPSpan();
                for (int j = 0; j < mPage.mGroupArrangement[i]; j++)
                {
                    subGroupRec.AnimalStim.Add(animal[thisCursor1]);
                    subGroupRec.ExpressionStim.Add(mathExpression[thisCursor1]);
                    subGroupRec.AnswerStim.Add(displayedAnswer[thisCursor1].ToString());
                    subGroupRec.Confirm.Add(choice[thisCursor1]);
                    subGroupRec.ConfirmRT.Add(choiceMadeTime[thisCursor1] - choiceShowTime[thisCursor1]);
                    subGroupRec.ConfirmCorrectness.Add(correctness[thisCursor1]);
                    subGroupRec.ExposureTime.Add(mathOff[thisCursor1] - mathOn[thisCursor1]);

                    thisCursor1++;
                }

                subGroupRec.UserOrder = userInputOrder[i];
                subGroupRec.StimOrder = rightOrder[i];
                subGroupRec.OrderRT = orderOff[i] - orderOn[i];
                subGroupRec.OrderCorrectness = subGroupRec.UserOrder.Equals(subGroupRec.StimOrder);
                subGroupRec.GroupID = mPage.mGroupArrangement[i];
                subGroupRec.SubGroupID = PageOpSpan.getSubGroupID(i);

                rec.Add(subGroupRec);
            }

            //DB work
            mPage.mMainWindow.mDB.AddOpSpanExpressionRecord(rec, mPage.mMainWindow.mUserID);
            mPage.mMainWindow.mDB.AddOPSpanOrderRecord(rec, mPage.mMainWindow.mUserID);
        }

        private void makeHeaderPracMath(ref StreamWriter sw)
        {
            string header = "";
            for (int i = 0; i < HEADER_PRACMATH.Length; i++)
            {
                header += HEADER_PRACMATH[i] + "\t";
            }

            sw.WriteLine(header);
        }

        private void writePracMathContent(ref StreamWriter sw)
        {
            int len = 0;
            if (mMathPracEquations != null)
            {
                len = mMathPracEquations.Count;
            }

            string content = "";
            for (int i = 0; i < len; i++)
            {
                content = "";
                content += mMathPracEquations[i].Equation + "\t";
                content += mMathPracEquations[i].Result + "\t";
                content += mMathPracRTs[i] + "\t";
                content += mMathPracEquations[i].Answer + "\t";
                content += mMathPracAnswers[i] + "\t";
                sw.WriteLine(content);
            }
        }

        private void makeHeaderPracOrder(ref StreamWriter sw)
        {
            string header = "";
            for (int i = 0; i < HEADER_PRACORDER.Length; i++)
            {
                header += HEADER_PRACORDER[i] + "\t";
            }
            sw.WriteLine(header);
        }

        //"RealOrder", "UserAnswer", "Corectness"
        private void writePracOrderContent(ref StreamWriter sw)
        {
            int len = 0;
            if (mPracOrderRealOrder != null)
            {
                len = mPracOrderRealOrder.Count;
            }
            string content = "";
            for (int i = 0; i < len; i++)
            {
                content = "";
                content += mPracOrderRTs[i] + "\t";

                for(int j = 0; j < mPracOrderRealOrder[i].Count; j++)
                {
                    content += mPracOrderRealOrder[i][j].ToString() + ",";
                }

                content += "\t";

                for (int k = 0; k < mPracOrderAnswers[i].Count; k++)
                {
                    content += mPracOrderAnswers[i][k].ToString() + ",";
                }

                content += "\t";

                content += mPracOrderCorrectness[i] + "\t";

                sw.WriteLine(content);
            }
        }

        private void makeHeaderInfoCollection(ref StreamWriter sw)
        {
            string header = "";

            for (int i = 0; i < HEADER_INFO_COLLECTION.Length; i++)
            {
                header += HEADER_INFO_COLLECTION[i] + "\t";
            }
                
            sw.WriteLine(header);
        }

        private void fillInfoCollection(ref StreamWriter sw)
        {
            int len = mathExpression.Count;
            string content = "";

            for (int i = 0; i < len; i++)
            {
                content = "";
                content += mathExpression[i] + "\t";
                content += mathOn[i] + "\t";
                content += mathOff[i] + "\t";
                content += displayedAnswer[i] + "\t";
                content += choice[i] + "\t";
                content += correctness[i] + "\t";
                content += choiceShowTime[i] + "\t";
                content += choiceMadeTime[i] + "\t";
                content += animal[i] + "\t";
                //content += groupID[i] + ",";
                sw.WriteLine(content);
            }
        }

        private void makeHeaderOrder(ref StreamWriter sw)
        {
            string header = "";

            for (int i = 0; i < HEADER_ORDER.Length; i++)
            {
                header += HEADER_ORDER[i] + "\t";
            }

            sw.WriteLine(header);
        }

        private void fillOrder(ref StreamWriter sw)
        {
            int len = rightOrder.Count;
            string content = "";

            for (int i = 0; i < len; i++)
            {
                content = "";
                content += orderOn[i] + "\t";
                content += orderOff[i] + "\t";
                content += rightOrder[i] + "\t";
                content += userInputOrder[i];
                sw.WriteLine(content);
            }
        }
    }
}

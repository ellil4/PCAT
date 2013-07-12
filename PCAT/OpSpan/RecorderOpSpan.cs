using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PCATData;

namespace FiveElementsIntTest.OpSpan
{
    public class RecorderOpSpan //: RecorderBase
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

        private static string[] HEADER_INFO_COLLECTION = {"exp," + "exp on time," + "exp off time," + 
                                                      "displayed answer," + "choice, correctness," + 
                                                    "choice Show time," + "choice made time," + "animal," + "group ID"};

        private static string[] HEADER_ORDER = { "order on," + "order off," + "right order," + "user input order"};

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

        public void outputReport(string filepathInfoCollection, string filepathOrder)
        {
            //output info collection
            //abstractOut(makeHeaderInfoCollection, fillInfoCollection, filepathInfoCollection);
            //output order collection
            //abstractOut(makeHeaderOrder, fillOrder, filepathOrder);
            PCATDataSaveReport();
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

        private void makeHeaderInfoCollection(ref StreamWriter sw)
        {
            string header = "";

            for (int i = 0; i < HEADER_INFO_COLLECTION.Length; i++)
            {
                header += HEADER_INFO_COLLECTION[i];
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
                content += mathExpression[i] + ",";
                content += mathOn[i] + ",";
                content += mathOff[i] + ",";
                content += displayedAnswer[i] + ",";
                content += choice[i] + ",";
                content += correctness[i] + ",";
                content += choiceShowTime[i] + ",";
                content += choiceMadeTime[i] + ",";
                content += animal[i] + ",";
                //content += groupID[i] + ",";
                sw.WriteLine(content);
            }
        }

        private void makeHeaderOrder(ref StreamWriter sw)
        {
            string header = "";

            for (int i = 0; i < HEADER_ORDER.Length; i++)
            {
                header += HEADER_ORDER[i];
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
                content += orderOn[i] + ",";
                content += orderOff[i] + ",";
                content += rightOrder[i] + ",";
                content += userInputOrder[i];
                sw.WriteLine(content);
            }
        }
    }
}

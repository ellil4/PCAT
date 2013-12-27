using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PCATData;

namespace FiveElementsIntTest.OpSpan2
{
    public class RecorderOpSpan2 : RecorderBase
    {
        //chart 1
        public List<string> mathExpression;
        public List<long> mathOn;
        public List<long> mathOff;
        public List<long> mathDure;//op2
        public List<String> equaLv;

        public List<string> displayedAnswer;
        public List<long> choiceShowTime;
        public List<long> choiceMadeTime;
        public List<long> choiceDure;//op2
        public List<string> choice;
        public List<bool> correctness;//original answers`
        public List<string> animal;
        public List<bool> isPractise;
        public List<bool> isExtra;
        public List<bool> isOvertime;

        public List<int> spanWidth;//op2
        public List<int> groupNum;//op2

        public List<int> inGroupNum;

        //chart 2
        public List<long> orderOn;
        public List<long> orderOff;
        public List<long> orderDure;//op2
        public List<string> rightOrder;
        public List<string> userInputOrder;
        public List<bool> isPractiseG;
        public List<bool> isExtraG;

        public List<int> spanWidthG;//op2
        public List<int> groupNumG;//op2

        private static string[] HEADER_INFO_COLLECTION = 
        {"inter", "inter on", "inter off", "inter dure", "inter lv",  "displayed answer", "choice", "correctness",
        "choice Show time", "choice made time", "choice dure", "mem target", 
        "isPractise", "isExtra", "isOvertime", "span width", "group num", "in group num", "baseline"};

        private static string[] HEADER_ORDER = 
        {"order on", "order off", "order dure", "right order", "user input order",
        "isPractise", "isExtra",  "span width", "group num"};

        private static string[] HEADER_PRACMATH = 
        { "inter", "ShownResult", "inter dure", "TrueAnswer", "UserAnswer" };

        private static string[] HEADER_PRACORDER = 
            {"order dure", "RealOrder", "UserAnswer", "Corectness"};

        //chart 3 practise math
        public List<StEquation> mathPracEquations;
        public List<long> mathPracOn;//op2
        public List<long> mathPracOff;//op2
        public List<long> mathPracRTs;
        public List<string> mathPracAnswers;
        
        //chart 4 practise order
        public List<string> orderPracRealOrder;
        public List<string> orderPracAnswers;
        public List<bool> orderPracCorrectness;
        public List<long> orderPracOn;//op2
        public List<long> orderPracOff;//op2
        public List<long> orderPracRTs;

        public BasePage mPage;

        public RecorderOpSpan2(BasePage page)
        {
            mPage = page;

            mathExpression = new List<string>();
            mathOn = new List<long>();
            mathOff = new List<long>();
            mathDure = new List<long>();
            equaLv = new List<string>();
            displayedAnswer = new List<string>();
            choice = new List<string>();
            correctness = new List<bool>();
            choiceShowTime = new List<long>();
            choiceMadeTime = new List<long>();
            choiceDure = new List<long>();
            animal = new List<string>();
            isOvertime = new List<bool>();
            isExtra = new List<bool>();
            isExtraG = new List<bool>();
            isPractise = new List<bool>();
            isPractiseG = new List<bool>();
            spanWidth = new List<int>();
            spanWidthG = new List<int>();
            groupNum = new List<int>();
            groupNumG = new List<int>();
            inGroupNum = new List<int>();

            orderOn = new List<long>();
            orderOff = new List<long>();
            orderDure = new List<long>();
            rightOrder = new List<string>();
            userInputOrder = new List<string>();

            mathPracEquations = new List<StEquation>();
            mathPracOn = new List<long>();
            mathPracOff = new List<long>();
            mathPracRTs = new List<long>();
            mathPracAnswers = new List<string>();

            orderPracRealOrder = new List<string>();
            orderPracAnswers = new List<string>();
            orderPracCorrectness = new List<bool>();
            orderPracOn = new List<long>();
            orderPracOff = new List<long>();
            orderPracRTs = new List<long>();
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
            if (mathPracEquations != null)
            {
                len = mathPracEquations.Count;
            }

            string content = "";
            for (int i = 0; i < len; i++)
            {
                content = "";
                content += mathPracEquations[i].Equation + "\t";
                content += mathPracEquations[i].Result + "\t";
                content += mathPracRTs[i] + "\t";
                content += mathPracEquations[i].Answer + "\t";
                content += mathPracAnswers[i] + "\t";
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
            if (orderPracRealOrder != null)
            {
                len = orderPracRealOrder.Count;
            }
            string content = "";
            for (int i = 0; i < len; i++)
            {
                content = "";

                content += orderPracRTs[i] + "\t";

                content += orderPracRealOrder[i] + "\t";

                content += orderPracAnswers[i] + "\t";

                content += orderPracCorrectness[i] + "\t";

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
                content += mathDure[i] + "\t";
                content += equaLv[i] + "\t";
                content += displayedAnswer[i] + "\t";
                content += choice[i] + "\t";
                content += correctness[i] + "\t";
                content += choiceShowTime[i] + "\t";
                content += choiceMadeTime[i] + "\t";
                content += choiceDure[i] + "\t";
                content += animal[i] + "\t";
                content += isPractise[i] + "\t";
                content += isExtra[i] + "\t";
                content += isOvertime[i] + "\t";
                content += spanWidth[i] + "\t";
                content += groupNum[i] + "\t";
                content += inGroupNum[i] + "\t";
                content += mPage.mInterTimeLimit.ToString() + "\t";
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
                content += orderDure[i] + "\t";
                content += rightOrder[i] + "\t";
                content += userInputOrder[i] + "\t";
                content += isPractiseG[i] + "\t";
                content += isExtraG[i] + "\t";
                content += spanWidthG[i] + "\t";
                content += groupNumG[i] + "\t";

                sw.WriteLine(content);
            }
        }
    }
}

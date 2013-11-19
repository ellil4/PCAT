using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest
{
    class OutputDigitSymbol
    {
        public static string[] HEADER = {"correctness\t", "user input left\t", "answer left\t", "time left\t",
                                        "user input right\t", "answer right\t", "time right"};

        public OutputDigitSymbol()
        {
 
        }

        public void Output(string destPath, 
            ref EvalueDigitSymbol valuator, ref FEITTimer timer)
        {
            StreamWriter sw = null;

            try
            {
                sw = File.CreateText(destPath);
                createHeader(ref sw);
                writeContent(ref sw, ref valuator, ref timer);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void createHeader(ref StreamWriter sWriter)
        {
            string content = "\t";
            for (int i = 0; i < HEADER.Length; i++)
            {
                content += HEADER[i];
            }
            sWriter.WriteLine(content);
        }

        private void writeContent(ref StreamWriter sWriter, 
            ref EvalueDigitSymbol valuator, ref FEITTimer timer)
        {
            string content = "";
            int len = valuator.mTF.Count;
            for (int i = 0; i < len; i++)
            {
                content = "";
                content += "block" + (i + 1).ToString() + "\t";

                content += valuator.mTF[i].ToString() + "\t";

                content += valuator.mLeftInput[i].ToString() + "\t";
                content += valuator.mLeftAnswer[i].ToString() + "\t";
                content += timer.mTracker[i * 2].ToString() + "\t";

                content += valuator.mRightInput[i].ToString() + "\t";
                content += valuator.mRightAnswer[i].ToString() + "\t";
                content += timer.mTracker[i * 2 + 1].ToString();

                sWriter.WriteLine(content);
            }
        }
    }
}

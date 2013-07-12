using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest.CtSpan
{
    public class FormWriter
    {
        public FormWriter(List<StGraphItem> items, List<StResult> results)
        {
            //do write
            StreamWriter SW = File.CreateText(
                System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + 
                DateTime.Now.ToFileTime().ToString());

            SW.WriteLine(GetHeaderLine());

            for(int i = 0; i < results.Count; i++)
            {
                SW.WriteLine(GetRecLine(items[i], results[i]));
            }

            SW.Close();
        }

        public String GetHeaderLine()
        {
            return "SpanWidth\tPractiseMode\tGroup\tGroupSeq\tTarget\tShape\tColor\tProportion\tScatterTar\tScatterComm\tProcCode\tProcPos\tProcACC\tProcRT\tRecACC\tRecRT\tRecCorNum\tRecCRESP\tRecRESP\tRecCorCode";
        }

        public String GetRecLine(StGraphItem item, StResult result)
        {
            String retval = "";

            String countingCorrectness = "";
            String correctness = "";

            if (result.CountingCorrectness)
            {
                countingCorrectness = "1";
            }
            else
            {
                countingCorrectness = "0";
            }

            if (result.Correctness)
            {
                correctness = "1";
            }
            else
            {
                correctness = "0";
            }

            retval += result.SpanWidth + "\t" + result.PractiseMode + "\t" + result.GroupNum + "\t" + result.GroupSeq + "\t" +
                item.TarCount + "\t" + item.InterCircleCount + "\t" + item.InterTriCount + "\t" +
                (((item.InterTriCount + item.InterCircleCount)) / item.TarCount).ToString() + "\t" +
                item.DistanceTar + "\t" + item.DistanceComm + "\t" + result.ClickOnType + "\t" + result.ClickOnPosition + "\t" +
                countingCorrectness + "\t" + result.GraphRT + "\t" +
                correctness + "\t" + result.RT + "\t" + result.UserSerialCorrectCount + "\t" +
                result.StdSerial + "\t" + result.UserSerial + "\t" + result.UserSerialCorrectMask;

            return retval;
        }
    }
}

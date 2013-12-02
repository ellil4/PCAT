using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest.VocabCommon
{
    public class VCReportWriter
    {
        public VCReportWriter(String path, StVCResult[] results, List<StVCItem> items)
        {

            StreamWriter sw = File.CreateText(path);
            int len = results.Length;
            sw.WriteLine("RT\tSelected\tScore");

            for (int i = 0; i < len; i++)
            {
                if (results[i] != null)
                {
                    String line;
                    if (results[i].SelectedItemIndex != -1)
                    {
                        line = results[i].RT + "\t" +
                            results[i].SelectedItemIndex + "\t" +
                            items[i].Weights[results[i].SelectedItemIndex];
                    }
                    else
                    {
                        line = results[i].RT + "\t" +
                            results[i].SelectedItemIndex + "\t" +
                            "2";
                    }
                    sw.WriteLine(line);
                }
            }

            sw.Close();
        }
    }
}

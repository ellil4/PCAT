using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest.VocabCommon
{
    public class VCReportWriter
    {
        public VCReportWriter(String path, List<StVCResult> results, List<StVCItem> items)
        {

            StreamWriter sw = File.CreateText(path);
            int len = results.Count;
            sw.WriteLine("RT\tSelected\tScore");

            for (int i = 0; i < len; i++)
            {
                String line = results[i].RT + "\t" + 
                    results[i].SelectedItemIndex + "\t" + 
                    items[i].Weights[results[i].SelectedItemIndex];
                sw.WriteLine(line);
            }

            sw.Close();
        }
    }
}

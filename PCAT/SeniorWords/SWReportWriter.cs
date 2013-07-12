using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest.SeniorWords
{
    public class SWReportWriter
    {
        public SWReportWriter(String path, List<StSWResult> results, List<StSWItem> items)
        {

            StreamWriter sw = File.CreateText(path);
            int len = results.Count;
            sw.WriteLine("RT, Selected, Score");

            for (int i = 0; i < len; i++)
            {
                String line = results[i].RT + "," + 
                    results[i].SelectedItemIndex + "," + 
                    items[i].Weights[results[i].SelectedItemIndex];
                sw.WriteLine(line);
            }

            sw.Close();
        }
    }
}

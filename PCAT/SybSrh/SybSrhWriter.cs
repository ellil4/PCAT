using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest.SybSrh
{
    public class SybSrhWriter
    {
        private void writeHeader(ref StreamWriter sw)
        {
            //make header
            String header = "";
            //target part1
            for (int i = 0; i < 2; i++)
            {
                header += "T" + i + "type\t";
                header += "T" + i + "index\t";
                header += "isTarT" + i + "\t";
            }
            //selections part2
            for (int i = 0; i < 5; i++)
            {
                header += "S" + i + "type\t";
                header += "S" + i + "index\t";
                header += "isTarS" + i + "\t";
            }
            //part3
            header += "RT\t Answer\t Correctness \t Iden \t SameCateg \t OpCategNum";
            sw.WriteLine(header);
        }

        private String appendOneElem2Str(String input, SybSrhVisualElem elem)
        {
            input += elem.Type + "\t";
            input += elem.Index + "\t";

            if (elem.IfTrue)
            {
                input += "1\t";
            }
            else
            {
                input += "0\t";
            }

            return input;
        }

        private void writeResultContent(List<SybSrhResult> results, ref StreamWriter sw)
        {
            //content
            //part1
            String line;
            List<int> typeCollection = new List<int>();
            for (int i = 0; i < results.Count; i++)
            {
                line = "";
                //stems
                for (int j = 0; j < 2; j++)
                {
                    line = appendOneElem2Str(line, results[i].Item.Target[j]);
                }
                //selections
                for (int k = 0; k < 5; k++)
                {
                    line = appendOneElem2Str(line, results[i].Item.Selection[k]);
                }
                //part3

                line += results[i].RT + "\t";

                if (results[i].Answer)
                {
                    line += 1 + "\t";
                }
                else
                {
                    line += 0 + "\t";
                }

                if (results[i].Correctness)
                {
                    line += 1 + "\t";
                }
                else
                {
                    line += 0 + "\t";
                }

                //iden
                if (results[i].Item.Target[0].IfTrue || results[i].Item.Target[1].IfTrue)
                {
                    line += 1 + "\t";
                }
                else
                {
                    line += 0 + "\t";
                }

                //same category
                if (results[i].Item.Target[0].Type == results[i].Item.Target[1].Type)
                {
                    line += 1 + "\t";
                }
                else
                {
                    line += 0 + "\t";
                }

                //count types
                for (int j = 0; j < results[i].Item.Selection.Length; j++)//5 max
                {
                    if (j == 0)
                    {
                        typeCollection.Add(results[i].Item.Selection[j].Type);
                    }
                    else
                    {
                        for (int k = 0; k < typeCollection.Count; k++)
                        {
                            if (results[i].Item.Selection[j].Type == typeCollection[k])
                                break;

                            if (k == typeCollection.Count - 1)
                            {
                                typeCollection.Add(results[i].Item.Selection[j].Type);
                            }
                        }
                    }
                }

                line += typeCollection.Count + "\t";
                typeCollection.Clear();

                sw.WriteLine(line);
            }
        }

        public void WriteResults(String path, List<SybSrhResult> results)
        {
            StreamWriter sw = File.CreateText(path);

            writeHeader(ref sw);
            writeResultContent(results, ref sw);

            sw.Close();
        }

    }
}

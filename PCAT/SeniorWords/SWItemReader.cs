using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Text.RegularExpressions;


namespace FiveElementsIntTest.SeniorWords
{
    public class SWItemReader
    {        

        public List<StSWItem> ReadSheet(String path)
        {
            List<StSWItem> retval = new List<StSWItem>();

            StreamReader rd = null;

            try
            {
                rd = new StreamReader(path, UnicodeEncoding.GetEncoding("GB2312"));
                Regex rex = new Regex("[^\t]+");

                rd.ReadLine();
                String lineContent;

                while ((lineContent = rd.ReadLine()) != null)
                {
                    MatchCollection mc = rex.Matches(lineContent);
                    StSWItem item = new StSWItem();

                    for (int i = 1; i < mc.Count; i++)
                    {
                        if (i == 1)
                        {
                            item.Casual = mc[i].Value;
                        }
                        else if (i > 1 && i < 7)
                        {
                            item.Selections.Add(mc[i].Value);
                        }
                        else if (i >= 7 && i < 12)
                        {
                            item.Weights.Add(Int32.Parse(mc[i].Value));
                        }
                        
                    }

                    retval.Add(item);
                }

            }
            catch (Exception)
            {
                if (System.Windows.Forms.MessageBox.Show("文件或文件结构错误") ==
                   System.Windows.Forms.DialogResult.OK)
                {
                    if(rd != null)
                        rd.Close();

                    System.Environment.Exit(0);
                }
            }
            finally
            {
                rd.Close();
                rd = null;
            }

            return retval;
        }
    }
}

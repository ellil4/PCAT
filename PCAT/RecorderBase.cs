using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest
{
    public class RecorderBase
    {
        public delegate void FuncPart(ref StreamWriter sw);

        public void abstractOut(FuncPart headerFunc, FuncPart contentFunc, string filepath)
        {
            try
            {
                //File.CreateText(filepath);
                StreamWriter sw = new StreamWriter(filepath, false, Encoding.GetEncoding("gb2312"));
                headerFunc(ref sw);
                contentFunc(ref sw);
                sw.Close();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }
    }
}

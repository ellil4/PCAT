using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibTabCharter
{
    public class TabCharter
    {
        private String mLocation;

        public TabCharter(String location)
        {
            mLocation = location;
        }

        private String makeLine(List<String> arr)
        {
            String retval = "";
            for (int i = 0; i < arr.Count; i++)
                retval += arr[i] + "\t";
            return retval;
        }

        public void Create(List<String> header)
        {
            if (!File.Exists(mLocation))
            {
                StreamWriter sw = 
                    new StreamWriter(
                        mLocation, false, Encoding.GetEncoding("GB2312"));

                sw.WriteLine(makeLine(header));
                sw.Close();
            }
        }

        public void Append(List<String> content)
        {
            if (File.Exists(mLocation))
            {
                StreamWriter sw =
                    new StreamWriter(
                        mLocation, true, Encoding.GetEncoding("GB2312"));
                
                sw.WriteLine(makeLine(content));
                sw.Close();
            }
        }
       
    }
}

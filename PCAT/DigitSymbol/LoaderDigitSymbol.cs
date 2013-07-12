using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace FiveElementsIntTest
{
    public class LoaderDigitSymbol
    {
        private string FILE_ADDR;
        private StreamReader mReader;

        public LoaderDigitSymbol()
        {

        }

        private string GetContentStr()
        {
            //try to read all
            string content = "";
            string buf;
            FILE_ADDR = 
                System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "DIGIT.txt";

            mReader = File.OpenText(FILE_ADDR);
            {
                while ((buf = mReader.ReadLine()) != null)
                {
                    content += buf;
                }
            }

            mReader.Close();
            return content;
        }

        public ArrayList GetContentDigits()
        {
            ArrayList retval = new ArrayList();
            Regex rex = new Regex("[1-9]");
            MatchCollection collection = rex.Matches(GetContentStr());
            for (int i = 0; i < collection.Count; i++)
            {
                retval.Add(int.Parse(collection[i].Value.ToString()));
            }

            return retval;
        }

    }
}

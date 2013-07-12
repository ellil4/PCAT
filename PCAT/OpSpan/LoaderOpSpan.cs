using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace FiveElementsIntTest.OpSpan
{
    public class LoaderOpSpan
    {
        private String FILE_ADDR;
        private StreamReader mReader;
        private String mFilename;

        public LoaderOpSpan(String Filename)
        {
            mFilename = Filename;
            FILE_ADDR =
                System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + mFilename;
            mReader = new StreamReader(FILE_ADDR, Encoding.GetEncoding("gb2312"));
        }

        public List<TrailOS_ST> GetTrails()
        {
            List<TrailOS_ST> retval = new List<TrailOS_ST>();

            String targetStr;
            Regex rex = new Regex("[^,]+");
            MatchCollection mts;

            mReader.ReadLine();
            while ((targetStr = mReader.ReadLine()) != null)
            {
                mts = rex.Matches(targetStr);
                TrailOS_ST stos = new TrailOS_ST();
                stos.equation = mts[1].Value.ToString();
                stos.result = mts[2].Value.ToString();
                
                if(int.Parse(mts[3].Value.ToString()) == 1)
                {
                    stos.correctness = true;
                }
                else
                {
                    stos.correctness = false;
                }

                stos.memTarget = mts[4].Value.ToString();
                retval.Add(stos);
            }

            
            return retval;
        }

         ~LoaderOpSpan()
        {
            mReader.Close();
        }
    }
}

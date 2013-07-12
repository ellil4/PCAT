using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace FiveElementsIntTest.SymSpan
{
    class LoaderSymmSpan
    {
        private string ADDR;
        private string mFilename;
        private StreamReader mSR;

        public LoaderSymmSpan(string filename)
        {
            mFilename = filename;
            ADDR = GetBaseFolder() + mFilename;

            mSR = new StreamReader(ADDR, Encoding.GetEncoding("gb2312"));
        }

        public static string GetBaseFolder()
        {
            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        public List<TrailSS_ST> GetTrails()
        {
            List<TrailSS_ST> retval = new List<TrailSS_ST>();

            Regex rex = new Regex("\\w+");
            mSR.ReadLine();
            
            while (mSR.Peek() >= 0)
            {
                MatchCollection mc = rex.Matches(mSR.ReadLine());
                TrailSS_ST trail = new TrailSS_ST();
                trail.FileName = mc[0].Value;

                if (((String)mc[1].Value).Equals("1"))
                {
                    trail.IsSymm = true;
                }
                else
                {
                    trail.IsSymm = false;
                }

                retval.Add(trail);
            }

            return retval;
        }

         ~LoaderSymmSpan()
        {
            mSR.Close();
        }
    }
}

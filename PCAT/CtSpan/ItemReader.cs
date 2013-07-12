using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace FiveElementsIntTest.CtSpan
{
    public class ItemReader
    {
        public List<StGraphItem> mItems;

        public ItemReader(String addr)
        {
            mItems = doRead(addr);
        }

        private List<StGraphItem> doRead(String addr)
        {
            List<StGraphItem> retval = new List<StGraphItem>();

            StreamReader SR = File.OpenText(addr);

            String Line = null;
            Regex rex = new Regex("[^\t]+");
            
            while ((Line = SR.ReadLine()) != null)
            {
                MatchCollection mts = rex.Matches(Line);
                StGraphItem sgi = new StGraphItem();

                sgi.TarCount = Int32.Parse(mts[0].Value);
                sgi.InterCircleCount = Int32.Parse(mts[1].Value);
                sgi.InterTriCount = Int32.Parse(mts[2].Value);
                sgi.DistanceTar = (short)Int32.Parse(mts[3].Value);
                sgi.DistanceComm = (short)Int32.Parse(mts[4].Value);

                retval.Add(sgi);
            }

            SR.Close();

            return retval;
        }
    }
}

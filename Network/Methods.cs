using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace Network
{
    public class Methods
    {
        public static String AppendInfo2String(String src, String info)
        {
            return src + "\t" + info;
        }

        public static IPAddress GetIPAFromString(String ipaStr)
        {
            Regex rex = new Regex("[^.]+");
            MatchCollection mc = rex.Matches(ipaStr);
            IPAddress retval = new IPAddress(new byte[] { byte.Parse(mc[0].Value), 
                byte.Parse(mc[1].Value), 
                byte.Parse(mc[2].Value), 
                byte.Parse(mc[3].Value)});
            return retval;
        }
    }
}

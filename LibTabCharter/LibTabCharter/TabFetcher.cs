using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace LibTabCharter
{
    public class TabFetcher
    {
       
        String mToken = "";
        String mLocation = "";
        StreamReader mSR;
        Regex mRex;

        public TabFetcher(String location, String token)
        {
            mToken = token;
            mLocation = location;
            mRex = new Regex("[^" + mToken + "]+");
        }

        public long Open()
        {
            long retval = 0;

            try
            {
                mSR = new StreamReader(mLocation, Encoding.GetEncoding("GB2312"));
                retval = mSR.BaseStream.Length;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return retval;
        }

        public void Close()
        {
            mSR.Close();
        }

        /*public List<String> GetLineFrom(long position)
        {
            return getLineBase(funcSeek, position);
        }*/

        public List<String> GetLineAt(int num)
        {
            List<String> retval = new List<string>();

            String strBuf = "";
            int lineCount = 0;

            while ((strBuf = mSR.ReadLine()) != null)
            {
                if (lineCount == num)
                {
                    MatchCollection mc = mRex.Matches(strBuf);
                    for (int i = 0; i < mc.Count; i++)
                    {
                        retval.Add(mc[i].Value);
                        
                    }

                    break;
                }

                lineCount++;
            }
            mSR.Dispose();
            mSR = new StreamReader(mLocation, Encoding.GetEncoding("GB2312"));
            return retval;
        }

        public List<String> GetLineBy()
        {
            List<String> retval = new List<string>();

            /*if (func != null)
            {
                func(para);
            }*/

            String strBuf = "";

            if (mSR.Peek() != -1)
            {
                strBuf = mSR.ReadLine();
            }

            MatchCollection mc = mRex.Matches(strBuf);
            for (int i = 0; i < mc.Count; i++)
            {
                retval.Add(mc[i].Value);
            }

            return retval;
            //return getLineBase();
        }

        public int GetLineCount()
        {
            int retval = mSR.ReadToEnd().Split('\n').Length;
            Close();
            Open();
            return retval;
        }

        /*public int getHeaderByteLen()
        {
            long curPos = mSR.BaseStream.Position;
            funcSeek(0);
            String header = mSR.ReadLine();
            funcSeek(curPos);
            return header.Length;
        }*/

        //delegate void readingSpecificFunc(long para);

        /*private void funcSeek(long position)
        {
            mSR.BaseStream.Seek(position, SeekOrigin.Begin);
        }*/

        /*private List<String> getLineBase(readingSpecificFunc func = null, long para = 0)
        {
         
        }*/
    }
}

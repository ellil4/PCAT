using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using LibTabCharter;

namespace FiveElementsIntTest.SymSpan
{
    class LoaderSymmSpan
    {
        private string mPicDscpADDR;
        private string mFixedItemsADDR;
        private string mFixedLocationExeADDR;
        private string mFixedSymmExeADDR;

        private StreamReader mSR;

        public LoaderSymmSpan(string picDscpFilePath, string fixedItemsFilePath, 
                string fixedLocationExePath, string fixedSymmExePath)
        {
            mPicDscpADDR = GetBaseFolder() + picDscpFilePath;
            mFixedItemsADDR = GetBaseFolder() + fixedItemsFilePath;
            mFixedSymmExeADDR = GetBaseFolder() + fixedSymmExePath;
            mFixedLocationExeADDR = GetBaseFolder() + fixedLocationExePath;

            mSR = new StreamReader(mPicDscpADDR, Encoding.GetEncoding("gb2312"));
        }

        public List<List<int>> GetFixedLocationExe()
        {
            //scheme 2,2,3,3
            int[] scheme = {2, 2, 3, 3};
            List<List<int>> retval = new List<List<int>>();

            TabFetcher fet = new TabFetcher(mFixedLocationExeADDR, "\\t");
            fet.Open();
            fet.GetLineBy();//skip header

            for (int i = 0; i < scheme.Length; i++)
            {
                List<int> group = new List<int>();
                for (int j = 0; j < scheme[i]; j++)
                {
                    List<String> line = fet.GetLineBy();
                    group.Add(Int32.Parse(line[2]) - 1);
                }
                retval.Add(group);
            }
            

            fet.Close();
            return retval;
        }

        public List<TrailSS_ST> GetFixedSymmExe()
        {
            List<TrailSS_ST> retval = new List<TrailSS_ST>();

            TabFetcher fet = new TabFetcher(mFixedSymmExeADDR, "\\t");
            fet.Open();
            List<String> line = null;
            fet.GetLineBy();//skip header
            //int i = 0;
            while ((line = fet.GetLineBy()).Count != 0)
            {
                //i++;
                TrailSS_ST st = new TrailSS_ST();
                st.FileName = line[1] + ".bmp";
                if (line[2] == "j")
                {
                    st.IsSymm = false;
                }
                else
                {
                    st.IsSymm = true;
                }

                retval.Add(st);
            }
            fet.Close();
            return retval;
        }

        public List<TrailsGroupSS> GetFixedComprehExe(int[] scheme)
        {
            List<TrailsGroupSS> retval = new List<TrailsGroupSS>();

            TabFetcher fet = new TabFetcher(mFixedItemsADDR, "\\t");
            fet.Open();
            fet.GetLineBy();//skip header

            for (int i = 0; i < scheme.Length; i++)
            {
                TrailsGroupSS ss = new TrailsGroupSS();
                for (int j = 0; j < scheme[i]; j++)
                {
                    List<String> line = fet.GetLineBy();
                    TrailSS_ST st = new TrailSS_ST();

                    st.FileName = line[1] + ".bmp";
                    if (line[2] == "j")
                    {
                        st.IsSymm = false;
                    }
                    else
                    {
                        st.IsSymm = true;
                    }

                    st.Position = Int32.Parse(line[4]) - 1;
                    ss.Trails.Add(st);
                }

                retval.Add(ss);
            }

            fet.Close();
            return retval;
        }

        public List<TrailsGroupSS> GetFixedItemGroups(int[] scheme)
        {
            List<TrailsGroupSS> retval = new List<TrailsGroupSS>();
            TabFetcher fetcher = new TabFetcher(mFixedItemsADDR, "\\t");
            fetcher.Open();

            //jump over exe
            for(int a = 0; a < 7; a++)//7 = 2 + 2 + 2 + 1(header)
                fetcher.GetLineBy();

            for (int i = 0; i < scheme.Length; i++)
            {
                TrailsGroupSS ss = new TrailsGroupSS();
                for (int j = 0; j < scheme[i]; j++)
                {
                    List<String> line = fetcher.GetLineBy();

                    TrailSS_ST st = new TrailSS_ST();
                    st.FileName = line[1] + ".bmp";
                    if (line[2] == "j")
                    {
                        st.IsSymm = false;
                    }
                    else
                    {
                        st.IsSymm = true;
                    }
                    st.Position = Int32.Parse(line[4]) - 1;
                    ss.Trails.Add(st);
                }

                retval.Add(ss);
            }

            fetcher.Close();
            return retval;
        }

        public static string GetBaseFolder()
        {
            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        public List<TrailSS_ST> GetResourceList()
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

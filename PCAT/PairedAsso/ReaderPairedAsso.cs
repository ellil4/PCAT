using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;

namespace FiveElementsIntTest.PairedAsso
{
    public class ReaderPairedAsso
    {
        
        
        public static List<List<StPair>> GetLearningItems()
        {
            List<List<StPair>> retval = new List<List<StPair>>();
            String BasePath = FEITStandard.GetExePath() + "PAIREDASSO\\";
            int groupLen = PagePairedAsso.mGroupLen;
            int groupCount = 3;

            TabFetcher fet = new TabFetcher(BasePath + "Learn.txt", "\\t");
            fet.Open();
            fet.GetLineBy();

            List<String> line = null;

            for (int j = 0; j < groupCount; j++)
            {
                List<StPair> group = new List<StPair>();
                for (int i = 0; i < groupLen; i++)
                {
                    line = fet.GetLineBy();
                    StPair pair = new StPair();
                    pair.First = line[1];
                    pair.Second = line[2];
                    group.Add(pair);
                }
                retval.Add(group);
            }

            fet.Close();

            return retval;
        }

        public static List<List<StTest>> GetTestItems()
        {
            List<List<StTest>> retval = new List<List<StTest>>();

            String BasePath = FEITStandard.GetExePath() + "PAIREDASSO\\";
            int groupLen = PagePairedAsso.mGroupLen;
            int groupCount = 3;

            TabFetcher fet = new TabFetcher(BasePath + "Test.txt", "\\t");
            fet.Open();
            fet.GetLineBy();

            List<String> line = null;
            for (int k = 0; k < groupCount; k++)
            {
                List<StTest> group = new List<StTest>();
                for (int i = 0; i < groupLen; i++)
                {
                    line = fet.GetLineBy();
                    StTest test = new StTest();
                    test.Pair.First = line[1];
                    test.Pair.Second = line[2];
                    for (int j = 0; j < 9; j++)
                    {
                        test.Chars9[j] = line[3 + j];
                    }
                    test.CellAnswer1 = Int32.Parse(line[12]);
                    test.CellAnswer2 = Int32.Parse(line[13]);
                    group.Add(test);
                }
                retval.Add(group);
            }

            fet.Close();
            return retval;
        }
    }
}

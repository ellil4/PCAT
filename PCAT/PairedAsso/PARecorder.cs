using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FiveElementsIntTest.PairedAsso
{
    public class PARecorder : RecorderBase
    {
        private PagePairedAsso mPage;
        private static string[] mHeader = { "Test", "Word1", "Word2", "CAnswer1", "CAnswer2", "ACC", "RT", "Answer1", "Answer2" };
        public PARecorder(PagePairedAsso page)
        {
            mPage = page;
        }

        public void outputReport(String path)
        {
            abstractOut(makeHeader, writeRec, path);
        }

        private void makeHeader(ref StreamWriter sw)
        {
            string header = "";
            for (int i = 0; i < mHeader.Length; i++)
            {
                header += mHeader[i] + "\t";
            }
            sw.WriteLine(header);
        }

        private void writeRec(ref StreamWriter sw)
        {
            string content = "";
            for (int i = 0; i < mPage.mRTs.Count; i++)
            {
                content = "";

                content += ((i / PagePairedAsso.mGroupLen) + 1).ToString() + "\t";
                content += mPage.mTestItems[i / PagePairedAsso.mGroupLen][i % PagePairedAsso.mGroupLen].Pair.First + "\t";
                content += mPage.mTestItems[i / PagePairedAsso.mGroupLen][i % PagePairedAsso.mGroupLen].Pair.Second + "\t";
                content += mPage.mTestItems[i / PagePairedAsso.mGroupLen][i % PagePairedAsso.mGroupLen].CellAnswer1 + "\t";
                content += mPage.mTestItems[i / PagePairedAsso.mGroupLen][i % PagePairedAsso.mGroupLen].CellAnswer2 + "\t";
                if (mPage.mOrders[i].Count == 2 &&
                    mPage.mTestItems[i / PagePairedAsso.mGroupLen][i % PagePairedAsso.mGroupLen].CellAnswer1 == (mPage.mOrders[i][0] + 1) &&
                    mPage.mTestItems[i / PagePairedAsso.mGroupLen][i % PagePairedAsso.mGroupLen].CellAnswer2 == (mPage.mOrders[i][1] + 1))
                {
                    content += "true" + "\t";
                }
                else
                {
                    content += "false" + "\t";
                }
                content += mPage.mRTs[i] + "\t";

                if (mPage.mOrders[i].Count > 0)
                {
                    content += (mPage.mOrders[i][0] + 1) + "\t";

                    if(mPage.mOrders[i].Count > 1)
                        content += (mPage.mOrders[i][1] + 1) + "\t";
                }

                sw.WriteLine(content);
            }
        }
    }
}

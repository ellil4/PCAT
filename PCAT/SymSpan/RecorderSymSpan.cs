using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PCATData;

namespace FiveElementsIntTest.SymSpan
{
    public class RecorderSymSpan : RecorderBase
    {
        public List<TrailSS_ST> inters;
        public List<bool> symmJudgeCorrectness;
        public List<long> symmOnTime;
        public List<long> symmOffTime;
        public List<long> choiceShownTime;
        public List<long> choiceMadeTime;

        public List<List<int>> shownPosition;
        public List<List<int>> userSelPosition;
        public List<long> posOnTime;
        public List<long> posOffTime;
        public List<bool> posCorrectness;

        //prac part
        //symm
        public List<string> pracSymmPicName;
        public List<bool> pracSymmIsSymm;
        public List<long> pracSymmRTs;
        public List<bool> pracSymmCorrect;
        //position
        public List<string> pracPosPos;
        public List<string> pracPosUserSel;
        public List<long> pracPosRTs;
        public List<bool> pracPosCorrect;


        private static string[] interHeader = {"symm pic name", "is symm", "position", "symm on time",
            "symm off time", "choice correctness", "choice shown time", "choice made time", "segment ID"};

        private static string[] positionHeader = {"shown positions", "user selected",
            "position on time", "position off time", "correctness", "elements in array"};

        private static string[] pracSymmHeader = { "symm pic name", "is symm", "RT", "choice correctness"};

        private static string[] pracPositionHeader = {"shown positions", "user selected", "RT", "correctness"};

        public PageSymmSpan mPage;

        public RecorderSymSpan(PageSymmSpan page)
        {
            mPage = page;

            inters = new List<TrailSS_ST>();
            symmJudgeCorrectness = new List<bool>();
            symmOnTime = new List<long>();
            symmOffTime = new List<long>();
            choiceShownTime = new List<long>();
            choiceMadeTime = new List<long>();

            shownPosition = new List<List<int>>();
            userSelPosition = new List<List<int>>();
            posOnTime = new List<long>();
            posOffTime = new List<long>();
            posCorrectness = new List<bool>();

            pracSymmPicName = new List<string>();
            pracSymmRTs = new List<long>();
            pracSymmIsSymm = new List<bool>();
            pracSymmCorrect = new List<bool>();

            pracPosPos = new List<string>();
            pracPosRTs = new List<long>();
            pracPosUserSel = new List<string>();
            pracPosCorrect = new List<bool>();
        }

        public void PCATDataSaveReport()
        {
            List<QRecSymSpan> rec = new List<QRecSymSpan>();
            int thisCursor = 0;
            for (int i = 0; i < mPage.mTestGroupScheme.Length; i++)
            {
                QRecSymSpan subgrpRec = new QRecSymSpan();
                for (int j = 0; j < mPage.mTestGroupScheme[i]; j++)
                {
                    subgrpRec.SymStim.Add(inters[thisCursor].FileName);
                    subgrpRec.IsSym.Add(inters[thisCursor].IsSymm);
                    subgrpRec.SymExposureTime.Add(symmOffTime[thisCursor] - symmOnTime[thisCursor]);
                    subgrpRec.SymCorrectness.Add(symmJudgeCorrectness[thisCursor]);
                    subgrpRec.SymRT.Add(choiceMadeTime[thisCursor] - choiceShownTime[thisCursor]);

                    thisCursor++;
                }

                subgrpRec.PosStim = shownPosition[i];
                subgrpRec.UserPos = userSelPosition[i];
                subgrpRec.PosRT = posOffTime[i] - posOnTime[i];
                subgrpRec.Correctness = posCorrectness[i];

                subgrpRec.GroupID = mPage.mTestGroupScheme[i];
                subgrpRec.SubGroupID = PageSymmSpan.getSubGroupID(i);

                rec.Add(subgrpRec);
            }

            mPage.mMainWindow.mDB.AddSymSpanPosRecord(rec, mPage.mMainWindow.mUserID);
            mPage.mMainWindow.mDB.AddSymSpanSymmRecord(rec, mPage.mMainWindow.mUserID);
        }

        public void outputReport(string filepathInter, string filepathPos, 
            string filepathPracSymm, string filepathPracPos)
        {
            abstractOut(makeHeaderInter, fillInterChart, filepathInter);
            abstractOut(makeHeaderPos, fillPosChart, filepathPos);
            abstractOut(makeHeaderPracSymm, fillPracSymmChart, filepathPracSymm);
            abstractOut(makeHeaderPracPos, fillPracPos, filepathPracPos);
            //PCATDataSaveReport();
        }

        private void makeHeaderInter(ref StreamWriter sw)
        {
            string header = "";
            for (int i = 0; i < interHeader.Length; i++)
            {
                header += interHeader[i] + "\t";
            }
            sw.WriteLine(header);
        }

        private void makeHeaderPos(ref StreamWriter sw)
        {
            string header = "";
            for (int i = 0; i < positionHeader.Length; i++)
            {
                header += positionHeader[i] + "\t";
            }
            sw.WriteLine(header);
        }

        private void makeHeaderPracPos(ref StreamWriter sw)
        {
            string header = "";
            for (int i = 0; i < pracPositionHeader.Length; i++)
            {
                header += pracPositionHeader[i] + "\t";
            }
            sw.WriteLine(header);
        }

        private void makeHeaderPracSymm(ref StreamWriter sw)
        {
            string header = "";
            for (int i = 0; i < pracSymmHeader.Length; i++)
            {
                header += pracSymmHeader[i] + "\t";
            }
            sw.WriteLine(header);
        }

        private void fillInterChart(ref StreamWriter sw)
        {
            string content = "";
            for(int i = 0; i < inters.Count; i++)
            {
                content = "";

                content += inters[i].FileName + "\t";
                content += inters[i].IsSymm.ToString() + "\t";
                content += inters[i].Position.ToString() + "\t";

                content += symmOnTime[i].ToString() + "\t";
                content += symmOffTime[i].ToString() + "\t";
                content += symmJudgeCorrectness[i].ToString() + "\t";
                content += choiceShownTime[i].ToString() + "\t";
                content += choiceMadeTime[i].ToString() + "\t";

                sw.WriteLine(content);
            }
        }

        private void fillPracSymmChart(ref StreamWriter sw)
        {
            //"symm pic name", "is symm", "RT", "choice correctness"
            
            for (int i = 0; i < pracSymmPicName.Count; i++)
            {
                string content = "";

                content += pracSymmPicName[i] + "\t";
                content += pracSymmIsSymm[i] + "\t";
                content += pracSymmRTs[i] + "\t";
                content += pracSymmCorrect[i] + "\t";

                sw.WriteLine(content);
            }
        }

        private void fillPracPos(ref StreamWriter sw)
        {
            //"shown positions", "user selected", "RT", "correctness"
            
            for (int i = 0; i < pracPosPos.Count; i++)
            {
                string content = "";
                content += pracPosPos[i] + "\t";
                content += pracPosUserSel[i] + "\t";
                content += pracPosRTs[i] + "\t";
                content += pracPosCorrect[i] + "\t";

                sw.WriteLine(content);
            }
        }

        private void fillPosChart(ref StreamWriter sw)
        {
            string content = "";

            for (int i = 0; i < shownPosition.Count; i++)
            {
                content = "";

                for (int j = 0; j < (shownPosition[i]).Count; j++)
                {
                    content += "<" + (shownPosition[i])[j].ToString() + ">";
                }

                content += "\t";

                for (int k = 0; k < userSelPosition[i].Count; k++)
                {
                    content += "<" + (userSelPosition[i])[k].ToString() + ">";
                }

                content += "\t";

                content += posOnTime[i].ToString() + "\t";
                content += posOffTime[i].ToString() + "\t";
                content += posCorrectness[i].ToString() + "\t";

                sw.WriteLine(content);
            }
        }
    }
}

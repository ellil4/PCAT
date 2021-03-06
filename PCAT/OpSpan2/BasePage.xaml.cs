﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibTabCharter;
using System.Diagnostics;

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// page class of OpSpan2
    /// </summary>
    public partial class BasePage : Page
    {
        public MainWindow mMainWindow;
        public static int[] mTestScheme = { 2, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 ,9, 9};
        public static int[] mPracScheme = {2};
        public static int[] mMemPracScheme = { 2, 3 };
        
        public int mCurSchemeAt = 0;
        public int mCurInGrpAt = 0;
        public int mExeDidCount = 0;

        public List<List<string>> mMemPrac;//2, 3
        public bool mSecondMemPrac = false;
        public List<StEquation> mInterPrac;
        public List<StTrailGroupOS> mComprehPrac;
        public bool mSecondComprehPrac = false;
        public List<StTrailGroupOS> mTest;
        public bool mSecondFormal = false;


        public Stage mStage;
        public RecorderOpSpan2 mRecorder;

        public Stopwatch mTimeline;
        public long mInterTimeLimit = 2000;

        private bool mSchemeReturned = false;
        private bool mSchemeIterated = false;

        public SECOND_ARCHI_TYPE ARCTYPE;

        public BasePage(MainWindow mw, SECOND_ARCHI_TYPE tp)
        {
            InitializeComponent();

            ARCTYPE = tp;

            mMainWindow = mw;
            mRecorder = new RecorderOpSpan2(this);
            
            //load 1
            mMemPrac = new List<List<string>>();
            loadMemOrderPractise();
            //load 2
            mInterPrac = new List<StEquation>();
            loadEquationPractise();
            loadComprehPractise();
            loadTest();

            mTimeline = new Stopwatch();
        }

        public int GetGroupAtInSpan(int posInScheme, int[] scheme)
        {
            int retval = -1;
            int thisSize = scheme[posInScheme];
            int firstThisSize = -1;

            for (int i = 0; i < scheme.Length; i++)
            {
                if (scheme[i] == thisSize)
                {
                    firstThisSize = i;
                    break;
                }
            }

            if(firstThisSize != -1)
                retval = posInScheme - firstThisSize;

            return retval;
        }

        public bool SchemeReturned()
        {
            return mSchemeReturned;
        }

        public bool SchemeIterated()
        {
            return mSchemeIterated;
        }

        public void ResetSchemeIterationStatus()
        {
            mSchemeIterated = false;
        }

        private void resetSchemeStatus()
        {
            mSchemeReturned = false;
            mSchemeIterated = false;
        }

        public void DoCursorIteration()
        {
            mCurInGrpAt++;
            switch(mStage)
            {
                case Stage.MemPrac:
                    if (mCurInGrpAt == mMemPracScheme[mCurSchemeAt])
                    {
                        mCurInGrpAt = 0;
                        mCurSchemeAt++;
                        mSchemeIterated = true;

                        if (mCurSchemeAt == mMemPracScheme.Length)
                        {
                            mCurSchemeAt = 0;
                            mSchemeReturned = true;
                        }
                    }
                    break;
                case Stage.ComprehPrac:
                    if (mCurInGrpAt == mPracScheme[mCurSchemeAt])
                    {
                        mCurInGrpAt = 0;
                        mCurSchemeAt = 0;
                        mSchemeIterated = true;
                        mSchemeReturned = true;
                    }
                    break;
                case Stage.Formal:
                    if (mCurInGrpAt == mTestScheme[mCurSchemeAt])
                    {
                        mCurInGrpAt = 0;
                        mCurSchemeAt++;
                        mSchemeIterated = true;

                        if (mCurSchemeAt == mTestScheme.Length)
                        {
                            mCurSchemeAt = 0;
                            mSchemeReturned = true;
                        }
                    }
                    break;
            }
            
        }

        public bool IfGroupPassed(int tarIndex, int[] scheme)
        {
            bool retval = false;
            bool interPassed = false;
            int targetSpanLen = scheme[tarIndex];
            
            int intersCountBefore = 0;
            for (int i = 0; i < tarIndex; i++)
            {
                intersCountBefore += scheme[i];
            }

            int choiceOff = -1, orderOff = -1;
            switch (mStage)
            {
                case Stage.ComprehPrac:
                    choiceOff = intersCountBefore;
                    orderOff = 0;
                    break;
                case Stage.Formal:
                    choiceOff = intersCountBefore + mExeDidCount * BasePage.mPracScheme[0];
                    orderOff = mExeDidCount;
                    break;
            }

            int rightCount = 0;
            for (int j = 0; j < targetSpanLen; j++)
            {
                if (mRecorder.choice[j + choiceOff] ==
                    mRecorder.correctness[j + choiceOff].ToString())
                    rightCount++;
            }

            float rightCountLimf = (float)targetSpanLen * 0.66666667f;
            int rightCountLimI = (int)Math.Round(rightCountLimf);
            if (rightCount >= rightCountLimI)
                interPassed = true;

            if (mRecorder.rightOrder[tarIndex + orderOff].Equals(
                mRecorder.userInputOrder[tarIndex + orderOff]) &&
                interPassed)
                retval = true;

            return retval;
        }

        private void loadEquationPractise()
        {
            TabFetcher fetcher = null;
            if (ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                fetcher = new TabFetcher(
                    FEITStandard.GetExePath() + "OP\\opspanbaseline.txt", "\\t");
            }
            else if (ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                fetcher = new TabFetcher(
                    FEITStandard.GetExePath() + "SYMM\\fixedSymmExeBaseline.txt", "\\t");
            }
            fetcher.Open();

            fetcher.GetLineBy();//skip header
            List<String> line;
            while ((line = fetcher.GetLineBy()).Count != 0)
            {
                StEquation equ = new StEquation();
                equ.Equation = line[1];
                equ.Result = Int32.Parse(line[2]);
                if (Int32.Parse(line[3]) == 1)
                {
                    equ.Answer = true;
                }
                else
                {
                    equ.Answer = false;
                }

                mInterPrac.Add(equ);
            }

            fetcher.Close();
        }

        private void loadMemOrderPractise()
        {
            TabFetcher fetcher = null;
            if (ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                fetcher = new TabFetcher(FEITStandard.GetExePath() + "OP\\opspanword.txt", "\\t");
            }
            else if(ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                fetcher = new TabFetcher(FEITStandard.GetExePath() + "SYMM\\locationExe.txt", "\\t");
            }


            fetcher.Open();
            fetcher.GetLineBy();//skip header

            for (int i = 0; i < mMemPracScheme.Length; i++)
            {
                List<string> group = new List<string>();
                for (int j = 0; j < mMemPracScheme[i]; j++)
                {
                    List<string> line = fetcher.GetLineBy();
                    group.Add(line[2]);
                }
                mMemPrac.Add(group);
            }
        }

        private void loadComprehPractise()
        {
            if (ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                mComprehPrac =
                    readFixedFromFile(
                    FEITStandard.GetExePath() + "OP\\opspan.txt", mPracScheme, 1);
            }
            else if (ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                mComprehPrac =
                    readFixedFromFile(
                    FEITStandard.GetExePath() + "SYMM\\fixedItems.txt", mPracScheme, 1);                    
            }
        }

        private void loadTest()
        {
            if (ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                mTest =
                    readFixedFromFile(
                    FEITStandard.GetExePath() + "OP\\opspan.txt", mTestScheme, 3);
            }
            else if(ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                mTest =
                    readFixedFromFile(
                    FEITStandard.GetExePath() + "SYMM\\fixedItems.txt", mTestScheme, 3);
            }
        }

        private List<StTrailGroupOS> readFixedFromFile(string path, int[] scheme, int begFromLine)
        {
            TabFetcher fet = new TabFetcher(path, "\\t");

            fet.Open();

            for (int k = 0; k < begFromLine; k++)
                fet.GetLineBy();//skip lines

            List<StTrailGroupOS> groups = new List<StTrailGroupOS>();
            //practise
            for (int i = 0; i < scheme.Length; i++)
            {
                StTrailGroupOS group = new StTrailGroupOS();
                for (int j = 0; j < scheme[i]; j++)
                {
                    List<String> line = fet.GetLineBy();
                    StTrailOS_ST st = new StTrailOS_ST();
                    st.equation = line[1];
                    st.result = line[2];
                    if (Int32.Parse(line[3]) == 1)
                    {
                        st.correctness = true;
                    }
                    else
                    {
                        st.correctness = false;
                    }
                    st.memTarget = line[4];
                    st.equationLevel = line[9];
                    group.mTrails.Add(st);
                }
                groups.Add(group);
            }
            fet.Close();
            return groups;
        }

        public void ClearAll()
        {
            amBaseCanvas.Children.Clear();
        }

        private void amCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amBaseCanvas);
            //systest
            ShowTitle();
            //ShowInstructionEquationPrac();
            //ShowInstructionComprehPrac();
            //ShowInstructionFormal();
            mTimeline.Start();
        }

        public void ProgressReturn()
        {
            mCurSchemeAt = 0;
            mCurInGrpAt = 0;
            resetSchemeStatus();
        }

        private long getInterTimeLimit()
        {
            if (mRecorder.mathPracRTs.Count > 0)
            {
                long retval = 0;
                long midval = 0;
                List<long> truezRTs = new List<long>();
                for (int i = 0; i < mRecorder.mathPracRTs.Count; i++)
                {
                    if (mRecorder.mathPracEquations[i].Answer.ToString() ==
                        mRecorder.mathPracAnswers[i])
                    {
                        midval += mRecorder.mathPracRTs[i];
                        truezRTs.Add(mRecorder.mathPracRTs[i]);
                    }
                }

                if (truezRTs.Count > 0)
                {

                    midval = midval / truezRTs.Count;
                    midval *= 2;

                    int finalPickedValueCount = 0;

                    for (int j = 0; j < truezRTs.Count; j++)
                    {
                        if (truezRTs[j] <= midval)
                        {
                            retval += truezRTs[j];
                            finalPickedValueCount++;
                        }
                    }

                    retval /= finalPickedValueCount;
                    retval *= 2;

                    return retval;
                }
                else
                {
                    return 2000;
                }
            }
            else
            {
                return 2000;
            }
        }

        public void ShowBoard(object obj)
        {
            ClearAll();
            UserControl uc = (UserControl)obj;
            amBaseCanvas.Children.Add(uc);
            Canvas.SetTop(uc, FEITStandard.SCREEN_EDGE_Y);
            Canvas.SetLeft(uc, FEITStandard.SCREEN_EDGE_X);
        }

        public void ShowBoardAnimal()
        {
            BoardAnimal ba =
                new BoardAnimal(this);

            ShowBoard(ba);
            ba.Run();
        }

        public void ShowTitle()
        {
            ProgressReturn();
            mStage = Stage.MemPrac;
            BoardTitle bt = new BoardTitle(this);
            ShowBoard(bt);
        }

        public void ShowInstructionEquationPrac()
        {
            ProgressReturn();
            mStage = Stage.InterPrac;
            if (ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                BoardInstructionEquationPractise biep =
                    new BoardInstructionEquationPractise(this);

                ShowBoard(biep);
            }
            else if(ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                ShowSymmInst0();
            }
        }

        public void ShowSymmInst0()
        {
            BoardSymmInst0 bsi0 = new BoardSymmInst0(this);
            ShowBoard(bsi0);
        }

        public void ShowSymmInst1()
        {
            BoardSymmInst1 bsi1 = new BoardSymmInst1(this);
            ShowBoard(bsi1);
        }

        /*public void ShowSymmInst2()
        {
            BoardSymmInst2 bsi2 = new BoardSymmInst2(this);
            ShowBoard(bsi2);
        }*/

        public void ShowInstructionComprehPrac()
        {
            ProgressReturn();
            mInterTimeLimit = getInterTimeLimit();
            mStage = Stage.ComprehPrac;
            BoardInstructionComprehPrac bic = new BoardInstructionComprehPrac(this);

            ShowBoard(bic);
        }

        public void ShowInstructionFormal()
        {
            ProgressReturn();
            mStage = Stage.Formal;
            BoardInstructionFormal bif = new BoardInstructionFormal(this);

            ShowBoard(bif);
        }

        public string GetAnimalPracRealOrder(int schemeID)
        {
            string retval = "";
            for (int i = 0; i < mMemPrac[schemeID].Count; i++)
            {
                retval += mMemPrac[schemeID][i];
                if (ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    retval += ",";
            }
            return retval;
        }

        public string GetComprehPracAnimalRealOrder()
        {
            string retval = "";
            for (int i = 0; i < mComprehPrac[0].mTrails.Count; i++)
            {
                retval += mComprehPrac[0].mTrails[i].memTarget;
                if (ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    retval += ",";
            }
            return retval;
        }

        public string GetFormalAnimalRealOrder(int schemeID2Check)
        {
            string retval = "";
            for (int i = 0; i < mTest[schemeID2Check].mTrails.Count; i++)
            {
                retval += mTest[schemeID2Check].mTrails[i].memTarget;
                if (ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
                    retval += ",";
            }
            return retval;
        }

        public void ShowOrderSelectPage()
        {
            BoardOrderOP boo = null;
            if (mStage == Stage.MemPrac)
            {
                boo = new BoardOrderOP(this, mSecondMemPrac);
            }
            else if (mStage == Stage.ComprehPrac)
            {
                boo = new BoardOrderOP(this, mSecondComprehPrac);
            }
            else if (mStage == Stage.Formal)
            {
                boo = new BoardOrderOP(this, mSecondFormal);
            }
            ShowBoard(boo);
        }

        public void ShowEquationPage()
        {
            BoardEquation be = new BoardEquation(this);
            ShowBoard(be);
        }

        public void ShowEquationJudgePage()
        {
            BoardEquationJudge bej = null;
            if(mStage == Stage.InterPrac)
            {
                bej = new BoardEquationJudge(this);
            }
            else if (mStage == Stage.ComprehPrac)
            {
                bej = new BoardEquationJudge(this);
            }
            else if (mStage == Stage.Formal)
            {
                bej = new BoardEquationJudge(this);
            }

            ShowBoard(bej);
        }

        public void ShowGroupTitle()
        {
            BoardGroupTitle bgt = new BoardGroupTitle(this);
            ShowBoard(bgt);
            bgt.Run();
        }



        void outputData()
        {
            string interFilename = "inter_" + mMainWindow.mDemography.GenBriefString() + ".txt";
            string orderFilename = "order_" + mMainWindow.mDemography.GenBriefString() + ".txt";
            string pracMathFilename = "pracMath_" + mMainWindow.mDemography.GenBriefString() + ".txt";
            string pracOrderFilename = "pracOrder_" + mMainWindow.mDemography.GenBriefString() + ".txt";

            if (ARCTYPE == SECOND_ARCHI_TYPE.OPSPAN)
            {
                mRecorder.outputReport(FEITStandard.GetRepotOutputPath() + "op\\" + interFilename,
                        FEITStandard.GetRepotOutputPath() + "op\\" + orderFilename,
                        FEITStandard.GetRepotOutputPath() + "op\\" + pracMathFilename,
                        FEITStandard.GetRepotOutputPath() + "op\\" + pracOrderFilename);
            }
            else if (ARCTYPE == SECOND_ARCHI_TYPE.SYMMSPAN)
            {
                mRecorder.outputReport(FEITStandard.GetRepotOutputPath() + "symm\\" + interFilename,
                    FEITStandard.GetRepotOutputPath() + "symm\\" + orderFilename,
                    FEITStandard.GetRepotOutputPath() + "symm\\" + pracMathFilename,
                    FEITStandard.GetRepotOutputPath() + "symm\\" + pracOrderFilename);
            }
        }

        public void ShowFinishPage(object obj)
        {
            outputData();
            BoardFinish bf = new BoardFinish(this);
            ShowBoard(bf);
        }
    }

    public enum Stage
    {
        MemPrac, InterPrac, ComprehPrac, Formal
    }
}

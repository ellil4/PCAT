using System;
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

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// page class of OpSpan2
    /// </summary>
    public partial class BasePage : Page
    {
        public MainWindow mMainWindow;
        public static int[] mTestScheme = {2, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8 ,9, 9, 10, 10};
        public static int[] mPracScheme = {2};
        public static int[] mAnimalPracScheme = { 2, 3 };
        
        public int mCurSchemeAt = 0;
        public int mCurInGrpAt = 0;

        public List<List<string>> mAnimalPrac;//2, 3
        public bool mSecondAnimalPrac = false;
        public List<StEquation> mEquationPrac;

        public Stage mStage;
        public RecorderOpSpan2 mRecorder;

        public BasePage(MainWindow mw)
        {
            InitializeComponent();

            mMainWindow = mw;
            mRecorder = new RecorderOpSpan2(this);
            
            //load 1
            mAnimalPrac = new List<List<string>>();
            loadAnimalOrderPractise();
            //load 2
            mEquationPrac = new List<StEquation>();
            loadEquationPractise();
        }

        public int GetGroupAt(int posInScheme, int[] scheme)
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
            if (mCurInGrpAt == 0 && mCurSchemeAt == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SchemeIterated()
        {
            if (mCurInGrpAt == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DoCursorIteration()
        {
            mCurInGrpAt++;
            switch(mStage)
            {
                case Stage.AnimalPrac:
                    if (mCurInGrpAt == mAnimalPracScheme[mCurSchemeAt])
                    {
                        mCurInGrpAt = 0;
                        mCurSchemeAt++;

                        if (mCurSchemeAt == mAnimalPracScheme.Length)
                            mCurSchemeAt = 0;
                    }
                    break;
                case Stage.ComprehPrac:
                    if (mCurInGrpAt == mPracScheme[mCurSchemeAt])
                    {
                        mCurInGrpAt = 0;
                        mCurSchemeAt = 0;
                    }
                    break;
                case Stage.Formal:
                    if (mCurInGrpAt == mTestScheme[mCurSchemeAt])
                    {
                        mCurInGrpAt = 0;
                        mCurSchemeAt++;

                        if (mCurSchemeAt == mTestScheme.Length)
                            mCurSchemeAt = 0;
                    }
                    break;
            }
            
        }

        private void loadEquationPractise()
        {
            TabFetcher fetcher =
                    new TabFetcher(
                        FEITStandard.GetExePath() + "OP\\opspanbaseline.txt", "\\t");
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

                mEquationPrac.Add(equ);
            }

            fetcher.Close();
        }

        private void loadAnimalOrderPractise()
        {
            TabFetcher fetcher =
                new TabFetcher(FEITStandard.GetExePath() + "OP\\opspanword.txt", "\\t");

            fetcher.Open();
            fetcher.GetLineBy();//skip header

            for (int i = 0; i < mAnimalPracScheme.Length; i++)
            {
                List<string> group = new List<string>();
                for (int j = 0; j < mAnimalPracScheme[i]; j++)
                {
                    List<string> line = fetcher.GetLineBy();
                    group.Add(line[2]);
                }
                mAnimalPrac.Add(group);
            }
        }

        public void ClearAll()
        {
            amBaseCanvas.Children.Clear();
        }

        private void amCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amBaseCanvas);
            ShowTitle(null);
        }

        public void ShowBoard(object obj)
        {
            ClearAll();
            UserControl uc = (UserControl)obj;
            amBaseCanvas.Children.Add(uc);
            Canvas.SetTop(uc, FEITStandard.SCREEN_EDGE_Y);
            Canvas.SetLeft(uc, FEITStandard.SCREEN_EDGE_X);
        }

        public void ShowBoardAnimal(object obj)
        {
            BoardAnimal ba =
                new BoardAnimal(this, mAnimalPrac[mCurSchemeAt][mCurInGrpAt]);

            ShowBoard(ba);
            ba.Run();
        }

        public void ShowTitle(object obj)
        {
            BoardTitle bt = new BoardTitle(this);
            ShowBoard(bt);
        }

        public void ShowInstructionAnimalPrac(object obj)
        {
            mStage = Stage.AnimalPrac;
            BoardInstructionAnimalPrac biap = new BoardInstructionAnimalPrac(this);
            ShowBoard(biap);
        }

        public void ShowInstructionEquationPrac(object obj)
        {
            mStage = Stage.EquationPrac;
            BoardInstructionEquationPractise biep = 
                new BoardInstructionEquationPractise(this);

            ShowBoard(biep);
        }

        public string GetAnimalPracRealOrder(int schemeID)
        {
            string retval = "";
            for (int i = 0; i < mAnimalPrac[schemeID].Count; i++)
            {
                retval += mAnimalPrac[schemeID][i];
            }
            return retval;
        }

        public void ShowOrderSelectPage(object obj)
        {
            BoardOrderOP boo = new BoardOrderOP(this);
            ShowBoard(boo);
        }

        public void ShowEquationPage(object obj)
        {
            BoardEquation be = new BoardEquation(this);
            ShowBoard(be);
        }

        public void SHowEquationJudgePage(object obj)
        {
            BoardEquationJudge bej = new BoardEquationJudge(this);
            ShowBoard(bej);
        }
    }

    public enum Stage
    {
        AnimalPrac, EquationPrac, ComprehPrac, Formal
    }
}

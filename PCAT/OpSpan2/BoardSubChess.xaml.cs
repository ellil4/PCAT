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
using System.Text.RegularExpressions;

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// BoardSubChess.xaml 的互動邏輯
    /// </summary>
    public partial class BoardSubChess : UserControl
    {
        public List<ElementChess> mElems;
        public bool mEditable = true;
        public string mAnswer = "";
        public BoardOrderOP mBoardOrder;

        public BoardSubChess()
        {
            InitializeComponent();
            Init();
        }

        public void SetParent(BoardOrderOP bo)
        {
            mBoardOrder = bo;
        }

        public void Init()
        {
            mElems = new List<ElementChess>();
            mElems.Add(elementChess1);
            mElems.Add(elementChess2);
            mElems.Add(elementChess3);
            mElems.Add(elementChess4);
            mElems.Add(elementChess5);
            mElems.Add(elementChess6);
            mElems.Add(elementChess7);
            mElems.Add(elementChess8);
            mElems.Add(elementChess9);
            mElems.Add(elementChess10);
            mElems.Add(elementChess11);
            mElems.Add(elementChess12);
            mElems.Add(elementChess13);
            mElems.Add(elementChess14);
            mElems.Add(elementChess15);
            mElems.Add(elementChess16);

            for (int i = 0; i < mElems.Count; i++)
            {
                mElems[i].SetNum("");
                mElems[i].HideDot();
                mElems[i].ID = i;
                mElems[i].MouseUpEventFunc = add2Answer;
            }
        }

        public void ShowDot(int index)
        {
            mElems[index].ShowDot();
        }

        public void ClearAllShown()
        {
            for (int i = 0; i < mElems.Count; i++)
            {
                mElems[i].HideDot();
            }
        }

        public string GetAnswerStr()
        {
            return mAnswer;
        }

        private bool answerStringContainsID(string num)
        {
            bool retval = false;
            Regex rex = new Regex("[^,]+");
            MatchCollection mc = rex.Matches(mAnswer);
            for (int i = 0; i < mc.Count; i++)
            {
                if (mc[i].Value.Equals(num))
                {
                    retval = true;
                    break;
                }
            }
            return retval;
        }

        private void add2Answer(ElementChess sender)
        {
            if (mEditable && mBoardOrder.mCur < mBoardOrder.mLen && !answerStringContainsID(sender.ID.ToString()))
            {
                mAnswer += sender.ID.ToString() + ",";
                mElems[sender.ID].ShowDot();
                mElems[sender.ID].SetNum((mBoardOrder.mCur + 1).ToString());
                mBoardOrder.mCur++;
            }
        }

        private int getLastAnswerIndex()
        {
            Regex rex = new Regex("[^,]+");
            MatchCollection mc = rex.Matches(mAnswer);
            return Int32.Parse(mc[mc.Count - 1].Value);
        }

        public void ClearOne()
        {
            int idx = getLastAnswerIndex();
            mElems[idx].HideDot();
            mElems[idx].SetNum("");
            mAnswer = mAnswer.Remove(mAnswer.Length - 1);//remove comma
            while (!String.IsNullOrEmpty(mAnswer) && mAnswer[mAnswer.Length - 1] != ',')
            {
                mAnswer = mAnswer.Remove(mAnswer.Length - 1);
            }
        }
    }
}

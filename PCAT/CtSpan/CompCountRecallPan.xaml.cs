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

namespace FiveElementsIntTest.CtSpan
{
    /// <summary>
    /// CountRecallPan.xaml 的互動邏輯
    /// </summary>
    public partial class CompCountRecallPan : UserControl
    {
        //? == -1; 0 == uninitialized; 1-9 == 1-9
        public List<int> mAnswers;

        public CompCountRecallPan()
        {
            InitializeComponent();
            mAnswers = new List<int>();
        }

        public void Clear()
        {
            mAnswers.Clear();
            refresh();
        }

        private void refresh()
        {
            String text2show = "";
            for(int i = 0; i < mAnswers.Count; i++)
            {
                if(i != 0)
                    text2show += "-";

                if(mAnswers[i] != -1)
                {
                    text2show += mAnswers[i].ToString();
                }
                else
                {
                    text2show += "?";
                }
            }

            amTextBlock.Text = text2show;
        }

        private void amBtn1_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(1);
                refresh();
            }
        }

        private void amBtn2_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(2);
                refresh();
            }
        }

        private void amBtn3_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(3);
                refresh();
            }
        }

        private void amBtn4_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(4);
                refresh();
            }
        }

        private void amBtn5_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(5);
                refresh();
            }
        }

        private void amBtn6_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(6);
                refresh();
            }
        }

        private void amBtn7_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(7);
                refresh();
            }
        }

        private void amBtn8_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(8);
                refresh();
            }
        }

        private void amBtn9_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(9);
                refresh();
            }
        }

        private void amBtnErase_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count - 1 >= 0)
            {
                mAnswers.RemoveAt(mAnswers.Count - 1);
                refresh();
            }
        }

        private void amBtnEmpty_Click(object sender, RoutedEventArgs e)
        {
            if (mAnswers.Count < 9)
            {
                mAnswers.Add(-1);
                refresh();
            }
        }

        public delegate void DeleConfirmOp();
        public DeleConfirmOp mfConfirmOp = null;

        private void amBtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            mfConfirmOp();
        }
    }
}

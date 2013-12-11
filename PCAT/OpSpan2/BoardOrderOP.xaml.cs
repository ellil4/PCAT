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
using System.Timers;

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// BoardOrderOP.xaml 的互動邏輯
    /// </summary>
    public partial class BoardOrderOP : UserControl
    {
        List<Label> mAnimalButtons;

        public BasePage mBasePage;

        public BoardOrderOP(BasePage bp)
        {
            InitializeComponent();
            mAnimalButtons = new List<Label>();

            mBasePage = bp;

            init();
            register();

        }

        void init()
        {
            mAnimalButtons.Add(amAni1);
            mAnimalButtons.Add(amAni2);
            mAnimalButtons.Add(amAni3);
            mAnimalButtons.Add(amAni4);
            mAnimalButtons.Add(amAni5);
            mAnimalButtons.Add(amAni6);
            mAnimalButtons.Add(amAni7);
            mAnimalButtons.Add(amAni8);
            mAnimalButtons.Add(amAni9);
            mAnimalButtons.Add(amAni10);
            mAnimalButtons.Add(amAni11);
            mAnimalButtons.Add(amAni12);

            amTBAnswer.Text = "";
            amQuesLabel.Visibility = System.Windows.Visibility.Hidden;
        }

        void register()
        {
            for (int i = 0; i < mAnimalButtons.Count; i++)
            {
                mAnimalButtons[i].MouseUp += new MouseButtonEventHandler(Animal_MouseUp);
            }

            amAniQues.MouseUp += new MouseButtonEventHandler(amAniQues_MouseUp);
        }

        void amAniQues_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (amTBAnswer.Text.Length < 10)
            {
                amTBAnswer.Text += ((Label)sender).Content;
            }
        }

        void Animal_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!amTBAnswer.Text.Contains(((Label)sender).Content.ToString()) && 
                amTBAnswer.Text.Length < 10)
            {
                ((Label)sender).BorderBrush =
                    new SolidColorBrush(Color.FromRgb(255, 119, 0));
                amTBAnswer.Text += ((Label)sender).Content;
 
            }
        }

        private void amBtnClear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            amTBAnswer.Text = "";

            for(int i = 0; i < mAnimalButtons.Count; i++)
            {
                mAnimalButtons[i].BorderBrush = 
                    new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        private void amAniQues_MouseEnter(object sender, MouseEventArgs e)
        {
            amQuesLabel.Visibility = System.Windows.Visibility.Visible;
        }

        private void amAniQues_MouseLeave(object sender, MouseEventArgs e)
        {
            amQuesLabel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void amBtnConfirm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (mBasePage.mStage)
            {
                case Stage.AnimalPrac:
                    string realOrder = mBasePage.GetAnimalPracRealOrder(mBasePage.mCurSchemeAt);
                    string userOrder = amTBAnswer.Text;

                    mBasePage.mRecorder.mPracOrderRealOrder.Add(realOrder);
                    mBasePage.mRecorder.mPracOrderAnswers.Add(userOrder);

                    if (realOrder.Equals(userOrder))
                    {
                        mBasePage.mRecorder.mPracOrderCorrectness.Add(true);

                        amTBNotice.Text = "正确";
                        amTBNotice.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                        amTBNotice.Visibility = System.Windows.Visibility.Visible;

                        Timer out2Equa = new Timer();
                        out2Equa.Interval = 500;
                        out2Equa.AutoReset = false;
                        out2Equa.Elapsed += new ElapsedEventHandler(out2Equa_Elapsed);
                        out2Equa.Enabled = true;
                        
                    }
                    else
                    {
                        mBasePage.mRecorder.mPracOrderCorrectness.Add(false);

                        amTBNotice.Visibility = System.Windows.Visibility.Visible;

                        Timer outAsWrong = new Timer();
                        outAsWrong.Interval = 1500;
                        outAsWrong.AutoReset = false;
                        outAsWrong.Elapsed += new ElapsedEventHandler(outAsWrong_Elapsed);
                        outAsWrong.Enabled = true;
                    }
                    break;
                case Stage.ComprehPrac:
                    break;
                case Stage.Formal:
                    break;
            }
        }

        void out2Equa_Elapsed(object sender, ElapsedEventArgs e)
        {
            //go base line test
        }

        void outAsWrong_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!mBasePage.mSecondAnimalPrac)
            {
                mBasePage.mSecondAnimalPrac = true;
                mBasePage.mCurInGrpAt = 0;
                mBasePage.ShowBoardAnimal(null);
            }
            else
            {
                //go base line test

            }
        }
    }
}

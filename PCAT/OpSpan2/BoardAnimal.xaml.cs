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
    /// BoardAnimal.xaml 的互動邏輯
    /// </summary>
    public partial class BoardAnimal : UserControl
    {
        string mContent;
        BasePage mBasePage;

        public delegate void TimeDele();

        public BoardAnimal(BasePage page, string content)
        {
            InitializeComponent();
            mBasePage = page;
            mContent = content;
            label1.Content = content;
        }

        public void Run()
        {
            Timer t = new Timer();
            t.Interval = 1000;
            t.AutoReset = false;
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mBasePage.Dispatcher.Invoke(new TimeDele(nexBlankMask));
        }

        void nexBlankMask()
        {
            mBasePage.ClearAll();
            Timer tb = new Timer();
            tb.Interval = 250;
            tb.AutoReset = false;
            tb.Elapsed += new ElapsedEventHandler(tb_Elapsed_go_out);
            tb.Enabled = true;
        }

        void tb_Elapsed_go_out(object sender, ElapsedEventArgs e)
        {
            switch (mBasePage.mStage)
            {
                case Stage.AnimalPrac:
                    
                    mBasePage.DoCursorIteration();

                    if (!mBasePage.SchemeIterated())//
                    {
                        mBasePage.ShowBoardAnimal(null);
                    }
                    else//Scheme Iterated
                    {
                        mBasePage.ShowOrderSelectPage(null);
                    }

                    break;
                case Stage.ComprehPrac:
                    mBasePage.DoCursorIteration();

                    if (!mBasePage.SchemeIterated())//
                    {
                        mBasePage.ShowEquationPage(null);
                    }
                    else//Scheme Iterated
                    {
                        mBasePage.ShowOrderSelectPage(null);
                    }
                    break;
                case Stage.Formal:
                    mBasePage.DoCursorIteration();

                    if (!mBasePage.SchemeIterated())//
                    {
                        mBasePage.ShowEquationPage(null);
                    }
                    else//Scheme Iterated
                    {
                        mBasePage.ShowOrderSelectPage(null);
                    }
                    break;
            }
        }
    }
}

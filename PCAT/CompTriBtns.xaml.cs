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
using FiveElementsIntTest.OpSpan;

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompTriBtns.xaml 的互動邏輯
    /// </summary>
    public partial class CompTriBtns : UserControl
    {
        public static int OUTWIDTH = 430;
        public static int OUTHEIGHT = 100;

        public NumCheckReaction mConfirmMethod;
        public NumCheckReactionI mClearMethod;
        public NumCheckReaction mBlankMethod;

        public CompTriBtns()
        {
            InitializeComponent();
        }

        private void borderBlank_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //DoReaction(mBlankMethod);
            mBlankMethod();
        }

        private void borderClear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mClearMethod();
        }

        private void borderConfirm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mConfirmMethod();
        }

        public delegate void NumCheckReaction();
        public delegate int NumCheckReactionI();
        
    }
}

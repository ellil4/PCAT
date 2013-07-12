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
    /// GraphConfirmBtn.xaml 的互動邏輯
    /// </summary>
    public partial class GraphConfirmBtn : UserControl
    {
        public delegate void DoFunc();
        public DoFunc mfDo;

        public GraphConfirmBtn()
        {
            InitializeComponent();
            amImage.Opacity = 0.5;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mfDo();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            amImage.Opacity = 1.0;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            amImage.Opacity = 0.5;
        }
    }
}

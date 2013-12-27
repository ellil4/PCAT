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

namespace FiveElementsIntTest.OpSpan2
{
    /// <summary>
    /// ElementChess.xaml 的互動邏輯
    /// </summary>
    public partial class ElementChess : UserControl
    {
        public int ID = -1;
        public delegate void DeleMouseUp(ElementChess sender);

        public DeleMouseUp MouseUpEventFunc;

        public ElementChess()
        {
            InitializeComponent();
        }

        public void SetNum(string num)
        {
            amNum.Content = num;
        }

        public string GetNum()
        {
            return amNum.Content.ToString();
        }

        public void ShowDot()
        {
            amDot.Visibility = System.Windows.Visibility.Visible;
        }

        public void HideDot()
        {
            amDot.Visibility = System.Windows.Visibility.Hidden;
        }

        public bool IsDotShown()
        {
            if (amDot.Visibility == System.Windows.Visibility.Visible)
                return true;
            else
                return false;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUpEventFunc((ElementChess)sender);
        }
    }
}

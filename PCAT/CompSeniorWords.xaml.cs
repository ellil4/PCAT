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

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompSeniorWords.xaml 的互動邏輯
    /// </summary>
    public partial class CompSeniorWords : UserControl
    {

        public bool isSelected = false;

        public CompSeniorWords()
        {
            InitializeComponent();
            MouseOver(false);
            SetSelected(false);
            amImage.Visibility = System.Windows.Visibility.Hidden;
        }

        public void SetText(String content)
        {
            amLabel.Content = content;
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                amBorder.Visibility = Visibility.Visible;
                isSelected = true;
            }
            else
            {
                amBorder.Visibility = Visibility.Hidden;
                isSelected = false;
            }
        }

        public void MouseOver(bool isOver)
        {
            if (isOver)
            {
                amBorder.Visibility = Visibility.Visible;
            }
            else 
            {
                if(!isSelected)
                    amBorder.Visibility = Visibility.Hidden;
            }
        }
    }
}

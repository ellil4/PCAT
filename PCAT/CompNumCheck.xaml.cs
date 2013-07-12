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
    /// CompNumCheck.xaml 的互動邏輯
    /// </summary>
    public partial class CompNumCheck : UserControl
    {
        protected UIGroupNumChecks mParent;
        protected int mID;
        protected bool mClicked = false;

        public CompNumCheck(String txt, UIGroupNumChecks parent, int id)
        {
            InitializeComponent();
            
            mParent = parent;
            mID = id;
            amDigiLabel.Content = "";
            amTxtLabel.Content = txt;
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mParent.onAction(mID);
        }

        public void setOderDigit(int order)
        {
            amDigiLabel.Content = order.ToString();
        }

        public void setUnClicked()
        {
            mClicked = false;
        }

        public void setClicked()
        {
            mClicked = true;
        }

        public bool clicked()
        {
            return mClicked;
        }
    }
}

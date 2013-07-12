using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FiveElementsIntTest.SymSpan
{
    class CompNumCheckSS : CompNumCheck
    {
        public static int OUTWIDTH = 64;
        public static int OUTHEIGHT = 64;
        public static Thickness DEF_MARGIN_THICKNESS = new Thickness(13, 2, 0, 0);

        public CompNumCheckSS(String txt, UIGroupNumChecks parent, int id) : 
            base(txt, parent, id)
        {
            amTxtLabel.Visibility = Visibility.Collapsed;
            this.Width = OUTWIDTH;
            this.Height = OUTHEIGHT;
            this.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            this.BorderThickness = new Thickness(2.0);
            //this.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            amDigiLabel.Width = OUTWIDTH;//40;
            amDigiLabel.Height = OUTHEIGHT;//62;
            //amDigiLabel.BorderThickness = new Thickness(1.0);
            //amDigiLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 55, 0));
            amDigiLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            amDigiLabel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            amDigiLabel.FontSize = 44;
            amDigiLabel.Padding = new Thickness(0);
            amDigiLabel.Margin = DEF_MARGIN_THICKNESS;

            amBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }
    }
}

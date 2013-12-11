using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;


namespace FiveElementsIntTest.Cube
{
    class bordercontrol
    {
        
      public bordercontrol()
        {
           
        }


      public Border GenBorder(int _width, int _Height) //border属性
        {
            Border retval = new Border();
            retval.Height = _Height;
            retval.Width = _width;
            retval.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            retval.BorderThickness = new Thickness(1.0);
            retval.Visibility = Visibility.Hidden;
            
            return retval;
        }
        

    }
}

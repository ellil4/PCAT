using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FiveElementsIntTest
{
    class PageCommon
    {
        //canvas, auxiliary line
        public static void InitCommonPageElements(ref Canvas cvs)
        {
            cvs.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            cvs.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        public static void AddAuxBDR(ref Canvas cvs, ref Border bdr, ref Border bdrLarge, ref MainWindow mw)
        {
            if (mw.mbEngiMode)
            {
                cvs.Children.Add(bdr);
                cvs.Children.Add(bdrLarge);

                Canvas.SetTop(bdr, FEITStandard.PAGE_BEG_Y);
                Canvas.SetLeft(bdr, FEITStandard.PAGE_BEG_X);

                Canvas.SetTop(bdrLarge, FEITStandard.SCREEN_EDGE_Y);
                Canvas.SetLeft(bdrLarge, FEITStandard.SCREEN_EDGE_X);
                bdrLarge.BorderBrush = new SolidColorBrush(Color.FromRgb(50, 50, 50));

            }
        }
    }
}

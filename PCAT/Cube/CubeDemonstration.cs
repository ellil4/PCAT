using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace FiveElementsIntTest.Cube
{
    public class CubeDemonstration
    {
        private PageCube mPage;
        Image DemL;
        public CubeDemonstration(PageCube pgCube)
        {
            mPage = pgCube;
             DemL = new Image();
           DemL.Width= 1000;
           DemL.Height = 680;
           BitmapImage bdem = new BitmapImage();
           bdem.BeginInit();
           bdem.UriSource = new Uri("/PCAT;component/Res/Cube示例.JPG", UriKind.Relative);
           bdem.EndInit();
           DemL.Stretch = Stretch.Fill;
           DemL.Source = bdem;
           mPage.mBaseCanvas.Children.Add(DemL);
           Canvas.SetLeft(DemL, FEITStandard.PAGE_BEG_X-100);
           Canvas.SetTop(DemL, FEITStandard.PAGE_BEG_Y-10);

        }
        public void clear()
        {
            mPage.mBaseCanvas.Children.Remove(DemL);
        }
    }
}

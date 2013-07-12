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
using System.Drawing;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.CtSpan
{
    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class GraphicToken : UserControl
    {
        public int mX = -1, mY = -1;
        public TokenType mType;
        GraphControl mControl;
        public int HALF_HEIGHT = -1;
        public int HALF_WIDHT = -1;
        public int mClickCount = 0;

        public GraphicToken(TokenType type, GraphControl control)
        {
            InitializeComponent();

            mType = type;
            mControl = control;

            Bitmap bmp = null;

            switch (type)
            {
                case TokenType.DARKCIRCLE:
                    bmp = FiveElementsIntTest.Properties.Resources.CSDarkblueCircle;
                    break;
                case TokenType.LIGHTCIRCLE:
                    bmp = FiveElementsIntTest.Properties.Resources.CSLightblueCircle;
                    break;
                case TokenType.TRIANGLE:
                    bmp = FiveElementsIntTest.Properties.Resources.CSTriangle;
                    break;
            }

            amImage.Source = 
            System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), 
                IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            
            HALF_WIDHT = bmp.Width / 2;
            HALF_HEIGHT = bmp.Height / 2;

            setAsIdle();
        }

        public void SetPos(int x, int y)
        {
            mX = x;
            mY = y;
        }

        private void setAsMouseOver()
        {
            BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(92, 92, 92));
            BorderThickness = new Thickness(1);
        }

        private void setAsIdle()
        {
            BorderThickness = new Thickness(0);
        }

        private void setAsClicked()
        {
            BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            BorderThickness = new Thickness(2);
        }

        private delegate void clickDelayDele();

        private void clickElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(
                DispatcherPriority.Normal, new clickDelayDele(setAsIdle));
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mControl.DotOnTape(mX, mY, mType, 0);
            setAsClicked();

            Timer T = new Timer();
            T.Elapsed +=new ElapsedEventHandler(clickElapsed);
            T.Interval = 500;
            T.AutoReset = false;
            T.Enabled = true;
            //mark as clicked
            mClickCount++;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            setAsMouseOver();
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            setAsIdle();
        }
    }
}

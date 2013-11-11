using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Drawing;
using FiveElementsIntTest.ITFigure;
using System.Timers;
using System.Windows.Threading;


namespace FiveElementsIntTest
{
    public class FEITCentralFlipper
    {
        private PageITFigure mPage;
        private Canvas mCanvas;

        private finishDelegate mFinFunc;

        private List<Bitmap> mStepBmp;
        private List<long> mStepInterval;
        private int mStepNum = 0;

        private IntPtr mIntPtr = IntPtr.Zero;

        public FEITCentralFlipper(ref Canvas _canvas, PageITFigure _page)
        {
            mCanvas = _canvas;
            mPage = _page;
        }

        public Point getCentraloGraphBegin(ref BitmapSource src)
        {
            Point ret = new Point();

            double grpBegX = FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - src.Width) / 2;
            double grpBegY = FEITStandard.PAGE_BEG_Y + (FEITStandard.PAGE_HEIGHT - src.Height) / 2;

            ret.X = (int)grpBegX;
            ret.Y = (int)grpBegY;

            return ret;
        }

        public void Show(BitmapSource source)
        {
            mPage.amCentralImage.Source = source;
            mPage.amCentralImage.Width = source.Width;
            mPage.amCentralImage.Height = source.Height;
        }

        public void CentralShow(BitmapSource source, int yOff)
        {
            System.Drawing.Point pt = getCentraloGraphBegin(ref source);
            Canvas.SetTop(mPage.amCentralImage, pt.Y + yOff);
            Canvas.SetLeft(mPage.amCentralImage, pt.X);

            Show(source);
        }

        public void SetupStepLink(List<Bitmap> bmps, List<long> intervals,
            finishDelegate finFunc, startDelegate stopFunc)
        {
            mStepBmp = bmps;
            mStepInterval = intervals;
            mStepNum = 0;

            mFinFunc = finFunc;
            stopFunc();
        }

        private delegate void invokeDelegate();

        public delegate void startDelegate();
        public delegate void finishDelegate();

        void timeTrigger(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new invokeDelegate(DoStepLink));
        }

        public void DoStepLink()
        {
            if (mStepNum < mStepBmp.Count)
            {
                if (mIntPtr != IntPtr.Zero)
                    BitmapSourceFactory.DeleteObject(mIntPtr);

                CentralShow(BitmapSourceFactory.GetBitmapSource(mStepBmp[mStepNum], out mIntPtr), 0);
                Timer t = new Timer();
                t.Interval = mStepInterval[mStepNum];
                t.AutoReset = false;
                t.Enabled = true;
                t.Elapsed += new ElapsedEventHandler(timeTrigger);
                mStepNum++;
            }
            else
            {
                mFinFunc();
            }
        }

        ~FEITCentralFlipper()
        {
            if (mIntPtr != IntPtr.Zero)
                BitmapSourceFactory.DeleteObject(mIntPtr);
        }
    }
}

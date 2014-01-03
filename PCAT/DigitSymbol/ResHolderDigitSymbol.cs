using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using PCATData;

namespace FiveElementsIntTest
{
    public class ResHolderDigitSymbol
    {
        protected BitmapSource mImageO;
        protected BitmapSource mImageX;
        protected BitmapSource mImageBar;
        protected BitmapSource mImageArrow;

        public ResHolderDigitSymbol()
        {
            Bitmap bitmapO = FiveElementsIntTest.Properties.Resources.rightimg;//替换原来的circle
            mImageO = 
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmapO.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmapO.Width, bitmapO.Height));

            Bitmap bitmapX = FiveElementsIntTest.Properties.Resources.X;
            mImageX = 
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmapX.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmapX.Width, bitmapX.Height));

            Bitmap bitmapBar = FiveElementsIntTest.Properties.Resources.BAR;
            mImageBar =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmapBar.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmapBar.Width, bitmapBar.Height));

            Bitmap bitmapArrow = FiveElementsIntTest.Properties.Resources.arrow;
            mImageArrow = 
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmapArrow.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmapArrow.Width, bitmapArrow.Height));
        }

        public BitmapSource GetImage(SYMBOL_TYPE type)
        {
            BitmapSource ret = null;

            switch (type)
            {
                case SYMBOL_TYPE.BAR:
                    ret = mImageBar;
                    break;
                case SYMBOL_TYPE.O:
                    ret = mImageO;
                    break;
                case SYMBOL_TYPE.X:
                    ret = mImageX;
                    break;
            }

            return ret;
        }

        public BitmapSource GetArrowSource()
        {
            return mImageArrow;
        }
    }
}

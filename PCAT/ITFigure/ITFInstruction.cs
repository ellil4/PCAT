using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.ITFigure
{
    class ITFInstruction
    {
        private PageITFigure mPage;
        private IntPtr mIntPtr = IntPtr.Zero;

        public ITFInstruction(PageITFigure page)
        {
            mPage = page;
        }

        ~ITFInstruction()
        {
            if (mIntPtr != IntPtr.Zero)
                BitmapSourceFactory.DeleteObject(mIntPtr);
        }

        public void Show()
        {
            CompCentralText cct = new CompCentralText();
            CompCentralText cct2 = new CompCentralText();
            CompCentralText cct3 = new CompCentralText();
            CompCentralText cct4 = new CompCentralText();

            int yOff = -240;
            int lineGap = 0;
            int lineOff = 60;

            if (mIntPtr != IntPtr.Zero)
                BitmapSourceFactory.DeleteObject(mIntPtr);

            mPage.mFlipper.CentralShow(
                BitmapSourceFactory.GetBitmapSource(
                FiveElementsIntTest.Properties.Resources.ITF_EXAMPLE, out mIntPtr), 50);

            cct.PutTextToCentralScreen("      请对屏幕中央闪现的左右两条竖线进行辨别，",
                "Microsoft YaHei", 36, ref mPage.amBaseCanvas,
                 yOff,
                System.Windows.Media.Color.FromRgb(220, 220, 220));
            cct2.PutTextToCentralScreen("注意哪条竖线更长。本测验无速度要求，请认真",
                "Microsoft YaHei", 36, ref mPage.amBaseCanvas,
                 yOff + lineOff + lineGap,
                System.Windows.Media.Color.FromRgb(220, 220, 220));
            cct3.PutTextToCentralScreen("辨别,并等待掩蔽刺激消失后，按相应键作答。   ",
                "Microsoft YaHei", 36, ref mPage.amBaseCanvas,
                 yOff + lineOff * 2 + lineGap,
                System.Windows.Media.Color.FromRgb(220, 220, 220));
            cct4.PutTextToCentralScreen("按空格键继续",
                "Microsoft YaHei", 36, ref mPage.amBaseCanvas,
                 220 + lineGap,
                System.Windows.Media.Color.FromRgb(220, 220, 220));
        }
    }
}

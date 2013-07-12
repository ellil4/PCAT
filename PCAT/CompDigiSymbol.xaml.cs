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
using PCATData;

namespace FiveElementsIntTest
{
    /// <summary>
    /// CompDigiSymbol.xaml 的互動邏輯
    /// </summary>
    /// 

    public enum CURSOR_STATUS
    {
        NONE, LEFT, RIGHT
    }

    public partial class CompDigiSymbol : UserControl
    {
        protected PageDigitSymbol mPage;
        public static int OUTWIDTH = 81;
        public static int OUTHEIGHT = 100;

        private SYMBOL_TYPE mLeftSymbol;
        private SYMBOL_TYPE mRightSymbol;

        public CompDigiSymbol(PageDigitSymbol _page)
        {
            InitializeComponent();
            mPage = _page;
        }

        public CompDigiSymbol(char digit, SYMBOL_TYPE first, SYMBOL_TYPE second, PageDigitSymbol page)
        {
            InitializeComponent();
            mPage = page;
            setContent(digit, first, second);
        }

        public void setContent(char digit, SYMBOL_TYPE first, SYMBOL_TYPE second)
        {
            TxtDigi.Content = digit;
            amInputImage1.Source = mPage.mResHolder.GetImage(first);
            mLeftSymbol = first;
            amInputImage2.Source = mPage.mResHolder.GetImage(second);
            mRightSymbol = second;
        }

        public void fillAnswer(SYMBOL_TYPE content, bool fieldLeft)
        {
            if (fieldLeft == true)
            {
                amInputImage1.Source = mPage.mResHolder.GetImage(content);
                mLeftSymbol = content;
            }
            else
            {
                amInputImage2.Source = mPage.mResHolder.GetImage(content);
                mRightSymbol = content;
            }
        }

        public SYMBOL_TYPE GetLeft()
        {
            return mLeftSymbol;
        }

        public SYMBOL_TYPE GetRight()
        {
            return mRightSymbol;
        }

        public int GetNumber()
        {
            return int.Parse((TxtDigi.Content.ToString()));
        }

        public void SetCursor(CURSOR_STATUS status)
        {
            switch (status)
            {
                case CURSOR_STATUS.NONE:
                    amCursorLeft.Source = null;
                    amCursorRight.Source = null;
                    break;
                case CURSOR_STATUS.LEFT:
                    amCursorLeft.Source = mPage.mResHolder.GetArrowSource();
                    amCursorRight.Source = null;
                    break;
                case CURSOR_STATUS.RIGHT:
                    amCursorLeft.Source = null;
                    amCursorRight.Source = mPage.mResHolder.GetArrowSource();
                    break;
            }
        }
    }
}

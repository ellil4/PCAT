using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;


namespace FiveElementsIntTest
{
    public class LayoutInstruction
    {
        Canvas mBaseCanvas;

        public LayoutInstruction(ref Canvas _canvas)
        {
            mBaseCanvas = _canvas;
        }

        public void addTitle(int yOff, int xOff, String content, 
            String fontFamily, int size, Color color, bool ifBold = false)
        {
            Label title = new Label();
            title.Content = content;
            int titleLen = title.Content.ToString().Length;
            title.Foreground = new SolidColorBrush(color);
            title.FontSize = size;
            if (ifBold)
            {
                title.FontWeight = 
                    (FontWeight)System.ComponentModel.TypeDescriptor.GetConverter(
                    typeof(FontWeight)).ConvertFromString("Bold");
            }
            title.FontFamily = new FontFamily(fontFamily);

            mBaseCanvas.Children.Add(title);
            Canvas.SetTop(title, FEITStandard.PAGE_BEG_Y + yOff);
            Canvas.SetLeft(title, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - title.FontSize * titleLen) / 2 + xOff);

            title.Focusable = false;
        }

        public void addInstruction(int yOff, int xOff, int width, int height,
            String content, String fontFamily, int size, Color color, bool ifBold = false)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.AppendText(content);
            rtb.Width = width;
            rtb.Height = height;
            rtb.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            rtb.Foreground = new SolidColorBrush(color);
            rtb.HorizontalAlignment = HorizontalAlignment.Center;

            rtb.BorderThickness = new Thickness(0);
            rtb.FontSize = size;
            rtb.FontFamily = new FontFamily(fontFamily);
            rtb.IsReadOnly = true;
            
            if(ifBold)
                rtb.FontWeight = FontWeights.Bold;

            mBaseCanvas.Children.Add(rtb);
            Canvas.SetTop(rtb, FEITStandard.PAGE_BEG_Y + yOff);
            Canvas.SetLeft(rtb, FEITStandard.PAGE_BEG_X + 
                (FEITStandard.PAGE_WIDTH - rtb.Width) / 2 + xOff);

            rtb.Focusable = false;
        }
    }
}

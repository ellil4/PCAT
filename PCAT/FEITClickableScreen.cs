using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Timers;

namespace FiveElementsIntTest
{
    class FEITClickableScreen
    {
        public delegate void clickableAreaReaction();

        clickableAreaReaction mfReaction;
        Timer mt = null;

        public FEITClickableScreen(ref Canvas canvas, clickableAreaReaction _reaction)
        {
            mfReaction = _reaction;
            addClickableArea(ref canvas);
        }

        public FEITClickableScreen(ref Canvas canvas, clickableAreaReaction _reaction, ref Timer t)
        {
            mt = t;
            mfReaction = _reaction;
            addClickableArea(ref canvas);
        }

        public void addClickableArea(ref Canvas canvas)
        {
            TextBlock tb = new TextBlock();
            tb.Height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            tb.Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;

            canvas.Children.Add(tb);
            Canvas.SetTop(tb, 0);
            Canvas.SetLeft(tb, 0);

            tb.MouseUp += new System.Windows.Input.MouseButtonEventHandler(tb_MouseUp);
        }

        void tb_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (mt != null)
            {
                mt.Stop();
            }

            mfReaction();
        }
    }
}

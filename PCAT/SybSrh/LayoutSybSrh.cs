using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace FiveElementsIntTest.SybSrh
{
    public class LayoutSybSrh
    {
        private PageSybSrh mPage;
        public static int COMP_SIDE_LEN = 100;
        public static int HORIZONTAL_GAP = 40;

        public LayoutSybSrh(PageSybSrh pg)
        {
            mPage = pg;
        }

        public void SetSelectionLayout()
        {
            mPage.clearAll();

            for (int i = 0; i < 7; i++)
            {
                CompImage ci = mPage.mImages[i];
                mPage.amCanvas.Children.Add(ci);

                if (i < 2)
                {
                    Canvas.SetLeft(ci, 
                        FEITStandard.PAGE_BEG_X +
                        (FEITStandard.PAGE_WIDTH - COMP_SIDE_LEN * 2 - HORIZONTAL_GAP) / 
                        2 + i * (COMP_SIDE_LEN + HORIZONTAL_GAP));

                    Canvas.SetTop(ci, FEITStandard.PAGE_BEG_Y + 125);
                }
                else if (i >= 2 && i < 7)
                {
                    Canvas.SetLeft(ci,
                        FEITStandard.PAGE_BEG_X +
                        (FEITStandard.PAGE_WIDTH - COMP_SIDE_LEN * 5 - HORIZONTAL_GAP * 3) / 
                        2 + (i - 2) * (COMP_SIDE_LEN + HORIZONTAL_GAP));

                    Canvas.SetTop(ci, FEITStandard.PAGE_BEG_Y + 350);
                }
                /*else if (i >= 6 && i < 10)
                {
                    Canvas.SetLeft(ci,
                        FEITStandard.PAGE_BEG_X +
                        (FEITStandard.PAGE_WIDTH - COMP_SIDE_LEN * 4 - HORIZONTAL_GAP * 3) / 
                        2 + (i - 6) * (COMP_SIDE_LEN + HORIZONTAL_GAP));

                    Canvas.SetTop(ci, FEITStandard.PAGE_BEG_Y + 400);
                }*/
            }

            Rectangle rect = new Rectangle();
            rect.Height = 2;
            rect.Width = 600;
            rect.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            rect.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            mPage.amCanvas.Children.Add(rect);
            Canvas.SetTop(rect, FEITStandard.PAGE_BEG_Y + 275);
            Canvas.SetLeft(rect, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - rect.Width) / 2);
        }

        public void SetInstructionLayout()
        {
            mPage.clearAll();
            PageTitle pt = new PageTitle();
            mPage.amCanvas.Children.Add(pt);
            Canvas.SetLeft(pt, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(pt, FEITStandard.PAGE_BEG_Y);
        }

        public void SetInstructionLayout2()
        {
            mPage.clearAll();
            PageTitle2 pt = new PageTitle2();
            mPage.amCanvas.Children.Add(pt);
            Canvas.SetLeft(pt, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(pt, FEITStandard.PAGE_BEG_Y);
        }

        public void SetInstructionLayout3()
        {
            mPage.clearAll();
            PageTitle3 pt = new PageTitle3();
            mPage.amCanvas.Children.Add(pt);
            Canvas.SetLeft(pt, FEITStandard.PAGE_BEG_X);
            Canvas.SetTop(pt, FEITStandard.PAGE_BEG_Y);
        }

        public void SetReport(long RT, float CorrectRate)
        {
            mPage.clearAll();
            LayoutInstruction instruction = new LayoutInstruction(ref mPage.amCanvas);
            if (RT != -1)
            {
                instruction.addInstruction(250, 0, 800, 300,
                    "    你的平均反应时是：" + RT.ToString() + "毫秒;你的正确率是：" +
                    (CorrectRate * 100).ToString("0.0") + "%,按空格键继续。",
                    "KaiTi", 38, System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
            else
            {
                instruction.addTitle(175, 0, "你没有输入,按空格键继续下一组", "KaiTi", 38,
                    System.Windows.Media.Color.FromRgb(255, 255, 255), false);
            }
        }

        public void SetEndPage()
        {
            mPage.clearAll();

            LayoutInstruction instruction = new LayoutInstruction(ref mPage.amCanvas);

            instruction.addTitle(190, 0, "测试结束，程序将自动退出", "KaiTi", 44,
                System.Windows.Media.Color.FromRgb(255, 255, 255), false);
        }
    }
}

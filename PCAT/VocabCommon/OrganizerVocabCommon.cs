using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FiveElementsIntTest.VocabCommon
{
    public class OrganizerVocabCommon
    {
        PageVocabCommon mPage;
        static int HEIGHT_SELECTION = 60;
        static int HEAD_SPACE = 200;

        public OrganizerVocabCommon(PageVocabCommon _page)
        {
            mPage = _page;
        }

        private void addMainLabel()
        {
            mPage.amCanvas.Children.Add(mPage.mMainLabel);
            Canvas.SetLeft(mPage.mMainLabel, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - mPage.mMainLabel.Width) / 2);
            Canvas.SetTop(mPage.mMainLabel, FEITStandard.PAGE_BEG_Y +
                (HEAD_SPACE - mPage.mMainLabel.Height) / 2 + 50); 
        }

        private void addSelections()
        {
            for (int i = 0; i < 5; i++)
            {
                mPage.amCanvas.Children.Add(mPage.mSelections[i]);

                Canvas.SetTop(mPage.mSelections[i],
                    HEAD_SPACE + FEITStandard.PAGE_BEG_Y + i * (HEIGHT_SELECTION));

                Canvas.SetLeft(mPage.mSelections[i],
                    FEITStandard.PAGE_BEG_X +
                    (FEITStandard.PAGE_WIDTH - mPage.mSelections[i].Width) / 2);

            }
        }

        private void addBtn()
        {

            mPage.amCanvas.Children.Add(mPage.mNextBtn);
            Canvas.SetLeft(mPage.mNextBtn, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - mPage.mNextBtn.Width) / 2);
            Canvas.SetTop(mPage.mNextBtn, FEITStandard.PAGE_BEG_Y +
                (HEAD_SPACE + HEIGHT_SELECTION * 5 + 20));
        }

        public void arrangeLayout()
        {
            addMainLabel();
            addSelections();
            addBtn();
        }

        public void EndPage()
        {
            mPage.clearAll();
            mPage.mMainLabel.Text = "测试结束，程序将自动退出";
            mPage.mMainLabel.Width = 800;
            addMainLabel();
            
        }

        public void StartPage()
        {
            mPage.clearAll();
            /*mPage.mMainLabel.Text = "点击鼠标左键开始测试";
            mPage.mMainLabel.Width = 800;
            addMainLabel();
            Canvas.SetTop(mPage.mMainLabel, FEITStandard.PAGE_BEG_Y + 220); 
            new FEITClickableScreen(ref mPage.amCanvas, mPage.TestStart);*/

            PageTitle title = new PageTitle(mPage);

            if (mPage.mTestType == TestType.Similarity)
            {
                title.textBlock1.Text = "    请思考每次呈现的两个词语有什么相似的地方，选出你认为最准确的选项。";
                title.label1.Content = "类同";
                title.label2.Content = "Similarities";
            }

            mPage.amCanvas.Children.Add(title);
            Canvas.SetTop(title, FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(title, FEITStandard.PAGE_BEG_X);
        }
    }
}

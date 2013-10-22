using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Drawing;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Forms;

namespace FiveElementsIntTest.GraphicRecog
{
    public class LayoutGFR
    {
        private PageGraphicRecog mPage;
        private static int INTERX = 27;

        private int mTestTitlteCount = 1;
        private int mLearnTitleCount = 1;

        public LayoutGFR(PageGraphicRecog page)
        {
            mPage = page;
        }

        public void LaySelectionPage()
        {
            mPage.clearAll();

            //target
            mPage.amCanvas.Children.Add(mPage.mTargetImage);
            Canvas.SetTop(mPage.mTargetImage, (double)FEITStandard.PAGE_BEG_Y);
            Canvas.SetLeft(mPage.mTargetImage,
                (double)FEITStandard.PAGE_BEG_X + (FEITStandard.PAGE_WIDTH - CompImage.SIZE) / 2);

            //selections
            for (int i = 0; i < 4; i++)
            {
                mPage.amCanvas.Children.Add(mPage.mImageGroup.mImages[i]);
                Canvas.SetTop(mPage.mImageGroup.mImages[i],
                    FEITStandard.PAGE_BEG_Y + CompImage.SIZE + 100);
                Canvas.SetLeft(mPage.mImageGroup.mImages[i],
                    FEITStandard.PAGE_BEG_X + i * (INTERX + CompImage.SIZE) + INTERX);
            }

            for (int i = 0; i < 4; i++)
            {
                mPage.amCanvas.Children.Add(mPage.mImageGroup.mImages[i + 4]);
                Canvas.SetTop(mPage.mImageGroup.mImages[i + 4],
                    FEITStandard.PAGE_BEG_Y + CompImage.SIZE * 2 + 100);
                Canvas.SetLeft(mPage.mImageGroup.mImages[i + 4],
                    FEITStandard.PAGE_BEG_X + i * (INTERX + CompImage.SIZE) + INTERX);
            }
        }

        public void UpdateSelectionPage(int pageTotalIndex)
        {
            try
            {
                mPage.mTargetImage.SetGraph(
                    new Bitmap(FEITStandard.BASE_FOLDER + PageGraphicRecog.TEST_FOLDER +
                        mPage.mDoList[pageTotalIndex * GRFormReader.IN_TEST_PAGE]));

                for (int i = 0; i < UIImageGroup.SELCOUNT; i++)
                {
                    String addr = FEITStandard.BASE_FOLDER + PageGraphicRecog.TEST_FOLDER +
                        mPage.mDoList[pageTotalIndex * GRFormReader.IN_TEST_PAGE + i + 1];

                    mPage.mImageGroup.mImages[i].SetGraph(new Bitmap(addr));
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("读取资源文件时发生错误或试题文件缺损") == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
        }

        public void LayLearningPage()
        {
            mPage.clearAll();

            mPage.amCanvas.Children.Add(mPage.mLearnImage1);
            Canvas.SetLeft(mPage.mLearnImage1, FEITStandard.PAGE_BEG_X + 
                (FEITStandard.PAGE_WIDTH - CompImage.SIZE * 2 - 100) / 2);
            Canvas.SetTop(mPage.mLearnImage1, FEITStandard.PAGE_BEG_Y +
                (FEITStandard.PAGE_HEIGHT - CompImage.SIZE) / 2);

            mPage.amCanvas.Children.Add(mPage.mDoubleArrow);
            Canvas.SetLeft(mPage.mDoubleArrow, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - mPage.mDoubleArrow.Width) / 2);
            Canvas.SetTop(mPage.mDoubleArrow, FEITStandard.PAGE_BEG_Y +
                (FEITStandard.PAGE_HEIGHT - mPage.mDoubleArrow.Height) / 2);

            mPage.amCanvas.Children.Add(mPage.mLearnImage2);
            Canvas.SetLeft(mPage.mLearnImage2, FEITStandard.PAGE_BEG_X +
                (FEITStandard.PAGE_WIDTH - CompImage.SIZE * 2 - 100) / 2 + 100 + CompImage.SIZE);
            Canvas.SetTop(mPage.mLearnImage2, FEITStandard.PAGE_BEG_Y +
                (FEITStandard.PAGE_HEIGHT - CompImage.SIZE) / 2);
        }

        public void UpdateLearningPage(int learningPageIndex)
        {
            try
            {
                mPage.mLearnImage1.SetGraph(
                    new Bitmap(
                        FEITStandard.BASE_FOLDER + PageGraphicRecog.TEST_FOLDER +
                        mPage.mLearnList[learningPageIndex * GRFormReader.IN_LEARNING_PAGE]));

                mPage.mLearnImage2.SetGraph(
                    new Bitmap(
                        FEITStandard.BASE_FOLDER + PageGraphicRecog.TEST_FOLDER +
                        mPage.mLearnList[learningPageIndex * GRFormReader.IN_LEARNING_PAGE + 1]));
            }
            catch (Exception)
            {
                if (MessageBox.Show("读取资源文件时发生错误或指导文件缺损") == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
        }

        public void LayEndPage()
        {
            mPage.clearAll();
            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen(
                "测试结束，程序将自动退出", "Microsoft YaHei", 50 ,ref mPage.amCanvas, 0, 
                System.Windows.Media.Color.FromRgb(255, 255, 255));
        }

        public void LayStartPage()
        {
            mPage.clearAll();

            LayoutInstruction li = new LayoutInstruction(ref mPage.amCanvas);
            li.addTitle(40, 0, "图形联想记忆", "KaiTi", 50, System.Windows.Media.Color.FromRgb(255, 255, 255));
            li.addInstruction(200, 0, 800, 300, 
                "    待会儿你会看见一些图形对，左边图形是一个小动物 (十二生肖)，右边是一个无意义图形。你需要把两个图形联系起来进行记忆，" + 
                "在学习完这些图形对后我们将进行记忆测验。准备好了吗？点击鼠标继续。", "KaiTi", 40, 
                System.Windows.Media.Color.FromRgb(255, 255, 255));

            FEITClickableScreen fcs = 
                new FEITClickableScreen(ref mPage.amCanvas, LayStartPage2);
        }

        private void laySingleLineInstruction(String text)
        {
            mPage.clearAll();
            CompCentralText cct = new CompCentralText();
            cct.PutTextToCentralScreen(
                text, "KaiTi", 50, ref mPage.amCanvas, 0,
                System.Windows.Media.Color.FromRgb(255, 255, 255));
        }

        public void LayStartPage2()
        {
            laySingleLineInstruction("下面请注意记！");

            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed_start);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        private delegate void systemThreadDelegate();

        private void t_Elapsed_start(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new systemThreadDelegate(mPage.TestStart));
        }

        public void t_Elapsed_test_next(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new systemThreadDelegate(LaySelectionPage));
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new systemThreadDelegate(mPage.Next));
        }

        public void t_Elapsed_learn_next(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new systemThreadDelegate(LayLearningPage));
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new systemThreadDelegate(mPage.Next));
        }

        public void LayTestTitle()
        {
            switch (mTestTitlteCount)
            { 
                case 1:
                laySingleLineInstruction("记忆测试（一）");
                break;
                case 2:
                laySingleLineInstruction("记忆测试（二）");
                break;
                case 3:
                laySingleLineInstruction("记忆测试（三）");
                break;
            }

            mTestTitlteCount++;

            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed_test_next);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
        }

        public void LayLearnTitle()
        {
            switch (mLearnTitleCount)
            {
                case 1:
                    laySingleLineInstruction("下面再记忆一遍！");
                    break;
                case 2:
                    laySingleLineInstruction("下面记忆第三遍！");
                    break;
            }

            mLearnTitleCount++;

            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed_learn_next);
            t.Interval = 3000;
            t.AutoReset = false;
            t.Enabled = true;
        }
    }
}

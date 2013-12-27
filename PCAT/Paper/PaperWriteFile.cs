using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;



namespace FiveElementsIntTest.Paper
{
    class PaperWriteFile
    {

        public List<List<String>> mLines_paper;//存放一套测试结果
        List<String> temporary_list;//开辟空间 写文件
        List<String> resline; //为写文件服务
        TabCharter mTabCharter;//写入硬盘
        String file_Loca = ""; //系统时间文件地址
        String titletext = "题号";
        List<String> head_list = new List<string>();
        PagePaper cPagePaper;
        MainWindow mMainWindow;


        public PaperWriteFile(PagePaper _PagePaper, MainWindow mw)
        {
            cPagePaper = _PagePaper;
            mMainWindow = mw;
            file_Loca = "Report\\Paper_test_result\\" + mMainWindow.mDemography.GenBriefString() + ".txt";
            mTabCharter = new TabCharter(file_Loca);//写入硬盘
            mLines_paper = new List<List<string>>();
            resline = new List<string>();
            
        }

        public void mLines_Head(List<String> list)
        {
            for (int i = 0; i < list.Count; i++)
                head_list.Add(list[i]);
            mLines_paper.Add(head_list);
        }





        public void mLines_Record(int num, List<String> list)
        {
            temporary_list = new List<String>();//开辟空间

            mLines_paper.Add(temporary_list);

            for (int i = 0; i < list.Count; i++)
            {
                mLines_paper[num + 1].Add(list[i]);
            }

        }



        public void outPutresult()
        {

            head_list.Insert(0, titletext);
            if (cPagePaper.line_num_count >= cPagePaper.line_num - 1 || cPagePaper.wr_thre)
            {
                for (int i = 0; i < mLines_paper.Count; i++)
                {

                    resline.Clear();
                    for (int j = 0; j < mLines_paper[i].Count; j++)//
                    {
                        resline.Add(mLines_paper[i][j]);
                    }
                    if (i == 0) mTabCharter.Create(resline);
                    else
                        mTabCharter.Append(resline);

                }
            }
        }



    }//---------类
}//空间名


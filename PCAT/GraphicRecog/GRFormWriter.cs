using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace FiveElementsIntTest.GraphicRecog
{
    public class GRFormWriter
    {
        private PageGraphicRecog mPage;
        public void Save(PageGraphicRecog page)
        {
            StreamWriter sw;
            mPage = page;

            String folder = FEITStandard.BASE_FOLDER + PageGraphicRecog.TEST_FOLDER ;
            if(!Directory.Exists(folder))
            {
                if (MessageBox.Show("输出路径错误") == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            
            String outpath = FEITStandard.BASE_FOLDER + PageGraphicRecog.TEST_FOLDER + "Record.csv";
            if(!File.Exists(outpath))
            {
                sw = File.CreateText(outpath);
                writeHeader(ref sw);
            }
            else
            {
                sw = new StreamWriter(outpath, true, Encoding.GetEncoding("gb2312"));
            }

            writeRecord(ref sw);

            sw.Close();
        }

        private void writeHeader(ref StreamWriter sw)
        {
            String content = "subject,";

            for(int o = 0; o < GRFormReader.TESTSPAN_COUNT; o++)
            {
                for(int i = 0; i < GRFormReader.TESTSPAN_PAGE_COUNT; i++)
                {
                    content += "item" + i + ",";
                    content += "corectness,";
                    content += "RT,";
                }

                content += "correct count" + ",";
            }

            content.Remove(content.Length - 1);
            sw.WriteLine(content);
        }

        private void writeRecord(ref StreamWriter sw)
        {
            String content = "S" + FEITStandard.GetStamp() + ",";

            for (int i = 0; i < mPage.mUserAnswer.Count; i++)
            {
                content += mPage.mUserAnswer[i] + ",";
            }

            content.Remove(content.Length - 1);
            sw.WriteLine(content);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FiveElementsIntTest.GraphicRecog
{
    public class GRFormReader
    {
        public static int IN_TEST_PAGE = 10;//1 target + 8 selections + 1 Answer
        public static int TESTSPAN_PAGE_COUNT = 12;
        public static int TESTSPAN_COUNT = 3;
        public static int TOTAL_ITEM = TESTSPAN_PAGE_COUNT * TESTSPAN_COUNT;

        public static int IN_LEARNING_PAGE = 2;
        public static int LEARNINGSPAN_PAGE_COUNT = 12;//12

        public enum SCRIPT_TYPE
        {
            TEST, LEARNING
        };

        public static List<String> GetList(String path, SCRIPT_TYPE type)
        {
            StreamReader rd = null;
            List<String> retval = new List<String>();

            try
            {
                rd = File.OpenText(path);
                String lineContent;
                Regex rex = new Regex("[^,]+");

                rd.ReadLine();//header

                //pages
                while((lineContent = rd.ReadLine()) != null)
                {
                    MatchCollection mts = rex.Matches(lineContent);

                    //selections
                    int upper = 0;

                    if (type == SCRIPT_TYPE.LEARNING)
                    {
                        upper = IN_LEARNING_PAGE;
                    }
                    else
                    {
                        upper = IN_TEST_PAGE;
                    }

                    for (int i = 0; i < upper; i++)
                    {
                        retval.Add(mts[i].Value);
                    }
                }
                
            }
            catch (Exception)
            {
                if (MessageBox.Show("测试文件读取错误") == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            finally
            {
                if(rd != null)
                    rd.Close();
            }

            return retval;
        }
    }
}

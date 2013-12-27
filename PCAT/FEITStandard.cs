using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCATData;

namespace FiveElementsIntTest
{
    public enum TestType
    {
        DigitSymbol, SymbolSearch, OpSpan, SymSpan, CtSpan,
        VocAsso, GraphAsso, Paper, Cube, Vocabulary, Similarity, PortraitMemory, OpSpan2, SymmSpan2
    }

    public class FEITStandard
    {
        public static String[] TEST_TITLE = { "数字符号", "符号搜索", "操作广度", 
                                        "对称广度", "计数广度", "词对联想", "图对联想", 
                                        "折纸测验", "魔方旋转", "词汇测验", "类同测验","人像特点联系回忆", 
                                        "操作广度2", "对称广度2"};

        public static int PAGE_WIDTH = 800;
        public static int PAGE_HEIGHT = 600;
        public static int EDGE_WIDTH = 1024;
        public static int EDGE_HEIGHT = 768;

        public static String BASE_FOLDER = 
            System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public static int PAGE_BEG_X = 
            ((int)System.Windows.SystemParameters.PrimaryScreenWidth) / 2 
            - PAGE_WIDTH / 2;
        public static int PAGE_BEG_Y = 
            ((int)System.Windows.SystemParameters.PrimaryScreenHeight) / 2
            - PAGE_HEIGHT / 2 - 50;
        public static int SCREEN_EDGE_X = ((int)System.Windows.SystemParameters.PrimaryScreenWidth) / 2
            - EDGE_WIDTH / 2;
        public static int SCREEN_EDGE_Y = ((int)System.Windows.SystemParameters.PrimaryScreenHeight / 2
            -EDGE_HEIGHT / 2 - 50);

        public static string GetRepotOutputPath()
        {
            return GetExePath() + "Report\\";
        }

        public static string GetExePath()
        {
            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        public static SYMBOL_TYPE[] SYMBOL_LEFT =
            new SYMBOL_TYPE[] 
            { SYMBOL_TYPE.NONE, SYMBOL_TYPE.BAR, SYMBOL_TYPE.X, SYMBOL_TYPE.O, SYMBOL_TYPE.O, 
                SYMBOL_TYPE.BAR, SYMBOL_TYPE.X, SYMBOL_TYPE.X, SYMBOL_TYPE.O, SYMBOL_TYPE .BAR};

        public static SYMBOL_TYPE[] SYMBOL_RIGHT =
            new SYMBOL_TYPE[] 
            { SYMBOL_TYPE.NONE, SYMBOL_TYPE.BAR, SYMBOL_TYPE.O, SYMBOL_TYPE.X, SYMBOL_TYPE.O, 
                SYMBOL_TYPE.O, SYMBOL_TYPE.X, SYMBOL_TYPE.BAR, SYMBOL_TYPE.BAR, SYMBOL_TYPE.X };

        public enum FEIT_SIGNAL
        {
            CONFIRM, BACKSPACE, BLANK, DENY
        };

        public static String GetStamp()
        {
            DateTime dt = System.DateTime.Now;
            String str = "";

            str += dt.Year.ToString()+"-";
            str += dt.Month.ToString() + "-";
            str += dt.Day.ToString() + "-";
            str += dt.Hour.ToString() + "-";
            str += dt.Minute.ToString() + "-";
            str += dt.Second.ToString();

            return str;
        }

        public static int GetTestIndexByName(String name)
        {
            int retval = -1;

            for (int i = 0; i < FEITStandard.TEST_TITLE.Length; i++)
            {
                if (name.Equals(FEITStandard.TEST_TITLE[i]))
                {
                    retval = i;
                    break;
                }
            }

            return retval;
        }
    }

    public enum SECOND_ARCHI_TYPE
    {
        OPSPAN, SYMMSPAN
    };
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    public class Names
    {
        public static String DIGIT_SYMBOL_TABLENAME = "DIGIT_SYMBOL";
        public static String[] DIGIT_SYMBOL_COLNAME = 
            { "Stim", "ItemLeft", "RTLeft", "ItemRight", "RTRight", "Correctness" };

        public static String SYMBOL_SEARCH_TABLENAME = "SYMBOL_SEARCH";
        public static String[] SYMBOL_SEARCH_COLNAME = { "Stim", "Answer", "RT", "Correctness" };

        public static String OPSPAN_ORDER_TABLENAME = "OPSPAN_ORDER";
        public static String[] OPSPAN_ORDER_COLNAME = { "StimOrder", "UserOrder", "RT", 
                                                      "Correctness", "GroupID", "SubGroupID"};

        public static String OPSAPN_EXPRESSION_TABLENAME = "OPSPAN_EXPRESSION";
        public static String[] OPSPAN_EXPRESSION_COLNAME = { "AnimalStim", "ExpressionStim", "AnswerStim",
                                                           "Choice", "RT", "Correctness", 
                                                           "ExposeTime", "GroupID", "SubGroupID" };

        public static String SYMSPAN_SYMM_TABLENAME = "SYMSPAN_SYM";
        public static String[] SYMSPAN_SYMM_COLNAME = { "SymStim", "IsSym", "PosStim", "ExposureTime", 
                                                      "Correctness", "SymRT", "GroupID", "SubGroupID"};

        public static String SYMSPAN_POS_TABLENAME = "SYMSPAN_POS";
        public static String[] SYMSPAN_POS_COLNAME = { "PosStims", "UserPos", "PosRT", 
                                                      "Correctness", "GroupID", "SubGroupID"};

        public static String GRAPH_ASSO_TABLENAME = "GRAPH_ASSO";
        public static String WORD_ASSO_TABLENAME = "WORD_ASSO";


        public static String[] ASSO_COLNAME = { "Target", "S0_", "S1_", "S2_", 
                                                "S3_", "S4_", "S5_", "S6_", "S7_", 
                                                "RT", "CorrectAnswer", "UserAnswer" };

        public static String CUBE_TABLENAME = "CUBE";
        public static String PAPER_TABLENAME = "PAPER";
        public static String[] SPACE_COLNAME = { "Target", "Target2", "S0_", "S1_", "S2_", 
                                                "S3_", "S4_", "S5_", "S6_", "S7_", 
                                                "RT", "CorrectAnswer", "UserAnswer" };

        public static String VOCAB_TABLENAME = "VOCAB";
        public static String SIMI_TABLENAME = "SIMI";
        public static String[] VOCSIMI_COLNAME = { "Target", "S0_", "S1_", "S2_", 
                                                "S3_", "S4_", "RT", "W0_", "W1_", 
                                                "W2_", "W3_", "W4_", "UserAnswer" };

        public static String USER_TABLE_NAME = "USER_TABLE";
        public static String ODOMETER_TABLENAME = "ODOMETER";

        public static String SQLitePath = AppDomain.CurrentDomain.BaseDirectory + "dblite";
    }
}

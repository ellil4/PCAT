using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class MessageHeader
    {
        public static String REGISTER = "REGISTER";
        public static String GRANT = "GRANT";//with test list
        public static String DENY = "DENY";
        public static String PING = "PING";
        public static String PING_OK = "PING_OK";

        public static String TEST_BEGIN = "TEST_BEGIN";
        public static String TEST_END = "TEST_END";
        public static String ALL_END = "ALL_END";
    }
}

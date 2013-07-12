using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    public class ConnectionInfo
    {
        public String Server = "";
        public String DBName = "";
        public String Port = "";
        public String UserName = "";
        public String Password = "";

        public ConnectionInfo()
        { }

        public ConnectionInfo(String server, String dbName,
            String port, String userName, String password)
        {
            Server = server;
            DBName = dbName;
            Port = port;
            UserName = userName;
            Password = password;
        }
    }
}

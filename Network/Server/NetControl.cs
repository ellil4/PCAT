using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using PCATData;
using System.Text.RegularExpressions;
using System.Threading;

namespace Network
{
    public class NetControl
    {
        public PCATTableRoutine mDB;

        public delegate void AddRecordDelegate(List<String> recordContent);
        public AddRecordDelegate mAddRecFunc;
        public delegate void RemoveRecordDelegate(String ipa);
        public RemoveRecordDelegate mRemoveRecFunc;
        public delegate void ChangeRecordStaDelegate(String ipa, String newSta);
        public ChangeRecordStaDelegate mChangeRecStaFunc;
        public List<String> mTestsStrArr;

        public NetControl(PCATTableRoutine DB, List<String> tests)
        {
            mDB = DB;
            mTestsStrArr = tests;
        }

        private bool isIP(String input)
        {
            Regex rex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return rex.IsMatch(input);
        }
        
        public TcpListener mListener = null;

        public IPAddress GetLocalIPv4()
        {
            IPAddress[] ipas = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            String str = null;
            int i = 0;
            for (i = 0; i < ipas.Length; i++)
            {
                str = ipas[i].ToString();
                if (isIP(str))
                    break;
            }

            return ipas[i];
        }

        public delegate void OnServerStartEventDelegate(IPAddress ipa);

        public OnServerStartEventDelegate mfOnServerStart;

        public void ServerStart()
        {
            //TcpListener listener = new TcpListener(ipas[i], 8500);

            mListener.Start();
            mfOnServerStart(GetLocalIPv4());

            while (true)
            {
                lock (mListener)
                {
                    TcpClient client = mListener.AcceptTcpClient();
                    ClientThread ct = new ClientThread(client, this);
                }
            }
        }
    }
}


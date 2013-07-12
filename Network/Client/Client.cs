using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Network
{
    public class Client
    {
        NetworkStream mStream;
        TcpClient mClient;
        private static int BUFLEN = 1024;
        private byte[] mBuf;
        MessageProcessor mMP;
        bool mReadOver = false;
        IPAddress mServerIPA;

        public Client(IPAddress ServerIpa)
        {
            mBuf = new byte[BUFLEN];
            mMP = new MessageProcessor();

            mServerIPA = ServerIpa;
        }

        public void Ping()
        {
            mStream = OpenDialog();
            String request = "<4>PING";
            SendAndListen(request);
        }

        //one way informing
        private void SendMessage(String message, String additionalMessage)
        {
            mClient = new TcpClient();
            mClient.Connect(mServerIPA, 8500);
            mStream = mClient.GetStream();

            if (!String.IsNullOrEmpty(additionalMessage))
                message = message + "\t" + additionalMessage;

            message = "<" + message.Length + ">" + message;
            byte[] bufByte = Encoding.Unicode.GetBytes(message);
            mStream.Write(bufByte, 0, bufByte.Length);

            if (mStream != null)
                mStream.Close();

            mClient.Close();
        }

        public void SendAllEndMessage()
        {
            SendMessage("ALL_END", null);
        }

        public void SendTestEndMessage()
        {
            SendMessage("TEST_END", null);
        }

        public void SendTestBeginMessage(String TestName)
        {
            SendMessage("TEST_BEGIN", TestName);
        }

        public NetworkStream OpenDialog()
        {
            try
            {
                mClient = new TcpClient();
                mClient.Connect(mServerIPA, 8500);
                return mClient.GetStream();
            }
            catch (SocketException e)
            {
                throw e;
            }
        }

        public void SendAndListen(String request)
        {
            byte[] bufByte = Encoding.Unicode.GetBytes(request);
            mStream.Write(bufByte, 0, bufByte.Length);

            lock (mStream)
            {
                AsyncCallback callback = new AsyncCallback(onGettingRegisterResponse);
                mStream.BeginRead(mBuf, 0, BUFLEN, callback, null);
            }
        }

        //dual way dialog
        public void RegisterRequest(String name, String gender, int age,
            String health, String qualif, String job, String other)
        {
            mReadOver = false;

            mStream = OpenDialog();

            String request = appendInfo2String(MessageHeader.REGISTER, name);
            request = appendInfo2String(request, gender);
            request = appendInfo2String(request, age.ToString());
            request = appendInfo2String(request, health);
            request = appendInfo2String(request, qualif);
            request = appendInfo2String(request, job);
            request = appendInfo2String(request, other);
            request = appendInfo2String(request, Dns.GetHostName());

            request = "<" + request.Length + ">" + request;

            SendAndListen(request);
        }

        private void onGettingRegisterResponse(IAsyncResult ar)
        {
            int red = 0;

            try
            {
                lock (mStream)
                {
                    red = mStream.EndRead(ar);
                }

                String content = Encoding.Unicode.GetString(mBuf, 0, red);
                Array.Clear(mBuf, 0, BUFLEN);

                List<String> messages = new List<string>();
                mMP.GetMessages(content, ref messages);

                for (int i = 0; i < messages.Count; i++)
                {
                    parseAndRunRegister(messages[i]);
                    mReadOver = true;
                }
            }
            catch (Exception)
            { }

            if (!mReadOver)
            {
                lock (mStream)
                {
                    AsyncCallback callback = new AsyncCallback(onGettingRegisterResponse);
                    mStream.BeginRead(mBuf, 0, BUFLEN, callback, null);
                }
            }
            else
            {
                if(mStream != null)
                    mStream.Close();

                mClient.Close();
            }
        }

        private void parseAndRunRegister(String message)
        {
            Regex rex = new Regex("[^\t]+");
            MatchCollection mts = rex.Matches(message);

            if (mts[0].Value.Equals(MessageHeader.GRANT))
            {
                doGrant(message);
            }
            else if (mts[0].Value.Equals(MessageHeader.DENY))
            {
                doDeny(message);
            }
            else if ((mts[0].Value.Equals(MessageHeader.PING_OK)))
            {
                doPingOK();
            }
        }

        public delegate void pingOKDelegateFunc();
        public pingOKDelegateFunc mfPingOKFunc;

        private void doPingOK()
        {
            mfPingOKFunc();
            mClient.Close();
        }

        public delegate void doGrantDelegateFunc(String message);
        public doGrantDelegateFunc mfDoGrantFunc;

        private void doGrant(String message)
        {
            mfDoGrantFunc(message);
            mClient.Close();
        }

        private void doDeny(String message)
        { }



        private String appendInfo2String(String src, String info)
        {
            return src + "\t" + info;
        }
    }
}

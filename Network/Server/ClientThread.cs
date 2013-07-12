using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using PCATData;

namespace Network
{
    public class ClientThread
    {

        NetworkStream mStream;
        TcpClient mClient;
        private static int BUFLEN = 1024;
        private byte[] mBuf;
        MessageProcessor mMP;
        bool mReadOver = false;
        NetControl mNetCtrl;

        public ClientThread(TcpClient client, NetControl netCtrl)
        {
            mClient = client;
            mBuf = new byte[BUFLEN];
            mMP = new MessageProcessor();
            mNetCtrl = netCtrl;

            mStream = mClient.GetStream();

            AsyncCallback callback = new AsyncCallback(onReadFinished);//it`s a new thread
            mStream.BeginRead(mBuf, 0, BUFLEN, callback, null);
        }

        private void onReadFinished(IAsyncResult ar)
        {
            int red = 0;

            try
            {
                lock (mStream)
                {
                    red = mStream.EndRead(ar);
                }

                String content = Encoding.Unicode.GetString(mBuf);
                Array.Clear(mBuf, 0, BUFLEN);

                List<String> messages = new List<string>();
                mMP.GetMessages(content, ref messages);

                for (int i = 0; i < messages.Count; i++)
                {
                    ParseAndRun(messages[i]);
                    mReadOver = true;
                }

                if (!mReadOver)
                {
                    lock (mStream)
                    {
                        AsyncCallback callback = new AsyncCallback(onReadFinished);
                        mStream.BeginRead(mBuf, 0, BUFLEN, callback, null);
                    }
                }
            }
            catch (Exception)
            {
 
            }
        }

        private void CloseConnection()
        {
            if (mStream != null)
                mStream.Close();

            mClient.Close();
        }

        private void ParseAndRun(String request)
        {
            Regex rex = new Regex("[^\t]+");
            MatchCollection mts = rex.Matches(request);

            if (mts[0].Value.Equals(MessageHeader.REGISTER))
            {
                doRegister(mts);
            }
            else if (mts[0].Value.Equals(MessageHeader.TEST_BEGIN))
            {
                doTestBegin(mts);
            }
            else if (mts[0].Value.Equals(MessageHeader.TEST_END))
            {
                doTestEnd(mts);
            }
            else if (mts[0].Value.Equals(MessageHeader.ALL_END))
            {
                doALLEnd(mts);
            }
            else if (mts[0].Value.Equals(MessageHeader.PING))
            {
                responsePing();
            }
        }

        private IPAddress getIPA(EndPoint endPoint)
        {
            IPAddress retval = 
                new IPAddress(
                    Encoding.Unicode.GetBytes(endPoint.ToString().Split(':')[0]));

            return retval;
        }

        private String getPort(EndPoint endPoint)
        {
            return endPoint.ToString().Split(':')[1];
        }

        //without echo
        private void SendMessageOfDialog(String message)
        {
            message = "<" + message.Length + ">" + message;
            byte[] bufByte = Encoding.Unicode.GetBytes(message);
            mStream.Write(bufByte, 0, bufByte.Length);
        }

        private void doRegister(MatchCollection mts)
        {
            //do register first
            String name = mts[1].Value;
            String gender = mts[2].Value;
            int age = int.Parse(mts[3].Value);
            String health = mts[4].Value;
            String qualif = mts[5].Value;
            String job = mts[6].Value;
            String other = mts[7].Value;
            String machineName = mts[8].Value;

            StUserRegisterFeedback feedback =
                mNetCtrl.mDB.AddUser(name, gender, age, health, qualif, job, other);

            //then send message back
            String str2Send = MessageHeader.GRANT;
            //connect info
            str2Send = Methods.AppendInfo2String(
                str2Send, mNetCtrl.mDB.mConnInfo.Server);
            str2Send = Methods.AppendInfo2String(
                str2Send, mNetCtrl.mDB.mConnInfo.DBName);
            str2Send = Methods.AppendInfo2String(
                str2Send, mNetCtrl.mDB.mConnInfo.Port);
            str2Send = Methods.AppendInfo2String(
                str2Send, mNetCtrl.mDB.mConnInfo.UserName);
            str2Send = Methods.AppendInfo2String(
                str2Send, mNetCtrl.mDB.mConnInfo.Password);

            //USERID
            str2Send = Methods.AppendInfo2String(
                str2Send, feedback.id.ToString());

            //test info
            for(int i = 0; i < mNetCtrl.mTestsStrArr.Count; i++)
            {
                str2Send = Methods.AppendInfo2String(str2Send, mNetCtrl.mTestsStrArr[i]);
            }

            SendMessageOfDialog(str2Send);

            //close
            CloseConnection();

            //do UI work
            List<String> userInfoList = new List<string>();
            userInfoList.Add(feedback.id.ToString());
            userInfoList.Add(feedback.adminNum.ToString());
            userInfoList.Add("idle");
            userInfoList.Add(mClient.Client.RemoteEndPoint.ToString().Split(':')[0]);
            userInfoList.Add(machineName);
            userInfoList.Add(name);

            mNetCtrl.mAddRecFunc(userInfoList);
        }

        private void responsePing()
        {
            SendMessageOfDialog("PING_OK");
            CloseConnection();
        }

        private void doTestBegin(MatchCollection mts)
        {
            //InvokeUI
            mNetCtrl.mChangeRecStaFunc(
                mClient.Client.RemoteEndPoint.ToString().Split(':')[0], 
                mts[1].Value);

            CloseConnection();
        }

        private void doTestEnd(MatchCollection mts)
        {
            //invokeUI
            mNetCtrl.mChangeRecStaFunc(
                mClient.Client.RemoteEndPoint.ToString().Split(':')[0],
                "idle");

            CloseConnection();
        }

        private void doALLEnd(MatchCollection mts)
        {
            //invokeUI
            mNetCtrl.mRemoveRecFunc(
                mClient.Client.RemoteEndPoint.ToString().Split(':')[0]);

            CloseConnection();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    class MessageProcessor
    {
        String mContent = "";

        private StMessage getFirstMessageInfo(String totalContent)
        {
            StMessage retval = new StMessage();

            int markStart = -1;

            for (int i = 0; i < totalContent.Length; i++)
            {
                if (totalContent[i] == '<')
                {
                    markStart = i;
                }

                if (markStart != -1)
                {
                    if (totalContent[i] == '>')
                    {
                        retval.HeaderLen = i - markStart + 1;
                        retval.MessageStartPos = i + 1;
                        retval.MessageLength = Int32.Parse(totalContent.Substring(markStart + 1, i - markStart - 1));
                        break;
                    }
                }
                
            }

                return retval;
        }

        public void GetMessages(String input, ref List<String> messages)
        {
            if(messages == null)
                messages = new List<string>();

            String totalContent = mContent + input;

            StMessage firstMessageInfo = getFirstMessageInfo(totalContent);

            while(firstMessageInfo.IsValid())
            {
                if (totalContent.Length < firstMessageInfo.MessageLength + firstMessageInfo.HeaderLen)
                    break;

                String message =
                    totalContent.Substring(firstMessageInfo.MessageStartPos, firstMessageInfo.MessageLength);

                messages.Add(message);

                totalContent = totalContent.Remove(0, firstMessageInfo.MessageLength + firstMessageInfo.HeaderLen);

                firstMessageInfo = getFirstMessageInfo(totalContent);
            }

            mContent = totalContent;

        }

        public void Clear()
        {
            mContent = "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace FiveElementsIntTest.SybSrh
{
    class SybSrhSourceFetcher
    {
        public String mBasePath;
        public byte[] mBuffer;

        private String getPackName(int type)
        {
            return mBasePath + type.ToString() + ".qai";
        }

        public SybSrhSourceFetcher(String basePath)
        {
            mBasePath = basePath;
            mBuffer = new byte[4096];
        }

        public Bitmap GetPic(int type, int index)
        {
            return new Bitmap(getFileStream(type, index));
        }

        public int GetTypeElemCount(int type)
        {
            QAIF.Reader reader = new QAIF.Reader(getPackName(type));
            int retval = (int)reader.GetItemCount();
            reader.Finish();
            return retval;
        }

        private Stream getFileStream(int type, int index)
        {
            QAIF.Reader reader = new QAIF.Reader(getPackName(type));
            
            int redLen = reader.GetData(ref mBuffer, index);
            Stream retval = new MemoryStream(mBuffer, 0, redLen);
            reader.Finish();
            return retval;
        }
    }
}

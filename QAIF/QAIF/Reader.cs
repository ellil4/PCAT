using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QAIF
{
    public class Reader : RWBase
    {
        String mPath;

        FileStream mFSi;
        BinaryReader mBRi;

        FileStream mFSd;
        BinaryReader mBRd;

        long mItemCount = -1;

        public Reader(String IndexFilepath)
        {
            mPath = IndexFilepath;

            QAIFDataSpanInfo info = QAIF.GetFileSpanInfo(IndexFilepath);
            mIndexBeg = info.Begin;
            mItemCount = info.Length;

            //index
            mFSi = new FileStream(
                IndexFilepath, FileMode.Open, FileAccess.Read);
            mBRi = new BinaryReader(mFSi);

            //data
            mFSd = new FileStream(
                QAIF.GetDataFilename(IndexFilepath), FileMode.Open, FileAccess.Read);
            mBRd = new BinaryReader(mFSd);
        }

        public QAIFDataSpanInfo GetDataSpanInfo(long index)
        {
            QAIFDataSpanInfo retval = new QAIFDataSpanInfo();

            mFSi.Position = GetIndexPosition(index);
            retval.Begin = mBRi.ReadInt64();
            retval.Length = mBRi.ReadInt64();

            return retval;
        }

        public long GetItemCount()
        {
            return mItemCount;
        }

        public int GetData(ref byte[] buffer, long index)
        {
            int retval = -1;

            QAIFDataSpanInfo info = GetDataSpanInfo(index);
            mFSd.Position = info.Begin;
            retval = mBRd.Read(buffer, 0, (int)info.Length);

            return retval;
        }

        public void Finish()
        {
            if (mFSi != null)
                mFSi.Close();

            if (mFSd != null)
                mFSd.Close();

            if (mBRi != null)
                mBRi.Close();

            if (mBRd != null)
                mBRd.Close();
        }
    }
}

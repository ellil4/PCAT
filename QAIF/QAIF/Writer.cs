using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QAIF
{
    public class Writer : RWBase
    {
        String mPath;

        //index
        BinaryWriter mBWi;
        FileStream mFSi;

        //data
        BinaryWriter mBWd;
        FileStream mFSd;
        long mDataTill = -1;

        long mItemCount = -1;

        static long INDEX_BEG = 16;

        public Writer(String path, bool append)
        {
            mPath = path;

            if (!File.Exists(path) &&
                    !File.Exists(QAIF.GetDataFilename(path)))
            {
                append = false;
            }

            if (!append)
            {
                if (File.Exists(path) &&
                    File.Exists(QAIF.GetDataFilename(path)))
                {
                    File.Delete(path);
                    File.Delete(QAIF.GetDataFilename(path));
                }

                mFSi = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);

                mFSd = new FileStream(
                    QAIF.GetDataFilename(path), FileMode.CreateNew, FileAccess.Write);

                mIndexBeg = 0;
                mDataTill = 0;
                mItemCount = 0;
            }
            else//append
            {
                QAIFDataSpanInfo infoIdx = QAIF.GetFileSpanInfo(path);
                mIndexBeg = infoIdx.Begin;
                mItemCount = infoIdx.Length;

                mFSi = new FileStream(path, FileMode.Append, FileAccess.ReadWrite);

                mFSd = new FileStream(
                    QAIF.GetDataFilename(path), FileMode.Append, FileAccess.Write);

                FileInfo info = new FileInfo(QAIF.GetDataFilename(path));
                mDataTill = info.Length;
            }

            mBWi = new BinaryWriter(mFSi);
            mBWd = new BinaryWriter(mFSd);

            if (!append)
            {
                mFSi.Position = 0;
                mBWi.Write(INDEX_BEG);
                mIndexBeg = INDEX_BEG;//
                mBWi.Write(mItemCount);
            }
        }

        private void copyFile2Datapack(String path, long len)
        {
            FileStream srcFS = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader srcBR = new BinaryReader(srcFS);
            
            byte[] data = srcBR.ReadBytes((int)len);
            mFSd.Seek(mDataTill, SeekOrigin.Begin);
            mBWd.Write(data, 0, (int)len);

            srcFS.Close();
            srcBR.Close();
        }

        public void Add(String path, long index)
        {
            if (index <= mItemCount)
            {
                //index
                FileInfo fi = new FileInfo(path);

                mFSi.Seek(GetIndexPosition(index), SeekOrigin.Begin);
                copyFile2Datapack(path, fi.Length);
                mBWi.Write(mDataTill);//beg
                mBWi.Write(fi.Length);//len

                mDataTill += fi.Length;
                
                if(index == mItemCount)
                    mItemCount++;
            }
        }

        public void Add(long len, int offset, byte[] src, long index)
        {
            if (index <= mItemCount)
            {
                mFSi.Seek(GetIndexPosition(index), SeekOrigin.Begin);
                mFSd.Seek(mDataTill, SeekOrigin.Begin);
                mBWd.Write(src, offset, (int)len);
                mBWi.Write(mDataTill);//beg
                mBWi.Write(len);//len

                mDataTill += len;

                if (index == mItemCount)
                    mItemCount++;
            }
        }

        public void Add(short[] src, long index)
        {
            if (index <= mItemCount)
            {
                mFSi.Seek(GetIndexPosition(index), SeekOrigin.Begin);
                mFSd.Seek(mDataTill, SeekOrigin.Begin);

                for (int i = 0; i < src.Length; i++)
                    mBWd.Write(src[i]);

                long lenByte = src.Length * 2;
                mBWi.Write(mDataTill);//beg
                mBWi.Write(lenByte);//len

                mDataTill += lenByte;

                if (index == mItemCount)
                    mItemCount++;
            }
        }

        private void updateItemCount()
        {
            mFSi.Position = QAIF.INFOPOS_ITEM_COUNT;
            mBWi.Write(mItemCount);
        }

        public void Finish()
        {
            updateItemCount();

            if (mFSd != null)
                mFSd.Close();

            if (mFSi != null)
                mFSi.Close();

            if (mBWd != null)
                mBWd.Close();

            if (mBWi != null)
                mBWi.Close();
        }

    }
}

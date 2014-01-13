using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QAIF
{
    public class QAIF
    {
        public static long INFOPOS_INDEX_BEG = 0;
        public static long INFOPOS_ITEM_COUNT = 8;
        public static String INDEX_EXT = "qai";
        public static String DATA_EXT = "qad";
        public static long STRIDE = 16;//beg and len

        public static QAIFDataSpanInfo GetFileSpanInfo(String filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            QAIFDataSpanInfo retval = new QAIFDataSpanInfo();
            fs.Position = INFOPOS_INDEX_BEG;
            retval.Begin = br.ReadInt64();
            fs.Position = INFOPOS_ITEM_COUNT;
            retval.Length = br.ReadInt64();

            fs.Close();
            br.Close();

            return retval;

        }

        public static String GetDataFilename(String indexFilename)
        {
            indexFilename = indexFilename.Remove(indexFilename.Length - 3);
            String retval = indexFilename + QAIF.DATA_EXT;
            return retval;
        }

        public static void DumpData2HDD(
            String path, byte[] data, int off, int len)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(data, off, len);
            fs.Close();
            bw.Close();
        }
    }
}

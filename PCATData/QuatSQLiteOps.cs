using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace PCATData
{
    class QuatSQLiteOps : QuatAbstractDBOps
    {
        SQLiteConnection mConn;

        public QuatSQLiteOps(string dbAddr)
        {
            if (!File.Exists(dbAddr))
            {
                SQLiteConnection.CreateFile(dbAddr);
                //
            }

            mConn = new SQLiteConnection();
            SQLiteConnectionStringBuilder strBuilder = new SQLiteConnectionStringBuilder();
            strBuilder.DataSource = dbAddr;
            mConn.ConnectionString = strBuilder.ToString();
        }

        override public void Open()
        {
            mConn.Open();
        }

        override public void Close()
        {
            mConn.Close();
        }

        override public void ExecuteX(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(mConn);
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        override public Object Query(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(mConn);
            command.CommandText = sql;
            SQLiteDataReader reader = command.ExecuteReader();
            return reader;
        }

        override public bool TableExists(string tableName)
        {
            bool retval = false;

            try
            {
                Open();
                SQLiteDataReader reader = (SQLiteDataReader)Query("SELECT * FROM " + tableName);
                reader.Close();
                retval = true;
            }
            catch (SQLiteException)
            {

            }
            finally
            {
                if (mConn != null)
                    Close();
            }
            return retval;
        }
    }
}

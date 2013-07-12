using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PCATData
{
    class QuatMySQLOps : QuatAbstractDBOps
    {
        private MySqlConnection mConnection;

        public QuatMySQLOps(string server, string dbName,
            string port, string userName, string pass)
        {
            string connectString = "server=" + server +
                ";user=" + userName + ";database=" + dbName +
                ";port=" + port + ";password=" + pass;
            mConnection = new MySqlConnection(connectString);
        }

        //non value back
        override public void ExecuteX(string sql)
        {
            try
            {
                //mConnection.Open();
                MySqlCommand cmd = new MySqlCommand(sql, mConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }

            //mConnection.Close();
        }

        override public void Open()
        {
            try
            {
                mConnection.Open();
            }
            catch (MySqlException e)
            {
                throw e;
            }
        }

        override public void Close()
        {
            mConnection.Close();
        }

        //rows of data back
        override public Object Query(string sql)
        {
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;

            try
            {
                cmd = new MySqlCommand(sql, mConnection);
                reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }

            return reader;
        }

        override public bool TableExists(string tableName)
        {
            bool retval = false;

            Open();

            MySqlDataReader reader = (MySqlDataReader)Query("SELECT * FROM " + tableName);

            if (reader != null)
            {
                retval = true;
                reader.Close();
            }

            Close();

            return retval;
        }

    }
}

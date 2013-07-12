using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Net;
using System.Data.SQLite;

namespace PCATData
{
    public class PCATTableRoutine
    {
        private QuatAbstractDBOps mDBOperator;
        public ConnectionInfo mConnInfo;
        public VERSION mVersion;

        //tools
        //single quatation
        static public String toolAppendSQ2Str(String str)
        {
            String retval = "'";
            retval += str;
            retval += "'";
            return retval;
        }

        static public String toolGenTableHeaderStr(List<String> headerItem, int count)
        {
            String rt = "";

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < headerItem.Count; j++)
                {
                    rt += headerItem[j] + i + " VARCHAR(20), ";
                }
            }

            return rt;
        }

        static public String getGender(bool gender)
        {
            if (gender)
                return "FEMALE";
            else
                return "MALE";
        }

        public PCATTableRoutine(ConnectionInfo info, VERSION ver)
        {
            mVersion = ver;

            try
            {
                mConnInfo = info;

                if(mVersion == VERSION.MANAGER || 
                    mVersion == VERSION.CLIENT)
                    initMySQL();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1042)
                    throw e;
            }
        }

        public PCATTableRoutine(String dbPath)
        {
            mVersion = VERSION.STANDALONE;
            InitSQLite(dbPath);
        }

        public bool TableExists(String tableName)
        {
            return mDBOperator.TableExists(tableName);
        }

        public void SetupMySQLConnection(ConnectionInfo ConnInfo)
        {
            mDBOperator = new QuatMySQLOps(ConnInfo.Server, ConnInfo.DBName,
                ConnInfo.Port, ConnInfo.UserName, ConnInfo.Password);
        }

        public void initMySQL()
        {
            SetupMySQLConnection(mConnInfo);

            try
            {
                mDBOperator.Open();
                mDBOperator.Close();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1049)//create database
                {
                    mDBOperator.Close();

                    mDBOperator = new QuatMySQLOps(mConnInfo.Server, "",
                        mConnInfo.Port, mConnInfo.UserName, mConnInfo.Password);
                    mDBOperator.Open();
                    mDBOperator.ExecuteX("create database PCATDB");
                    mDBOperator.Close();

                    mDBOperator = new QuatMySQLOps(mConnInfo.Server, mConnInfo.DBName,
                        mConnInfo.Port, mConnInfo.UserName, mConnInfo.Password);

                    if (!mDBOperator.TableExists(Names.USER_TABLE_NAME))
                    {
                        CreateUserTable();
                    }

                    if (!mDBOperator.TableExists(Names.ODOMETER_TABLENAME))
                    {
                        CreateOdometer();
                    }
                }
                else if (e.Number == 1042)//wrong connection input
                {
                    throw e;
                }
            }
            finally
            {
                if (mDBOperator != null)
                    mDBOperator.Close();
            }
        }

        public void SetupSQLiteConnection(String path)
        {
            mDBOperator = new QuatSQLiteOps(path);
        }

        public void InitSQLite(String path)
        {
            SetupSQLiteConnection(path);

            if (!mDBOperator.TableExists(Names.USER_TABLE_NAME))
            {
                CreateUserTable();
            }

            if (!mDBOperator.TableExists(Names.ODOMETER_TABLENAME))
            {
                CreateOdometer();
            }
        }

        public void CreateUserTable()
        {
            mDBOperator.Open();
            string SQL = "CREATE TABLE " + Names.USER_TABLE_NAME + "(";

            if(mVersion == VERSION.MANAGER)
            {
                SQL += "ID BIGINT(20) NOT NULL AUTO_INCREMENT PRIMARY KEY,";
            }
            else if(mVersion == VERSION.STANDALONE)
            {
                SQL += "ID INTEGER NOT NULL PRIMARY KEY,";
            }

            SQL += "GROUP_MARK VARCHAR(128)," +
            "TIME VARCHAR(20)," +
            "NAME VARCHAR(128) NOT NULL, " +
            "GENDER CHAR(4) NOT NULL," +
            "AGE INT NOT NULL," +
            "HEALTH VARCHAR(64)," +
            "QUALIF VARCHAR(64)," +
            "JOB VARCHAR(128)," +
            "OTHER VARCHAR(255)" +
            ")";

            mDBOperator.ExecuteX(SQL);
            mDBOperator.Close();
        }

        public QRecUser QueryUserTable(long id)
        {
            QRecUser retval = new QRecUser();
            mDBOperator.Open();
            String SQL = "SELECT * FROM USER_TABLE WHERE ID = " + id;
            //object result = ;

            if (mVersion != VERSION.STANDALONE)
            {
                MySqlDataReader reader = (MySqlDataReader)mDBOperator.Query(SQL);
                while (reader.Read())
                {
                    retval.ID = reader.GetInt64(0).ToString();
                    retval.GroupMark = reader.GetString(1);
                    retval.Time = reader.GetString(2);
                    retval.Name = reader.GetString(3);
                    retval.Gender = reader.GetString(4);
                    retval.Age = reader.GetInt16(5).ToString();
                    retval.Health = reader.GetString(6);
                    retval.Qualif = reader.GetString(7);
                    retval.Job = reader.GetString(8);
                    retval.Other = reader.GetString(9);
                }
            }
            else
            {
                SQLiteDataReader readerLite = (SQLiteDataReader)mDBOperator.Query(SQL);
                while (readerLite.Read())
                {
                    retval.ID = ((long)readerLite["ID"]).ToString();
                    retval.GroupMark = (String)readerLite["GROUP_MARK"];
                    retval.Time = (String)readerLite["TIME"];
                    retval.Name = (String)readerLite["NAME"];
                    retval.Gender = (String)readerLite["GENDER"];
                    retval.Age = ((int)readerLite["AGE"]).ToString();
                    retval.Health = (String)readerLite["HEALTH"];
                    retval.Qualif = (String)readerLite["QUALIF"];
                    retval.Job = (String)readerLite["JOB"];
                    retval.Other = (String)readerLite["OTHER"];
                }
            }

            mDBOperator.Close();
            return retval;
        }

        public void UpdateUserTable(QRecUser user)
        {
            mDBOperator.Open();
            String SQL = "UPDATE " + Names.USER_TABLE_NAME + " SET " +
                "GROUP_MARK = " + toolAppendSQ2Str(user.GroupMark) + ", " +
                "TIME = " + toolAppendSQ2Str(user.Time) + ", " +
                "NAME = " + toolAppendSQ2Str(user.Name) + ", " +
                "GENDER = " + toolAppendSQ2Str(user.Gender) + ", " +
                "AGE = " + user.Age + ", " +
                "HEALTH = " + toolAppendSQ2Str(user.Health) + ", " +
                "QUALIF = " + toolAppendSQ2Str(user.Qualif) + ", " +
                "JOB = " + toolAppendSQ2Str(user.Job) + ", " +
                "OTHER = " + toolAppendSQ2Str(user.Other) + " " + 
                "WHERE ID = " + user.ID + ";";
            mDBOperator.ExecuteX(SQL);
            mDBOperator.Close();
        }

        public void CreateOdometer()
        {
            mDBOperator.Open();
            mDBOperator.ExecuteX("CREATE TABLE " + Names.ODOMETER_TABLENAME +
                "(MARK BIGINT PRIMARY KEY, NUM BIGINT, PASSWORD VARCHAR(48));");
            mDBOperator.ExecuteX(
                "INSERT INTO " + Names.ODOMETER_TABLENAME + 
                "(MARK, NUM, PASSWORD) VALUES(0, 0, 'pcat');");
            mDBOperator.Close();
        }

        public long QueryOdometer(bool close)
        {
            long retval = long.MinValue;
            String SQL = "SELECT NUM FROM " + Names.ODOMETER_TABLENAME + " WHERE MARK = 0";

            mDBOperator.Open();

            if (mVersion == VERSION.MANAGER)
            {
                MySqlDataReader reader = (MySqlDataReader)mDBOperator.Query(SQL);

                while (reader.Read())
                {
                    retval = reader.GetInt64(0);
                }

                reader.Close();
            }
            else if (mVersion == VERSION.STANDALONE)
            {
                SQLiteDataReader readerLite = (SQLiteDataReader)mDBOperator.Query(SQL);

                while (readerLite.Read())
                {
                    retval = (long)readerLite["NUM"];
                }

                readerLite.Close();
            }

            if(close)
                mDBOperator.Close();

            return retval;
        }

        public long ApplyNumFromOdometer()
        {
            long retval = -1;

            retval = QueryOdometer(false);

            long val2set = retval + 1;

            String SQL2 = "UPDATE " + Names.ODOMETER_TABLENAME +
                " SET NUM = " + val2set +
                " WHERE MARK = 0;";

            mDBOperator.ExecuteX(SQL2);
            mDBOperator.Close();

            return retval;
        }

        public void SetOdometer(long num)
        {
            mDBOperator.Open();
            String SQL = "UPDATE " + Names.ODOMETER_TABLENAME +
                " SET NUM = " + num +
                " WHERE MARK = 0;";
            mDBOperator.ExecuteX(SQL);
            mDBOperator.Close();
        }

        public void SetPassword(String pw)
        {
            mDBOperator.Open();
            String SQL = "UPDATE " + Names.ODOMETER_TABLENAME +
                " SET PASSWORD = " + toolAppendSQ2Str(pw) +
                " WHERE MARK = 0;";
            mDBOperator.ExecuteX(SQL);
            mDBOperator.Close();
        }

        public String GetPassword()
        {
            String retval = "";
            String SQL = "SELECT PASSWORD FROM " + Names.ODOMETER_TABLENAME + " WHERE MARK = 0";

            mDBOperator.Open();

            if (mVersion == VERSION.MANAGER)
            {
                MySqlDataReader reader = (MySqlDataReader)mDBOperator.Query(SQL);

                while (reader.Read())
                {
                    retval = reader.GetString(0);
                }

                reader.Close();
            }
            else if (mVersion == VERSION.STANDALONE)
            {
                SQLiteDataReader readerLite = (SQLiteDataReader)mDBOperator.Query(SQL);

                while (readerLite.Read())
                {
                    retval = (String)readerLite["PASSWORD"];
                }

                readerLite.Close();
            }

            mDBOperator.Close();

            return retval;
        }

        public StUserRegisterFeedback AddUser(String name, String gender, int age,
            String health, String qualif, String job, String other)
        {
            name = toolAppendSQ2Str(name);
            health = toolAppendSQ2Str(health);
            qualif = toolAppendSQ2Str(qualif);
            job = toolAppendSQ2Str(job);
            other = toolAppendSQ2Str(other);

            StUserRegisterFeedback feedback = new StUserRegisterFeedback();
            //feedback.adminGroupName = GetNameFromOdometer();
            feedback.adminNum = ApplyNumFromOdometer();


            String SQL = "INSERT INTO USER_TABLE (GROUP_MARK, TIME, NAME, GENDER, AGE, HEALTH, QUALIF, JOB, OTHER) ";
            SQL += "VALUES (" + toolAppendSQ2Str(feedback.adminNum.ToString()) + "," +
                toolAppendSQ2Str(DateTime.Now.ToString()) + "," +
                name + "," + toolAppendSQ2Str(gender) + "," + age + "," +
                health + "," + qualif + "," + job + "," + other + ");";

            mDBOperator.Open();

            mDBOperator.ExecuteX(SQL);

            if (mVersion == VERSION.MANAGER)
            {
                MySqlDataReader reader = 
                    (MySqlDataReader)mDBOperator.Query("SELECT LAST_INSERT_ID() FROM " + 
                        Names.USER_TABLE_NAME);

                while (reader.Read())
                {
                    feedback.id = reader.GetInt64(0);
                }

                reader.Close();
            }
            else if (mVersion == VERSION.STANDALONE)
            {
                SQLiteDataReader readerLite = 
                    (SQLiteDataReader)mDBOperator.Query("SELECT last_insert_rowid() FROM " 
                        + Names.USER_TABLE_NAME);

                while (readerLite.Read())
                {
                    feedback.id = readerLite.GetInt64(0);
                    break;
                }

                readerLite.Close();
            }

            mDBOperator.Close();

            return feedback;
        }

        public void CreateTable(int itemCount, String tableName, String[] columnNames)
        {
            String SQL = "CREATE TABLE " + tableName + "(" +
                "ID BIGINT(20) PRIMARY KEY ,";

            List<String> header = new List<string>();
            for (int i = 0; i < columnNames.Length; i++)
            {
                header.Add(columnNames[i]);
            }

            SQL += toolGenTableHeaderStr(header, itemCount);

            SQL += "FOREIGN KEY(ID) REFERENCES USER_TABLE(ID) ON DELETE CASCADE ON UPDATE CASCADE";

            SQL += ");";

            mDBOperator.Open();
            mDBOperator.ExecuteX(SQL);
            mDBOperator.Close();
        }

        private void addRecord(List<String> data, String[] columnNames, String tableName, long ID, int itemCount)
        {
            String SQL = "INSERT INTO " + tableName + " ";
            string cols = "(ID,";

            for (int i = 0; i < itemCount; i++)
            {
                for (int j = 0; j < columnNames.Length; j++)
                    cols += columnNames[j] + i + ",";
            }

            cols = cols.Remove(cols.Length - 1);
            cols += ") VALUES (" + ID + ",";

            for (int j = 0; j < data.Count; j++)
                cols += toolAppendSQ2Str(data[j]) + ",";

            cols = cols.Remove(cols.Length - 1);
            cols += ")";

            SQL += cols;

            mDBOperator.Open();
            mDBOperator.ExecuteX(SQL);
            mDBOperator.Close();
        }

        public void CreateDigiSymbolTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.DIGIT_SYMBOL_TABLENAME, Names.DIGIT_SYMBOL_COLNAME);
        }

        public void AddDigiSymbolRecord(List<QRecDigiSymbol> result, long ID)
        {
            List<String> serializedResult = new List<string>();

            for (int i = 0; i < result.Count; i++)
            {
                serializedResult.Add(result[i].Stim.ToString());
                serializedResult.Add(result[i].Left.ToString());
                serializedResult.Add(result[i].RTLeft.ToString());
                serializedResult.Add(result[i].Right.ToString());
                serializedResult.Add(result[i].RTRight.ToString());
                serializedResult.Add(result[i].Correctness.ToString());
            }

            addRecord(serializedResult,
                Names.DIGIT_SYMBOL_COLNAME, Names.DIGIT_SYMBOL_TABLENAME,
                ID, result.Count);
        }

        //symbol search
        public void CreateSymbolSearchTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.SYMBOL_SEARCH_TABLENAME, Names.SYMBOL_SEARCH_COLNAME);
        }

        public void AddSymbolSearchRecord(List<QRecSymbolSearch> result, long ID)
        {
            List<String> serializedResult = new List<string>();

            for (int i = 0; i < result.Count; i++)
            {
               /* String stimStr = "";
                for (int j = 0; j < result[i].Stim.Count; j++)
                {
                    stimStr += result[i].Stim[j].ToString();
                }*/

               // serializedResult.Add(stimStr);
                serializedResult.Add(result[i].Answer.ToString());
                serializedResult.Add(result[i].RT.ToString());
                serializedResult.Add(result[i].Correctness.ToString());
            }

            addRecord(serializedResult,
                Names.SYMBOL_SEARCH_COLNAME, Names.SYMBOL_SEARCH_TABLENAME,
                ID, result.Count);
        }

        //OP Span
        public void CreateOPSpanOrderTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.OPSPAN_ORDER_TABLENAME, Names.OPSPAN_ORDER_COLNAME);
        }

        public void CreateOpSpanExpressionTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.OPSAPN_EXPRESSION_TABLENAME, Names.OPSPAN_EXPRESSION_COLNAME);
        }

        public void AddOPSpanOrderRecord(List<QRecOPSpan> answers, long userID)
        {
            List<String> serializedResult = new List<string>();
            for (int i = 0; i < answers.Count; i++)
            {
                serializedResult.Add(answers[i].StimOrder);
                serializedResult.Add(answers[i].UserOrder);
                serializedResult.Add(answers[i].OrderRT.ToString());
                serializedResult.Add(answers[i].OrderCorrectness.ToString());
                serializedResult.Add(answers[i].GroupID.ToString());
                serializedResult.Add(answers[i].SubGroupID.ToString());
            }

            addRecord(serializedResult,
                Names.OPSPAN_ORDER_COLNAME, Names.OPSPAN_ORDER_TABLENAME,
                userID, answers.Count);
        }

        public void AddOpSpanExpressionRecord(List<QRecOPSpan> answers, long userID)
        {
            List<String> serializedResult = new List<String>();
            int totalCount = 0;

            //one sub-group
            for (int i = 0; i < answers.Count; i++)
            {
                for (int j = 0; j < answers[i].AnimalStim.Count; j++)
                {
                    serializedResult.Add(answers[i].AnimalStim[j]);
                    serializedResult.Add(answers[i].ExpressionStim[j]);
                    serializedResult.Add(answers[i].AnswerStim[j]);
                    serializedResult.Add(answers[i].Confirm[j].ToString());
                    serializedResult.Add(answers[i].ConfirmRT[j].ToString());
                    serializedResult.Add(answers[i].ConfirmCorrectness[j].ToString());
                    serializedResult.Add(answers[i].ExposureTime[j].ToString());
                    serializedResult.Add(answers[i].GroupID.ToString());
                    serializedResult.Add(answers[i].SubGroupID.ToString());

                    totalCount++;
                }
            }

            addRecord(serializedResult,
                Names.OPSPAN_EXPRESSION_COLNAME, Names.OPSAPN_EXPRESSION_TABLENAME,
                userID, totalCount);
        }

        public void CreateSymSpanSymmTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.SYMSPAN_SYMM_TABLENAME, Names.SYMSPAN_SYMM_COLNAME);
        }

        public void CreateSymSpanPosTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.SYMSPAN_POS_TABLENAME, Names.SYMSPAN_POS_COLNAME);
        }

        public void AddSymSpanSymmRecord(List<QRecSymSpan> answers, long userID)
        {
            List<String> serializedResult = new List<string>();
            int totalCount = 0;

            for (int i = 0; i < answers.Count; i++)
            {
                for (int j = 0; j < answers[i].SymStim.Count; j++)
                {
                    serializedResult.Add(answers[i].SymStim[j]);
                    serializedResult.Add(answers[i].IsSym[j].ToString());
                    serializedResult.Add(answers[i].PosStim[j].ToString());
                    serializedResult.Add(answers[i].SymExposureTime[j].ToString());
                    serializedResult.Add(answers[i].SymCorrectness[j].ToString());
                    serializedResult.Add(answers[i].SymRT[j].ToString());
                    serializedResult.Add(answers[i].GroupID.ToString());
                    serializedResult.Add(answers[i].SubGroupID.ToString());

                    totalCount++;
                }
            }

            addRecord(serializedResult,
                Names.SYMSPAN_SYMM_COLNAME, Names.SYMSPAN_SYMM_TABLENAME,
                userID, totalCount);
        }

        public void AddSymSpanPosRecord(List<QRecSymSpan> answers, long userID)
        {
            List<String> serializedAnswers = new List<string>();

            for (int i = 0; i < answers.Count; i++)
            {
                String posStim = "";
                String userPos = "";
                for (int j = 0; j < answers[i].PosStim.Count; j++)
                {
                    posStim += answers[i].PosStim[j].ToString();
                    userPos += answers[i].UserPos[j].ToString();

                    if (j != answers[i].PosStim.Count - 1)
                    {
                        posStim += ",";
                        userPos += ",";
                    }
                }

                serializedAnswers.Add(posStim);
                serializedAnswers.Add(userPos);
                serializedAnswers.Add(answers[i].PosRT.ToString());
                serializedAnswers.Add(answers[i].Correctness.ToString());
                serializedAnswers.Add(answers[i].GroupID.ToString());
                serializedAnswers.Add(answers[i].SubGroupID.ToString());
            }

            addRecord(serializedAnswers,
                Names.SYMSPAN_POS_COLNAME, Names.SYMSPAN_POS_TABLENAME,
                userID, answers.Count);
        }

        public void CreateGraphAssoTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.GRAPH_ASSO_TABLENAME, Names.ASSO_COLNAME);
        }

        public void CreateWordAssoTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.WORD_ASSO_TABLENAME, Names.ASSO_COLNAME);
        }

        public void AddAssoRecord(List<QRecStdMultiChoice> result, long userID, String tableName)
        {
            List<String> serial = new List<String>();
            for (int i = 0; i < result.Count; i++)
            {
                serial.Add(result[i].Target);
                serial.Add(result[i].SS[0]);
                serial.Add(result[i].SS[1]);
                serial.Add(result[i].SS[2]);
                serial.Add(result[i].SS[3]);
                serial.Add(result[i].SS[4]);
                serial.Add(result[i].SS[5]);
                serial.Add(result[i].SS[6]);
                serial.Add(result[i].SS[7]);
                serial.Add(result[i].RT.ToString());
                serial.Add(result[i].CorrectAnswer.ToString());
                serial.Add(result[i].UserAnswer.ToString());
            }

            addRecord(serial,
                Names.ASSO_COLNAME, tableName,
                userID, result.Count);
        }

        public void CreateCubeTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.CUBE_TABLENAME, Names.SPACE_COLNAME);
        }

        public void CreatePaperTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.PAPER_TABLENAME, Names.SPACE_COLNAME);
        }

        public void AddSpaceRecord(
            List<QRecSpace> result, long userID, String tablename)
        {
            List<String> serial = new List<String>();
            for (int i = 0; i < result.Count; i++)
            {
                serial.Add(result[i].Target);
                serial.Add(result[i].Target2);
                serial.Add(result[i].SS[0]);
                serial.Add(result[i].SS[1]);
                serial.Add(result[i].SS[2]);
                serial.Add(result[i].SS[3]);
                serial.Add(result[i].SS[4]);
                serial.Add(result[i].SS[5]);
                serial.Add(result[i].SS[6]);
                serial.Add(result[i].SS[7]);
                serial.Add(result[i].RT.ToString());
                serial.Add(result[i].CorrectAnswer.ToString());
                serial.Add(result[i].UserAnswer.ToString());
            }

            addRecord(serial,
                Names.SPACE_COLNAME, tablename,
                userID, result.Count);
        }

        public void CreateVocabTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.VOCAB_TABLENAME, Names.VOCSIMI_COLNAME);
        }

        public void CreateSimiTable(int itemCount)
        {
            CreateTable(itemCount,
                Names.SIMI_TABLENAME, Names.VOCSIMI_COLNAME);
        }

        public void AddVocSimiRecord(
            List<QRecStdMultiChoice> result, long userID, String tablename)
        {
            List<String> serial = new List<String>();
            for (int i = 0; i < result.Count; i++)
            {
                serial.Add(result[i].Target);
                serial.Add(result[i].SS[0]);
                serial.Add(result[i].SS[1]);
                serial.Add(result[i].SS[2]);
                serial.Add(result[i].SS[3]);
                serial.Add(result[i].SS[4]);
                serial.Add(result[i].RT.ToString());
                serial.Add(result[i].SWeight[0]);
                serial.Add(result[i].SWeight[1]);
                serial.Add(result[i].SWeight[2]);
                serial.Add(result[i].SWeight[3]);
                serial.Add(result[i].SWeight[4]);
                //serial.Add(result[i].CorrectAnswer.ToString());
                serial.Add(result[i].UserAnswer.ToString());
            }

            addRecord(serial,
                Names.VOCSIMI_COLNAME, tablename,
                userID, result.Count);
        }

        public List<QRecUser> AcquireUserRecords()
        {
            List<QRecUser> retval = new List<QRecUser>();

            mDBOperator.Open();

            String SQL = 
                "SELECT * FROM USER_TABLE";

            if (mVersion == VERSION.MANAGER)
            {
                MySqlDataReader reader = (MySqlDataReader)mDBOperator.Query(SQL);
                
                while (reader.Read())
                {
                    QRecUser rec = new QRecUser();

                    rec.ID = reader.GetString(0);
                    rec.GroupMark = reader.GetString(1);
                    rec.Time = reader.GetString(2);
                    rec.Name = reader.GetString(3);
                    rec.Gender = reader.GetString(4);
                    rec.Age = reader.GetString(5);
                    rec.Health = reader.GetString(6);
                    rec.Qualif = reader.GetString(7);
                    rec.Job = reader.GetString(8);
                    rec.Other = reader.GetString(9);

                    retval.Add(rec);
                }

            }
            else if (mVersion == VERSION.STANDALONE)
            {
                SQLiteDataReader readerLite = (SQLiteDataReader)mDBOperator.Query(SQL);

                while (readerLite.Read())
                {
                    QRecUser rec = new QRecUser();

                    rec.ID = readerLite["ID"].ToString();
                    rec.GroupMark = (String)readerLite["GROUP_MARK"];
                    rec.Time = (String)readerLite["TIME"];
                    rec.Name = (String)readerLite["NAME"];
                    rec.Gender = (String)readerLite["GENDER"];
                    rec.Age = readerLite["AGE"].ToString();
                    rec.Health = (String)readerLite["HEALTH"];
                    rec.Qualif = (String)readerLite["QUALIF"];
                    rec.Job = (String)readerLite["JOB"];
                    rec.Other = (String)readerLite["OTHER"];

                    retval.Add(rec);
                }
            }

            mDBOperator.Close();

            return retval;
        }
    }
}

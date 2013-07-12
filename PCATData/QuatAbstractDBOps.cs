using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCATData
{
    abstract public class QuatAbstractDBOps
    {
        abstract public void ExecuteX(string sql);
        abstract public void Open();
        abstract public void Close();
        abstract public Object Query(string sql);
        abstract public bool TableExists(string tableName);
    }
}

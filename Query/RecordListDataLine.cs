using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Query
{
    public class RecordListDataLine
    {
        public String ID { get; set; }
        public String GroupMark { get; set; }
        public String TestTime { get; set; }
        public String SubjectName { get; set; }
        public String Gender { get; set; }
        public String Age { get; set; }

        public RecordListDataLine()
        { }

        public RecordListDataLine(String id, String groupMark,
            String testTime, String subjectName, String gender, String age)
        {
            GroupMark = groupMark;
            TestTime = testTime;
            SubjectName = subjectName;
            Gender = gender;
            Age = age;
            ID = id;
        }
    }
}

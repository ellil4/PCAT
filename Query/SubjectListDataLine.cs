using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Query
{
    public class SubjectListDataLine
    {
        public String ID { get; set; }
        public String GroupMark {get; set;}
        public String Status { get; set; }
        public String IP { get; set; }
        public String MachineName { get; set; }
        public String SubjectName { get; set; }

        public SubjectListDataLine()
        { }

        public SubjectListDataLine(String id, String groupMark, String status, 
            String ip, String machineName, String subjectName)
        {
            GroupMark = groupMark;
            Status = status;
            IP = ip;
            MachineName = machineName;
            SubjectName = subjectName;
            ID = id;
        }
    }
}

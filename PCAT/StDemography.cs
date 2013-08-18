using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest
{
    public class StDemography
    {
        public String Name;
        public String Gender;
        public int Age;
        public String Health;
        public String Education;
        public String Job;
        public String Note;
        public String Time;

        public String GenString()
        {
            return Name + "_" + Gender + "_" + Age + "_" + Health + "_" + Education + "_" + Job + "_" + Time;
        }
    }
}

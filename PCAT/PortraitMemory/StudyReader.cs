using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;




namespace FiveElementsIntTest.PortraitMemory
{
    class StudyReader
    {

        TabFetcher _tf;
        String _fPath = @"PortraitMemory\PortraitMemory_Study\study.txt";
        List<String> _tableLine;
        List<List<String>> _tableContent;

        public StudyReader()
        {
            _tf = new TabFetcher(_fPath, "\\t");

            _tableContent = new List<List<string>>();
        }

        public List<List<String>> GetStudyContent()
        {

            _tf.Open();

            loadTable();

            _tf.Close();

            return _tableContent;
        }

        private void loadTable()//
        {
            
            List<String> line;

            _tableLine = _tf.GetLineBy();

            while (_tableLine.Count > 0)
            {
                _tableLine = _tf.GetLineBy();

                line = new List<string>();

                line = _tableLine;

                _tableContent.Add(line);
            }

            _tableContent.RemoveAt(_tableContent.Count - 1);

           
        }










    }//class
}//namespace

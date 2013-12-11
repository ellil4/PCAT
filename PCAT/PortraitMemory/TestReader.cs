using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;

namespace FiveElementsIntTest.PortraitMemory
{
    class TestReader
    {
        TabFetcher _testTf;
        private String _testFilePath = @"PortraitMemory\PortraitMemory_test\test.txt";
        private List<String> _tableLine;
        private List<List<String>> _tableContent;
        public List<List<String>> _TestList;
        public List<List<String>> _ResultList;


        public TestReader()
        {
            _testTf = new TabFetcher(_testFilePath, "\\t");

            _tableContent = new List<List<string>>();

            _TestList = new List<List<string>>();

            _ResultList = new List<List<string>>();

            _testTf.Open();

            loadTable();

            _testTf.Close();

            needList();

           
        }

        public List<List<String>> GetTestContent()
        {

            //_testTf.Open();

            //loadTable();

            //_testTf.Close();

            return _tableContent;
        }

        private void loadTable()//
        {
            
            List<String> line;

            _tableLine = _testTf.GetLineBy();

            _ResultList.Add(_tableLine);

            while (_tableLine.Count > 0)
            {
                _tableLine = _testTf.GetLineBy();

                line = new List<string>();

                line = _tableLine;

                _tableContent.Add(line);
            }

            _tableContent.RemoveAt(_tableContent.Count - 1);

           
        }

        private void needList()
        {
            List<String> temp;
            List<String> rtemp;
            for (int i = 0; i < _tableContent.Count; i++)
            {
                temp = new List<string>();
                rtemp = new List<string>();

                for (int j = 0; j < _tableContent[i].Count; j++)
                {
                    if (j < 2)
                    {
                        temp.Add(_tableContent[i][j]);
                        rtemp.Add(_tableContent[i][j]);
                    }

                    if (j >= 2 && j < 8)
                    {
                        rtemp.Add(_tableContent[i][j]);
                    }
                    if (j >= 8)
                    {
                        temp.Add(_tableContent[i][j]);
                    }

                }
                _TestList.Add(temp);

                _ResultList.Add(rtemp);

            }
            _ResultList[0].RemoveRange(8,18);
        }

    }//class
}//namespace

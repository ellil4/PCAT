using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;


namespace FiveElementsIntTest.PortraitMemory
{

    class TestResult
    {
        
        TabCharter mTabCharter;//写入硬盘

        String file_Loca = ""; //系统时间文件地址
        
        private List<List<String>> _putout;

        private List<List<String>> _usersOption;

        public int _Account = 0;

        MainWindow mMainWindow;

        public TestResult(List<List<String>> outtep,List<List<String>> usemp, MainWindow mw)
        {
            mMainWindow = mw;
            file_Loca = @"Report\PortraitMemory_result\" + mMainWindow.mDemography.GenBriefString() + ".txt";
            mTabCharter = new TabCharter(file_Loca);//写入硬盘

            _putout = outtep;

            _usersOption = usemp;

            dealResult();
            
            outPutresult();
        }

        private void dealResult()
        {
            _putout[0].Add("Answer1");
            _putout[0].Add("Answer2");
            _putout[0].Add("Answer3");
            _putout[0].Add("ACC1");
            _putout[0].Add("ACC2");
            _putout[0].Add("ACC3");
            _putout[0].Add("RT");
            _putout[0].Add("Score");

            for (int i = 0; i < _usersOption.Count;i++ )
            {
                int score = 0;

                for (int j = 0; j < _usersOption[i].Count-1;j++ )
                {
                   
                    _putout[i + 1].Add(_usersOption[i][j]);
                }


                int length = _putout[i + 1].Count;

                if (_putout[i + 1][_putout[i + 1].Count - 6] == _putout[i + 1][_putout[i + 1].Count - 3])
                {
                    _putout[i + 1].Add("T");
                    score++;
                }
                else
                {
                    _putout[i + 1].Add("F");
                }
                

                if (_putout[i + 1][_putout[i + 1].Count - 6] == _putout[i + 1][_putout[i + 1].Count - 3])
                {
                    _putout[i + 1].Add("T");
                    score++;
                }
                else
                {
                    _putout[i + 1].Add("F");
                }

                
                if (_putout[i + 1][_putout[i + 1].Count - 6] == _putout[i + 1][_putout[i + 1].Count - 3])
                {
                    _putout[i + 1].Add("T");
                    score++;
                }
                else
                {
                    _putout[i + 1].Add("F");
                }

                _putout[i + 1].Add(_usersOption[i][_usersOption[i].Count -1] +"ms");

                _putout[i + 1].Add(score.ToString());

                if (score ==3)
                _Account++;
            }


        }

        private void outPutresult()
        {
            mTabCharter.Create(_putout[0]);
            
            for (int i = 1; i < _putout.Count; i++)
            {

                mTabCharter.Append(_putout[i]);

            }
           
        }






    }//class
}//namespace

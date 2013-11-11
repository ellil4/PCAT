using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;

namespace FiveElementsIntTest.SybSrh
{
    public class SybSrhFixedReader
    {
        TabFetcher tf;
   //    String fpath = @"SybSrh/sybsrhprac.txt";

       List<String> _Tableline;

       SybSrhVisualElem tar;

       SybSrhVisualElem sel;

       SybSrhItem mSybSrhItem;

       List<SybSrhItem> _test;

       public List<SybSrhItem> GetContext(String filepath)//   () 
       {
          //  tf = new TabFetcher(fpath, "\\t");  
                _test = new List<SybSrhItem>();

              tf = new TabFetcher(filepath, "\\t");

               tf.Open();

               loadTable();

                tf.Close();

                 return _test;
       }

        private void loadTable()//
        {
            List<List<String>> temp = new List<List<string>>();
         
            List<String> line;

            _Tableline = tf.GetLineBy();

            while (_Tableline.Count > 0)
              {
                  _Tableline = tf.GetLineBy();

                  line = new List<string>();

                  line = _Tableline;

                  temp.Add(line);
            }

            temp.RemoveAt(temp.Count - 1);

              loadContent(temp);
            
    
        }

        private void loadContent(List<List<String>> tm) 
        {
                List<SybSrhItem> rev = new List<SybSrhItem>();

                for (int i = 0; i < tm.Count;i++ )
                {
                          int j = 0;

                          mSybSrhItem = new SybSrhItem();
              
                          while(j < tm[i].Count - 3){

                             for (int y = 0; y < 2;y++ )
                               {
                                    tar = new SybSrhVisualElem();

                                    tar.Type = Convert.ToInt32(tm[i][j++]);

                                    tar.Index = Convert.ToInt32(tm[i][j++]);

                                    String tem =tm[i][j++];
                           
                                 if (tem == "1")

                                        tar.IfTrue = true;

                                    else if (tem == "0")

                                        tar.IfTrue = false;

                                    mSybSrhItem.Target[y] = tar;
                                 }

                             for (int p = 0; p < 5;p++)
                             {
                                 sel = new SybSrhVisualElem();

                                 sel.Type = Convert.ToInt32(tm[i][j++]);

                                 sel.Index = Convert.ToInt32(tm[i][j++]);

                                 String tem = tm[i][j++];
                        
                                 if (tem == "1")

                                     tar.IfTrue = true;

                                 else if (tem == "0")

                                     tar.IfTrue = false;

                                 mSybSrhItem.Selection[p] = sel;
                     
                             }

                        }//while 

                         rev.Add(mSybSrhItem);

                        } //for

                _test = rev;//

            }//loadcontent
            
    }//-----------class
}//spacename

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;

namespace FiveElementsIntTest.Paper
{
    class PaperReadFile
    {

        TabFetcher tf = new TabFetcher("Paper\\Paper_Test_Paper\\PF-C.txt", "\\s");//PF-C.txt
        
        public List<String> Surface = new List<string>();//表头
        List<String> Question_test = new List<string>(); //记录从文件里读取的毛数据
        
        List<String> Question = new List<String>();//题目、表的内容
       
        public List<String> line;

        

        public List<String> image_name = new List<string>();  //公共变量 图片名
        public int item_num = 0; //题号

       

        PagePaper vPagePaper;
        public PaperReadFile(PagePaper _PagePaper)
        {
            vPagePaper = _PagePaper;
            tf.Open();
            loadTableHead();

            
         }
        private void loadTableHead()//读取表头
        {

            line = tf.GetLineAt(vPagePaper.line_num_count);

            if (vPagePaper.line_num_count == 0)
            {

                for (int i = 0; i < line.Count; i++)
                {

                    if (line[i] != " " )
                    {
                        Surface.Add(line[i].ToString());
                    }
                }
               

            }
             vPagePaper.line_num_count++;

        }

        public void loadTableContent()
        {//读取每一道题
            Question_test.Clear();

            //if ( vPagePaper.line_num_count < vPagePaper.line_num )
            //{

                line = tf.GetLineAt(vPagePaper.line_num_count);

                if (vPagePaper.line_num_count < vPagePaper.line_num)
                {

                    for (int i = 1; i < (line.Count - 4); i++)
                    {
                        if (line[i] != " " && line[i] != "Pa" && line[i] != "PaS")
                        {
                            Question_test.Add(line[i].ToString());
                           
                           
                            //orga.Qnum.Foreground = Brushes.White;
                            //orga.Qnum.Content = "共" + (line_num -1)+ "道题：" + "第" + (line_num_count) + "题";
                            //  
                           
                        }
                        
                    }

                }// ---for

            //}
                RecordLine(Question_test);//显示题
        }
        private List<String> RecordLine(List<String> question)//每一题的内容 每一条的内容
        {
          //  String image_name = "";
            image_name.Clear();
           
            item_num = 0;
            
            for(int i=0;i<question.Count;i++){

               
               if( i < 4 )
               {
                   
                   
                   if (question[i].ToString() != "#")
                    {
                      image_name.Add(question[i].ToString());
                      item_num++;
                     
                    }

               }
               else if (i >= 4 && i <= question.Count - 1)
               {
                  
                    
                           image_name.Add(question[i].ToString());
                      

               }
              //  break;

             }

            return image_name;

        }

    }//类
}//---------命名空间

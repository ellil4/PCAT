using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace LibTabCharter
{
    class Program
    {
        static void Main(string[] args)
        {
            /*TabCharter ltc = new TabCharter("C:\\Users\\el\\Desktop\\dashes.txt");
            List<String> header = new List<string>();
            header.Add("汉字");
            header.Add("憨子");
            header.Add("汉子");
            header.Add("汗渍");
            ltc.Create(header);
            ltc.Append(header);*/

            TabFetcher tf = new TabFetcher("ss0.txt", "\\s");
            tf.Open();
            List<String> line;
            List<String> Surface0 = new List<string>();
            List<String> Question0 = new List<string>();


           
            List<String> Question1 = new List<string>();
            List<String> Question2 = new List<string>();
            List<String> Question3 = new List<string>();
            List<String> Question4 = new List<string>();
            List<String> Question5 = new List<string>();
            List<String> Question6 = new List<string>();
            List<String> Question7 = new List<string>();

            ArrayList Question = new ArrayList();
            List<String> Anstandard = new List<string>();


            line = tf.GetLineBy();
            //while((line = tf.GetLineBy()).Count != 0)
            //{
            Console.WriteLine(line.Count);
                for (int i = 0; i < line.Count; i++)
                {

                    if (i < 51 && line[i] != " ")
                    {
                        Surface0.Add(line[i].ToString());
                    }

                    else if (i >= 52 && i < 103)
                    {

                        Question0.Add(line[i].ToString());

                    }
                    else if (i >= 104 && i <= 154)
                    {
                        Question1.Add(line[i].ToString());
                    }
                    else if (i >= (52 * 3) && i <= (50 + 52 * 3))
                    {
                        Question2.Add(line[i].ToString());
                    }
                    else if (i >= (52 * 4) && i <= (50 + 52 * 4))
                    {
                        Question3.Add(line[i].ToString());
                    }
                    else if (i >= (52 * 5) && i <= (50 + 52 * 5))
                    {
                        Question4.Add(line[i].ToString());
                    }
                    else if (i >= (52 * 6) && i <= (50 + 52 * 6))
                    {
                        Question5.Add(line[i].ToString());
                    }
                    else if (i >= (52 * 7) && i <= (50 + 52 * 7))
                    {
                        Question6.Add(line[i].ToString());
                    }
                    else if (i >= (52 * 8) && i <= (50 + 52 * 8))
                    {
                        Question7.Add(line[i].ToString());
                    }

                }

                Question.Add(Question0);
                Question.Add(Question1);
                Question.Add(Question2);
                Question.Add(Question3);
                Question.Add(Question4);
                Question.Add(Question5);
                Question.Add(Question6);
                Question.Add(Question7);
                Anstandard.Add(Question0[Question0.Count - 4]);
                Anstandard.Add(Question1[Question1.Count - 4]);
                Anstandard.Add(Question2[Question2.Count - 4]);
                Anstandard.Add(Question3[Question3.Count - 4]);
                Anstandard.Add(Question4[Question4.Count - 4]);
                Anstandard.Add(Question5[Question5.Count - 4]);
                Anstandard.Add(Question6[Question6.Count - 4]);
                
                tf.Close();
                Console.WriteLine();
                Console.WriteLine("第一行"+Surface0.Count+"列");
                for (int i = 0; i < Anstandard.Count; i++)
                {
                    Console.Write(Anstandard[i]);

                    Console.Write("\t");


                }

        //        for (int itext=0; itext < Question.Count; itext++)
        //        {

        //        List<String> ss = (List<String>)Question[itext];
                

        //        Console.WriteLine("第" + (itext + 1) + "题：" + "有" + ss.Count + "列：");
        //             for (int j = 0; j < ss.Count; j++)
        //        {
        //            if (ss[j].ToString() == "#")
        //            {

                       
        //                Console.Write(ss[j]);

        //                Console.Write("\t");
        //            }
        //            else if (ss[j].ToString() != "#")
        //            {
        //                Console.Write(ss[j]);
        //             //   Console.Write(Surface0[j]);
        //                Console.Write("\t");
        //            }
        //        }
                   
        //}

                Console.ReadLine();//持续显示窗口
          //  }

            tf.Close();
        }
    }
}

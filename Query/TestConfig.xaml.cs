using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using FiveElementsIntTest;

namespace Query
{
    /// <summary>
    /// TestConfig.xaml 的互動邏輯
    /// </summary>
    public partial class TestConfig : Window
    {
        public static String mPath = 
            AppDomain.CurrentDomain.BaseDirectory + "testConfig";

        private static void existenceOp()
        {
            if (!File.Exists(mPath))
            {
                (File.CreateText(mPath)).Close(); ;
            }
        }

        public TestConfig()
        {
            InitializeComponent();

            existenceOp();

            FileStream fs = 
                new FileStream(mPath, FileMode.Open, FileAccess.Read);

            //init from config file
            StreamReader sr = new StreamReader(fs);
            String line;
            
            while ((line = sr.ReadLine()) != null)
            {
                amLBChosenList.Items.Add(line);
            }

            updateWaitList();

            sr.Close();
            fs.Close();
        }

        public static List<TestType> GetTestArrayFromFile()
        {
            existenceOp();

            List<TestType> retval = new List<TestType>();

            FileStream fs =
                new FileStream(mPath, FileMode.Open, FileAccess.Read);

            //init from config file
            StreamReader sr = new StreamReader(fs);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                retval.Add((TestType)FEITStandard.GetTestIndexByName(line));
            }

            sr.Close();
            fs.Close();

            return retval;
        }

        public static List<String> GetTestStrArrayFromFile()
        {
            List<String> retval = new List<string>();
            List<TestType> tests = GetTestArrayFromFile();
            for (int i = 0; i < tests.Count; i++)
            {
                retval.Add(tests[i].ToString());
            }

            return retval;
        }

        public List<TestType> GetTestArray()
        {
            List<TestType> retval = new List<TestType>();

            for (int i = 0; i < amLBChosenList.Items.Count; i++)
            {
                int idx = 
                    FEITStandard.GetTestIndexByName(amLBChosenList.Items[i].ToString());

                TestType type = (TestType)idx;

                retval.Add(type);
            }

            return retval;
        }

        private bool hasChosen(int index)
        {
            bool retval = false;

            for (int i = 0; i < amLBChosenList.Items.Count; i++)
            {
                if (FEITStandard.TEST_TITLE[index].Equals(amLBChosenList.Items[i].ToString()))
                {
                    retval = true;
                    break;
                }
            }

            return retval;
        }

        private void updateWaitList()
        {
            for (int j = 0; j < FEITStandard.TEST_TITLE.Length; j++)
            {
                if (!hasChosen(j))
                    amLBWaitList.Items.Add(FEITStandard.TEST_TITLE[j]);
            }
        }

        private void updateConfigFile()
        {
            FileStream fs = 
                new FileStream(mPath, FileMode.Truncate, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < amLBChosenList.Items.Count; i++)
            {
                sw.WriteLine(amLBChosenList.Items[i].ToString());
            }

            sw.Close();
            fs.Close();
        }

        private void amBtnPick_Click(object sender, RoutedEventArgs e)
        {
            int menuIdx = amLBWaitList.SelectedIndex;
            if (menuIdx != -1)
            {
                String itemText = amLBWaitList.Items[menuIdx].ToString();
                amLBWaitList.Items.RemoveAt(menuIdx);
                amLBChosenList.Items.Add(itemText);

                if (menuIdx < amLBWaitList.Items.Count - 1)
                {
                    amLBWaitList.SelectedIndex = menuIdx;
                }
                else
                {
                    amLBWaitList.SelectedIndex = amLBWaitList.Items.Count - 1;
                }
            }
        }

        private void amBtnUnPick_Click(object sender, RoutedEventArgs e)
        {
            int menuIdx = amLBChosenList.SelectedIndex;
            if (menuIdx != -1)
            {
                String itemText = amLBChosenList.Items[menuIdx].ToString();
                amLBChosenList.Items.RemoveAt(menuIdx);
                amLBWaitList.Items.Add(itemText);

                if (menuIdx < amLBChosenList.Items.Count - 1)
                {
                    amLBChosenList.SelectedIndex = menuIdx;
                }
                else
                {
                    amLBChosenList.SelectedIndex = amLBChosenList.Items.Count - 1;
                }
            }
        }

        private void amBtnUp_Click(object sender, RoutedEventArgs e)
        {
            int menuIdx = amLBChosenList.SelectedIndex;

            if (menuIdx > 0 && menuIdx != -1)
            {
                String itemText = amLBChosenList.Items[menuIdx].ToString();
                amLBChosenList.Items.RemoveAt(menuIdx);
                amLBChosenList.Items.Insert(menuIdx - 1, itemText);
                amLBChosenList.SelectedIndex = menuIdx - 1;
            }
        }

        private void amBtnDown_Click(object sender, RoutedEventArgs e)
        {
            int menuIdx = amLBChosenList.SelectedIndex;

            if (menuIdx < amLBChosenList.Items.Count - 1 && menuIdx != -1)
            {
                String itemText = amLBChosenList.Items[menuIdx].ToString();
                amLBChosenList.Items.RemoveAt(menuIdx);
                amLBChosenList.Items.Insert(menuIdx + 1, itemText);
                amLBChosenList.SelectedIndex = menuIdx + 1;
            }
        }

        private void amBtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            updateConfigFile();
            Close();
        }

        private void amBtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

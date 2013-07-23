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
using LibTabCharter;

namespace FiveElementsIntTest.CtSpan
{
    /// <summary>
    /// DevToolWnd.xaml 的互動邏輯
    /// </summary>
    public partial class DevToolWnd : Window
    {
        public Organizer mOrg;
        public StGraphItem mItem;
        public TabCharter mCharter;
        private List<CSPoint> mLocations;

        public DevToolWnd(Organizer org)
        {
            InitializeComponent();
            mOrg = org;
            mCharter = new TabCharter(
                System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "CTSpanDev.txt");
            List<String> header = new List<string>();
            header.Add("TarCount");
            header.Add("InterCircleCount");
            header.Add("InterTriCount");
            header.Add("DistanceTar");
            header.Add("DistanceComm");

            for(int i = 0; i < 20; i++)
            {
                header.Add("Tar" + i);
            }

            for(int i = 0; i < 20; i++)
            {
                header.Add("Tri" + i);
            }

            for(int i = 0 ; i < 20; i++)
            {
                header.Add("Cir" + i);
            }

            mCharter.Create(header);
        }

        private void amBtnDo_Click(object sender, RoutedEventArgs e)
        {
            StGraphItem item = new StGraphItem();
            item.DistanceComm = short.Parse(amTBDisComm.Text);
            item.DistanceTar = short.Parse(amTBDisTar.Text);
            item.InterCircleCount = int.Parse(amTBInterCir.Text);
            item.InterTriCount = int.Parse(amTBInterTri.Text);
            item.TarCount = int.Parse(amTBTarCount.Text);

            mLocations = mOrg.ShowGraph(item);
            mItem = item;
        }

        private void amBtnSave_Click(object sender, RoutedEventArgs e)
        {
            List<String> content = new List<string>();
            content.Add(mItem.TarCount.ToString());
            content.Add(mItem.InterCircleCount.ToString());
            content.Add(mItem.InterTriCount.ToString());
            content.Add(mItem.DistanceTar.ToString());
            content.Add(mItem.DistanceComm.ToString());

            int filled = 0;

            for(int i = 0; i < mLocations.Count; i++)
            {
                if (mLocations[i].type == 0)
                {
                    content.Add(mLocations[i].x.ToString() + "," + mLocations[i].y.ToString());
                    filled++;
                }

                if (i == mLocations.Count - 1)
                {
                    for (int j = 0; j < (20 - filled); j++)
                    {
                        content.Add("void");
                    }
                }
            }


            filled = 0;
            for(int i = 0; i < mLocations.Count; i++)
            {
                if (mLocations[i].type == 1)
                {
                    content.Add(mLocations[i].x.ToString() + "," + mLocations[i].y.ToString());
                    filled++;
                }

                if (i == mLocations.Count - 1)
                {
                    for (int j = 0; j < (20 - filled); j++)
                    {
                        content.Add("void");
                    }
                }
            }

            filled = 0;
            for(int i = 0; i < mLocations.Count; i++)
            {
                if (mLocations[i].type == 2)
                {
                    content.Add(mLocations[i].x.ToString() + "," + mLocations[i].y.ToString());
                    filled++;
                }

                if (i == mLocations.Count - 1)
                {
                    for (int j = 0; j < (20 - filled); j++)
                    {
                        content.Add("void");
                    }
                }
            }

            mCharter.Append(content);
        }
    }
}

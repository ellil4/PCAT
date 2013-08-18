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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FiveElementsIntTest.PairedAsso
{
    /// <summary>
    /// PagePairedAsso.xaml 的互動邏輯
    /// </summary>
    public partial class PagePairedAsso : Page
    {
        public MainWindow mMainWindow;

        public static int mGroupLen = 18;
        public int mGrpAt = 0;
        public int mItemInGrpAt = 0;

        public PagePairedAsso(MainWindow mw)
        {
            InitializeComponent();
            mMainWindow = mw;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref amBaseCanvas);
            CompChinese9Cells comp = new CompChinese9Cells(this);
            String[] str = new String[] {"人", "口", "大", "小", "汉", "字", "简", "繁", "英"};
            comp.SetCharas(str);
            amBaseCanvas.Children.Add(comp);
            Canvas.SetTop(comp, 300);
            Canvas.SetLeft(comp, 300);

        }

        public void clearAll()
        {
            amBaseCanvas.Children.Clear();
        }

        public void next()
        { }
    }
}

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

namespace FiveElementsIntTest.Cube
{
    /// <summary>
    /// Page1.xaml 的互動邏輯
    /// </summary>
    public partial class PageCube : Page
    {
        public MainWindow mMainWindow;

        public PageCube(MainWindow _mainWindow)
        {
            InitializeComponent();
            mMainWindow = _mainWindow;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageCommon.InitCommonPageElements(ref mBaseCanvas);
            ClearAll();

            loadTestPage();
        }

        private void loadTestPage()
        {
            OrganizerTrailCubes org = new OrganizerTrailCubes(this);

            //test code
            org.SetFirstGraphTexture('A', FiveElementsIntTest.Properties.Resources.HalfTexDwonLeft);
            org.SetFirstGraphTexture('B', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetFirstGraphTexture('C', FiveElementsIntTest.Properties.Resources.ArrowTexRight);
            org.SetFirstGraphTexture('D', FiveElementsIntTest.Properties.Resources.HalfTexUpLeft);
            
            org.SetFirstGraphTexture('I', FiveElementsIntTest.Properties.Resources.HalfTexDwonRight);
            org.SetFirstGraphTexture('J', FiveElementsIntTest.Properties.Resources.HalfTexDwonLeft);
            org.SetFirstGraphTexture('K', FiveElementsIntTest.Properties.Resources.AxeTex);
            org.SetFirstGraphTexture('L', FiveElementsIntTest.Properties.Resources.HalfTexUpLeft);

            org.SetFirstGraphTexture('E', FiveElementsIntTest.Properties.Resources.HalfTexDwonRight);
            org.SetFirstGraphTexture('F', FiveElementsIntTest.Properties.Resources.HalfTexDwonLeft);

            org.SetSelectionGraph(0, 'A', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetSelectionGraph(0, 'D', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetSelectionGraph(0, 'B', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);
            org.SetSelectionGraph(0, 'C', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);

            org.SetSelectionGraph(1, 'B', FiveElementsIntTest.Properties.Resources.AxeTex);
            org.SetSelectionGraph(1, 'D', FiveElementsIntTest.Properties.Resources.AxeTex);

            org.SetSelectionGraph(2, 'B', FiveElementsIntTest.Properties.Resources.AxeTex);
            org.SetSelectionGraph(2, 'C', FiveElementsIntTest.Properties.Resources.AxeTex);
            org.SetSelectionGraph(2, 'A', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetSelectionGraph(2, 'D', FiveElementsIntTest.Properties.Resources.ArrowTexUp);


            org.SetSelectionGraph(3, 'A', FiveElementsIntTest.Properties.Resources.AxeTex);
            org.SetSelectionGraph(3, 'C', FiveElementsIntTest.Properties.Resources.AxeTex);
            org.SetSelectionGraph(3, 'D', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetSelectionGraph(3, 'B', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);

            org.SetSelectionGraph(4, 'A', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetSelectionGraph(4, 'D', FiveElementsIntTest.Properties.Resources.ArrowTexUp);

            org.SetSelectionGraph(5, 'A', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetSelectionGraph(5, 'C', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);
            org.SetSelectionGraph(5, 'B', FiveElementsIntTest.Properties.Resources.AxeTex);
            org.SetSelectionGraph(5, 'D', FiveElementsIntTest.Properties.Resources.AxeTex);

            org.SetSelectionGraph(6, 'A', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);
            org.SetSelectionGraph(6, 'D', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);

            org.SetSelectionGraph(7, 'A', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);
            org.SetSelectionGraph(7, 'D', FiveElementsIntTest.Properties.Resources.ArrowTexLeft);
            org.SetSelectionGraph(7, 'B', FiveElementsIntTest.Properties.Resources.ArrowTexUp);
            org.SetSelectionGraph(7, 'C', FiveElementsIntTest.Properties.Resources.ArrowTexUp);

            org.SetRotatingGraph("right", true);
        }

        public void ClearAll()
        {
            mBaseCanvas.Children.Clear();
            PageCommon.AddAuxBDR(ref mBaseCanvas, ref mAuxBorder, ref mAuxBorder1024, ref mMainWindow);
        }
    }
}

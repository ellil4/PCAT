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
using System.Timers;



namespace FiveElementsIntTest.PortraitMemory
{
    /// <summary>
    /// MemoryControl.xaml 的交互逻辑
    /// </summary>
    public partial class MemoryControl : UserControl
    {

        private static int actual_x =30;
        
      //  private static int actual_y = 0;//

        Image portrailImage;

        /*********************************信息******************************/
        StudyReader mStudyReader;
        String _imageName;
        String _personName;
        String _personJob;
        String _personLike;
        public List<List<String>> temp;
        /*******************************计时器***********************************/

        private Timer _t_Study;
        private Timer _t_blank;
        private int _inferCount = -1;
        private bool _inferControl = false;

        private int imagewith = 270;
        private int imageheight = 350;

        public MemoryControl()
        {
            InitializeComponent();
            MemoryCanvas.Height = 600;
            MemoryCanvas.Width = 800;
            mStudyReader = new StudyReader();
            temp = mStudyReader.GetStudyContent();
           
          
        }
        public void displayInformation(bool choose)
        {
            studyTime();
            blankTime();
            if (!choose)
            {
               
                _t_blank.Start(); 
                
              //  _t_Study.Start();
              
            }
            else
            {
                _inferControl = true;
             //   studyTime();
                _t_blank.Start(); 
                
               // _t_Study.Start();
                
            }


        }


        private void layoutContent()
        {
           layoutImage();
            layoutName();
            layoutJob();
            layoutLike();
        }

        private void inputInformationOne(int i)
        {
             
            _imageName  = temp[i][1];
            _personName = temp[i][2];
            _personJob  = temp[i][3];
            _personLike = temp[i][4];
               
               
        }

        private void inputInformationTwo()
        {
            for (int i = temp.Count / 2; i < temp.Count; i++)
            {
                int j = 1;
                while (j < temp[i].Count)
                {
                    _imageName = temp[i][j];
                    _personName = temp[i][j++];
                    _personJob = temp[i][j++];
                    _personLike = temp[i][j++];

                }

            }

        }

        private void layoutImage()//Canvas.Left="85" Canvas.Top="95"<Image  Height="395" Name="portrailImage" Stretch="Fill" Width="250" />
        {
            portrailImage = new Image();
            portrailImage.Stretch = Stretch.Fill;


            System.Drawing.Image img = System.Drawing.Image.FromFile("PortraitMemory\\PortraitMemory_image\\" + _imageName + ".jpg");
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);

            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(imagewith, imageheight));//FromEmptyOptions() 源图像大小
            portrailImage.Source = WpfBitmap;

            
            MemoryCanvas.Children.Add(portrailImage);
            Canvas.SetLeft(portrailImage, 150 );
            Canvas.SetTop(portrailImage,  120);

        }


        private void layoutName() //Canvas.Left="470" Canvas.Top="187"
        {
            _nameLabel.Content = _personName;
            Canvas.SetLeft(_nameLabel, actual_x + 430);
            Canvas.SetTop(_nameLabel, 227);
        }
        private void layoutJob() //Canvas.Left="470" Canvas.Top="287"
        {
            _jobLabel.Content = _personJob;
            Canvas.SetLeft(_jobLabel, actual_x + 430);
            Canvas.SetTop(_jobLabel, 287);

        }



        private void layoutLike()//Canvas.Left="470" Canvas.Top="387"
        {
            _likeLabel.Content = _personLike;
            Canvas.SetLeft(_likeLabel, actual_x + 430);
            Canvas.SetTop(_likeLabel, 347);
        }

        

        private void clearControl()
        {
            MemoryCanvas.Children.Remove(portrailImage);
            _nameLabel.Content = "";
            _jobLabel.Content = "";
            _likeLabel.Content = "";
        }

        private void studyTime()
        {
            _t_Study = new Timer(6000);
            _t_Study.AutoReset = false;
            ///////
            _t_Study.Elapsed += new ElapsedEventHandler(_t_Study_Elapsed);



        }

        void _t_Study_Elapsed(object sender, ElapsedEventArgs e)
        {
            MemoryCanvas.Dispatcher.Invoke(new displaystudytime(st_processing));
        }
        private delegate void displaystudytime();

        private void st_processing() //超时跳出测试
        {
            clearControl();
            
            _t_Study.Stop();
            _t_blank.Start();

        }
        private void blankTime()
        {
            _t_blank = new Timer(2000);
            _t_blank.AutoReset = false;
            _t_blank.Enabled = true;
            _t_blank.Elapsed += new ElapsedEventHandler(_t_blank_Elapsed);
        }

        void _t_blank_Elapsed(object sender, ElapsedEventArgs e)
        {

            MemoryCanvas.Dispatcher.Invoke(new displayblanktime(bl_processing));
        }
        private delegate void displayblanktime();

        private void bl_processing() //超时跳出测试
        {
            if (!_inferControl)
            {
                _inferCount++;
                
                if (_inferCount < temp.Count / 2)
                {
                    inputInformationOne(_inferCount);
                    layoutContent();

                    _t_blank.Stop();
                    _t_Study.Enabled = true;//////
                    _t_Study.Start();
                }
                else
                {
                    _t_Study.Stop();
                    _t_blank.Stop();
                    
                    _inferCount--;
                }

            }
            else
            {
                
                _inferCount++;

                if (_inferCount >= temp.Count / 2 && _inferCount < temp.Count)
                {
                   inputInformationOne(_inferCount);
                    layoutContent();

                    _t_blank.Stop();
                    _t_Study.Enabled = true;//////
                    _t_Study.Start();
                }
                else
                {
                    _t_Study.Stop();
                    _t_blank.Stop();
                   
                }

            }

        }

        public void Close_Thread()
        {
            _t_blank.Dispose();
            _t_Study.Dispose();
        }

    }//class
}//namespace

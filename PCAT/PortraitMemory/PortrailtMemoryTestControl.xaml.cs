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


namespace FiveElementsIntTest.PortraitMemory
{
    /// <summary>
    /// PortrailtMemoryTestControl.xaml 的交互逻辑
    /// </summary>
    public partial class PortrailtMemoryTestControl : UserControl
    {
        //private static int actual_x = 0;
        
        //private static int actual_y = 0;

        Image portrailImage;

        

        private Label _useLabel;
        private Label _recodernameLabel;
        private Label _recoderjobLabel;
        private Label _recoderlikeLabel;

       // private Label temp;

        private List<Label> _nameLabel;
        private List<Label> _jobLabel;
        private List<Label> _likeLabel;
        private int _left = 12;
        private int _usualLeft = 12;
        private int _leftadd = 107;
        private int _top = 11;
        private int _topadded = 70;

        private bool _namepanel = true;//控制版面锁定
        private bool _jobpanel = true;
        private bool _likepanel = true;
        private bool _namecontrol = false;
        private bool _jobcontrol = false;
        private bool _likecontrol = false;

        private int imagewith = 270;
        private int imageheight = 350;

        PagePortrailtMemory vPagePortrailtMemory;

        public PortrailtMemoryTestControl(PagePortrailtMemory _pm)
        {
            InitializeComponent();
            _nameLabel = new List<Label>();
            _jobLabel = new List<Label>();
            _likeLabel = new List<Label>();
            vPagePortrailtMemory = _pm;
            memorytestCanvas.Width = 800;
            memorytestCanvas.Height = 600;


            Canvas.SetLeft(picturecanvas, memorytestCanvas.Width * 0.08);//横向
            Canvas.SetTop(picturecanvas, memorytestCanvas.Height * 0.15);

            Canvas.SetLeft(namecanvas, memorytestCanvas.Width *0.54);//横向
            Canvas.SetTop(namecanvas, memorytestCanvas.Height *0.15);

            Canvas.SetLeft(jobcanvas, memorytestCanvas.Width*0.54);//横向
            Canvas.SetTop(jobcanvas, memorytestCanvas.Height*0.37);

            Canvas.SetLeft(likecanvas, memorytestCanvas.Width *0.54);//横向
            Canvas.SetTop(likecanvas, memorytestCanvas.Height*0.59);

            Canvas.SetLeft(nametip, memorytestCanvas.Width * 0.42);//横向
            Canvas.SetTop(nametip, memorytestCanvas.Height * 0.24);

            Canvas.SetLeft(jobtip, memorytestCanvas.Width * 0.42);//横向
            Canvas.SetTop(jobtip, memorytestCanvas.Height * 0.44);

            Canvas.SetLeft(liketip, memorytestCanvas.Width * 0.42);//横向
            Canvas.SetTop(liketip, memorytestCanvas.Height * 0.64);
        }
        public void LayoutTestPage(List<String> temp)
        {
            _left = 12;
            _usualLeft = 12;

            
            List<String> name = new List<string>();
            List<String> job = new List<string>();
            List<String> like = new List<string>();
            layoutPortrailtImage(temp[1]);
                for (int i = 2; i < temp.Count ;i++ )
                {
                    
                   if(i<8) name.Add(temp[i]);
                  
                   else if(i>=8 && i<14) job.Add(temp[i]);
                   
                   else if(i>= 14)like.Add(temp[i]);

                }
                layoutNameLabel(name);
                layoutJobLabel(job);
                layoutLikeLabel(like);
                _namecontrol = false;
                _jobcontrol = false;
                _likecontrol = false;

                vPagePortrailtMemory.SetCountDowner();
                
       }
           
      

        private void layoutPortrailtImage(String path)
        {
            portrailImage = new Image();
            portrailImage.Stretch = Stretch.Fill;

            System.Drawing.Image img = System.Drawing.Image.FromFile("PortraitMemory\\PortraitMemory_image\\"+ path +".jpg");
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);

            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(imagewith, imageheight));//FromEmptyOptions() 源图像大小
            portrailImage.Source = WpfBitmap;
            


            picturecanvas.Children.Add(portrailImage);
            Canvas.SetLeft(portrailImage,0 );//横向
            Canvas.SetTop(portrailImage, 0);//纵向 475 FEITStandard.PAGE_BEG_Y +30 

        }
        private void layoutNameLabel(List<String> names)
        {
            for (int i = 0; i < 6;i++ )
            {
                labelAttribute();
                _useLabel.Content = names[i];
                _nameLabel.Add(_useLabel);

                namecanvas.Children.Add(_useLabel);

                if (i < 3)
                {
                    Canvas.SetTop(_useLabel, _top);//

                    Canvas.SetLeft(_useLabel, _left);//

                    _left = _left + _leftadd;
                }
                else
                {
                    Canvas.SetTop(_useLabel, _topadded);//

                    Canvas.SetLeft(_useLabel, _usualLeft);//

                    _usualLeft = _usualLeft + _leftadd;
                }

                 
            
            }


        }

        private void layoutJobLabel(List<String> jobs)
        {
            _left = 12;
            _usualLeft = 12;
            for (int i = 0; i < 6; i++)
            {
                labelAttribute();

                _useLabel.Content = jobs[i];
                _jobLabel.Add(_useLabel);

                jobcanvas.Children.Add(_useLabel);

                if (i < 3)
                {
                    Canvas.SetTop(_useLabel, _top);//

                    Canvas.SetLeft(_useLabel, _left);//

                    _left = _left + _leftadd;
                }
                else
                {
                    Canvas.SetTop(_useLabel, _topadded);//

                    Canvas.SetLeft(_useLabel, _usualLeft);//

                    _usualLeft = _usualLeft + _leftadd;
                }

            }
        }

        private void layoutLikeLabel(List<String> likes)
        {
            _left = 12;
            _usualLeft = 12;

            for (int i = 0; i < 6; i++)
            {
                labelAttribute();

                _useLabel.Content = likes[i];

                _likeLabel.Add(_useLabel);

                likecanvas.Children.Add(_useLabel);

                if (i < 3)
                {
                    Canvas.SetTop(_useLabel, _top);//

                    Canvas.SetLeft(_useLabel, _left);//

                    _left = _left + _leftadd;
                }
                else
                {
                    Canvas.SetTop(_useLabel, _topadded);//

                    Canvas.SetLeft(_useLabel, _usualLeft);//

                    _usualLeft = _usualLeft + _leftadd;
                }

            }
        }

        private void labelAttribute()// Name="label1" 
        {
            _useLabel = new Label();
            _useLabel.Height = 51;
            _useLabel.Width = 98;
            _useLabel.BorderThickness = new Thickness(3.0);
            _useLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            _useLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            _useLabel.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            _useLabel.FontFamily = new FontFamily("Adobe Heiti Std");
            _useLabel.FontSize = 24.0;

            _useLabel.MouseDown += new MouseButtonEventHandler(_useLabel_MouseDown);
            _useLabel.MouseEnter += new MouseEventHandler(_useLabel_MouseEnter);
            _useLabel.MouseLeave += new MouseEventHandler(_useLabel_MouseLeave);
            
        }

        void _useLabel_MouseLeave(object sender, MouseEventArgs e)
        {
           
                if (!_namecontrol)
                {
                    for (int i = 0; i < _nameLabel.Count; i++)
                    {
                        if (_nameLabel[i].Equals(sender))
                        {
                            _nameLabel[i].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        }
                    }
                }
                if (!_jobcontrol)
                {
                    for (int i = 0; i < _jobLabel.Count; i++)
                    {
                        if (_jobLabel[i].Equals(sender))
                        {
                            _jobLabel[i].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        }
                    }
                }
                if (!_likecontrol)
                {
                    for (int i = 0; i < _likeLabel.Count; i++)
                    {
                        if (_likeLabel[i].Equals(sender))
                        {
                            _likeLabel[i].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        }
                    }
                }
            
        }

        void _useLabel_MouseEnter(object sender, MouseEventArgs e)
        {

            if (!_namecontrol)
                {
                    for (int i = 0; i < _nameLabel.Count; i++)
                    {
                        if (_nameLabel[i].Equals(sender))
                        {
                            _nameLabel[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 69, 0));


                        }
                    }

                }
            if (!_jobcontrol)
                {
                    for (int i = 0; i < _nameLabel.Count; i++)
                    {
                        if (_jobLabel[i].Equals(sender))
                        {
                            _jobLabel[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 69, 0));


                        }
                    }

                }
            if (!_likecontrol)
                {
                    for (int i = 0; i < _likeLabel.Count; i++)
                    {
                        if (_likeLabel[i].Equals(sender))
                        {
                            _likeLabel[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 69, 0));


                        }
                    }

                }
            
        }

        void _useLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_namepanel)
                {
                    vPagePortrailtMemory._IsOption = true;

                    for (int i = 0; i < _nameLabel.Count; i++)
                    {
                        
                        
                        if (_nameLabel[i].Equals(sender))
                        {
                            if (_recodernameLabel != null)
                            {
                                if (_recodernameLabel.Equals(sender)) _recodernameLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                else _recodernameLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                            }
                           
                           _nameLabel[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 69, 0));

                           
                          
                            _recodernameLabel = _nameLabel[i];

                            _namecontrol = true;//控制锁定面板

                            vPagePortrailtMemory._NameAnswer = (i+1).ToString();
                        }
                    }
                }
                if (_jobpanel)
                {
                    vPagePortrailtMemory._IsOption = true;

                    for (int j = 0; j < _jobLabel.Count; j++)
                    {

                        if (_jobLabel[j].Equals(sender))
                        {
                            if (_recoderjobLabel != null)
                            {
                                if (_recoderjobLabel.Equals(sender)) _recoderjobLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                else _recoderjobLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                            }
                            _jobLabel[j].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 69, 0));
                            

                           

                            _recoderjobLabel = _jobLabel[j];
                           
                            _jobcontrol = true;

                            vPagePortrailtMemory._JobAnswer = (j+7).ToString();
                         //   _namepanel = false;
                        }
                    }
                }
                if (_likepanel)
                {
                    vPagePortrailtMemory._IsOption = true;

                    for (int k = 0; k < _likeLabel.Count; k++)
                    {
                        

                        if (_likeLabel[k].Equals(sender))
                        {
                            if (_recoderlikeLabel != null)
                            {
                                if (_recoderlikeLabel.Equals(sender)) _recoderlikeLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                else _recoderlikeLabel.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                            }
                            
                            _likeLabel[k].BorderBrush = new SolidColorBrush(Color.FromRgb(255,69, 0));

                            _recoderlikeLabel = _likeLabel[k];

                            _likecontrol = true;

                            vPagePortrailtMemory._LikeAnswer = (k+13).ToString();
                            
                        }
                    }
                }

            }
        }

        public void ClearLists()
        {

          _nameLabel.Clear();
          _jobLabel.Clear();
          _likeLabel.Clear();


        }

        


        public void HideComponent()
        {
            picturecanvas.Visibility = System.Windows.Visibility.Hidden;
            namecanvas.Visibility = System.Windows.Visibility.Hidden;
            jobcanvas.Visibility = System.Windows.Visibility.Hidden;
            likecanvas.Visibility = System.Windows.Visibility.Hidden;
           
            nametip.Visibility = System.Windows.Visibility.Hidden;
            jobtip.Visibility = System.Windows.Visibility.Hidden;
            liketip.Visibility = System.Windows.Visibility.Hidden;
        }

        public void DisplayComponent()
        {
            vPagePortrailtMemory._tip_display.Visibility = System.Windows.Visibility.Hidden;
            picturecanvas.Visibility = System.Windows.Visibility.Visible;
            namecanvas.Visibility = System.Windows.Visibility.Visible;
            jobcanvas.Visibility = System.Windows.Visibility.Visible;
            likecanvas.Visibility = System.Windows.Visibility.Visible;
            nametip.Visibility = System.Windows.Visibility.Visible;
            jobtip.Visibility = System.Windows.Visibility.Visible;
            liketip.Visibility = System.Windows.Visibility.Visible;
        }

        





    }//class
}//namespace

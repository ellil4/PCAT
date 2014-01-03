﻿using System;
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

namespace FiveElementsIntTest.VocabCommon
{
    /// <summary>
    /// PageTitle.xaml 的互動邏輯
    /// </summary>
    public partial class PageTitle : UserControl
    {
        PageVocabCommon mPage;
        public PageTitle(PageVocabCommon pg)
        {
            InitializeComponent();
            mPage = pg;
        }

        private void amStartBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mPage.TestStart();
        }
    }
}

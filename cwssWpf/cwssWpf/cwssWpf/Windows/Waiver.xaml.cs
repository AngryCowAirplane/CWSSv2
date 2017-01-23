﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace cwssWpf.Windows
{
    /// <summary>
    /// Interaction logic for Waiver.xaml
    /// </summary>
    public partial class Waiver : Window
    {
        public Waiver()
        {
            var waiverPath = System.IO.Path.Combine(Environment.CurrentDirectory, "AppData", @"CW Acknowledgement of Risk and Sign-In Sheet.pdf");

            InitializeComponent();
            webBrowser.Navigate("file:///" + waiverPath);
        }
    }
}
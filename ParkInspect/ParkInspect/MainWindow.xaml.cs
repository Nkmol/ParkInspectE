﻿using System.Windows;
using MahApps.Metro.Controls;
using ParkInspect.ViewModel;
using MahApps.Metro.Controls.Dialogs;

namespace ParkInspect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void ShowLoginDialog(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Main.showLoginDialog(this);
        }
    }
}
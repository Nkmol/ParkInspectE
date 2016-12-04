using System.Collections.Generic;
using MahApps.Metro.Controls;

namespace ParkInspect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ExportView : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public ExportView()
        {
            InitializeComponent();
        }

        public void FillGrid<T>(IEnumerable<T> data)
        {
            this.ExportGrid.ItemsSource = data;
        }



    }
}

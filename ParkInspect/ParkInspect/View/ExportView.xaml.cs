using System.Collections.Generic;
using MahApps.Metro.Controls;
using ParkInspect.Services;
using ParkInspect.ViewModel;

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

        public void FillGrid<T>(IEnumerable<T> data, DataService service)
        {
            this.ExportGrid.ItemsSource = data;
            ((ExportViewModel)this.DataContext).SetService(service);
        }



    }
}

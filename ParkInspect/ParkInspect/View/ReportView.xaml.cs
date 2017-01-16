using System.Windows;
using MahApps.Metro.Controls;
using ParkInspect.ViewModel;
using Syncfusion.RDL.DOM;
using Syncfusion.Windows.Reports;


namespace ParkInspect
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : MetroWindow
    {
        public ReportView()
        {
            InitializeComponent();
        }

        public void LoadReport(string path)
        {

            ((ReportViewModel)DataContext).LoadReport(path);

            ReportDataSource datasource = new ReportDataSource();

            Viewer.RefreshReport();

        }
    }
}

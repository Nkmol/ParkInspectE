using System.Windows;
using MahApps.Metro.Controls;
using ParkInspect.ViewModel;


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
            Viewer.RefreshReport();

        }
    }
}

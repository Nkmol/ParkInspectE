using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using ParkInspect.ViewModel;
using Syncfusion.Windows.Reports;
using Syncfusion.Windows.Reports.Viewer;


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

        public void LoadInspectionReport(string path, int id)
        {

            ((ReportViewModel)DataContext).LoadReport(path);

            List<ReportParameter> paramList = new List<ReportParameter>();
            ReportParameter param = new ReportParameter();
            param.Name = "InspectionID";
            param.Values = new List<string>() {"" + id + ""};
            paramList.Add(param);
            Viewer.SetParameters(paramList);

            Viewer.RefreshReport();

        }
    }
}

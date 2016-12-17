using GalaSoft.MvvmLight;
using Microsoft.SqlServer.ReportingServices2005;
using Syncfusion.Windows.Reports.Viewer;

namespace ParkInspect.ViewModel
{
    
    public class ReportViewModel : ViewModelBase
    {

        private ReportViewer _report;

        public ReportViewer Report
        {
            get { return _report; }
            set
            {
                _report = value; 
               
            }
        }

        
        public ReportViewModel()
        {
            
        }

        public void LoadReport(string path)
        {
            Report = new ReportViewer();
            ReportParameter param = new ReportParameter();
            
            Report.ReportPath = path;
           
            Report.RefreshReport();
            RaisePropertyChanged("Report");
        }
    }
}
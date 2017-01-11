using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.SqlServer.ReportingServices2005;
using ParkInspect.Repository;
using Syncfusion.Windows.Reports.Viewer;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using ParkInspect.Model.Factory.Builder;


namespace ParkInspect.ViewModel
{
    
    public class ReportViewModel : ViewModelBase
    {

        private string _nameFilter;

        public string NameFilter
        {
            get { return (_nameFilter ?? ""); }
            set
            {
                _nameFilter = value;
                UpdateReports();
            }
        }

        private ReportViewer _report;
        public ObservableCollection<Report> Names { get; set; }
        public IEnumerable<Report> Data { get; set; }

        public ReportViewer Report
        {
            get { return _report; }
            set
            {
                _report = value; 
               
            }
        }

        public RelayCommand CreateCommand { get; set; }

        
        public ReportViewModel(IRepository context)
        {

            CreateCommand = new RelayCommand(OpenDesignView);

            SetData();
            UpdateReports();

        }

        public void SetData()
        {
            List<Report> list = new List<Report>();
            foreach (string s in Directory.GetFiles("reports", "*.rdl"))
            {
                list.Add(new Report(s));
            }

            Data = list.AsEnumerable();
        }

        public void OpenDesignView()
        {

            ReportDesignView view = new ReportDesignView();
            view.Show();
            
        }

        public void OpenReportView(int index)
        {

            if (index < 0)
                return;

            Report r = Data.ElementAt(index);

            ReportView view = new ReportView();
            view.LoadReport(r.Path);
            view.Show();

        }

        public void UpdateReports()
        {

            Names = new ObservableCollection<Report>(Data.Where(x => x.Name.ToLower().Contains(NameFilter.ToLower())));
            RaisePropertyChanged("Names");
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

    public class Report
    {
        public string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public Report(string path)
        {
            this.Path = path;
            this.Name = path.Split('\\')[1];
        }
    }
}
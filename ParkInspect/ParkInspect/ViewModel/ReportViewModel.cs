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
using System.Windows;
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

        public Report SelectedReport { get; set; }

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
        public RelayCommand ImportCommand { get; set; }
        public RelayCommand OpenReportCommand { get; set; }

        private DialogManager _dialog { get; set; }
        
        public ReportViewModel(IRepository context, DialogManager dialog)
        {

            _dialog = dialog;

            if (!Directory.Exists("reports"))
                Directory.CreateDirectory("reports");

            CreateCommand = new RelayCommand(OpenDesignView);
            ImportCommand = new RelayCommand(ImportReport);
            OpenReportCommand = new RelayCommand(OpenReport);

            UpdateReports();

        }

        public void OpenDesignView()
        {

            ReportDesignView view = new ReportDesignView();
            view.Show();

            //Refresh on close
            view.Closed += (sender, args) =>
            {
                UpdateReports();
            };

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

        public void ImportReport()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "rdl files (*.rdl)|*.rdl|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ReportView view = new ReportView();

                int count = 1;
                string name = openFileDialog.SafeFileName;

                if (File.Exists(@"reports/" + name))
                {
                    while (File.Exists(@"reports/" + count + "_" + openFileDialog.SafeFileName))
                        count++;

                    name = count + "_" + openFileDialog.SafeFileName;
                    File.Copy(openFileDialog.FileName, @"reports/" + name);

                    _dialog.ShowMessage("Action", "Er bestaat al een bestand met de naam " + openFileDialog.SafeFileName + ". Het bestand is geïmporteerd als: " + name);

                }
                else
                {
                    _dialog.ShowMessage("Action", openFileDialog.SafeFileName + " is succesvol geïmporteerd!");
                }
                    

                UpdateReports();

            }
        }

        public void OpenReport()
        {

            if (SelectedReport == null)
            {
                return;
            }

            ReportView view = new ReportView();
            view.LoadReport(SelectedReport.Path);
            view.Show();
            

        }

        public void UpdateReports()
        {

            List<Report> list = new List<Report>();
            foreach (string s in Directory.GetFiles("reports", "*.rdl"))
            {
                list.Add(new Report(s));
            }

            Data = list.AsEnumerable();

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
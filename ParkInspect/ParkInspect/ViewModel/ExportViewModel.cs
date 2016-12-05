using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;
using DataService = ParkInspect.Model.DataService;

namespace ParkInspect.ViewModel
{
    public class ExportViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string _available { get; set; }
        private string _selected { get; set; }

        public string Available
        {
            get { return _available; }
            set { _available = value; }
        }

        public string Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        private IEnumerable _data;

        public IEnumerable Data
        {
            get { return _data; }
            set
            {
                _data = value;
                if(AvailableColumns == null && SelectedColumns == null)
                    UpdateColumns();
            }
        }

        public ObservableCollection<string> AvailableColumns { get; set; }
        public ObservableCollection<string> SelectedColumns { get; set; }


        public RelayCommand ExportCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand AddAllCommand { get; set; }
        public RelayCommand RemoveAllCommand { get; set; }

        public ParkinglotService Service;

        public ExportViewModel(IRepository context)
        {
            Service = new ParkinglotService(context);
            ExportCommand = new RelayCommand(Export);
            AddCommand = new RelayCommand(AddColumn);
            RemoveCommand = new RelayCommand(RemoveColumn);
            AddAllCommand = new RelayCommand(AddAll);
            RemoveAllCommand = new RelayCommand(RemoveAll);

        }

        private void UpdateColumns()
        {

            var type = Data.GetType().GenericTypeArguments[0];

            AvailableColumns = new ObservableCollection<string>();
            SelectedColumns = new ObservableCollection<string>();

            for (var index = 0; index < type.GetProperties().Length; index++)                
                AvailableColumns.Add(type.GetProperties()[index].Name);

            Notify();

        }

        private void AddAll()
        {

            foreach (var column in AvailableColumns)
            {
                SelectedColumns.Add(column);
            }

            AvailableColumns.Clear();
            UpdateData();
            Notify();
               
        }

        private void RemoveAll()
        {
            foreach (var column in SelectedColumns)
                AvailableColumns.Add(column);

            SelectedColumns.Clear();
            UpdateData();
            Notify();
        }

        private void AddColumn()
        {
            if (Available != null)
            {
                SelectedColumns.Add(Available);
                AvailableColumns.Remove(Available);
            }

            UpdateData();
            Notify();
        }

        private void RemoveColumn()
        {
            if (Selected != null)
            {
                AvailableColumns.Add(Selected);
                SelectedColumns.Remove(Selected);
            }

            UpdateData();
            Notify();
        }

        private void Export()
        {
            ExportFactory.ExportPdf(Data.Cast<dynamic>());
        }

        private void Notify()
        {
            RaisePropertyChanged("AvailableColumns");
            RaisePropertyChanged("SelectedColumns");
            RaisePropertyChanged("Data");
        }

        private void UpdateData()
        {

            DataTable table = new DataTable();
            table.Rows.Clear();
            table.Columns.Clear();

            var  data = Service.GetData(SelectedColumns.ToList());

            foreach (ExpandoObject expando in data)
            {

                var dict = (IDictionary<string, object>) expando;

                foreach (var key in dict.Keys)
                {
                    if (!table.Columns.Contains(key))
                        table.Columns.Add(key);

                }

                table.Rows.Add(dict.Values.ToArray());

            }

            Data = table.DefaultView;

        }
       
    }
}

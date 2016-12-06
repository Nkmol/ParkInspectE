using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ExportViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private DataService Service { get; set; }
        public Type ExportableType { get; set; }
        public IEnumerable ExpandoData { get; set; }
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

        public string AliasSelected { get; set; }

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

        private ObservableCollection<Alias> _aliasColumns;
        public ObservableCollection<Alias> AliasColumns
        {
            get { return _aliasColumns; }
            set
            {
                _aliasColumns = value;
                UpdateData();
            }
        }

        public RelayCommand ExportCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand AddAllCommand { get; set; }
        public RelayCommand RemoveAllCommand { get; set; }
        public RelayCommand AliasChangedCommand { get; set; }

        public ExportViewModel(IRepository context)
        {

            ExportCommand = new RelayCommand(Export);
            AddCommand = new RelayCommand(AddColumn);
            RemoveCommand = new RelayCommand(RemoveColumn);
            AddAllCommand = new RelayCommand(AddAll);
            RemoveAllCommand = new RelayCommand(RemoveAll);
            AliasChangedCommand = new RelayCommand(AliasChanged);

        }

        public void SetService(DataService service)
        {
            this.Service = service;
        }

        private void AliasChanged()
        {
            UpdateData();
            Notify();
        }

        private void UpdateColumns()
        { 

            var type = Data.GetType().GenericTypeArguments[0];
            ExportableType = type;

            AvailableColumns = new ObservableCollection<string>();
            SelectedColumns = new ObservableCollection<string>();
            AliasColumns = new ObservableCollection<Alias>();

            for (var index = 0; index < type.GetProperties().Length; index++)                
                AvailableColumns.Add(type.GetProperties()[index].Name);

            Notify();

        }

        private void AddAll()
        {

            foreach (var column in AvailableColumns)
            {
                AliasColumns.Add(new Alias(column));
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

            AliasColumns.Clear();
            SelectedColumns.Clear();
            UpdateData();
            Notify();
        }

        private void AddColumn()
        {
            if (Available != null)
            {
                AliasColumns.Add(new Alias(Available));
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

                foreach (var alias in AliasColumns)
                {
                    if(alias.Equals(Selected))
                        AliasColumns.Remove(alias);
                }

            }

            UpdateData();
            Notify();
        }

        private void Export()
        {
            ExportFactory.ExportPdf(ExpandoData.Cast<dynamic>(), ExportableType, SelectedColumns.ToArray(), GetAliases().ToArray());
        }

        private void Notify()
        {
            RaisePropertyChanged("AvailableColumns");
            RaisePropertyChanged("SelectedColumns");
            RaisePropertyChanged("AliasColumns");
            RaisePropertyChanged("Data");
        }

        private void UpdateData()
        {

            if (SelectedColumns.Count > 0)
            {

                var table = new DataTable();
                table.Rows.Clear();
                table.Columns.Clear();

                object[] param = new object[2];
                param[0] = SelectedColumns.ToList();
                param[1] = GetAliases();

                MethodInfo mi = typeof(DataService).GetMethod("GetData").MakeGenericMethod(ExportableType);
                ExpandoData = (IEnumerable) mi.Invoke(Service, param);

                foreach (ExpandoObject expando in ExpandoData)
                {

                    var dict = (IDictionary<string, object>)expando;

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

        private List<string> GetAliases()
        {
            List<string> aliases = new List<string>();

            foreach(var alias in AliasColumns)
                aliases.Add(alias.Value);
            
            return aliases;

        }

    }

    public class Alias
    {

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public Alias(string value)
        {
            this.Value = value;
        }

    }
}

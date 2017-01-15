
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
using ParkInspect.Model.Factory;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ExportViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public IEnumerable<dynamic> ExpandoData { get; set; }
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

        public DataView Viewable { get; set; }

        private IEnumerable<dynamic> _data;

        public IEnumerable<dynamic> Data
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

        /*
         * Used to update the data when the alias is changed
         */
        private void AliasChanged()
        {
            UpdateData();
            Notify();
        }

        /*
         * Sets the columns for the first time. Only called when Data is set for the first time.
         */
        private void UpdateColumns()
        { 

            var type = Data.GetType().GenericTypeArguments[0];

            AvailableColumns = new ObservableCollection<string>();
            SelectedColumns = new ObservableCollection<string>();
            AliasColumns = new ObservableCollection<Alias>();

            for (var index = 0; index < type.GetProperties().Length; index++)                
                AvailableColumns.Add(type.GetProperties()[index].Name);

            Notify();

        }

        /*
         * Adds all AvailableColumns to the SelectedColumns
         */
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

        /*
         * Removes all SelectedColumns and adds them to the AvailableColumns
         */
        private void RemoveAll()
        {
            foreach (var column in SelectedColumns)
                AvailableColumns.Add(column);

            AliasColumns.Clear();
            SelectedColumns.Clear();
            UpdateData();
            Notify();
        }

        /*
         * Moves one column from the AvailableColumns to the SelectedColumns
         */
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

        /*
         * Moves one column from the SelectedColumns to the AvailableColumns
         */
        private void RemoveColumn()
        {
            if (Selected != null)
            {
                AvailableColumns.Add(Selected);
                SelectedColumns.Remove(Selected);

                foreach (var alias in AliasColumns)
                {
                    if(alias.Value.Equals(Selected))
                        AliasColumns.Remove(alias);
                }

            }

            UpdateData();
            Notify();
        }

        /*
         * Exports all data using the ExportFactory
         */
        private void Export()
        {
            var type = Data.GetType().GenericTypeArguments[0];
            ExportFactory.ExportPdf(ExpandoData, type, SelectedColumns.ToArray(), GetAliases().ToArray());
        }

        /*
         * Notify all binded data. Updates the UI.
         */
        private void Notify()
        {
            RaisePropertyChanged("AvailableColumns");
            RaisePropertyChanged("SelectedColumns");
            RaisePropertyChanged("AliasColumns");
            RaisePropertyChanged("Viewable");
        }

        /*
         * Adds all data to the datagrid using ExpandoObjects and Reflection.
         */
        private void UpdateData()
        {

            if (SelectedColumns.Count > 0)
            {

                var table = new DataTable();
                table.Rows.Clear();
                table.Columns.Clear();

                var type = Data.GetType().GenericTypeArguments[0];
                ExpandoData = GetAbstractData();

                foreach (var expando in ExpandoData)
                {

                    var dict = (IDictionary<string, object>)expando;

                    foreach (var key in dict.Keys)
                    {
                        if (!table.Columns.Contains(key))
                            table.Columns.Add(key);

                    }

                    table.Rows.Add(dict.Values.ToArray());

                }

                Viewable = table.DefaultView;

            }

        }

        /*
         * Generate a list of aliases
         */
        private List<string> GetAliases()
        {
            List<string> aliases = new List<string>();

            foreach(var alias in AliasColumns)
                aliases.Add(alias.Value);
            
            return aliases;

        }

        private IEnumerable<ExpandoObject> GetAbstractData()
        {

            var columns = SelectedColumns.ToList();
            var alias = GetAliases();

            var query = Data.AsEnumerable().Select(x =>
            {
                var data = new ExpandoObject();
                for (var i = 0; i < columns.Count; i++)
                {
                    ((IDictionary<string, object>)data)
                     .Add((alias != null ? alias[i] : columns[i]), x.GetType().GetProperty(columns[i]).GetValue(x));
                }
                return data;
            });

            return query;
        }


    }

    /*
     * Used to fill the alias DataGrid.
     */
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

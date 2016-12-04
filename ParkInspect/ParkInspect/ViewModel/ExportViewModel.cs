using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model;

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
                UpdateColumns();
            }
        }

        public ObservableCollection<string> AvailableColumns { get; set; }
        public ObservableCollection<string> SelectedColumns { get; set; }


        public RelayCommand ExportCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        public ExportViewModel()
        {
            
            ExportCommand = new RelayCommand(Export);
            AddCommand = new RelayCommand(AddColumn);
            RemoveCommand = new RelayCommand(RemoveColumn);


        }

        private void UpdateColumns()
        {

            var type = Data.GetType().GenericTypeArguments[0];


            var size = type.GetProperties().Length;
            AvailableColumns = new ObservableCollection<string>();
            SelectedColumns = new ObservableCollection<string>();

            for (var index = 0; index < type.GetProperties().Length; index++)
            {
                
                AvailableColumns.Add(type.GetProperties()[index].Name);
            }

            RaisePropertyChanged("AvailableColumns");
            RaisePropertyChanged("SelectedColumns");

        }

        private void AddColumn()
        {
            if (Available != null)
            {
                SelectedColumns.Add(Available);
                AvailableColumns.Remove(Available);
            }

            RaisePropertyChanged("AvailableColumns");
            RaisePropertyChanged("SelectedColumns");
        }

        private void RemoveColumn()
        {
            if (Selected != null)
            {
                AvailableColumns.Add(Selected);
                SelectedColumns.Remove(Selected);
            }

            RaisePropertyChanged("AvailableColumns");
            RaisePropertyChanged("SelectedColumns");
        }

        private void Export()
        {
            ExportFactory.ExportPdf(Data.Cast<dynamic>());
        }
       
    }
}

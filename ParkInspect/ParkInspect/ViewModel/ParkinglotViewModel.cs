using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations.Model;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{

    public class ParkinglotViewModel : ViewModelBase
    {

        public string Message { get; set; }
        public ObservableCollection<Parkinglot> Parkinglots { get; set; }
        public ObservableCollection<Region> Regions { get; set; }
        public ObservableCollection<Inspection> Inspections { get; set; }
        protected ParkinglotService Service { get; set; }
        private Parkinglot _parkinglot;
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand NewCommand { get; set; }

        public Parkinglot Parkinglot
        {
            get
            {
                return _parkinglot;
            }
            set
            {
                _parkinglot = value;
                RaisePropertyChanged("Parkinglot");
                RaisePropertyChanged(("Name"));
                RaisePropertyChanged(("Zip"));
                RaisePropertyChanged(("Region"));
                RaisePropertyChanged(("Number"));
                RaisePropertyChanged(("Clarification"));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string Name
        {
            get { return _parkinglot?.name; }
            set
            {
                _parkinglot.name = value;
                RaisePropertyChanged(("Name"));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string Zip
        {
            get { return _parkinglot?.zipcode; }
            set
            {
                _parkinglot.zipcode = value;
                RaisePropertyChanged(("Zip"));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string Region
        {
            get { return _parkinglot?.region_name; }
            set
            {
                _parkinglot.region_name = value;
                RaisePropertyChanged(("Region"));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public int Number
        {
            get { return (_parkinglot == null ? 0 :_parkinglot.number.GetValueOrDefault()); }
            set
            {
                _parkinglot.number = value;
                RaisePropertyChanged(("Number"));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string Clarification
        {
            get { return _parkinglot?.clarification; }
            set
            {
                _parkinglot.clarification = value;
                RaisePropertyChanged(("Clarification"));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public ParkinglotViewModel(IRepository context)
        {

            SaveCommand = new RelayCommand(Save, () => CanSave());
            NewCommand = new RelayCommand(NewParkinglot);
            Service = new ParkinglotService(context);
            Parkinglots = new ObservableCollection<Parkinglot>(Service.GetAllParkinglots());
            Regions = new ObservableCollection<Region>(Service.GetAllRegions());
            NewParkinglot();
           
        }

        private void NewParkinglot()
        {

            Message = "Add a new Parkinglot";
            Parkinglot = new Parkinglot();
            RaisePropertyChanged("Parkinglot");
            Parkinglot.id = -1;

        }

        private void Save()
        {

            if (Parkinglot.id < 0)
            {
                Message = (Service.AddParkinglot(Parkinglot) ? "The parkinglot was added!" : "Something went wrong.");
            }
            else
            {
                Message = (Service.UpdateParkinglot(Parkinglot)
                    ? "The parkinglot was updated!"
                    : "Something went wrong.");
            }

            RaisePropertyChanged("Message");
            Parkinglots = new ObservableCollection<Parkinglot>(Service.GetAllParkinglots());
            RaisePropertyChanged("Parkinglots");
            ExportFactory.ExportPdf(
                Service.GetAllParkinglots(),
                new string[] {"name", "region_name"},
                new string[] {"Parkinglot", "Region"});
        }

        private bool CanSave()
        {

            if (Parkinglot?.name == null || Parkinglot.name.Equals(""))
                return false;

            if (Parkinglot?.region_name == null || Parkinglot.region_name.Equals(""))
                return false;

            if (Parkinglot?.clarification == null || Parkinglot.clarification.Equals(""))
                return false;

            if (Parkinglot?.zipcode == null || Parkinglot.zipcode.Equals(""))
                return false;

            return (Parkinglot?.number != null && Parkinglot.number > 0);

        }

    }
}
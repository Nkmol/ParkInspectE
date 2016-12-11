using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations.Model;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.View.UserControls;
using ParkInspect.View.UserControls.Popup;
using ParkInspect.ViewModel.Regio;

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
        private string _nameFilter;
        private string _zipFilter;
        private string _numberFilter;
        private string _regionFilter;
        private string _clarificationFilter;
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand NewCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }

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

        public string NameFilter
        {
            get { return _nameFilter; }
            set { _nameFilter = value;
                UpdateParkinglots();
            }
        }

        public string ZipFilter
        {
            get { return _zipFilter; }
            set
            {
                _zipFilter = value;
                UpdateParkinglots();
            }
        }

        public string NumberFilter
        {
            get { return _numberFilter; }
            set
            {
                _numberFilter = value;
                UpdateParkinglots();
            }
        }

        public string RegionFilter
        {
            get { return _regionFilter; }
            set
            {
                _regionFilter = value;
                UpdateParkinglots();
            }
        }

        public string ClarificationFilter
        {
            get { return _clarificationFilter; }
            set
            {
                _clarificationFilter = value;
                UpdateParkinglots();
            }
        }

        public PopupCoordinator PopupCoordinator { get; set; }

        public ParkinglotViewModel(IRepository context, PopupCoordinator popupCoordinator)
        {
            PopupCoordinator = popupCoordinator;
            SearchCommand = new RelayCommand(SearchRegionAsync);

            SaveCommand = new RelayCommand(Save, () => CanSave());
            NewCommand = new RelayCommand(NewParkinglot);
            Service = new ParkinglotService(context);
            UpdateParkinglots();
            Regions = new ObservableCollection<Region>(Service.GetAllRegions());
            NewParkinglot();
           
        }

        private async void SearchRegionAsync()
        {
            //TODO Don't let ViewModel know of UserControl/View?
            await PopupCoordinator.ShowSelectPopupAsync<RegionViewModel>(this, "Selecteer een regio.", new RegionOverview(), x => Region = x.Name);
        }

        private void UpdateParkinglots()
        {

            var filters = new Dictionary<string, string>()
            {
                {"name", NameFilter},
                {"region_name", RegionFilter },
                {"number", NumberFilter },
                {"zipcode", ZipFilter },
                {"clarification", ClarificationFilter }
            };

            Parkinglots = new ObservableCollection<Parkinglot>(Service.GetAllParkinglotsWhere(filters));
            RaisePropertyChanged("Parkinglots");
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
            UpdateParkinglots();
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
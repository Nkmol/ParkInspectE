using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ParkinglotVM
{
    public class ParkinglotViewModel : ViewModelBase
    {
        private readonly DialogManager _dialogManager;
        private readonly Parkinglot _parkinglot;

        #region ViewModel POCO properties
        public string Name
        {
            get { return _parkinglot.name; }
            set
            {
                _parkinglot.name = value;
                RaisePropertyChanged();
            }
        }

        public string Zipcode
        {
            get { return _parkinglot.zipcode; }
            set
            {
                _parkinglot.zipcode = value;
                RaisePropertyChanged();
            }
        }

        public string Region
        {
            get { return _parkinglot.region_name; }
            set
            {
                _parkinglot.region_name = value;
                RaisePropertyChanged();
            }
        }

        public string Number
        {
            get { return _parkinglot.number; }
            set
            {
                _parkinglot.number = value;
                RaisePropertyChanged();
            }
        }

        public string Clarification
        {
            get { return _parkinglot.clarification; }
            set
            {
                _parkinglot.clarification = value;
                RaisePropertyChanged();
            }
        }

        public string Streetname
        {
            get { return _parkinglot.streetname; }
            set
            {
                _parkinglot.streetname = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Property Form

        public string FormName { get; set; }
        public string FormZipcode { get; set; }
        public string FormRegion { get; set; }
        public string FormNumber { get; set; }
        public string FormClarification { get; set; }
        public string FormStreetname { get; set; }
        #endregion

        protected ParkinglotService Service { get; set; }

        public ObservableCollection<Region> Regions { get; set; }

        public RelayCommand<ParkinglotOverviewViewModel> SaveCommand { get; set; }
        public RelayCommand EditCommand { get; set; }

        public string Message { get; set; }

        public ParkinglotViewModel(IRepository context, Parkinglot parkinglot, DialogManager dialogManager)
        {
            _dialogManager = dialogManager;
            _parkinglot = parkinglot;
            Service = new ParkinglotService(context);

            Regions = new ObservableCollection<Region>(Service.GetAll<Region>()); // TODO: Load this once
            SaveCommand = new RelayCommand<ParkinglotOverviewViewModel>(Add, _ => _parkinglot.id <= 0);
            EditCommand = new RelayCommand(Edit, () => _parkinglot.id > 0);

            FillForm();
        }

        // TODO: Improve the way to make a 'property shadow object'. There is need for double property decleration at the moment.
        private void FillForm()
        {
            FormName = Name;
            FormZipcode = Zipcode;
            FormRegion = Region;
            FormNumber = Number;
            FormClarification = Clarification;
            FormStreetname = Streetname;
        }

        private void SaveForm()
        {
            Name = FormName;
            Zipcode = FormZipcode;
            Region = FormRegion;
            Number = FormNumber;
            Clarification = FormClarification;
            Streetname = FormStreetname;
        }

        public void Add(ParkinglotOverviewViewModel overview)
        {
            SaveForm();
            Message = Service.Add(_parkinglot) ? "De parkeerplaats is toegevoegd!" : "Er is iets misgegaan tijdens het toevoegen.";

            _dialogManager.ShowMessage("Parkeerplaats toevoegen", Message);

            overview.Parkinglots.Add(this);
        }

        public void Edit()
        {
            SaveForm();
            Message = Service.Update(_parkinglot) ? "De parkeerplaats is aangepast!" : "Er is iets misgegaan tijdens het aanpassen.";

            _dialogManager.ShowMessage("Parkeerplaats bewerken", Message);
        }
    }
}
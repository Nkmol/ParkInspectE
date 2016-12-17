using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ParkinglotVM
{
    public class ParkinglotViewModel : ViewModelBase
    {
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

        public int? Number
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
        #endregion

        protected ParkinglotService Service { get; set; }

        public ObservableCollection<RegionViewModel> Regions { get; set; }

        public RelayCommand<ParkinglotOverviewViewModel> SaveCommand { get; set; }
        public RelayCommand EditCommand { get; set; }

        public string Message { get; set; } // Transform to error popup

        public ParkinglotViewModel(IRepository context, Parkinglot parkinglot)
        {
            _parkinglot = parkinglot;
            Service = new ParkinglotService(context);

            Regions = new ObservableCollection<RegionViewModel>(Service.GetAll<Region>().Select(x => new RegionViewModel(x))); // TODO: Load this once
            SaveCommand = new RelayCommand<ParkinglotOverviewViewModel>(Save);
            EditCommand = new RelayCommand(Edit, () => _parkinglot.id > 0);
        }

        public void Save(ParkinglotOverviewViewModel overview)
        {
            Message = Service.InsertOrUpdate(_parkinglot) ? "De parkeerplaats is toegevoegd!" : "Er is iets misgegaan tijdens het toevoegen.";
            overview.Parkinglots.Add(this);
        }

        public void Edit()
        {
            Message = Service.InsertOrUpdate(_parkinglot) ? "De parkeerplaats is angepast!" : "Er is iets misgegaan tijdens het aanpassen.";
            //overview.Parkinglots.Add(this);
        }
    }
}

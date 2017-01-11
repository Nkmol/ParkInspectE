﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
   public class RegionViewModel : ViewModelBase
    {
        protected RegionService Service;
        private IEnumerable<Region> Data { get; set; }

        public ICommand SaveNewRegionCommand { get; set; }
        public ICommand DeleteRegionCommand { get; set; }

        private ObservableCollection<Region> _regions;

        public ObservableCollection<Region> Regions
        {
            get { return _regions; }
            set
            {
                _regions = value;
                base.RaisePropertyChanged();
            }
        }

        private Region _newRegion;

        public Region NewRegion
        {
            get
            {
                return _newRegion;
            }
            set
            {
                _newRegion = value;
                RaisePropertyChanged();
            }
        }
        private Region _selectedRegion;

        public Region SelectedRegion
        {
            get { return _selectedRegion; }

            set
            {
                _selectedRegion = value;
                base.RaisePropertyChanged();
            }
        }

        private string _regionFilter;

        public string RegionFilter
        {
            get
            {
                return _regionFilter;
            }
            set
            {
                _regionFilter = value;
                Filter();
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                base.RaisePropertyChanged();
            }
        }


        private DialogManager _dialog;

        public RegionViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new RegionService(context);
            Data = Service.GetAll<Region>();

            Regions = new ObservableCollection<Region>(Service.GetAll<Region>());
            NewRegion = new Region();
            
            SaveNewRegionCommand = new RelayCommand(SaveNewRegionMethod);
            DeleteRegionCommand = new RelayCommand(DeleteRegionMethod);

        }

        private void SaveNewRegionMethod()
        {

            if (NewRegion == null || NewRegion.name == null)
            {
                return;
            }

            Service.Add<Region>(NewRegion);
            Regions.Add(NewRegion);
            base.RaisePropertyChanged();

            NewRegion = new Region();
        }

        private void DeleteRegionMethod()
        {
            if (SelectedRegion == null)
            {
                Message = (Service.Delete<Region>(SelectedRegion) ? "Something went wrong." : "Selecteer een regio!");
                _dialog.ShowMessage("Action", Message);
                return;
            }
            Service.Delete<Region>(SelectedRegion);
            Regions.Remove(SelectedRegion);
            base.RaisePropertyChanged();
        }

        private void Filter()
        {
            var builder = new FilterBuilder();
            builder.Add("name", RegionFilter);

            var filters = builder.Get();
            var result = Data.Where(a => a.Like(filters));

            Regions = new ObservableCollection<Region>(result);
            RaisePropertyChanged("Regions");
        }
    }
}

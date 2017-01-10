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

namespace ParkInspect.ViewModel
{
   public class RegionViewModel : ViewModelBase
    {
        protected RegionService Service;
        public ICommand SaveNewRegionCommand { get; set; }

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
                base.RaisePropertyChanged();
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


        private DialogManager _dialog;

        public RegionViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new RegionService(context);

            SaveNewRegionCommand = new RelayCommand(SaveNewRegionMethod);

            Regions = new ObservableCollection<Region>(Service.GetAllRegions());
        }

        private void SaveNewRegionMethod()
        {
            if (SelectedRegion == null)
            {
                return;
            }

            Service.InsertRegion(NewRegion);
            Regions.Add(NewRegion);
            base.RaisePropertyChanged();
        }
    }
}

using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.Region
{
    public class RegionOverviewViewModel : ViewModelBase
    {
        private readonly ClientService _service;

        private ObservableCollection<RegionViewModel> Data { get; }

        private RegionViewModel _selectedRegion;

        public ObservableCollection<RegionViewModel> Regions { get; set; }

        public RegionViewModel SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                Set(ref _selectedRegion, value);
                SelectedRegion?.Reset();
            }
        }

        #region Filter Properties
        private string _nameFilter;

        public string NameFilter
        {
            get { return _nameFilter; }
            set
            {
                _nameFilter = value;
                UpdateFilter();
            }
        }
        #endregion

        public RelayCommand NewCommand { get; set; }

        private readonly IRepository _context;
        private readonly DialogManager _dialog;

        public RegionOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _context = context;
            _dialog = dialog;
            _service = new ClientService(context);

            Data = new ObservableCollection<RegionViewModel>(_service.GetAll<ParkInspect.Region>().Select(x => new RegionViewModel(context, x, dialog)));
            Regions = Data;

            NewCommand = new RelayCommand(NewRegion);
            NewRegion();
        }

        public void NewRegion()
        {
            SelectedRegion = new RegionViewModel(_context, new ParkInspect.Region(), _dialog);
            RaisePropertyChanged("Regions");
        }

        private void UpdateFilter()
        {
            var builder = new FilterBuilder();
            builder.Add("Name", NameFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Regions = new ObservableCollection<RegionViewModel>(result);
            RaisePropertyChanged("Regions");
        }
    }
}

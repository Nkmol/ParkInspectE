using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ParkinglotVM
{
    public class ParkinglotOverviewViewModel : ViewModelBase
    {
        private readonly IRepository _context;
        private readonly DialogManager _dialog;
        private ParkinglotViewModel _selectedParkinglot;

        private ObservableCollection<ParkinglotViewModel> Data { get; set; }
        public ObservableCollection<ParkinglotViewModel> Parkinglots { get; set; }

        #region filter Properties

        private string _nameFilter;
        private string _zipFilter;
        private string _numberFilter;
        private string _regionFilter;
        private string _clarificationFilter;
        private string _streetnameFilter;

        public string NameFilter
        {
            get { return _nameFilter; }
            set
            {
                _nameFilter = value;
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

        public string StreetnameFilter
        {
            get { return _streetnameFilter; }
            set
            {
                _streetnameFilter = value;
                UpdateParkinglots();
            }
        }

        #endregion

        public ParkinglotOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            _context = context;
            NewCommand = new RelayCommand(NewParkinglot);

            Service = new ParkinglotService(context);
            Data = new ObservableCollection<ParkinglotViewModel>(Service.GetAll<Parkinglot>().Select(x => new ParkinglotViewModel(context, x, dialog)));
            Parkinglots = new ObservableCollection<ParkinglotViewModel>(Data);
            NewParkinglot();
        }

        public ParkinglotViewModel SelectedParkinglot
            // Backing value needed, because needs to "raise" change for child bind properties for update
        {
            get { return _selectedParkinglot; }
            set { Set(ref _selectedParkinglot, value); }
        }

        protected ParkinglotService Service { get; set; }

        public RelayCommand NewCommand { get; set; }

        public RelayCommand ExportCommand { get; set; }

        public void UpdateParkinglots()
        {
            var builder = new FilterBuilder();
            builder.Add("Name", NameFilter);
            builder.Add("Region", RegionFilter);
            builder.Add("Number", NumberFilter);
            builder.Add("Zipcode", ZipFilter);
            builder.Add("Clarification", ClarificationFilter);
            builder.Add("Streetname", StreetnameFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Parkinglots = new ObservableCollection<ParkinglotViewModel>(result);
            RaisePropertyChanged("Parkinglots");
        }

        public void NewParkinglot()
        {
            SelectedParkinglot = new ParkinglotViewModel(_context, new Parkinglot(), _dialog);
        }

        public void ParkinglotsChanged()
        {
            Data = new ObservableCollection<ParkinglotViewModel>(Service.GetAll<Parkinglot>().Select(x => new ParkinglotViewModel(_context, x, _dialog)));
            Parkinglots = Data;
            RaisePropertyChanged("Parkinglots");
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
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
        private readonly DialogManager _dialog;
        private IEnumerable<ParkinglotViewModel> Data { get; set; }
        private ParkinglotViewModel _selectedParkinglot;
        public ObservableCollection<ParkinglotViewModel> Parkinglots { get; set; }

        public ParkinglotViewModel SelectedParkinglot // Backing value needed, because needs to "raise" change for child bind properties for update
        {
            get { return _selectedParkinglot; }
            set { Set(ref _selectedParkinglot, value); }
        }

        protected ParkinglotService Service { get; set; }

        public RelayCommand NewCommand { get; set; }

        public RelayCommand ExportCommand { get; set; }

        #region filter Properties

        private string _nameFilter;
        private string _zipFilter;
        private string _numberFilter;
        private string _regionFilter;
        private string _clarificationFilter;

        public string NameFilter
        {
            get { return _nameFilter; }
            set {
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
        #endregion

        public ParkinglotOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;

            NewCommand = new RelayCommand(() => NewParkinglot(context));
            ExportCommand = new RelayCommand(Export);

            Service = new ParkinglotService(context);
            Data = Service.GetAll<Parkinglot>().Select(x => new ParkinglotViewModel(context, x));
            Parkinglots = new ObservableCollection<ParkinglotViewModel>(Data);
            NewParkinglot(context);
        }

        public void UpdateParkinglots()
        {
            var builder = new FilterBuilder();
            builder.Add("Name", NameFilter);
            builder.Add("Region", RegionFilter);
            builder.Add("Number", NumberFilter);
            builder.Add("Zipcode", ZipFilter);
            builder.Add("Clarification", ClarificationFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Parkinglots = new ObservableCollection<ParkinglotViewModel>(result);
            RaisePropertyChanged("Parkinglots");
        }

        private void NewParkinglot(IRepository context)
        {
            SelectedParkinglot = new ParkinglotViewModel(context, new Parkinglot());
        }

        private void Export()
        {

            ExportView export = new ExportView();
            export.Show();

            FilterBuilder builder = new FilterBuilder();
            builder.Add("Name", NameFilter);
            builder.Add("Region", RegionFilter);
            builder.Add("Number", NumberFilter);
            builder.Add("Zipcode", ZipFilter);
            builder.Add("Clarification", ClarificationFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            export.FillGrid(result);
        }
    }
}
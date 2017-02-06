using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ClientVM
{
    public class ClientOverviewViewModel : ViewModelBase
    {
        private readonly IRepository _context;
        private readonly DialogManager _dialog;
        private readonly ClientService _service;

        private ClientViewModel _selectedClient;

        public ClientOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _context = context;
            _dialog = dialog;
            _service = new ClientService(context);

            Data =
                new ObservableCollection<ClientViewModel>(
                    _service.GetAll<Client>().Select(x => new ClientViewModel(context, x, dialog)));
            Clients = Data;

            NewCommand = new RelayCommand(NewClient);
            NewClient();
        }

        private ObservableCollection<ClientViewModel> Data { get; set; }

        public ObservableCollection<ClientViewModel> Clients { get; set; }

        public ClientViewModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                Set(ref _selectedClient, value);
                SelectedClient?.Reset();
            }
        }

        public RelayCommand NewCommand { get; set; }

        public void NewClient()
        {
            SelectedClient = new ClientViewModel(_context, new Client(), _dialog);
        }

        public void ClientsChanged()
        {
            Data = new ObservableCollection<ClientViewModel>(_service.GetAll<Client>().Select(x => new ClientViewModel(_context, x, _dialog)));
            Clients = Data;
            RaisePropertyChanged("Clients");
        }

        private void UpdateFilter()
        {
            var builder = new FilterBuilder();
            builder.Add("Name", NameFilter);
            builder.Add("Phonenumber", PhoneFilter);
            builder.Add("Email", EmailFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Clients = new ObservableCollection<ClientViewModel>(result);
            RaisePropertyChanged("Clients");
        }

        #region Filter Properties

        private string _emailFilter;
        private string _nameFilter;
        private string _phoneFilter;

        public string NameFilter
        {
            get { return _nameFilter; }
            set
            {
                _nameFilter = value;
                UpdateFilter();
            }
        }

        public string PhoneFilter
        {
            get { return _phoneFilter; }
            set
            {
                _phoneFilter = value;
                UpdateFilter();
            }
        }

        public string EmailFilter
        {
            get { return _emailFilter; }
            set
            {
                _emailFilter = value;
                UpdateFilter();
            }
        }

        #endregion
    }
}
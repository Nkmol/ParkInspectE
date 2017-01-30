using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel.AssignmentVM;

namespace ParkInspect.ViewModel.ClientVM
{
    public class ClientOverviewViewModel : ViewModelBase
    {
        private readonly ClientService _service;

        private ObservableCollection<ClientViewModel> Data { get; set; }

        private ClientViewModel _selectedClient;

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

        public RelayCommand NewCommand { get; set; }

        private readonly IRepository _context;
        private readonly DialogManager _dialog;

        public ClientOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _context = context;
            _dialog = dialog;
            _service = new ClientService(context);

            Data = new ObservableCollection<ClientViewModel>(_service.GetAll<Client>().Select(x => new ClientViewModel(context, x, dialog)));
            Clients = Data;

            NewCommand = new RelayCommand(NewClient);
            NewClient();
        }

        public void NewClient()
        {
            SelectedClient = new ClientViewModel(_context, new Client(), _dialog);
            RaisePropertyChanged();
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
    }
}

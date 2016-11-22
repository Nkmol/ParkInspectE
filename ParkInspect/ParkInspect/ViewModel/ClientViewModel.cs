using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        private Client _client;

        private Client _selectedClient;

        protected ClientService Service;

        public ClientViewModel(IRepository context)
        {
            Service = new ClientService(context);
            Clients = new ObservableCollection<Client>(Service.GetAllClients());
            Client = new Client();

            CompleteClientCommand = new RelayCommand(CompleteClient);
        }

        public ObservableCollection<Client> Clients { get; set; }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { Set(ref _selectedClient, value); }
        }

        public ICommand CompleteClientCommand { get; set; }

        public Client Client
        {
            get { return _client; }
            set { Set(ref _client, value); }
        }

        private void CompleteClient()
        {
            Service.AddClient(Client);
        }
    }
}
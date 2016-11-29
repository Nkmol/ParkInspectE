using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ParkInspect.Repository;
using ParkInspect.Services;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace ParkInspect.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        public ObservableCollection<Client> Clients { get; set; }

        protected ClientService Service;

        private Client _selectedClient;

        public Client SelectedClient
        {
            get
            {
                return _selectedClient;
            }
            set
            {
                Set(ref _selectedClient, value);
            }
        }

        public ICommand CompleteClientCommand { get; set; }

        private Client _client;

        public Client Client
        {
            get
            {
                return _client;
            }
            set
            {
                Set(ref _client, value);
            }
        }

        public ClientViewModel(IRepository context)
        {
            Service = new ClientService(context);
            Clients = new ObservableCollection<Client>(Service.GetAllClients());
            Client = new Client();

            CompleteClientCommand = new RelayCommand(CompleteClient);
        }

        private void CompleteClient()
        {
            Service.addClient(Client);
        }
    }
}

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
        private Client _selectedClient;

        protected ClientService Service;

        public ClientViewModel(IRepository context)
        {
            Service = new ClientService(context);
            Clients = new ObservableCollection<Client>(Service.GetAllClients());
            SelectedClient = new Client();
            Assignments = new ObservableCollection<Asignment>(SelectedClient.Asignment);
            Contactpersons = new ObservableCollection<Contactperson>(SelectedClient.Contactperson);

            CompleteClientCommand = new RelayCommand(CompleteClient);
            ResetButtonCommand = new RelayCommand(Reset);
            UpdateButtonCommand = new RelayCommand(UpdateClient);
        }

        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<Asignment> Assignments { get; set; }

        public ObservableCollection<Contactperson> Contactpersons { get; set; }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                Set(ref _selectedClient, value);
                RaisePropertyChanged();
            }
        }

        public ICommand CompleteClientCommand { get; set; }
        public ICommand ResetButtonCommand { get; set; }
        public ICommand UpdateButtonCommand { get; set; }

        private void CompleteClient()
        {
            if (string.IsNullOrEmpty(SelectedClient.name) || string.IsNullOrEmpty(SelectedClient.phonenumber) || string.IsNullOrEmpty(SelectedClient.email))
                return;
            Service.AddClient(SelectedClient);
            Clients.Add(SelectedClient);
        }

        private void Reset()
        {
            SelectedClient = new Client();
        }

        private void UpdateClient()
        {
            if (string.IsNullOrEmpty(SelectedClient.name) || string.IsNullOrEmpty(SelectedClient.phonenumber) || string.IsNullOrEmpty(SelectedClient.email))
                return;
            Service.UpdateClient(SelectedClient);
        }
    }
}
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
            Assignments = new ObservableCollection<Asignment>(SelectedClient.Assignments);
            Contactpersons = new ObservableCollection<Contactperson>(SelectedClient.Contactpersons);

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
            if (SelectedClient.name == null || SelectedClient.phonenumber == null || SelectedClient.email == null)
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
            Service.UpdateClient(SelectedClient);
        }
    }
}
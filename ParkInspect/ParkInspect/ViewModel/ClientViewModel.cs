using System.Collections.ObjectModel;
using System.Windows;
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
            CompleteClientCommand = new RelayCommand(CompleteClient, CanCreate);
            ResetButtonCommand = new RelayCommand(Reset);
            UpdateButtonCommand = new RelayCommand(UpdateClient, CanUpdate);
            SelectedClient = new Client();
            Assignments = new ObservableCollection<Asignment>(SelectedClient.Asignments);
            Contactpersons = new ObservableCollection<Contactperson>(SelectedClient.Contactpersons);
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
                CompleteClientCommand.RaiseCanExecuteChanged();
                UpdateButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand CompleteClientCommand { get; set; }
        public ICommand ResetButtonCommand { get; set; }
        public RelayCommand UpdateButtonCommand { get; set; }

        private bool CanCreate()
        {
            return (SelectedClient != null) && (SelectedClient.id == 0);
        }

        private bool CanUpdate()
        {
            return (SelectedClient != null) && (SelectedClient.id != 0);
        }

        private void CompleteClient()
        {
            Service.AddClient(SelectedClient);
            Clients.Add(SelectedClient);
            CompleteClientCommand.RaiseCanExecuteChanged();
            UpdateButtonCommand.RaiseCanExecuteChanged();

            MessageBox.Show("Klant toegevoegd");
        }

        private void Reset()
        {
            SelectedClient = new Client();
        }

        private void UpdateClient()
        {
            Service.UpdateClient(SelectedClient);
            CompleteClientCommand.RaiseCanExecuteChanged();
            UpdateButtonCommand.RaiseCanExecuteChanged();

            MessageBox.Show("Klant geupdate");
        }
    }
}
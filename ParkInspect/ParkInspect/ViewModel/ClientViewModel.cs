using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.Model.Factory;

namespace ParkInspect.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        private string _emailFilter;

        private string _nameFilter;
        private string _phoneFilter;
        private Client _selectedClient;

        private readonly IEnumerable<Client> Data;

        protected ClientService Service;

        public ClientViewModel(IRepository context)
        {
            Service = new ClientService(context);
            CompleteClientCommand = new RelayCommand(CompleteClient, CanCreate);
            ResetButtonCommand = new RelayCommand(Reset);
            UpdateButtonCommand = new RelayCommand(UpdateClient, CanUpdate);
            SelectedClient = new Client();
            Assignments = new ObservableCollection<Asignment>(SelectedClient.Asignments);
            Contactpersons = new ObservableCollection<Contactperson>(SelectedClient.Contactpersons);
            Data = Service.GetAllClients();
            UpdateClients();
        }

        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<Asignment> Assignments { get; set; }

        public ObservableCollection<Contactperson> Contactpersons { get; set; }

        public string NameFilter
        {
            get { return _nameFilter; }
            set
            {
                _nameFilter = value;
                UpdateClients();
            }
        }

        public string PhoneFilter
        {
            get { return _phoneFilter; }
            set
            {
                _phoneFilter = value;
                UpdateClients();
            }
        }

        public string EmailFilter
        {
            get { return _emailFilter; }
            set
            {
                _emailFilter = value;
                UpdateClients();
            }
        }


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
            UpdateClients();

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
            UpdateClients();

            MessageBox.Show("Klant geupdate");
        }

        private void UpdateClients()
        {
            var builder = new FilterBuilder();
            builder.Add("name", NameFilter);
            builder.Add("phonenumber", PhoneFilter);
            builder.Add("email", EmailFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Clients = new ObservableCollection<Client>(result);
            RaisePropertyChanged("Clients");
        }
    }
}
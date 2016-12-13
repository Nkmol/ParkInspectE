using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        private DialogManager _dialog;
        private readonly IEnumerable<Client> Data;
        private string _emailFilter;

        private string _nameFilter;
        private string _phoneFilter;
        private Client _selectedClient;
        protected ClientService Service;

        public ClientViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new ClientService(context);
            ResetButtonCommand = new RelayCommand(Reset);
            SaveCommand = new RelayCommand(SaveClient);
            Data = Service.GetAll<Client>();
            UpdateClients();
            Reset();
            Assignments = new ObservableCollection<Asignment>(SelectedClient.Asignments);
            Contactpersons = new ObservableCollection<Contactperson>(SelectedClient.Contactpersons);
        }

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Asignment> Assignments { get; set; }
        public ObservableCollection<Contactperson> Contactpersons { get; set; }

        public string Name
        {
            get { return _selectedClient.name; }
            set
            {
                _selectedClient.name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Phonenumber
        {
            get { return _selectedClient.phonenumber; }
            set
            {
                _selectedClient.phonenumber = value;
                RaisePropertyChanged("Phonenumber");
            }
        }

        public string Email
        {
            get { return _selectedClient.email; }
            set
            {
                _selectedClient.email = value;
                RaisePropertyChanged("Email");
            }
        }

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
                RaisePropertyChanged("name");
                RaisePropertyChanged("phonenumber");
                RaisePropertyChanged("email");
            }
        }

        public ICommand ResetButtonCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        private void Reset()
        {
            SelectedClient = new Client();
            RaisePropertyChanged("SelectedClient");
        }

        private void SaveClient()
        {
            if (SelectedClient.id == 0)
            {
                Service.Add(SelectedClient);
                _dialog.ShowMessage("Actie", "Klant toegevoegd");
            }
            else
            {
                Service.Update(SelectedClient);
                _dialog.ShowMessage("Actie", "Klant geupdate");
            }

            UpdateClients();
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
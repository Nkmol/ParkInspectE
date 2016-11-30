using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ContactpersonViewModel : ViewModelBase
    {
        private Client _selectedClient;

        private Contactperson _selectedContactperson;

        protected ContactpersonService Service;

        public ContactpersonViewModel(IRepository context)
        {
            Service = new ContactpersonService(context);
            Contactpersons = new ObservableCollection<Contactperson>(Service.GetAllContactpersons());
            Clients = new ObservableCollection<Client>(Service.GetAllClients());
            CompleteContactpersonCommand = new RelayCommand(CompleteContactperson);
            ResetButtonCommand = new RelayCommand(Reset);
            UpdateButtonCommand = new RelayCommand(UpdateContactperson);
            DeleteContactpersonCommand = new RelayCommand(DeleteContactperson);
            SelectedClient = new Client();
            SelectedContactperson = new Contactperson();
        }

        public ObservableCollection<Contactperson> Contactpersons { get; set; }
        public ObservableCollection<Client> Clients { get; set; }

        public Contactperson SelectedContactperson
        {
            get { return _selectedContactperson; }
            set
            {
                Set(ref _selectedContactperson, value);

                if (_selectedContactperson?.Client != null)
                    SelectedClient = _selectedContactperson.Client;

                RaisePropertyChanged();
            }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                Set(ref _selectedClient, value);
                RaisePropertyChanged();
            }
        }

        public ICommand CompleteContactpersonCommand { get; set; }
        public ICommand ResetButtonCommand { get; set; }
        public ICommand UpdateButtonCommand { get; set; }
        public ICommand DeleteContactpersonCommand { get; set; }

        private void CompleteContactperson()
        {
            if (string.IsNullOrEmpty(SelectedContactperson.firstname) || string.IsNullOrEmpty(SelectedContactperson.lastname) ||
                (SelectedClient == null))
                return;

            SelectedContactperson.client_id = SelectedClient.id;
            Service.AddContactperson(SelectedContactperson);
            Contactpersons.Add(SelectedContactperson);
        }

        private void Reset()
        {
            SelectedContactperson = new Contactperson();
        }

        private void UpdateContactperson()
        {
            if (string.IsNullOrEmpty(SelectedContactperson.firstname) || string.IsNullOrEmpty(SelectedContactperson.lastname) ||
                (SelectedClient == null))
                return;

            SelectedContactperson.client_id = SelectedClient.id;
            Service.UpdateContactperson(SelectedContactperson);
        }

        private void DeleteContactperson()
        {
            Service.DeleteContactpeson(SelectedContactperson);
            Contactpersons.Remove(SelectedContactperson);
            Reset();
        }
    }
}
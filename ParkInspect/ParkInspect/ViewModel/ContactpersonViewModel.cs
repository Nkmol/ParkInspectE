using System.Collections.ObjectModel;
using System.Windows;
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
            CompleteContactpersonCommand = new RelayCommand(CompleteContactperson, CanCreate);
            ResetButtonCommand = new RelayCommand(Reset);
            UpdateButtonCommand = new RelayCommand(UpdateContactperson, CanUpdate);
            DeleteContactpersonCommand = new RelayCommand(DeleteContactperson, CanDelete);
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
                UpdateButtonCommand.RaiseCanExecuteChanged();
                CompleteContactpersonCommand.RaiseCanExecuteChanged();
                DeleteContactpersonCommand.RaiseCanExecuteChanged();
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

        public RelayCommand CompleteContactpersonCommand { get; set; }
        public ICommand ResetButtonCommand { get; set; }
        public RelayCommand UpdateButtonCommand { get; set; }
        public RelayCommand DeleteContactpersonCommand { get; set; }

        private void Reset()
        {
            SelectedContactperson = new Contactperson();
        }

        private bool CanUpdate()
        {
            return (SelectedContactperson != null) && (SelectedContactperson.id != 0);
        }

        private bool CanCreate()
        {
            return (SelectedContactperson != null) && (SelectedContactperson.id == 0);
        }

        private bool CanDelete()
        {
            return (SelectedContactperson != null) && (SelectedContactperson.id != 0);
        }

        private void CompleteContactperson()
        {
            SelectedContactperson.client_id = SelectedClient.id;
            Service.AddContactperson(SelectedContactperson);
            Contactpersons.Add(SelectedContactperson);
            CompleteContactpersonCommand.RaiseCanExecuteChanged();
            UpdateButtonCommand.RaiseCanExecuteChanged();
            DeleteContactpersonCommand.RaiseCanExecuteChanged();

            MessageBox.Show("Contactpersoon toegevoegd");
        }

        private void UpdateContactperson()
        {
            SelectedContactperson.client_id = SelectedClient.id;
            Service.UpdateContactperson(SelectedContactperson);
            CompleteContactpersonCommand.RaiseCanExecuteChanged();
            UpdateButtonCommand.RaiseCanExecuteChanged();
            DeleteContactpersonCommand.RaiseCanExecuteChanged();

            MessageBox.Show("Contactpersoon geupdate");
        }

        private void DeleteContactperson()
        {
            Service.DeleteContactpeson(SelectedContactperson);
            Contactpersons.Remove(SelectedContactperson);
            Reset();
            CompleteContactpersonCommand.RaiseCanExecuteChanged();
            UpdateButtonCommand.RaiseCanExecuteChanged();
            DeleteContactpersonCommand.RaiseCanExecuteChanged();

            MessageBox.Show("Contactpersoon verwijderd");
        }
    }
}
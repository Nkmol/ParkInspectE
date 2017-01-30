using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ContactpersonVM
{
    public class ContactpersonViewModel : ViewModelBase
    {
        private string _clientFilter;

        private readonly DialogManager _dialog;

        private string _firstnameFilter;
        private string _lastnameFilter;
        private Client _selectedClient;

        private Contactperson _selectedContactperson;

        protected ContactpersonService Service;
        public ClientViewModel ClientviewModel { get; set; }

        public ContactpersonViewModel(IRepository context, DialogManager dialog, ClientViewModel clientvm)
        {
            _dialog = dialog;
            Service = new ContactpersonService(context);
            Data = Service.GetAll<Contactperson>();
            ResetButtonCommand = new RelayCommand(Reset);
            SaveCommand = new RelayCommand(SaveContactperson);
            DeleteContactpersonCommand = new RelayCommand(DeleteContactperson, CanDelete);
            SelectedClient = new Client();
            UpdateContactpersons();
            ClientviewModel = clientvm;
            Reset();
        }

        private IEnumerable<Contactperson> Data { get; set; }

        public ObservableCollection<Contactperson> Contactpersons { get; set; }

        public Contactperson SelectedContactperson
        {
            get { return _selectedContactperson; }
            set
            {
                _selectedContactperson = value ?? new Contactperson();

                if (_selectedContactperson?.Client != null)
                    SelectedClient = _selectedContactperson.Client;

                RaisePropertyChanged("firstname");
                RaisePropertyChanged("lastname");
                RaisePropertyChanged("client_id");
                DeleteContactpersonCommand.RaiseCanExecuteChanged();
            }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                Set(ref _selectedClient, value);
                RaisePropertyChanged("SelectedClient");
            }
        }

        public string Firstname
        {
            get { return _selectedContactperson.firstname; }
            set
            {
                _selectedContactperson.firstname = value;
                RaisePropertyChanged("Firstname");
            }
        }

        public string Lastname
        {
            get { return _selectedContactperson.lastname; }
            set
            {
                _selectedContactperson.lastname = value;
                RaisePropertyChanged("Lastname");
            }
        }

        

        public ICommand ResetButtonCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand DeleteContactpersonCommand { get; set; }

        

        private bool CanDelete()
        {
            return (SelectedContactperson != null) && (SelectedContactperson.id != -1);
        }

        private void SaveContactperson()
        {
            if (SelectedContactperson.id < 0)
            {
                SelectedContactperson.client_id = SelectedClient.id;
                Service.Add(SelectedContactperson);
                _dialog.ShowMessage("Contactpersoon toevoegen", "Contactpersoon toegevoegd");
            }
            else
            {
                SelectedContactperson.client_id = SelectedClient.id;
                Service.Update(SelectedContactperson);
                _dialog.ShowMessage("Contactpersoon bijwerken", "Contactpersoon bijgewerkt");
            }

            UpdateContactpersons();
            DeleteContactpersonCommand.RaiseCanExecuteChanged();
        }

        private void DeleteContactperson()
        {
            Service.Delete(SelectedContactperson);
            UpdateContactpersons();
            DeleteContactpersonCommand.RaiseCanExecuteChanged();
            _dialog.ShowMessage("Contactpersoon verwijderen", "Contactpersoon verwijderd");
        }
    }
}
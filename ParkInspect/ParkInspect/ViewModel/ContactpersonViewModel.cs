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
        private string _selectedClient;

        private Contactperson _selectedContactperson;

        protected ContactpersonService Service;

        public ContactpersonViewModel(IRepository context)
        {
            Service = new ContactpersonService(context);
            Contactpersons = new ObservableCollection<Contactperson>(Service.GetAllContactpersons());
            Clients = new ObservableCollection<string>(Service.GetAllClients());
            CompleteContactpersonCommand = new RelayCommand(CompleteContactperson);
            ResetButtonCommand = new RelayCommand(Reset);
            UpdateButtonCommand = new RelayCommand(UpdateContactperson);
            SelectedContactperson = new Contactperson();
        }

        public ObservableCollection<Contactperson> Contactpersons { get; set; }
        public ObservableCollection<string> Clients { get; set; }

        public Contactperson SelectedContactperson
        {
            get { return _selectedContactperson; }
            set
            {
                Set(ref _selectedContactperson, value);
                RaisePropertyChanged();
            }
        }

        public string SelectedClient
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

        private void CompleteContactperson()
        {
            if ((SelectedContactperson.firstname == null) || (SelectedContactperson.lastname == null))
                return;

            SelectedContactperson.client_id = Service.GetClientIdFromName(SelectedClient);
            Service.AddContactperson(SelectedContactperson);
            Contactpersons.Add(SelectedContactperson);
        }

        private void Reset()
        {
            SelectedContactperson = new Contactperson();
        }

        private void UpdateContactperson()
        {
            Service.UpdateContactperson(SelectedContactperson);
        }
    }
}
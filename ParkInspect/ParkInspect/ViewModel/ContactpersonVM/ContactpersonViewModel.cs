using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ContactpersonVM
{
    public class ContactpersonViewModel : ViewModelBase
    {
        private readonly DialogManager _dialog;

        protected ContactpersonService Service;

        public ContactpersonViewModel(IRepository context, Contactperson data, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new ContactpersonService(context);
            Data = data;
            SaveCommand = new RelayCommand<ContactpersonOverviewViewModel>(Save);
            DeleteCommand = new RelayCommand<ContactpersonOverviewViewModel>(Delete);
            Reset();
        }

        public Contactperson Data { get; }

        public RelayCommand<ContactpersonOverviewViewModel> SaveCommand { get; set; }
        public RelayCommand<ContactpersonOverviewViewModel> DeleteCommand { get; set; }

        public string Message { get; set; }
        public bool CanDelete => Data.id > 0;

        public void Reset()
        {
            FillForm();
        }        

        private void SaveForm()
        {
            Firstname = FormFirstname;
            Lastname = FormLastname;
            Client = FormClient;
        }

        private void FillForm()
        {
            FormFirstname = Firstname;
            FormLastname = Lastname;
            FormClient = Client;
        }

        private void Save(ContactpersonOverviewViewModel overview)
        {
            if (Data.id <= 0)
                Add(overview);
            else
                Edit();
            
            overview.NewContactperson();
        }

        private void Add(ContactpersonOverviewViewModel overview)
        {
            SaveForm();

            Data.client_id = Client.id;

            var rs = Service.Add(Data);

            Message = rs
                ? "De contactpersoon is toegevoegd!"
                : "Er is iets misgegaan tijdens het toevoegen.";
            _dialog.ShowMessage("Contactpersoon toevoegen", Message);

            if(rs)
                overview.Contactpersons.Add(this);
        }

        private void Edit()
        {
            SaveForm();

            Data.client_id = Client.id;
            Message = Service.Update(Data)
                ? "De contactpersoon is aangepast!"
                : "Er is iets misgegaan tijdens het aanpassen.";

            _dialog.ShowMessage("Contactpersoon bewerken", Message);
        }

        private void Delete(ContactpersonOverviewViewModel overview)
        {
            var message = Service.Delete(Data)
                ? "De contactpersoon is verwijderd!"
                : "Er is iets misgegaan tijdens het verwijderen.";

            _dialog.ShowMessage("Contactpersoon verwijderen", message);

            overview.Contactpersons.Remove(this);
            overview.NewContactperson();
        }

        #region Properties

        public Client Client
        {
            get { return Data.Client; }
            set { Data.Client = value; }
        }

        public string Firstname
        {
            get { return Data.firstname; }
            set { Data.firstname = value; }
        }

        public string Lastname
        {
            get { return Data.lastname; }
            set { Data.lastname = value; }
        }

        #endregion

        #region Property Form

        private string _formFirstname;

        public string FormFirstname
        {
            get { return _formFirstname; }
            set { Set(ref _formFirstname, value); }
        }

        private string _formLastname;

        public string FormLastname
        {
            get { return _formLastname; }
            set { Set(ref _formLastname, value); }
        }

        private Client _formClient;

        public Client FormClient
        {
            get { return _formClient; }
            set { Set(ref _formClient, value); }
        }

        #endregion
    }
}
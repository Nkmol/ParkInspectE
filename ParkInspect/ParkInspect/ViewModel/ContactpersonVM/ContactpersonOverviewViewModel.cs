using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ContactpersonVM
{
    public class ContactpersonOverviewViewModel : ViewModelBase
    {
        private readonly IRepository _context;
        private readonly DialogManager _dialog;

        private ContactpersonViewModel _selectedContactperson;

        public ContactpersonOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _context = context;
            _dialog = dialog;

            Data =
                new ObservableCollection<ContactpersonViewModel>(
                    context.GetAll<Contactperson>().Select(x => new ContactpersonViewModel(context, x, dialog)));
            Contactpersons = Data;

            NewCommand = new RelayCommand(NewContactperson);
            NewContactperson();
        }

        private ObservableCollection<ContactpersonViewModel> Data { get; set; }

        public ObservableCollection<ContactpersonViewModel> Contactpersons { get; set; }

        public ContactpersonViewModel SelectedContactperson
        {
            get { return _selectedContactperson; }
            set
            {
                Set(ref _selectedContactperson, value);
                SelectedContactperson?.Reset();
            }
        }

        public RelayCommand NewCommand { get; set; }

        public void NewContactperson()
        {
            SelectedContactperson = new ContactpersonViewModel(_context, new Contactperson(), _dialog);
            RaisePropertyChanged();
        }

        public void ContactpersonsChanged()
        {
            Data = new ObservableCollection<ContactpersonViewModel>(_context.GetAll<Contactperson>().Select(x => new ContactpersonViewModel(_context, x, _dialog)));
            Contactpersons = Data;
            RaisePropertyChanged("Contactpersons");
        }

        private void UpdateContactpersons()
        {
            var builder = new FilterBuilder();
            builder.Add("Firstname", FirstnameFilter);
            builder.Add("Lastname", LastnameFilter);
            builder.Add("Client.name", ClientFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Contactpersons = new ObservableCollection<ContactpersonViewModel>(result);
            RaisePropertyChanged("Contactpersons");
        }

        #region Filters

        private string _firstnameFilter;

        public string FirstnameFilter
        {
            get { return _firstnameFilter; }
            set
            {
                _firstnameFilter = value;
                UpdateContactpersons();
            }
        }

        private string _lastnameFilter;

        public string LastnameFilter
        {
            get { return _lastnameFilter; }
            set
            {
                _lastnameFilter = value;
                UpdateContactpersons();
            }
        }

        private string _clientFilter;

        public string ClientFilter
        {
            get { return _clientFilter; }
            set
            {
                _clientFilter = value;
                UpdateContactpersons();
            }
        }

        #endregion
    }
}
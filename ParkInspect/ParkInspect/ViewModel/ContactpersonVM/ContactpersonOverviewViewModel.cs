using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;

namespace ParkInspect.ViewModel.ContactpersonVM
{
    public class ContactpersonOverviewViewModel
    {
        private ObservableCollection<ContactpersonViewModel> Data;
        private IRepository _context;
        private DialogManager _dialog;

        private Contactperson SelectedContactperson
        {
            get
            {
                
            }
            set
            {
                
            }
        }

        public ContactpersonOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _context = context;
            _dialog = dialog;
        }

        #region Properties

        public string FormFirstname
        {
            get
            {
                
            }
            set
            {
                
            }
        }

        #endregion

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

        private void UpdateContactpersons()
        {
            var builder = new FilterBuilder();
            builder.Add("firstname", FirstnameFilter);
            builder.Add("lastname", LastnameFilter);
            builder.Add("Client.name", ClientFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Contactpersons = new ObservableCollection<Contactperson>(result);
            RaisePropertyChanged("Contactpersons");
        }
    }
}

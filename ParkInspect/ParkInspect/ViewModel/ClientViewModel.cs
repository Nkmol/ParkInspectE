using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        public ObservableCollection<Client> Clients { get; set; }
        protected ClientService Service;

        public ClientViewModel(IRepository context)
        {
            Service = new ClientService(context);
            Clients = new ObservableCollection<Client>(Service.GetClientWithName("Klaas"));
        }
    }
}

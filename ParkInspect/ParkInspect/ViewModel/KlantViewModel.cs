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
    public class KlantViewModel : ViewModelBase
    {
//        public ObservableCollection<Client> Klanten { get; set; }
        protected KlantService Service;

        public KlantViewModel(IRepository context)
        {
            Service = new KlantService(context);
//            Klanten = new ObservableCollection<Klant>(Service.GetKlantsWithName("Klaas"));
        }
    }
}

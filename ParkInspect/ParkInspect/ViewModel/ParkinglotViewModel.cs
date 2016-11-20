using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{

    public class ParkinglotViewModel : ViewModelBase
    {
        public ObservableCollection<Parkinglot> Parkinglots { get; set; }
        protected ParkinglotService Service { get; set; }
        public Parkinglot Parkinglot { get; set; }

        public ParkinglotViewModel(IRepository context)
        {

            Service = new ParkinglotService(context);
            Parkinglots = new ObservableCollection<Parkinglot>(Service.GetAllParkinglots());
            Parkinglot = new Parkinglot();
            
        }

        private void Save()
        {
            Service.AddParkinglot(Parkinglot);
        }
    }
}
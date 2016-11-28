using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations.Model;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{

    public class ParkinglotViewModel : ViewModelBase
    {

        public ObservableCollection<Parkinglot> Parkinglots { get; set; }
        public ObservableCollection<Region> Regions { get; set; }
        public ObservableCollection<Inspection> Inspections { get; set; }
        protected ParkinglotService Service { get; set; }
        private Parkinglot _parkinglot;

        public Parkinglot Parkinglot
        {
            get
            {
                return _parkinglot;
            }
            set
            {
                _parkinglot = value;
                RaisePropertyChanged("Parkinglot");
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedRegion { get; set; }



        public RelayCommand SaveCommand { get; set; }
        public RelayCommand NewCommand { get; set; }

        public ParkinglotViewModel(IRepository context)
        {

            SaveCommand = new RelayCommand(Save, () => CanSave());
            NewCommand = new RelayCommand(NewParkinglot);
            Service = new ParkinglotService(context);
            Parkinglots = new ObservableCollection<Parkinglot>(Service.GetAllParkinglots());
            Regions = new ObservableCollection<Region>(Service.GetAllRegions());
            NewParkinglot();
           
        }

        private void NewParkinglot()
        {

            Parkinglot = new Parkinglot();
            RaisePropertyChanged("Parkinglot");
            Parkinglot.id = -1;

        }

        private void Save()
        {


            Parkinglot.region_name = SelectedRegion;

            if (Parkinglot.id < 0)
            {
                if (Service.AddParkinglot(Parkinglot))
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Fail");
                }

            }
            else

            {
                if (Service.UpdateParkinglot(Parkinglot))
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Fail");
                }
            }
        }

        private bool CanSave()
        {

            if (Parkinglot == null)
                return false;

            if (Parkinglot.name == null || Parkinglot.name.Equals(""))
                return false;

            if (Parkinglot.region_name == null || Parkinglot.region_name.Equals(""))
                return false;

            if (Parkinglot.clarification == null || Parkinglot.clarification.Equals(""))
                return false;

            if (Parkinglot.zipcode == null || Parkinglot.zipcode.Equals(""))
                return false;

            if (Parkinglot.number == null || Parkinglot.number < 1)
                return false;

            return true;


        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using ParkInspect.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace ParkInspect.ViewModel
{
    public class PrepareViewModel : ViewModelBase
    {
        protected InspectionService service;
        public ObservableCollection<Inspection> inspections { get; set; }
        public Inspection _selectedInspection;
        public string _client_name;
        public string _region_name;
        public string _parkinglot_name;
        public string _region_zip;
        public int _region_number;
        public string _clarification;

        public String client_name
        {
            get
            {
                return _client_name;
            }
            set
            {
                _client_name = value;
                base.RaisePropertyChanged();

            }
        }
        public String parkinglot_name
        {
            get
            {
                return _parkinglot_name;
            }
            set
            {
                _parkinglot_name = value;
                base.RaisePropertyChanged();
            }
        }
        public String region_name
        {
            get
            {
                return _region_name;
            }
            set
            {
                _region_name = value;
                base.RaisePropertyChanged();
            }
        }

        public String region_zip
        {
            get
            {
                return _region_zip;
            }
            set
            {
                _region_zip = value;
                base.RaisePropertyChanged();

            }
        }
        public int region_number
        {
            get
            {
                return _region_number;
            }
            set
            {
                _region_number = value;
                base.RaisePropertyChanged();

            }
        }
        public String clarification
        {
            get
            {
                return _clarification;
            }
            set
            {
                _clarification = value;
                base.RaisePropertyChanged();

            }
        }
        public Inspection selectedInspection
        {
            get
            {
                return _selectedInspection;
            }
            set
            {
                _selectedInspection = value;
                client_name = selectedInspection.Asignment.Client.name;
                region_name = selectedInspection.Parkinglot.Region.name;
                parkinglot_name = selectedInspection.Parkinglot.name;
                region_zip = selectedInspection.Parkinglot.zipcode;
                region_number = (int)selectedInspection.Parkinglot.number;
                clarification = selectedInspection.clarification;
                base.RaisePropertyChanged();
            }
        }

        public PrepareViewModel(IRepository context)
        {
            service = new InspectionService(context);
            inspections = new ObservableCollection<Inspection>(service.GetAllInspections());
        }
    }
}

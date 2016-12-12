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
using System.Windows;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi;
using System.Text.RegularExpressions;
using System.Reflection;

namespace ParkInspect.ViewModel
{
    public class PrepareViewModel : ViewModelBase
    {
        protected InspectionService service;
        public ObservableCollection<String> directionItems { get; set; }
        public ObservableCollection<Inspection> inspections { get; set; }
        private OfflineViewMode offlineViewModel;
        public Inspection _selectedInspection;
        public string _client_name;
        public string _region_name;
        public string _parkinglot_name;
        public string _region_zip;
        public int _region_number;
        public string _clarification;
        public string _directionsName;
        private string _home_adress;
        public RelayCommand saveDirections{ get; set; }
        public RelayCommand getDirections { get; set; }

        #region properties

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
        public String home_adress
        {
            get
            {
                return _home_adress;
            }
            set
            {
                _home_adress = value;
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
        public String directions_name
        {
            get
            {
                return _directionsName;
            }
            set
            {
                _directionsName = value;
                base.RaisePropertyChanged();

            }
        }
        #endregion
        public PrepareViewModel(IRepository context, OfflineViewMode offlineViewModel)
        {
            service = new InspectionService(context);
            directionItems = new ObservableCollection<string>();
            inspections = new ObservableCollection<Inspection>(service.GetAllInspections());
            saveDirections = new RelayCommand(SaveDirections);
            getDirections = new RelayCommand(GetDirections);
            this.offlineViewModel = offlineViewModel;

        }
        private void SaveDirections()
        {
            String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (_directionsName != "")
            {
                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(runpath + "/directions/" + _directionsName + ".txt");
                foreach (var item in directionItems)
                {
                    SaveFile.WriteLine(item.ToString());
                }
                offlineViewModel.LoadDirections();
            }
            else
            {
                MessageBox.Show("Vul een naam in!");
            }
        }
        private void GetDirections()
        {
            var drivingDirectionRequest = new DirectionsRequest
            {
                Origin = home_adress,
                Destination = region_zip + " " + region_name
            };
            drivingDirectionRequest.Language = "nl";
            DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
            Route nRoute = drivingDirections.Routes.First();
            Leg leg = nRoute.Legs.First();
            int counter = 1;
            foreach (Step step in leg.Steps)
            {
               directionItems.Add(counter + ". " + StripHTML(step.HtmlInstructions));
                counter++;
            }
        }
        private string StripHTML(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
    }
}

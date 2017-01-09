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
using System.IO;
using MahApps.Metro.Controls.Dialogs;
using ParkInspect.Model;

namespace ParkInspect.ViewModel
{
    public class PrepareViewModel : ViewModelBase
    {
        public InspectionService service;
        public ObservableCollection<String> directionItems { get; set; }
        public ObservableCollection<Inspection> inspections { get; set; }
        public ObservableCollection<Direction> directions { get; set; }

        public Inspection _selectedInspection;
        public Direction _selectedDirection;
        public string _inspection_id;
        public string _inspection_date;
        public string _inspection_deadline;
        public string _client_name;
        public string _region_name;
        public string _parkinglot_name;
        public string _region_zip;
        public string _region_number;
        public string _clarification;
        public string _directions_save_name;
        public int _selectedInspectionID;
        private string _home_adress;
        private String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private string _current_direction_item;
        private DialogManager _dialog;
        private OfflineViewModel vm;
        private string _street;

        public RelayCommand saveDirections { get; set; }
        public RelayCommand getDirections { get; set; }

        #region properties
        public string inspection_id
        {
            get
            {
                return _inspection_id;
            }
            set
            {
                _inspection_id = value;
                base.RaisePropertyChanged();

            }
        }
        public String inspection_date
        {
            get
            {
                return _inspection_date;
            }
            set
            {
                _inspection_date = value;
                base.RaisePropertyChanged();

            }
        }
        public String inspection_deadline
        {
            get
            {
                return _inspection_deadline;
            }
            set
            {
                _inspection_deadline = value;
                base.RaisePropertyChanged();

            }
        }
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
        public String street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value;
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
        public string region_number
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
                inspection_id = selectedInspection.id.ToString();
                inspection_date = selectedInspection.date.ToString();
                inspection_deadline = selectedInspection.deadline.ToString();
                client_name = selectedInspection.Asignment.Client.name;
                region_name = selectedInspection.Parkinglot.Region.name;
                parkinglot_name = selectedInspection.Parkinglot.name;
                street = selectedInspection.Parkinglot.streetname;
                region_zip = selectedInspection.Parkinglot.zipcode;
                region_number = selectedInspection.Parkinglot.number.ToString();
                clarification = selectedInspection.clarification;
                directionItems.Clear();
                base.RaisePropertyChanged();
            }
        }
        public Direction selectedDirection
        {
            get
            {
                return _selectedDirection;
            }
            set
            {
                _selectedDirection = value;
                base.RaisePropertyChanged();
            }
        }
        public String current_direction_item
        {
            get
            {
                return _current_direction_item;
            }
            set
            {
                _current_direction_item = value;
                base.RaisePropertyChanged();

            }
        }
        public String directions_save_name
        {
            get
            {
                return _directions_save_name;
            }
            set
            {
                _directions_save_name = value;
                base.RaisePropertyChanged();

            }
        }
        #endregion
        public PrepareViewModel(IRepository context, DialogManager dialog, OfflineViewModel offlineVM)
        {
            vm = offlineVM;
            _dialog = dialog;
            service = new InspectionService(context);
            directions = new ObservableCollection<Direction>();
            directionItems = new ObservableCollection<string>();
            inspections = new ObservableCollection<Inspection>(service.GetAllInspections());
            saveDirections = new RelayCommand(SaveDirections);
            getDirections = new RelayCommand(GetDirections);

        }
        private void SaveDirections()
        {
            if (directionItems.Count > 0)
            {
                String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                {
                    System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(runpath + "/directions/" + _directions_save_name + ".txt");
                    SaveFile.WriteLine("ID:" + selectedInspection.id);
                    SaveFile.WriteLine("HOME:" + _home_adress);
                    foreach (String s in directionItems)
                    {
                        SaveFile.WriteLine(s);
                    }
                    SaveFile.Close();
                    _dialog.ShowMessage("Succes!", "De routebeschrijving is succesvol opgelsagen!");
                }

                vm.LoadDirections();
            }
            else
            {
                _dialog.ShowMessage("Fout!", "Laad eerst een navigatie in!");
            }
        }
        private void GetDirections()
        {
            if (String.IsNullOrWhiteSpace(home_adress))
            {
                _dialog.ShowMessage("Fout!", "Voer een geldig vertrek adres in!");
            }
            else
            {
                //route on gmaps
                try
                {
                    var drivingDirectionRequest = new DirectionsRequest
                    {
                        Origin = home_adress,
                        Destination = street + " " + region_zip + " " + region_name
                    };
                    drivingDirectionRequest.Language = "nl";
                    DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
                    Route nRoute = drivingDirections.Routes.First();
                    Leg leg = nRoute.Legs.First();
                    int counter = 1;
                    //direction items
                    directionItems.Clear();
                    foreach (Step step in leg.Steps)
                    {
                        directionItems.Add(counter + ". " + StripHTML(step.HtmlInstructions));
                        counter++;
                    }
                }
                catch (Exception e)
                {
                    directionItems.Clear();
                    _dialog.ShowMessage("Fout!", "Er ging iets fout met het laden van de routebeschrijving! (Zijn de adres gegevens correct?)");
                }
            }
        }
        private string StripHTML(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
    }
}
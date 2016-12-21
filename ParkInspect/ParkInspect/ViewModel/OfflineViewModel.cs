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

namespace ParkInspect.ViewModel
{
    public class OfflineViewModel : ViewModelBase
    {
        public InspectionService service;
        public ObservableCollection<String> directionItems { get; set; }
        public ObservableCollection<Direction> _directions;

        public Inspection _selectedInspection;
        public Direction _selectedDirection;
        public int _inspection_id;
        public string _inspection_date;
        public string _inspection_deadline;
        public string _client_name;
        public string _region_name;
        public string _parkinglot_name;
        public string _region_zip;
        public int _region_number;
        public string _clarification;
        public string _directions_save_name;
        private string _home_adress;
        private String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private string _current_direction_item;
        private DialogManager _dialog;
        public RelayCommand saveDirections { get; set; }
        public RelayCommand getDirections { get; set; }
        public RelayCommand deleteInspection { get; set; }
        public RelayCommand setDirections { get; set; }
        public RelayCommand next_direction { get; set; }
        public RelayCommand prev_direction { get; set; }


        public class Direction
        {
            private string _Name;
            public List<String> direction_items = new List<string>();
            public int index = 0;
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    if (_Name != value)
                    {
                        _Name = value;
                    }
                }
            }
        }
        #region properties
        public int inspection_id
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
                inspection_id = selectedInspection.id;
                inspection_date = selectedInspection.date.ToString();
                inspection_deadline = selectedInspection.deadline.ToString();
                client_name = selectedInspection.Asignment.Client.name;
                region_name = selectedInspection.Parkinglot.Region.name;
                parkinglot_name = selectedInspection.Parkinglot.name;
                region_zip = selectedInspection.Parkinglot.zipcode;
                region_number = (int)selectedInspection.Parkinglot.number;
                clarification = selectedInspection.clarification;
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
                SetDirectionItems();
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
        public ObservableCollection<Direction> directions
        {
            get
            {
                return _directions;
            }
            set
            {
                _directions = value;
                base.RaisePropertyChanged();
            }
        }
        public OfflineViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            service = new InspectionService(context);
            directions = new ObservableCollection<Direction>();
            directionItems = new ObservableCollection<string>();
            saveDirections = new RelayCommand(SaveDirections);
            deleteInspection = new RelayCommand(DeleteInspection);
            next_direction = new RelayCommand(NextDirection);
            prev_direction = new RelayCommand(PrevDirection);
            getDirections = new RelayCommand(GetDirections);
            LoadDirections();
        }
        private void PrevDirection()
        {
            if (_selectedDirection.index > 0)
            {
                int index = _selectedDirection.index;
                current_direction_item = _selectedDirection.direction_items[index - 1];
                _selectedDirection.index--;
            }
        }
        private void NextDirection()
        {
            if (_selectedDirection.index + 1 < _selectedDirection.direction_items.Count)
            {
                int index = _selectedDirection.index;
                current_direction_item = _selectedDirection.direction_items[index + 1];
                _selectedDirection.index++;
            }
        }
        private void SaveDirections()
        {
            if (directionItems.Count > 0)
            {
                String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (String.IsNullOrWhiteSpace(_directions_save_name))
                {
                    _dialog.ShowMessage("Fout!", "Vul een geldige naam in!");
                }
                else
                {
                    System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(runpath + "/directions/" + _directions_save_name + ".txt");
                    SaveFile.WriteLine("ID:" + selectedInspection.id);
                    SaveFile.WriteLine("HOME:" + _home_adress);
                    foreach (String s in directionItems)
                    {
                        SaveFile.WriteLine(s);
                    }
                    SaveFile.Dispose();
                    SaveFile.Close();
                    _dialog.ShowMessage("Succes!", "De routebeschrijving is succesvol opgelsagen!");
                }

                LoadDirections();
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
                        Destination = region_zip + " " + region_name
                    };
                    drivingDirectionRequest.Language = "nl";
                    DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
                    Route nRoute = drivingDirections.Routes.First();
                    Leg leg = nRoute.Legs.First();
                    int counter = 1;
                    //direction items
                    foreach (Step step in leg.Steps)
                    {
                        directionItems.Add(counter + ". " + StripHTML(step.HtmlInstructions));
                        counter++;
                    }
                }
                catch (Exception e)
                {
                    _dialog.ShowMessage("Fout!", "Er ging iets fout met het laden van de routebeschrijving!");
                }
            }
        }
        private void SetDirectionItems()
        {
            String line;
            System.IO.StreamReader file = new System.IO.StreamReader(runpath + "/directions/" + _selectedDirection.Name + ".txt");
            while ((line = file.ReadLine()) != null)
            {
                if (!line.Contains("ID:") && !line.Contains("HOME:"))
                {
                    _selectedDirection.direction_items.Add(line);
                }
            }
            file.Dispose();
            file.Close();
            current_direction_item = _selectedDirection.direction_items[_selectedDirection.index];

        }
        private string StripHTML(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
        public void LoadDirections()
        {
            directions = new ObservableCollection<Direction>();
            foreach (String name in Directory.GetFiles(runpath + "/directions", "*.txt").Select((Path.GetFileNameWithoutExtension)))
            {
                Direction direction = new Direction();
                direction.Name = name;
                directions.Add(direction);
            }
        }
        private void DeleteInspection()
        {
            File.Delete(runpath + "/directions/" + selectedDirection.Name + ".txt");
            //LoadDirections(); gvd
            _dialog.ShowMessage("Succes!", "De opgeslagen inspectie is verwijderd!");
        }

    }
}

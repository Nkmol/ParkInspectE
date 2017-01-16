using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using ParkInspect.Services;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Reflection;
using System.IO;
using ParkInspect.Model;

namespace ParkInspect.ViewModel
{
    public class OfflineViewModel : ViewModelBase
    {
        public InspectionService service;
        public ObservableCollection<String> directionItems { get; set; }
        public ObservableCollection<Direction> _directions;

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
        private string _home_adress;
        private List<string> deleted_list;
        private String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private string _current_direction_item;
        private DialogManager _dialog;
        private string _street;

        public RelayCommand deleteInspection { get; set; }
        public RelayCommand setDirections { get; set; }
        public RelayCommand next_direction { get; set; }
        public RelayCommand prev_direction { get; set; }

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
                region_zip = selectedInspection.Parkinglot.zipcode;
                street = selectedInspection.Parkinglot.streetname;
                region_number = selectedInspection.Parkinglot.number.ToString();
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
                if (value != null)
                {
                    _selectedDirection = value;
                    SetDirectionItems();
                    base.RaisePropertyChanged();
                }
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
            Directory.CreateDirectory(runpath + "/directions");
            _dialog = dialog;
            deleted_list = new List<string>();
            service = new InspectionService(context);
            directions = new ObservableCollection<Direction>();
            directionItems = new ObservableCollection<string>();
            deleteInspection = new RelayCommand(DeleteInspection);
            next_direction = new RelayCommand(NextDirection);
            prev_direction = new RelayCommand(PrevDirection);
            Reset();
            CleanFiles();
            LoadDirections();
        }
        private void PrevDirection()
        {
            if (selectedDirection != null)
            {
                if (_selectedDirection.index > 0)
                {
                    int index = _selectedDirection.index;
                    current_direction_item = _selectedDirection.direction_items[index - 1];
                    _selectedDirection.index--;
                }
            }
        }
        private void NextDirection()
        {
            if (selectedDirection != null)
            {
                if (_selectedDirection.index + 1 < _selectedDirection.direction_items.Count)
                {
                    int index = _selectedDirection.index;
                    current_direction_item = _selectedDirection.direction_items[index + 1];
                    _selectedDirection.index++;
                }
            }
        }
        private void SetDirectionItems()
        {
            String line;
            System.IO.StreamReader file;
            if (_selectedDirection != null)
            {
                {

                    file = new System.IO.StreamReader(runpath + "/directions/" + _selectedDirection.Name + ".txt");
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
                file.Dispose();
                file.Close();
            }
        }
        public void LoadDirections()
        {
            if (selectedDirection == null)
            {
                directions.Clear();
            }
            foreach (String name in Directory.GetFiles(runpath + "/directions", "*.txt").Select((Path.GetFileNameWithoutExtension)))
            {
                if (!name.Equals("deletelist"))
                {
                    if (!DirectionExists(name))
                    {
                        Direction direction = new Direction();
                        direction.Name = name;
                        directions.Add(direction);
                    }
                }
            }
        }
        private bool DirectionExists(String name)
        {
            foreach (Direction d in directions)
            {
                if (d.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
        private void DeleteInspection()
        {
            String temp_name;
            if (selectedDirection != null)
            {
                temp_name = selectedDirection.Name;
                directions.Remove(selectedDirection);
                deleted_list.Add(selectedDirection.Name);
                selectedDirection = null;
                AddtoDeleteList(temp_name);
                Reset();
                _dialog.ShowMessage("Succes!", "De opgeslagen inspectie is verwijderd!");

            }
            else
            {
                _dialog.ShowMessage("Er ging iets fout!", "Je moet wel een opgeslagen routebeschrijving selecteren!");
            }

        }
        public void CleanFiles()
        {
            String path = runpath + "/directions/deletelist.txt";
            FileInfo fileInfo = new FileInfo(runpath + "/directions/deletelist.txt");

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                { }
            }
            String line;
            System.IO.StreamReader fileReader = new System.IO.StreamReader(runpath + "/directions/deletelist.txt");
            while ((line = fileReader.ReadLine()) != null)
            {
                DeleteFile(line);
            }
            fileReader.Close();
            File.WriteAllText(runpath + "/directions/deletelist.txt", string.Empty);

        }
        private void DeleteFile(string name)
        {
            FileInfo file = new FileInfo(runpath + "/directions/" + name + ".txt");
            if (!IsFileLocked(file))
            {
                file.Delete();
                deleted_list.Remove(name);
            }

        }
        private void AddtoDeleteList(String name)
        {
            String path = runpath + "/directions/deletelist.txt";
            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                { }
            }
            TextWriter tw = new StreamWriter(path, true);
            tw.WriteLine(name);
            tw.Dispose();
            tw.Close();


        }

        private void Reset()
        {
            inspection_id = "";
            inspection_date = "";
            inspection_deadline = "";
            client_name = "";
            region_name = "";
            parkinglot_name = "";
            region_zip = "";
            street = "";
            region_number = "";
            clarification = "";
            current_direction_item = "";
            if (_selectedDirection != null)
            {
                _selectedDirection.direction_items.Clear();
                _selectedDirection.index = 0;
            }
        }
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

    }
}

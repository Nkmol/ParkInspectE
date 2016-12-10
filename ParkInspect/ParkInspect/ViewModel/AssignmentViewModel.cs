using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AssignmentViewModel: ViewModelBase
    {
        private ObservableCollection<Asignment> _opdrachtCollection;

        public ObservableCollection<Asignment> OpdrachtenCollection
        {
            get { return _opdrachtCollection; }
            set
            {
                _opdrachtCollection = value;

                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<Asignment> _searchedAsignments;

        public ObservableCollection<Asignment> SearchedAsignments
        {
            get { return _searchedAsignments; }
            set
            {
                _searchedAsignments = value;
                base.RaisePropertyChanged();
            }
        }



        private readonly AssignmentService _service;


        // should be a unique object not a reference to an object in OpdrachtCollection/SearchedCollection.
        private Asignment _selectedAsignment;
        public Asignment SelectedAsignment
        {
            get { return _selectedAsignment; }

            set
            {
                Set(ref _selectedAsignment, value);

                base.RaisePropertyChanged();
            }
        }

        private Inspection _selectedInspection;
        public Inspection SelectedInspection
        {
            get
            {
                return _selectedInspection;
            }
            set
            {
                _selectedInspection = value;
                base.RaisePropertyChanged();
            }
        }


        public List<string> InspectionIdentifier
        {
            get
            {
                var temp = new List<string>();
                foreach (var var in InspectionList)
                {
                    temp.Add(var.Parkinglot.name + " " + var.State1.state1);
                }
                return temp;
            }
        }





        private string _searchbar;

        public string SearchBar
        {
            get { return _searchbar; }
            set
            {
                _searchbar = value;
                base.RaisePropertyChanged();
            }
        }

        private IEnumerable<State> _assignmentStateList;

        public IEnumerable<State> AssignmentStatusList
        {
            get { return _assignmentStateList; }
            set
            {
                _assignmentStateList = value;
                base.RaisePropertyChanged();
            }
        }

        private IEnumerable<Client> _clientList;

        public IEnumerable<Client> ClientList
        {
            get { return _clientList; }
            set
            {
                _clientList = value;
                base.RaisePropertyChanged();
            }
        }

        private IEnumerable<Inspection> _inspectionlist;
        public IEnumerable<Inspection> InspectionList
        {
            get { return _inspectionlist; }
            set
            {
                _inspectionlist = value;
                base.RaisePropertyChanged();

            }
        }


        public ICommand CreateAsignmentCommand { get; set; }
        public ICommand EditAsignmentCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand RemoveInspectionCommand { get; set; }
        public ICommand CreateInspectionCommand { get; set; }


        public ICommand ResetCommand { get; set; }

        private string _commandError;

        public string CommandError
        {
            get { return _commandError; }
            set
            {
                _commandError = value;
                base.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the OpdrachtViewModel class.
        /// </summary>
        public AssignmentViewModel(IRepository repository)
        {
            _service = new AssignmentService(repository);
            UpdateProperties();
            SetEmptySelectedAsignment();


            CreateAsignmentCommand = new RelayCommand(CreateAsignment, CanCreateAsignment);
            EditAsignmentCommand = new RelayCommand(EditAsignment, CanEditAsignment);

            RemoveInspectionCommand = new RelayCommand(RemoveInsection);
            CreateInspectionCommand = new RelayCommand(CreateInspection);

            SearchCommand = new RelayCommand(Search);
            ResetCommand = new RelayCommand(ResetAsignement);
        }



        private void UpdateProperties()
        {
            OpdrachtenCollection = new ObservableCollection<Asignment>(_service.GetAllAsignments());
            SearchedAsignments = OpdrachtenCollection;
            ClientList = _service.GetAllClients();
            AssignmentStatusList = _service.GetAllStates();
            InspectionList = _service.GetAllInspections();

            _searchbar = "";


        }

        // function should be cahnged with popup implementation
        public void RemoveInsection()
        {
            if (_selectedInspection != null && EditValidation())
            {
                _selectedAsignment.Inspections.Remove(_selectedInspection);

                _service.UpdateAssignment(_selectedAsignment);
                UpdateProperties();
            }
            else
            {
                CommandError = "Inspection Remove Error.";
            }
        }

        // funcion should be changed to creating a new inspection, will be implemented as ading an existing one for now.
        public void CreateInspection()
        {
            if (_selectedInspection != null && EditValidation())
            {

                _selectedAsignment.Inspections.Add(_selectedInspection);

                _service.UpdateAssignment(_selectedAsignment);
                UpdateProperties();
            }
            else
            {
                CommandError = "Inspecion addition error.";
            }
        }


        public void CreateAsignment()
        {
            // needs validation
            if (CreateValidation())
            {
                if (_selectedAsignment.date == null)
                {
                    _selectedAsignment.date = DateTime.Today;
                }


                _service.CreateNewAssignemnt(_selectedAsignment);
                SetEmptySelectedAsignment();
                CommandError = "Created";
                UpdateProperties();
            }
        }

        private bool CanCreateAsignment()
        {
            return _selectedAsignment.id == 0;
        }

        private bool CreateValidation()
        {

            CommandError = "";
            // IT messages.
            if (_selectedAsignment == null)
            {
                CommandError = "No Selected Asignment, please contact your IT department.";
                return false;
            }
            if (_selectedAsignment.state == null) CommandError = "State is null, please contact your IT department.";

            // user messages.
            if (_selectedAsignment.Client == null) CommandError = "Geen klant geselecteerd.";
            if (_selectedAsignment.State1 == null) CommandError = "Geen status geselecteerd.";
            if (_selectedAsignment.deadline == DateTime.MinValue) CommandError = "De gestelde deadline is niet geldig.";

            return CommandError.Equals("");



        }



        public void EditAsignment()
        {
            if (EditValidation())
            {

                try
                {
                    _service.UpdateAssignment(_selectedAsignment);
                    SetEmptySelectedAsignment();

                    CommandError = "Updated";
                    UpdateProperties();
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(InvalidOperationException))
                    {
                        _service.DeleteAssignment(_selectedAsignment);
                        _service.CreateNewAssignemnt(_selectedAsignment);
                        SetEmptySelectedAsignment();

                        CommandError = "Updated";
                        UpdateProperties();
                    }
                    else
                    {
                        CommandError = "Exeption thrown contact IT.";
                    }


                }


            }
        }

        private bool CanEditAsignment()
        {
            return _selectedAsignment.id != 0;
        }


        private bool EditValidation()
        {
            CommandError = "";

            // IT messages.
            if (_selectedAsignment == null)
            {
                CommandError =
                    "_selectedAsignment is null, please contact your ICT department, something went horrably wrong";
                return false;
            }
            if (_selectedAsignment.client_id == 0) CommandError = "Clientid is null, please contact you IT department.";


            // user messages.
            if (_selectedAsignment.Client == null) CommandError = "No Client Selected.";
            if (_selectedAsignment.State1 == null) CommandError = "No Status selected.";
            if (_selectedAsignment.deadline == DateTime.MinValue) CommandError = "Unless your client is jesus christ, your deadline is not set properly.";
            if (OpdrachtenCollection.FirstOrDefault(a => a.id == _selectedAsignment.id) == null) CommandError = "Selected assignment could not be found. Maybe someone removed it.";


            return CommandError.Equals("");
        }

        public void ResetAsignement()
        {
            SetEmptySelectedAsignment();

            CommandError = "";
        }

        public void Search()
        {
            AlterVisableAsignments();
        }



        private void AlterVisableAsignments()
        {

            if (SearchBar.Equals("") || SearchBar == null)
            {
                SearchedAsignments = OpdrachtenCollection;
                return;
            }
            var temp = new ObservableCollection<Asignment>();

            foreach (var asignment in OpdrachtenCollection)
            {
                if (asignment.Client.name != null && asignment.clarification != null)
                {
                    if (asignment.Client.name.ToUpper().Contains(SearchBar.ToUpper()) || asignment.clarification.ToUpper().Contains(SearchBar.ToUpper()))
                    {
                        temp.Add(asignment);
                    }
                }
            }

            SearchedAsignments = temp;
        }

        private void SetEmptySelectedAsignment()
        {
            SelectedAsignment = new Asignment { deadline = DateTime.Today };
            base.RaisePropertyChanged();

        }
    }
}
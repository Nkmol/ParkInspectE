using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
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
    public class AssignmentViewModel : ViewModelBase
    {
        private IEnumerable<Asignment> _opdrachtCollection;

        public IEnumerable<Asignment> OpdrachtenCollection
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



        #region All filters

        private string _clientfilter;

        public string ClientFilter
        {
            get { return _clientfilter; }
            set
            {
                _clientfilter = value;
                Search();
                base.RaisePropertyChanged();
            }
        }

        private string _datefilter;

        public string DateFilter
        {
            get { return _datefilter; }
            set
            {
                _datefilter = value;
                Search();
                base.RaisePropertyChanged();
            }
        }
        private string _statefilter;

        public string StateFilter
        {
            get { return _statefilter; }
            set
            {
                _statefilter = value;
                Search();
                base.RaisePropertyChanged();
            }
        }
        private string _deadlineFilter;

        public string DeadlineFilter
        {
            get { return _deadlineFilter; }
            set
            {
                _deadlineFilter = value;
                Search();
                base.RaisePropertyChanged();
            }
        }
        private string _clarificationFilter;

        public string ClarificationFilter
        {
            get { return _clarificationFilter; }
            set
            {
                _clarificationFilter = value;
                Search();
                base.RaisePropertyChanged();
            }
        }




        #endregion

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


            EditAsignmentCommand = new RelayCommand(EditAsignment);

            RemoveInspectionCommand = new RelayCommand(RemoveInspection);
            CreateInspectionCommand = new RelayCommand(CreateInspection);

            SearchCommand = new RelayCommand(Search);
            ResetCommand = new RelayCommand(ResetAsignement);
        }



        private void UpdateProperties()
        {
            OpdrachtenCollection = new ObservableCollection<Asignment>(_service.GetAllAsignments());
            SearchedAsignments = new ObservableCollection<Asignment>( OpdrachtenCollection);
            ClientList = _service.GetAllClients();
            AssignmentStatusList = _service.GetAllStates();
            InspectionList = _service.GetAllInspections();

            _clientfilter = "";


        }

        // function should be cahnged with popup implementation
        public void RemoveInspection()
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

        }

        public void EditAsignment()
        {

            if (SelectedAsignment.id != 0)
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
            else if (SelectedAsignment.id == 0)
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
            var builder = new FilterBuilder();
            builder.Add("Client.name", ClientFilter);
            builder.Add("state", StateFilter);
            builder.Add("date", DateFilter);
            builder.Add("deadline", DeadlineFilter);
            builder.Add("clarification", ClarificationFilter);

            var filters = builder.Get();
            var result = OpdrachtenCollection.Where(a => a.Like(filters));
            SearchedAsignments = new ObservableCollection<Asignment>(result);
            RaisePropertyChanged("SearchedAsignments");
        }

        private void SetEmptySelectedAsignment()
        {
            SelectedAsignment = new Asignment { deadline = DateTime.Today };
            base.RaisePropertyChanged();

        }
    }
}
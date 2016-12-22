using System;
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
    public class InspectionViewModel : ViewModelBase
    {
        // collections
        private ObservableCollection<Inspection> _allInspections;
        public ObservableCollection<State> InspectionStateList { get; set; }
        public ObservableCollection<Parkinglot> ParkinglotList { get; set; }
        public ObservableCollection<Asignment> Assignmentlist { get; set; }
        public ObservableCollection<Form> FormList { get; set; }

        private ObservableCollection<Inspection> _inspections;
        public ObservableCollection<Inspection> InspectieCollection
        {
            get
            {
                return _inspections;
            }
            set
            {
                _inspections = value;
                base.RaisePropertyChanged();
            }
        }

       
        private ObservableCollection<Employee> _employeelist;
        public ObservableCollection<Employee> EmployeesList
        {
            get { return _employeelist; }
            set
            {
                _employeelist = value;
                base.RaisePropertyChanged();
            }
        }
     

        // functional properties
        private Inspection _selectedInspection;
        public Inspection SelectedInspection
        {
            get
            {
                return _selectedInspection;

            }
            set
            {
                Set(ref _selectedInspection, value);
                InspectionInspectors = new ObservableCollection<Employee>(value.Employees);

                base.RaisePropertyChanged();
            }
        }


        private ObservableCollection<Employee> _inspecteurs;
        public ObservableCollection<Employee> InspectionInspectors
        {
            get
            {
                return _inspecteurs;
            }
            set
            {
                _inspecteurs = value;
                base.RaisePropertyChanged();
            }
        }
       

        private Employee _selectedInspecteur;
        public Employee SelectedInspecteur
        {
            get
            {
                return _selectedInspecteur;
            }
            set
            {
                _selectedInspecteur = value;
                base.RaisePropertyChanged();
            }
        }


        private Employee _selectedEmployee;
        public Employee SelectedEmployee

        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                base.RaisePropertyChanged();
            }
        }

        #region List of filters
        private string _parkinglotFilter;

        public string ParkinglotFilter
        {
            get
            {
                return _parkinglotFilter;
            }
            set
            {
                _parkinglotFilter = value;
                UpdateOverview();
            }
        }

        private string _stateFilter;

        public string StateFilter
        {
            get
            {
                return _stateFilter;
            }
            set
            {
                _stateFilter = value;
                UpdateOverview();
            }
        }

        private string _dateFilter;

        public string DateFilter
        {
            get
            {
                return _dateFilter;
            }
            set
            {
                _dateFilter = value;
                UpdateOverview();
            }
        }

        private string _deadlineFilter;

        public string DeadlineFIlter
        {
            get
            {
                return _deadlineFilter;
            }
            set
            {
                _deadlineFilter = value;
                UpdateOverview();
            }
        }

        private string _clarificationFilter;

        public string ClarificationFilter
        {
            get
            {
                return _clarificationFilter;
            }
            set
            {
                _clarificationFilter = value;
                UpdateOverview();
            }
        }




        #endregion

        private string _commandError;
        public string CommandError
        {
            get
            {
                return _commandError;

            }

            set
            {
                _commandError = value;
                base.RaisePropertyChanged();
            }
        }

        // commands
        public ICommand ResetCommand { get; set; }
        public ICommand CreateInspectionCommand { get; set; }
        public ICommand EditInspectionCommand { get; set; }
        public ICommand AddInspecteurCommand { get; set; }
        public ICommand RemoveInspecteurCommand { get; set; }

        private readonly InspectionService _service;
       
        public InspectionViewModel(IRepository context)
        {
            // set services and Lists
            _service = new InspectionService(context);
            UpdateProperties();
            SetNewInspection();

            // set commands
            ResetCommand = new RelayCommand(ResetInspection);
            CreateInspectionCommand = new RelayCommand(CreateInspection, CanCreateInspection);
            EditInspectionCommand = new RelayCommand(EditInspection, CanEditInspection);


            RemoveInspecteurCommand = new RelayCommand(RemoveInspecteur, CanRemoveInspecteur);
            AddInspecteurCommand = new RelayCommand(AddInspecteur, CanAddInspecteur);

        }

        private bool CanAddInspecteur()
        {
            return SelectedEmployee != null;
        }

        private bool CanRemoveInspecteur()
        {
            return SelectedInspecteur != null; 
        }

        private void UpdateOverview()
        {
            var builder = new FilterBuilder();
            builder.Add("Parkinglot.name", ParkinglotFilter);
            builder.Add("State1.state1", StateFilter);
            builder.Add("date", DateFilter);
            builder.Add("deadline", DeadlineFIlter);
            builder.Add("clarification", ClarificationFilter);

            var filters = builder.Get();
            var result = _allInspections.Where(a => a.Like(filters));
            InspectieCollection = new ObservableCollection<Inspection>(result);
            RaisePropertyChanged();
        }

        private void UpdateProperties()
        {
            InspectieCollection = new ObservableCollection<Inspection>(_service.GetAllInspections());
            _allInspections = new ObservableCollection<Inspection>(_service.GetAllInspections());
            ParkinglotList = new ObservableCollection<Parkinglot>(_service.GetAllParkinglots());
            InspectionStateList = new ObservableCollection<State>(_service.GetAllStates());
            Assignmentlist = new ObservableCollection<Asignment>(_service.GetallAsignments());
            FormList = new ObservableCollection<Form>(_service.GetAllForms());
            EmployeesList = new ObservableCollection<Employee>(_service.GetAllInspecteurs());
        }

        private void AddInspecteur()
        {
          
            if (SelectedEmployee == null) return;
            if (InspectionInspectors.Contains(SelectedEmployee)) return;

            SelectedInspection.Employees.Add(SelectedEmployee);
            InspectionInspectors.Add(SelectedEmployee);

            SelectedEmployee = null;
            UpdateProperties();
        }

        private void RemoveInspecteur()
        {
            if (SelectedInspecteur == null) return;

            SelectedInspection.Employees.Remove(SelectedInspecteur);
            InspectionInspectors.Remove(SelectedInspecteur);

            SelectedInspecteur = null;
            UpdateProperties();
        }

        private bool CanEditInspection()
        {
            return _selectedInspection.id != 0;
        }

        private bool CanCreateInspection()
        {
            return _selectedInspection.id == 0;
        }

       

        public void ResetInspection()
        {
            SetNewInspection();
            CommandError = "";
        }

        public void CreateInspection()
        {
            if (!CreateInspectionValidation()) return;
            if (_selectedInspection.date == null) { _selectedInspection.date = DateTime.Today; }


            _service.CreateNewAssignemnt(_selectedInspection);
            SetNewInspection();
            CommandError = "Created";
            UpdateProperties();
        }

        private bool CreateInspectionValidation()
        {
            CommandError = "";
            if (_selectedInspection == null)
            {
                CommandError =
                    "_selectedAsignment is null, please contact your ICT department, something went horrably wrong";
                return false;
            }

            if (_selectedInspection.Asignment == null)
            {
                CommandError = "Geen opdracht geselecteerd";
                return false;
            }

            if (_selectedInspection.Parkinglot == null) CommandError = "Geen Parkeerplaats geselecteerd";
            if (_selectedInspection.State1 == null) CommandError = "Geen status geselecteerd";
            if (_selectedInspection.deadline < DateTime.Today) CommandError = "Deadline te vroeg.";
            if (_selectedInspection.deadline > _selectedInspection.Asignment.deadline)
                CommandError = "Inspectie deadline valt buiten de opdracht.";

            return CommandError.Equals("");
        }


        public void EditInspection()
        {
            if (EditInspectionValidation())
            {
                _service.UpdateInspection(_selectedInspection);
                SetNewInspection();

                CommandError = "Updated";
                UpdateProperties();
            }
        }


        private bool EditInspectionValidation()
        {

            CommandError = "";
            if (_selectedInspection == null)
            {
                CommandError =
              "_selectedAsignment is null, please contact your ICT department, something went horrably wrong";
                return false;
            }

            if (_selectedInspection.Asignment == null) CommandError = "Geen opdracht geselecteerd";
            if (_selectedInspection.Parkinglot == null) CommandError = "Geen Parkeerplaats geselecteerd";
            if (_selectedInspection.State1 == null) CommandError = "Geen status geselecteerd";
            if (_selectedInspection.date >= _selectedInspection.deadline) CommandError = "Deadline is al geweest.";


            return CommandError.Equals("");
        }

        private void SetNewInspection()
        {
            SelectedInspection = new Inspection
            {
                deadline = DateTime.Now,
                date = DateTime.Today
            };

            SelectedEmployee = null;
            SelectedInspecteur = null;

            base.RaisePropertyChanged();

        }
    }
}
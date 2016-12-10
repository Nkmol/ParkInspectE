using System;
using System.Collections.ObjectModel;
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
    public class InspectionViewModel : ViewModelBase
    {
        // collections
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

        public ObservableCollection<State> InspectionStateList { get; set; }
        public ObservableCollection<Parkinglot> ParkinglotList { get; set; }
        public ObservableCollection<Asignment> Assignmentlist { get; set; }


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
        /// <summary>
        /// Initializes a new instance of the InspectieViewModel class.
        /// </summary>
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

            RemoveInspecteurCommand = new RelayCommand(RemoveInspecteur);
            AddInspecteurCommand = new RelayCommand(AddInspecteur);

        }

        private void AddInspecteur()
        {
            // popup window to select and inspecteur
            var tempInspecteur = new Employee
            {
                firstname = "john",
                lastname = "Cena"
            };
            // validation
            SelectedInspection.Employees.Add(tempInspecteur);
            UpdateProperties();


        }

        private void RemoveInspecteur()
        {
            // popup window to select and inspecteur
            
            // validation
            //action
            SelectedInspection.Employees.Remove(SelectedInspecteur);
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

        private void UpdateProperties()
        {
            InspectieCollection = new ObservableCollection<Inspection>(_service.GetAllInspections());
            ParkinglotList = new ObservableCollection<Parkinglot>(_service.GetAllParkinglots());
            InspectionStateList = new ObservableCollection<State>(_service.GetAllStates());
            Assignmentlist = new ObservableCollection<Asignment>(_service.GetallAsignments());
        }

        public void ResetInspection()
        {
            SetNewInspection();
            CommandError = "";
        }

        public void CreateInspection()
        {
            if (CreateInspectionValidation())
            {
                if (_selectedInspection.date == null) { _selectedInspection.date = DateTime.Today; }


                _service.CreateNewAssignemnt(_selectedInspection);
                SetNewInspection();
                CommandError = "Created";
                UpdateProperties();

            }
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

            if (_selectedInspection.Asignment == null) CommandError = "Geen opdracht geselecteerd";
            if (_selectedInspection.Parkinglot == null) CommandError = "Geen Parkeerplaats geselecteerd";
            if (_selectedInspection.State1 == null) CommandError = "Geen status geselecteerd";


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

        // same code as createInspection validtion
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


            return CommandError.Equals("");
        }

        private void SetNewInspection()
        {
            _selectedInspection = new Inspection {deadline = DateTime.Now};
            base.RaisePropertyChanged();

        }
    }
}
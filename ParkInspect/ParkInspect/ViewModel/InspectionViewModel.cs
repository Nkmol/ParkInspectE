using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel.AssignmentVM;
using ParkInspect.ViewModel.Popup;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class InspectionViewModel : ViewModelBase, ICreateUpdatePopup
    {
        public Inspection Data;


        // TODO Global Data
        public ObservableCollection<Inspection> Inspections { get; set; }
        public ObservableCollection<Parkinglot> Parkinglots { get; set; }
        public ObservableCollection<Form> Forms { get; set; }
        public ObservableCollection<Employee> Inspectors { get; set; }
        public ObservableCollection<State> States { get; set; }

        public Employee SelectedInspector { get; set; }
        public string Message { get; set; }

        // commands
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand AssignInspectorCommand { get; set; }
        public RelayCommand UnassignInspecteurCommand { get; set; }

        public Action PopupDone { get; set; }

        public object SelectedItemPopup => this;

        private readonly InspectionService _service;

        #region ViewModel POCO Properties

        // Is not used in the form
        public int Id
        {
            get { return Data.id; }
            set { Data.id = value; }
        }

        public Asignment Assigment
        {
            get { return Data.Asignment; }
            set
            {
                Data.Asignment = value;
                RaisePropertyChanged();
            }
        }

        public Form Form
        {
            get { return Data.Form; }
            set
            {
                Data.Form = value;
                RaisePropertyChanged();
            }
        }

        public Inspection FollowUpInspection
        {
            get { return Data.Inspection2; }
            set
            {
                Data.Inspection2 = value;
                RaisePropertyChanged();
            }
        }

        public Parkinglot Parkinglot
        {
            get { return Data.Parkinglot; }
            set
            {
                Data.Parkinglot = value;
                RaisePropertyChanged();
            }
        }

        public State State
        {
            get { return Data.State1; }
            set
            {
                Data.State1 = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Employee> AssignedInspectors
        {
            get { return new ObservableCollection<Employee>(Data.Employees); }
            set
            {
                Data.Employees = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? Deadline
        {
            get { return Data.deadline; }
            set
            {
                Data.deadline = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? Date
        {
            get { return Data.date; }
            set
            {
                Data.date = value;
                RaisePropertyChanged();
            }
        }

        public string Clarification
        {
            get { return Data.clarification; }
            set
            {
                Data.clarification = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        [PreferredConstructor]
        public InspectionViewModel(IRepository context) : this(context, null)
        {
        }

        public InspectionViewModel(IRepository context, Inspection data = null)
        {
            Data = data ?? new Inspection();

            // Default values
            Deadline = DateTime.Now.AddDays(1);
            Date = DateTime.Now;

            // set services and Lists
            _service = new InspectionService(context);
            EmptyForm();

            // set commands
            ResetCommand = new RelayCommand(EmptyForm);
            AddCommand = new RelayCommand(Add, CanCreateInspection);
            EditCommand = new RelayCommand(Edit, CanEditInspection);


            UnassignInspecteurCommand = new RelayCommand(UnassignInspecteur, CanRemoveInspecteur);
            AssignInspectorCommand = new RelayCommand(AssignInspector, CanAddInspecteur);

            States = new ObservableCollection<State>(_service.GetAll<State>());
            Inspections = new ObservableCollection<Inspection>(_service.GetAll<Inspection>());
            Parkinglots = new ObservableCollection<Parkinglot>(_service.GetAll<Parkinglot>());
            Forms = new ObservableCollection<Form>(_service.GetAll<Form>());
            Inspectors = new ObservableCollection<Employee>(_service.GetAll<Employee>());
        }

        private bool CanAddInspecteur()
        {
            return SelectedInspector != null;
        }

        private bool CanRemoveInspecteur()
        {
            return SelectedInspector != null; 
        }

        private void AssignInspector()
        {
          
            //if (SelectedEmployee == null) return;
            //if (InspectionInspectors.Contains(SelectedEmployee)) return;

            //SelectedInspection.Employees.Add(SelectedEmployee);
            //InspectionInspectors.Add(SelectedEmployee);

            //SelectedEmployee = null;
            //UpdateProperties();
        }

        private void UnassignInspecteur()
        {
            //if (SelectedInspecteur == null) return;

            //SelectedInspection.Employees.Remove(SelectedInspecteur);
            //InspectionInspectors.Remove(SelectedInspecteur);

            //SelectedInspecteur = null;
            //UpdateProperties();
        }

        private bool CanEditInspection()
        {
            //return _selectedInspection.id != 0;
            return false;
        }

        private bool CanCreateInspection()
        {
            //return _selectedInspection.id == 0;
            return true;
        }      

        public void Add()
        {
            PopupBeforeFinish();
            //if (!CreateInspectionValidation()) return;
            //if (_selectedInspection.date == null) { _selectedInspection.date = DateTime.Today; }

            ////_service.CreateNewAssignemnt(_selectedInspection);
            //CommandError = "Created";
            //UpdateProperties();
            //SetNewInspection();
            //PopupBeforeFinish();
        }

        public void Edit()
        {
            //if (EditInspectionValidation())
            //{
            //    _service.UpdateInspection(_selectedInspection);
            //    SetNewInspection();

            //    CommandError = "Updated";
            //    UpdateProperties();

            //    PopupBeforeFinish();
            //}
        }


        private void EmptyForm()
        {
            base.RaisePropertyChanged();
        }

        private void PopupBeforeFinish()
        {
            PopupDone();
        }
    }
}
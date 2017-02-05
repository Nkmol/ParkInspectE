using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.View.UserControls;
using ParkInspect.View.UserControls.Popup;
using ParkInspect.ViewModel.Popup;
using Microsoft.Practices.ServiceLocation;
using System.Threading.Tasks;
using System.Resources;

namespace ParkInspect.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class InspectionViewModel : ViewModelBase, ICreateUpdatePopup
    {
        private Inspection _data;
        public Inspection Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                if (_data != null)
                {
                    isSaved = true;
                }
            }
        }


        public ObservableCollection<Inspection> Inspections { get; set; }
        public ObservableCollection<Parkinglot> Parkinglots { get; set; }
        public ObservableCollection<Form> Forms { get; set; }
        public ObservableCollection<State> States { get; set; }
        public ObservableCollection<Employee> Inspectors { get; set; }

        public Employee SelectedInspector { get; set; }
        public Employee SelectedAssignedInspector { get; set; }

        public string Message { get; set; }

        // commands
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AssignInspectorCommand { get; set; }
        public RelayCommand UnassignInspecteurCommand { get; set; }
        public RelayCommand SearchFormCommand { get; set; }
        public RelayCommand FillFormCommand { get; set; }

        public Action PopupDone { get; set; }

        public object SelectedItemPopup => this;

        private readonly InspectionService _service;
        private PopupManager _popupManager;

        private DateTime? _boundryStartDate;
        private DateTime? _boundryEndDate;
        
        private string _fillFormText;
        public string FillFormText
        {
           get
            {
                return _fillFormText;
            }
            set
            {
                Set(ref _fillFormText, value);
            }
        }
        
        public DateTime? BoundryStartDate
        {
            get { return _boundryStartDate; }
            set
            {
                if (_boundryStartDate != value)
                {
                    Set(ref _boundryStartDate, value);
                }
            }
        }

        public DateTime? BoundryEndDate
        {
            get { return _boundryEndDate; }
            set
            {
                if (_boundryEndDate != value)
                {
                    Set(ref _boundryEndDate, value);
                }
            }
        }

        #region ViewModel POCO Properties

        // Is not used in the form
        public int AssignmentId
        {
            get { return Data.assignment_id; }
            set { Data.assignment_id = value; }
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

        public ObservableCollection<Employee> AssignedInspectors { get; set; }

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

        private bool isSaved;

        #endregion

        private Template _selectedTemplate;

        public Template SelectedTemplate
        {
            get { return _selectedTemplate; }
            set { _selectedTemplate = value; }
        }

        #region ViewModel Form
        private Template _selectedTemplateForm;

        public Template SelectedTemplateForm
        {
            get { return _selectedTemplateForm; }
            set { _selectedTemplateForm = value; }
        }

        #endregion

        public bool SelectTemplateIsEnabled => Data.id <= 0 || !(State.state1 == "voltooid" && Form == null);

        [PreferredConstructor]
        public InspectionViewModel(IRepository context) : this(context, null)
        {
        }

        public InspectionViewModel(IRepository context, PopupManager popupManager, Inspection data = null)
        {
            _data = data ?? new Inspection();

            _service = new InspectionService(context);
            _popupManager = popupManager;

            // set commands
            ResetCommand = new RelayCommand(EmptyForm);
            SaveCommand = new RelayCommand(Save);

            AssignedInspectors = new ObservableCollection<Employee>(Data.Employees);

            UnassignInspecteurCommand = new RelayCommand(UnassignInspecteur, () => SelectedAssignedInspector != null);
            AssignInspectorCommand = new RelayCommand(AssignInspector, () => SelectedInspector != null);
            
            States = new ObservableCollection<State>(_service.GetAll<State>());
            Inspections = new ObservableCollection<Inspection>(_service.GetAll<Inspection>());
            Parkinglots = new ObservableCollection<Parkinglot>(_service.GetAll<Parkinglot>());
            Forms = new ObservableCollection<Form>(_service.GetAll<Form>());
            Inspectors = new ObservableCollection<Employee>(_service.GetAll<Employee>().OrderBy(x => x.firstname));

            SelectedTemplate = Data.Form?.Template;
            SelectedTemplateForm = Data.Form?.Template;
        }

        private void LoadInspector()
        {
            // TODO Automatic injection without Current.GetInstance
            Inspectors = new ObservableCollection<Employee>(ServiceLocator.Current.GetInstance<GlobalViewModel>().Employees
                .Where(x => x.role == "Inspector")
                .OrderBy(x => x.firstname)
                );
        }

        private void AssignInspector()
        {
            AssignedInspectors.Add(SelectedInspector);
            Inspectors.Remove(SelectedInspector);
        }

        private void UnassignInspecteur()
        {
            Inspectors.Add(SelectedAssignedInspector);
            AssignedInspectors.Remove(SelectedAssignedInspector);

            LoadInspector();
            RaisePropertyChanged(() => Inspectors);
        }

        private void Save()
        {
            SelectedTemplate = SelectedTemplateForm;
            PopupBeforeFinish();
            isSaved = true;
        }

        private void EmptyForm()
        {
            Parkinglot = null;
            Form = null;
            State = null;
            FollowUpInspection = null;
            Date = null;
            Deadline = null;
            Clarification = null;
            AssignedInspectors = new ObservableCollection<Employee>();
            SelectedInspector = null;

            base.RaisePropertyChanged();
        }

        private void PopupBeforeFinish()
        {
            PopupDone();
        }
    }
}

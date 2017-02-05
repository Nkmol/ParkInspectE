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
                updateFormButtonText();
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
                _fillFormText = value;
                RaisePropertyChanged("FillFormText");
            }
        }
        
        public DateTime? BoundryStartDate
        {
            get { return _boundryStartDate; }
            set
            {
                if (_boundryStartDate != value)
                {
                    _boundryStartDate = value;
                    RaisePropertyChanged();
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
                    _boundryEndDate = value;
                    RaisePropertyChanged();
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

        [PreferredConstructor]
        public InspectionViewModel(IRepository context) : this(context, null)
        {
        }

        public InspectionViewModel(IRepository context, PopupManager popupManager, Inspection data = null)
        {
            isSaved = true;
            if (data == null)
            {
                isSaved = false;
            }
            _data = data ?? new Inspection();

            _service = new InspectionService(context);
            _popupManager = popupManager;

            // set commands
            ResetCommand = new RelayCommand(EmptyForm);
            SaveCommand = new RelayCommand(Save);
            FillFormCommand = new RelayCommand(FillForm);

            AssignedInspectors = new ObservableCollection<Employee>(Data.Employees);

            UnassignInspecteurCommand = new RelayCommand(UnassignInspecteur, () => SelectedAssignedInspector != null);
            AssignInspectorCommand = new RelayCommand(AssignInspector, () => SelectedInspector != null);
            SearchFormCommand = new RelayCommand(SearchCommand);
            
            States = new ObservableCollection<State>(_service.GetAll<State>());
            Inspections = new ObservableCollection<Inspection>(_service.GetAll<Inspection>());
            Parkinglots = new ObservableCollection<Parkinglot>(_service.GetAll<Parkinglot>());
            Forms = new ObservableCollection<Form>(_service.GetAll<Form>());
            Inspectors = new ObservableCollection<Employee>(_service.GetAll<Employee>().OrderBy(x => x.firstname));
            updateFormButtonText();
        }

        public void updateFormButtonText()
        {
            if (Data.Form == null)
            {
                FillFormText = "Vragenlijst aanmaken";
            } else
            {
                if (Data.Form.Formfields != null && Data.Form.Formfields.ElementAtOrDefault(0) != null && Data.Form.Formfields.ElementAtOrDefault(0).value != null)
                {
                    FillFormText = "Vragenlijst inzien";
                } else
                {
                    FillFormText = "Vragenlijst invullen";
                }
            }
        }

        private bool templatePopupCompleted
        {
            set
            {
                if (value == true) {
                    //SimpleIoc.Default.GetInstance<DashboardViewModel>().SelectedTab = 3;
                    //_popupManager.ShowPopupNoButton<FormViewModel>("Vragenlijst invullen", new FormPopup(), null);
                    ServiceLocator.Current.GetInstance<FormViewModel>().CreateForm(Data);
                    ServiceLocator.Current.GetInstance<FormViewModel>().SaveForm(true);
                    updateFormButtonText();
                }
            }
        }

        public async void FillForm()
        {   
            if (Data == null || !isSaved)
            {
                ServiceLocator.Current.GetInstance<DialogManager>().ShowMessage("Vragenlijst", "Gelieve eerst de inspectie op te slaan voor aanmaken vragenlijst.");
                return;
            }
            if (_popupManager == null)
            {
                _popupManager = SimpleIoc.Default.GetInstance<PopupManager>();
            }
            if (this.Data.Form == null)
            {
                await _popupManager.ShowPopup<FormViewModel>("Template selecteren", new SelectTemplatePopup(), x => templatePopupCompleted = (x.TemplatesViewModel.SelectedVersion != null) );
            } else
            {
                _popupManager.ShowPopupNoButton<FormViewModel>("Vragenlijst inzien", new FormPopup(), null);
                ServiceLocator.Current.GetInstance<FormViewModel>().LoadForm(Data);
            }
        }

        public void createForm()
        {
            FormViewModel formViewModel = ServiceLocator.Current.GetInstance<FormViewModel>();
            formViewModel.CreateForm(Data);
            //LoadInspector();
        }

        // TODO BOB
        private void SearchCommand()
        {
            //_popupManager.ShowPopup<FormViewModel>("Template", new SelectTemplatePopup(),null);
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
            PopupBeforeFinish();
            isSaved = true;
            updateFormButtonText();
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

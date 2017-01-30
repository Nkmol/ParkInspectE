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
        public Inspection Data;

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

        public Action PopupDone { get; set; }

        public object SelectedItemPopup => this;

        private readonly InspectionService _service;
        private readonly PopupManager _popupManager;

        private DateTime? _boundryStartDate;
        private DateTime? _boundryEndDate;

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

        #endregion

        [PreferredConstructor]
        public InspectionViewModel(IRepository context) : this(context, null)
        {
        }

        public InspectionViewModel(IRepository context, PopupManager popupManager, Inspection data = null)
        {
            Data = data ?? new Inspection();

            _service = new InspectionService(context);
            _popupManager = popupManager;

            // set commands
            ResetCommand = new RelayCommand(EmptyForm);
            SaveCommand = new RelayCommand(Save);

            AssignedInspectors = new ObservableCollection<Employee>(Data.Employees);

            UnassignInspecteurCommand = new RelayCommand(UnassignInspecteur, () => SelectedAssignedInspector != null);
            AssignInspectorCommand = new RelayCommand(AssignInspector, () => SelectedInspector != null);
            SearchFormCommand = new RelayCommand(SearchCommand);

            LoadInspector();
        }

        // TODO BOB
        private void SearchCommand()
        {
            //_popupManager.ShowPopup<FormViewModel>("Template", new SelectTemplatePopup(), x => Form = x.);
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

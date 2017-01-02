using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.View.UserControls.Inspection;
using ParkInspect.ViewModel.Popup;

namespace ParkInspect.ViewModel.AssignmentVM
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class AssignmentViewModel : ViewModelBase
    {
        private readonly AssignmentService _service;
        private readonly PopupManager _popupManager;
        private readonly Asignment _assignment;
        private DialogManager _dialogManager;

        public Inspection SelectedInspection { get; set; }

        // TODO global data
        public ObservableCollection<Form> Forms { get; set; }
        public ObservableCollection<State> States { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Asignment> Assignments { get; set; }

        public RelayCommand<AssignmentOverviewViewModel> SaveCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }

        #region ViewModel Poco properties
        public Client Client
        {
            get { return _assignment.Client; }
            set
            {
                _assignment.Client = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? Date
        {
            get { return _assignment.date; }
            set
            {
                _assignment.date = value;
                RaisePropertyChanged();
            }
        }
        public State State
        {
            get { return _assignment.State1; }
            set
            {
                _assignment.State1 = value;
                RaisePropertyChanged();
            }
        }

        public string Clarification
        {
            get { return _assignment.clarification; }
            set
            {
                _assignment.clarification = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Deadline
        {
            get { return _assignment.deadline; }
            set
            {
                _assignment.deadline = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Inspection> Inspections
        {
            get { return new ObservableCollection <Inspection>(_assignment.Inspections); }
            set
            {
                _assignment.Inspections = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Property Form
        public DateTime? FormDate { get; set; }
        public State FormState { get; set; }
        public string FormClarification { get; set; }
        public System.DateTime FormDeadline { get; set; }
        public virtual Client FormClient { get; set; }
        public virtual ObservableCollection<Inspection> FormInspections { get; set; }
        #endregion

        public string Message { get; set; }

        public AssignmentViewModel(IRepository repository, Asignment assignment, PopupManager popupManager, DialogManager dialogManager)
        {
            _dialogManager = dialogManager;
            _assignment = assignment;
            _popupManager = popupManager;
            _service = new AssignmentService(repository);

            // TODO global data
            Forms = new ObservableCollection<Form>(_service.GetAll<Form>());
            States = new ObservableCollection<State>(_service.GetAll<State>());
            Clients = new ObservableCollection<Client>(_service.GetAll<Client>());

            SaveCommand = new RelayCommand<AssignmentOverviewViewModel>(Add, (_) => _assignment.id <= 0);
            EditCommand = new RelayCommand(Edit, () => _assignment.id > 0);
            AddInspectionCommand = new RelayCommand(ShowPopup);

            FillForm();
        }

        // TODO: Improve the way to make a 'property shadow object'. There is need for double property decleration at the moment.
        private void FillForm()
        {
            FormDate = Date;
            FormState = State;
            FormClarification = Clarification;
            FormDeadline = Deadline;
            FormClient = Client;
            FormInspections = Inspections;
        }

        private void SaveForm()
        {
            Date = FormDate;
            State = FormState;
            Clarification = FormClarification;
            Deadline = FormDeadline;
            Client = FormClient;
            Inspections = FormInspections;
        }

        private void ShowPopup()
        {
            _popupManager.ShowUpdateNewPopup<Inspection>("Voeg een inspectie toe aan de huidige Opdracht", new InspectionManageControl(),
                x =>
                {
                    x.assignment_id = _assignment.id;
                    FormInspections.Add(x);
                    RaisePropertyChanged();
                });
        }

        public void Add(AssignmentOverviewViewModel overview)
        {
            if (FormDate == null) FormDate = DateTime.Today;

            SaveForm();
            Message = _service.InsertOrUpdate(_assignment) ? "De opdracht is toegevoegd!" : "Er is iets misgegaan tijdens het toevoegen.";
            _dialogManager.ShowMessage("Opdracht toevoegen", Message);

            overview.Assignments.Add(this);
        }

        public void Edit()
        {
           SaveForm();
            Message = _service.InsertOrUpdate(_assignment) ? "De opdracht is aangepast!" : "Er is iets misgegaan tijdens het aanpassen.";

            _dialogManager.ShowMessage("Opdracht bewerken", Message);
        }
    }
}
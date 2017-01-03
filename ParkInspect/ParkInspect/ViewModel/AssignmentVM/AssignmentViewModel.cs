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
        private DialogManager _dialogManager;

        public readonly Asignment Data;
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
            get { return Data.Client; }
            set
            {
                Data.Client = value;
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
        public State State
        {
            get { return Data.State1; }
            set
            {
                Data.State1 = value;
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

        public DateTime Deadline
        {
            get { return Data.deadline; }
            set
            {
                Data.deadline = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<InspectionViewModel> Inspections { get; set; }
        #endregion

        #region Property Form
        public DateTime? FormDate { get; set; }
        public State FormState { get; set; }
        public string FormClarification { get; set; }
        public System.DateTime FormDeadline { get; set; }
        public virtual Client FormClient { get; set; }
        public virtual ObservableCollection<InspectionViewModel> FormInspections { get; set; }
        #endregion

        public string Message { get; set; }

        public AssignmentViewModel(IRepository repository, Asignment data, PopupManager popupManager, DialogManager dialogManager)
        {
            _dialogManager = dialogManager;
            Data = data;
            _popupManager = popupManager;
            _service = new AssignmentService(repository);

            // Default values
            Date = DateTime.Now;
            Deadline = DateTime.Now.AddDays(1);

            Inspections = new ObservableCollection<InspectionViewModel>(Data.Inspections.Select(x => new InspectionViewModel(repository, x)));

            // TODO global data
            Forms = new ObservableCollection<Form>(_service.GetAll<Form>());
            States = new ObservableCollection<State>(_service.GetAll<State>());
            Clients = new ObservableCollection<Client>(_service.GetAll<Client>());

            SaveCommand = new RelayCommand<AssignmentOverviewViewModel>(Add, (_) => Data.id <= 0);
            EditCommand = new RelayCommand(Edit, () => Data.id > 0);
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
            _popupManager.ShowUpdateNewPopup<InspectionViewModel>("Voeg een inspectie toe aan de huidige Opdracht", new InspectionManageControl(),
                x =>
                {
                    x.Id = Data.id;
                    FormInspections.Add(x);
                    RaisePropertyChanged();
                });
        }

        public void Add(AssignmentOverviewViewModel overview)
        {
            if (FormDate == null) FormDate = DateTime.Today;

            SaveForm();
            Message = _service.InsertOrUpdate(this) ? "De opdracht is toegevoegd!" : "Er is iets misgegaan tijdens het toevoegen.";
            _dialogManager.ShowMessage("Opdracht toevoegen", Message);

            overview.Assignments.Add(this);
        }

        public void Edit()
        {
           SaveForm();
            Message = _service.InsertOrUpdate(this) ? "De opdracht is aangepast!" : "Er is iets misgegaan tijdens het aanpassen.";

            _dialogManager.ShowMessage("Opdracht bewerken", Message);
        }
    }
}
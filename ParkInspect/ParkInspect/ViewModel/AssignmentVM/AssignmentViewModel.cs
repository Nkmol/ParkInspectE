using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.SqlServer.ReportingServices2005;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.View.UserControls;
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
        public InspectionViewModel SelectedInspection { get; set; }

        public RelayCommand<AssignmentOverviewViewModel> SaveCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }
        public RelayCommand EditInspectionCommand { get; set; }
        public RelayCommand RemoveInspectionCommand { get; set; }
        public RelayCommand RestoreCommand { get; set; }
        public RelayCommand PrepareCommand { get; set; }
        public RelayCommand ReportCommand { get; set; }


        #region ViewModel Poco properties

        public Client Client
        {
            get { return Data.Client; }
            set { Data.Client = value; }
        }

        public DateTime? Date
        {
            get { return Data.date; }
            set { Data.date = value; }
        }

        public State State
        {
            get { return Data.State1; }
            set { Data.State1 = value; }
        }

        public string Clarification
        {
            get { return Data.clarification; }
            set { Data.clarification = value; }
        }

        public DateTime Deadline
        {
            get { return Data.deadline; }
            set { Data.deadline = value; }
        }

        public ObservableCollection<InspectionViewModel> Inspections { get; set; }
        #endregion

        #region Property Form

        private DateTime? _formDate;

        public DateTime? FormDate
        {
            get { return _formDate; }
            set { Set(ref _formDate, value); }
        }

        private State _formState;

        public State FormState
        {
            get { return _formState; }
            set { Set(ref _formState, value); }
        }

        private string _formClarification;

        public string FormClarification
        {
            get { return _formClarification; }
            set { Set(ref _formClarification, value); }
        }

        private DateTime _formDeadline;

        public System.DateTime FormDeadline
        {
            get { return _formDeadline; }
            set { Set(ref _formDeadline, value); }
        }

        private Client _formClient;

        public virtual Client FormClient
        {
            get { return _formClient; }
            set { Set(ref _formClient, value); }
        }

        private ObservableCollection<InspectionViewModel> _formInspections;

        public virtual ObservableCollection<InspectionViewModel> FormInspections
        {
            get { return _formInspections; }
            set { Set(ref _formInspections, value); }
        }

        #endregion

        public string Message { get; set; }

        // Keeps track of inspections that are unassigned, which will be removed on confirmation
        public ObservableCollection<InspectionViewModel> UnassignedInspections;

        public AssignmentViewModel(IRepository repository, Asignment data, PopupManager popupManager, DialogManager dialogManager)
        {
            _dialogManager = dialogManager;
            Data = data;
            _popupManager = popupManager;
            _service = new AssignmentService(repository);

            // Default values
            Date = Date ?? DateTime.Now;
            if (Deadline == DateTime.MinValue)
                Deadline = DateTime.Now.AddDays(1);

            Inspections = new ObservableCollection<InspectionViewModel>(Data.Inspections.Select(x => new InspectionViewModel(repository, popupManager, x)));
            UnassignedInspections = new ObservableCollection<InspectionViewModel>();

            SaveCommand = new RelayCommand<AssignmentOverviewViewModel>(Save);
            EditCommand = new RelayCommand(Edit, () => Data.id > 0);
            AddInspectionCommand = new RelayCommand(ShowAddPopup);
            EditInspectionCommand = new RelayCommand(ShowEditPopup, () => SelectedInspection != null);
            RemoveInspectionCommand = new RelayCommand(UnassignInspection, () => SelectedInspection != null);
            RestoreCommand = new RelayCommand(Reset);
            PrepareCommand = new RelayCommand(ShowPreparePopup, () => SelectedInspection != null);
            ReportCommand = new RelayCommand(GenerateReport, () => SelectedInspection != null);

            FillForm();
        }
        private void ShowPreparePopup()
        {
            var Inspection = Data.Inspections.FirstOrDefault(inspec => inspec.id == SelectedInspection.Data.id); 
            if(Inspection == null)
            {
                _dialogManager.ShowMessage("Fout", "Sla de opdracht eerst op voordat je je inspectie gaat voorbereiden!");
            } else
                _popupManager.ShowPopupNoButton<PrepareViewModel>("Voorbereiden", new PrepareControl(SelectedInspection.Data), null);
        }

        // TODO: Improve the way to make a 'property shadow object'. There is need for double property decleration at the moment.
        private void FillForm()
        {
            FormDate = Date;
            FormState = State;
            FormClarification = Clarification;
            FormDeadline = Deadline;
            FormClient = Client;
            FormInspections = new ObservableCollection<InspectionViewModel>(Inspections); // Unlink
        }

        private void SaveForm()
        {
            Date = FormDate;
            State = FormState;
            Clarification = FormClarification;
            Deadline = FormDeadline;
            Client = FormClient;
            Inspections = new ObservableCollection<InspectionViewModel>(FormInspections);
        }

        public void Reset()
        {
            FillForm();
            UnassignedInspections.Clear();
           // RaisePropertyChanged(() => Inspections);
        }

        private void ShowAddPopup()
        {
            _popupManager.ShowUpdateNewPopup<InspectionViewModel>("Voeg een inspectie toe aan de huidige Opdracht", new InspectionManageControl(),
                x =>
                {
                    x.AssignmentId = Data.id;
                    FormInspections.Add(x);
                    RaisePropertyChanged();
                },
                x => 
                {
                    x.BoundryStartDate = FormDate;
                    x.BoundryEndDate = FormDeadline;
                });
        }

        private void ShowEditPopup()
        {
            _popupManager.ShowUpdateNewPopup("Bewerk onderstaande inspectie", new InspectionManageControl(),
                x =>
                {
                    RaisePropertyChanged();
                },
                x =>
                {
                    x.BoundryStartDate = FormDate;
                    x.BoundryEndDate = FormDeadline;
                },
                SelectedInspection);
        }

        private void UnassignInspection()
        {
            UnassignedInspections.Add(SelectedInspection);
            FormInspections.Remove(SelectedInspection);
        }

        private void Save(AssignmentOverviewViewModel overView)
        {
            if(Data.id <= 0) 
                Add(overView);
            else
                Edit();
        }

        public void Add(AssignmentOverviewViewModel overview)
        {
            if (FormDate == null) FormDate = DateTime.Today;

            SaveForm();
            Message = _service.Add(this) ? "De opdracht is toegevoegd!" : "Er is iets misgegaan tijdens het toevoegen.";
            _dialogManager.ShowMessage("Opdracht toevoegen", Message);

            overview.Assignments.Add(this);
        }

        public void Edit()
        {
           SaveForm();
            Message = _service.Update(this) ? "De opdracht is aangepast!" : "Er is iets misgegaan tijdens het aanpassen.";

            _dialogManager.ShowMessage("Opdracht bewerken", Message);
        }

        private void GenerateReport()
        {
            
            ReportView view = new ReportView();
            view.LoadInspectionReport("reports/InspectieRapport.rdl", SelectedInspection.Data.id);
            view.Show();

        }
    }
}
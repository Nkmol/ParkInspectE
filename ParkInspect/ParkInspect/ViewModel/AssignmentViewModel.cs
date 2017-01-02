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
using ParkInspect.View.UserControls;
using ParkInspect.View.UserControls.Inspection;
using ParkInspect.ViewModel.Popup;

namespace ParkInspect.ViewModel
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
        private readonly PopupManager _popup;


        private ObservableCollection<Asignment> Data { get; set; }

        public Asignment SelectedAssignment
        {
            get { return _selectedAssignment; }
            set { _selectedAssignment = value; RaisePropertyChanged("SelectedAssignment"); }
        }

        public Inspection SelectedInspection { get; set; }

        public ObservableCollection<Form> Forms { get; set; }
        public ObservableCollection<State> States { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Asignment> Assignments { get; set; }

        public RelayCommand CreateAsignmentCommand { get; set; }
        public RelayCommand EditAsignmentCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand NewInspectionCommand { get; set; }

        public string CommandError { get; set; }

        public AssignmentViewModel(IRepository repository, PopupManager popup)
        {
            _popup = popup;
            _service = new AssignmentService(repository);

            Data = new ObservableCollection<Asignment>(_service.GetAll<Asignment>());
            Assignments = Data;
            Forms = new ObservableCollection<Form>(_service.GetAll<Form>());
            States = new ObservableCollection<State>(_service.GetAll<State>());
            Clients = new ObservableCollection<Client>(_service.GetAll<Client>());

            CreateAsignmentCommand = new RelayCommand(CreateAsignment, CanCreateAsignment);
            EditAsignmentCommand = new RelayCommand(EditAsignment, CanEditAsignment);
            ResetCommand = new RelayCommand(ResetAsignement);

            NewInspectionCommand = new RelayCommand(ShowPopup);

            NewAssignment();
        }

        private void ShowPopup()
        {
            _popup.ShowUpdateNewPopup<Inspection>("Voeg een inspectie toe aan de huidige Opdracht", new InspectionManageControl(),
                x =>
                {
                    x.assignment_id = SelectedAssignment.id;
                    SelectedAssignment.Inspections.Add(x);
                    RaisePropertyChanged("SelectedAssignment");
                });
        }

        //private void MakeNewInspection(AssignmentViewModel t)
        //{
        //    if ((_newInspection?.Parkinglot == null) || (_newInspection.State1 == null) ||
        //        (_newInspection.deadline < DateTime.Today) || (_newInspection.deadline > _newInspection.Asignment.deadline))
        //        return;
        //    if (_newInspection.date == null) _newInspection.date = DateTime.Today;
        //    _service.Add(_newInspection);
        //    AssignedInspections = new ObservableCollection<Inspection>(_service.GetAll<Inspection>());
        //    _newInspection = new Inspection();
        //}

        //private void UpdateProperties()
        //{
        //    OpdrachtenCollection = new ObservableCollection<Asignment>(_service.GetAll<Asignment>());
        //    SearchedAsignments = new ObservableCollection<Asignment>(OpdrachtenCollection);
        //    ClientList = _service.GetAll<Client>();
        //    AssignmentStatusList = _service.GetAll<State>();
        //    AssignedInspections = new ObservableCollection<Inspection>(_service.GetAll<Inspection>());

        //    _clientfilter = "";
        //}

        public void CreateAsignment()
        {
            if (!CreateValidation()) return;

            if (SelectedAssignment.date == null) SelectedAssignment.date = DateTime.Today;

            _service.CreateNewAssignemnt(SelectedAssignment);
            NewAssignment();

            // UpdateProperties();
        }

        private bool CanCreateAsignment()
        {
            return SelectedAssignment.id == 0;
        }

        private bool CreateValidation()
        {
            CommandError = "";

            // IT messages.
            if (SelectedAssignment == null)
            {
                CommandError = "No Selected Asignment, please contact your IT department.";
                return false;
            }
            if (SelectedAssignment.state == null) CommandError = "State is null, please contact your IT department.";

            // user messages.
            if (SelectedAssignment.Client == null) CommandError = "Geen klant geselecteerd.";
            if (SelectedAssignment.State1 == null) CommandError = "Geen status geselecteerd.";
            if (SelectedAssignment.deadline == DateTime.MinValue) CommandError = "De gestelde deadline is niet geldig.";
            if (SelectedAssignment.deadline < DateTime.Today) CommandError = "De deadline is al geweest";

            foreach (var temp in SelectedAssignment.Inspections)
            {
                if (!(temp.date < SelectedAssignment.date) && !(temp.deadline > SelectedAssignment.deadline)) continue;

                CommandError = "Een van de inspecties valt buiten de opdracht.";
                break;
            }

            return CommandError.Equals("");
        }

        public void EditAsignment()
        {
            if (!EditValidation()) return;
            try
            {
                _service.UpdateAssignment(SelectedAssignment);
                NewAssignment();

                CommandError = "Updated";
                // UpdateProperties();
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(InvalidOperationException))
                {
                    _service.DeleteAssignment(SelectedAssignment);
                    _service.CreateNewAssignemnt(SelectedAssignment);
                    NewAssignment();

                    CommandError = "Updated";
                    // UpdateProperties();
                }
                else
                {
                    CommandError = "Exeption thrown contact IT.";
                }
            }
        }

        private bool EditValidation()
        {
            CommandError = "";

            // IT messages.
            if (SelectedAssignment == null)
            {
                CommandError =
                    "_selectedAsignment is null, please contact your ICT department, something went horrably wrong";
                return false;
            }
            if (SelectedAssignment.client_id == 0) CommandError = "Clientid is null, please contact you IT department.";


            // user messages.
            if (SelectedAssignment.Client == null) CommandError = "No Client Selected.";
            if (SelectedAssignment.State1 == null) CommandError = "No Status selected.";
            if (SelectedAssignment.deadline == DateTime.MinValue)
                CommandError = "Unless your client is jesus christ, your deadline is not set properly.";
            if (SelectedAssignment.deadline < SelectedAssignment.date) CommandError = "De deadline is al geweest";
            //if (OpdrachtenCollection.FirstOrDefault(a => a.id == SelectedAssignment.id) == null)
            //    CommandError = "Selected assignment could not be found. Maybe someone removed it.";

            foreach (var temp in SelectedAssignment.Inspections)
                if ((temp.date < SelectedAssignment.date) || (temp.deadline > SelectedAssignment.deadline))
                {
                    CommandError = "Een van de inspecties valt buiten de opdracht.";
                    break;
                }
            return CommandError.Equals("");
        }

        private bool CanEditAsignment()
        {
            if (SelectedAssignment == null) return false;
            return SelectedAssignment.id != 0;
        }

        //public void RemoveInspection()
        //{
        //    if (SelectedInspection == null) return;

        //    SelectedAssignment.Inspections.Remove(SelectedInspection);
        //    AssignmentInspections.Remove(SelectedInspection);
        //}

        // funcion should be changed to creating a new inspection, will be implemented as adding an existing one for now.
        //public void CreateInspection()
        //{
        //    if (SelectedInspectionBox == null) return;
        //    if (AssignmentInspections.Contains(SelectedInspectionBox)) return;

        //    SelectedAssignment.Inspections.Add(SelectedInspectionBox);
        //    AssignmentInspections.Add(SelectedInspectionBox);

        //    SelectedInspectionBox = null;
        //}

        public void ResetAsignement()
        {
            NewAssignment();

            CommandError = "";
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
            var result = Assignments.Where(a => a.Like(filters));
            Assignments = new ObservableCollection<Asignment>(result);
            RaisePropertyChanged();
        }

        private void NewAssignment()
        {
            SelectedAssignment = new Asignment
            {
                deadline = DateTime.Today,
                date = DateTime.Today
            };
            SelectedInspection = null;
            //SelectedInspectionBox = null;

            base.RaisePropertyChanged();
        }

        #region All filters

        private string _clientfilter;

        public string ClientFilter
        {
            get { return _clientfilter; }
            set
            {
                _clientfilter = value;
                AlterVisableAsignments();
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
                AlterVisableAsignments();
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
                AlterVisableAsignments();
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
                AlterVisableAsignments();
                base.RaisePropertyChanged();
            }
        }

        private string _clarificationFilter;
        private Asignment _selectedAssignment;

        public string ClarificationFilter
        {
            get { return _clarificationFilter; }
            set
            {
                _clarificationFilter = value;
                AlterVisableAsignments();
                base.RaisePropertyChanged();
            }
        }

        #endregion
    }
}
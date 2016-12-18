﻿using System;
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

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AssignmentViewModel : ViewModelBase
    {
        private IEnumerable<Asignment> _opdrachtCollection;
        public IEnumerable<Asignment> OpdrachtenCollection
        {
            get { return _opdrachtCollection; }
            set
            {
                _opdrachtCollection = value;

                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<Asignment> _searchedAsignments;
        public ObservableCollection<Asignment> SearchedAsignments
        {
            get { return _searchedAsignments; }
            set
            {
                _searchedAsignments = value;
                base.RaisePropertyChanged();
            }
        }

        //used to update the datagrid in the managements tab.
        private ObservableCollection<Inspection> _inspections;
        public ObservableCollection<Inspection> AssignmentInspections
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

        private IEnumerable<State> _assignmentStateList;
        public IEnumerable<State> AssignmentStatusList
        {
            get { return _assignmentStateList; }
            set
            {
                _assignmentStateList = value;
                base.RaisePropertyChanged();
            }
        }

        private IEnumerable<Client> _clientList;
        public IEnumerable<Client> ClientList
        {
            get { return _clientList; }
            set
            {
                _clientList = value;
                base.RaisePropertyChanged();
            }
        }

        private IEnumerable<Inspection> _inspectionlist;
        public IEnumerable<Inspection> InspectionList
        {
            get { return _inspectionlist; }
            set
            {
                _inspectionlist = value;
                base.RaisePropertyChanged();

            }
        }


        private Asignment _selectedAsignment;
        public Asignment SelectedAsignment
        {
            get { return _selectedAsignment; }

            set
            {
                _selectedAsignment = value;
                AssignmentInspections = new ObservableCollection<Inspection>(value.Inspections);
                base.RaisePropertyChanged();
            }
        }

        private Inspection _selectedInspection;
        public Inspection SelectedInspection
        {
            get
            {
                return _selectedInspection;
            }
            set
            {
                _selectedInspection = value;
                base.RaisePropertyChanged();
            }
        }

        private Inspection _selectedInspectionBox;
        public Inspection SelectedInspectionBox
        {
            get
            {
                return _selectedInspectionBox;
            }
            set
            {
                _selectedInspectionBox = value;
                base.RaisePropertyChanged();
            }
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



        private readonly AssignmentService _service;
        public ICommand CreateAsignmentCommand { get; set; }
        public ICommand EditAsignmentCommand { get; set; }
        public ICommand RemoveInspectionCommand { get; set; }
        public ICommand CreateInspectionCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        private string _commandError;

        public string CommandError
        {
            get { return _commandError; }
            set
            {
                _commandError = value;
                base.RaisePropertyChanged();
            }
        }

        public AssignmentViewModel(IRepository repository)
        {
            _service = new AssignmentService(repository);
            UpdateProperties();
            SetEmptySelectedAsignment();


            CreateAsignmentCommand = new RelayCommand(CreateAsignment, CanCreateAsignment);
            EditAsignmentCommand = new RelayCommand(EditAsignment, CanEditAsignment);

            RemoveInspectionCommand = new RelayCommand(RemoveInspection, CanRemoveInspection);
            CreateInspectionCommand = new RelayCommand(CreateInspection, CanAddInspection);

            ResetCommand = new RelayCommand(ResetAsignement);
        }

        private bool CanAddInspection()
        {
            return SelectedInspectionBox != null;
        }

        private bool CanRemoveInspection()
        {
            return SelectedInspection != null;
        }

        private void UpdateProperties()
        {
            OpdrachtenCollection = new ObservableCollection<Asignment>(_service.GetAllAsignments());
            SearchedAsignments = new ObservableCollection<Asignment>(OpdrachtenCollection);
            ClientList = _service.GetAllClients();
            AssignmentStatusList = _service.GetAllStates();
            InspectionList = _service.GetAllInspections();

            _clientfilter = "";
        }

        public void CreateAsignment()
        {
            if (!CreateValidation()) return;

            if (_selectedAsignment.date == null) _selectedAsignment.date = DateTime.Today;
            
            _service.CreateNewAssignemnt(_selectedAsignment);
            SetEmptySelectedAsignment();

            CommandError = "Created";
            UpdateProperties();
        }

        private bool CanCreateAsignment()
        {
            return _selectedAsignment.id == 0;
        }

        private bool CreateValidation()
        {
            CommandError = "";

            // IT messages.
            if (_selectedAsignment == null)
            {
                CommandError = "No Selected Asignment, please contact your IT department.";
                return false;
            }
            if (_selectedAsignment.state == null) CommandError = "State is null, please contact your IT department.";

            // user messages.
            if (_selectedAsignment.Client == null) CommandError = "Geen klant geselecteerd.";
            if (_selectedAsignment.State1 == null) CommandError = "Geen status geselecteerd.";
            if (_selectedAsignment.deadline == DateTime.MinValue) CommandError = "De gestelde deadline is niet geldig.";
            if (_selectedAsignment.deadline < DateTime.Today) CommandError = "De deadline is al geweest";

            foreach (var temp in _selectedAsignment.Inspections)
            {
                if (!(temp.date < _selectedAsignment.date) && !(temp.deadline > _selectedAsignment.deadline)) continue;

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
                _service.UpdateAssignment(_selectedAsignment);
                SetEmptySelectedAsignment();

                CommandError = "Updated";
                UpdateProperties();
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(InvalidOperationException))
                {
                    _service.DeleteAssignment(_selectedAsignment);
                    _service.CreateNewAssignemnt(_selectedAsignment);
                    SetEmptySelectedAsignment();

                    CommandError = "Updated";
                    UpdateProperties();
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
            if (_selectedAsignment == null)
            {
                CommandError =
                    "_selectedAsignment is null, please contact your ICT department, something went horrably wrong";
                return false;
            }
            if (_selectedAsignment.client_id == 0) CommandError = "Clientid is null, please contact you IT department.";


            // user messages.
            if (_selectedAsignment.Client == null) CommandError = "No Client Selected.";
            if (_selectedAsignment.State1 == null) CommandError = "No Status selected.";
            if (_selectedAsignment.deadline == DateTime.MinValue) CommandError = "Unless your client is jesus christ, your deadline is not set properly.";
            if (_selectedAsignment.deadline < _selectedAsignment.date) CommandError = "De deadline is al geweest";
            if (OpdrachtenCollection.FirstOrDefault(a => a.id == _selectedAsignment.id) == null) CommandError = "Selected assignment could not be found. Maybe someone removed it.";

            foreach (var temp in _selectedAsignment.Inspections)
            {
                if (temp.date < _selectedAsignment.date || temp.deadline > _selectedAsignment.deadline)
                {
                    CommandError = "Een van de inspecties valt buiten de opdracht.";
                    break;
                }

            }
            return CommandError.Equals("");
        }

        private bool CanEditAsignment()
        {
            if (SelectedAsignment == null) return false;
            return _selectedAsignment.id != 0;
        }




       
        public void RemoveInspection()
        {
            if (SelectedInspection == null) return;

            SelectedAsignment.Inspections.Remove(SelectedInspection);
            AssignmentInspections.Remove(SelectedInspection);
        }

        // funcion should be changed to creating a new inspection, will be implemented as adding an existing one for now.
        public void CreateInspection()
        {
            if (SelectedInspectionBox == null) return;
            if (AssignmentInspections.Contains(SelectedInspectionBox)) return;

            SelectedAsignment.Inspections.Add(SelectedInspectionBox);
            AssignmentInspections.Add(SelectedInspectionBox);

            SelectedInspectionBox = null;

        }




        public void ResetAsignement()
        {
            SetEmptySelectedAsignment();

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
            var result = OpdrachtenCollection.Where(a => a.Like(filters));
            SearchedAsignments = new ObservableCollection<Asignment>(result);
            RaisePropertyChanged();
        }

        private void SetEmptySelectedAsignment()
        {
            SelectedAsignment = new Asignment
            {
                deadline = DateTime.Today,
                date = DateTime.Today
            };
            SelectedInspection = null;
            SelectedInspectionBox = null;

            base.RaisePropertyChanged();

        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private readonly EmployeeService _employeeService;
        private List<Role> PossibleRoles { get; set; }

        // list of tabs is a lits of booleans used to show or hide tabs

        #region List of tabs
        private bool _showAbsence;

        public bool ShowAbsence
        {
            get
            {
                return _showAbsence;
            }
            set
            {
                _showAbsence = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _showRaport;
        public bool ShowRaport
        {
            get
            {
                return _showRaport;
            }
            set
            {
                _showRaport = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _showEmployees;
        public bool ShowEmployee
        {
            get
            {
                return _showEmployees;
            }
            set
            {
                _showEmployees = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _showForms;
        public bool ShowForms
        {
            get
            {
                return _showForms;
            }
            set
            {
                _showForms = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _showInspections;
        public bool ShowInspections
        {
            get
            {
                return _showInspections;
            }
            set
            {
                _showInspections = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _showAssignments;
        public bool ShowAssignments
        {
            get
            {
                return _showAssignments;
            }
            set
            {
                _showAssignments = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _showParkinglots;
        public bool ShowParkinglots
        {
            get
            {
                return _showParkinglots;
            }
            set
            {
                _showParkinglots = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _showClients;
        public bool ShowClients
        {
            get
            {
                return _showClients;
            }
            set
            {
                _showClients = value;
                base.RaisePropertyChanged();
            }
        }
        #endregion

        #region Collections of bools for each dashboard tab.




        private ObservableCollection<bool> _absenceTabs;
        public ObservableCollection<bool> AbsenceTabs
        {
            get { return _absenceTabs; }
            set
            {
                _absenceTabs = value;
                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<bool> _clientTabs;
        public ObservableCollection<bool> ClientTabs
        {
            get { return _clientTabs; }
            set
            {
                _clientTabs = value;
                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<bool> _inspectionTabs;
        public ObservableCollection<bool> InspectionTabs
        {
            get { return _inspectionTabs; }
            set
            {
                _inspectionTabs = value;
                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<bool> _employeeTabs;
        public ObservableCollection<bool> EmployeeTabs
        {
            get { return _employeeTabs; }
            set
            {
                _employeeTabs = value;
                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<bool> _formTabs;
        public ObservableCollection<bool> FormTabs
        {
            get { return _formTabs; }
            set
            {
                _formTabs = value;
                base.RaisePropertyChanged();
            }
        }

        private ObservableCollection<bool> _assignmentTabs;
        public ObservableCollection<bool> AssignmentsTabs
        {
            get { return _assignmentTabs; }
            set
            {
                _assignmentTabs = value;
                base.RaisePropertyChanged();
            }
        }
        #endregion


        // Collection containing the visibility status of all tabs in the aplication. currently capped at 30 booleans. can be edited in the constructor.
        private ObservableCollection<bool> _tabStatus;
        public ObservableCollection<bool> TabStatus
        {
            get { return _tabStatus; }
            set
            {
                _tabStatus = value;
                base.RaisePropertyChanged();
            }
        }

        public DataSync.DataSynchroniser synchroniser;
        public RelayCommand syncCommand;

        private int _selectedTab;
        public int SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                _selectedTab = value;
                RaisePropertyChanged("SelectedTab");
            }
        }

        public DashboardViewModel(IRepository repository)
        {

            _employeeService = new EmployeeService(repository);
            /*
                        AbsenceTabs = new ObservableCollection<bool>(Enumerable.Repeat(true, 2));
                        AssignmentsTabs = new ObservableCollection<bool>(Enumerable.Repeat(true, 2));
                        FormTabs = new ObservableCollection<bool>(Enumerable.Repeat(true, 2));
                        EmployeeTabs = new ObservableCollection<bool>(Enumerable.Repeat(true, 2));
                        InspectionTabs = new ObservableCollection<bool>(Enumerable.Repeat(true, 2));
                        ClientTabs = new ObservableCollection<bool>(Enumerable.Repeat(true, 2));
            */
            TabStatus = new ObservableCollection<bool>(Enumerable.Repeat(true, 30));
            ShowDefaultTabs();

            PossibleRoles = _employeeService.GetAllRoles().ToList();
            synchroniser = new DataSync.DataSynchroniser();
            syncCommand = new RelayCommand(synchroniser.synchronise);
        }

        public void ChangeAuthorization(Role role)
        {
            PossibleRoles = _employeeService.GetAllRoles().ToList();

            if (!PossibleRoles.Contains(role)) { ShowDefaultTabs(); return; }

            switch (role.role1)
            {
                case "Employee":
                    ChangeAuthorizationToEmployee();
                    break;
                case "Inspector":
                    ChangeAuthorizationToInspector();
                    break;
                case "Manager":
                    ChangeAuthorizationToManager();
                    break;
                default:
                    ShowAllTabs();
                    break;
            }
        }

        private void ChangeAuthorizationToEmployee()
        {
            ShowAllTabs();

            TabStatus[6] = false;
            TabStatus[7] = false;          
        }

        private void ChangeAuthorizationToManager()
        {
            ShowAllTabs();
        }


        private void ChangeAuthorizationToInspector()
        {
            ShowAllTabs();

            TabStatus[5] = false;
            TabStatus[0] = false;
            TabStatus[6] = false;
            TabStatus[2] = false;
        }


        // Waring: security flaw, will be triggered with a known role object with unknown role status.
        private void ShowAllTabs()
        {
            for (int i = 0; i < TabStatus.Count; i++)
            {
                TabStatus[i] = true;
            }
        }

        private void ShowDefaultTabs()
        {
            ShowAllTabs();

            TabStatus[0] = false;
            TabStatus[2] = false;
            TabStatus[4] = false;
            TabStatus[5] = false;
            TabStatus[6] = false;
            TabStatus[7] = false;
        }


        /* security lockdown. hides all functionality exept login
        private void HideAllTabs()
        {
            ShowAssignments = false;
            ShowForms = false;
            ShowClients = false;
            ShowInspections = false;
            ShowParkinglots = false;
            ShowRaport = false;
            ShowEmployee = false;
            ShowAbsence = false;
        }
        */
    }
}
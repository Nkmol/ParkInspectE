using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using ParkInspect.Model;
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
        private readonly IDataService _dataService;
        private MainWindow dashboardWindow;
        private EmployeeService _employeeService;



        private List<Role> PossibleRoles { get; set; }



        public int Height = 100;
        public int Width = 100;




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



        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        public DashboardViewModel(IRepository repository)
        {


            ShowDefaultTabs();
            _employeeService = new EmployeeService(repository);
            PossibleRoles = _employeeService.GetAllRoles().ToList();

        }


        public void ChangeAuthorization(Role role)
        {
            PossibleRoles = _employeeService.GetAllRoles().ToList();

            if (PossibleRoles.Contains(role))
            {
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
            else
            {
                ShowDefaultTabs();
            }


        }

        private void ChangeAuthorizationToEmployee()
        {
            ShowAssignments = true;
            ShowForms = true;
            ShowClients = true;
            ShowInspections = true;
            ShowParkinglots = true;
            ShowRaport = false;
            ShowEmployee = false;
            ShowAbsence = true;
        }

        private void ChangeAuthorizationToManager()
        {
            ShowAssignments = true;
            ShowForms = true;
            ShowClients = true;
            ShowInspections = true;
            ShowParkinglots = true;
            ShowRaport = true;
            ShowEmployee = true;
            ShowAbsence = true;
        }

        private void ChangeAuthorizationToInspector()
        {
            ShowAssignments = false;
            ShowForms = true;
            ShowClients = false;
            ShowInspections = true;
            ShowParkinglots = true;
            ShowRaport = false;
            ShowEmployee = false;
            ShowAbsence = true;
        }


        // Waring: security flaw, will be triggered with a known role object with unknown role status.
        private void ShowAllTabs()
        {
            ShowAssignments = true;
            ShowForms = true;
            ShowClients = true;
            ShowInspections = true;
            ShowParkinglots = true;
            ShowRaport = true;
            ShowEmployee = true;
            ShowAbsence = true;
        }

        private void ShowDefaultTabs()
        {
            ShowAssignments = false;
            ShowForms = false;
            ShowClients = false;
            ShowInspections = true;
            ShowParkinglots = true;
            ShowRaport = false;
            ShowEmployee = false;
            ShowAbsence = false;
        }
        // security lockdown. hides all functionality exept login
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



    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const int AmountTabs = 8;

        private readonly IDataService _dataService;
        private MainWindow dashboardWindow;
        private EmployeeService _employeeService;
        private List<Role> PossibleRoles { get; set; }

        public int Height = 100;
        public int Width = 100;
        public ObservableCollection<bool> TabsStatus { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        public DashboardViewModel(IRepository repository)
        {
            TabsStatus = new ObservableCollection<bool>(Enumerable.Repeat(true, AmountTabs)); // Init with a Capacity and Default value

            _employeeService = new EmployeeService(repository);
            PossibleRoles = _employeeService.GetAllRoles().ToList();

        }

        private void ResetTabsState()
        {
            for (var i = 0; i < TabsStatus.Count; i++)
                TabsStatus[i] = true;
        }


        public void ChangeAuthorization(Role role)
        {
            PossibleRoles = _employeeService.GetAllRoles().ToList();

            switch (role?.role1)
            {
                case "Employee":
                    ChangeAuthorizationToEmployee();
                    break;
                case "Inspector":
                    ChangeAuthorizationToInspector();
                    break;
                default:
                    ResetTabsState();
                    break;
            }
        }

        private void ChangeAuthorizationToEmployee()
        {
            ResetTabsState();
            TabsStatus[5] = false;
            TabsStatus[6] = false;
        }

        private void ChangeAuthorizationToInspector()
        {
            ResetTabsState();
            TabsStatus[0] = false;
            TabsStatus[2] = false;
            TabsStatus[5] = false;
            TabsStatus[6] = false;
        }

    }
}